<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BankAccountMonthlyStatus.aspx.cs" Inherits="HTB.v2.intranetx.bank.BankAccountMonthlyStatus" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>HTB.ASP [ Konto Monatlicher Status ]</title>
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
    </style>
</head>
<body>
    <ctl:header ID="hdr" runat="server" />
    <table width="100%" border="0" cellpadding="1" cellspacing="0" bgcolor="#000000">
        <tr>
            <td>
                <table width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
                    <tr>
                        <td class="smallText">
                            <a href="/v2/intranet/intranet/intranet.asp">Intranet</a> |
                            <a href="/v2/intranet/intranet/mydata.asp"> Meine Daten</a> | 
                            <a href="Bank.aspx">Bank</a> | Konto Monatlicher Status | 
                            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="javascript:window.print()">Drucken</asp:HyperLink>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" EnablePartialRendering="true" EnableScriptLocalization="true" />
    <asp:UpdatePanel ID="updPanel1" runat="server">
        <ContentTemplate>
            <table width="100%" border="0" cellspacing="0" bgcolor="#FFFFFF">
                <tr>
                    <td>
                        <table border="0" align="center" cellpadding="3" cellspacing="0">
                            <tr>
                                <td colspan="2" class="tblHeader" title="Konto Monatlicher Status">
                                    Konto Monatlicher Status
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <ctl:message ID="ctlMessage" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table border="0" align="center" cellpadding="3" cellspacing="0">
                                        <tr>
                                            <td class="EditCaptionTopLine">
                                                <div align="right">
                                                    <strong>Datum</strong>:&nbsp;</div>
                                            </td>
                                            <td class="EditDataTopLine">
                                                <asp:Label runat="server" ID="lblFrom" Text="&nbsp;vom&nbsp;"/>
                                                <asp:TextBox ID="txtDateFrom" runat="server" MaxLength="10" ToolTip="Datum" onFocus="this.style.backgroundColor='#DFF4FF'"
                                                    onBlur="this.style.backgroundColor=''" size="10" />
                                                <ajax:MaskedEditExtender ID="Datum_MaskedEditExtender" runat="server" Century="2000" TargetControlID="txtDateFrom" Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                                    OnInvalidCssClass="MaskedEditError" MaskType="Date" ErrorTooltipEnabled="True" />
                                                <ajax:MaskedEditValidator ID="Datum_MaskedEditValidator" runat="server" ControlExtender="Datum_MaskedEditExtender" ControlToValidate="txtDateFrom" InvalidValueMessage="Datum is ung&uuml;ltig!"
                                                    Display="Dynamic" InvalidValueBlurredMessage="*" />
                                                <ajax:CalendarExtender ID="Datum_CalendarExtender" runat="server" TargetControlID="txtDateFrom" PopupButtonID="Datum_CalendarButton" />
                                                <asp:ImageButton runat="Server" ID="Datum_CalendarButton" ImageUrl="~/v2/intranet/images/CalendarHS.png" AlternateText="Click to show calendar" />
                                                
                                                <asp:Label runat="server" ID="lblTo" Text="&nbsp;&nbsp;&nbsp;bis&nbsp;"/>

                                                <asp:TextBox ID="txtDateTo" runat="server" MaxLength="10" ToolTip="Datum" onFocus="this.style.backgroundColor='#DFF4FF'"
                                                    onBlur="this.style.backgroundColor=''" size="10" />
                                                <ajax:MaskedEditExtender ID="Datum_MaskedEditExtender2" runat="server" Century="2000" TargetControlID="txtDateTo" Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                                    OnInvalidCssClass="MaskedEditError" MaskType="Date" ErrorTooltipEnabled="True" />
                                                <ajax:MaskedEditValidator ID="Datum_MaskedEditValidator2" runat="server" ControlExtender="Datum_MaskedEditExtender2" ControlToValidate="txtDateTo" InvalidValueMessage="Datum is ung&uuml;ltig!"
                                                    Display="Dynamic" InvalidValueBlurredMessage="*" />
                                                <ajax:CalendarExtender ID="Datum_CalendarExtender2" runat="server" TargetControlID="txtDateTo" PopupButtonID="Datum_CalendarButton2" />
                                                <asp:ImageButton runat="Server" ID="Datum_CalendarButton2" ImageUrl="~/v2/intranet/images/CalendarHS.png" AlternateText="Click to show calendar" />

<%--                                                <strong><asp:Label ID="lblDate" runat="server" /></strong>--%>

                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right"><strong>Nur Kundenbewegungen</strong>:&nbsp;</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:CheckBox ID="chkOnlyKlient" runat="server" />
                                            </td>
                                        </tr>
                                        <tr id="trEmpty" runat="server" visible="false">
                                            <td colspan="2" class="EditData">&nbsp;</td>
                                        </tr>
                                        <tr id="trStartingBalance" runat="server" visible="false">
                                            <td class="EditCaption">
                                                <div align="right"><strong>Anfangssaldo</strong>:&nbsp;</div>
                                            </td>
                                            <td class="EditData">
                                                <strong><asp:Label ID="lblStartingBalance" runat="server" /></strong>
                                            </td>
                                        </tr>
                                        <tr id="trEmpty1" runat="server" visible="false">
                                            <td colspan="2" class="EditData">&nbsp;</td>
                                        </tr>
                                        <tr id="trPaymentsECP" runat="server" visible="false">
                                            <td class="EditCaption">
                                                <div align="right"><strong>Eingang ECP</strong>:&nbsp;</div>
                                            </td>
                                            <td class="EditData">
                                                <strong><asp:Label ID="lblPaymentsECP" runat="server" /></strong>
                                            </td>
                                        </tr>
                                        <tr id="trPaymentsClient" runat="server" visible="false">
                                            <td class="EditCaption">
                                                <div align="right"><strong>Eingang Kunden</strong>:&nbsp;</div>
                                            </td>
                                            <td class="EditData">
                                                <strong><asp:Label ID="lblPaymentsClient" runat="server" /></strong>
                                            </td>
                                        </tr>
                                        <tr id="trTransfersToClient" runat="server" visible="false">
                                            <td class="EditCaption">
                                                <div align="right">
                                                    <strong>Ausgang Kunden</strong>:&nbsp;</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:Label ID="lblTransfersToClient" runat="server" />
                                            </td>
                                        </tr>
                                        <tr id="trProvision" runat="server" visible="false">
                                            <td class="EditCaption">
                                                <div align="right"><strong>Ausgang Sonstige</strong>:&nbsp;</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:Label ID="lblOtherExpenses" runat="server" />
                                            </td>
                                        </tr>
                                        <tr id="trEmpty2" runat="server" visible="false">
                                            <td colspan="2" class="EditData">&nbsp;</td>
                                        </tr>
                                        <tr id="trTotalPayments" runat="server" visible="false">
                                            <td class="EditCaption">
                                                <div align="right"><strong>Total Eingang</strong>:&nbsp;</div>
                                            </td>
                                            <td class="EditData">
                                                <strong><asp:Label ID="lblTotalPayments" runat="server" /></strong>
                                            </td>
                                        </tr>
                                        <tr id="trTotalExpenses" runat="server" visible="false">
                                            <td class="EditCaption">
                                                <div align="right"><strong>Total Ausgang</strong>:&nbsp;</div>
                                            </td>
                                            <td class="EditData">
                                                <strong><asp:Label ID="lblTotalExpenses" runat="server" /></strong>
                                            </td>
                                        </tr>
                                        <tr id="trEmpty3" runat="server" visible="false">
                                            <td colspan="2" class="EditData">&nbsp;</td>
                                        </tr>
                                        <tr id="trMonthlyBalance" runat="server" visible="false">
                                            <td class="EditCaption">
                                                <div align="right"><strong>Bewegungen</strong>:&nbsp;</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:Label ID="lblMonthlyBalance" runat="server" />
                                            </td>
                                        </tr>
                                        <tr id="trBalance" runat="server" visible="false">
                                            <td class="EditCaption">
                                                <div align="right"><strong>Endsaldo</strong>:&nbsp;</div>
                                            </td>
                                            <td class="EditData">
                                                <strong><asp:Label ID="lblBalance" runat="server" /></strong>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">
                                                    &nbsp;</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:Button ID="btnCalculate" runat="server" class="btnSave" title="Go" Text="Go" OnClick="btnCalculate_Click" />
                                                <asp:LinkButton ID="btnNew" runat="server" title="Neu" Text="Neu" OnClick="btnNew_Click" Visible="false" />
                                                <%--<asp:LinkButton ID="btnSave" runat="server" title="Speichern" Text="Speichern" OnClick="btnSave_Click" Visible="false" />--%>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table border="0" align="center" cellpadding="3" cellspacing="0">
                                        <tr>
                                            <td valign="top" class="dataTable">
                                                <asp:GridView ID="gvTransactions" runat="server" AllowSorting="false" AutoGenerateColumns="False" CellPadding="2" CellSpacing="2" BorderStyle="Inset" Width="100%"
                                                    AlternatingRowStyle-BackColor="AntiqueWhite" BorderWidth="1" BorderColor="black" ShowFooter="true" BackColor="white" CssClass="docText">
                                                    <RowStyle />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Buchung #">
                                                            <ItemTemplate>
                                                                <%# Eval("InvoiceID")%>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" CssClass="docText" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Date" HeaderText="Datum" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="InvoiceCustInkAktId" HeaderText="Aktenzahl" />
                                                        <asp:BoundField DataField="Sender" HeaderText="Sender" />
                                                        <asp:BoundField DataField="Receiver" HeaderText="Empf&auml;nger" HtmlEncode="false"/>
                                                        <asp:TemplateField HeaderText="Betrag">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAppliedAmount" runat="server" Text='<%# Eval("AppliedAmount")%>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" CssClass="docText" />
                                                            <FooterTemplate>
                                                                <strong>
                                                                    <%= GetTotalTransactionsAmount()%>
                                                                </strong>
                                                            </FooterTemplate>
                                                            <FooterStyle CssClass="tblDataAll" HorizontalAlign="Right" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" class="tblFooter1">
                                                <div align="right">
                                                    <asp:Button ID="btnCancel" runat="server" class="btnCancel" title="Fertig" Text="Fertig" OnClick="btnCancel_Click" />
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
            <asp:HiddenField runat="server" ID="hdnStart"/>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
    <ctl:footer ID="ftr" runat="server" />
</body>
</html>
