// Copyright 2001-2005 Interakt Online. All rights reserved.

if (typeof top.jsWindowRecordsets=="undefined") {
	top.jsWindowRecordsets = [];
}
function JSRecordset(rsName) {
	var rawdata = top['jsRawData_'+rsName];
	var metadata = rawdata[0];
	this.Filter = false;
	this.fields = [];
	this.fieldNameIndex = [];
	for (var i=0; i<metadata.length; i++) {
		//later add more field info; fields and field are objects
		var fieldName = metadata[i];
		Array_push(this.fields, {'name':fieldName});
		//the index of this field value in the values array of one row
		this.fieldNameIndex[fieldName] = i;
	}
	// all but first and last rawdata array elements are the rows
	this.rows = rawdata.slice(1, -1);
	this.origRows = this.rows ;
	//MoveFirst
	this.rowIndex = -1;
	top.jsWindowRecordsets[rsName] = this;
}

/*
* call with index or fieldname
*/
JSRecordset.prototype.Fields = function(fieldIndex) {
	if (typeof fieldIndex == "string") {
		if (this.fieldNameIndex[fieldIndex]) {
			fieldIndex = this.fieldNameIndex[fieldIndex];
		} else {
			fieldIndex = this.fieldNameIndex[fieldIndex.toLowerCase()];
		}
	}
	try {
	return this.rows[this.rowIndex][fieldIndex];
	} catch(e) { }
}

JSRecordset.prototype.Move = function(i) {
	this.rowIndex = i;
}


JSRecordset.prototype.MoveFirst = function() {
	this.rowIndex = -1;
}

JSRecordset.prototype.MoveLast = function() {
	this.rowIndex = this.RecordCount();
}

JSRecordset.prototype.MoveNext = function() {
	this.rowIndex++;
	if (this.EOF()) {
		return false;
	}	else {
		return true;
	}
}

JSRecordset.prototype.MovePrev = function() {
	this.rowIndex--;
	if (this.BOF()) {
		return false;
	}	else {
		return true;
	}
}

JSRecordset.prototype.EOF = function() {
	return this.rowIndex >= this.RecordCount();
}

JSRecordset.prototype.BOF = function() {
	return this.rowIndex < 0;
}

JSRecordset.prototype.RecordCount = function() {
	if (typeof this.intRecordCount=="undefined") {
		this.intRecordCount = this.rows.length;
	}
	return this.intRecordCount;
}

function fsort(a, b) {
	return 1;
}
JSRecordset.prototype.sort = function(sortField, sortHow) {
//	var sarr = this.rows.sort(fsort);
}
JSRecordset.prototype.find = function(searchField, searchCriteria, searchValue) {
	var searchFieldIndex = this.fieldNameIndex[searchField];
	switch(searchCriteria) {
	case "=":
		for (var i=0; i<this.rows.length;i++) {
			if (this.rows[i][searchFieldIndex] == searchValue) {
				this.rowIndex = i;
				return true;
			}
		}
		break;
	case "begins with":
		searchValue = searchValue.replace(/([\[\]\(\)\*\\])/g, '\\$1');
		var regx = new RegExp("^" + searchValue, "gi");
		regx.compile("^"+searchValue, "gi");
		for (var i=0; i<this.rows.length; i++) {
			if (regx.test(this.rows[i][searchFieldIndex])) {
				this.rowIndex = i;
				return true;
			}
		}
		break;
	}
	return false;
}

JSRecordset.prototype.Insert = function(row, index) {
	var newRow = new Array();
	for (var i=0; i<this.fields.length; i++) {
		newRow[this.fieldNameIndex[this.fields[i].name]] = row[this.fields[i].name];
	}
	Array_push(this.origRows, newRow);
	
	this.rows = this.origRows;

	this.refreshFilteredData();
}
function JSRecordset_setFilter(fkey, criteria, value) {
	if (typeof fkey == "undefined") {
		this.Filter = null;
	} else {
		this.Filter = {'field':fkey, 'criteria':criteria, 'value':value};
	}
	this.refreshFilteredData();
}

function JSRecordset_refreshFilteredData () {
	delete this.intRecordCount;
	this.MoveFirst();
	var filteredRows = [];
	this.rows = this.origRows;
	for(var i=0; i<this.origRows.length; i++) {
		this.rowIndex = i;
		if (this.matchFilter()) {
			Array_push(filteredRows, this.origRows[i]);
		}
	}
	this.rows = filteredRows;
	this.MoveFirst();
}
JSRecordset.prototype.setFilter = JSRecordset_setFilter;
JSRecordset.prototype.refreshFilteredData = JSRecordset_refreshFilteredData;

function JSRecordset_matchFilter() {
	if (!this.Filter) {
		return true;
	}
	var match = true;
	switch(this.Filter.criteria) {
		case "=":
			match = this.Fields(this.Filter.field) == this.Filter.value;
			break;
		case "begins with":
			match = this.Fields(this.Filter.field).toLowerCase().indexOf(this.Filter.value)==0;
		default:
	}
	return match;
}
JSRecordset.prototype.matchFilter = JSRecordset_matchFilter;
