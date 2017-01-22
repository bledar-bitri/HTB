<%@ Page Language="C#" Culture="de-DE" AutoEventWireup="true" CodeBehind="EditAktInt.aspx.cs" Inherits="HTB.v2.intranetx.aktenint.EditAktInt" %>

<%@ Register TagPrefix="ctl" TagName="workflow" Src="~/v2/intranetx/global_files/CtlWorkflow.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>HTB.ASP [ Intervention editieren ]</title>
    <link href="../../intranet/styles/inksys.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        body
        {
            margin-left: 5px;
            margin-top: 5px;
            margin-right: 5px;
            margin-bottom: 5px;
        }
        a:link
        {
            color: #CC0000;
        }
        a:visited
        {
            color: #CC0000;
        }
        a:hover
        {
            color: #CC0000;
        }
        a:active
        {
            color: #CC0000;
        }
        
        OPTION.dis
        {
            background-color: white;
            color: #999999;
        }
        
        .modalBackground
        {
            background-color: #CCCCCC;
            filter: alpha(opacity=40);
            opacity: 0.5;
        }
    </style>
    <script language="JavaScript" type="text/JavaScript">

        function newAuftraggeber() {
            MM_openBrWindow('../../intranet/auftraggeber/newAuftraggeber.asp?pop=true', 'browserwindow', 'toolbar=yes,location=yes,status=yes,menubar=yes,scrollbars=yes,resizable=yes,width=800,height=600')
        }
        function newKlient() {
            MM_openBrWindow('../klienten/NewKlient.aspx?pop=true&IdKlnControl=hidKLNID&OldKLNIdControl=txtKLN&KLNTextSection=tdKlientText&Refresh=false&NewFocus=RefKlient', 'klientwindow', 'menubar=yes,scrollbars=yes,resizable=yes,width=800,height=600')
            KLNname1 = form1.hidKLNText.Value;
            var strKlientInfos = document.createTextNode(KLNname1);
            document.getElementById("tdKlientText").innerHTML = KLNname1;
        }

        function newGegner() {
            MM_openBrWindow('../../intranet/gegner/newgegner.asp?pop=true&IdControl=hidGEGID&OldIdControl=txtGEN&TextSection=GegnerText&Refresh=false&NewFocus=RefKlient', 'gegnerwindow', 'menubar=yes,scrollbars=yes,resizable=yes,width=800,height=600')
            name1 = form1.hidGEGText.Value;
            var strGegnerInfos = document.createTextNode(name1);
            document.getElementById("GegnerText").innerHTML = name1;
        }

        function findAuftraggeber() {
            ret = window.showModalDialog("../../intranet/global_forms/auftraggeberbrowsernew2.asp?retID=AGN&amp;retName=AGNName", 1, "status:yes; resizable:yes; dialogHeight=750px");
            if (typeof ret != "undefined") {
                document.getElementById('txtAGN').value = ret;
                __doPostBack("ctl00_content_txtAGN", "")
            }
        }

        function findKlient() {

            ret = window.showModalDialog("../../intranet/global_forms/klientbrowsernew2.asp?retID=KLN&amp;retName=KLName", 1, "status:yes; resizable:yes; dialogWidth=600px; dialogHeight=750px");
            if (typeof ret != "undefined") {
                document.getElementById('txtKLN').value = ret;
                __doPostBack("ctl00_content_txtKLN", "")
            }
        }

        function findGegner() {
            ret = window.showModalDialog("../../intranet/global_forms/gegnerbrowsernew2.asp?retID=GEN&amp;retName=GEName", 1, "status:yes; resizable:yes; dialogHeight=750px");
            if (typeof ret != "undefined") {
                document.getElementById('txtGEN').value = ret;
                __doPostBack("ctl00_content_txtGEN", "")
            }
        }
    </script>
    <script src="../../intranet/globalcode/lib.js" type="text/javascript" language="javascript"></script>
</head>
<body>
    <ctl:headerNoMenu ID="header" runat="server" />
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" EnablePartialRendering="true" EnableScriptLocalization="true" />
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        function BeginRequestHandler(sender, args) {
            document.getElementById('<%= btnSubmit.ClientID %>').innerText = "Processing";
            document.getElementById('<%= btnSubmit.ClientID %>').disabled = true;
        }
    </script>
    <table width="100%" border="0" cellpadding="1" cellspacing="0" bgcolor="#000000">
        <tr>
            <td>
                <table width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
                    <tr>
                        <td class="smallText">
                            <a href="../../intranet/intranet/intranet.asp">Intranet</a> | <a href="../../intranet/intranet/inkasso.asp">CollectionInvoice</a> | <a href="../../intranet/aktenint/aktenint.asp">
                                Interventionsakte (&Uuml;bersicht)</a> | Interventionsakt editieren
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <br />
    <ajax:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0">
        <ajax:TabPanel ID="TabPanel1" runat="server" HeaderText="Haupt Window">
            <ContentTemplate>
                <asp:UpdatePanel ID="updPanel1" runat="server">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="txtAGN" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtKLN" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtGEN" EventName="TextChanged" />
                    </Triggers>
                    <ContentTemplate>
                        <table width="100%" border="0" cellspacing="0" cellpadding="1">
                            <tr>
                                <td bgcolor="#000000">
                                    <table width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
                                        <tr>
                                            <td>
                                                <p>
                                                    &nbsp;</p>
                                                <table border="0" align="center" cellpadding="3" cellspacing="0">
                                                    <tr>
                                                        <td colspan="2" class="tblHeader" title="INTERVENTION EDITIEREN ">
                                                            INTERVENTION EDITIEREN
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" class="tblDataAll">
                                                            <ctl:message ID="ctlMessage" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="EditCaption">
                                                            <div align="right">
                                                                Akt Nr.:</div>
                                                        </td>
                                                        <td class="EditData">
                                                            <asp:TextBox ID="txtAktNr" runat="server" Enabled="false" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';"
                                                                size="30" MaxLength="30" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="EditCaption">
                                                            <div align="right">
                                                                Akt Typ:</div>
                                                        </td>
                                                        <td class="EditData">
                                                            <asp:DropDownList ID="ddlAktType" runat="server" class="docText" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top" class="EditCaption">
                                                            <div align="right">
                                                                Akt Anlagedatum:
                                                            </div>
                                                        </td>
                                                        <td class="EditData">
                                                            <asp:TextBox ID="txtAnlagedatum" runat="server" class="docText" onfocus="this.style.backgroundColor='#DFF4FF'; this.select();" onblur="this.style.backgroundColor=''"
                                                                size="25" MaxLength="50" />
                                                            <ajax:MaskedEditExtender ID="Datum_MaskedEditExtender" runat="server" Century="2000" TargetControlID="txtAnlagedatum" Mask="99/99/9999" MessageValidatorTip="true"
                                                                OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" MaskType="Date" ErrorTooltipEnabled="True" />
                                                            <ajax:MaskedEditValidator ID="Datum_MaskedEditValidator" runat="server" ControlExtender="Datum_MaskedEditExtender" ControlToValidate="txtAnlagedatum" InvalidValueMessage="Datum is ung&uuml;ltig!"
                                                                Display="Dynamic" InvalidValueBlurredMessage="*" />
                                                            <ajax:CalendarExtender ID="Datum_CalendarExtender" runat="server" TargetControlID="txtAnlagedatum" PopupButtonID="AnlageDatum_CalendarButton" />
                                                            <asp:ImageButton runat="Server" ID="AnlageDatum_CalendarButton" ImageUrl="~/v2/intranet/images/CalendarHS.png" AlternateText="Click to show calendar" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top" class="EditCaption">
                                                            <div align="right">
                                                                AZ:</div>
                                                        </td>
                                                        <td class="EditData">
                                                            <asp:TextBox ID="txtAZ" runat="server" class="docText" onfocus="this.style.backgroundColor='#DFF4FF'; this.select();" onblur="this.style.backgroundColor=''" size="25"
                                                                MaxLength="50" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top" class="EditCaption">
                                                            <div align="right">
                                                                Auftraggeber:</div>
                                                        </td>
                                                        <td class="EditData">
                                                            <asp:TextBox ID="txtAGN" runat="server" AutoPostBack="true" OnTextChanged="txtAGN_TextChanged" class="docText" onfocus="this.style.backgroundColor='#DFF4FF'; this.select();"
                                                                onblur="this.style.backgroundColor=''" size="15" MaxLength="15" />
                                                            <input name="cmdAG" type="button" class="smallText" id="cmdAG" title="?" onclick="findAuftraggeber();" value="? [F11]">
                                                            <input name="cmdAGNeu" type="button" class="smallText" id="cmdAGNeu" title="neu" onclick="newAuftraggeber();" value="neu [F12]">
                                                            <br>
                                                            <asp:TextBox ID="txtAGNName" runat="server" Enabled="false" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';"
                                                                size="70" MaxLength="70" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top" class="EditCaption">
                                                            <div align="right">
                                                                Klient:</div>
                                                        </td>
                                                        <td class="EditData">
                                                            <asp:TextBox ID="txtKLN" runat="server" AutoPostBack="true" OnTextChanged="txtKLN_TextChanged" class="docText" onfocus="this.style.backgroundColor='#DFF4FF'; this.select();"
                                                                onblur="this.style.backgroundColor='';" size="15" MaxLength="15" />
                                                            <input name="cmdKL" type="button" class="smallText" id="cmdKL" title="?" onclick="findKlient();" value="? [F11]">
                                                            <input name="cmdKLNeu" type="button" class="smallText" id="cmdKLNeu" title="neu" onclick="newKlient();" value="neu [F12]">
                                                            <span class="docText"></span>
                                                            <input name="hidKLNID" type="hidden" id="hidKLNID">
                                                            <input name="hidKLNText" type="hidden" id="hidKLNText">
                                                            <br>
                                                            <asp:TextBox ID="txtKLName" runat="server" Enabled="false" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';"
                                                                size="70" MaxLength="70" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top" class="EditCaption">
                                                            &nbsp;
                                                        </td>
                                                        <td class="EditData" id="tdKlientText" runat="server">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top" class="EditCaption">
                                                            <div align="right">
                                                                Gegner:</div>
                                                        </td>
                                                        <td class="EditData">
                                                            <asp:TextBox ID="txtGEN" runat="server" AutoPostBack="true" OnTextChanged="txtGEN_TextChanged" class="docText" onfocus="this.style.backgroundColor='#DFF4FF'; this.select();"
                                                                onblur="this.style.backgroundColor='';" size="15" MaxLength="15" />
                                                            <input name="cmdGE" type="button" class="smallText" id="cmdGE" title="?" onclick="findGegner();" value="? [F11]">
                                                            <input name="cmdGENeu" type="button" class="smallText" id="cmdGENeu" title="neu" onclick="newGegner();" value="neu [F12]">
                                                            <input name="hidGEGID" type="hidden" id="hidGEGID">
                                                            <input name="hidGEGText" type="hidden" id="hidGEGText">
                                                            <br>
                                                            <asp:TextBox ID="txtGEName" runat="server" Enabled="false" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';"
                                                                size="70" MaxLength="70" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top" class="EditCaption">
                                                            &nbsp;
                                                        </td>
                                                        <td class="EditData" id="GegnerText">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap class="EditCaption">
                                                            <div align="right">
                                                                Inkassant:</div>
                                                        </td>
                                                        <td class="EditData">
                                                            <asp:DropDownList ID="ddlSB" runat="server" class="docText" AutoPostBack="true" OnSelectedIndexChanged="ddlSB_SelectedIndexChanged" />
                                                            <asp:Button ID="btnFindUser" runat="server" class="smallText" title="Hier k&ouml;nnen Sie einen Inkassanten suchen." Text="find" OnClick="btnFindUser_Click" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap class="EditCaption">
                                                            <div align="right">
                                                                AZ - Kunde:</div>
                                                        </td>
                                                        <td class="EditData">
                                                            <asp:TextBox ID="txtAktIntIZ" runat="server" class="docText" onfocus="this.style.backgroundColor='#DFF4FF'; this.select();" onblur="this.style.backgroundColor=''"
                                                                size="30" MaxLength="30" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap class="EditCaption">
                                                            <div align="right">
                                                                Weg:</div>
                                                        </td>
                                                        <td class="EditData">
                                                            <asp:TextBox ID="txtWeg" runat="server" class="docText" onfocus="this.style.backgroundColor='#DFF4FF'; this.select();" onblur="this.style.backgroundColor=''" size="15"
                                                                MaxLength="15" />
                                                            km Luftlinie
                                                            <asp:Button ID="cmdCalcWeg" runat="server" class="smallText" title="Hier k&ouml;nnen Sie den Weg berechnen." Text="Berechnen" OnClick="cmdCalcWeg_Click" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap class="EditCaption">
                                                            <div align="right">
                                                                Weggeb&uuml;hr:</div>
                                                        </td>
                                                        <td class="EditData">
                                                            <asp:TextBox ID="txtCash" runat="server" class="docText" onfocus="this.style.backgroundColor='#DFF4FF'; this.select();" onblur="this.style.backgroundColor=''" size="15"
                                                                MaxLength="15" />
                                                            &euro;
                                                            <asp:Button ID="btnCash" runat="server" class="smallText" title="Hier k&ouml;nnen Sie die Weggeb&uuml;hr berechnen." Text="Berechnen" OnClick="btnCash_Click" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap class="EditCaption">
                                                            <div align="right">
                                                                Termin:</div>
                                                        </td>
                                                        <td class="EditData">
                                                            <asp:TextBox ID="txtTermin" runat="server" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="12" MaxLength="10" />
                                                            (tt.mm.jjjj)
                                                            <ajax:MaskedEditExtender ID="MaskedEditExtender1" runat="server" Century="2000" TargetControlID="txtTermin" Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                                                OnInvalidCssClass="MaskedEditError" MaskType="Date" ErrorTooltipEnabled="True" />
                                                            <ajax:MaskedEditValidator ID="MaskedEditValidator1" runat="server" ControlExtender="Datum_MaskedEditExtender" ControlToValidate="txtTermin" InvalidValueMessage="Datum is ung&uuml;ltig!"
                                                                Display="Dynamic" InvalidValueBlurredMessage="*" />
                                                            <ajax:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtTermin" PopupButtonID="TerminDatum_CalendarButton" />
                                                            <asp:ImageButton runat="Server" ID="TerminDatum_CalendarButton" ImageUrl="~/v2/intranet/images/CalendarHS.png" AlternateText="Click to show calendar" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap class="EditCaption">
                                                            <div align="right">
                                                                Termin AD:</div>
                                                        </td>
                                                        <td class="EditData">
                                                            <asp:TextBox ID="txtTerminAD" runat="server" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="12" MaxLength="10" />
                                                            (tt.mm.jjjj)
                                                            <ajax:MaskedEditExtender ID="MaskedEditExtender2" runat="server" Century="2000" TargetControlID="txtTerminAD" Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                                                OnInvalidCssClass="MaskedEditError" MaskType="Date" ErrorTooltipEnabled="True" />
                                                            <ajax:MaskedEditValidator ID="MaskedEditValidator2" runat="server" ControlExtender="Datum_MaskedEditExtender" ControlToValidate="txtTerminAD" InvalidValueMessage="Datum is ung&uuml;ltig!"
                                                                Display="Dynamic" InvalidValueBlurredMessage="*" />
                                                            <ajax:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtTerminAD" PopupButtonID="TerminAdDatum_CalendarButton" />
                                                            <asp:ImageButton runat="Server" ID="TerminAdDatum_CalendarButton" ImageUrl="~/v2/intranet/images/CalendarHS.png" AlternateText="Click to show calendar" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap class="EditCaption">
                                                            <div align="right">
                                                                SB Auftraggeber:</div>
                                                        </td>
                                                        <td class="EditData">
                                                            <asp:TextBox ID="txtAktIntSBAG" runat="server" class="docText" onfocus="this.style.backgroundColor='#DFF4FF'; this.select();" onblur="this.style.backgroundColor=''"
                                                                size="50" MaxLength="100" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap class="EditCaption">
                                                            <div align="right">
                                                                Zinsen aus Forderungen:</div>
                                                        </td>
                                                        <td class="EditData">
                                                            <asp:TextBox ID="txtZinssatz" runat="server" class="docText" onfocus="this.style.backgroundColor='#DFF4FF'; this.select();" onblur="this.style.backgroundColor=''"
                                                                size="10" MaxLength="5" />
                                                            (0.00 %)
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap class="EditCaption">
                                                            <div align="right">
                                                                Zinsen Betrag:</div>
                                                        </td>
                                                        <td class="EditData">
                                                            <asp:TextBox ID="txtZinsen" runat="server" class="docText" onfocus="this.style.backgroundColor='#DFF4FF'; this.select();" onblur="this.style.backgroundColor=''"
                                                                size="15" MaxLength="15" />
                                                            &euro;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap class="EditCaption">
                                                            <div align="right">
                                                                Kosten:</div>
                                                        </td>
                                                        <td class="EditData">
                                                            <asp:TextBox ID="txtKosten" runat="server" class="docText" onfocus="this.style.backgroundColor='#DFF4FF'; this.select();" onblur="this.style.backgroundColor=''"
                                                                size="15" MaxLength="15" />
                                                            &euro;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap class="EditCaption">
                                                            <div align="right">
                                                                Status:</div>
                                                        </td>
                                                        <td class="EditData">
                                                            <asp:DropDownList ID="ddlSatzStatus" runat="server" class="docText">
                                                                <asp:ListItem Value="0" Text="0 - Neu Erfasst" />
                                                                <asp:ListItem Value="1" Text="1 - In Bearbeitung" />
                                                                <asp:ListItem Value="2" Text="2 - Abgegeben" />
                                                                <asp:ListItem Value="3" Text="3 - Fertig" />
                                                                <asp:ListItem Value="4" Text="4 - Abgeschlossen" />
                                                                <asp:ListItem Value="5" Text="5 - Abgeschlossene Altakte" />
                                                                <asp:ListItem Value="8" Text="8 - Unter Schwellwert" />
                                                                <asp:ListItem Value="9" Text="9 - Wartet auf Bonitätsprüfung" />
                                                                <asp:ListItem Value="10" Text="10 - Storno aufgrund Bonität" />
                                                                <asp:ListItem Value="11" Text="11 - Sofortklage" />
                                                            </asp:DropDownList>
                                                            <asp:HiddenField ID="hdnOldstatus" runat="server" />
                                                            <asp:TextBox ID="txtKsvEmail" runat="server" class="docText" onfocus="this.style.backgroundColor='#DFF4FF'; this.select();" onblur="this.style.backgroundColor=''" />
                                                            &nbsp;
                                                            <asp:Button ID="btnMitteilung" runat="server" class="btnStandard" Text="Mitteilung senden ..." OnClick="btnMitteilung_Click" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap class="EditCaption">
                                                            <div align="right">
                                                                Als gedruckt kennzeichnen:</div>
                                                        </td>
                                                        <td class="EditData">
                                                            <asp:CheckBox ID="chkShowPrinted" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap class="EditCaption">
                                                            <div align="right">
                                                                Wiedervorlage:</div>
                                                        </td>
                                                        <td class="EditData">
                                                            <asp:CheckBox ID="chkWiedervorlage" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top" class="EditCaption">
                                                            <div align="right">
                                                                Beschreibung:</div>
                                                        </td>
                                                        <td class="EditData">
                                                            <strong>Auftraggeber Memo:</strong><br/>
                                                            <asp:Label runat="server" ID="lblOriginalMemo"/>
                                                            <br/>
                                                            <br/>
                                                            <asp:TextBox ID="txtMemo" runat="server" class="docText" Columns="80" onblur="this.style.backgroundColor='';" onfocus="this.style.backgroundColor='#DFF4FF';" Rows="8"
                                                                TextMode="MultiLine" />
                                                            <br>
                                                            <input name="Submit3" type="button" class="btnStandard" value="Zeitstempel" title="Hier können Sie den Zeitstempel setzten." onclick="GBInsertTimeStamp(document.forms(0).txtMemo,'<%=Session["MM_VorName"] + " " + Session["MM_NachName"] + " " + DateTime.Now %>');">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <img name="" src="" width="1" height="1" alt="">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" class="EditCaptionTopLine3">
                                                            <table width="100%" border="1" align="center" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
                                                                <tr>
                                                                    <td class="tblHeader" title="Buchungen">
                                                                        Buchungen
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <table width="100%" cellpadding="4" cellspacing="0">
                                                                            <tr>
                                                                                <td class="tblData1">
                                                                                    <asp:GridView ID="gvInvoices" runat="server" AllowSorting="false" AutoGenerateColumns="False" CellPadding="2" BorderStyle="Groove" Width="100%" ShowFooter="true">
                                                                                        <RowStyle />
                                                                                        <Columns>
                                                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                                                                <ItemTemplate>
                                                                                                    <a href="javascript:void(window.open('<%# Eval("DeletePopupUrl")%>','_blank','toolbar=no,scrollbars=yes,resizable=yes,width=800,height=800'))">
                                                                                                        <asp:Image ID="img" runat="server" ImageUrl='<%# Eval("DeleteUrl")  %>' BorderColor="White" />
                                                                                                    </a>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                                                                <ItemTemplate>
                                                                                                    <a href="javascript:void(window.open('<%# Eval("EditPopupUrl")%>','_blank','toolbar=no,scrollbars=yes,resizable=yes,width=800,height=800'))">
                                                                                                        <asp:Image ID="img" runat="server" ImageUrl='<%# Eval("EditUrl")  %>' BorderColor="White" />
                                                                                                    </a>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:HyperLinkField HeaderText="Nr." DataTextField="InvoiceID" DataNavigateUrlFields="InvoiceID" ItemStyle-HorizontalAlign="Right" DataTextFormatString="&lt;a href=javascript:MM_openBrWindow('/v2/intranetx/aktenink/ShowInvoice.aspx?InvId={0}','popSetAction','menubar=yes,scrollbars=yes,resizable=yes,width=800,height=800,top=10');&gt;{0}&lt;/a&gt;"
                                                                                                SortExpression="InvoiceID" />
                                                                                            <asp:BoundField HeaderText="Nr." DataField="PosInvoiceID" SortExpression="InvoiceID" ItemStyle-HorizontalAlign="Left" />
                                                                                            <asp:BoundField HeaderText="Datum" DataField="InvoiceDate" SortExpression="InvoiceDate" ItemStyle-HorizontalAlign="Center" />
                                                                                            <asp:BoundField HeaderText="Text" DataField="InvoiceDescription" SortExpression="InvoiceDescription" FooterText="Forderung:" FooterStyle-Font-Bold="True" FooterStyle-HorizontalAlign="Right" />
                                                                                            <asp:TemplateField HeaderText="Betrag" FooterStyle-Font-Bold="True" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                                                                                <ItemTemplate>
                                                                                                    <%# Eval("InvoiceAmount") %>
                                                                                                </ItemTemplate>
                                                                                                <FooterTemplate>
                                                                                                    <%# HTBUtilities.HTBUtils.FormatCurrency(GetTotalDue()) %>
                                                                                                </FooterTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:BoundField HeaderText="Fällig" DataField="DueDate" SortExpression="DueDate" ItemStyle-HorizontalAlign="Center" />
                                                                                        </Columns>
                                                                                    </asp:GridView>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td id="tdNewBuchung" runat="server">
                                                                                    <div align="right">
                                                                                        <input name="Submit4" type="button" class="smallText" title="Hier k&ouml;nnen Sie eine neue Buchung abstellen." onclick="MM_openBrWindow('/v2/intranetx/aktenint/EditBooking.aspx?IntAkt=<%=aktInt.AktIntID %>','popWindow','menubar=yes,scrollbars=yes,resizable=yes,width=800,height=800')"
                                                                                            value="Neue Buchung">
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="tblHeader" title="Dokumente">
                                                                        Dokumente
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <table width="100%" border="0" cellspacing="0" cellpadding="4">
                                                                            <tr>
                                                                                <td valign="top" class="tblData1">
                                                                                    <asp:GridView ID="gvDocs" runat="server" AllowSorting="false" AutoGenerateColumns="False" CellPadding="2" BorderStyle="Groove" Width="100%" ShowFooter="true">
                                                                                        <RowStyle />
                                                                                        <Columns>
                                                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                                                                <ItemTemplate>
                                                                                                    <a href="javascript:void(window.open('<%# Eval("DeletePopupUrl")%>','_blank','toolbar=no,menubar=no'))">
                                                                                                        <asp:Image ID="img" runat="server" ImageUrl='<%# Eval("DeleteUrl")  %>' BorderColor="White" />
                                                                                                    </a>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:BoundField HeaderText="Datum" DataField="DokCreationTimeStamp" SortExpression="DokCreationTimeStamp" ItemStyle-HorizontalAlign="Left" />
                                                                                            <asp:BoundField HeaderText="Typ" DataField="DokTypeCaption" SortExpression="DokTypeCaption" ItemStyle-HorizontalAlign="Left" />
                                                                                            <asp:BoundField HeaderText="Bezeichnung" DataField="DokCaption" SortExpression="DokCaption" ItemStyle-HorizontalAlign="Left" />
                                                                                            <asp:TemplateField HeaderText="Anlage" ItemStyle-HorizontalAlign="Left">
                                                                                                <ItemTemplate>
                                                                                                    <a href="javascript:void(window.open('../../intranet/documents/files/<%# Eval("DokAttachment")  %>','popSetAction','menubar=yes,scrollbars=yes,resizable=yes,width=800,height=800,top=10'))">
                                                                                                        <%# Eval("DokAttachment")  %>
                                                                                                    </a>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                        </Columns>
                                                                                    </asp:GridView>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <div align="right">
                                                                            <br>
                                                                            <input name="Submit6" type="button" class="smallText" title="Hier k&ouml;nnen Sie ein neues Dokument anlegen." onclick="MM_openBrWindow('../../intranet/documents/newaktintdok.asp?ADAktTyp=3&AktID=<%=aktInt.AktIntID %>&GegnerID=<%=aktInt.GegnerID %>&KlientID=<%=aktInt.KlientID %>','newDok','menubar=yes,scrollbars=yes,resizable=yes,width=800,height=600')"
                                                                                value="Neues Dokument">
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="tblFooter1">
                                                                        <div align="right">
                                                                            <asp:Button ID="btnSubmit" runat="server" class="btnSave" OnClick="btnSubmit_Click" Text="Speichern" title="Speichern" />
                                                                            <asp:Button ID="btnCancel" runat="server" class="btnCancel" title="Abbrechen" Text="Abbrechen" OnClick="btnCancel_Click" />
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <p>
                                                    &nbsp;</p>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </ContentTemplate>
        </ajax:TabPanel>
        <ajax:TabPanel ID="TabPanel2" runat="server" HeaderText="Workflow">
            <ContentTemplate>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table width="100%" border="0" cellspacing="0" cellpadding="1" bgcolor="#FFFFFF">
                            <tr>
                                <td>
                                    <table width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
                                        <tr>
                                            <td align="center">
                                                <ctl:workflow runat="server" ID="ctlWorkflow" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </ContentTemplate>
        </ajax:TabPanel>
    </ajax:TabContainer>
    </form>
    <ctl:footer ID="ftr" runat="server" />
</body>
</html>
