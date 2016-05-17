# Python-MySQL-Replication

### binlogstream.py

###### cannot read out binlog row event raw data

It uses `COM_BINLOG_DUMP_GTID (0x1e)` instead of `COM_BINLOG_DUMP (0x12)`.
Currently in MariaDB `CMD_END (0x1e)`.

Change it to 0x12 to support reading binlog for both MySQL and MariaDB.

`binlog_name` and `binlog_pos` can be added to limit binlog output.

```python
self.__binlog_name = binlog_name.encode()
self.__binlog_pos = binlog_pos
...
# binlog_name_info_size (4 bytes)
#prelude += struct.pack('<I', 3)
prelude += struct.pack('<I', len(self.__binlog_name))
# empty_binlog_name (4 bytes)
#prelude += b'\0\0\0'
prelude += self.__binlog_name
...
```
