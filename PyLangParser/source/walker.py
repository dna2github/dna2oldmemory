"""
@author: Seven Lju
@date: 2016.04.27
"""

constStops = [
  '\n', '\t', ' ', '~', '!', '#', '$', '%',
  '@', '&', '*', '(', ')', '-', '=', '+', '[',
  ']', '{', '}', '\\', '|', '\'', '"', ';',
  ':', ',', '<', '.', '>', '/', '?', '^', '`'
]

class TextWalker(object):

  def __init__(self, text, stops=constStops):
    self.text = text
    self.cursor = 0
    self.n = len(text)
    self.stops = stops
    self.token = ""
    self.stop = '\n'

  def __iter__(self):
    return self

  def __next__(self):
    if self.cursor >= self.n:
      raise StopIteration()
    i = self.cursor
    while True:
      if i >= self.n:
        self.stop = '\0'
        break
      self.stop = self.text[i]
      if self.stop in self.stops:
        break
      i += 1
    self.token = self.text[self.cursor:i]
    self.cursor = i + 1
    return (self.token, self.stop)

  def next(self):
    return self.__next__()

  def skipString(self, pair=None, markEscape='\\'):
    _stops = self.stops
    if pair is None:
      pair = self.stop
    self.stops = [pair, markEscape]
    i = self.cursor
    token = ""
    while True:
      string = self.__next__()
      token += string[0]
      stop = string[1]
      if stop == pair:
        break
      elif stop == markEscape:
        token += stop
        token += self.text[self.cursor]
        self.cursor += 1
      if stop == '\0':
        break
      if self.cursor >= self.n or self.cursor <= 0:
        token += self.stop
        break
    self.stops = _stops
    self.token = token
    return (self.token, self.stop)

  def skipLongString(self, start, end=None, markEscape='\\'):
    if end is None:
      end = start
    if self.cursor >= self.n:
      return ("", '\0')
    if not (start and end):
      return ("", self.stop)
    if self.stop != start[0]:
      return ("", self.stop)
    if self.text[self.cursor:self.cursor+len(start)-1] != start[1:]:
      return ("", self.stop)
    _stops = self.stops
    token = ""
    end1st = end[0]
    endlen = len(end)
    self.stops = [end1st, markEscape]
    self.cursor += len(start) - 1
    while True:
      string = self.__next__()
      token += string[0]
      stop = string[1]
      if stop == end1st and self.cursor + endlen - 1 <= self.n:
        if self.text[self.cursor:self.cursor+endlen-1] == end[1:]:
          self.stop = end
          break
        else:
          token += stop
      elif stop == markEscape:
        token += stop
        token += self.text[self.cursor]
        self.cursor += 1
      if stop == '\0':
        break
      if self.cursor >= self.n or self.cursor <= 0:
        token += self.stop
        break
    self.cursor += endlen - 1
    self.stops = _stops
    self.token = token
    return (self.token, self.stop)

  def seeForward(self, n):
    return self.text[self.cursor:self.cursor + n]

  def seeBackward(self, n):
    start = self.cursor - n
    return self.text[start:self.cursor]
