<%@ Page Language="C#" Culture="de-DE" AutoEventWireup="true" CodeBehind="AktInkPayment.aspx.cs" Inherits="HTB.v2.intranetx.aktenink.AktInkPayment" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>HTB.ASP [ Inkassoakt buchen ]</title>
    <link href="../../intranet/styles/inksys.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
<!--

body {
	margin-left: 5px;
	margin-top: 5px;
	margin-right: 5px;
	margin-bottom: 5px;
	background-image: url(../../intranet/images/osxback.gif);
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
    <script src="../../intranet/globalcode/lib.js" type="text/javascript" language="javascript"></script>
</head>
<body>
    <ctl:headerNoMenu ID="header" runat="server" />
    <form id="form1" runat="server" defaultbutton="btnSubmit">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" EnablePartialRendering="true"  EnableScriptLocalization="true" />
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        function BeginRequestHandler(sender, args) {
            document.getElementById('<%= btnSubmit.ClientID %>').innerText = "Processing";
            document.getElementById('<%= btnSubmit.ClientID %>').disabled = true;
        }
    </script>
    <asp:UpdatePanel ID="updPanel1" runat="server">
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="txtTotalPayment" EventName="TextChanged" />
    </Triggers>
    <ContentTemplate>
        <table width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
                <tr>
                    <td>
                        <table width="632" border="0" align="center" cellpadding="3" cellspacing="0">
                            <tr>
                                <td colspan="2" class="tblHeader">
                                    INKASSOAKT Buchen
                                </td>
                            </tr>
                            <tr>
                                <td width="10%" valign="top" nowrap class="EditCaption">
                                    <div align="right">
                                        <strong>Akt Nr.&nbsp;</strong>:</div>
                                </td>
                                <td class="EditData">
                                    <%=QryInkassoakt.CustInkAktID%>
                                    <%
                                        if (QryInkassoakt.CustInkAktOldID != "")
                                        {
                                    %>
                                           [<%= QryInkassoakt.CustInkAktOldID %>]
                                    <%
                                        }
                                    %>
                                    <br>
                                    <span class="style3">
                                        <%
                                            if (!string.IsNullOrEmpty(QryInkassoakt.CustInkAktGothiaNr) )
                                            {
                                         %>
                                                <%=QryInkassoakt.CustInkAktGothiaNr%>
                                          <% } %>
                                    </span>
                                </td>
                            </tr>
                            <tr>
                                <td width="10%" nowrap class="EditCaption">
                                    <div align="right">
                                        Akt Anlagedatum:&nbsp;</div>
                                </td>
                                <td class="EditData">
                                    <%=QryInkassoakt.CustInkAktEnterDate.ToShortDateString()%>
                                </td>
                            </tr>
                            <tr>
                                <td width="10%" valign="top" nowrap class="EditCaption">
                                    <div align="right">
                                        Auftraggeber:&nbsp;</div>
                                </td>
                                <td class="EditData">
                                    <strong>
                                    <%=QryInkassoakt.AuftraggeberName1%></strong>&nbsp;<%=QryInkassoakt.AuftraggeberName2%><br>
                                    <%=QryInkassoakt.AuftraggeberStrasse%><br>
                                    <%=QryInkassoakt.AuftraggeberLKZ%>&nbsp;<%=QryInkassoakt.AuftraggeberPLZ%>&nbsp;<%=QryInkassoakt.AuftraggeberOrt%>
                                </td>
                            </tr>
                            <tr>
                                <td width="10%" valign="top" nowrap class="EditCaption">
                                    <div align="right">
                                        Klient:&nbsp;</div>
                                </td>
                                <td class="EditData">
                                    <strong>
                                    <%=QryInkassoakt.KlientName1%></strong>&nbsp;<%=QryInkassoakt.KlientName2%><br>
                                    <%=QryInkassoakt.KlientStrasse%><br>
                                    <%=QryInkassoakt.KlientLKZ%>&nbsp;<%=QryInkassoakt.KlientPLZ%>&nbsp;<%=QryInkassoakt.KlientOrt%>
                                </td>
                            </tr>
                            <tr>
                                <td width="10%" valign="top" class="EditCaption">
                                    <div align="right">
                                        Gegner:&nbsp;</div>
                                </td>
                                <td class="EditData">
                                    <p>
                                        <strong>
                                        <%=QryInkassoakt.GegnerLastName1%></strong>,&nbsp;<%=QryInkassoakt.GegnerLastName2%><br>
                                        <%=QryInkassoakt.GegnerLastStrasse%><br>
                                        <%=QryInkassoakt.GegnerLastZipPrefix%>&nbsp;<%=QryInkassoakt.GegnerLastZip%>&nbsp;<%=QryInkassoakt.GegnerLastOrt%></p>
                                    <p>
                                        &nbsp;</p>
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
                                    <%=QryInkassoakt.CustInkAktInvoiceDate.ToShortDateString()%>
                                </td>
                            </tr>
                            <tr>
                                <td class="EditCaption">
                                    <strong>Offene Forderung</strong>:&nbsp;</div>
                                </td>
                                <td class="EditData">
                                    <strong><asp:Label ID="lblBalance" runat="server" /></strong>
                                </td>
                            </tr>
                            <tr>
                                <td class="EditCaption">
                                    &nbsp;
                                </td>
                                <td class="EditData">
                                    <ctl:message ID="ctlMessage" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="EditCaption">
                                    <div align="right">
                                        <strong>Buchungsart</strong>:&nbsp;</div>
                                </td>
                                <td class="EditData">
                                    <asp:DropDownList ID="ddlPaymentType" runat="server" class="docText" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlPaymentType_SelectedIndexChanged">
                                        <asp:ListItem Text="Zahlung UW" Value="transfer" />
                                        <asp:ListItem Text="Aussendienst  Zahlung" Value="payment_to_collector" />
                                        <asp:ListItem Text="Direkt Zahlung" Value="direct_payment" />
                                        <asp:ListItem Text="Returnware" Value="return" />
                                        <asp:ListItem Text="Kosten Reduktion" Value="credit" />
                                        <asp:ListItem Text="Belastung" Value="debit" />
                                        <asp:ListItem Text="Klient Kosten" Value="expense_client" />
                                        <asp:ListItem Text="Klient Zinsen" Value="interest_client" />
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr id="trDate" runat="server">
                                <td class="EditCaption">
                                    <div align="right">
                                        <strong>Datum (Eingang)</strong>:&nbsp;</div>
                                </td>
                                <td class="EditData" valign="bottom">
                                    <asp:TextBox ID="txtDate" runat="server" MaxLength="10" ToolTip="Zahlungsdatum"
                                        onFocus="this.style.backgroundColor='#DFF4FF'" onBlur="this.style.backgroundColor=''"
                                        size="10" />
                                    <ajax:MaskedEditExtender ID="Datum_MaskedEditExtender" runat="server" Century="2000"
                                        TargetControlID="txtDate" Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                        OnInvalidCssClass="MaskedEditError" MaskType="Date" ErrorTooltipEnabled="True" />
                                    <ajax:MaskedEditValidator ID="Datum_MaskedEditValidator" runat="server" ControlExtender="Datum_MaskedEditExtender"
                                        ControlToValidate="txtDate" InvalidValueMessage="Datum is ung&uuml;ltig!" Display="Dynamic"
                                        InvalidValueBlurredMessage="*"  />
                                    <ajax:CalendarExtender ID="Datum_CalendarExtender" runat="server" TargetControlID="txtDate" PopupButtonID="Datum_CalendarButton" />
                                    <asp:ImageButton runat="Server" ID="Datum_CalendarButton" ImageUrl="~/v2/intranet/images/CalendarHS.png" AlternateText="Click to show calendar" />
                                </td>
                            </tr>
                            <tr>
                                <td class="EditCaption">
                                    <div align="right">
                                        <strong>Betrag (Eingang)</strong>:&nbsp;</div>
                                </td>
                                <td class="EditData">
                                    <asp:TextBox ID="txtTotalPayment" runat="server" AutoPostBack="true" OnTextChanged="txtTotalPayment_TextChanged" onFocus="this.style.backgroundColor='#DFF4FF'" onBlur="this.style.backgroundColor=''" />
                                </td>
                            </tr>
                            <tr id="trInvoiceBillNumber" runat="server" visible="false">
                                <td class="EditCaption">
                                    <div align="right">
                                        <strong>Beleg (Eingang)</strong>:&nbsp;</div>
                                </td>
                                <td class="EditData">
                                    <asp:TextBox ID="txtInvoiceBillNumber" runat="server" onFocus="this.style.backgroundColor='#DFF4FF'" onBlur="this.style.backgroundColor=''" />
                                </td>
                            </tr>
                            <tr>
                                <td class="EditCaption">
                                    <div align="right">
                                        <strong>Beschreibung</strong>:&nbsp;</div>
                                </td>
                                <td class="EditData">
                                    <asp:TextBox ID="txtComment" runat="server" TextMode="MultiLine" Columns="50" Rows="5"
                                        onFocus="this.style.backgroundColor='#DFF4FF'" onBlur="this.style.backgroundColor=''" />
                                </td>
                            </tr>
                            <tr id="trManual" runat="server">
                                <td class="EditCaption">
                                    <div align="right">
                                        <strong>Manuel Buchen</strong>:&nbsp;</div>
                                </td>
                                <td class="EditData">
                                    <asp:CheckBox ID="chkManual" runat="server" OnCheckedChanged="chkManual_CheckedChanged"
                                        AutoPostBack="true" />
                                </td>
                            </tr>
                            <tr id="trClientAmount" runat="server" visible="false">
                                <td class="EditCaption">
                                    <div align="right">
                                        <strong>Klient &euro;</strong>:&nbsp;</div>
                                </td>
                                <td class="EditData">
                                    <asp:TextBox ID="txtClientAmount" runat="server" onFocus="this.style.backgroundColor='#DFF4FF'"
                                        onBlur="this.style.backgroundColor=''" />
                                </td>
                            </tr>
                            <tr id="trCollectionAmount" runat="server" visible="false">
                                <td class="EditCaption">
                                    <div align="right">
                                        <strong>ECP &euro;</strong>:&nbsp;</div>
                                </td>
                                <td class="EditData">
                                    <asp:TextBox ID="txtCollectionAmount" runat="server" onFocus="this.style.backgroundColor='#DFF4FF'"
                                        onBlur="this.style.backgroundColor=''" />
                                </td>
                            </tr>
                            <tr id="trInstallmentsGrid" runat="server">
                                <td class="tblData1"  colspan="2">
                                    <asp:GridView ID="gvInstallments" runat="server" AllowSorting="false" AutoGenerateColumns="False"
                                        CellPadding="2" CellSpacing="2" BorderStyle="Inset" Width="100%" AlternatingRowStyle-BackColor="AntiqueWhite">
                                        <RowStyle />
                                        <Columns>
                                            <%--Hidden Column--%>
                                            <asp:TemplateField HeaderText="ID" Visible="false"> 
                                                <ItemTemplate>
                                                    <asp:Label id="lblInstallmentId" runat="server" Text='<%# Eval("InstallmentID")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="Rate Datum" DataField="RateDueDate" SortExpression="RateDueDate" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField HeaderText="Betrag" DataField="RateAmount" SortExpression="RateAmount" ItemStyle-HorizontalAlign="Right" />
                                            
                                            <asp:BoundField HeaderText="Verschoben bis" DataField="PostponeTillDate" SortExpression="PostponeTillDate" >
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>

                                            <asp:BoundField HeaderText="Verschiebungsgrund" DataField="PostponeReason" SortExpression="PostponeReason" HtmlEncode="false"/>
                                            <asp:BoundField HeaderText="Verschoben vom" DataField="PostponeBy" SortExpression="PostponeBy" HtmlEncode="false"/>

                                            <asp:BoundField HeaderText="Kontonummer Kunde" DataField="BankAccountNumber" SortExpression="BankAccountNumber" ItemStyle-HorizontalAlign="Center" />

                                            <asp:TemplateField HeaderText="Betrag (Eingang)">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtReceivedAmount" runat="server" Width="80px" Text='<%#Eval("ReceivedAmount") %>' onFocus="this.style.backgroundColor='#DFF4FF'" onBlur="this.style.backgroundColor=''" style="text-align: right"/>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td class="EditCaption">
                                    <div align="right">&nbsp;</div>
                                </td>
                                <td class="EditData">
                                    <asp:Button ID="btnSubmit" runat="server" class="btnSave" Text="Buchen" OnClick="Submit_Click" />&nbsp;
                                    <input name="btnClose" id="btnClose" type="button" class="btnCancel" value="Schlie&szlig;en" onclick="window.opener.location.reload(); window.close();" />
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
