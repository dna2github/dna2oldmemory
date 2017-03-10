import os
import sys
import json
from pymysqlreplication import BinLogStreamReader
from pymysqlreplication.event import (
    RotateEvent,
    QueryEvent,
    IntvarEvent,
)
from pymysqlreplication.row_event import (
    UpdateRowsEvent,
    WriteRowsEvent,
    DeleteRowsEvent,
)


SELF_DIR = os.path.dirname(__file__)

pimport = None
if len(sys.argv) > 1:
    package = sys.argv[1]
    filename = os.path.join(SELF_DIR, package, "config.json")
    sys.path = [os.path.join(SELF_DIR, package)] + sys.path
    try:
        pimport = __import__("process")
    except:
        sys.path.pop()
        sys.path.append(SELF_DIR)
        pimport = __import__("process")
else:
    filename = os.path.join(SELF_DIR, "sync.json")
    sys.path.append(SELF_DIR, package)
    pimport = __import__("process")

config = None
with open(filename, "r") as f:
    config = json.loads(f.read())

mysql_settings = {
    "host": config["database"]["host"],
    "port": config["database"]["port"],
    "user": config["database"]["user"],
    "passwd": config["database"]["passwd"],
}
stream = BinLogStreamReader(
    connection_settings=mysql_settings,
    server_id=0,
    log_file=config["binlog"]["name"],
    log_pos=config["binlog"]["pos"],
    resume_stream=True
)

def simple_mim_query_sql(sql):
    return ' '.join([one.strip() for one in sql.split('\n')])

tmp_id = -1
for evt in stream:
    if isinstance(evt, RotateEvent):
        config["binlog"]["name"] = evt.next_binlog
    elif isinstance(evt, IntvarEvent):
        tmp_id = evt.value
    elif isinstance(evt, QueryEvent):
        pimport.process("query", evt.schema, "", simple_mim_query_sql(evt.query), tmp_id)
        tmp_id = -1
    elif isinstance(evt, UpdateRowsEvent):
        for row in evt.rows:
            pimport.process("update", evt.schema, evt.table, row["after_values"], row["before_values"])
    elif isinstance(evt, WriteRowsEvent):
        for row in evt.rows:
            pimport.process("insert", evt.schema, evt.table, row["values"], None)
    elif isinstance(evt, DeleteRowsEvent):
        for row in evt.rows:
            pimport.process("delete", evt.schema, evt.table, row["values"], None)
    config["binlog"]["pos"] = evt.packet.log_pos

stream.close()

with open(filename, 'w+') as f:
    f.write(json.dumps(config))

pimport.post_all(config["binlog"]["name"], config["binlog"]["pos"])
