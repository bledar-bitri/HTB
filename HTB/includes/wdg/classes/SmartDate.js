// Copyright 2001-2005 Interakt Online. All rights reserved.

$SMD_MAIN_CLASSNAME = 'SmartDate';
$SMD_DIVPREFIX = 'smd_explanation_div_';
$SMDCAL_DIVPREFIX = 'smd_explanation_div_';

$SMD_GLOBALOBJECT = "SmartDates";
window[$SMD_GLOBALOBJECT] = [];

$SMD_TICK_INCREMENT = [
	[0, 1, 60]
];

$SMD_VISUAL_ALERT_DIV_CLASSNAME = 'MXW_SMD_visual_alert_div';
$SMDCAL_VISUAL_ALERT_DIV_CLASSNAME = 'MXW_SMDCAL_visual_alert_div';
$SMD_VISUAL_ALERT_INPUT_CLASSNAME = 'MXW_SMD_visual_alert_input';

$SMD_DATETIME_MASK_SEPARATORS = ['-', '/', '[', ']', '(', ')', '*', '+', '.', '\s', ':'];
$SMD_DATETIME_MASK_REGEXP = '[';
for(var i=0;i<$SMD_DATETIME_MASK_SEPARATORS.length; i++) {
	$SMD_DATETIME_MASK_REGEXP += "\\"+$SMD_DATETIME_MASK_SEPARATORS[i]+'|';
}
$SMD_DATETIME_MASK_REGEXP += ']';
$SMD_DATETIME_MASK_REGEXP = new RegExp($SMD_DATETIME_MASK_REGEXP,"g");

function SMD_date2regexp(txt, hold) {
	if (typeof hold == 'undefined') {
		hold = false;
	}
	txt = txt.replace(/[\/\-\.:]/g, 'DATESEPARATOR');
	txt = txt.replace(/([-\/\[\]()\*\+\.:])/g, '\\$1');
	if (hold) {
		txt = txt.replace(/DATESEPARATOR/g, '([\\/\\-\\.:])');
		txt = txt.replace(/(\\s)/g, '(\s)');
		txt = txt.replace(/ /g, '( )');
	} else {
		txt = txt.replace(/DATESEPARATOR/g, '[\\/\\-\\.:]');
		txt = txt.replace(/(\\s)/g, '\s');
	}
	txt = txt.replace(/yyyy/gi, '([0-9]{1,4})');
	txt = txt.replace(/yy/gi, '([0-9]{1,4})');
	txt = txt.replace(/y/gi, '([0-9]{1,4})');

	txt = txt.replace(/mm/ig, '([0-9]{1,2})');
	txt = txt.replace(/m/ig, '([0-9]{1,2})');

	txt = txt.replace(/dd/ig, '([0-9]{1,2})');
	txt = txt.replace(/d/ig, '([0-9]{1,2})');

	txt = txt.replace(/HH/ig, '([0-9]{1,2})');
	txt = txt.replace(/H/ig, '([0-9]{1,2})');

	txt = txt.replace(/hh/ig, '([0-9]{1,2})');
	txt = txt.replace(/h/ig, '([0-9]{1,2})');

	txt = txt.replace(/ii/ig, '([0-9]{1,2})');
	txt = txt.replace(/i/ig, '([0-9]{1,2})');

	txt = txt.replace(/ss/ig, '([0-9]{1,2})');
	txt = txt.replace(/s/ig, '([0-9]{1,2})');

	txt = txt.replace(/tt/ig, '(AM|PM|aM|Am|am|pM|Pm|pm)');
	txt = txt.replace(/t/ig, '(A|P|a|p)');

	var re = new RegExp('^' + txt + '$');
	return re;
}

function MXW_SmartDate(input, hasCalendar) {
	if (is.safari && is.version < 1.4) {
		return;
	}
	if (typeof hasCalendar == 'undefined') {
		this.hasCalendar = false;
	} else {
		this.hasCalendar = true;
	}
	this.name = input;
	this.input = document.getElementById(input);

	this.savedCSSStyle = this.input.style.cssText;
	if(this.savedCSSStyle == "{}") {
		//IEMac5.2 bug
		this.savedCSSStyle = "";
	}

	this.input.disableAutocomplete = true;
	this.input.setAttribute('disableAutocomplete', true);
	this.input.autocomplete = "off";
	this.input.setAttribute('autocomplete', 'off');
	
	this.inputRange = is.ie&&is.windows?this.input.createTextRange():(is.mozilla?null:null);
	this.defaultNow = (WDG_getAttributeNS(this.input, 'defaultnow')+'').toLowerCase()=="yes";
	this.restricttomask = (WDG_getAttributeNS(this.input, 'restricttomask')+'').toLowerCase()=="yes";
	this.curGroup = "d";
	var mask = WDG_getAttributeNS(this.input, 'mask');
	if (/H/.test(mask)) {
		mask = mask.replace(/\s*t/gi,"");
	}
	
	this.mask = MXW_SmartDate_normalizemask(mask);
	this.input.value = MXW_SmartDate_normalizevalue(this.mask, this.input.value);
	this.strikes = 0;
	this.input.maskDirty = false;

	var div = utility.dom.createElement('div', {
		'id': $SMD_DIVPREFIX+(this.hasCalendar?'CAL':'') + input, 
		'style': 'position: absolute; display: none; ', 
		'className': (this.hasCalendar?$SMD_VISUAL_ALERT_DIV_CLASSNAME:$SMDCAL_VISUAL_ALERT_DIV_CLASSNAME)
	});
	div.innerHTML = WDG_Messages["the_date_format_is"] + this.mask;
	this.div = document.body.appendChild(div);
	this.div.style.display = 'none';
	this.div.style.position = 'absolute';
	
	this.spinner = (WDG_getAttributeNS(this.input, 'spinner')+'').toLowerCase()=="yes";

	var obj = this;
	this.readonly = (WDG_getAttributeNS(this.input, 'readonly')+'') == 'true';
	if (!this.readonly) {
		utility.dom.attachEvent(obj.input, (is.mozilla || is.opera)?"keypress":"keydown", function (e){return MXW_SmartDate_keydownhandler(obj, e);}, 1, false, false);
		utility.dom.attachEvent(obj.input, "keyup", function (e){return MXW_SmartDate_keyhandler(obj, e);}, 1, false, false);
	}
	utility.dom.attachEvent(obj.input, "blur", function (e){return MXW_SmartDate_blurhandler(obj, e);}, 1, false, false);
	utility.dom.attachEvent(obj.input, "focus", function (e){return MXW_SmartDate_focushandler(obj);}, 1, false, false);
	utility.dom.attachEvent(obj.input, "mouseup", function (e){
		obj.detectGroup(e);	
	}, 1, false, false);

	if (obj.input.form) {
		var frm = obj.input.form;
		if (typeof frm != 'undefined') {
			utility.dom.attachEvent2(frm, 'submit', function(e) {
				var toret = MXW_SmartDate_formhandler(obj, e);
				o = utility.dom.setEventVars(e);
				if (!toret) {
					if (typeof UNI_disableButtons == 'function') {
						UNI_disableButtons(frm, /.*/);
					}

					frm.setAttribute('widgets_error', 'true');
					utility.dom.stopEvent(o.e);
					return false;
				}
			});
		}
	}

	this.spinner = new MXW_Spin(this, $SMD_TICK_INCREMENT, this.spinner);

	window[$SMD_GLOBALOBJECT][input] = this;

	if (this.defaultNow && this.input.value == "") {
		this.setNow();
	}

	return this;
}

function MXW_SmartDate_normalizemask(mask) {
	toret = '';

	var date = mask.split(/[\-\/\[\]()\*\+\\\.]/g);
	var sep = '', c = '', dt = '', tm = '';
	var c = '';
	sep = /([\-\/\[\]()\*\+\\\.])/.exec(mask)[0];
	Array_each(date, function(c, i) {
		c = c.toUpperCase();
		switch (c) {
			case 'D' : 
				date[i] = 'DD';
				break;
			case 'M' : 
				date[i] = 'MM';
				break;
			case 'Y' : 
				date[i] = 'YY';
				break;
		}
	});
	dt = date.join(sep).toUpperCase();
	if (mask.indexOf(' ') >= 0) {
		//DATE TIME
		maskarr = mask.split(' ');
		
		var tmp = mask.substring(mask.indexOf(' ')+1, mask.length).replace(/m/ig, 'i');
		
		var time = tmp.split(/ /g);
		if (typeof time[1] != 'undefined') {
			var trailing = ' '  + time[1];
		} else {
			var trailing = '';
		}
		time = time[0].split(':');
		
		
		Array_each(time, function(c, i) {
			switch (c) {
				case 'H' : 
				case 'HH' : 
					time[i] = 'HH';
					break;
				case 'h' : 
				case 'hh' : 
					time[i] = 'hh';
					break;
				case 'i' : 
				case 'ii' :
				case 'I' : 
				case 'II':
					time[i] = 'II';
					break;
				case 's' : 
				case 'ss' :
				case 'S' : 
				case 'SS':
					time[i] = 'SS';
					break;
			}
		});
		tm = time.join(':') + trailing;
		dt = dt.split(' ')[0] + ' ' + tm;
	}
	return dt;
}

function MXW_SmartDate_normalizevalue(mask, value) {
	var re = SMD_date2regexp(mask, true);
	var arr = re.exec(value);
	if (arr) {
		var toret = '';
		Array_shift(arr);
		Array_each(arr, function(el, i) {
			if (el.match(/^[0-9]*$/)) {
				if (parseInt(el, 10) < 10) {
					arr[i] = '0' + parseInt(el, 10);
				}
			}
			toret += arr[i];
		});
		return toret;
	} else {
		return value;
	}
}

function MXW_SmartDate_toregexp(txt) {
	txt = txt.replace(/([\-\/\[\]()\*\+\\\.])/g, '\\$1');
	txt = txt.replace(/M/g, '\\d');
	txt = txt.replace(/D/g, '\\d');
	txt = txt.replace(/Y/g, '\\d');
	txt = txt.replace(/H/ig, '\\d');
	txt = txt.replace(/I/g, '\\d');
	txt = txt.replace(/S/g, '\\d');
	txt = txt.replace(/tt/g, '(AM|PM|aM|Am|am|pM|Pm|pm)');
	txt = txt.replace(/t/g, '(A|P|a|p)');
	txt = txt.replace(/\?/g, '.');
	txt = txt.replace(/\./g, '\.');
	return txt;
}

function MXW_SmartDate_dateToMask(date) {
	var y = date.getFullYear();
	var m = date.getMonth()+1;
	var d = date.getDate();
	var h = date.getHours();
	var i = date.getMinutes();
	var s = date.getSeconds();

	if (m < 10) m = "0" + m;
	if (d < 10) d = "0" + d;
	
	if (i < 10) i = "0" + i;
	if (s < 10) s = "0" + s;
	if (/yyyy/i.test(this.mask)) {
		while (y.length < 4) {
			y = "0" + y;
		}
	} else if(/yy/i.test(this.mask)) {
		y +="";
		y = y.replace(/(.*)(\d\d)$/, "$2");

		while (y.length < 2) {
			y = "0" + y;
		}
	} else if(/y/i.test(this.mask)) {
		y +="";
		y = y.replace(/(.*)(\d)$/, "$2");
		while (y.length < 1) {
			y = "0" + y;
		}
	}

	var tmp = this.mask;
	tmp = tmp.replace(/D+/i, d);
	tmp = tmp.replace(/M+/i, m);
	tmp = tmp.replace(/Y+/i, y);
	tmp = tmp.replace(/I+/i, i);
	tmp = tmp.replace(/S+/i, s);

	if (this.mask.toLowerCase().indexOf('t') >= 0) {
		if (this.mask.toLowerCase().indexOf('tt') >= 0) {
			var toadd = ['AM', 'PM'];
		} else {
			var toadd = ['A', 'P'];
		}

		var t = "";
		if(/HH|H/.test(this.mask)) {
			//24-hour clock
			t = "";
		} else if(/hh|h/.test(this.mask)) {
			//12-hour clock	
			if(h==0) {
				h = 12;
				t = toadd[1];
			} else if (h>=13 && h<=23){
				h = h - 12;
				t = toadd[1];
			} else if (h >= 1 && h <=12) {
				t = toadd[0];
			}
		}
		tmp = tmp.replace(/t+/i, t);
	}
	if (h < 10 && /hh/i.test(this.mask)){
		h = "0" + h;
	}
	tmp = tmp.replace(/H+/i, h);

	if (this.input.value != tmp) {
		this.input.maskDirty = true;
		this.input.value = tmp;
	}
	return tmp;
}
MXW_SmartDate.prototype.dateToMask = MXW_SmartDate_dateToMask;

function MXW_SmartDate_validate() {
	var mask = this.mask;
	var size = this.input.value.length;
	if (size > mask.length) {
		size = mask.length;
	}
	// validate the input value with the masks' regexp
	var re = new RegExp('^' + MXW_SmartDate_toregexp(mask.substr(0, size)) + '$');
	return this.input.value.match(re);
}
MXW_SmartDate.prototype.validate = MXW_SmartDate_validate;

function MXW_SmartDate_blurhandler(obj, evt) {
	if (obj.mousedown) {
		return true;
	}
	MXW_visualAlert(obj, 0, 'SMD'+(obj.hasCalendar?'CAL':''));
	obj.strikes = 0;
	
	obj.kt_focused = false;

	if (!obj.validate()) { 
		if (obj.input.lastGoodMatched) {
			obj.input.value = obj.input.lastGoodMatched;
		} else {
			obj.input.value = '';
		}
	} else {
		obj.input.lastGoodMatched = obj.input.value;
	}
	/*
	if(obj.input.value !="" && obj.input.value.length != obj.mask.length) {
		obj.setNow();
		return true;
	}
	*/
	// re-get the text size
	size = obj.input.value.length;

	//if we have entered 10 chars that means we have entered a potential date
	//check this date and convert it eventually to a valid date
	if (size == obj.mask.length) {
		var tmp = obj.getInputDate();
		if (tmp) {
			obj.dateToMask(tmp);
		}
	}

	if (obj.input.maskDirty) {
		obj.input.maskDirty = false;
		if (obj.input.fireEvent) {
			obj.input.fireEvent("onchange");
		} else if(document.createEvent){
			var me = document.createEvent("Events");
			me.initEvent('change', 0, 0);
			obj.input.dispatchEvent(me);
		}
	}

	return true;
}

function MXW_SmartDate_formhandler(obj, evt) {
	//if there is no 
	if (obj.defaultNow && obj.input.value == '') {
		obj.setNow();
		return true;
	} else {
		var test_value = obj.input.value;
		if (/ tt/.test(obj.mask)) {
			test_value = test_value.replace(/ A$/i, ' AM');
			test_value = test_value.replace(/ P$/i, ' PM');
		}

		var re_full = new RegExp('^' + MXW_SmartDate_toregexp(obj.mask) + '$');
		var re_piece = new RegExp('^' + MXW_SmartDate_toregexp(obj.mask.substring(0, obj.input.value.length)) + '$');

		if (obj.restricttomask) {
			if (!obj.input.value.match(re_full)) {
				obj.strikes = 3;
				MXW_visualAlert(obj, 1, 'SMD'+(obj.hasCalendar?'CAL':''));
				utility.dom.stopEvent(evt);
				try {
					obj.input.focus();
				} catch(e) { }
				return false;
			} else {
				var olddate = obj.getInputDate();
				if (olddate) {
					MXW_visualAlert(obj, 0, 'SMD'+(obj.hasCalendar?'CAL':''));
					var date = olddate;
					date = obj.dateToMask(date);
					return true;
				} else {
					MXW_visualAlert(obj, 1, 'SMD'+(obj.hasCalendar?'CAL':''));
					try {
						obj.input.focus();
					} catch(e) { }
					utility.dom.stopEvent(evt);
					return false;
				}
			}
		} else {
			if (obj.input.value == '') {
				return true;
			}
			if (test_value.match(re_piece)) {
				return true;
			} else {
				obj.strikes = 3;
				MXW_visualAlert(obj, 1, 'SMD'+(obj.hasCalendar?'CAL':''));
				utility.dom.stopEvent(evt);
				return false;
			}
		}
	}
}

function MXW_SmartDate_keyhandler(obj, evt) {
	if (!obj.kt_focused) {
		utility.dom.stopEvent(evt);
		return false;
	}

	if(evt.shiftKey || evt.ctrlKey) {
		return;
	}

	if (!obj.validate()) { 
		if (obj.input.lastGoodMatched) {
			obj.input.value = obj.input.lastGoodMatched;
		} else {
			obj.input.value = '';
		}
		obj.strikes++;
	} else {
		obj.input.lastGoodMatched = obj.input.value;
	}

	if(evt.keyCode != 8 && obj.input.value.length != 0) { // backspace and tab
		obj.completeSmartDate();
	}

	if(is.mozilla) {
		obj.detectGroup(evt);
	}
	spin_stop(evt);
	var kc = is.mozilla ? evt.charCode : evt.keyCode;
	if(!obj.mousedown && !(kc>=48 && kc<=57 || kc>=96 && kc<=105) ) {
		//obj.selectGroup();
	}

	if (obj.strikes >= 3) {
		MXW_visualAlert(obj, 1, 'SMD'+(obj.hasCalendar?'CAL':''));	
		obj.strikes = 3;
	}
	if (obj.mask.length == obj.input.value.length) {
		obj.strikes = 0;
		var tmp = obj.getInputDate();
		if (!tmp) {
			MXW_visualAlert(obj, 1, 'SMD'+(obj.hasCalendar?'CAL':''));	
		} else {
			MXW_visualAlert(obj, 0, 'SMD'+(obj.hasCalendar?'CAL':''));	
		}
	}
	return true;
}

function MXW_SmartDate_setNow() {
		date = new Date();
		this.dateToMask(date);
}
MXW_SmartDate.prototype.setNow = MXW_SmartDate_setNow;

function MXW_SmartDate_allowedChar(e) {
	var kc = is.mozilla?e.charCode:e.keyCode;
	var mkc = is.mozilla?e.keyCode:0;

	if (
		is.ie && (kc>=48 && kc<=57 || kc>=96 && kc<=105 || kc==38 || kc==40 || kc==107 || kc==109 || kc==187 || kc==189 || kc==190 || kc==46) 
			||
		(is.mozilla || is.opera) && (kc>=48 && kc<=57 || kc==45 || kc==43 || mkc==40 || mkc==38 || kc==61)
			||
		(kc == 190 || kc == 110 || kc == 46 || kc==8 || kc==37 || kc==39 || kc==33 || kc==34 || kc==35 || kc==36 || mkc==46 || mkc==8 || mkc==37 || mkc==39 || mkc==33 || mkc==34 || mkc==35 || mkc==36)
	) {
		return true;
	}
	return false;
}

function MXW_SmartDate_keydownhandler(obj, evt) {
	if (!obj.kt_focused) {
		//IE bug: any input type text or textarea continue to receive some keyboard events when the focus goes directly to the address bar (either by clicking on it or by using the ALT+D shortcut
		utility.dom.stopEvent(evt);
		return false;
	}
	var myevnt = utility.dom.setEventVars(evt);
	var kc = is.mozilla?myevnt.e.charCode:myevnt.e.keyCode;
	var mkc = is.mozilla?myevnt.e.keyCode:0;
	if(evt.shiftKey || evt.ctrlKey) {
		return;
	}
	//IE catches the keyup when refreshing page with F5
	if(kc == 116) {
		return false;
	}
	if(kc == 9) {
		return;
	}

	if (!MXW_SmartDate_allowedChar(myevnt.e)) {
		utility.dom.stopEvent(myevnt.e);
		return false;
	}
	// if the user pressed "." we autocomplete with the current date
	if ((kc == 190 || kc == 110 || kc == 46) && (obj.input.value.length == 0)) {
		obj.setNow();
		utility.dom.stopEvent(myevnt.e);
		return false;
	}
	if(!is.mozilla) {
		obj.detectGroup(myevnt.e);
	}

	if (
		(is.mozilla || is.opera) && (kc==45 || kc==43 || mkc==40 || mkc==38 || kc==61) 
		|| 
		is.ie && (kc==38 || kc==40 || kc==107 || kc==109 || kc==187 || kc==189) 
	) {
		utility.dom.stopEvent(myevnt.e);
		if (!window[$SPN_GLOBALOBJECT]['timeout']) {
			var direction = (
				is.mozilla && (kc==43 || mkc==38 || kc==61) 
				|| 
				is.ie && (kc==38 || kc==107 || kc==187)
				)
				?
				1:-1;
			obj.spin(direction, 1, myevnt.e);
			spin_start(obj, direction);
		}
		if (
			is.mozilla && (kc==43 || kc==45 || mkc==38 || kc==61) 
			|| 
			is.ie && (kc==107 || kc==109 || kc==187 || kc==189 || kc==38 || kc==40) 
		){
			utility.dom.stopEvent(myevnt.e);
			return false;
		}
	}
	return true;
}

MXW_SmartDate.prototype.selectGroup = function() {
	if (this.mask.length==this.input.value.length) {
		MXW_setSelectionRange(this.input, this.selStart, this.selEnd+1);
	}
}

MXW_SmartDate.prototype.detectGroup  = function(evt){
	var kc = is.mozilla?evt.keyCode:evt.keyCode;
	if (is.ie && is.windows) {
		var selText = document.selection.createRange();
		var selLength = selText.text.length;

		var dir = 0;
		var selPos = 0;
		if (kc == 37  || kc == 39 || kc==36 || kc==35 || kc==34 || kc==33) {
			dir += kc == 37? -1: kc == 39?1:(kc==35 || kc==34)	?0:0;
			if(dir==1) {
				selText.collapse(false);
				selPos= -selText.moveStart("character", -1000) - 1;
			} else if(dir==-1){
				selText.collapse(true);
				selPos= -selText.moveStart("character", -1000);
			}
			selPos += kc == 37? -1: kc == 39?1:(kc==35 || kc==34)?this.mask.length:-selPos;
		} else {
			selText.collapse(true);
			var selPos= -selText.moveStart("character", -1000) - 1;
		}

		if (selPos == this.mask.length) {
			selPos--;
		}
		
		selPos += dir;
		var curMaskChar = this.mask.charAt(selPos);

		while(selPos < this.mask.length && !/[MDYHIST]/i.test(curMaskChar) ) {
			curMaskChar = this.mask.charAt(++selPos);
		}

		if (selPos == this.mask.length) {
			selPos--;
			curMaskChar = this.mask.charAt(selPos);
		}

		this.selStart = this.mask.indexOf(curMaskChar);
		this.selEnd = this.mask.lastIndexOf(curMaskChar);
		this.curGroup = curMaskChar.toLowerCase();
	} else if(is.mozilla) {
		var selPos = this.input.selectionStart;
		var selEnd = this.input.selectionEnd;
		var selLength = selEnd - selPos;

		var dir = 0;
		if (kc == 37  || kc == 39 || kc==36 || kc==35 || kc==34 || kc==33) {
			dir += kc == 37? -1: kc == 39?1:(kc==35 || kc==34)	?0:0;
			if (dir == 1) {
				selPos = selEnd-1;
			}
			selPos += kc == 37? -1: kc == 39?1:(kc==35 || kc==34)?this.mask.length:-selPos;
		}
	
		if (selPos==this.mask.length) {
			selPos--;
		}
		selPos += dir;
		var curMaskChar = this.mask.charAt(selPos);
		while(selPos<this.mask.length && !/[MDYHIST]/i.test(curMaskChar) ) {
			curMaskChar = this.mask.charAt(++selPos);
		}
		if (selPos==this.mask.length) {
			selPos--;
			curMaskChar = this.mask.charAt(selPos);
		}
	
		this.selStart = this.mask.indexOf(curMaskChar);
		this.selEnd = this.mask.lastIndexOf(curMaskChar);
		this.curGroup = curMaskChar.toLowerCase();
	}
}
MXW_SmartDate.prototype.spin = function(direction, step, e) {
	MXW_visualAlert(this, 0, 'SMD'+(this.hasCalendar?'CAL':''));
	this.strikes = 0;
	if(e) {
		spin_stop(e);
	}

	if (!this.validate()) {
		return false;
	}

	var date = this.getInputDate();
	if (typeof step == "undefined") {	
		step = 1;
	}
	if (date) {
		date = dateAdd(date, this.curGroup, direction*step);
		this.dateToMask(date);
		this.selectGroup();
	} else {
		MXW_visualAlert(this, 1, 'SMD'+(this.hasCalendar?'CAL':''));
	}
}

function MXW_SmartDate_blur(evt) {
	var obj = this.input;
	if (obj.value.length != obj.mask.length && obj.value.length > 0) {
		date = this.getInputDate();
		if (date) {
			var tmp = this.dateToMask(date);
			obj.lastGoodMatched = tmp;
		} else {
			MXW_visualAlert(this, 1, 'SMD'+(obj.hasCalendar?'CAL':''));
		}
	}
}
MXW_SmartDate.prototype.blur = MXW_SmartDate_blur;

function MXW_SmartDate_focushandler(obj) {
	obj.input.maskDirty = false;
	obj.kt_focused = true;
	if(!obj.validate()) {
		obj.input.maskDirty = true;
		obj.input.value = "";
	}
}

function MXW_SmartDate_getInputDate() {
	var value = this.input.value;
	//nothing prefilled, return now()
	if(value.length == 0) {
		return new Date();
	}

	var dateMask = this.mask;
	if (/ tt/.test(dateMask)) {
		value = value.replace(/ A$/i, ' AM');
		value = value.replace(/ P$/i, ' PM');
	}
	
	var re = SMD_date2regexp(dateMask);
	var arr = re.exec(value);
	if (!arr) {
		return new Date();
	}

	var vYear = vMonth = vDay = null;
	var vHour = vHour12h = vHour24H = vMinutes = vSeconds = vTimeMarker1C = vTimeMarker2C = null;

	var groups = dateMask.split($SMD_DATETIME_MASK_REGEXP);
	var groupIdx = 0;
	for (var i = 0; i< groups.length; i++) {
		var currentGroupMask = groups[i];
		groupIdx++;
		if (groupIdx>=arr.length) {
			break;
		}
		var groupValue = arr[groupIdx];

		switch(currentGroupMask) {
		case 'YYYY':
		case 'YY': 
			vYear = parseInt(groupValue, 10);
			if (vYear < 1000) {
				if (vYear < 10) {
					vYear = 2000 + vYear;
				} else {
					if (vYear < 70) {
						vYear = 2000 + vYear;
					} else {
						vYear = 1900 + vYear;
					}
				}
			}
			break;
		case 'MM':
			vMonth = parseInt(groupValue, 10);
			break;
		case 'DD': 
			vDay = parseInt(groupValue, 10);
			break;
		case 'HH': 
			vHour24H = parseInt(groupValue, 10);
			break;
		case 'hh': 
			vHour12h = parseInt(groupValue, 10);
			break;
		case 'II':
			vMinutes = parseInt(groupValue, 10);
			break;
		case 'SS':
			vSeconds = parseInt(groupValue, 10);
			break;
		case 't':
			vTimeMarker1C = groupValue;
			break;
		case 'tt':
			vTimeMarker2C = groupValue;
			break;
		}
	}


	vYear = vYear == null?1900:vYear;
	vMonth = vMonth == null?0:vMonth;
	vDay = vDay == null?1:vDay;

	vMinutes = vMinutes == null?0:vMinutes;
	vSeconds = vSeconds == null?0:vSeconds;
	var vHourOffset = 0;

	if (vHour12h != null) {
		if (vHour12h >= 1 && vHour12h <= 12) {
			vHour = vHour12h;
			if ((vTimeMarker1C || vTimeMarker2C || "").charAt(0)=="P") {
				if (vHour12h == 12) {
					vHour = 0;
				} else {
					if (vHour12h < 12) {
						vHour = vHour12h + 12;
					}
				}
			} else {
			}
			//must add 12 to hour if time is PM
			//also, must add 12 if vHour12h in 12h format is greater than 11, which is invalid
			//vHourOffset = ( (vTimeMarker1C || vTimeMarker2C || "").charAt(0)=="P" || vHour12h>11)?12:0;
			//vHour = vHour12h + vHourOffset;
		} else {
			vHour = -1000;
		}
	} else if(vHour24H != null) {
		vHour = vHour24H;
	} else {
		vHour = 0;
	}

	var o = {
		'year': vYear, 
		'month': vMonth, 
		'day': vDay,
		'hour': vHour, 
		'minutes': vMinutes, 
		'seconds': vSeconds
	};

	//compute date at 12:00 PM  to avoid time saving problems
	if (o['hour'] == 0 && dateMask.toLowerCase().indexOf('h') < 0) {
		o['hour'] = 12;
	}
	if (!this.isValid(o['year'], o['month'], o['day'], o['hour'], o['minutes'], o['seconds'])) {
		return false;
	}
	date = new Date(o['year'], o['month']-1, o['day'], o['hour'], o['minutes'], o['seconds']);
	return date;
}
MXW_SmartDate.prototype.getInputDate = MXW_SmartDate_getInputDate;

function MXW_SmartDate_isValid(year, month, day, hour, minutes, seconds) {
	var month_length = [31,28,31,30,31,30,31,31,30,31,30,31];
	if (! (parseInt(year) > 0)) { return false; }
	if (! (parseInt(month) > 0 && parseInt(month) <= 12)) { return false; }
	if ((
			(parseInt(year) % 4 == 0) 
			&& 
			(parseInt(year) % 100 != 0)
		) 
		|| 
			(parseInt(year) % 400 == 0)
		) {
			month_length[1] = 29;
	}
	if (! (parseInt(day) > 0 && parseInt(day) <= month_length[parseInt(month)-1])) { return false; }

	month_length[1] = 28;
	if (! (parseInt(hour) >= 0 && parseInt(hour) <= 23)) { return false; }
	if (! (parseInt(minutes) >= 0 && parseInt(minutes) <= 59)) { return false; }
	if (! (parseInt(seconds) >= 0 && parseInt(seconds) <= 59)) { return false; }
	
	return true;
}
MXW_SmartDate.prototype.isValid = MXW_SmartDate_isValid;


/**
	complete the current typing text with the next char from the mask
	@param
		obj - SmartDate DOM Object
		mask - the Mask
	
**/
function MXW_SmartDate_completeSmartDate() {
	var obj = this.input;
	var mask = this.mask;
	var size = obj.value.length;
	var sw=true;
	var tmp = obj.value;

	while (sw) {
		if (mask.length<=size) {
			break;
		}
		switch (mask.charAt(size)) {
			case 'M':
			case 'D':
			case 'Y':
			case 'H':
			case 'h':
			case 'I':
			case 'S':
			case 't':
				sw = false;
				break;
			default:
				obj.maskDirty = true;
				tmp += mask.charAt(size) + "";
		}
		size++;
	}
	if (tmp!=obj.value) {
		obj.value = tmp;
		if (is.opera) {
			MXW_setSelectionRange(obj, obj.value.length+1, obj.value.length+1);
		}
	}
	obj.lastGoodMatched = obj.value;
}
MXW_SmartDate.prototype.completeSmartDate = MXW_SmartDate_completeSmartDate;

function dateAdd(date, what, howmuch) {
	var y = date.getFullYear();
	var m = date.getMonth()+1;
	var d = date.getDate();
	var h = date.getHours();
	var i = date.getMinutes();
	var s = date.getSeconds();

	switch (what) {
		case "d":
			d += howmuch;
			break;
		case "m":
			m += howmuch;
			break;
		case "y":
			y += howmuch;
			break;
		case "h":
			h += howmuch;
			break;
		case "i":
			i += howmuch;
			break;
		case "s":
			s += howmuch;
			break;
		case "t":
			if (h > 12) {
				h = h-12;
			} else {
				h = h + 12;
			}
			break;
	}
	//compute date at 12:00PM to avoid time saving problems
	date = new Date(y, m-1, d, h, i, s);
	return date;
}

