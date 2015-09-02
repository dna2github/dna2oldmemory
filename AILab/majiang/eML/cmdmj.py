#!/usr/bin/python

from eML.Object import *
import random

class GameOver:
    def __init__(self, message):
        self.message = message


def format_cards(player):
    return str(player._decompress(player.cursor))


def print_players(message, turn, player):
    if isinstance(message, list):
        for m in message:
            print m
    else:
        print message
    print '#%d) %s' % (turn, format_cards(player))


def check_hu(card, turn, game):
    r = []
    for j in [0, 1, 2, 3]:
        if turn == j:
            continue
        pln = game.players[j]
        if pln.canHu(card):
            r.append(j)
    if len(r) > 0:
        raise GameOver('Winner %s (Chonger: %d)' % (str(r), turn))


ai = random.sample([7, 11, 13, 14], 1)[0]
game = Game(size=124)
game.init()
game.setAI((ai & 8) > 0, (ai & 4) > 0, (ai & 2) > 0, (ai & 1) > 0)
game.dump()
try:
    n = None
    i = 0
    while True:
        pl = game.players[i]
        pl.pick(game.deck)
        if game.deck.remain() == 0:
            raise GameOver('No winner')
        if pl.ai:
            print_players(
                '-- debug: [ai %d] pick %d (remain=%d)' % (
                    i, pl.state[1], game.deck.remain()
                ), i, pl)
        else:
            print_players(
                '-- debug: [hm %d] pick %d (remain=%d)' % (
                    i, pl.state[1], game.deck.remain()
                ), i, pl)
        if pl.canHu():
            pl.history.append([pl.state[1]])
            raise GameOver('Winner #%d (Zimo)' % i)
        if pl.ai:
            st = pl.canAnGang()
            if len(st) > 0:
                r = game.agent.angang(pl, st)
                if r is not None:
                    print_players(
                        '-- debug: [ai %d] angang %d' % (i, r[0]),
                        i, pl)
                    continue
            st = pl.canGangPlus()
            if len(st) > 0:
                r = game.agent.gangplus(pl, st)
                if r is not None:
                    print_players(
                        '-- debug: [ai %d] gangplus %d' % (i, r[0]),
                        i, pl)
                    check_hu(r[0], i, game)
                    continue
            ck = game.agent.kick(pl)
            print_players(
                '-- debug: [ai %d] kick %d' % (i, ck),
                i, pl)
        else:
            r = pl.canAnGang()
            if len(r) > 0:
                print format_cards(pl)
                t = raw_input('> angang(%s) n/index > ' % str(r))
                if t != 'n':
                    ck = r[int(t)]
                    pl.kick([ck] * 4)
                    print_players(
                        '-- debug: [hm %d] angang %d' % (i, ck),
                        i, pl)
                    continue
            r = pl.canGangPlus()
            if len(r) > 0:
                print format_cards(pl)
                t = raw_input('> gangplus(%s) n/index > ' % str(r))
                if t != 'n':
                    ck = r[int(t)]
                    pl.kick([ck])
                    print_players(
                        '-- debug: [ai %d] gangplus %d' % (i, ck),
                        i, pl)
                    check_hu(ck, i, game)
                    continue
            print format_cards(pl)
            ck = int(raw_input('> kick > '))
            pl.kick([ck])
            print_players(
                '-- debug: [hm %d] kick %d' % (i, ck),
                i, pl)
        while True:
            mark = None
            check_hu(ck, i, game)
            for j in [0, 1, 2, 3]:
                if i == j:
                    continue
                pln = game.players[j]
                if pln.ai:
                    if pln.canGang(ck):
                        r = game.agent.gang(pln, ck)
                        if r is not None:
                            print_players(
                                '-- debug: [ai %d] gang %d' % (j, ck),
                                j, pln)
                            mark = 'gang'
                            i = j
                            break
                    if pln.canPeng(ck):
                        r = game.agent.peng(pln, ck)
                        if r is not None:
                            print_players([
                                '-- debug: [ai %d] peng %d' % (j, ck),
                                '-- debug: [ai %d] kick %d' % (j, r[2])
                                ], j, pln)
                            ck = r[2]
                            mark = 'peng'
                            i = j
                            break
                else:
                    if pln.canGang(ck):
                        print format_cards(pln)
                        t = raw_input('> gang(%d) n/y > ' % ck)
                        if t == 'y':
                            pln.kick([ck] * 3)
                            print_players(
                                '-- debug: [hm %d] gang %d' % (j, ck),
                                j, pln)
                            mark = 'gang'
                            i = j
                            break
                    if pln.canPeng(ck):
                        print format_cards(pln)
                        t = raw_input('> peng(%d) n/y > ' % ck)
                        if t == 'y':
                            tk = int(raw_input('> kick > '))
                            pln.kick([ck, ck, tk])
                            print_players([
                                '-- debug: [hm %d] peng %d' % (j, ck),
                                '-- debug: [hm %d] kick %d' % (j, tk)
                                ], j, pln)
                            ck = tk
                            mark = 'peng'
                            i = j
                            break
            if mark == 'gang': break
            if mark == 'peng': continue
            j = (i + 1) % 4
            pln = game.players[j]
            if pln.ai:
                st = pln.canChi(ck)
                if len(st) > 0:
                    r = game.agent.chi(pln, ck, st)
                    if r is not None:
                        print_players([
                            '-- debug: [ai %d] chi %d >- %d, %d' % (j, ck, r[0], r[1]),
                            '-- debug: [ai %d] kick %d' % (j, r[2])
                            ], j, pln)
                        ck = r[2]
                        mark = 'chi'
                        i = j
                        continue
            else:
                r = pln.canChi(ck)
                if len(r) > 0:
                    print format_cards(pln)
                    t = raw_input('> chi(%s) n/index > ' % str(r))
                    if t != 'n':
                        tk = int(raw_input('> kick > '))
                        t = int(t)
                        r = [r[t][0], r[t][1], tk]
                        print_players([
                            '-- debug: [hm %d] chi %d >- %d, %d' % (j, ck, r[0], r[1]),
                            '-- debug: [hm %d] kick %d' % (j, r[2])
                            ], j, pln)
                        pln.kick(r)
                        ck = tk
                        mark = 'chi'
                        i = j
                        continue
            i = (i+1)%4
            break
        
except GameOver as e:
    game.dump()
    print '==============================='
    print e.message
except:
    raise
