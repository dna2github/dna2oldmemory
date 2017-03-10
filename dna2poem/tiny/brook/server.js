var express = require('express'),
    bodyParser = require('body-parser'),
    app = express();

app.use(bodyParser.json());
app.use(express.static('static'));

require('./apis.js')(app);
require('./views.js')(app);

var server = app.listen(8080, '0.0.0.0', function () {
  var host = server.address().address;
  var port = server.address().port;

  console.log('Brook Server listening at http://%s:%s', host, port);
});
