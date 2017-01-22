function nxt_form_attach() {
	var tmp;
	var scroll = utility.dom.getPageScroll();
	var size = utility.dom.getPageInnerSize();
	tmp = utility.dom.getElementsByClassName(document, 'KT_tng', 'DIV');
	Array_each(tmp, (function(elem){
		////////////////////
		//	STEP 0: add the float class to the div
		////////////////////
		var footer = utility.dom.getElementsByClassName(elem, 'KT_bottombuttons');
		if (footer.length == 1) {
			footer = footer[0];
			var elementList = utility.dom.getElementsByClassName(footer, 'KT_operations');
			Array_each(elementList, function(element) {
				 utility.dom.classNameAdd(element, 'KT_left');
			 });
		}
		////////////////////
		//	STEP 1:  copy footer buttons to header
		////////////////////
		if ($NXT_FORM_SETTINGS.duplicate_buttons && !(is.ie && is.mac)) {
			var footer = utility.dom.getElementsByClassName(elem, 'KT_bottombuttons', 'DIV');
			if (footer.length == 1) {
				footer = footer[0];
				var header = document.createElement('DIV');
				header.className = 'KT_topbuttons';
				header.innerHTML = footer.innerHTML;
				var tmp = footer.parentNode.insertBefore(header, footer.parentNode.firstChild);
				Array_each(['input'], function(tagname) {
					var from = footer.getElementsByTagName(tagname);
					var to = header.getElementsByTagName(tagname);
					Array_each(from, function(asd, i) {
						to[i].onclick = from[i].onclick;
						to[i].onfocus = from[i].onfocus;
					});
				});
				//utility.dom.classNameAdd(utility.dom.getElementsByClassName(tmp, 'KT_operations'), 'clearfix');
			} else {
				footer = null;
				header = null
			}
		}

		if (typeof $NXT_FORM_SETTINGS.merge_down_value != 'undefined' && $NXT_FORM_SETTINGS.merge_down_value && !(is.ie && is.mac)) {
			if (typeof multiple_edits != "undefined") {
				if (!multiple_edits) {
					return;
				}
			} else {
				var tables = utility.dom.getElementsBySelector('div.KT_tngform table.KT_tngtable');
				if (tables.length == 1) {
					return;
				}
			}
			var labels = utility.dom.getElementsByTagName(elem, 'label'); var visited_labels = [];
			Array_each(labels, function(label) {
				var normal = label.htmlFor.toString().replace(/_\d+$/, '');
				var first = document.getElementById(normal+'_1');
				if (first.tagName.toLowerCase() == 'input' && first.type && first.type.toLowerCase() == 'file') {
					return;
				}
				if (Array_indexOf(visited_labels, normal) < 0) { // it's the first label
					var normal_re = new RegExp('^' + normal, 'g');
					Array_push(visited_labels, normal);
					var elem = utility.dom.createElement('input', {
						'type': 'button', 
						'className': 'merge_down', 
						'value': 'v', 
						'tabindex': 1000
					});
					elem.tabIndex = 1000;
					//elem.className = 'merge_down';
					elem.onclick = function(e) {
						var first = document.getElementById(normal+'_1'), elements_to = [];
						var form = first.form;
						
						Array_each(form.elements, function(input) {
							if (input.id.toString().match(normal_re) && input.id != normal+'_1') {
								Array_push(elements_to, input);
							}
						})
						Array_each(elements_to, function(element_to) {
							if (first.tagName.toLowerCase() == 'input' && first.type == 'checkbox') {
								try { element_to.checked = first.checked; } catch(e) { }
								return true;
							}
							if (first.tagName.toLowerCase() == 'select') {
								try { element_to.selectedIndex = first.selectedIndex; } catch(e) { }
								return true;
							}
							try { element_to.value = first.value; } catch(e) { }

							var ktml1 = UNI_isktml(first);
							var ktml2 = UNI_isktml(element_to);
							if (ktml1 && ktml2) {
								if (ktml2.displayMode == 'RICH') {
									ktml2.setContent(ktml1.getContent());
								} else {
									ktml2.textarea.value = hndlr_load(ktml1.getContent(), "CODE");
								}
							}
							
						})
					}
					var inp = document.getElementById(label.htmlFor.toString());
					//find the last element in the container element to insert the copy down button after (it should be the last)
					while(inp.nextSibling) {
						inp = inp.nextSibling;
					}
					utility.dom.insertAfter(elem, inp);
				}
			});
		}

	}));
}

function nxt_form_insertasnew(obj, var_name) {
	var frm = obj.form;
	if (is.ie && frm.action == '') {
		var action = window.location.href
	} else {
		var action = frm.action.toString();
	}
	parts = action.split("?");
	var qs = new QueryString(parts[1]); var new_qs = [];
	var re = new RegExp(var_name, 'g');
	Array_each(qs.keys, function(key, i) {
		if (! key.match(re)) {
			Array_push(new_qs, key+'='+qs.values[i]);
		}
	});
	var new_part = new_qs.join('&');
	action = parts[0];
	if (new_part != '')
		action += '?' + new_part;
	frm.action = action;
	return true;
}

utility.dom.attachEvent2(window, 'onload', nxt_form_attach);

