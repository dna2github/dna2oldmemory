from walker import TextWalker

# parse common INSERT, UPDATE, DELETE statement without function call in sql
# UPDATE tablename SET field='value' WHERE id=1
# if UPDATE tablename SET field='value' WHERE id=1 and group<2 => {id:1, group: 2}
# the same for DELETE
# currently not support like NOW()

def parseHeader(parser, state):
    op = parser.next()
    op = op[0].lower()
    state["op"] = op
    if op == "insert" or op == "delete":
        parser.next() # skip INTO, FROM
    table = parser.next()
    table = table[0]
    state["table"] = table
    return True

def parseInsert(parser, state):
    sleep = True
    memory = None
    fields = []
    values = []
    stage = 0
    level = 0
    for one in parser:
        token = one[0]
        stop  = one[1]
        if stop in ['(']:
            sleep = False
            stage += 1
            level += 1
            if level > 1 and len(token) > 0:
                memory = token # SQL function
            continue
        if sleep: continue
        if stage == 0: continue
        if len(token) > 0:
            if token == 'NULL':
                memory = None
            else:
                memory = token
        elif stop in ['"', "'"]:
            memory = parser.skipString()[0]
        if stop in [',', ')']:
            if level > 1:
                pass
            elif stage == 1:
                fields.append(memory)
                memory = None
            else:
                values.append(memory)
                memory = None
        if stop in [')']:
            if level < 2:
                sleep = True
            level -= 1
    #print fields, values
    for i, field in enumerate(fields):
        state["data"][field] = values[i]

def parseUpdate(parser, state):
    sleep = True
    stage = 0
    field = None
    value = None
    for one in parser:
        token = one[0]
        stop  = one[1]
        if token in ['SET', 'WHERE', 'OR', 'AND', 'NOT']:
            stage += 1
            sleep = False
            continue
        if sleep: continue
        if stage == 0: continue
        if len(token) > 0:
            if field is None:
                field = token
            else:
                if token == 'NULL':
                    value = None
                else:
                    value = token
                state["data"][field] = value
                field = None
                value = None
        elif stop in ['"', "'"]:
            value = parser.skipString()[0]
            state["data"][field] = value
            field = None
            value = None

def parseDelete(parser, state):
    sleep = True
    stage = 0
    field = None
    value = None
    for one in parser:
        token = one[0]
        stop  = one[1]
        if token in ['WHERE', 'OR', 'AND', 'NOT']:
            stage += 1
            sleep = False
            continue
        if sleep: continue
        if stage == 0: continue
        if len(token) > 0:
            if field is None:
                field = token
            else:
                if token == 'NULL':
                    value = None
                else:
                    value = token
                state["data"][field] = value
                field = None
                value = None
        elif stop in ['"', "'"]:
            value = parser.skipString()[0]
            state["data"][field] = value
            field = None
            value = None

def parse(sql):
    state = {
        "op": None,
        "table": None,
        "data": {},
    }
    parser = TextWalker(sql, stops=['=', ',', '"', '\'', '(', ')', ' ', '\t', '\n', '\r'])
    parseHeader(parser, state)
    op = state["op"]
    if op == "insert":
        parseInsert(parser, state)
    elif op == "update":
        parseUpdate(parser, state)
    elif op == "delete":
        parseDelete(parser, state)
    print state
    return state
