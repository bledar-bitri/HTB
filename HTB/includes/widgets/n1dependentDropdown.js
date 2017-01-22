// Copyright 2001, 2003 InterAKT Online. All rights reserved.
var n1ddAll = new Array();
function ddAddEvent(oldHandler,newHandler) {
	var me = function () {
		newHandler();
		if (oldHandler) {
			oldHandler();
		}
	}
	return me;
}
window.onload = ddAddEvent(window.onload,n1updateMenus);

function registerN1Menu(detailList, boundTo) {
	if (document.getElementById) {
		var detailListO = document.getElementById(detailList);
		detailListO.boundTo = boundTo;
		n1ddAll[detailList] = 1;
	}
}

function n1updateMenu(detailList, defaultValue, boundTo) {
	var detailListArr;
	if (!detailList || !boundTo) {
		detailListArr = this.detailList;
		boundTo = this.name;
	} else {
		detailListArr = new Array();
		detailListArr[detailListArr.length]=detailList;
	}

	for (detailList in detailListArr) {
		detailList = detailListArr[detailList];
		
		var detailListO = document.getElementById(detailList);
		var boundToO = document.getElementById(boundTo);

		var ddfks = eval('ddfks_' + detailList);
		var ddnames = eval('ddnames_' + detailList);

		if (defaultValue) {
			for (i=0;i<boundToO.options.length;i++) {
				if (boundToO.options[i].value == ddfks[defaultValue]) {
					boundToO.selectedIndex = i;
					break;
				}
			}
		}

		var n1sel = ddfks[boundToO.options[boundToO.selectedIndex].value];
		detailListO.value=ddnames[n1sel];
	}
}

function n1updateMenus() {
	for(i in n1ddAll) {
		//also set detail lists for elements as now they are initialized
		var detailO = document.getElementById(i);
		var boundTo = detailO.boundTo;
		var boundToO = document.getElementById(boundTo);
		
		if (!boundToO.detailList) {
			boundToO.detailList = new Array()
			boundToO.onchange = n1updateMenu;
			boundToO.onkeyup = n1updateMenu;
			boundToO.onmouseup = n1updateMenu;
		}
		boundToO.detailList[boundToO.detailList.length] = i;
		var dddefval = eval('dddefval_' + i);
		n1updateMenu(i, dddefval, boundTo);
	}	
}
