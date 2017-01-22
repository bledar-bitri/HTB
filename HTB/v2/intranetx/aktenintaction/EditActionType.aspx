<%@ Page Language="C#" Culture="de-DE" AutoEventWireup="true" CodeBehind="EditActionType.aspx.cs" Inherits="HTB.v2.intranetx.aktenintaction.EditActionType" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>HTB.ASP [ T&auml;tigkeit editieren ]</title>
    <link href="../../intranet/styles/inksys.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        body {
	        margin-left: 5px;
	        margin-top: 5px;
	        margin-right: 5px;
	        margin-bottom: 5px;
	        background-image: url(../images/osxback.gif);
        }
        a:link {
	        color: #CC0000;
        }
        a:visited {
	        color: #CC0000;
        }
        a:hover {
	        color: #CC0000;
        }
        a:active {
	        color: #CC0000;
        }
        .style2 {	color: #FF0000;
	        font-weight: bold;
        }
    </style>
    <script language="JavaScript" type="text/JavaScript">
        function MM_goToURL() { //v3.0
            var i, args = MM_goToURL.arguments; document.MM_returnValue = false;
            for (i = 0; i < (args.length - 1); i += 2) eval(args[i] + ".location='" + args[i + 1] + "'");
        }
    </script>
</head>
<body>
    <ctl:headerNoMenu ID="header" runat="server" />
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" EnablePartialRendering="true"  EnableScriptLocalization="true" />
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        function BeginRequestHandler(sender, args) {
            document.getElementById('<%= btnSubmit.ClientID %>').innerText = "Processing";
            document.getElementById('<%= btnSubmit.ClientID %>').disabled = true;
        }
    </script>
    <asp:UpdatePanel ID="updPanel1" runat="server">
        <ContentTemplate>

            <table width="100%" border="0" cellspacing="0" cellpadding="1">
        <tr>
            <td bgcolor="#000000">
                <table width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
                    <tr>
                        <td>
                            <p>&nbsp;</p>
                            <table border="0" align="center" cellpadding="3" cellspacing="0">
                                <tr>
                                    <td colspan="2" class="tblHeader" title="T&Auml;TIGKEIT EDITIEREN">
                                        <asp:Label ID="lblHeader" runat="server"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="EditCaption">
                                        <div align="right">Bezeichnung:</div>
                                    </td>
                                    <td class="EditData">
                                        <asp:TextBox id="txtKlientenTypCaption" runat="server" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="50" maxlength="100" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="EditCaption">
                                        <div align="right">Next Step:</div>
                                    </td>
                                    <td class="EditData">
                                        <asp:DropDownList id="ddlNextStep" runat="server" class="docText" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="EditCaption">
                                        <div align="right">Standard:</div>
                                    </td>
                                    <td class="EditData">
                                        <asp:CheckBox id="chkDefault" runat="server" class="docText" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="EditCaption">
                                        <div align="right">Ratenansuchen:</div>
                                    </td>
                                    <td class="EditData">
                                        <asp:CheckBox id="chkIsInstallment" runat="server" class="docText" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="EditCaption">
                                        <div align="right">Telefonisch:</div>
                                    </td>
                                    <td class="EditData">
                                        <asp:CheckBox id="chkIsThroughPhone" runat="server" class="docText" />
                                    </td>
                                </tr>
                                 <tr>
                                    <td class="EditCaption">
                                        <div align="right">Pers&ouml;nliches CollectionInvoice:</div>
                                    </td>
                                    <td class="EditData">
                                        <asp:CheckBox id="chkAktIntActionIsPersonalCollection" runat="server" class="docText" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="EditCaption">
                                        <div align="right">Kassierungsverpflichtend:</div>
                                    </td>
                                    <td class="EditData">
                                        <asp:CheckBox id="chkIsWithCollection" runat="server" class="docText" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="EditCaption">
                                        <div align="right">Direktzahlung (AG):</div>
                                    </td>
                                    <td class="EditData">
                                        <asp:CheckBox id="chkIsDirectPayment" runat="server" class="docText" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="EditCaption">
                                        <div align="right">Verlängerungsersuchen:</div>
                                    </td>
                                    <td class="EditData">
                                        <asp:CheckBox id="chkIsExtensionRequest" runat="server" class="docText" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="EditCaption">
                                        <div align="right">Telefon / Email erhebung:</div>
                                    </td>
                                    <td class="EditData">
                                        <asp:CheckBox id="chkIsTelAndEmailCollection" runat="server" class="docText" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="EditCaption">
                                        <div align="right">Storno:</div>
                                    </td>
                                    <td class="EditData">
                                        <asp:CheckBox id="chkIsVoid" runat="server" class="docText" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="EditCaption">
                                        <div align="right">Positiv [Statistik]:</div>
                                    </td>
                                    <td class="EditData">
                                        <asp:CheckBox id="chkIsPositive" runat="server" class="docText" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="EditCaption">
                                        <div align="right">Internal [nur f&uuml;r Office]:</div>
                                    </td>
                                    <td class="EditData">
                                        <asp:CheckBox id="chkIsInternal" runat="server" class="docText" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="EditCaption" colspan="2">
                                        <img name="" src="" width="1" height="1" alt="" />
                                    </td>
                                </tr>
                                 <tr>
                                    <td class="EditCaption">
                                        <div align="right">Auto Sicherstellung:</div>
                                    </td>
                                    <td class="EditData">
                                        <asp:CheckBox id="chkAktIntActionIsAutoRepossessed" runat="server" class="docText" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="EditCaption">
                                        <div align="right">Auto Kassierung:</div>
                                    </td>
                                    <td class="EditData">
                                        <asp:CheckBox id="chkAktIntActionIsAutoMoneyCollected" runat="server" class="docText" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="EditCaption">
                                        <div align="right">Auto Zahlung:</div>
                                    </td>
                                    <td class="EditData">
                                        <asp:CheckBox id="chkAktIntActionIsAutoPayment" runat="server" class="docText" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="EditCaption">
                                        <div align="right">Auto Zahlungsmeldung AG:</div>
                                    </td>
                                    <td class="EditData">
                                        <asp:CheckBox id="chkAktIntActionIsAutoPaymentInquiry" runat="server" class="docText" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="EditCaption">
                                        <div align="right">Auto Rechnung schreiben:</div>
                                    </td>
                                    <td class="EditData">
                                        <asp:CheckBox id="chkAktIntActionIsReceivable" runat="server" class="docText" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="EditCaption">
                                        <div align="right">Auto Negative:</div>
                                    </td>
                                    <td class="EditData">
                                        <asp:CheckBox id="chkAktIntActionIsAutoNegative" runat="server" class="docText" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" class="tblFooter1">
                                        <div align="right">
                                            <asp:Button id="btnSubmit" runat="server" class="btnSave" text="Speichern" onclick="btnSubmit_Click" />
                                            <input name="Submit2" type="button" class="btnCancel" title="Abbrechen" onclick="MM_goToURL('parent','../../intranet/aktenintaction/actiontypes.asp');return document.MM_returnValue" value="Abbrechen">
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
    <ctl:footer ID="ftr" runat="server" />
</body>
</html>
