"""
@author: Seven Lju
@date: 2014-11-15

a python source code parser without using ast lib
"""

from core import SourceWalker, Token, SourceFile

python_keywords = [
    'if', 'for', 'while', 'else', 'not', 'in',
    'try', 'except', 'class', 'def', 'with',
    'elif', 'and', 'as', 'assert', 'del', 'break',
    'continue', 'exec', 'finally', 'from', 'is',
    'global', 'import', 'lambda', 'or', 'pass',
    'print', 'raise', 'return', 'yield'
]


class PythonSource(SourceFile):
    _default_exclude_for_query = ["keyword", "string", "text",
                                  "comment", "number"]

    def __init__(self, name, filename, mapping=None):
        super(PythonSource, self).__init__(name, filename, mapping)

    def dump(self, excludeSet=None):
        if excludeSet is None:
            excludeSet = [
                one for one in self._default_exclude_for_query
                if one != "keyword"
            ]
        super(PythonSource, self).dump(excludeSet)

    def newToken(self, token):
        self.tokens.append(token)

    def getReferences(self):
        references = self.queryCategory("ref")
        return [one.token for one in references]

    def newReference(self, module=None, filename=None, reference=None):
        if reference is not None:
            module = reference.name
            filename = reference.filename

        if module is None or filename is None:
            return False

        ref = self.getReference(filename=filename)
        if ref is not None:
            if ref.name == module:
                return True
            return False
        if reference is None:
            parser = PythonParser(module, filename)
            ref = parser.getSource()
        else:
            ref = reference
        self.references.append(ref)

    def queryExact(self, token, querySet=None, excludeNotSymbol=True):
        if excludeNotSymbol:
            excludeSet = self._default_exclude_for_query
        else:
            excludeSet = None
        return super(PythonSource, self).queryExact(
            token, querySet, excludeSet)

    def queryFuzzy(self, token, querySet=None, excludeNotSymbol=True):
        if excludeNotSymbol:
            excludeSet = self._default_exclude_for_query
        else:
            excludeSet = None
        return super(PythonSource, self).queryFuzzy(
            token, querySet, excludeSet)


class PythonParser:
    def __init__(self, module, filename):
        self._source = None
        self._state = {
            "next": None,
            "data": None,
        }
        self._parse(module, filename)

    def getSource(self):
        return self._source

    def _parse_callback(self, category, lineNo, lineType, level, token, stop):
        state = self._state["next"]

        if stop == ':':
            self._state["next"] = None
            self._state["data"] = None
# case like "def func(foo=test(1,2,3), bar=None):"
        elif state == "pinit" and stop == '(':
            self._state["data"] += 1
        elif state == "pinit" and stop == ')':
            self._state["data"] -= 1

        if category is None:
# case like "def func(foo = None):"
            if stop == '=' and state == "param":
                self._state["next"] = "pinit"
# case like "def func(foo=test(1,2,3), bar=None):"
            elif stop == ',' and state == "pinit":
                if self._state["data"] == 0:
                    self._state["next"] = "param"
# case like "from os import *"
            elif stop == '*' and state == "ref":
                self._source.newToken(
                    Token(self._state["data"] + stop, "ref", lineNo, level)
                )
                self._state["next"] = None
                self._state["data"] = None
            return

        if category in ["comment", "number", "string"]:
            if len(token) > 0 and token[-1] == '\n':
                category = "text"
            self._source.newToken(
                Token(token, category, lineNo, 0))
            return

# mark more categories: class, func, ref, super, param, pinit, keyword
# ref_from is a temporary category
        if token in python_keywords:
            if token == "class":
                self._state["next"] = "class"
            elif token == "def":
                self._state["next"] = "func"
            elif token == "from":
                self._state["next"] = "ref_from"
            elif token == "import":
                self._state["next"] = "ref"
            t = Token(token, "keyword", lineNo, level)
            if lineType == 'C':
                t.connectToPrevLine()
            self._source.newToken(t)
            return

        if state is not None:
            category = state
            if category == "class":
                self._state["next"] = "super"
            elif category == "func":
                self._state["data"] = 0
                self._state["next"] = "param"
# case like "def func(foo=None):"
            elif category == "param" and stop == '=':
                self._state["next"] = "pinit"
# from package.path import sth => ref:package.path.sth
# import package.path.sth      => ref:package.path.sth
            elif category == "ref_from":
                self._state["data"] = token + '.'
                self._state["next"] = None
                return
            elif category == "ref":
                if self._state["data"] is None:
                    self._state["data"] = ""
                token = self._state["data"] + token
                self._state["data"] = None
                self._state["next"] = None
        t = Token(token, category, lineNo, level)
        if lineType == 'C':
            t.connectToPrevLine()
        self._source.newToken(t)

    def _parse(self, module, filename):
        self._source = PythonSource(module, filename)
        _private_parse(filename, self._parse_callback)


def _private_parse_string(walker):
    longString = False
# assume the stop is ' or "
    if walker.forward(2) == walker.stop * 2:
        longString = True
    buf = ""
    if longString:
        walker.string(walker.stop)
        walker.string(walker.stop)
    while True:
        if not walker.string(walker.stop):
            raise Exception("String without pair quote marks")
        buf += walker.token
        if not longString:
            break
        elif walker.forward(2) == walker.stop * 2:
            buf += '\n'
            break
        else:
            buf += walker.stop
    if longString:
        walker.string(walker.stop)
        walker.string(walker.stop)
    walker.token = buf


def _private_fill_length(word, length, fill=' '):
    n = len(word)
    if n >= length:
        return word
    else:
        return word + (fill * (length - n))


def _private_noop_callback(c, n, t, l, w, s):
    pass


def _private_callback(category, lineNo, lineType, level, token, stop):
    if category is None:
        return
    if category in ["number", "string", "comment"]:
        print "%s:#%5d#|%s  | %s" % (
            _private_fill_length(category, 8), lineNo, lineType, token)
    elif token in python_keywords:
        return
    else:
        print "%s:#%5d#|%s%2d| %s" % (
            _private_fill_length(category, 8), lineNo, lineType, level, token)


# callback(category, lineNo, lineType, level, token)
#   category = number, string, comment, token
#   lineType = L, C
def _private_parse(filename, callback=_private_noop_callback):
    if callback is None:
        callback = _private_noop_callback
    walker = SourceWalker(filename)
    dot = False
    intent = [0]
    level = 1
    line = 1
    newLine = True
    continueLine = False
    continueableLine = 0
    markLine = 'L'
    while walker.next(dot):
        if continueLine:
            markLine = 'C'
        else:
            markLine = 'L'

        if walker.isAtEmptyLine():
            walker.lineToEnd(start=None)
            newLine = True
            line += 1
            continue
        if walker.isAtCommentLine(commentLine='#'):
            walker.lineToEnd(start=None)
            callback("comment", line, markLine,
                     level, walker.token.strip(), walker.stop)
            newLine = True
            line += 1
            continue

        if newLine:
            newLine = False
            intentTmp = len(walker.intent())
            intentPrev = intent[-1]
            if continueLine:
                pass
            elif intentPrev < intentTmp:
                intent.append(intentTmp)
                level = len(intent)
            elif intentPrev > intentTmp:
                try:
                    level = intent.index(intentTmp) + 1
                    intent = intent[0:level]
                except:
                    raise Exception("Invalid intent")

        dot = False
# the string cases:
#   a="string"       a='''string'''     """comment"""    b=a+"string"
#   a!='string'
# the walker.toekn should be empty from the previous walker.next() in
# 'while'; ignore walker.token this time and read string value
# directly.
        if walker.stop in ['"', '\'']:
            _private_parse_string(walker)
            callback("string", line, markLine,
                     level, walker.token, walker.stop)
            line += walker.token.count('\n')
            continue
# the member access cases:
#   from package0.package1.file import sth
#   node0.node1.node2.leaf = sth
        if walker.stop == '.':
            dot = True
            continue

# the multiple line cases:
#   tuple/callable (
#       arg0, arg1, arg2, ...)
#   array [
#       sth]
#   dict {
#       key0: val0,
#       key1: val1,
#       ...
#   }
# at the end of this function, there is another case:
#   statement_segment0 \
#       statment_segment1 \
#       statment_segment2 \
#       ...
        if walker.stop in ['(', '[', '{']:
            continueableLine += 1
        elif walker.stop in [')', ']', '}']:
            continueableLine -= 1

        if len(walker.token) == 0:
            callback(None, line, markLine,
                     level, None, walker.stop)
        elif walker.isTokenNumber():
            callback("number", line, markLine,
                     level, walker.token, walker.stop)
        else:
            callback("token", line, markLine,
                     level, walker.token, walker.stop)

# the tail comment case:
#   statements # comment
        if walker.stop == '#':
            walker.lineToEnd(start='#')
            callback("comment", line, markLine,
                     level, walker.token, walker.stop)
        if walker.stop == '\n':
            if continueableLine > 0:
                continueLine = True
            elif walker.backwardWithSkip(1) == '\\':
                continueLine = True
            else:
                continueLine = False
            newLine = True
            line += 1


def _private_option():
    parser = argparse.ArgumentParser(
        description='Extract a Python source file into tokens.',
        usage='python -m engine7.parser.python.Python filename')
    parser.add_argument('filename', metavar='filename',
                        help='file name of a Python source file')
    args = parser.parse_args()
    return args.filename


import argparse

if __name__ == "__main__":
    filename = _private_option()
    _private_parse(filename, _private_callback)
