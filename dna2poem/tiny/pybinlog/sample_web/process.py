"""
action = query, insert, update, delete
schema, table
data = query:sql, insert,update,delete:row
extra = query:intvar, update:origin_row, insert,update:none
"""
import os
import json
from ghost import Ghost
from bs4 import BeautifulSoup

BASE_DIR = os.path.dirname(__file__)
ghost = Ghost()

# object (id, hello, world)
# hello (id, object_id, data)
# world (id, object_id, data)
related_table = ["object", "hello", "world"]
related_schema = ["test"]
items = {}

def process(action, schema, table, data, extra):
    global items
    if action == "query":
        return
    if schema not in related_schema: return
    if table not in related_table: return
    obj = None
    body = False
    if table == "object":
        table = "object"
        obj = data.get("id")
        body = True
    else:
        table = "object"
        obj = data.get("object_id")
    if obj is None: return
    if table not in items: items[table] = {}
    if action == "insert":
        items[table][obj] = "u"
    elif action == "update":
        items[table][obj] = "u"
    elif action == "delete":
        if body:
            items[table][obj] = "d"
        else:
            items[table][obj] = "u"

def post_all(binlog_name, binlog_pos):
    global items
    print items
    login()
    if "object" in items:
        objects = items["object"]
        for _id, _op in objects.iteritems():
            if _op == "u":
                update_object(_id)
            else:
                delete_object(_id)

def _strip(text):
    return " ".join(text.split('\n')).strip()

def _split(text):
    return [one for one in _strip(text).split(' ' ) if one != ""]

def parse_object_content(content):
    obj = {}
    soup = BeautifulSoup(content, "html.parser")
    obj["div"] = _strip(soup.select_one("div").text)
    obj["select"] = _split(soup.select_one("select > option").text)
    print obj
    return obj

session = None
def login():
    global session
    if session is not None: return
    session = ghost.start()
    session.wait_timeout = 30
    session.open("http://127.0.0.1:8080/login/")
    session.wait_for_page_loaded()
    session.evaluate("$('#username').val('username'); $('#password').val('password');")
    session.click(selector="#login", btn=0)
    session.wait_for_selector("#ready")

def update(url, filename, parser):
    global session
    print url, filename
    session.open(url)
    session.wait_for_page_loaded()
    content, exres = session.evaluate('document.body.innerHTML')
    obj = parser(content)

    with open(filename, "w+") as f:
        f.write(json.dumps(obj).encode('utf-8'))

def delete(filename):
    print filename
    os.remove(filename)

def update_object(_id):
    url = "http://127.0.0.1:8080/objects/%d/" % _id
    filename = os.path.join(BASE_DIR, "data", "obj%d" % _id)
    update(url, filename, parse_object_content)

def delete_object(_id):
    filename = os.path.join(BASE_DIR, "data", "obj%d" % _id)
    delete(filename)
