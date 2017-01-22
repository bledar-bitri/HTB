//configuration variables

// return an array with checked checkboxes names and values
function nxt_list_collect_checked(form) {
	var res = [];
	Array_each(utility.dom.getElementsByTagName(form, 'INPUT'), function(input, i){
		if (input.type == "checkbox") {
			if (input.name.match(/^kt_pk/) && input.checked) {
				Array_push(res, [input.name, input.value, input]);
			}
		}
	});
	return res;
}
// when clicking on a link, select the checkbox on the same row
function nxt_list_select_cbx_from_link(o) {
	var parent = utility.dom.getParentByTagName(o, "TR");
	var cbx = null;
	Array_each(utility.dom.getElementsByTagName(parent, "INPUT"), function(input, i){
		if (!cbx && input.type == 'checkbox' && input.name.match(/^kt_pk/)) {
			cbx = input;
		}
	});
	if (cbx) {
		cbx.checked = true;
		nxt_list_set_checkbox_state(cbx);
	}
	return cbx;
}

// get a list of inputs as returned by "nxt_list_collect_checked" 
// return a form element attached to the end of the current document
// submits to the delete page should send nothing, defaults are what we want
// 	- items will be named: FOO_1, FOO_2, FOO_3
// submits to the edit page should send {method: 'GET', skip_rename: 1}
// 	- items will be named: FOO, FOO_1, FOO_2
// raw inputs has the same format as inputs, but will never get it's items renamed
function nxt_list_submit_inputs(inputs, options, raw_inputs) {
	var default_options = {
		action: '',
		method: 'POST', // form method
		rename: true,   // wether to rename inputs before adding
		skip_rename: 0  // number of items to skip from renaming at the start of the array
	}
	// put defaults options in if they were not passed
	for (var x in default_options) {
		if (typeof options[x] == 'undefined') {
			options[x] = default_options[x];
		}
	}
	if (typeof raw_inputs == 'undefined') raw_inputs = [];
	// make form
	var frm = utility.dom.createElement(
		"FORM", 
		{action: options.action, method: options.method, style: "display: none"}
	);
	Array_each(inputs, function(input, i){
		var iname = input[0];
		if (options.rename && i >= options.skip_rename) {
			iname += '_' + (i - options.skip_rename + 1);
		}
		frm.appendChild(utility.dom.createElement( 'INPUT', { 'type': 'hidden', 'name': iname,  'id': iname, 'value': input[1] }));
	});
	Array_each(raw_inputs, function(input, i){
		frm.appendChild(utility.dom.createElement('INPUT', {'type': 'hidden', 'id': input[0], 'name': input[0], 'value': input[1]}));
	});
	// post form
	document.body.appendChild(frm);
	return frm;
}

// edit links and buttons
function nxt_list_edit_link_row() {
	// autoselect current
	nxt_list_select_cbx_from_link(this);
	var form = utility.dom.getParentByTagName(this, 'FORM');
	return nxt_list_edit_link_base(form);
}
function nxt_list_edit_link_form(elem, page_url) {
	var form = utility.dom.getParentByTagName(elem, 'FORM');
	nxt_list_edit_link_base(form, page_url);
}
function nxt_list_edit_link_base(form, page_url) {
	if (!nxt_list_check_changed()) {
		return false;
	}
	list_has_changed = false;
	// collect
	var inputs = nxt_list_collect_checked(form);
	if (inputs.length == 0) {
		alert(NXT_Messages['please_select_record']);
		return true;
	}

	var res = [];
	var variables = [['KT_back', '1']];
	var qs = new QueryString(form.param_name);
	Array_each(qs.keys, function(key, i) {
		if (key != 'KT_back') {
			Array_push(variables, [key, qs.values[i]]);
		}
	});

	Array_each(inputs, function(check, i) {
		var hidden = utility.dom.getChildrenByTagName(check[2].parentNode, 'input');
		Array_push(res, [hidden[1].name, hidden[1].value, hidden[1]]);
	});

	if (typeof page_url == 'undefined') {
		page_url = form.form_action;
	}

	var frm = nxt_list_submit_inputs(
		res, 
		{action: page_url, method: 'GET', skip_rename: 1}, 
		variables
	);
	frm.submit();
	return false;
}

// delete links an buttons
function nxt_list_delete_link_row() {
	// autoselect current
	var cbx = nxt_list_select_cbx_from_link(this);
	var table = utility.dom.getParentByTagName(this, "table");
	Array_each(utility.dom.getElementsByTagName(table, "INPUT"), function(input, i){
		if (input.type == 'checkbox' && input.name.match(/^kt_pk/)) {
			if (input != cbx) {
				input.checked = false;
			}
		}
	});
	var form = utility.dom.getParentByTagName(this, 'FORM');
	return nxt_list_delete_link_base(form);
}
function nxt_list_delete_link_form(elem) {
	var form = utility.dom.getParentByTagName(elem, 'FORM');
	nxt_list_delete_link_base(form);
}
function nxt_list_delete_link_base(form) {
	if (!nxt_list_check_changed()) {
		return false;
	}
	list_has_changed = false;
	// collect
	var inputs = nxt_list_collect_checked(form);

	if (inputs.length == 0) {
		alert(NXT_Messages['please_select_record']);
		return false;
	}

	if (!confirm(NXT_Messages['are_you_sure_delete'])) {
		return false;
	}

	//get the hidden id_... from the form
	var res = [];
	Array_each(inputs, function(check, i) {
		var hidden = utility.dom.getChildrenByTagName(check[2].parentNode, 'input');
		Array_push(res, [hidden[1].name, hidden[1].value, hidden[1]]);
	});

	//build the query string
	var res_action = form.form_action;
	Array_each(res, function(input, i){
		var iname = input[0];
		if (i >= 1) {
			iname += '_' + (i);
		}
		res_action += ((res_action.indexOf('?') >= 0) ? '&' : '?') + iname + '=' + input[1];
	});
	res_action += ((res_action.indexOf('?') >= 0) ? '&' : '?') + 'KT_back=1';

	var variables = [['KT_Delete1', '1']];
	var qs = new QueryString(form.param_name);
	Array_each(qs.keys, function(key, i) {
		if (key != 'KT_back') {
			res_action += ((res_action.indexOf('?') >= 0) ? '&' : '?') + key + '=' + qs.values[i];
		}
	});

	var frm = nxt_list_submit_inputs(
		inputs, 
		{action: res_action, method: 'POST'}, 
		variables
	);
	// GET: id_usr=1&id_usr_1=2&KT_back=1
	// POST: kt_pk_user_usr_1=1&kt_pk_user_usr_2=2&KT_Delete1=1
	frm.submit();

	return false;
}

// add new item button
// the button object must be passed
function nxt_list_additem(o) {
	if (!nxt_list_check_changed()) {
		return false;
	}
	list_has_changed = false;
	var a_href = utility.dom.getLink(utility.dom.getElementsBySelector('a.KT_additem_op_link')[0]);
	if (a_href.indexOf("?") != -1) {
		parts = a_href.split("?");
		var param_name = parts[1];
		param_name = param_name.replace(/&amp;/, '&');
		var qs = new QueryString(param_name);
		form_action = parts[0];
	}
	var sel = utility.dom.getElementById(utility.dom.getParentByTagName(o, 'div'), "no_new");
	if (sel.length == 1) {
		var count = sel[0].value;
	} else {
		var count = 1;
	}
	var form = utility.dom.getParentByTagName(o, 'FORM');
	var variables = [
		['no_new', count], 
		['KT_back', 1]
	];
	Array_each(qs.keys, function(key, i) {
		if (key != 'KT_back') {
			Array_push(variables, [key, qs.values[i]]);
		}
	});
	var frm = nxt_list_submit_inputs(
		[], 
		{'action': form_action, 'method': 'GET'}, 
		variables
	);

	frm.submit();
	return false;
}

// delete links an buttons

function nxt_list_first_data_row(table) {
	var first_data_row = 1 + utility.dom.getElementsByClassName(table, 'KT_row_filter', 'tr').length;
	return first_data_row;
}

function nxt_list_last_data_row(table) {
	var last_data_row = table.rows.length - 1;
	return last_data_row;
}

function nxt_list_check_changed(e) {
	if (typeof e != 'undefined') {
		var o = utility.dom.setEventVars(e);
	}
	if (typeof list_has_changed != 'undefined' && list_has_changed == true)	{
		if (confirm(NXT_Messages['are_you_sure_move'])) {
			return true;
		} else {
			try {
			utility.dom.stopEvent(o.e);
			} catch(e) { }
			return false;
		}
	}
	return true;
}

function nxt_list_check_changed_unload() {
	if (typeof list_has_changed != 'undefined' && list_has_changed == true)	{
		return NXT_Messages['are_you_sure_move'];
	}
}

function nxt_list_move_link_row_up(e) {
	var o = utility.dom.setEventVars(e);
	toret = nxt_list_move_link_row(this, -1);
	utility.dom.stopEvent(o.e);
	return toret;
}
function nxt_list_move_link_row_down(e) {
	var o = utility.dom.setEventVars(e);
	toret = nxt_list_move_link_row(this, 1);
	utility.dom.stopEvent(o.e);
	return toret;
}
function nxt_list_move_link_row(obj, direction) {
	// autoselect current
	//nxt_list_select_cbx_from_link(obj);
	var form = utility.dom.getParentByTagName(obj, 'FORM');
	var table = utility.dom.getParentByTagName(obj, 'table');
	var row = utility.dom.getParentByTagName(obj, 'tr');

	var row_length = table.getElementsByTagName('tbody')[0].getElementsByTagName('tr').length;
	var has_filter = utility.dom.getElementsByClassName(table, "KT_row_filter").length;
	row_length = row_length + has_filter;

	var tbody = utility.dom.getParentByTagName(obj, 'tbody');

	row.style.backgroundColor = $nxt_tr_movehighlight_color;
	row.setAttribute('moved', '1');

	var first_data_row = nxt_list_first_data_row(table);

	var	index1 = row.rowIndex;
	var	index2 = index1 + direction;

	if (direction == 1 && index1 == ( table.rows.length-1 ) ) {
		return false;
	}
	if (direction == -1 && index1 <= first_data_row) {
		return false;
	}

	list_has_changed = true;

	var r2 = table.rows[index2];
	var r1 = table.rows[index1];

	if ( direction == -1 ) {
		tbody.insertBefore(tbody.removeChild(r1),r2);
	}

	if ( direction == 1 ) {
		tbody.insertBefore(tbody.removeChild(r2),r1);
	}

	var checked_attr = [];
	Array_each([r1, r2], function(row, row_index) {
		Array_each(utility.dom.getElementsByTagName(row, 'input'), function(el) {
			if (el.type.toLowerCase() == 'checkbox') {
				checked_attr[row_index] = el.checked;
			}
		});
	});
	//update the hidden fields : field1, field2 and the collecter
	var input1 = utility.dom.getElementsByClassName(r1, 'KT_orderhidden');
	var input2 = utility.dom.getElementsByClassName(r2, 'KT_orderhidden');
	if (input1.length == 1 && input2.length == 1) {
		input1 = input1[0];
		input2 = input2[0];
		//old_order | new_order
		var tmp1 = input1.value.split('|');
		var tmp2 = input2.value.split('|');

		input1.value =  tmp1[0] + '|' + tmp2[1];
		input2.value = tmp2[0] + '|' + tmp1[1];
	}
	//set the class names
	if (Array_indexOf(utility.dom.getClassNames(r1), 'KT_even') >= 0) {
		if (r1.getAttribute('moved') != '1') { utility.dom.classNameRemove(r1, 'KT_even'); }
		if (r2.getAttribute('moved') != '1') { utility.dom.classNameAdd(r2, 'KT_even'); }
	} else {
		if (r1.getAttribute('moved') != '1') { utility.dom.classNameRemove(r2, 'KT_even'); }
		if (r2.getAttribute('moved') != '1') { utility.dom.classNameAdd(r1, 'KT_even'); }
	}
	if (typeof $NXT_LIST_SETTINGS.row_effects != 'undefined' && $NXT_LIST_SETTINGS.row_effects) {
		if (Array_indexOf(utility.dom.getClassNames(r1), 'KT_highlight') >= 0) {
			if (r1.getAttribute('moved') != '1') { utility.dom.classNameRemove(r1, 'KT_highlight'); }
		} else {
			if (r1.getAttribute('moved') != '1') { utility.dom.classNameRemove(r2, 'KT_highlight'); }
		}
	}
	Array_each([r1, r2], function(row, row_index) {
		Array_each(utility.dom.getElementsByTagName(row, 'input'), function(el) {
			if (el.type.toLowerCase() == 'checkbox') {
				el.checked = checked_attr[row_index];
			}
		});
	});
	
	var first_row = r1.rowIndex > r2.rowIndex ? r2 : r1 ;
	var second_row = r1.rowIndex > r2.rowIndex ? r1 : r2 ;

	if (first_row.rowIndex == first_data_row) {
		//show all on second, hide on first
		nxt_hide_move_link(utility.dom.getElementsByClassName(first_row, 'KT_order')[0], 1);
		var tagname = '';

		if ($NXT_LIST_SETTINGS.show_as_buttons) { tagname = 'input';
		} else { tagname = 'a'; }

		Array_each(
			utility.dom.getElementsByClassName(second_row, 'KT_order')[0].getElementsByTagName(tagname), 
			function(el) { el.style.visibility = 'visible'; }
		);
	} else if (second_row.rowIndex == row_length) {
		//show all on first, hide on second_row
		nxt_hide_move_link(utility.dom.getElementsByClassName(second_row, 'KT_order')[0], 0);

		if ($NXT_LIST_SETTINGS.show_as_buttons) { tagname = 'input';
		} else { tagname = 'a'; }

		Array_each(
			utility.dom.getElementsByClassName(first_row, 'KT_order')[0].getElementsByTagName(tagname), 
			function(el) { el.style.visibility = 'visible'; }
		);
	}
	if (row_length == 2) {
		//hide second on first row, hide first on second row
		nxt_hide_move_link(utility.dom.getElementsByClassName(first_row, 'KT_order')[0], 1);	
		nxt_hide_move_link(utility.dom.getElementsByClassName(second_row, 'KT_order')[0], 0);
	}
	var op_link = utility.dom.getElementsBySelector('a.KT_move_op_link');
	for (var i =0; i < op_link.length; i++) {
		var to_op = op_link[i];
		var inp = utility.dom.getNextSiblingByTagName(to_op, 'input');
		if (inp != null) {
			to_op = inp;
		}
		to_op.style.display = '';
		var th = utility.dom.getParentByTagName(to_op, 'th');
		Array_each(th.getElementsByTagName('a'), function(el) {
			if (el != op_link[i]) {
				el.style.display = 'none';
			}
		});
		utility.dom.classNameAdd(th, 'KT_order_selected');

	}

	return false;
}

function nxt_list_move_link_form(elem) {
	var form = utility.dom.getParentByTagName(elem, 'FORM');

	//collect and fill;
	var str = [];
	var at_least_one = true;
	Array_each(utility.dom.getElementsByTagName(form, 'INPUT'), function(input, i){
		if (input.type == "checkbox") {
			if (input.name.match(/^kt_pk/)) {
				var tr = utility.dom.getParentByTagName(input, 'tr');
				var other = utility.dom.getElementsByClassName(tr, 'KT_orderhidden');
				if (other.length > 0) {
					var tmp = other[0].value.split('|');
					Array_push(str, input.value + '|' + other[0].value);
				}
			}
		}
	});
	str = str.join(',');
	if (at_least_one) {
		var parts = window.location.href.toString().split('?');
		if (parts.length == 1) {
			parts[1] = '';
		}
		var qs = new QueryString(parts[1]); var action_url = '', variables = [];
		Array_each(qs.keys, function(key, i) {
			Array_push(variables, key+'='+qs.values[i]);
		});
		action_url = parts[0] + '?' + variables.join('&');
		list_has_changed = false;
		var frm = nxt_list_submit_inputs(
			[], 
			{action: action_url, method: 'POST'}, 
			[[$NXT_MOVE_SETTINGS['orderfield'], str]]
		);
		frm.submit();
	}
	return false;
}

// check a box and set color row
function nxt_list_set_checkbox_state(cbx, state) {
	var row = utility.dom.getParentByTagName(cbx, "TR");
	if (typeof state == 'undefined') {
		state = cbx.checked;
	} else {
		cbx.checked = state;
	}
	if (typeof $NXT_LIST_SETTINGS.row_effects != 'undefined' && $NXT_LIST_SETTINGS.row_effects) {
		if (row.getAttribute('moved') != '1') {
			if (state) {
				utility.dom.classNameAdd(row, "KT_highlight");
			} else {
				utility.dom.classNameRemove(row, "KT_highlight");
			}
		}
	}
}

// mass checkbox click
function nxt_list_cbxmass_onchange() {
	var item = this;
	Array_each(utility.dom.getElementsByTagName(item.parent_div, "INPUT"), function(input, i){
		if (input.type == 'checkbox' && typeof input.parent_div != 'undefined') {
			nxt_list_set_checkbox_state(input, item.checked);
		}
	})
}

// normal checkbox click
function nxt_list_cbx_onchange(e) {
	var o = utility.dom.setEventVars(e);
	var state = true;
	var mass = null;
	Array_each(utility.dom.getElementsByTagName(cb.parent_div, "INPUT"), function(input, i){
		if (input.type == 'checkbox' && typeof input.parent_div != 'undefined') {
			if (input.name != 'KT_selAll') {
				state = state && input.checked;
				nxt_list_set_checkbox_state(input);
			} else {
				mass = input;
			}
		}
	});
	if (mass) {
		mass.checked = state;
	}
	utility.dom.stopEvent(o.e);
	return true;
} 

// toggle filter row visibility
function nxt_list_toggle_filter() {
	utility.dom.toggleElem(this.filter_div);
}

function nxt_list_attach() {
	var great_parent;
	if (typeof is == 'undefined') {
		is = new BrowserCheck();
	}
	$nxt_tr_over_color = '';
	$nxt_tr_even_color = '';
	$nxt_tr_highlight_color = '';
	$nxt_tr_movehighlight_color = '';

	if (is.mozilla || is.ie || is.safari) {
		for (var d = 0; d < document.styleSheets.length; d++) {
			var imp = utility.dom.getImports(document.styleSheets[d]);
			for (var i = 0; i < imp.length; i++) {
				if (imp[i].href.match(/nxt\.css/)) {
					try {
						$nxt_tr_over_color = utility.dom.getRuleBySelector(imp[i], /KT_over/)[0].style.backgroundColor;
						$nxt_tr_even_color = utility.dom.getRuleBySelector(imp[i], /KT_even/)[0].style.backgroundColor;
						$nxt_tr_highlight_color = utility.dom.getRuleBySelector(imp[i], /KT_highlight/)[0].style.backgroundColor;
						$nxt_tr_movehighlight_color = utility.dom.getRuleBySelector(imp[i], /KT_movehighlight/)[0].style.backgroundColor;
					} catch(exc) {
						$nxt_tr_over_color = '';
						$nxt_tr_even_color = '';
						$nxt_tr_highlight_color = '';
						$nxt_tr_movehighlight_color = '';
					}
				}
			}
		}
	}

	great_parent = utility.dom.getElementsByClassName(document, 'KT_tng', 'DIV');
	Array_each(great_parent, function(elem) {
		////////////////////
		//	STEP 0: add the float class to the div
		////////////////////
		var footer = utility.dom.getElementsBySelector('div.KT_bottombuttons', elem);
		if (footer.length == 1) {
			var elementList = utility.dom.getElementsBySelector('div.KT_operations', footer[0]);
			Array_each(elementList, function(element) {
				 utility.dom.classNameAdd(element, 'KT_left');
			 });
		}
		////////////////////
		//	STEP 1:  copy footer buttons to header
		////////////////////
		
		if ($NXT_LIST_SETTINGS.duplicate_buttons) {
			if (footer.length == 1) {
				footer = footer[0];
				var header = document.createElement('DIV');
				header.className = 'KT_topbuttons';
				header.innerHTML = footer.innerHTML;
				footer.parentNode.insertBefore(header, footer.parentNode.firstChild);
				//if (is.ie) {
					Array_each(['input'], function(tagname) {
						var from = footer.getElementsByTagName(tagname);
						var to = header.getElementsByTagName(tagname);
						Array_each(from, function(asd, i) {
							to[i].onclick = from[i].onclick;
						});
					});
				//}
			}
		}

		////////////////////
		//	STEP 2 :  copy footer navigation to header
		////////////////////
		if ($NXT_LIST_SETTINGS.duplicate_navigation) {
			var footer = utility.dom.getElementsBySelector('div.KT_bottomnav');
			if (footer.length == 1) {
				footer = footer[0];
				var header = footer.cloneNode(true);//document.createElement('DIV');
				header.className = 'KT_topnav';
				//header.innerHTML = footer.innerHTML;
				var ins_before_node = utility.dom.getElementsByClassName(footer.parentNode, 'KT_options', 'div')[0];
				footer.parentNode.insertBefore(header, ins_before_node);
				//if (is.ie) {
					Array_each(['input'], function(tagname) {
						var from = footer.getElementsByTagName(tagname);
						var to = header.getElementsByTagName(tagname);
						Array_each(from, function(asd, i) {
							to[i].onclick = from[i].onclick;
						});
					});
				//}
			}
		}

		////////////////////
		//	STEP 3 : add onclick handlers for checkboxes
		////////////////////
		var parent_div = utility.dom.getElementsByClassName(elem, 'KT_tngtable', 'TABLE');
		if (parent_div.length != 1) {
			return;
		}
		parent_div = parent_div[0];
		var inputs = utility.dom.getElementsByTagName(elem, "INPUT");
		var form_action;
		var param_name;
		//TODO inputs
		Array_each(inputs, function(input, i){
			if (input.type == "checkbox") {
				if (input.name.match(/^kt_pk/)) { // found one of our checkboxes
					input.parent_div = parent_div;
					////////////////////
					//	STEP 3.5 : add delete link on <a> tags
					////////////////////
					// fix links on delete links
					// also get form_action and param_name from first edit link - not anymore
					var parent_row = utility.dom.getParentByTagName(input, 'TR');
					//var td = Array_last(parent_row.getElementsByTagName('TD'));
					Array_each(parent_row.getElementsByTagName('A'), function(a, i){
						if (!form_action) {
							if (Array_indexOf(utility.dom.getClassNames(a), 'KT_edit_link') >= 0) {
								var href, parts;
								href = a.href;
								if (href.indexOf("?") != -1) {
									parts = href.split("?");
									form_action = parts[0];
									param_name = parts[1];
									param_name = param_name.replace(/&amp;/, '&');
									param_name = param_name.replace(/[^&]*=[^&]*&KT_back=1/g, '').replace('&$', '');
									//param_name = param_name.replace(/=.*$/, '');
								}
							}
						}
						if (a.href.toString().match(/#delete/)) { // delete link
							a.onclick = nxt_list_delete_link_row;
						}
						if (a.href.toString().match(/#move_up/)) { // move up link
							a.onclick = nxt_list_move_link_row_up;
						}
						if (a.href.toString().match(/#move_down/)) { // move down link
							a.onclick = nxt_list_move_link_row_down;
						}
						
					});
				} else if (input.name == 'KT_selAll'){ // mass checkbox
					input.parent_div = parent_div;
					input.onclick = nxt_list_cbxmass_onchange;
				}
			}
		});

		////////////////////
		//	STEP 4 : add click / mouseover / mouseout for tr
		////////////////////
		var table = utility.dom.getElementsBySelector('table.KT_tngtable', elem)[0];
		var rows = table.getElementsByTagName('tr');
		var num_rows = 0;
		//TODOrows
		Array_each(rows, function(row, i) {
			var classes = utility.dom.getClassNames(row);
			if (Array_indexOf(['KT_row_order', 'KT_row_filter'], classes[0]) < 0) {
				if (row.cells.length > 1) {
					num_rows++;
				}
				if (typeof $NXT_LIST_SETTINGS.row_effects != 'undefined' && $NXT_LIST_SETTINGS.row_effects) {
					/* mouseover */
					utility.dom.attachEvent(row, 'mouseover', function(e) {
						var o = utility.dom.setEventVars(e);
						var tr = utility.dom.getParentByTagName(o.targ, 'tr');
						if (tr.getAttribute('moved') != '1') {
							tr.setAttribute('oldBackgroundColor', tr.style.backgroundColor);
							if (typeof $nxt_tr_over_color != 'undefined') {
								tr.style.backgroundColor = $nxt_tr_over_color;
							}
						}
						//utility.dom.classNameAdd(tr, 'KT_over');
						utility.dom.stopEvent(o.e);
						return false;
					});
					/* mouseout */
					utility.dom.attachEvent(row, 'mouseout', function(e) {
						var o = utility.dom.setEventVars(e);
						var tr = utility.dom.getParentByTagName(o.targ, 'tr');
						if (tr.getAttribute('moved') != '1') {
							tr.style.backgroundColor = tr.getAttribute('oldBackgroundColor');
						}
						//utility.dom.classNameRemove(tr, 'KT_over');
						utility.dom.stopEvent(o.e);
						return false;
					});
				}
				/* click */
				utility.dom.attachEvent(row, 'click', function(e) {
					var o = utility.dom.setEventVars(e);
					var tr = utility.dom.getParentByTagName(o.targ, 'tr');
					var inputs = utility.dom.getElementsByTagName(tr, "INPUT");
					Array_each(inputs, function(input, i){
						if (input.type == "checkbox" && input.name.match(/^kt_pk/)) {
							if (o.targ != input) {
								var tmp = input.checked;
								input.checked = !tmp;
							}
							var state = true;
							var mass = null;
							Array_each(utility.dom.getElementsByTagName(input.parent_div, "INPUT"), function(input2, i){
								if (input2.type == 'checkbox' && typeof input2.parent_div != 'undefined') {
									if (input2.name != 'KT_selAll') {
										state = state && input2.checked;
										nxt_list_set_checkbox_state(input2);
									} else {
										mass = input2;
									}
								}
							});
							if (mass) {
								mass.checked = state;
							}
						}
					});
					return false;
				});
			}
			row = null;
		});
		
		if (num_rows == 0) {
			//hide buttons
			Array_each(['KT_topbuttons', 'KT_bottombuttons'], function(cname) {
				var container = utility.dom.getElementsByClassName(document.body, cname, 'div');
				if (container.length == 1) {
					var links = utility.dom.getElementsByTagName(container[0], 'a');
					Array_each(links, function(el) {
                        if (typeof el.onclick != 'undefined' && el.onclick != null && el.onclick) {
                            if(el.onclick.toString().indexOf('edit_link') >= 0 
                            ||  el.onclick.toString().indexOf('delete_link') >= 0 ) {
                                el.style.display = 'none';
                            }
                        }
					})
				}
			
			})
		}
		var op_link = utility.dom.getElementsBySelector('a.KT_move_op_link');
		for (var i =0; i < op_link.length; i++) {
			op_link[i].style.display = 'none';

			var inp = utility.dom.getNextSiblingByTagName(op_link[i], 'input');
			if (inp != null) {
				inp.style.display = 'none';
			}
		}

		if (typeof $NXT_LIST_SETTINGS.record_counter != "undefined"
			&& $NXT_LIST_SETTINGS.record_counter) {
			var first_data_row = nxt_list_first_data_row(table);
			for (var i = 0, idx=1; i < parent_div.rows.length; i++) {
				var row = parent_div.rows[i];
				if (row.cells.length == 1) {
					row.cells[0].colSpan = parseInt(row.cells[0].colSpan, 10) + 1;
					return;
				}
				if (!i) {
					var td = document.createElement('th');
					td.innerHTML = 'No.';
				} else {
					var td = document.createElement('td');
					if (i >= first_data_row) {
						td.innerHTML = idx++ + $NAV_Text_start - 1;
					} else {
						td.innerHTML = '&nbsp;';
					}
				}
				utility.dom.insertAfter(td, row.cells[0]);
			}
		}

		var tds = utility.dom.getElementsByClassName(
			parent_div.getElementsByTagName('tbody')[0], 'KT_order');
		if (nxt_list_last_data_row(table) != nxt_list_first_data_row(table)) {
			nxt_hide_move_link(tds[0], 1);
			nxt_hide_move_link(tds[tds.length-1], 0);
		} else {
			nxt_hide_move_link(tds[0], '*');
		}

		var forms = utility.dom.getElementsByTagName(elem, 'FORM');
		var good_form = []
		Array_each(forms, function(form) {
			var parent = utility.dom.getParentByTagName(form, 'div');
			if (parent.className.match(/KT_tng(list|form)/)) {
				Array_push(good_form, form);
			}
		});
		if (good_form.length == 1) {
			form = good_form[0];
			form.form_action = form_action;
			form.param_name = param_name;
		}
	});
}

function nxt_hide_move_link(td, link_index) {
	if(typeof td == 'undefined') {
		return true;
	}
	var buttons = [];
	if ($NXT_LIST_SETTINGS.show_as_buttons) {
		Array_each(td.getElementsByTagName('input'), function (input, idx) {
			if (input.type == 'button') {
				Array_push(buttons, input);
			}
		})
		if (buttons.length == 0) {
			buttons = td.getElementsByTagName('a')
		}
	} else {
		buttons = td.getElementsByTagName('a')
	}
	for (var i = 0; i < buttons.length; i++) {
		if (link_index == '*') {
			buttons[i].style.visibility = 'hidden';
		} else {
			if (i == link_index) {
				buttons[i].style.visibility = 'hidden';
			} else {
				buttons[i].style.visibility = 'visible';
			}
		}
	}
}


function nxt_list_attach_timeout() {
	if (typeof $style_executed == 'undefined') {
		window.setTimeout('nxt_list_attach_timeout()', 50);
		return;
	}
	nxt_list_attach();
}


function nxt_list_detach() {
	function nullifyProperties(object) {
		for(var prop in object) {
			if(typeof prop == "object")
				nullifyProperties(prop);
			try {
				delete object[prop]
				object[prop] = null;
			}
			catch (e) { }
		}
		try {
			delete object;
			object = null;
		} catch (e) { }
	}

	var containers = utility.dom.getElementsByClassName(document, 'KT_tng', 'div');
	for (var i = 0; i < containers.length; i++) {
		var container = containers[i];
		var nodes = utility.dom.getElementsByTagName(container, '*');
		for (var k = 0; k < nodes.length; k++) {
			utility.dom.stripAttributes(nodes[k], ['parent_div']);
		}
		utility.dom.stripAttributes(containers[i], ['parent_div']);
	}
	var tables = utility.dom.getElementsByClassName(document, 'KT_tngtable', 'table');
	for (var i = 0; i < tables.length; i++) {
		var table = tables[i];
		var nodes = utility.dom.getElementsByTagName(container, '*');
		for (var k = 0; k < nodes.length; k++) {
			utility.dom.stripAttributes(nodes[k], ['parent_div']);
		}
		utility.dom.stripAttributes(tables[i], ['parent_div']);
	}
	if (document.all) {
		nullifyProperties(window);
	}
	if (window.CollectGarbage) {
		window.CollectGarbage();
	}
}

utility.dom.attachEvent2(window, 'onload', nxt_list_attach);
window.onbeforeunload = nxt_list_check_changed_unload;

utility.dom.attachEvent2(window, 'onunload', nxt_list_detach);
