/*
 @author: Seven Lju
 @date: 2016-05-16
 @desc: example to read binary log
 */
var mysql      = require('mysql');
var dumpBinlog = require('./DumpBinlog');

var connection = mysql.createConnection({
  host       : 'localhost',
  socketPath : '/tmp/mysql.sock',
  user       : 'root',
  password   : '',
  debug      : true
});


// assume executing below commands in order for local test
connection.query('SET @master_binlog_checksum=\'NONE\'');
connection.query('SET @mariadb_slave_capabilitym=4');
dumpBinlog(connection, {startPos: 4, logName: 'binlog.000001'});

connection.on("close", function (err) {
    console.log("SQL CONNECTION CLOSED.");
});
connection.on("error", function (err) {
    console.log("SQL CONNECTION ERROR: " + err);
});
