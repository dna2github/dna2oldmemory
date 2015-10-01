ROLE_MELIN = 9
ROLE_PASSY = 8
ROLE_GLANS = 5
ROLE_GCOMM = 2
ROLE_OBERN = -1
ROLE_BCOMM = -2
ROLE_BLANS = -5
ROLE_KILLR = -8
ROLE_MOGAN = -9
ROLE_MODEL = -10

# team: {major, minor, people, fail_condition}
# order: {table, index, role: {type, index}}
#    table: MELIN (1:good|MODEL, 0:bad)
#           PASSY (1:MELIN|MOGAN, 0:unkown)
#           GCOMM, GLANS, OBERN, BLANS (0:unkown)
#           KILLR, MOGAN, MODEL: (1:bad, 0:unknown)
def genTeam(major, minor, people, fail_condition):
    return {
        "major": major,
        "minor": minor,
        "people": people,
        "fail_condition": fail_condition
    }

def isGreyBad(role):
    return role == ROLE_BLANS

def isLonglyBad(role):
    return role == ROLE_OBERN

def isVisibleBad(role):
    return role in [ROLE_KILLR, ROLE_MOGAN, ROLE_BCOMM, ROLE_BLANS, ROLE_OBERN]

def isBlackBad(role):
    return role in [ROLE_KILLR, ROLE_MOGAN, ROLE_MODEL, ROLE_BCOMM]

def isFollowable(role):
    return role in [ROLE_MELIN, ROLE_MOGAN]

def genOrder(rolelist, index, selfindex):
    role = rolelist[selfindex]
    state = []
    for one in rolelist:
        if isBlackBad(role):
            if isBlackBad(one) or isGreyBad(one):
                state.append(1)
            else:
                state.append(0)
        elif role == ROLE_MELIN:
            if isVisibleBad(one):
                state.append(1)
            else:
                state.append(0)
        elif role == ROLE_PASSY:
            if isFollowable(one):
                state.append(1)
            else:
                state.append(0)
        else:
            state.append(0)
    return {
        "table": state,
        "index": index, # current leader
        "role": {
            "type": rolelist[selfindex],
            "index": selfindex
        }
    }

class Role(object):
    def vote(self, team, order): pass
    def task(self, team, order): pass
    def lead(self, team, order): pass

class HumanRole(Role):
    def dump(self, team, order):
        print "vvvvvvvvvvvvvvvvvvvvvvvvvvvvvv"
        print "team=", team, "  order=", order, "  role=", order["role"]
        print "^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^"

    def vote(self, team, order):
        self.dump(team, order)
        c = raw_input("^ vote (y/n): ")
        if c == "y": return True  # pro
        else:        return False # con

    def task(self, team, order):
        self.dump(team, order)
        c = raw_input("^ task (o/x): ")
        if c == 'o': return True  # pass
        else:        return False # fail

    def lead(self, team, order):
        self.dump(team, order)
        # at first team["people"] is a number, then it is a list
        c = raw_input("^ team %d-%d (%dP e.g. 0,1,2): " %
            (team["major"]+1, team["minor"]+1, team["people"])
        )
        people = [int(one) for one in c.split(',')]
        team["people"] = people
        return people

ROLE_STRATEGY = {
  "9": HumanRole(),
  "8": HumanRole(),
  "5": HumanRole(),
  "2": HumanRole(),
  "-1": HumanRole(),
  "-2": HumanRole(),
  "-5": HumanRole(),
  "-8": HumanRole(),
  "-9": HumanRole(),
  "-10": HumanRole()
}
