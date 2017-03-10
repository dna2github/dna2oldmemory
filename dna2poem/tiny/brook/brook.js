var request = require('request');

function eventNotify(event, notifee) {
  request({
    method: 'POST',
    uri: notifee,
    'content-type': 'application/json',
    body: JSON.stringify(event)
  }, function (error, response, body) {
    if (Math.floor(response.statusCode / 100) == 2) {
      console.log('event fired');
    } else {
      console.log('error: ' + response.statusCode);
      console.log(body);
    }
  });
}

function eventNotifyAll(event, notifees) {
  console.log(event);
  for (var n = notifees.length, i = 0; i < n; i++) {
    console.log(notifees[i]);
    eventNotify(event, notifees[i]);
  }
}

function MemoryEventBrook() {
  this.brooks = []
}

MemoryEventBrook.prototype = {
  _lookupById: function (eid) {
    var n = this.brooks.length - 1;
    if (n > eid) n = eid;
    for (var index = n; index >= 0; index--) {
      if (this.brooks[index].id === eid) return [index, this.brooks[index]];
    }
    return [-1, null];
  },
  eventCreate: function (name, ip) {
    for (var i = 0, n = this.brooks.length; i < n; i++) {
      if (this.brooks[i].name === name) return null;
    }
    var new_id = 0;
    if (this.brooks.length > 0) {
      new_id = this.brooks[this.brooks.length - 1].id;
    }
    var cur = {
      id: new_id,
      name: name,
      contributors: [ip],
      listeners: [],
      events: []
    }
    this.brooks.push(cur);
    return new_id;
  },
  isContributor: function (eid, ip) {
    var cur = this._lookupById(eid),
        index = cur[0];
    if (index < 0) return false;
    var cur = cur[1];
    return (cur.contributors.indexOf(ip) >= 0);
  },
  eventDelete: function (eid, actip) {
    var cur = this._lookupById(eid),
        index = cur[0];
    if (index < 0) return false;
    return this.brooks.splice(index, 1)[0];
  },
  eventListen: function (eid, url) {
    var cur = this._lookupById(eid),
        index = cur[0];
    if (index < 0) return false;
    cur = cur[1];
    for (var i = 0, n = cur.listeners.length; i < n; i++) {
      if (cur.listeners[i] === url) return true;
    }
    cur.listeners.push(url);
    return true;
  },
  eventContribute: function (eid, actip, ip) {
    if (!this.isContributor(eid, actip)) return false;
    var cur = this._lookupById(eid),
        index = cur[0];
    if (index < 0) return false;
    // TODO check operator ip
    cur = cur[1];
    for (var i = 0, n = cur.contributors.length; i < n; i++) {
      if (cur.contributors[i] === ip) return true;
    }
    cur.contributors.push(url);
    return true;
  },
  eventLeave: function (eid, actip, url, ip) {
    var cur = this._lookupById(eid),
        index = cur[0];
    if (index < 0) return false;
    cur = cur[1];
    if (url) {
      for (var i = 0, n = cur.listeners; i < n; i++) {
        if (cur.listeners[i] === url)
          return !!cur.listeners.splice(i, 1)[0];
      }
    } else if (ip) {
      if (!this.isContributor(eid, actip)) return false;
      for (var i = 0, n = cur.contributors; i < n; i++) {
        if (cur.contributors[i] === ip)
          return !!cur.contributors.splice(i, 1)[0];
      }
    }
    return false;
  },
  eventFire: function (eid, actip, data, duration, klass, version) {
    if (!this.isContributor(eid, actip)) return false;
    if (!duration) duration = 86400;
    if (!klass) klass = null;
    if (!version) version = 1;
    var cur = this._lookupById(eid),
        index = cur[0];
    if (index < 0) return false;
    cur = cur[1];
    var timestamp = new Date().getTime() / 1000,
        timedead = timestamp + duration;
    var new_event = {
      timestamp: timestamp,
      timedead: timedead,
      version: version,
      klass: klass,
      data: data
    };
    cur.events.push(new_event);
    setTimeout(function () {
      eventNotifyAll(new_event, cur.listeners);
    }, 0);
    return true;
  },
  eventQuery: function (eid, dt_a, dt_b, klasses) {
    if (!dt_a) dt_a = -86400;
    if (!dt_b) dt_b = 0;
    if (!klasses) klasses = null;
    var cur = this._lookupById(eid),
        index = cur[0];
    if (index < 0) return null;
    cur = cur[1];
    var timestamp = new Date().getTime() / 1000,
        filtered = [],
        buf = null;
    for (var i = 0, n = cur.events.length; i < n; i++) {
      if (cur.events[i].timedead < timestamp) continue;
      filtered.push(cur.events[i]);
    }
    buf = filtered;
    filtered = [];
    if (klasses) {
      for (var i = 0, n = buf.length; i < n; i++) {
        if (klasses.indexOf(buf[i].klass) < 0) continue;
        fitlered.push(buf[i]);
      }
      buf = filtered;
      filtered = [];
    }
    dt_a += timestamp;
    dt_b += timestamp;
    for (var i = 0, n = buf.length; i < n; i++) {
      if (buf[i].timestamp <= dt_a || buf[i].timestamp > dt_b) continue;
      filtered.push(buf[i]);
    }
    return filtered;
  },
  eventCleanTimer: function (interval) {
    var that = this,
        timer_id = setInterval(_clean, interval * 1000);
    function _clean() {
      timestamp = new Date().getTime();
      for (var n = that.brooks.length, i = 0; i < n; i++) {
        var filtered = [],
            cur = that.brooks[i];
        for (var m = cur.events.length, j = 0; j < m; j++) {
          if (cur.events[j].timedead >= timestamp) filtered.push(cur.events[j]);
        }
        if (cur.events.length !== filtered.length) {
          cur.events = filtered;
        }
      }
    }
  }
};

var _brook = {
  MemoryEventBrook: MemoryEventBrook
};
module.exports = _brook;
