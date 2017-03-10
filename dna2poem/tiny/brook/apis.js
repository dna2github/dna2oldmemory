var brook = require('./brook.js'),
    meb = new brook.MemoryEventBrook();

meb.eventCleanTimer(3600);

function get_ip (req) {
  var ipaddr = req.header('x-forwarded-for'); 
  if (ipaddr) ipAddress = ipaddr.split(',')[0];
  if (!ipaddr) ipaddr = req.connection.remoteAddress;
  return ipaddr;
}

function _apis (app) {
  app.get('/api/v1/test', function (req, rep) {
    rep.send('Hello ' + get_ip(req) +'!');
  });

  app.post('/api/v1/create', function (req, rep) {
    var eid = meb.eventCreate(
      req.body.name,
      get_ip(req)
    );
    rep.header("Content-Type", "application/json");
    rep.send(JSON.stringify({eid: eid}));
  });
  app.post('/api/v1/delete', function (req, rep) {
    var r = meb.eventDelete(
      req.body.eid,
      get_ip(req)
    );
    rep.header("Content-Type", "application/json");
    if (r) rep.send(JSON.stringify({status: 'done'}));
      else rep.send(JSON.stringify({status: 'fail'}));
  });
  app.post('/api/v1/listen', function (req, rep) {
    var r = meb.eventListen(
      req.body.eid,
      req.body.url
    );
    rep.header("Content-Type", "application/json");
    if (r) rep.send(JSON.stringify({status: 'done'}));
      else rep.send(JSON.stringify({status: 'fail'}));
  });
  app.post('/api/v1/contribute', function (req, rep) {
    var r = meb.eventContribute(
      req.body.eid,
      get_ip(req),
      req.body.ip
    );
    rep.header("Content-Type", "application/json");
    if (r) rep.send(JSON.stringify({status: 'done'}));
      else rep.send(JSON.stringify({status: 'fail'}));
  });
  app.post('/api/v1/leave', function (req, rep) {
    var r = meb.eventLeave(
      req.body.eid,
      get_ip(req),
      req.body.url,
      req.body.ip
    );
    rep.header("Content-Type", "application/json");
    if (r) rep.send(JSON.stringify({status: 'done'}));
      else rep.send(JSON.stringify({status: 'fail'}));
  });
  app.post('/api/v1/fire', function (req, rep) {
    var r = meb.eventFire(
      req.body.eid,
      get_ip(req),
      req.body.data,
      req.body.duration,
      req.body.klass,
      req.body.version
    );
    rep.header("Content-Type", "application/json");
    if (r) rep.send(JSON.stringify({status: 'done'}));
      else rep.send(JSON.stringify({status: 'fail'}));
  });
  app.post('/api/v1/query', function (req, rep) {
    var query = meb.eventQuery(
      req.body.eid,
      req.body.dt_a,
      req.body.dt_b,
      req.body.klasses
    );
    rep.header("Content-Type", "application/json");
    rep.send(JSON.stringify({eid: req.body.eid, query: query}));
  });
}

module.exports = _apis;
