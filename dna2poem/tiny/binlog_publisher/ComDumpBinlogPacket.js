/*
 @author: Seven Lju
 @date: 2016-05-16
 @desc: mysql/mariadb binary log packet command
 */

module.exports = ComDumpBinlogPacket;

const BINLOG_DUMP_NON_BLOCK = 0x0100;
const BINLOG_SEND_ANNOTATE_ROWS_EVENT = 0x0200;

function ComDumpBinlogPacket(options) {
  options = options || {};

  this.command  = 0x12;
  this.startPos = options.startPos || 4;
  this.flags    = options.flags    || BINLOG_SEND_ANNOTATE_ROWS_EVENT;
  this.serverId = options.serverId || 0;
  this.logName  = options.logName  || 'mysql-binlog.000001';
}

ComDumpBinlogPacket.prototype.parse = function(parser) {
  this.command  = parser.parseUnsignedNumber(1);
  this.startPos = parser.parseUnsignedNumber(4);
  this.flags    = parser.parseUnsignedNumber(2);
  this.serverId = parser.parseUnsignedNumber(4);
  this.logName  = parser.parseNullTerminatedString();
};

ComDumpBinlogPacket.prototype.write = function(writer) {
  writer.writeUnsignedNumber(1, this.command);
  writer.writeUnsignedNumber(4, this.startPos);
  writer.writeUnsignedNumber(2, this.flags);
  writer.writeUnsignedNumber(4, this.serverId);
  writer.writeNullTerminatedString(this.logName);
};
