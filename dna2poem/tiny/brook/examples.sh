#!/bin/bash

npm install
node server.js > /dev/null 2>&1 &

cat > _listener.js <<EOF
var express = require('express'),
    bodyParser = require('body-parser'),
    app = express();

app.use(bodyParser.json());
app.post('/', function (req, rep) {
  console.log(req.body);
  rep.send('');
});

app.listen(9090, function () {});
EOF
node _listener.js > /dev/null 2>&1 &

curl -XPOST -H 'Content-Type: application/json' -d '{"name":"test"}' "http://127.0.0.1:8080/api/v1/create"
curl -XPOST -H 'Content-Type: application/json' -d '{"eid":0, "url":"http://127.0.0.1:9090/"}' "http://127.0.0.1:8080/api/v1/listen"
curl -XPOST -H 'Content-Type: application/json' -d '{"eid":0, "data":"hello world"}' "http://127.0.0.1:8080/api/v1/fire"

curl -XPOST -H 'Content-Type: application/json' -d '{"eid":0}' "http://127.0.0.1:8080/api/v1/query"
