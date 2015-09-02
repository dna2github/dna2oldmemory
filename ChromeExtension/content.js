// replace alert to avoid a pop up dialog blocking script execuion
var script = document.createElement('script');
script.type = 'text/javascript';
script.textContent = 'window.nativeAlert=window.alert;window.alert=function(msg){console.log(msg);};';
(document.head || document.documentElement).appendChild(script);
console.log(script);

var links = document.getElementsByTagName('a');
var buttons = document.getElementsByTagName('button');
var list = [];
var n,i,t;
n = links.length;
for(i=0;i<n;i++) {
  t = links[i].href;
  if (!t) continue;
  if (t.toLowerCase().indexOf('javascript:') >= 0) continue;
  list.push(t);
  t = null;
}

// message from popup
chrome.extension.onMessage.addListener(function(request, sender, response){
  if(request.type!=='dna2rpc') return;
  var event = document.createEvent('HTMLEvents');
  event.initEvent('click', true, true);
  buttons[request.index].dispatchEvent(event);
  event = null;
});

// message to background
chrome.runtime.sendMessage(
 {type:'dna2links',
  links: list,
  buttons: buttons.length});
