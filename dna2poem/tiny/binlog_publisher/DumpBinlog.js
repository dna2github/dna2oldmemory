/*
 @author: Seven Lju
 @date: 2016-05-16
 @desc: main function for dumping binary log
 */

var Sequence            = require('mysql/lib/protocol/sequences/Sequence');
var Util                = require('util');
var Packets             = require('mysql/lib/protocol/packets');
var ComDumpBinlogPacket = require('./ComDumpBinlogPacket');
var BinlogPacket        = require('./BinlogPacket');

module.exports = dumpBinlog;

function bindToCurrentDomain(callback) {
  if (!callback) return;
  var domain = process.domain;
  return domain
    ? domain.bind(callback)
    : callback;
}

function dumpBinlog (connection, options, callback) {
  if (!callback && typeof options === 'function') {
    callback = options;
    options = {};
  }
  connection._implyConnect();
  return connection._protocol._enqueue(new DumpBinlog({
    startPos : options.startPos || 0,
    flags    : options.flags    || 0x0200,
    serverId : options.serverId || 0,
    logName  : options.logName  || 'mysql-binlog.000001',
    timeout  : options.timeout,
    currentConfig : connection.config
  }), bindToCurrentDomain(callback));
}


Util.inherits(DumpBinlog, Sequence);
function DumpBinlog(options, callback) {
  if (!callback && typeof options === 'function') {
    callback = options;
    options = {};
  }

  Sequence.call(this, options, callback);

  this.options = options;
  this.binlog = [];
}

DumpBinlog.prototype.start = function() {
  this.emit('packet', new ComDumpBinlogPacket({
    startPos : this.options.startPos,
    flags    : this.options.flags,
    serverId : this.options.serverId,
    logName  : this.options.logName
  }));
};

DumpBinlog.prototype['ErrorPacket'] = function(packet) {
  var err = this._packetToError(packet);
  err.fatal = true;
  this.end(err);
};

DumpBinlog.prototype['BinlogPacket'] = function(packet) {
  this.binlog.push(packet.message);
};

DumpBinlog.prototype['EofPacket'] = function(packet) {
  this.end(null, this.binlog);
};

DumpBinlog.prototype.determinePacket = function(firstByte, parser) {
  switch (firstByte) {
    case 0x00: return BinlogPacket;
    case 0xfe: return Packets.EofPacket;
    case 0xff: return Packets.ErrorPacket;
  }
};

