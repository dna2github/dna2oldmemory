import random

class Deck:
    normalCards = [
        # Tiao
        1, 2, 3, 4, 5, 6, 7, 8, 9,
        1, 2, 3, 4, 5, 6, 7, 8, 9,
        1, 2, 3, 4, 5, 6, 7, 8, 9,
        1, 2, 3, 4, 5, 6, 7, 8, 9,
        # Bing
        11,12,13,14,15,16,17,18,19,
        11,12,13,14,15,16,17,18,19,
        11,12,13,14,15,16,17,18,19,
        11,12,13,14,15,16,17,18,19,
        # Wan
        21,22,23,24,25,26,27,28,29,
        21,22,23,24,25,26,27,28,29,
        21,22,23,24,25,26,27,28,29,
        21,22,23,24,25,26,27,28,29,
        # Feng
        31,33,35,37,
        31,33,35,37,
        31,33,35,37,
        31,33,35,37,
        # Zhong, Fa, Bai
        41,43,45,
        41,43,45,
        41,43,45,
        41,43,45,
        # Hua
        51,53,55,57,59,61,63,65
    ]

    def __init__(self):
        pass

    def shuffle(self, size=136):
        # 124: Zhong Fa Bai -> Hua
        self.on = self.normalCards[0:size]
        random.shuffle(self.on)

    def pick(self, size=1):
        if len(self.on) < size:
            return None
        r = self.on[0:size]
        self.on = self.on[size:]
        return r

    def remain(self):
        return len(self.on)


class Player:
    # history:
    # [c1, c2, ..., c13]: initial state
    # (in, out)
    # (out, out, out): PENG, CHI, GANG
    # (in ,out, out, out, out): AnGANG
    def __init__(self, ai=False):
        self.history = []
        self.cursor = None
        self.state = None
        self.ai = ai

    def _compress(self, cards):
        cards.sort()
        r = []
        p = [0, 0] # CardID, CardNumber
        for c in cards:
            if p[0] != c:
                p = [c, 1]
                r.append(p)
            else:
                p[1] += 1
        return r

    def _decompress(self, cards):
        r = []
        for p in cards:
            if p[1] == 0:
                continue
            r.extend([p[0]] * p[1])
        return r

    def _find(self, cards, card):
        for p in cards:
            if p[0] == card:
                return p
        return None

    def number(self, card):
        p = self._find(self.cursor, card)
        if p is None:
            return 0
        return p[1]

    def init(self, deck):
        self.history = [deck.pick(13)]
        self.cursor = self._compress(self.history[0])
        self.state = ['wait']

    def canHu(self, card=None):
        cursor = []
        for t in self.cursor:
            cursor.append([t[0], t[1]])
        if card is not None:
            p = self._find(cursor, card)
            if p is None:
                p = [card, 1]
                cursor.append(p)
                cursor = self._decompress(cursor)
                cursor.sort()
                cursor = self._compress(cursor)
            else:
                p[1] += 1
        possibleHead = []
        pairCount = 0
        for p in cursor:
            if p[1] >= 2:
                pairCount += p[1] / 2
                possibleHead.append(p[0])
        if pairCount == 7:
            return True

        for c in possibleHead:
            seq = []
            for t in cursor:
                seq.append([t[0], t[1]])
            n = len(seq)
            p = self._find(seq, c)
            p[1] -= 2
            i = 0
            backtrack = None
            backtrackEnable = False
            while True:
                while i < n:
                    noShun = False
                    if seq[i][1] == 0:
                        i += 1
                        continue
                    if i+2 >= n:
                        noShun = True
                    elif seq[i][0] > 39:
                        noShun = True
                    elif seq[i+1][0]-1 != seq[i][0] or seq[i+1][1] == 0:
                        noShun = True
                    elif seq[i+2][0]-2 != seq[i][0] or seq[i+2][1] == 0:
                        noShun = True
                    if noShun:
                        if seq[i][1] >= 3:
                            seq[i][1] -= 3
                            continue
                        elif i+2 >= n:
                            break
                        else:
                            break
                    elif seq[i][1] >= 3:
                        if backtrackEnable:
                            seq[i][1] -= 3
                            backtrackEnable = False
                            backtrack = None
                            continue
                        else:
                            backtrack = [0, []]
                            backtrack[0] = i
                            for t in seq:
                                backtrack[1].append([t[0], t[1]])
                    seq[i][1] -= 1
                    seq[i+1][1] -= 1
                    seq[i+2][1] -= 1
                if i >= n:
                    return True
                if backtrack is None:
                    break
                else:
                    i = backtrack[0]
                    seq = backtrack[1]
                    backtrackEnable = True
        return False

    def canPeng(self, card):
        if self.state[0] == 'pick':
            return False
        for p in self.cursor:
            if p[1] >= 2 and p[0] == card:
                return True
        return False

    def canGang(self, card):
        if self.state[0] == 'pick':
            return False
        for p in self.cursor:
            if p[1] >= 3 and p[0] == card:
                return True
        return False

    def canGangPlus(self):
        if self.state[0] != 'pick':
            return []
        r = []
        for h in self.history:
            if len(h) != 3:
                continue
            elif h[0] != h[1]:
                continue
            elif self._find(self.cursor, h[0]) is not None:
                r.append(h[0])
        r.sort()
        return r

    def canAnGang(self):
        if self.state[0] != 'pick':
            return []
        r = []
        for p in self.cursor:
            if p[1] >= 4:
                r.append(p[0])
        return r

    def canChi(self, card):
        if self.state[0] == 'pick':
            return False
        r = []
        if card > 29:
            return r
        cardIDx = card % 10
        if cardIDx > 2:
            if (self._find(self.cursor, card-2) is not None
                    and self._find(self.cursor, card-1) is not None):
                r.append([card-2, card-1])
        if cardIDx > 1 and cardIDx < 9:
            if (self._find(self.cursor, card-1) is not None
                    and self._find(self.cursor, card+1) is not None):
                r.append([card-1, card+1])
        if cardIDx < 8:
            if (self._find(self.cursor, card+1) is not None
                    and self._find(self.cursor, card+2) is not None):
                r.append([card+1, card+2])
        return r

    def pick(self, deck):
        card = deck.pick()
        if card is None:
            return
        card = card[0]
        p = self._find(self.cursor, card)
        if p is None:
            p = [card, 0]
            self.cursor.append(p)
        p[1] += 1
        self.cursor = self._compress(self._decompress(self.cursor))
        self.state = ['pick', card]

    def kick(self, cards):
        for c in cards:
            p = self._find(self.cursor, c)
            p[1] -= 1
        self.cursor = self._compress(self._decompress(self.cursor))
        n = len(cards)
        self.state.extend(cards)
        self.history.append(self.state[1:])
        self.state = ['wait']

    def cards(self):
        return self.cursor


class SillyAgent:
    def peng(self, player, card):
        # if peng, should kick return [A, A, B]
        # A for peng, B for single kick
        if random.randint(0, 1) == 0:
            return None
        n = player.number(card)
        cards = player.cards()
        while True:
            ck = random.sample(cards, 1)[0][0]
            if ck != card or n > 2:
                break
        cks = [card, card, ck]
        player.kick(cks)
        return cks

    def chi(self, player, card, strategies):
        # if chi, should kick and return [A, B, C]
        # A,B for chi, C for single kick
        strategies.append(None)
        decision = random.sample(strategies, 1)[0]
        if decision is None:
            return None
        n1 = player.number(decision[0])
        n2 = player.number(decision[1])
        cards = player.cards()
        while True:
            ck = random.sample(cards, 1)[0][0]
            if ck != decision[0] and ck != decision[1]:
                break
            elif ck == decision[0] and n1 > 1:
                break
            elif ck == decision[1] and n2 > 1:
                break
        cks = [decision[0], decision[1], ck]
        player.kick(cks)
        return cks

    def gang(self, player, card):
        # if gang, should kick and return [A, A, A]
        # A for gang
        if random.randint(0, 1) == 0:
            return None
        n = player.number(card)
        cks = [card, card, card]
        player.kick(cks)
        return cks

    def gangplus(self, player, strategies):
        # if gangplus, should kick and return [A]
        # A for gangplus
        # A can be huable for the ohters
        strategies.append(None)
        r = random.sample(strategies, 1)[0]
        if r is None:
            return None
        cks = [r]
        player.kick(cks)
        return cks

    def angang(self, player, strategies):
        # if angang, should kick and return [A, A, A, A]
        # A for angang
        strategies.append(None)
        r = random.sample(strategies, 1)[0]
        if r is None:
            return None
        cks = [r] * 4
        player.kick(cks)
        return cks

    def kick(self, player):
        p = random.sample(player.cards(), 1)[0]
        player.kick([p[0]])
        return p[0]


class Game:
    def __init__(self, size=136, agent=SillyAgent()):
        self.deck = None
        self.players = None
        self.decksize = size
        self.agent = agent

    def printPlayer(self, player):
        print '  + HISTORY:'
        for h in player.history:
            print '  |-- %s' % str(h)
        print '  + HAND: %s' % str(player.cursor)

    def dump(self):
        for pl in self.players:
            self.printPlayer(pl)

    def init(self):
        self.players = [Player(), Player(), Player(), Player()]
        self.deck = Deck()
        self.deck.shuffle(size=self.decksize)
        for pl in self.players:
            pl.init(deck=self.deck)

    def setAI(self, b1, b2, b3, b4):
        self.players[0].ai = b1
        self.players[1].ai = b2
        self.players[2].ai = b3
        self.players[3].ai = b4
