import random

from roles import *

class Config(object):
    def __init__(self, tasks, fails, roles):
        self.tasks = tasks
        self.fails = fails
        self.roles = roles

    def role(self, roles):
        self.roles = roles

DECK_CONFIG = {
  "5":  Config([2,3,2,3,3], [1,1,1,1,1], [9,8,2,-8,-9]),
  "6":  Config([2,3,4,3,4], [1,1,1,1,1], [9,8,2,2,-8,-9]),
  "7":  Config([2,3,3,4,4], [1,1,1,2,1], [9,8,2,2,-1,-8,-9]),
  "8":  Config([3,4,4,5,5], [1,1,1,2,1], [9,8,2,2,2,-2,-8,-9]),
  "9":  Config([3,4,4,5,5], [1,1,1,2,1], [9,8,2,2,2,2,-8,-9,-10]),
  "10": Config([3,4,4,5,5], [1,1,1,2,1], [9,8,2,2,2,2,-1,-8,-9,10]),
}

# string raw_input(string)

class Deck(object):
    def __init__(self, n_player, config=None):
        if config is None:
            self.config = DECK_CONFIG[str(n_player)]
        else:
            self.config = config

        random.shuffle(self.config.roles)
        self.major = 0
        self.task_fail = 0
        self.team_fail = 0
        self.vote_history = []
        self.task_history = []
        self.team_history = []
        self.i = 0
        self.n = len(self.config.roles)

    def play(self):
        rolelist = self.config.roles
        while (self.major < 5 and self.task_fail < 3 and self.team_fail < 5):
            if self.major - self.task_fail >= 3: break
            one = ROLE_STRATEGY[str(rolelist[self.i])]
            info_team = genTeam(
                self.major,
                self.team_fail,
                self.config.tasks[self.major],
                self.config.fails[self.major]
            )
            info_order = genOrder(rolelist, self.i, self.i)
            one.lead(info_team, info_order)
            votec = 0
            for j, role in enumerate(rolelist):
                jorder = genOrder(rolelist, self.i, j)
                voteone = ROLE_STRATEGY[str(rolelist[j])]
                if voteone.vote(info_team, jorder): votec += 1
            if votec > self.n/2:
                print '[pass] vote for task %d-%d' % (
                    self.major, self.team_fail)
                self.team_fail = 0
                taskc = 0
                for j, role in enumerate(info_team["people"]):
                    jorder = genOrder(rolelist, self.i, role)
                    taskone = one = ROLE_STRATEGY[str(rolelist[role])]
                    if not taskone.task(info_team, jorder): taskc += 1
                if taskc >= self.config.fails[self.major]:
                    print '[fail] task %d (%d/%d)' % (
                        self.major, taskc, len(info_team["people"]))
                    self.task_fail += 1
                else:
                    print '[pass] task %d' % (self.major)
                self.major += 1
            else:
                print '[fail] vote for task %d-%d' % (
                    self.major, self.team_fail)
                self.team_fail += 1
            self.i = (self.i + 1) % self.n
        print '========================'
        print 'roles: ', rolelist
        if self.task_fail < 3 and self.team_fail < 5:
            print 'tasks pass'
        else:
            print 'tasks fail'
