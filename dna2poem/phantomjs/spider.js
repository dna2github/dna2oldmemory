// usage: phantomjs spider.js <url>

var system = require('system');

function text_walker(text, stop) {
  var cursor = 0, n = text.length, freq = {}, total = 0;
  if (!stop) {
    stop = [' ', '\t', '\n', '\r',
            '(', ')', '{', '}',
            '[', ']', '/', '?',
            '.', ',', '+', '-', '=', '_',
            '`', '~', '!', '@', '#', '$',
            '%', '^', '&', '*', '<', '>',
            '\\', ':', ';', '"', '\'', '|',
            '～', '｀', '！', '＃', '¥', '％',
            '…', '＊', '（', '）', '—', '－',
            '＝', '＋', '｜', '、', '］',
            '［', '｛', '｝', '“', '”',
            '‘', '’', '：', '；', '？', '／',
            '。', '，', '》', '《'];
  }
  return {
    has_next: function () {
      return cursor < n;
    },
    seek: function (position) {
      if (!position && position !== 0) return cursor;
      cursor = position;
      return cursor;
    },
    total: function () {
      return total;
    },
    freq: function(top, threshold) {
      var key, val, i, n;
      var result = []; // item={word,count}
      if (!top) top = Object.keys(freq).length;
      if (!threshold && threshold !== 0) threshold = 1;
      for (key in freq) {
        val = freq[key];
        if (val < threshold) continue;
        n = result.length;
        for (i = 0; i < n; i++) {
          if (val < result[i].count) break;
        }
        if (n < top) {
          result.splice(i, 0, {word:key, count:val});
        } else if (i > 0) {
          result.splice(i, 0, {word:key, count:val});
          result.splice(0, 1);
        }
      }
      return result.reverse();
    },
    next: function () {
      if (cursor >= n) return null;
      var word, ch, ch_code;
      word = '';
      while (cursor < n) {
        ch = text.charAt(cursor);
        ch_code = text.charCodeAt(cursor);
        cursor ++;
        if (stop.indexOf(ch) >= 0) break;
        if (ch_code > 255) {
          if (!!word) {
            ch = '';
            cursor --;
            break;
          } else {
            return {word: ch, stop: ''};
          }
        }
        word += ch;
      }
      return {word: word, stop: ch};
    },
    count: function (word) {
      total ++;
      if (word in freq) {
        freq[word]++;
      } else {
        freq[word] = 1;
      }
      return freq[word];
    }
  };
}

function word_streamer(text) {
  var walker = text_walker(text);
  var filter = ['the', 'and', 'a', 'an', 'is', 'are', 'was', 'were', 'can', 'will', 'shall', 'could', 'would', 'should', 'be'];
  var connector = ['.', '-', '+', '_'];
  var result = '';
  var word = '', stop = ' ', old_stop = null;
  while (walker.has_next()) {
    word = walker.next();
    stop = word.stop;
    word = word.word.toLowerCase();
    if (filter.indexOf(word) >= 0) continue;
    if (!!old_stop && !!word) {
      result += old_stop;
    } else if (!!word) {
      result += ' ';
    }
    old_stop = null;
    if (connector.indexOf(stop) >= 0) {
      old_stop = stop;
    }
    result += word;
  }
  return result;
}

function histogram(text) {
  var walker = text_walker(text);
  var filter = ['the', 'and', 'a', 'an', 'is', 'are', 'was', 'were', 'can', 'will', 'shall', 'could', 'would', 'should', 'be'];
  var word = ' ', stop = ' ';
  while (walker.has_next()) {
    word = walker.next();
    stop = word.stop;
    word = word.word.toLowerCase();
    if (filter.indexOf(word) >= 0) continue;
    if (word) walker.count(word);
  }
  return {
    total: walker.total(),
    top: walker.freq(50, 5)
  };
}

if ( typeof(phantom) !== "undefined" ) {
    var page = require('webpage').create();

    page.settings.loadImages = false;
    // page.customHeaders = {Authorization: 'Basic <base64 string>'};

    page.onConsoleMessage = function(msg) {
        console.log(msg);
    };
    
    page.onAlert = function(msg) {
        console.log(msg);
    };

    var base_url = system.args[1].split('/').slice(0, 3).join('/'); // 'https://en.wikipedia.org';
    var http_queue = [];

    function find_obj(list, key, value) {
      var i,n;
      n = list.length;
      for (i=0; i<n; i++) {
        if (list[i][key] === value) return {
          index: i,
          obj: list[i]
        };
      }
      return {
        index: -1,
        obj: null
      };
    }

    function cut_url(url) {
      if (url.indexOf(base_url) === 0) {
        return url.substring(base_url.length);
      }
      return url;
    }

    page.onResourceRequested = function(request) {
      http_queue.push({
        id: request.id,
        method: request.method,
        url: request.url,
        start: request.time
      });
    };

    page.onResourceReceived = function(response) {
      if (response.stage === 'end') {
        var r = find_obj(http_queue, response.id);
        if (r.index < 0) return;
        http_queue.splice(r.index, 1);
        r = r.obj;
        r.end = response.time;
        r.status = response.status;
        console.log('- ' + r.start + ' - ' + r.end  +'   <' + r.method + ': ' + r.status + '> ' + cut_url(r.url));
      }
    };

    page.viewportSize = {
      width: 1024,
      height: 768
    };

    page.open(system.args[1], function (status) {
        // XXX: when the web page is rendered in front end
        //      it will be a problem that calculating is
        //      too early. It should wait for all JavaScript
        //      loaded and calculated complete.
        var html = page.evaluate(function () {
            var a = document.body.getElementsByTagName('a');
            var links = [];
            for (var i = 0, n = a.length; i < n; i++) {
              links.push(a[i].href);
            }

            return {
              outer_html: document.body.outerHTML,
              text: document.body.outerText,
              links: links
            };
        });

        console.log('url: ', page.url);
        console.log('= frequencies ==========================================');
        var freqs = histogram(html.text);
        console.log('total:', freqs.total);
        for (var i = 0, n = freqs.top.length; i < n; i++) {
          console.log(freqs.top[i].word, freqs.top[i].count);
        }
        console.log('= word stream ==========================================');
        console.log(word_streamer(html.text));
        console.log('= links ================================================');
        for (var i = 0, n = html.links.length; i < n; i++) {
          console.log(html.links[i]);
        }
        phantom.exit();
    });
}
