document.addEventListener('DOMContentLoaded', function () {
  var data = chrome.extension.getBackgroundPage().pagedata;
  var dom = document.getElementById('result');
  var html = '';
  var i,n;
  n = data.links.length;
  for(i=0;i<n;i++) {
    html += '<div>' + data.links[i] + '</div>';
  }
  n = data.buttons;
  for(i=0;i<n;i++) {
    html += '<button mindex="'+i+'">Fire Click on Button '+i+'</button>';
  }
  dom.innerHTML = html;
  html = null;
  dom = null;
  data = null;
  var buttons = document.getElementsByTagName('button');
  n = buttons.length;
  for(i=0;i<n;i++) {
    Object.defineProperty(
      buttons[i],
      'mmindex',
      {value: i, writable: false, enumerable: false, configurable: true}
    );
    buttons[i].addEventListener('click', trigger);
  }
  buttons = null;

  function trigger() {
    var index = this.mmindex;
    // message to content
    chrome.tabs.query({active: true, currentWindow: true}, function(tabs){
      chrome.tabs.sendMessage(
        tabs[0].id,
        {type:'dna2rpc',index:index},
        function(response) {});  
    });
  }
});

