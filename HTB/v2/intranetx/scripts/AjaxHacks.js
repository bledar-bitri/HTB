Sys.Browser.WebKit = {}; //Safari 3 is considered WebKit
Sys.Browser.Chrome = {};

if (navigator.userAgent.indexOf(' Chrome/') > -1) {
    Sys.Browser.agent = Sys.Browser.Chrome;
    Sys.Browser.version = parseFloat(navigator.userAgent.match(/ Chrome\/(\d+\.\d+)/)[1]);
    Sys.Browser.name = 'Chrome';
    Sys.Browser.hasDebuggerStatement = true;
}
else if (navigator.userAgent.indexOf('WebKit/') > -1) {
    Sys.Browser.agent = Sys.Browser.WebKit;
    Sys.Browser.version = parseFloat(navigator.userAgent.match(/WebKit\/(\d+(\.\d+)?)/)[1]);
    Sys.Browser.name = 'WebKit';
}