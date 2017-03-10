"""
action = query, insert, update, delete
schema, table
data = query:sql, insert,update,delete:row
extra = query:intvar, update:origin_row, insert,update:none
"""
def process(action, schema, table, data, extra):
    if action == "query":
        print ("QUERY", extra, schema, table, data)
    elif action == "insert":
        if "id" in data:
            print ("INSERT", data["id"], schema, table)
    elif action == "update":
        if "id" in data:
            print ("UPDATE", data["id"], schema, table)
    elif action == "delete":
        if "id" in data:
            print ("DELETE", data["id"], schema, table)

def post_all(binlog_name, binlog_pos):
    print (binlog_name, binlog_pos)
