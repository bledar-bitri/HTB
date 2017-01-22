// Copyright 2001-2005 Interakt Online. All rights reserved.

function MXW_Calendar (boundTo) {
	if (is.safari && is.version < 1.4) {
		return;
	}
	this.input = document.getElementById(boundTo);

	var oldmask = WDG_getAttributeNS(this.input, 'mask');
	var mask = oldmask.replace(/ t$/, ' tt');
	WDG_setAttributeNS(this.input, 'mask', mask);

	this.readonly = (WDG_getAttributeNS(this.input, 'readonly')+'') == 'true';
	if (this.readonly) {
		this.input.readOnly = true;
	}
	//Calendar does not work on IE 5 on MAc, so we  have only SmartDate, and add spinner
	if (is.ie && is.mac) {
		WDG_setAttributeNS(this.input, 'spinner', 'yes');
		this.sd = new MXW_SmartDate(boundTo);
		return this;
	}
	this.sd = new MXW_SmartDate(boundTo);

	var paramObj = {};
	paramObj.inputField = boundTo;
	paramObj.button = boundTo + "_btn";
	paramObj.ifFormat = mask2calendar(this.sd.mask);
	paramObj.daFormat = mask2calendar(this.sd.mask);
	paramObj.label = WDG_Messages["calendar_button"];
	paramObj.firstDay = (WDG_getAttributeNS(this.input, 'mondayfirst') == 'true') ? 1 : 0 ;
	paramObj.singleClick = WDG_getAttributeNS(this.input, 'singleclick') == 'true';
	if (/(h|i|s|t)/.test(mask)) {
		paramObj.showsTime = true;
	}
	if (/ t$/.test(oldmask)) {
		paramObj.onUpdate = function(calendar) {
			var input_field = document.getElementById(boundTo);
			input_field.value = input_field.value.replace(/AM/gi, 'A');
			input_field.value = input_field.value.replace(/PM/gi, 'P');
		}
	}

	//WDG_setAttributeNS(this.input, 'mask', mask2calendar(paramObj.ifFormat));

	var btnAttributes = {
		"type":"button",
		"name":boundTo+"_btn",
		"class":"btnStandard",
		"id":boundTo+"_btn",
		"value":paramObj.label
	};

	var btnSrcAttributes = WDG_getAttributeNS(this.input, 'suppattrs')+'';
	if (btnSrcAttributes = btnSrcAttributes.match(/[^\s]+='[^']*'/gi)) {
		for (var i=0; i<btnSrcAttributes.length; i++) {
			var oAttr = btnSrcAttributes[i].match(/([^\s]+)='([^']*)'/i);
			if(oAttr) {
				btnAttributes[oAttr[1]] = oAttr[2];
			}
		}
	}
	this.button = utility.dom.createElement("input", btnAttributes);
	utility.dom.insertAfter(this.button, this.input);

	Calendar.setup(paramObj);
}
function mask2calendar(cfm) {
	cfm = cfm.replace(/yyyy/gi, '%Y');
	cfm = cfm.replace(/(yy)/gi, '%y');
	
	cfm = cfm.replace(/(mm)/gi, '%m');

	cfm = cfm.replace(/(dd)/gi, '%d');

	cfm = cfm.replace(/(hh)/g, '%I');
	
	cfm = cfm.replace(/(HH)/g, '%H');

	cfm = cfm.replace(/(ii)/gi, '%M');

	cfm = cfm.replace(/(ss)/gi, '%S');

	cfm = cfm.replace(/(tt)/gi, '%p');

	return cfm;
}
