"""
@author: Seven Lju
@date: 2014-11-15

a generic source walker to tokenize source file.
"""

import re

common_stop = [
    '\n', '\t', ' ', '~', '!', '#', '$', '%',
    '@', '&', '*', '(', ')', '-', '=', '+', '[',
    ']', '{', '}', '\\', '|', '\'', '"', ';',
    ':', ',', '<', '.', '>', '/', '?', '^', '`'
]


class SourceWalker(object):
    def __init__(self, filename, stop=common_stop):
        self._f = None
        self._filename = filename
        if stop is None:
            stop = ['\n']
        self._stop = stop
        if '\n' not in self._stop:
            self._stop.append('\n')
        self.reset()

    def __del__(self):
        if self._f is not None:
            self._f.close()

    def reset(self):
        if self._f is not None:
            self._f.close()
        self._f = open(self._filename, 'r')
        self._eof = False
        self._eol = False
        self._cursor = 0
        self._line = ""
        self.position = 0
        self.stop = None
        self.token = None

    def reachEndOfLine(self):
        """
        check if a line is parsed
        """
        return self._eol

    def backward(self, length):
        """
        get backward characters from current position
            _line="ab cde  f", token="cde", stop=' '
            backward(1)=" "
            backward(2)="b "
            backward(3)="ab "
            backward(4)="ab "
        """
        end = self._cursor - len(self.token) - 1
        start = end - length - len(self.token)
        if start < 0:
            start = 0
        return self._line[start:end]

    def forward(self, length):
        """
        get forward characters from current position
            _line="ab cde  f", token="cde", stop=' '
            forward(1)=" "
            forward(2)=" f"
            forward(3)=" f"
        """
        n = len(self._line)
        start = self._cursor
        end = start + length
        if end > n:
            end = n
        return self._line[start:end]

    def backwardWithSkip(self, length, skip=[' ', '\t']):
        """
        get backward characters from current position
            _line="ab cde  f", token="cde", stop=' '
            backward(1)="b"
            backward(2)="ab"
            backward(3)="ab"
        """
        end = self._cursor - 1
        start = end - length
        i = 0
        for char in self._line[0:end][::-1]:
            if char not in skip:
                break
            i += 1
        start -= i + len(self.token)
        end -= i + len(self.token)
        if start < 0:
            start = 0
        return self._line[start:end]

    def forwardWithSkip(self, length, skip=[' ', '\t']):
        """
        get forward characters from current position
            _line="ab cde  f", token="cde", stop=' '
            forward(1)="f"
            forward(2)="f"
        """
        n = len(self._line)
        start = self._cursor
        end = start + length
        i = 0
        for char in self._line[start:]:
            if char not in skip:
                break
            i += 1
        start += i
        end += i
        if end > n:
            end = n
        return self._line[start:end]

    def next(self, append=False):
        """
        parse next word before a stop
        """
        if self._eof:
            return False
        if self._cursor >= len(self._line):
            self._line = self._f.readline()
            self._cursor = 0
        if len(self._line) == 0:
            self._eof = True
            self._f.close()
            self._f = None
            return False
        self._eol = False
        if append:
            if self.token is None:
                self.token = ""
            if self.stop is not None:
                self.token += self.stop
        else:
            self.token = ""
        self.stop = None
        onechar = self._line[self._cursor]
        self.position += 1
        self._cursor += 1
        while onechar not in self._stop:
            self.token += onechar
            onechar = self._line[self._cursor]
            self.position += 1
            self._cursor += 1
        self.stop = onechar
        if self.stop == '\n':
            self._eol = True
        return True

    def intent(self, hits=[' ', '\t']):
        """
        get intent string
        """
        if len(self._line) == 0:
            return ""
        count = 0
        while self._line[count] in hits:
            count += 1
        return self._line[0:count]

    def string(self, start='"', backslash=True):
        """
        parse a string
        """
        if self.stop != start:
            return False
        end = self.stop
        token = ""
        skipNext = False
        while True:
            if not self.next():
                # meet EOF
                self.token = token
                self.stop = None
                return False
            if len(self.token) > 0:
                token += self.token
            if self.stop == end:
                if len(self.token) > 0 or not skipNext:
                    self.token = token
                    return True
            token += self.stop
            if backslash and self.stop == '\\' and not skipNext:
                skipNext = True
            else:
                skipNext = False

    def lineToEnd(self, start='/', stop=None):
        """
        parse the rest of a line; usually to get line comment and multiline
        string.
        """
        if start is not None and self.stop != start:
            return False
        originalStops = self._stop
        self._stop = ['\n']
        if stop is not None and stop != '\n':
            self._stop.append(stop)
        if self.stop == '\n':
            pass
        elif not self.next(True):
            raise Exception("Unknown error")
        self._stop = originalStops
        return True

    def isTokenNumber(self):
        """
        check if the current token is a number (integer or float without sign
        mark)
        """
        if re.match("^(([0-9]+\\.?)|([0-9]*\\.[0-9]+))$", self.token) is None:
            return False
        return True

    def isAtEmptyLine(self, hits=[' ', '\t', '\n']):
        """
        check if the line is empty except space and tab
        """
        if '\n' not in hits:
            hits.append('\n')
        for char in self._line:
            if char not in hits:
                return False
        return True

    def isAtCommentLine(self, commentLine='//', hits=[' ', '\t']):
        """
        check if the line only contains comment
        """
        try:
            index = self._line.index(commentLine)
            for char in self._line[0:index]:
                if char not in hits:
                    return False
            return True
        except:
            return False


class Token(object):
    def __init__(self, token, category, line, level):
        """
        token initialization
        """
# category = token, number, string, comment
        self.category = category
# token text
        self.token = token
# line number
#   hello
#   world
#   parser
# 'world' is at Line 2
        self.line = line
# scope level
#   def func():
#       print "hello world"
# 'def' is at Level 1 meanwhile 'print' is at Level 2
        self.level = level
# related to previous line
#   print (
#     "hello world"
#   )
# "hello world" is connected to its previous line with print
        self.connectPrevLine = False

    def connectToPrevLine(self, enable=True):
        self.connectPrevLine = enable


class SourceFile(object):
    def __init__(self, name, filename=None, mapping=None):
        """
        source file initialization
        """
# source file name, recommended econding the name:
#   c/c++:      /full/path/filename
#   java:       package.full.path.filename
#   python:     package.full.path.filename
#   javascript: /full/path/filename
#   asm:        /full/path/filename
        self.name = name
        self.filename = filename
# source mapping, to connect definition file and implementation
# file:
#   c/c++:      source.h    --map--> source.c
#   java:       source.java --map--> source_jni.h
#   python:     source.py   --map--> source_py.h
#   javascript: source.js   --map--> source_gyp.h
#   asm:        source.S    --map--> source.c
        self.mapping = mapping
# source reference, to declare more token:
#   c/c++:      source.h    --inc--> #include another.h
#   java:       sourc.java  --inc--> import another.package.Sth
#   python:     source.py   --inc--> import another.pakcage
#   javascript: source.js   --inc--> require('another')
#                           --inc--> [implicit, in HTML]
#   asm:        source.S    --inc--> .include another.S
#                           --inc--> [implicit, in Makefile]
        self.references = []
# source token stream
        self.tokens = []
# reserved to use, state machine to support building an AST for
# programming language
        self.state = None

    def dump(self, excludeCategorySet=None):
        if excludeCategorySet is None:
            excludeCategorySet = []
        lineType = ['-', '|']
        mark = 0
        level = 0
        for t in self.tokens:
            if t.category in excludeCategorySet:
                continue
            if t.connectPrevLine:
                mark = 1
            else:
                mark = 0
                level = t.level - 1
            print "%5d#%s- %s(%s) %s" % (
                t.line, lineType[mark],
                ' ' * (level * 2),
                t.category, t.token)

    def _string_index(self, string, substring):
        if string is None:
            return -1
        if substring is None:
            return -1
        try:
            return string.index(substring)
        except:
            return -1

    def _filterCategory(self, querySet=None, categorySet=[], exclude=True):
        if exclude:
            if categorySet is None:
                return querySet
            if len(categorySet) == 0:
                return querySet
        else:
            if categorySet is None:
                return []
            if len(categorySet) == 0:
                return []
        if querySet is None:
            querySet = self.tokens
        if exclude:
            return [
                one for one in querySet
                if one.category not in categorySet
            ]
        else:
            return [
                one for one in querySet
                if one.category in categorySet
            ]

    def queryExact(self, token, querySet=None, excludeCategorySet=None):
        if querySet is None:
            querySet = self.tokens
        querySet = self._filterCategory(categorySet=excludeCategorySet,
                                        querySet=querySet)

        return [
            one for one in querySet
            if one.token == token
            or one.token.split('.')[-1] == token
        ]

    def queryFuzzy(self, token, querySet=None, excludeCategorySet=None):
        if querySet is None:
            querySet = self.tokens
        querySet = self._filterCategory(categorySet=excludeCategorySet,
                                        querySet=querySet)

        return [
            one for one in querySet
            if self._string_index(one.token, token) >= 0
        ]

    def queryCategory(self, categorySet, querySet=None):
        if querySet is None:
            querySet = self.tokens
        if not isinstance(categorySet, list):
            categorySet = [categorySet]
        return self._filterCategory(categorySet=categorySet,
                                    exclude=False,
                                    querySet=querySet)

    def getReference(self, module=None, filename=None):
        if module is None and filename is None:
            return None

        if module is not None:
            for ref in self.references:
                if ref.name == module:
                    return ref
        elif filename is not None:
            for ref in self.references:
                if ref.filename == filename:
                    return ref
        return None
