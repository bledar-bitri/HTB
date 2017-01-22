// Copyright 2001-2005 Interakt Online. All rights reserved.

var di_UP = 38;
var di_DOWN = 40;
var di_LEFT = 37;
var di_RIGHT = 39;
var di_PgUP = 33;
var di_PgDOWN = 34;
var di_HOME = 36;
var di_END = 35;
var di_ENTER = 13;
var di_DELETE = 46;
var di_BACKSPACE = 8;
var di_TAB = 9;
var di_ESC = 27;
var KT_NAMESPACE_URI = 'http://www.interaktonline.com/MXWidgets';

function WDG_getAttributeNS(o, a) {
	if (is.opera) {
		if (o.getAttributeNS) {
			return o.getAttributeNS(KT_NAMESPACE_URI, a);
		} else {
			return "";
		}
	} else {
		return o.getAttribute('wdg:' + a);
	}
}

function WDG_setAttributeNS(o, a, v) {
	if (o.setAttributeNS) {
		o.setAttributeNS(KT_NAMESPACE_URI, a, v);
	} else {
		o.setAttribute('wdg:' + a, v);
	}
}

function MXWidgets_init() {
	is = new BrowserCheck();
	
	if (is.ie && is.mac) {
		return;
	}
	if (! (is.ie || is.opera || is.mozilla || is.safari)) { //could not recognize browser
		return;
	}

	var tmpwidget = {};
	if (typeof $MXW_relPath=="undefined") {
		$MXW_relPath = '';
	}
	if (!$MXW_relPath.match(/^\.\//)) {
		$MXW_relPath = "./" + $MXW_relPath;
	}

	if (typeof widgetized_els != 'undefined') {
		return;
	}
	widgetized_els = [];
	
	var widgets = document.getElementsByTagName("SELECT");
	for (var i=0; i<widgets.length; i++) {
		if (WDG_getAttributeNS(widgets[i], 'subtype')) {
			if (!widgets[i].getAttribute("id") || widgets[i].getAttribute("id") == '' || widgets[i].id == '') {
				widgets[i].id = newWidgetID();
			}
			if (Array_indexOf(widgetized_els, widgets[i].id) < 0) {
				Array_push(widgetized_els, widgets[i].id);
				var subtype = WDG_getAttributeNS(widgets[i], 'subtype').toString().replace(/^kt_/, '');
				var ss = 'tmpwidget = new MXW_'+ subtype +'("'+widgets[i].id+'")';
				eval(ss);
				tmpwidget.className = WDG_getAttributeNS(widgets[i], 'subtype');
			}
		}
	}

	var widgets = document.getElementsByTagName("INPUT");
	for (var i=0; i<widgets.length; i++) {
		if (WDG_getAttributeNS(widgets[i], 'subtype')) {
			if (!widgets[i].getAttribute("id")) {
				widgets[i].id = newWidgetID();
			}
			if (Array_indexOf(widgetized_els, widgets[i].id) < 0) {
				Array_push(widgetized_els, widgets[i].id);
				var subtype = WDG_getAttributeNS(widgets[i], 'subtype').toString().replace(/^kt_/, '');
				var ss = 'tmpwidget = new MXW_'+ subtype +'("'+widgets[i].id+'")';
				eval(ss);
				tmpwidget.className = WDG_getAttributeNS(widgets[i], 'subtype');
			}
		}
	}

	var widgets = document.getElementsByTagName("TEXTAREA");
	for (var i=0; i<widgets.length; i++) {
		if (WDG_getAttributeNS(widgets[i], 'subtype')) {
			if (!widgets[i].getAttribute("id")) {
				widgets[i].id = newWidgetID();
			}
			var subtype = WDG_getAttributeNS(widgets[i], 'subtype').toString().replace(/^kt_/, '');
			var ss = 'tmpwidget = new MXW_'+ subtype +'("'+widgets[i].id+'")';
			eval(ss);
			tmpwidget.className = WDG_getAttributeNS(widgets[i], 'subtype');
		}
	}
	if (typeof MXW_DynamicObject_simpleClose!="undefined" ) {
		//mark the focused element, and close the previous open editabletextfield or autocomplete
		utility.dom.attachEvent(document.body, "mousedown", function(){
			MXW_DynamicObject_simpleClose();
		}, 1);
	}
	MXWidgets_afterInit();
	Array_each(document.getElementsByTagName("form"), function(form) {
		form.removeAttribute('haschanged');
	});
}


function MXWidgets_afterInit() {
	if (typeof MXW_DynamicObject_updateFirst != 'undefined') {
		MXW_DynamicObject_updateFirst();
	}
}

function newWidgetID() {
	if (typeof window["widgetcount"]=="undefined") {
		window["widgetcount"] = 0;
		window["widgetdefaultidroot"] = "mxw_widget";
	}
	window["widgetcount"]++;
	while(document.getElementById(window["widgetdefaultidroot"]+window["widgetcount"])) {
		window["widgetcount"]++;
	}
	return window["widgetdefaultidroot"]+window["widgetcount"];
}

utility.dom.attachEvent_base(window, "load", MXWidgets_init, 1, true);

//configuration variables

$DDR_MAIN_CLASSNAME = 'DependentDropdown';
$DDR_DEPENDENT_OBJ = 'DependentDropdown';
$DDR_MASTERSELECT_OBJ = 'MasterSelect';
$DDR_DETAILSELECT_OBJ = 'DetailSelect';

if (typeof window[$DDR_MASTERSELECT_OBJ] == 'undefined') {
	window[$DDR_MASTERSELECT_OBJ] = {};
}
if (typeof window[$DDR_DEPENDENT_OBJ] == 'undefined') {
	window[$DDR_DEPENDENT_OBJ] = {};
}

function MXW_MasterSelect(select) {
	if (typeof window[$DDR_MASTERSELECT_OBJ][select.id] == 'undefined') {
		this.select = select;
		this.id = select.id;
		//_t("New MasterSelect:"+this.id +" is master.");
		var obj = this;
		window[$DDR_MASTERSELECT_OBJ][this.id] = this;
		var tmpo = select;
		utility.dom.attachEvent_base(tmpo, "change", function (e){MasterSelectChange(obj, e);}, 1, true);
		if (is.mozilla) {	
			//onchange not fired when navigating the list using the keyboard (up/down/pageuppagedown)
			//in IE onkeyup fires the onchange event
			utility.dom.attachEvent_base(tmpo, "keyup", function (e){MasterSelectChange(obj, e);}, 1, true);
		}
	} else {
		var obj = window[$DDR_MASTERSELECT_OBJ][select.id];
		//_t("MasterSelect:"+obj.id +" is master again.");
	}

	return obj;
}

function MXW_MasterSelect_change() {
	//_t('called change on master select:'+this.id);
	//MXW_DynamicObject_FireOnchange(this.id)
}
MXW_MasterSelect.prototype.change = MXW_MasterSelect_change;

function MasterSelectChange(obj, e) {
	//_t("The master has changed ["+obj.id+"]");
	//this triggers the javascript object pseudo-event
	window[$DDR_MASTERSELECT_OBJ][obj.id].change();
}

/*
* 
*/
$SPN_GLOBALOBJECT = "SpinnerObject";
if (typeof window[$SPN_GLOBALOBJECT] == 'undefined') {
	window[$SPN_GLOBALOBJECT] = {};
}

function MXW_Spin(obj, ticker, renderButtons) {
	this.renderButtons = renderButtons || false;
	this.obj = obj;
	this.height = this.obj.input.offsetHeight;
	this.ticker = ticker;
	if (this.renderButtons) {
		this.render();
	}
	return this;
}

function spin_start(obj, direction) {
	obj.spinStartTimeStamp = new Date();
	window[$SPN_GLOBALOBJECT]['active'] = obj;
	window[$SPN_GLOBALOBJECT]['active'].spinner.stopped = false;
	window[$SPN_GLOBALOBJECT]['timeout'] = window.setTimeout('spin_tick('+direction+')', 300);
}

function spin_stop(e) {
	var obj = window[$SPN_GLOBALOBJECT]['active'];
	if (obj) {
		obj.mousedown = false;
	}
	window.clearTimeout(window[$SPN_GLOBALOBJECT]['timeout']);
	window[$SPN_GLOBALOBJECT]['timeout'] = null;
}

function spin_tick(direction)  {
	var obj = window[$SPN_GLOBALOBJECT]['active'];

	var elapsed = (new Date()) - obj.spinStartTimeStamp;
	var step = obj.spinner.ticker[0][1];
	var timestep = obj.spinner.ticker[0][2];//ms

	for (var i = 1; i < obj.spinner.ticker.length; i++) {
		if (elapsed > obj.spinner.ticker[i][0]) {
			//use last tick setting, for the biggest elapsed time
			step = obj.spinner.ticker[i][1];
			timestep = obj.spinner.ticker[i][2];
		}
	}
	obj.spin(direction, step);
	//window.status = 'Changing: ' + elapsed + ', ' + (direction * step);
	window[$SPN_GLOBALOBJECT]['timeout'] = window.setTimeout('spin_tick('+direction+')', timestep);
}

function MXW_Spin_buttondown(i){
	document.getElementById(i).className = "MXW_Spin_div_down";
}
function MXW_Spin_buttonup(i) {
	document.getElementById(i).className = "MXW_Spin_div_up";
}

MXW_Spin.prototype.render = function() {
	//the widget is replaced by its container 
	//the class widget_container is used in validation
	this.container = utility.dom.createElement('span', {
		className:"widget_container"
	});
	this.container = utility.dom.insertAfter(this.container, this.obj.input);
	this.container.innerHTML = '<table class="MXW_Spin_table" border="0" cellpadding="0" cellspacing="0"><tr><td class="MXW_Spin_table_td"></td><td class="MXW_Spin_table_td"><div class="MXW_Spin"></div></td></tr></table>';

	this.obj.input.style.margin="-1px 0px";
	this.container.firstChild.rows[0].firstChild.appendChild(this.obj.input);
	this.div = this.container.firstChild.rows[0].lastChild.firstChild;

	// create the buttons
	if (this.height) {
		var tmp_height = Math.floor((this.height - 4) / 2); //padding
	} else {
		var tmp_height = Math.floor((20 - 4) / 2);
	}
	style_string = 'height: ' + tmp_height + 'px; line-height: ' + tmp_height + 'px';
	this.div.innerHTML = '<table border="0" cellspacing="0" cellpadding="0" class="MXW_Spin_table"><tr><td class="MXW_Spin_table_td MXW_Spin_td_btnup" style="' + style_string + '"><div id="'+this.obj.input.id+'_incbutton" class="MXW_Spin_div_up" style="' + style_string + '"></div></td></tr></table><table border="0" cellspacing="0" cellpadding="0" class="MXW_Spin_table"><tr><td class="MXW_Spin_td_btndown" style="' + style_string + '"><div id="'+this.obj.input.id+'_decbutton" class="MXW_Spin_div_up" style="' + style_string + '"></div></td></tr></table>';

	this.incbutton = this.div.firstChild.rows[0].firstChild;
	this.decbutton = this.div.lastChild.rows[0].lastChild;
	
	var spn = this;

	utility.dom.attachEvent_base(this.decbutton, "click", function (e){
		MXW_Spin_buttondown(spn.obj.input.id+'_decbutton'); 
		window.setTimeout("MXW_Spin_buttonup('"+spn.obj.input.id+"_decbutton')", 50);
		spn.obj.spin(-1);
	}, 1, false, false);

	utility.dom.attachEvent_base(this.decbutton, "dblclick", function (e){
		MXW_Spin_buttondown(spn.obj.input.id+'_decbutton'); window.setTimeout("MXW_Spin_buttonup('"+spn.obj.input.id+"_decbutton')", 50);
		spn.obj.spin(-1);
	}, 1, false, false);
	utility.dom.attachEvent_base(this.decbutton, "mousedown", function (e){
		utility.dom.stopEvent(e);
		spn.obj.mousedown = true;
		MXW_Spin_buttondown(spn.obj.input.id+'_decbutton');
		spin_start(spn.obj, -1);
	}, 1, false, false);
	utility.dom.attachEvent_base(this.decbutton, "mouseup", function (e){
		MXW_Spin_buttonup(spn.obj.input.id+'_decbutton');
		spin_stop(e);
	}, 1, false, false);
	utility.dom.attachEvent_base(this.decbutton, "mouseout", function (e){
		MXW_Spin_buttonup(spn.obj.input.id+'_decbutton');
		spin_stop(e);
	}, 1, false, false);
	this.decbutton.onselect = this.decbutton.onselectstart = this.decbutton.ondrag = this.decbutton.ondragstart = rf;


	utility.dom.attachEvent_base(this.incbutton, "click", function (e){
		MXW_Spin_buttondown(spn.obj.input.id+'_incbutton'); window.setTimeout("MXW_Spin_buttonup('"+spn.obj.input.id+"_incbutton')", 50);
		spn.obj.spin(1);
	}, 1, false, false);
	utility.dom.attachEvent(this.incbutton, "dblclick", function (e){
		MXW_Spin_buttondown(spn.obj.input.id+'_incbutton'); window.setTimeout("MXW_Spin_buttonup('"+spn.obj.input.id+"_incbutton')", 50);
		spn.obj.spin(1);
	}, 1, false, false);
	utility.dom.attachEvent_base(this.incbutton, "mousedown", function (e){
		utility.dom.stopEvent(e);
		spn.obj.mousedown = true;
		MXW_Spin_buttondown(spn.obj.input.id+'_incbutton');
		spin_start(spn.obj, 1);
	}, 1, false, false);
	utility.dom.attachEvent_base(this.incbutton, "mouseup", function (e){
		MXW_Spin_buttonup(spn.obj.input.id+'_incbutton');
		spin_stop(e);
	}, 1, false, false);
	utility.dom.attachEvent_base(this.incbutton, "mouseout", function (e){
		MXW_Spin_buttonup(spn.obj.input.id+'_incbutton');
		spin_stop(e);
	}, 1, false, false);
	this.incbutton.onselect = this.incbutton.onselectstart = this.incbutton.ondrag = this.incbutton.ondragstart = rf;

//	this.obj.input.style.margin = '0px';
	//this.div.style.margin = '0px';
}

function MXW_visualAlert(obj, toggle, prefix, x, y) {
	var classname =  window['$'+prefix+'_VISUAL_ALERT_INPUT_CLASSNAME']
	var divname = window['$'+prefix+'_DIVPREFIX'] + obj.name;
	var hasspinner = (typeof obj.spinner != 'undefined' && obj.spinner.renderButtons) ? 19 : 0;
	if (toggle) {
		utility.dom.classNameAdd(obj.input, classname);
		var helper = document.getElementById(divname);
		utility.dom.showElem(helper);

		var ppos = utility.dom.getAbsolutePos(obj.input);
		if (typeof x == 'undefined') {
			helper.style.left = (ppos.x + obj.input.offsetWidth + 1 + hasspinner) + "px";
		} else {
			helper.style.left = x + "px";
		}
		if (typeof y == 'undefined') {
			helper.style.top = (ppos.y + parseInt((obj.input.offsetHeight-helper.offsetHeight)/2, 10)) + "px";
		} else {
			helper.style.top = y + "px";
		}
	} else {
		utility.dom.classNameRemove(obj.input, classname);
		utility.dom.hideElem(document.getElementById(divname));
	}
}

function MXW_getSelectionStart(input) {
	if (input.setSelectionRange) {
		return input.selectionStart;
	} else if(input.createTextRange) {
		var selText = input.ownerDocument.selection.createRange().duplicate();
		return -selText.moveStart("character", -1000);
	}

}

function MXW_getSelectionEnd(input) {
	if (input.setSelectionRange) {
			return input.selectionEnd;
	} else if(input.createTextRange) {
		var selText = input.ownerDocument.selection.createRange().duplicate();
		selText.collapse(false);
		return -selText.moveStart("character", -1000);
	}
}

function MXW_setSelectionRange(input, selectionStart, selectionEnd) {
	if (input.setSelectionRange) {
		input.setSelectionRange(selectionStart, selectionEnd);
	} else if (input.createTextRange) {
		var range = input.createTextRange();
		range.collapse(true);
		range.moveStart("character", -1000);
		range.moveStart('character', selectionStart);
		range.moveEnd('character', selectionEnd - selectionStart);
		range.select();
	}
}

function rf(e) {
	return false;
}
function addDebugger() {
	if (document.getElementById("thedebugger")) {
		return;
	}
	var dbg = document.createElement("TEXTAREA");
	dbg.id="thedebugger";
	document.body.appendChild(dbg);
	dbg.style.width = "600px";
	dbg.style.height = "300px";
	dbg.style.fontSize = "11px";
	theDebuggerContainer = document.getElementById("thedebugger");
}
function _t(t) {
	//return;
	addDebugger();
	theDebuggerContainer.value += t + "\r\n";
	theDebuggerContainer.scrollTop = 100000;
}
function testclickhandler() {
	//_t("onclick handler original");
}

function testchangehandler() {
	//_t("onchange handler original");
}
