<%@ Page Language="C#" Culture="de-DE" AutoEventWireup="true" CodeBehind="PostponeRate.aspx.cs" Inherits="HTB.v2.intranetx.aktenink.PostponeRate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>HTB.ASP [ Rate verschiben ]</title>
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
        .style1
        {
            color: #FF0000;
        }
        .style3
        {
            color: #999999;
        }
        OPTION.dis
        {
            background-color: white;
            color: #999999;
        }
    </style>
</head>
<body id="bdy" runat="server">
    <ctl:headerNoMenu ID="hdr" runat="server" />
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" EnablePartialRendering="true" EnableScriptLocalization="true" />
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        function BeginRequestHandler(sender, args) {
            document.getElementById('<%= btnSubmit.ClientID %>').innerText = "Processing";
            document.getElementById('<%= btnSubmit.ClientID %>').disabled = true;
        }
    </script>
    <asp:UpdatePanel ID="updPanel1" runat="server">
        <Triggers>
            
        </Triggers>
        <ContentTemplate>
            <table width="632" border="0" align="center" cellpadding="3" cellspacing="0">
                <tr>
                    <td colspan="2" class="tblHeader">
                        Rate verschiben
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="tblDataAll">
                        <ctl:message ID="ctlMessage" runat="server"/>
                    </td>
                </tr>
                <tr>
                    <td width="10%" valign="top" class="EditCaption">
                        <div align="right">
                            <strong>Akt Nr.&nbsp;</strong>:</div>
                    </td>
                    <td class="EditData">
                        <asp:Label runat="server" ID="lblAktID" />
                    </td>
                </tr>
                <tr>
                    <td width="10%" class="EditCaption">
                        <div align="right">Akt Anlagedatum:&nbsp;</div>
                    </td>
                    <td class="EditData">
                        <asp:Label runat="server" ID="lblCustInkAktEnterDate" />
                    </td>
                </tr>
                <tr>
                    <td width="10%" valign="top" class="EditCaption">
                        <div align="right">Auftraggeber:&nbsp;</div>
                    </td>
                    <td class="EditData">
                        <strong><asp:Label runat="server" ID="lblAuftraggeberName1" /></strong>&nbsp;<asp:Label runat="server" ID="lblAuftraggeberName2" /><br />
                        <asp:Label runat="server" ID="lblAuftraggeberStrasse" /><br />
                        <asp:Label runat="server" ID="lblAuftraggeberLKZ" />&nbsp;<asp:Label runat="server" ID="lblAuftraggeberPLZ" />&nbsp;<asp:Label runat="server" ID="lblAuftraggeberOrt" />
                    </td>
                </tr>
                <tr>
                    <td width="10%" valign="top" class="EditCaption">
                        <div align="right">Klient:&nbsp;</div>
                    </td>
                    <td class="EditData">
                        <strong>
                            <asp:Label runat="server" ID="lblKlientName1" /></strong>&nbsp;<asp:Label runat="server" ID="lblKlientName2" /><br />
                        <asp:Label runat="server" ID="lblKlientStrasse" /><br />
                        <asp:Label runat="server" ID="lblKlientLKZ" />&nbsp;<asp:Label runat="server" ID="lblKlientPLZ" />&nbsp;<asp:Label runat="server" ID="lblKlientOrt" />
                    </td>
                </tr>
                <tr>
                    <td width="10%" valign="top" class="EditCaption">
                        <div align="right">Gegner:&nbsp;</div>
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
                            <strong>Rechnungsdatum</strong>:&nbsp;</div>
                    </td>
                    <td class="EditData">
                        <asp:Label runat="server" ID="lblCustInkAktInvoiceDate" />
                    </td>
                </tr>
                <tr>
                    <td class="EditCaption">
                        <div align="right">
                            <strong>Akt&nbsp;Workflow&nbsp;Datum</strong>:&nbsp;</div>
                    </td>
                    <td class="EditData">
                        <asp:Label runat="server" ID="lblCustInkAktNextWFLStep" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="tblDataAll">
                        <img name="" src="" width="1" height="1" alt="" />
                    </td>
                </tr>
                <tr>
                    <td class="EditCaption">
                        <div align="right">
                            <strong>Verschiben bis</strong>:&nbsp;</div>
                    </td>
                    <td class="EditData">
                        <asp:TextBox ID="txtCustInkAktRatePostponeTillDate" runat="server" MaxLength="10" ToolTip="Rate Datum" Text='<%#Eval("ReceivedDate") %>'
                            ValidationGroup="DateVG" onFocus="this.style.backgroundColor='#DFF4FF'" onBlur="this.style.backgroundColor=''"
                            size="10" />
                        <ajax:maskededitextender id="Datum_MaskedEditExtender" runat="server" century="2000"
                            targetcontrolid="txtCustInkAktRatePostponeTillDate" mask="99/99/9999" messagevalidatortip="true" onfocuscssclass="MaskedEditFocus"
                            oninvalidcssclass="MaskedEditError" masktype="Date" errortooltipenabled="True" />
                        <ajax:maskededitvalidator id="Datum_MaskedEditValidator" runat="server" controlextender="Datum_MaskedEditExtender"
                            controltovalidate="txtCustInkAktRatePostponeTillDate" invalidvaluemessage="Datum is ung&uuml;ltig!" display="Dynamic"
                            invalidvalueblurredmessage="*" validationgroup="DateVG" />
                        <ajax:calendarextender id="Datum_CalendarExtender" runat="server" targetcontrolid="txtCustInkAktRatePostponeTillDate" popupbuttonid="Datum_CalendarButton" />
                        <asp:ImageButton runat="Server" ID="Datum_CalendarButton" ImageUrl="~/v2/intranet/images/CalendarHS.png" AlternateText="Click to show calendar" />
                    </td>
                </tr>
                <tr>
                    <td class="EditCaption">
                        <div align="right">
                            <strong>Verschiebungsgrund</strong>:&nbsp;</div>
                    </td>
                    <td class="EditData">
                        <asp:TextBox runat="server" ID="txtCustInkAktRatePostponeReason" TextMode="MultiLine" Columns="80" Rows="8" class="docText" onFocus="this.style.backgroundColor='#DFF4FF';" onBlur="this.style.backgroundColor='';" />
                    </td>
                </tr>
                <tr>
                    <td class="EditCaption">
                        <div align="right">
                            <strong>Verschiben&nbsp;ohne&nbsp;Terminverlust</strong>:&nbsp;</div>
                    </td>
                    <td class="EditData">
                        <asp:CheckBox runat="server" ID="chkCustInkAktRatePostponeWithNoOverdue"  />
                    </td>
                </tr>
                <tr>
                    <td class="tblFooter1" colspan="2">
                        <div align="right">
                            <asp:Button ID="btnSubmit" runat="server" class="btnSave" OnClick="btnSubmit_Click" Text="Speichern" title="Speichern" />
                            <asp:Button ID="btnCancel" runat="server" class="btnCancel" title="Abbrechen" Text="Abbrechen" OnClick="btnCancel_Click" />
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
    <ctl:footer ID="ftr" runat="server" />
</body>
</html>
