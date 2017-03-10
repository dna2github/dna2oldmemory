'use strict';

(function () {

// ajax wrapper
function _xhr_new() {
  // not support IE6-8
  // return new window.ActiveXObject('Microsoft.XMLHTTP');
  try {
    return new window.XMLHttpRequest();
  } catch (e) {}
}

function _api(uri, options) {
  if (!options) options = {};
  if (!options.data) options.data = null;
  if (!options.method) options.method = 'GET';
  if (!options.async && options.async !== false) options.async = true;
  if (!options.timeout && options.timeout !== 0) options.timeout = 4000;
  if (!options.headers) options.headers = {};
  if (!options.doneFn) options.doneFn = _nop;
  if (!options.failFn) options.failFn = _nop;
  var api_prefix = '/api/v1',
      api_xhr = _xhr_new();
  api_xhr.open(options.method, api_prefix + uri, options.async);
  api_xhr.addEventListener('readystatechange', function (_, isabort) {
    if (isabort || api_xhr.readyState === window.XMLHttpRequest.DONE ) {
      if (api_xhr.status >= 200 && api_xhr.status < 300) {
        var data = _convert_response(api_xhr);
        (options.doneFn || _nop)(data);
      } else {
        (options.failFn || _nop)(api_xhr, api_xhr.statusText);
      }
      api_xhr = null;
    }
  });
  api_xhr.setRequestHeader('Content-Type', 'application/json');
  for (var key in options.headers) {
    api_xhr.setRequestHeader(key, options.headers[key]);
  }
  if (options.data) {
    api_xhr.send(JSON.stringify(options.data));
  } else {
    api_xhr.send();
  }

  function _nop() {}
  function _convert_response(xhr) {
    var type = xhr.getResponseHeader('Content-Type');
    if (type.indexOf('application/json;') >= 0) {
      return JSON.parse(xhr.response);
    }
    return xhr.reponse;
  }
}

window.$brook = {
  _api: _api
};

})();
