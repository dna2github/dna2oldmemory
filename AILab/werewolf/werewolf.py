import random

class Player(object):
  number = -1
  # number starts from 0
  role = None
  # S=seer, G=guard, W=witch, H=hunter, F=foolish, C=cupid, E=elder, B=bear
  # O=ordinary, L=littlegirl
  # K=werewolf, I=infectwolf, X=2ndwolf
  # T=Theif,
  # M=flutmusician
  uuid = None

  def __str__(self):
    return "Player %d:%s" % (self.number, self.role)

  def __repr__(self):
    return self.__str__()

class State(object):
  def __init__(self):
    self.reset()

  def __str__(self):
    return "%s" % str(self.__dict__)

  def __repr__(self):
    return self.__str__()

  def reset(self):
    self.saw = []
    self.died = []
    self.flut = []
    self.elder_health = 2
    self.kill = None
    self.kill_2nd = None
    self.guard = None
    self.infectMark = None
    self.infect = None
    self.rescueMark = None
    self.rescue = None
    self.poisonMark = None
    self.poison = None
    self.lovers = []
    self.last_2_cards = []
    self.bearroar = False


class Werewolf(object):
  def __init__(self):
    self.reset()

  def __str__(self):
    return "%s" % str(self.__dict__)

  def __repr__(self):
    return self.__str__()

  def set_players(self, players):
    self.player = players

  def set_roles(self, roles):
    self.role = roles

  def assign_roles(self):
    self.player = sorted(self.player, key=lambda x: x.number)
    random.shuffle(self.role)
    for i, player in enumerate(self.player):
      player.role = self.role[i]

  def reset(self):
    self.player = []
    self.role = []
    self.state = State()
    self.step = 'O' # O=not_start, P=prepare, N=night, D=daylight

  def signal_react(self): pass # beep for react

  def act_o2p(self):
    self.step = 'P'

  def act_n2d(self):
    killed = []
    if self.state.kill is not None:
      if self.state.kill.role == 'E':
        self.state.elder_health -= 1
      else:
        killed.append(self.state.kill)
    if self.state.kill_2nd is not None:
      if self.state.kill_2nd.role == 'E':
        self.state.elder_health -= 1
      else:
        killed.append(self.state.kill_2nd)
    if self.state.elder_health == 0:
      killed.append([player for player in self.player if player.role == 'E'][0])
      self.state.elder_health = -1
    if self.state.lovers[0] in killed or self.state.lovers[1] in killed:
      killed = list(set(killed) + set(self.state.lovers))
    if self.state.rescue == self.state.guard:
      killed = list(set(killed) + set([self.state.rescue]))
    self.state.died += killed
    self.step = 'D'

  def act_pd2n(self):
    self.state.kill = None
    self.state.kill_2nd = None
    self.state.infect = None
    self.state.rescue = None
    self.state.poison = None
    self.update_B()
    self.step = 'N'

  def select_display_T(self):
    return self.state.last_2_cards

  def act_T(self, card):
    cards = self.state.last_2_cards
    if self.state.last_2_cards[0] == card:
      t = cards[0]
      cards[1] = cards[0]
      cards[1] = t

  def select_CK(self):
    return list(set(self.player) - set(self.state.died))

  def display_C(self):
    return self.state.lovers

  def act_C(self, player_a, player_b):
    self.state.lovers = [player_a, player_b]

  def act_K(self, player):
    self.state.kill = player

  def select_S(self):
    return list(set(self.player) - set(self.state.saw))

  def display_S(self):
    return self.state.saw

  def act_S(self, player):
    self.state.saw.append(player)

  def select_M(self):
    return list(set(self.player) - set(self.state.died) - set(self.state.flut))

  def display_M(self):
    return self.state.flut

  def act_M(self, player_a, player_b):
    self.state.flut = list(set(self.state.flut) + set([player_a, player_b]))

  def select_G(self):
    return list(set(self.player) - set(self.state.died) - set([self.state.guard]))

  def display_G(self):
    return self.state.guard

  def act_G(self, player):
    self.state.guard = player

  def select_Wr(self):
    rescuable = []
    if self.state.rescueMark is not None:
      return rescuable
    if self.state.kill is not None:
      rescuable.append(self.state.kill)
    if self.state.kill_2nd is not None:
      rescuable.append(self.state.kill_2nd)
    return rescuable

  def select_Wp(self):
    if self.state.poisonMark is not None:
      return []
    return list(set(self.player) - set(self.state.died))

  def display_W(self):
    return (self.state.rescueMark, self.state.poisonMark)

  def act_W(self, rescue, poison):
    if rescue is not None and self.state.rescueMark is None:
      self.state.rescue = rescue
      self.state.rescueMark = rescue
      if self.state.kill == rescue:
        self.state.kill = None
      if self.state.kill_2nd == rescue:
        self.state.kill_2nd = None
    elif poison is not None and self.state.poisonMark is None:
      self.state.poison = poison
      self.state.poisonMark = poison

  def select_I(self):
    rescuable = []
    if self.state.infectMark is not None:
      return rescuable
    if self.state.kill is not None:
      rescuable.append(self.state.kill)
    if self.state.kill_2nd is not None:
      rescuable.append(self.state.kill_2nd)
    return rescuable

  def act_I(self, infect):
    if infect is not None and self.state.infectMark is not None:
      self.state.infect = infect
      self.state.infectMark = infect
      if self.state.kill == infect:
        self.state.kill = None
      if self.state.kill_2nd == infect:
        self.state.kill_2nd = infect

  def select_X(self):
    doable = True
    for player in self.state.died:
      if player.role == 'K':
        doable = False
        break
    if doable:
      alive = list(set(self.player) - set(self.state.died))
    else:
      alive = []
    return list(set(alive) - set([self.state.kill]))

  def act_X(self, player):
    self.state.kill_2nd = player

  def display_IXK(self):
    return [player for player in self.player if player.role in 'IXK']

  def display_B(self):
    return self.state.bearroar

  def update_B(self):
    alive = sorted(list(set(self.player) - set(self.state.died)), key=lambda x: x.number)
    bear = -1
    for i, player in enumerate(alive):
      if player.role == 'B':
        bear = i
        break
    if bear < 0:
      self.state.bearroar = False
    else:
      bearL = bear - 1
      bearR = bear + 1
      if bearR >= len(alive):
        bearR -= len(alve)
      self.state.bearroar = alive[bearL].role in 'IXK' or alive[bearR].role in 'IXK'

  def display_Lover(self, player):
    return player in self.state.lovers

  def display_H(self):
    if self.state.kill_2nd is not None and self.state.kill_2nd.role in 'H':
      return True
    if self.state.kill is not None and self.state.kill.role in 'H':
      return True
    return False

  def act_VoteH(self, players):
    self.state.died += players
