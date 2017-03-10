/*
 @author: Seven Lju
 @date: 2016-05-16
 @desc: mysql/mariadb binary log packet reader
 */

module.exports = BinlogPacket;
function BinlogPacket(options) {
  options = options || {};

  this.protocol41 = options.protocol41;
}

BinlogPacket.prototype.parse = function(parser) {
  parser.parseUnsignedNumber(1); // sikp first byte
  this.timestamp   = parser.parseUnsignedNumber(4);
  this.eventType   = parser.parseUnsignedNumber(1);
  this.eventName   = logEventTypeName[this.eventType];
  this.serverId    = parser.parseUnsignedNumber(4);
  this.eventLength = parser.parseUnsignedNumber(4) - 20;
  this.nextLogPos  = parser.parseUnsignedNumber(4);
  this.flags       = parser.parseUnsignedNumber(2);

  var process = logEventTypeParser[this.eventType];
  if (typeof process === 'function') {
    process(this, parser);
  } else {
    this.raw = parser.parseBuffer(parser._packetEnd - parser._offset);
  }
};

BinlogPacket.prototype.write = function(writer) {
};

var eventConfig = {
  binlogVersion: 4,
  binlogServer: null,
  evHeaderLength: [],
  tableMap: {}
};

function parseFormatDescriptionEvent(packet, parser) {
  var eventData = {};

  eventData.binlogVersion   = parser.parseUnsignedNumber(2);
  eventData.serverVersion   = parser.parseString(50);
  eventData.serverVersion   = eventData.serverVersion.replace(/\0/g, '');
  eventData.createTimestamp = parser.parseUnsignedNumber(4);
  eventData.headerLength    = parser.parseUnsignedNumber(1);
  eventData.headerLength = parser.parseBuffer(parser._packetEnd - parser._offset);
  // unknown event for 0
  eventData.headerLength = Buffer.concat([new Buffer([0]), eventData.headerLength]);
  packet.eventData = eventData;

  eventConfig.binlogVersion  = eventData.binlogVersion;
  eventConfig.binlogServer   = eventData.serverVersion;
  eventConfig.evHeaderLength = eventData.headerLength;
}

function parseTableMapEvent(packet, parser) {
  var eventData = {};
  eventData.tableId = parser.parseUnsignedNumber(4);
  if (eventConfig.evHeaderLength[packet.eventType] > 6) {
    eventData.tableId += parser.parseUnsignedNumber(2) << 32;
  }
  eventData.flags   = parser.parseUnsignedNumber(2);
  eventData.schema  = parser.parseLengthCodedString();
  parser.parseUnsignedNumber(1); // skip \0
  eventData.table   = parser.parseLengthCodedString();
  parser.parseUnsignedNumber(1); // skip \0

  var n       = parser.parseUnsignedNumber(1),
      colraw  = parser.parseBuffer(parser._packetEnd - parser._offset),
      coltype = colraw.slice(0, n),
      colmeta = colraw.slice(n + 1, n + colraw[n] + 1), // skip \0
      colnull = colraw.slice(n + colmeta.length + 1),
      t;
  eventData.columns = {
    column_raw      : colraw,
    column_type_def : coltype,
    column_meta_def : colmeta,
    null_bitmap     : colnull,
    count           : coltype.length,
    formated        : _buildcolumns(coltype, colmeta, colnull)
  };

  packet.eventData = eventData;
  eventConfig.tableMap[eventData.tableId] = eventData;

  function _getbit(buf, index) {
    return (buf[~~(index / 8)] >> (index % 8)) & 0x01;
  }

  function _buildcolumns (coltype, colmeta, colnull) {
    var columns = [],
        meta_index = 0,
        one;
    for (var i = 0, n = coltype.length; i < n; i++) {
      one = {};
      one.type = coltype[i];
      one.nullable = !!_getbit(colnull, i);
      switch (one.type) {
      case 246: /*NEWDECIMAL*/
      case 247: /*ENUM*/
      case 248: /*SET*/
      case 254: /*STRING*/
      case 253: /*VAR_STRING*/
      case 16: /*BIT*/
      case 15: /*VARCHAR*/
      case 0: /*DECIMAL*/
        one.meta = [
          colmeta[meta_index],
          colmeta[meta_index+1]
        ];
        meta_index += 2;
        break;
      case 4: /*FLOAT*/
      case 5: /*DOUBLE*/
      case 17: /*TIMESTAMP2*/
      case 18: /*DATETIME2*/
      case 19: /*TIME2*/
      case 245: /*JSON*/
      case 252: /*BLOB*/
      case 255: /*GEOMETRY*/
        one.meta = [
          colmeta[meta_index]
        ];
        meta_index += 1;
        break;
      default:
        one.meta = [];
      }
      columns.push(one);
    }
    return columns;
  }
}

function parseRowsEvent(packet, parser) {
  var eventData = {},
      is_update = false,
      t;
  eventData.tableId = parser.parseUnsignedNumber(4);
  if (eventConfig.evHeaderLength[packet.eventType] > 6) {
    eventData.tableId += parser.parseUnsignedNumber(2) << 32;
  }
  eventData.flags   = parser.parseUnsignedNumber(2);
  if (packet.eventType >= 30 && packet.eventType <= 32) { // version 2
    t = parser.parseUnsignedNumber(2);
    eventData.extra = parser.parseBuffer(t - 2);
  }

  var n = parser.parseUnsignedNumber(1),
      m = ~~((n + 7) / 8);

  var colpresent = parser.parseBuffer(m),
      tmap = eventConfig.tableMap[eventData.tableId];
  if (packet.eventType === 24 /*UPDATE_ROWS_EVENT_V1*/ ||
      packet.eventType === 31 /*UPDATE_ROWS_EVENT*/) {
    is_update = true;
    parser._offset += m;
  }

  var row;
  eventData.rows = [];
  while (parser._packetEnd > parser._offset) {
    row = _fetchrow(packet, parser, {
      map: tmap,
      colpresent: colpresent
    });
    if (is_update) {
      row = [row, _fetchrow(packet, parser, {
        map: tmap,
        colpresent: colpresent
      })];
    }
    eventData.rows.push(row);
  }

  packet.eventData = eventData;

  function _calc1 (buf) {
    if (!buf) return 0;
    var r = 0;
    for (var n = buf.length, i = 0, t = 0; i < n; i++) {
      t = buf[i];
      while (t > 0) {
        if (t % 2) r++;
        t = ~~(t / 2);
      }
    }
    return r;
  }

  function _getval(parser, tmap, coli) {
    var col = tmap.columns.formated[coli];
    switch (col.type) {
    case 246: /*NEWDECIMAL*/
      var _nd_s = col.meta[1],
          _nd_i = col.meta[0] - col.meta[1],
          _nd_c = [0, 1, 1, 2, 2, 3, 3, 4, 4, 4];
      return parser.parseBuffer(
        ~~(_nd_s / 9) * 4 + _nd_c[_nd_s % 9] +
        ~~(_nd_i / 9) * 4 + _nd_c[_nd_i % 9]
      );
    case 247: /*ENUM*/
    case 248: /*SET*/
    case 249: /*TINY_BLOB*/
    case 250: /*MEDIUM_BLOB*/
    case 251: /*LONG_BLOB*/
    case 252: /*BLOB*/
    case 245: /*JSON*/
    case 255: /*GEOMETRY*/
      return parser.parseBuffer(parser.parseUnsignedNumber(col.meta[0]));
    case 254: /*STRING*/
      return parser.parseBuffer(parser.parseUnsignedNumber(1));
    case 253: /*VAR_STRING*/
    case 15: /*VARCHAR*/
      var _s_len = (col.meta[1] << 8 | col.meta[0]);
      return parser.parseBuffer(parser.parseUnsignedNumber(_s_len>255?2:1));
    case 1: /*TINY*/
      return parser.parseUnsignedNumber(1);
    case 2: /*SHORT*/
      return parser.parseUnsignedNumber(2);
    case 9: /*INT24*/
    case 11: /*TIME*/
    case 10: /*DATE*/
      return parser.parseBuffer(3);
    case 3: /*LONG*/
    case 4: /*FLOAT*/
    case 7: /*TIMESTAMP*/
      return parser.parseUnsignedNumber(4);
    case 5: /*DOUBLE*/
    case 8: /*LONGLONG*/
    case 12: /*DATETIME*/
      return parser.parseBuffer(8);
    case 13: /*YEAR*/
      return parser.parseUnsignedNumber(1) + 1900;
    case 16: /*BIT*/
      return parser.parseBuffer(col.meta[0] + ~~((colmeta[1] + 7) / 8));
    case 19: /*TIME2*/
      return parser.parseBuffer(3 + ~~((col.meta[0] + 1) / 2));
    case 17: /*TIMESTAMP2*/
      return parser.parseBuffer(4 + ~~((col.meta[0] + 1) / 2));
    case 18: /*DATETIME2*/
      return parser.parseBuffer(5 + ~~((col.meta[0] + 1) / 2));
    case 6: /*NULL*/
    case 14: /*NEWDATE*/
    case 0: /*DECIMAL*/
    default:
      return null;
    }
    return 1;
  }

  function _getbit(buf, index) {
    return (buf[~~(index / 8)] >> (index % 8)) & 0x01;
  }

  function _fetchrow (packet, parser, info) {
    var tmap       = info.map,
        colpresent = info.colpresent,
        colnull    = parser.parseBuffer(~~((_calc1(colpresent) + 7) / 8));

    var null_bit_index = 0,
        is_null,
        fval,
        obj = {};
    for (var i = 0, n = tmap.columns.count; i < n; i++) {
      if (_getbit(colpresent, i) === 0) continue;
      is_null = _getbit(colnull, null_bit_index);
      if (!is_null) {
        fval = _getval(parser, tmap, i);
      } else {
        fval = null;
      }
      null_bit_index ++;
      obj[i] = fval;
    }
    return obj;
  }
}

function parseQueryEvent(packet, parser) {
  var eventData = {};

  var schema_len = 0,
      status_vars_len = 0;
  eventData.slaveId   = parser.parseUnsignedNumber(4);
  eventData.timestamp = parser.parseUnsignedNumber(4);
  schema_len          = parser.parseUnsignedNumber(1);
  eventData.errorCode = parser.parseUnsignedNumber(2);
  if (eventConfig.binlogVersion >= 4) {
    status_vars_len = parser.parseUnsignedNumber(2);
  }
  if (status_vars_len) {
    eventData.statusVars = parser.parseBuffer(status_vars_len);
  }
  eventData.schema    = parser.parseString(schema_len);
  parser.parseUnsignedNumber(1); // skip 0
  eventData.query     = parser.parseString(parser._packetEnd - parser._offset);

  packet.eventData = eventData;
}

function parseIntvarEvent(packet, parser) {
  var eventData = {};
  eventData.type  = parser.parseUnsignedNumber(1);
  eventData.value = [parser.parseUnsignedNumber(4), parser.parseUnsignedNumber(4)];
  packet.eventData = eventData;
}

const columnTypeName = {
  0: 'DECIMAL',
  1: 'TINY',
  2: 'SHORT',
  3: 'LONG',
  4: 'FLOAT',
  5: 'DOUBLE',
  6: 'NULL',
  7: 'TIMESTAMP',
  8: 'LONGLONG',
  9: 'INT24',
  10: 'DATE',
  11: 'TIME',
  12: 'DATETIME',
  13: 'YEAR',
  14: 'NEWDATE',
  15: 'VARCHAR',
  16: 'BIT',
  17: 'TIMESTAMP2',
  18: 'DATETIME2',
  19: 'TIME2',
  245: 'JSON',
  246: 'NEWDECIMAL',
  247: 'ENUM',
  248: 'SET',
  249: 'TINY_BLOB',
  250: 'MEDIUM_BLOB',
  251: 'LONG_BLOB',
  252: 'BLOB',
  253: 'VAR_STRING',
  254: 'STRING',
  255: 'GEOMETRY'
};

const logEventTypeName = [
  'UNKNOWN_EVENT',
  'START_EVENT_V3',
  'QUERY_EVENT',
  'STOP_EVENT',
  'ROTATE_EVENT',
  'INTVAR_EVENT',
  'LOAD_EVENT',
  'SLAVE_EVENT',
  'CREATE_FILE_EVENT',
  'APPEND_BLOCK_EVENT',
  'EXEC_LOAD_EVENT',
  'DELETE_FILE_EVENT',
  /**
    'NEW_LOAD_EVENT is like LOAD_EVENT except that it has a longer
    sql_ex, allowing multibyte TERMINATED BY etc; both types share the
    same class (Load_event)
  */
  'NEW_LOAD_EVENT',
  'RAND_EVENT',
  'USER_VAR_EVENT',
  'FORMAT_DESCRIPTION_EVENT',
  'XID_EVENT',
  'BEGIN_LOAD_QUERY_EVENT',
  'EXECUTE_LOAD_QUERY_EVENT',
  'TABLE_MAP_EVENT',
  /**
    'The PRE_GA event numbers were used for 5.1.0 to 5.1.15 and are
    therefore obsolete.
   */
  'PRE_GA_WRITE_ROWS_EVENT',
  'PRE_GA_UPDATE_ROWS_EVENT',
  'PRE_GA_DELETE_ROWS_EVENT',
  /**
    'The V1 event numbers are used from 5.1.16 until mysql-trunk-xx
  */
  'WRITE_ROWS_EVENT_V1',
  'UPDATE_ROWS_EVENT_V1',
  'DELETE_ROWS_EVENT_V1',
  /**
    'Something out of the ordinary happened on the master
   */
  'INCIDENT_EVENT',
  /**
    'Heartbeat event to be send by master at its idle time
    to ensure master's online status to slave
  */
  'HEARTBEAT_LOG_EVENT',
  /**
    'In some situations, it is necessary to send over ignorable
    data to the slave: data that a slave can handle in case there
    is code for handling it, but which can be ignored if it is not
    recognized.
  */
  'IGNORABLE_LOG_EVENT',
  'ROWS_QUERY_LOG_EVENT',
  /** Version 2 of the Row events */
  'WRITE_ROWS_EVENT',
  'UPDATE_ROWS_EVENT',
  'DELETE_ROWS_EVENT',
  'GTID_LOG_EVENT',
  'ANONYMOUS_GTID_LOG_EVENT',
  'PREVIOUS_GTIDS_LOG_EVENT',
  'TRANSACTION_CONTEXT_EVENT',
  'VIEW_CHANGE_EVENT',
  /* Prepared XA transaction terminal event similar to Xid */
  'XA_PREPARE_LOG_EVENT'
];

const logEventTypeParser = [
  'UNKNOWN_EVENT',
  'START_EVENT_V3',
  parseQueryEvent,
  'STOP_EVENT',
  'ROTATE_EVENT',
  parseIntvarEvent,
  'LOAD_EVENT',
  'SLAVE_EVENT',
  'CREATE_FILE_EVENT',
  'APPEND_BLOCK_EVENT',
  'EXEC_LOAD_EVENT',
  'DELETE_FILE_EVENT',
  'NEW_LOAD_EVENT',
  'RAND_EVENT',
  'USER_VAR_EVENT',
  parseFormatDescriptionEvent,
  'XID_EVENT',
  'BEGIN_LOAD_QUERY_EVENT',
  'EXECUTE_LOAD_QUERY_EVENT',
  parseTableMapEvent,
  'PRE_GA_WRITE_ROWS_EVENT',
  'PRE_GA_UPDATE_ROWS_EVENT',
  'PRE_GA_DELETE_ROWS_EVENT',
  parseRowsEvent, /*'WRITE_ROWS_EVENT_V1'*/
  parseRowsEvent, /*'UPDATE_ROWS_EVENT_V1'*/
  parseRowsEvent, /*'DELETE_ROWS_EVENT_V1'*/
  'INCIDENT_EVENT',
  'HEARTBEAT_LOG_EVENT',
  'IGNORABLE_LOG_EVENT',
  'ROWS_QUERY_LOG_EVENT',
  parseRowsEvent, /*'WRITE_ROWS_EVENT'*/
  parseRowsEvent, /*'UPDATE_ROWS_EVENT'*/
  parseRowsEvent, /*'DELETE_ROWS_EVENT'*/
  'GTID_LOG_EVENT',
  'ANONYMOUS_GTID_LOG_EVENT',
  'PREVIOUS_GTIDS_LOG_EVENT',
  'TRANSACTION_CONTEXT_EVENT',
  'VIEW_CHANGE_EVENT',
  'XA_PREPARE_LOG_EVENT'
];
