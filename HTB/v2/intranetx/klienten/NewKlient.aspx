<%@ Page Language="C#" Culture="de-DE" AutoEventWireup="true" CodeBehind="NewKlient.aspx.cs" Inherits="HTB.v2.intranetx.klienten.NewKlient" %>
<%@ Register TagPrefix="ctl" TagName="workflow" Src="~/v2/intranetx/global_files/CtlWorkflow.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>HTB.ASP [ Neuer Klient ]</title>
    <link href="../../intranet/styles/inksys.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        body
        {
            margin-left: 5px;
            margin-top: 5px;
            margin-right: 5px;
            margin-bottom: 5px;
            background-image: url(../images/osxback.gif);
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
        .style2
        {
            color: #FF0000;
            font-weight: bold;
        }
    </style>
    <script language="JavaScript" type="text/JavaScript">

        function getOrt() {
            if (form1.Ort.value == '') {
                MM_openBrWindow('../global_forms/getOrt.asp?km=' + form1.UserZIP.value, 'toolwindow', 'toolbar=yes,location=yes,status=yes,menubar=yes,scrollbars=yes,resizable=yes,width=5,height=5');
            }
            form1.Ort.select();
        }
    </script>
    <script src="../../intranet/globalcode/lib.js" type="text/javascript" language="javascript"></script>
</head>
<body id="bdy" runat="server">
    <ctl:header ID="header" runat="server" />
    <form name="form1" id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true"
        EnablePartialRendering="true" EnableScriptLocalization="true" />
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        function BeginRequestHandler(sender, args) {
            document.getElementById('<%= btnSubmit.ClientID %>').innerText = "Processing";
            document.getElementById('<%= btnSubmit.ClientID %>').disabled = true;
        }
    </script>
    <asp:UpdatePanel ID="updPanel1" runat="server">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="txtZIP" EventName="TextChanged" />
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
                                            <td colspan="2" class="tblHeader" title="NEUER KLIENT ">
                                                NEUER KLIENT
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" class="tblDataAll">
                                                <ctl:message ID="ctlMessage" runat="server"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">
                                                    Neue ID:</div>
                                            </td>
                                            <td class="EditData">
                                                *wird automatisch vergeben*
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">
                                                    Klienttyp:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:DropDownList ID="ddlClientType" runat="server" class="docText" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">
                                                    Zuordnung:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:CheckBox ID="chkInkasso" runat="server" />
                                                CollectionInvoice&nbsp;&nbsp;&nbsp;
                                                <asp:CheckBox ID="chkDetektei" runat="server" />
                                                Detektei
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">
                                                    Anrede:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox ID="txtAnrede" runat="server" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';"
                                                    onblur="this.style.backgroundColor='';" size="50" MaxLength="100" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">
                                                    Titel:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox ID="txtTitel" runat="server" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';"
                                                    onblur="this.style.backgroundColor='';" size="50" MaxLength="100" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">
                                                    Name:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox ID="txtName1" runat="server" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';"
                                                    onblur="this.style.backgroundColor='';" size="50" MaxLength="100" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                &nbsp;
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox ID="txtName2" runat="server" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';"
                                                    onblur="this.style.backgroundColor='';" size="50" MaxLength="100" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">
                                                    &nbsp;&nbsp;</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox ID="txtName3" runat="server" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';"
                                                    onblur="this.style.backgroundColor='';" size="50" MaxLength="100" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">
                                                    Strasse:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox ID="txtStrasse" runat="server" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';"
                                                    onblur="this.style.backgroundColor='';" size="50" MaxLength="100" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td nowrap class="EditCaption">
                                                <div align="right">
                                                    LKZ / PLZ / Ort:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox ID="txtLKZ" runat="server" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';"
                                                    onblur="this.style.backgroundColor='';" size="3" MaxLength="3"  Text="A"/>
                                                <asp:TextBox ID="txtZIP" runat="server" AutoPostBack="true" OnTextChanged="txtZip_TextChanged"
                                                    class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';"
                                                    size="12" MaxLength="12" />
                                                <asp:TextBox ID="txtOrt" runat="server" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';"
                                                    onblur="this.style.backgroundColor='';" size="50" MaxLength="50" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">Land:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:DropDownList ID="ddlUserState" runat="server" class="docText"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">
                                                    Ansprech (alt):
                                                </div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox ID="txtAnsprech" runat="server" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';"
                                                    onblur="this.style.backgroundColor='';" size="50" MaxLength="100" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption"><div align="right">Telefon:</div></td>
                                            <td nowrap class="EditData">+
                                                <asp:TextBox ID="txtPhoneCountry" runat="server" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="5" maxlength="5" />
                                                (
                                                <asp:TextBox ID="txtPhoneCity" runat="server" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="10" maxlength="10" />
                                                )              
                                                <asp:TextBox ID="txtPhone" runat="server" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="50" maxlength="50" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption"><div align="right">Fax:</div></td>
                                            <td nowrap class="EditData">+
                                                <asp:TextBox ID="txtFaxCountry" runat="server" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="5" maxlength="5" />
                                                (
                                                <asp:TextBox ID="txtFaxCity" runat="server" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="10" maxlength="10" />
                                                )              
                                                <asp:TextBox ID="txtFax" runat="server" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="50" maxlength="50" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">
                                                    E-Mail:
                                                </div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox ID="txtEMail" runat="server" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';"
                                                    onblur="this.style.backgroundColor='';" size="30" MaxLength="100" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">
                                                    Firmenbuchnummer:
                                                </div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox ID="txtFirmenbuchnummer" runat="server" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="50" MaxLength="100" />
                                            </td>
                                        </tr>

                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">
                                                    Versicherung:
                                                </div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox ID="txtVersicherung" runat="server" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="50" MaxLength="100" />
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Polizzennummer:&nbsp;&nbsp;
                                                <asp:TextBox ID="txtPolizzennummer" runat="server" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="50" MaxLength="100" />
                                            </td>
                                        </tr>

                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">
                                                    Benachrichtigung mittels:
                                                </div>
                                            </td>
                                            <td class="EditData">
                                                <asp:DropDownList runat="server" ID="ddlKlientNachricht" class="docText">
                                                    <asp:ListItem Value="2">E-Mail</asp:ListItem>
                                                    <asp:ListItem Value="1">Fax</asp:ListItem>
                                                    <asp:ListItem Value="3">Brief</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">
                                                    Klient erhält Zinsen:
                                                </div>
                                            </td>
                                            <td class="EditData">
                                                <asp:CheckBox ID="chkKlientReceivesInterest" runat="server" class="docText" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">Rechtsanwalt:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:DropDownList ID="ddlLawyer" runat="server" />
                                            </td>
                                        </tr>
                                        
                                        <tr>
                                            <td colspan="2">
                                                <img name="" src="" width="1" height="1" alt="" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" class="tblHeader">
                                                Kundenportal
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">
                                                    Benutzername:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox ID="txtUserName" runat="server" class="docText" size="50"  />                                                
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">
                                                    Passwort:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox ID="txtPassword" runat="server" class="docText" size="50" />
                                            </td>
                                        </tr>
                                        
                                        <tr>
                                            <td colspan="2">
                                                <img name="" src="" width="1" height="1" alt="" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" class="tblHeader">
                                                Akquise
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">
                                                    Kontakter:</div>
                                            </td>
                                            <td class="EditData">
                                                <img src="../../intranet/images/edituser16.gif" alt="" width="16" height="16" border="0"
                                                    align="middle" />&nbsp;
                                                <asp:TextBox ID="txtContacter" runat="server" class="docText" value="kein" size="50"
                                                    Enabled="false" />
                                                <input type="button" name="btnContacter" id="btnContacter" class="smallText" onclick="MM_openBrWindow('../../intranet/global_forms/userbrowserKLIENT.asp?retID=hdnContacterID&amp;retName=txtContacter&amp;type=1','ContacterBrowser','scrollbars=yes,resizable=yes,width=600,height=600')"
                                                    value="..." />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">
                                                    Werber:</div>
                                            </td>
                                            <td class="EditData">
                                                <img src="../../intranet/images/edituser16.gif" alt="" width="16" height="16" border="0"
                                                    align="middle" />&nbsp;
                                                <asp:TextBox ID="txtSalesPromoter" runat="server" class="docText" value="kein" size="50"
                                                    Enabled="false" />
                                                <input type="button" name="btnSalesPromoter" id="btnSalesPromoter" class="smallText"
                                                    onclick="MM_openBrWindow('../../intranet/global_forms/userbrowserKLIENT.asp?retID=hdnSalesPromoterID&amp;retName=txtSalesPromoter&amp;type=1','SalesPromoterBrowser','scrollbars=yes,resizable=yes,width=600,height=600')"
                                                    value="..." />
                                            </td>
                                        </tr>
                                        
                                        <tr>
                                            <td colspan="2">
                                                <img name="" src="" width="1" height="1" alt="" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" class="tblHeader" title="Bankverbindung Klient">
                                                Bankverbindung Klient
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">
                                                    BLZ:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox ID="txtBLZ1" runat="server" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';"
                                                    onblur="this.style.backgroundColor='';" size="20" MaxLength="20" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">
                                                    Kontonummer:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox ID="txtKTO1" runat="server" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';"
                                                    onblur="this.style.backgroundColor='';" size="50" MaxLength="50" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">
                                                    Bank:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox ID="txtBank1" runat="server" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';"
                                                    onblur="this.style.backgroundColor='';" size="50" MaxLength="100" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <img name="" src="" width="1" height="1" alt="" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" class="tblHeader" title="Bankverbindung CollectionInvoice">
                                                Bankverbindung CollectionInvoice
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">
                                                    BLZ:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox ID="txtBLZ2" runat="server" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';"
                                                    onblur="this.style.backgroundColor='';" size="20" MaxLength="20" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">
                                                    Kontonummer:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox ID="txtKTO2" runat="server" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';"
                                                    onblur="this.style.backgroundColor='';" size="50" MaxLength="50" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">
                                                    Bank:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox ID="txtBank2" runat="server" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';"
                                                    onblur="this.style.backgroundColor='';" size="50" MaxLength="100" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <img name="" src="" width="1" height="1" alt="" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" class="tblHeader" title="Treuhandkonto 2">
                                                Treuhandkonto 2
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">
                                                    BLZ:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox ID="txtBLZ3" runat="server" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';"
                                                    onblur="this.style.backgroundColor='';" size="20" MaxLength="20" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">
                                                    Kontonummer:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox ID="txtKTO3" runat="server" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';"
                                                    onblur="this.style.backgroundColor='';" size="50" MaxLength="50" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">
                                                    Bank:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox ID="txtBank3" runat="server" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';"
                                                    onblur="this.style.backgroundColor='';" size="50" MaxLength="100" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <ctl:message ID="ctlMessageWorkflow" runat="server"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="3">
                                                <ctl:workflow runat="server" ID="ctlWorkflow" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                        <ctl:message ID="ctlMessage2" runat="server"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tblHeader" colspan="2" title="Sonstiges">
                                                Sonstiges
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption" valign="top">
                                                <div align="right">
                                                    Beschreibung:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox ID="txtMemo" runat="server" class="docText" Columns="80" onblur="this.style.backgroundColor='';"
                                                    onfocus="this.style.backgroundColor='#DFF4FF';" Rows="8" TextMode="MultiLine" />
                                                <br />
                                                <input name="Submit3" type="button" class="btnStandard" value="Zeitstempel" title="Hier können Sie den Zeitstempel setzten."
                                                    onclick="GBInsertTimeStamp(document.forms(0).txtMemo,'<%=Session["MM_VorName"] + " " + Session["MM_NachName"] + " " + DateTime.Now %>');">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tblFooter1" colspan="2">
                                                <div align="right">
                                                    <asp:Button ID="btnSubmit" runat="server" class="btnSave" OnClick="btnSubmit_Click"
                                                        Text="Speichern" title="Speichern" />
                                                    <asp:Button ID="btnSubmit2" runat="server" class="btnCancel" OnClick="btnCancel_Click"
                                                        Text="Abbrechen" title="Abbrechen" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                        <p>
                            <br />
                        </p>
                    </td>
                </tr>
            </table>
            </td> </tr> </table>
            <asp:HiddenField ID="hdnContacterID" runat="server" Value="0" />
            <asp:HiddenField ID="hdnSalesPromoterID" runat="server" Value="0" />
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
    <ctl:footer ID="ftr" runat="server" />
</body>
</html>
