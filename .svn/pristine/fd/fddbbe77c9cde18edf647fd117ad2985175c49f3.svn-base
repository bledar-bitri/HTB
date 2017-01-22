function openAddressCorrectionNotificationDialog() {
    var w = screen.availWidth / 2;
    var h = screen.availHeight / 2;
    var features = "width=" + w + ",height=" + h + ",";
    features += "scrollbars=1,resizable=1,status=1";
    Notice = self.open('', 'Notice', features);
    Notice.focus();
}

function showDetail(mainId) {
    this.location = "http://localhost:6438/v2/intranetx/dv/DVSearch.aspx?URL=https://www.deltavista-online.at/de/adr/AdressDetails.asp%*mainId~" + mainId;
}

//------------------------------"mouseover" the table row 
function row_over(tag_uid) {
    if (tag_uid) { tag_uid.className = "row_ovr"; };
};

//------------------------------"mouseout" the table row
function row_out(tag_uid, c_name) {
    if (tag_uid) { tag_uid.className = c_name; };
};