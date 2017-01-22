<%@ Page Language="C#" Culture="de-DE" AutoEventWireup="true" CodeBehind="ViewAktInk.aspx.cs" Inherits="HTB.v2.intranetx.aktenink.ViewAktInk" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>HTB.ASP [ CollectionInvoiceakt editieren ]</title>
    <link href="../../intranet/styles/inksys.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
<!--

body {
	margin-left: 5px;
	margin-top: 5px;
	margin-right: 5px;
	margin-bottom: 5px;
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
.style1 {color: #FF0000}
.style3 {color: #999999}
OPTION.dis{background-color:white; color:#999999}

-->

</style>
    <script language="JavaScript" type="text/JavaScript">
        function MM_openBrWindow(theURL, winName, features) { //v2.0
            window.open(theURL, winName, features);
        }

        function MM_goToURL() { //v3.0
            var i, args = MM_goToURL.arguments; document.MM_returnValue = false;
            for (i = 0; i < (args.length - 1); i += 2) eval(args[i] + ".location='" + args[i + 1] + "'");
        }
    </script>
</head>
<body id="bdy" runat="server">
    <ctl:headerNoMenu ID="header" runat="server" />
    <form runat="server" id="form2" method="post" action="">
    <table width="100%" border="0" cellpadding="1" cellspacing="0" bgcolor="#000000">
        <tr>
            <td>
                <table width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
                    <tr>
                        <td class="smallText">
                            <a href="../../intranet/intranet/intranet.asp">Intranet</a> <a href="../../intranet/intranet/inkasso.asp">CollectionInvoice</a> <a href="../../intranet/aktenink/AktenStaff.asp">
                                CollectionInvoiceakten (&Uuml;bersicht)</a> | CollectionInvoiceakt editieren
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
        <tr>
            <td>
                <p>
                    &nbsp;</p>
                <table width="632" border="0" align="center" cellpadding="3" cellspacing="0">
                    <tr>
                        <td colspan="2" class="tblHeader">
                            INKASSOAKT Editieren
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" class="EditCaption">
                            <div align="right">
                                Aktionen:&nbsp;</div>
                        </td>
                        <td class="EditData">
                            <img src="../../intranet/images/ic_action16_dis.gif" width="16" height="16" border="0" title="Aktion" />
                            &nbsp;&nbsp;&nbsp;Bericht senden:
                            <asp:DropDownList runat="server" ID="ddlSendBericht" Enabled="false">
                                <asp:ListItem Text="Ja" Value="1" />
                                <asp:ListItem Text="Nein" Value="0" />
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td width="10%" valign="top" class="EditCaption">
                            <div align="right">
                                <strong>Akt Nr.&nbsp;</strong>:</div>
                        </td>
                        <td class="EditData">
                            <asp:Label runat="server" ID="lblCustInkAktID"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td width="10%" class="EditCaption">
                            <div align="right">
                                Akt Anlagedatum:&nbsp;</div>
                        </td>
                        <td class="EditData">
                            <asp:Label runat="server" ID="lblCustInkAktEnterDate"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td width="10%" valign="top" class="EditCaption">
                            <div align="right">
                                Auftraggeber:&nbsp;</div>
                        </td>
                        <td class="EditData">
                            <asp:Label runat="server" ID="lblAuftraggeber"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td width="10%" valign="top" class="EditCaption">
                            <div align="right">
                                Klient:&nbsp;</div>
                        </td>
                        <td class="EditData">
                            <asp:Label ID="lblKlient" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td width="10%" valign="top" class="EditCaption">
                            <div align="right">
                                Gegner:&nbsp;</div>
                        </td>
                        <td class="EditData">
                            <p><asp:Label ID="lblGegner" runat="server" /></p>
                        </td>
                    </tr>
                    <tr>
                        <td width="10%" class="EditCaption">
                            <div align="right">
                                <strong>Kundennummer</strong>:&nbsp;</div>
                        </td>
                        <td class="EditData">
                            <asp:Label ID="lblCustInkAktGothiaNr" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td width="10%" class="EditCaption">
                            <div align="right">
                                <strong>Rechnungsnummer</strong>:&nbsp;</div>
                        </td>
                        <td class="EditData">
                            <asp:Label ID="lblCustInkAktKunde" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="EditCaption">
                            <div align="right">
                                Rechnungsdatum:&nbsp;</div>
                        </td>
                        <td class="EditData">
                            <asp:Label ID="lblCustInkAktInvoiceDate" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="EditCaption">
                            <div align="right">
                                Akt&nbsp;Workflow:&nbsp;</div>
                        </td>
                        <td class="EditData">
                            <asp:Label runat="server" ID="lblCustInkAktNextWFLStep"/>
                            &nbsp;&nbsp;
                            <asp:Label runat="server" ID="lblNetWflAction"/>
                        </td>
                    </tr>
                    <tr runat="server" ID="trAussendienst" Visible="false">
                        <td width="10%" class="EditCaption">
                            <div align="right">
                                Aussendienst:&nbsp;</div>
                        </td>
                        <td class="EditData">
                            <asp:Label ID="lblAussendienst" runat="server" />
                        </td>
                    </tr>
                    <tr runat="server" ID="trInterventionStatus" Visible="false">
                        <td width="10%" class="EditCaption">
                            <div align="right">
                                Interventionsstatus:&nbsp;</div>
                        </td>
                        <td class="EditData">
                            <asp:Label ID="lblInterventionStatus" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="EditCaption">
                            <div align="right">Rechtsanwalt:</div>
                        </td>
                        <td class="EditData">
                            <asp:DropDownList ID="ddlLawyer" runat="server" Enabled="false"/>
                        </td>
                    </tr>
                </table>
                <br />
                
            </td>
        </tr>
    </table>
    </form>
    <ctl:footer ID="ftr" runat="server" />
</body>
</html>
