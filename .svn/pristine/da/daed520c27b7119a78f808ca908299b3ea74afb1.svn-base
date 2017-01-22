var Director = new Object();
var previousSelectedMainMenuIdx = -1;
var previousSelectedSubMenuIdx = -1;
var menuItemIsClicked = false;

function workBegins(clientHint) {
    if (clientHint.indexOf("dnbProgressWindow") >= 0) {
        Director.work = window.open('', 'Working', 'width=650,height=25,status=yes,top=250,left=100');
        var doc = Director.work.document;
        doc.writeln('<HTML><HEAD><TITLE>D&B wird kontaktiert</TITLE></HEAD><BODY>');
        doc.writeln('<FONT SIZE="2" FACE="Arial"><STRONG>Die Daten werden von D&B aufbereitet. Dies dauert ca. 15 - 90 Sekunden...</STRONG></FONT>');
        doc.writeln('</BODY></HTML>');
        doc.writeln('<SCRIPT>');
        doc.writeln('var startTime = new Date().getTime();');
        doc.writeln('var count = 0;');
        doc.writeln('var limit = 100;');
        doc.writeln('function tick() {');
        doc.writeln('if (count++ <= limit) {');
        doc.writeln('self.defaultStatus = Math.round((new Date().getTime()-startTime)/1000)+" Sekunden"');
        doc.writeln('setTimeout("tick()", 1000);}');
        doc.writeln('else self.close();}');
        doc.writeln('setTimeout("tick()", 1000);');
        doc.writeln('</' + 'SCRIPT' + '>');
        doc.close();
    }
}

function workEnds() {
    if (Director.work != null) {
        if (!Director.work.closed)
            Director.work.close();
        Director.work = null;
    }
}

function clickMenuItem(menuItemIdx) {
    var menuItem = Director.menu.items[menuItemIdx];

    if (menuItem.action != "") {
        if (menuItem.isMainMenu) {
            highlightMainMenuItem(menuItemIdx);
            hideSubMenu();
        }
        else {
            highlightSubMenuItem(menuItemIdx);
        }
        var url = Director.host + menuItem.action + Director.parms;
        if (menuItem.clientHint.indexOf("promptForReference") >= 0) {
            url = url + "&reference=" + prompt("Für die Zusendung von aktualisierten Berichten, die bei Abfrage in die Recherche gehen, geben Sie bitte als Referenz Ihre e-mail Adresse an.", "");
        }
        workBegins(menuItem.clientHint);
        if (menuItem.clientHint.indexOf("javaScript") >= 0) {
            eval(menuItem.action);
        }
        else {
            parent.Details.location = url;
        }
    }
    else {
        highlightMainMenuItemAndShowSubMenu(menuItemIdx);
    }
    menuItemIsClicked = true;
    showMenuItemCost(menuItemIdx);
}

function highlightMainMenuItemAndShowSubMenu(menuItemIdx) { //previewMainMenuItemSubMenu    // highlightMenuItemAndDisplaySubMenu
    var menuItem = Director.menu.items[menuItemIdx];
    highlightMainMenuItem(menuItemIdx);
    showSubMenu(menuItem.mainMenuItemIdx);
}

function previewSubMenu(menuItemIdx) {
    var menuItem = Director.menu.items[menuItemIdx];

    if (!menuItemIsClicked && menuItem.action == "") {
        highlightMainMenuItemAndShowSubMenu(menuItemIdx);
    }
}

function showPreviousSelectedMenuItemCost() {
    if (previousSelectedSubMenuIdx != -1) {
        showMenuItemCost(previousSelectedSubMenuIdx);
    }
    else {
        if (previousSelectedMainMenuIdx != -1) {
            showMenuItemCost(previousSelectedMainMenuIdx);
        }
        else {
            document.getElementById("menuPointsPlaceHolder").innerHTML = "";
        }
    }
}

function showMenuItemCost(menuItemIdx) {
    var item = Director.menu.items[menuItemIdx];
    document.getElementById("menuPointsPlaceHolder").innerHTML = item.cost;
}

function highlightMainMenuItem(menuItemIdx) {
    if (previousSelectedMainMenuIdx != -1) {
        document.getElementById("mainMenuBorderLeft" + previousSelectedMainMenuIdx).className = "mainMenuBorderLeft";
        document.getElementById("mainMenuBackground" + previousSelectedMainMenuIdx).className = "mainMenuBackground";
        document.getElementById("mainMenuItem" + previousSelectedMainMenuIdx).className = "mainMenuItem";
        document.getElementById("mainMenuBorderRight" + previousSelectedMainMenuIdx).className = "mainMenuBorderRight";
    }
    document.getElementById("mainMenuBorderLeft" + menuItemIdx).className = "mainMenuBorderLeftSelected";
    document.getElementById("mainMenuBackground" + menuItemIdx).className = "mainMenuBackgroundSelected";
    document.getElementById("mainMenuItem" + menuItemIdx).className = "mainMenuItemSelected";
    document.getElementById("mainMenuBorderRight" + menuItemIdx).className = "mainMenuBorderRightSelected";

    previousSelectedMainMenuIdx = menuItemIdx;
}

function highlightSubMenuItem(menuItemIdx) {
    if (previousSelectedSubMenuIdx != -1) {
        document.getElementById("subMenuItem" + previousSelectedSubMenuIdx).className = "subMenuItem";
    }
    document.getElementById("subMenuItem" + menuItemIdx).className = "subMenuItemSelected";
    previousSelectedSubMenuIdx = menuItemIdx;
}

function showSubMenu(mainMenuItemIdx) {
    document.getElementById("subMenuPlaceHolder").innerHTML = subMenu[mainMenuItemIdx];
    previousSelectedSubMenuIdx = -1;
}

function hideSubMenu() {
    document.getElementById("subMenuPlaceHolder").innerHTML = "&nbsp;";
    previousSelectedSubMenuIdx = -1;
}