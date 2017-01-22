// Copyright 2001-2005 Interakt Online. All rights reserved.

/*
 *
dependent dropdown
should create two objects : 
	masterselect, only if it doesn't exist
	details select
 */

function MXW_DependentDropdown(detailSelect) {
	this.detailSelect = document.getElementById(detailSelect);
	this.masterSelect = document.getElementById(WDG_getAttributeNS(this.detailSelect, 'triggerobject'));

	var tmp = new MXW_MasterSelect(this.masterSelect);
	
	this.recordset = new JSRecordset(WDG_getAttributeNS(this.detailSelect, 'recordset'));
	this.fkey = WDG_getAttributeNS(this.detailSelect, 'fkey');
	this.valuefield = WDG_getAttributeNS(this.detailSelect, 'valuefield');
	this.displayfield = WDG_getAttributeNS(this.detailSelect, 'displayfield');
	this.defaultValue = WDG_getAttributeNS(this.detailSelect, 'selected');

	window[$DDR_DEPENDENT_OBJ][this.masterSelect.id + '_' + this.detailSelect.id] = this;
	__sig__.connectByName(tmp, 'change', this, 'updateMe');
	this.initialize();
}



function MXW_DependentDropdown_initialize() {
	this.defaultOptions = [];
	for (var i=0; i < this.detailSelect.options.length; i++) {
		Array_push(this.defaultOptions, {
			'value': this.detailSelect.options[i].value, 
			'text': this.detailSelect.options[i].text
		});
	}

	if (this.defaultValue) {
		if(this.recordset.find(this.valuefield, "=", this.defaultValue) ) {
			for (var i=0;i<this.masterSelect.options.length;i++) {
				if (this.masterSelect.options[i].value == this.recordset.Fields(this.fkey)) {
					this.masterSelect.selectedIndex = i;
					this.updateMe();
					break;
				}
			}
		}
	} else {
		this.updateMe();
	}
}

function MXW_DependentDropdown_updateMe() {
	var detailSelect = this.detailSelect;
	var masterSelect = this.masterSelect;
	var defaultOptions = this.defaultOptions;

	if (detailSelect.options.length != 0 && !this.firstSelectMasterDone) {
		this.firstSelectMasterDone = true;
		var tmp = detailSelect[detailSelect.options.length - 1].value;
		if (this.recordset.find(this.valuefield, "=", tmp) ) {
			tmp = this.recordset.Fields(this.fkey);
		}
		if (masterSelect.selectedIndex != -1 && tmp == masterSelect.options[masterSelect.selectedIndex].value) {
			return;
		}
	}

	detailSelect.options.length = 0;

	if (masterSelect.options.length == 0) {
		return;
	}

	// first add defaults
	Array_each(defaultOptions, function(item, i) {
		detailSelect.options[detailSelect.options.length] = 
			new Option(utility.string.getInnerText(item['text']), item['value']);
	});

	// add values
	this.recordset.MoveFirst();
	if (masterSelect.selectedIndex != -1) { 
		var selectedValues = [];
		for(var i=0; i<masterSelect.options.length; i++) {
			if(masterSelect.options[i].selected)  {
				 Array_push(selectedValues, masterSelect.options[i].value);
			}
		}
		var optLength = 0, selOptIndex = 0;
		while (this.recordset.MoveNext()) {
				if(Array_indexOf(selectedValues, this.recordset.Fields(this.fkey))>=0) {
				var o = new Option(
					utility.string.getInnerText(this.recordset.Fields(this.displayfield)), 
					this.recordset.Fields(this.valuefield)
				);
				optLength++;
				if (this.defaultValue == this.recordset.Fields(this.valuefield)) {
					selOptIndex = optLength - 1;
				}
				detailSelect.options[detailSelect.options.length] = o;
			}
		}
		try { detailSelect.selectedIndex = selOptIndex; } catch(e) { }
	}
	if (typeof window[$DDR_MASTERSELECT_OBJ][this.detailSelect.id] != 'undefined') {
		window[$DDR_MASTERSELECT_OBJ][this.detailSelect.id].change();
	}
}
MXW_DependentDropdown.prototype.initialize = MXW_DependentDropdown_initialize;
MXW_DependentDropdown.prototype.updateMe = MXW_DependentDropdown_updateMe;
