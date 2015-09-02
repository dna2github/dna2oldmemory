// match target url
function match(url){
  var host = "null";
  if(typeof url == "undefined" || null == url)
    url = window.location.href;
  var regex = /(.*)\:\/\/([^\/]*).*/;
  var match = url.match(regex);
  if(typeof match != "undefined" && null != match)
    host = match[1];
  return host;
}
function entry(tabId, changeInfo, tab) {
  if(match(tab.url).toLowerCase()=="file"){
    chrome.pageAction.show(tabId);
  }
};
chrome.tabs.onUpdated.addListener(entry);

// message from content
var pagedata = {};
chrome.runtime.onMessage.addListener(function(request, sender, response){
  if(request.type!=="dna2links") return;
  pagedata.links = request.links;
  pagedata.buttons = request.buttons;
});

