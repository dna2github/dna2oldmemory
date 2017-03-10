function generateHtml(title, contents, scripts, csses) {
  if (!contents) contents = '';
  if (!csses) csses = [];
  if (!scripts) scripts = [];
  for (var n = scripts.length, i = 0; i < n; i++) {
    scripts[i] = '<script type="text/javascript" src="' + scripts[i] + '"></script>';
  }
  for (var n = csses.length, i = 0; i < n; i++) {
    csses[i] = '<link rel="stylesheet" type="text/css" href="' + csses[i] + '"/>';
  }
  return (
    '<html><head>'+
      '<meta name="viewport" content="width=device-width,initial-scale=1.0,' +
                 'maximum-scale=1.0,minimum-scale=1.0,user-scalable=no" />' +
      '<title>' + title +'</title>' +
      csses.join('') +
    '</head><body>' +
      contents +
      scripts.join('') +
    '</body></html>'
  );
}

function _views (app) {
  app.get('/brook/:eid', function (req, rep) {
    rep.send(generateHtml(
      'Event Brook',
      'Welcome to Event Brook #' + req.params.eid ,
      ['/js/brook.js']));
  });
}

module.exports = _views;
