import json
import requests
from bs4 import (
    BeautifulSoup,
)

MAX_INDEX = 100

def insert_index(word_arr, url, count):
    if not word_arr:
        return [(word, count)]
    n = len(word_arr)
    s = 0
    e = n - 1
    r = -1
    while s < e:
        m = (s + e) / 2
        value = word_arr[m][1]
        if value == count:
            while m < n and word_arr[m][1] == count: m += 1
            r = m
            break
        elif value < count:
            e = m - 1
            r = m
        else:
            s = m + 1
            r = m
    if r < MAX_INDEX:
        word_arr = word_arr[0:r] + [(url, count)] + word_arr[r+1:]
        word_arr = word_arr[0:MAX_INDEX]
        return word_arr
    else:
        return word_arr

INDEXES = {}

def put_into_index(url, keywords_count):
    global INDEXES
    for one in keywords_count:
        if one[0] in INDEXES:
            INDEXES[one[0]] = insert_index(INDEXES[one[0]], url, one[1])
        else:
            INDEXES[one[0]] = [(url, one[1])]

def search(q):
    global INDEXES
    gmissing = 0
    gn = len(q)
    possible = {}
    gradient = 1.0
    for word in q:
        if word in INDEXES:
            for one in INDEXES[word]:
                if one[0] in possible:
                    possible[one[0]] += gradient * one[1]
                else:
                    possible[one[0]] = gradient * one[1]
        else:
            gmissing += 1
        gradient *= 0.8
    possible = [(k, v) for (k, v) in possible.iteritems()]
    possible.sort(key=lambda x: -x[1])
    return possible[0:20]
    

SKIP_WORDS = [
    "the", "of", "to", "in", "it", "should", "will", "can", "may",
    "a", "an", "and", "or", "this", "that", "is", "are", "was", "by",
    "on", "were", "be", "for", "as", "with", "above"
]
WORD_STOPS = [
    " ", ",", ".", "(", ")", "~", "`", "!", "@", ";", "'",
    "#", "$", "%", "^", "&", "*", "-", "+", "=", "/", "\"",
    "<", ">", "?", "\\", "{", "}", "[", "]", "|", ":",
    "\n", "\t",
]
def split_into_words(text):
    word = ''
    words = []
    for ch in text:
        if ch in WORD_STOPS:
            word = word.lower()
            if word and not word in SKIP_WORDS:
                words.append(word)
            word = ''
            continue
        word += ch
    if word:
        words.append(word)
    return words

def histogram(text):
    dom = BeautifulSoup(text, "html.parser")
    a = dom.find_all('a')
    dom0 = dom.select('#bodyContent')
    if len(dom0) > 0:
        dom = dom0[0]
    text = dom.get_text()
    text = split_into_words(text)
    words = {}
    for word in text:
        if not word: continue
        if word in words:
            words[word] += 1
        else:
            words[word] = 1
    words = [(k, v) for (k, v) in words.iteritems()]
    words.sort(key=lambda x: -x[1])
    words = words[0:10]
    counts = {}
    for word_count in words:
        counts[word_count[0]] = word_count[1]
    links = [link.get('href') for link in a]
    return {
        "counts": counts,
        #"links": links,
    }

from flask import (
    Flask,
    Response,
    request,
)

app = Flask(__name__)
@app.route("/query")
def query():
    q = request.stream.read()
    q = split_into_words(q)
    result = search(q)
    return Response(json.dumps(result), mimetype="application/json")

@app.route("/spider", methods=["POST"])
def spider():
    url = request.stream.read()
    result = requests.get(url, verify=False)
    if result.status_code / 100 == 2:
        hist = histogram(result.text)
        put_into_index(result.url, [(k, v) for (k, v) in hist["counts"].iteritems()])
        hist["url"] = result.url
        return Response(json.dumps(hist), mimetype="application/json")
    else:
        return Response("400", status=400)

if __name__ == "__main__":
    app.run("127.0.0.1", 8080)
