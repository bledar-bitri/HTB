var x14f1aa424a2 = new Array();
function xf05eb1aeb1e(oldHandler,newHandler) {
var xffd29459256 = function () {
newHandler();
if (oldHandler) {
oldHandler();
}
}
return xffd29459256;
}
window.onload = xf05eb1aeb1e(window.onload,x8f8930702ae);

function initMenu(x3f6526655d4, xeeb76377cf4) {
var xcaf7787f51d = document.getElementById(x3f6526655d4);
xcaf7787f51d.xeeb76377cf4 = xeeb76377cf4;
x14f1aa424a2[x3f6526655d4] = 1;
}
function xa71542744f1(x3f6526655d4, defaultValue, xeeb76377cf4) {
var xf29b2a53bb1;
var xcaf7787f51d = document.getElementById(x3f6526655d4);
var x2fc1f097c18 = document.getElementById(xeeb76377cf4);
var x42845217b47 = eval('dddefaults_' + x3f6526655d4);
var x23f89cb425c = eval('ddfks_' + x3f6526655d4);
var x0622d73b6f3 = eval('ddnames_' + x3f6526655d4);


if (!x23f89cb425c) {
return;
}

var x935d2cf4fb5 = null;
if (document.createElement) {
x935d2cf4fb5 = document.createElement('div');
if (x935d2cf4fb5.innerText != "") {
x935d2cf4fb5 = null;
}
}


if (defaultValue) {

for (xf29b2a53bb1=0;xf29b2a53bb1<xcaf7787f51d.options.length;xf29b2a53bb1++) {
if (xcaf7787f51d.options[xf29b2a53bb1].value == defaultValue) {
xcaf7787f51d.selectedIndex = xf29b2a53bb1;
return;
}
}

while(xcaf7787f51d.options.length) {
xcaf7787f51d.options[0] = null;
}


var xb953958796a = x23f89cb425c[defaultValue];
if (!xb953958796a) {

return;
}


for(xf29b2a53bb1 in x42845217b47) {
if (x935d2cf4fb5) {
x935d2cf4fb5.innerHTML = x42845217b47[xf29b2a53bb1];
var xe5cc5fa95cc = new Option(x935d2cf4fb5.innerText, xf29b2a53bb1);
} else {
var xe5cc5fa95cc = new Option(x42845217b47[xf29b2a53bb1], xf29b2a53bb1);
}
xcaf7787f51d.options[xcaf7787f51d.options.length]=xe5cc5fa95cc;
}


for(xf29b2a53bb1 in x23f89cb425c) {
if (x23f89cb425c[xf29b2a53bb1] == xb953958796a) {
if (x935d2cf4fb5) {
x935d2cf4fb5.innerHTML = x0622d73b6f3[xf29b2a53bb1];
var xe5cc5fa95cc = new Option(x935d2cf4fb5.innerText, xf29b2a53bb1);
} else {
var xe5cc5fa95cc = new Option(x0622d73b6f3[xf29b2a53bb1], xf29b2a53bb1);
}
xcaf7787f51d.options[xcaf7787f51d.options.length]=xe5cc5fa95cc;
if (defaultValue == xf29b2a53bb1) {
xcaf7787f51d.selectedIndex = xcaf7787f51d.options.length-1;
}
}
}


var x8ce3863cdbf = x2fc1f097c18.xeeb76377cf4;
if(x8ce3863cdbf) {

xa71542744f1(xeeb76377cf4, xb953958796a, x8ce3863cdbf);
} else {

for (xf29b2a53bb1=0;xf29b2a53bb1<x2fc1f097c18.options.length;xf29b2a53bb1++) {
if (x2fc1f097c18.options[xf29b2a53bb1].value == xb953958796a) {
x2fc1f097c18.selectedIndex = xf29b2a53bb1;
return;
}
}
}
} else {
if (x2fc1f097c18.options.length > 0) {
if (x2fc1f097c18.selectedIndex == -1) {
x2fc1f097c18.selectedIndex = 0;
}
xb953958796a = x2fc1f097c18.options[x2fc1f097c18.selectedIndex].value;

while(xcaf7787f51d.options.length) {
xcaf7787f51d.options[0] = null;
}

for(xf29b2a53bb1 in x42845217b47) {
if (x935d2cf4fb5) {
x935d2cf4fb5.innerHTML = x42845217b47[xf29b2a53bb1];
var xe5cc5fa95cc = new Option(x935d2cf4fb5.innerText, xf29b2a53bb1);
} else {
var xe5cc5fa95cc = new Option(x42845217b47[xf29b2a53bb1], xf29b2a53bb1);
}
xcaf7787f51d.options[xcaf7787f51d.options.length]=xe5cc5fa95cc;
}


for(xf29b2a53bb1 in x23f89cb425c) {
if (x23f89cb425c[xf29b2a53bb1] == xb953958796a) {
if (x935d2cf4fb5) {
x935d2cf4fb5.innerHTML = x0622d73b6f3[xf29b2a53bb1];
var xe5cc5fa95cc = new Option(x935d2cf4fb5.innerText, xf29b2a53bb1);
} else {
var xe5cc5fa95cc = new Option(x0622d73b6f3[xf29b2a53bb1], xf29b2a53bb1);
}
xcaf7787f51d.options[xcaf7787f51d.options.length]=xe5cc5fa95cc;
if (defaultValue == xf29b2a53bb1) {
xcaf7787f51d.selectedIndex = xcaf7787f51d.options.length-1;
}
}
}
}
}
}
function xe7223c9d46e() {
var xeeb76377cf4 = this.name;
var x2fc1f097c18 = document.getElementById(xeeb76377cf4);
var xf29b2a53bb1;

var x935d2cf4fb5 = null;
if (document.createElement) {
x935d2cf4fb5 = document.createElement('div');
if (x935d2cf4fb5.innerText != "") {
x935d2cf4fb5 = null;
}
}
for (xf29b2a53bb1 in this.x3f6526655d4) {
var x3f6526655d4 = this.x3f6526655d4[xf29b2a53bb1];

var xcaf7787f51d = document.getElementById(x3f6526655d4);

var x42845217b47 = eval('dddefaults_' + x3f6526655d4);
var x23f89cb425c = eval('ddfks_' + x3f6526655d4);
var x0622d73b6f3 = eval('ddnames_' + x3f6526655d4);

if (xcaf7787f51d.options.length != 0) {
var xa32f29f6708 =xcaf7787f51d[xcaf7787f51d.options.length-1].value;
xa32f29f6708 = x23f89cb425c[xa32f29f6708];
if (x2fc1f097c18.selectedIndex != -1 && xa32f29f6708 == x2fc1f097c18.options[x2fc1f097c18.selectedIndex].value) {
return;
}
}

while(xcaf7787f51d.options.length) {
xcaf7787f51d.options[0] = null;
}

if(x2fc1f097c18.options.length == 0) {

continue;
}


for(xf29b2a53bb1 in x42845217b47) {
if (x935d2cf4fb5) {
x935d2cf4fb5.innerHTML = x42845217b47[xf29b2a53bb1];
var xe5cc5fa95cc = new Option(x935d2cf4fb5.innerText, xf29b2a53bb1);
} else {
var xe5cc5fa95cc = new Option(x42845217b47[xf29b2a53bb1], xf29b2a53bb1);
}
xcaf7787f51d.options[xcaf7787f51d.options.length]=xe5cc5fa95cc;
}


for(xf29b2a53bb1 in x23f89cb425c) {
if (x2fc1f097c18.selectedIndex!=-1 && x23f89cb425c[xf29b2a53bb1] == x2fc1f097c18.options[x2fc1f097c18.selectedIndex].value) {
if (x935d2cf4fb5) {
x935d2cf4fb5.innerHTML = x0622d73b6f3[xf29b2a53bb1];
var xe5cc5fa95cc = new Option(x935d2cf4fb5.innerText, xf29b2a53bb1);
} else {
var xe5cc5fa95cc = new Option(x0622d73b6f3[xf29b2a53bb1], xf29b2a53bb1);
}
xcaf7787f51d.options[xcaf7787f51d.options.length]=xe5cc5fa95cc;
}
}

if (xcaf7787f51d.onchange) {

xcaf7787f51d.onchange();
} else if (xcaf7787f51d.fireEvent) {

xcaf7787f51d.fireEvent('onchange');
} else {
var event = document.createEvent("MouseEvents");
event.initMouseEvent("mouseup", 1, 1, window, 1, 10, 50, 10, 50, 0, 0, 0, 0, 1, xcaf7787f51d);
xcaf7787f51d.dispatchEvent(event);
}
}
}
function x74b4f29aa4a() {
for(xf29b2a53bb1 in x14f1aa424a2) {
if(x14f1aa424a2[xf29b2a53bb1] == 1) {
return xf29b2a53bb1;
}
}
return false;
}
function x8f8930702ae() {

var xb034cc73c67 = new Array();
var x8b9b819fd94 = new Array();
for (xf29b2a53bb1 in x14f1aa424a2) {
x8b9b819fd94[xf29b2a53bb1] = x14f1aa424a2[xf29b2a53bb1];
}
for(xf29b2a53bb1 in x8b9b819fd94) {

var xbf90a4329e9 = document.getElementById(xf29b2a53bb1);
var xeeb76377cf4 = xbf90a4329e9.xeeb76377cf4;
x56152088e06(xf29b2a53bb1, xeeb76377cf4);

var x68125d63f67 = eval('dddefval_' + xf29b2a53bb1);
if(x68125d63f67) {
xb034cc73c67[xf29b2a53bb1] = x68125d63f67;

var x98980ae6293 = document.getElementById(xf29b2a53bb1);
while(x98980ae6293.xeeb76377cf4) {
x14f1aa424a2[x98980ae6293.xeeb76377cf4] = 0;
x98980ae6293 = document.getElementById(x98980ae6293.xeeb76377cf4);
}
}
}


for(xf29b2a53bb1 in xb034cc73c67) {
if(x14f1aa424a2[xf29b2a53bb1] == 1) {
var x7d2b2ce7a78 = document.getElementById(xf29b2a53bb1);
x14f1aa424a2[xf29b2a53bb1] = 0;
xa71542744f1(xf29b2a53bb1, xb034cc73c67[xf29b2a53bb1], x7d2b2ce7a78.xeeb76377cf4);
}
}


var x8f796d6a15f = x74b4f29aa4a();
while(x8f796d6a15f) {

var x98980ae6293 = document.getElementById(x8f796d6a15f);
while(x98980ae6293.xeeb76377cf4) {
x98980ae6293 = document.getElementById(x98980ae6293.xeeb76377cf4);
}

xa733e93b9c8(x98980ae6293);
x8f796d6a15f = x74b4f29aa4a();
}
}
function xa733e93b9c8(parentO) {
if (parentO.x3f6526655d4) {
for (xf29b2a53bb1 in parentO.x3f6526655d4) {
x3f6526655d4 = parentO.x3f6526655d4[xf29b2a53bb1];
var xcaf7787f51d = document.getElementById(x3f6526655d4);
if(x14f1aa424a2[x3f6526655d4] == 1) {
xa71542744f1(x3f6526655d4, "", parentO.id);
x14f1aa424a2[x3f6526655d4] = 0;
}
xa733e93b9c8(xcaf7787f51d);
}
}
}
function x56152088e06(x3f6526655d4, xeeb76377cf4) {
document.getElementById(xeeb76377cf4).onchange = xe7223c9d46e;
document.getElementById(xeeb76377cf4).onkeyup = xe7223c9d46e;
document.getElementById(xeeb76377cf4).onmouseup = xe7223c9d46e;

var x2fc1f097c18 = document.getElementById(xeeb76377cf4);
if (!x2fc1f097c18.x3f6526655d4){
x2fc1f097c18.x3f6526655d4 = new Array();
}
x2fc1f097c18.x3f6526655d4[x2fc1f097c18.x3f6526655d4.length] = x3f6526655d4;

}
