<%@ Page Language="C#" Culture="de-DE" AutoEventWireup="true" CodeBehind="EditInvoice.aspx.cs" Inherits="HTB.v2.intranetx.aktenink.EditInvoice" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>HTB.ASP [ CollectionInvoice - Buchung editieren ]</title>
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
</head>
<body id="bdy" runat="server">
    <ctl:headerNoMenu runat="server" ID="hdr" />
    <form id="form1" runat="server" defaultbutton="btnSubmit">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" EnablePartialRendering="true" EnableScriptLocalization="true" />
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        function BeginRequestHandler(sender, args) {
            document.getElementById('<%= btnSubmit.ClientID %>').innerText = "Processing";
            document.getElementById('<%= btnSubmit.ClientID %>').disabled = true;
            document.getElementById('<%= btnCancel.ClientID %>').disabled = true;
            document.getElementById('<%= btnApply.ClientID %>').disabled = true;
            document.getElementById('<%= btnUnApply.ClientID %>').disabled = true;
        }
    </script>
    <asp:UpdatePanel ID="updPanel1" runat="server">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="txtInvoiceNetAmount" EventName="TextChanged" />
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
                                            <td colspan="2" class="tblHeader" title="INTERVENTION - Buchung editieren ">
                                                INKASSO - Buchung editieren
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" class="EditCaption">
                                                <div align="right">
                                                    Re. Nr.:</div>
                                            </td>
                                            <td class="EditData">
                                                <strong><asp:Label runat="server" ID="lblInvoiceId" /></strong>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">
                                                    Akt Nr.:</div>
                                            </td>
                                            <td class="EditData">
                                                <strong><asp:Label runat="server" ID="lblAktId" /></strong>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" class="EditCaption">
                                                <div align="right">
                                                    Datum:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox runat="server" ID="txtInvoiceDate" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="12"
                                                    MaxLength="10" />
                                                <ajax:MaskedEditExtender ID="InvDate_MaskedEditExtender" runat="server" Century="2000" TargetControlID="txtInvoiceDate" Mask="99/99/9999" MessageValidatorTip="true"
                                                    OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" MaskType="Date" ErrorTooltipEnabled="True" />
                                                <ajax:MaskedEditValidator ID="InvDate_MaskedEditValidator" runat="server" ControlExtender="InvDate_MaskedEditExtender" ControlToValidate="txtInvoiceDate" InvalidValueMessage="Datum is ung&uuml;ltig!"
                                                    Display="Dynamic" InvalidValueBlurredMessage="*" />
                                                <ajax:CalendarExtender ID="InvDate_CalendarExtender" runat="server" TargetControlID="txtInvoiceDate" PopupButtonID="InvDate_CalendarButton" />
                                                <asp:ImageButton runat="Server" ID="InvDate_CalendarButton" ImageUrl="~/v2/intranet/images/CalendarHS.png" AlternateText="Click to show calendar" />
                                                (tt.mm.jjjj)
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" class="EditCaption">
                                                <div align="right">
                                                    Text:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox runat="server" ID="txtDescription" class="docText" onfocus="this.style.backgroundColor='#DFF4FF'; this.select();" onblur="this.style.backgroundColor=''"
                                                    size="50" MaxLength="50" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td nowrap class="EditCaption">
                                                <div align="right">
                                                    Betrag (ohne MWS):</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox runat="server" ID="txtInvoiceNetAmount" class="docText" AutoPostBack="true" OnTextChanged="txtInvoiceNetAmount_TextChanged" onFocus="this.style.backgroundColor='#DFF4FF'" onBlur="this.style.backgroundColor=''" />
                                            </td>
                                        </tr>
                                        <tr id="trTaxable" runat="server">
                                            <td valign="top" class="EditCaption">
                                                <div align="right">Steuerbar:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:CheckBox runat="server" ID="chkTaxable" />
                                            </td>
                                        </tr>
                                        <tr id="trTaxAmount" runat="server">
                                            <td valign="top" class="EditCaption">
                                                <div align="right">MWS:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:Label runat="server" ID="lblTaxAmount" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" class="EditCaption">
                                                <div align="right"><strong>Betrag (mit MWS)</strong>:</div>
                                            </td>
                                            <td class="EditData">
                                                <strong><asp:Label runat="server" ID="lblInvoiceAmount" /></strong>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td nowrap class="EditCaption">
                                                <div align="right"><strong>Applied</strong>:</div>
                                            </td>
                                            <td class="EditData">
                                                <strong><asp:Label runat="server" ID="lblAppliedAmount" /></strong>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td nowrap class="EditCaption">
                                                <div align="right"><strong>Offen</strong>:</div>
                                            </td>
                                            <td class="EditData">
                                                <strong><asp:Label runat="server" ID="lblUnappliedAmount" /></strong>
                                            </td>
                                        </tr>
                                        <tr id="trDueDate" runat="server">
                                            <td nowrap class="EditCaption">
                                                <div align="right">
                                                    F&auml;lligkeitsdatum:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox runat="server" ID="txtDueDate" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="12" MaxLength="10" />
                                                <ajax:MaskedEditExtender ID="DueDate_MaskedEditExtender" runat="server" Century="2000" TargetControlID="txtDueDate" Mask="99/99/9999" MessageValidatorTip="true"
                                                    OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" MaskType="Date" ErrorTooltipEnabled="True" />
                                                <ajax:MaskedEditValidator ID="DueDate_MaskedEditValidator" runat="server" ControlExtender="DueDate_MaskedEditExtender" ControlToValidate="txtDueDate" InvalidValueMessage="Datum is ung&uuml;ltig!"
                                                    Display="Dynamic" InvalidValueBlurredMessage="*" />
                                                <ajax:CalendarExtender ID="DueDate_CalendarExtender" runat="server" TargetControlID="txtDueDate" PopupButtonID="DueDate_CalendarButton" />
                                                <asp:ImageButton runat="Server" ID="DueDate_CalendarButton" ImageUrl="~/v2/intranet/images/CalendarHS.png" AlternateText="Click to show calendar" />
                                                (tt.mm.jjjj)
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" class="EditCaption">
                                                <div align="right">
                                                    Beschreibung (Grund für die Änderung):</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox ID="txtMemo" runat="server" class="docText" Columns="80" onblur="this.style.backgroundColor='';" onfocus="this.style.backgroundColor='#DFF4FF';" Rows="8"
                                                    TextMode="MultiLine" />
                                            </td>
                                        </tr>
                                        <tr id="trManual" runat="server">
                                            <td class="EditCaption">
                                                <div align="right">
                                                    <strong>Manuel Buchen</strong>:&nbsp;</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:CheckBox ID="chkManual" runat="server" OnCheckedChanged="chkManual_CheckedChanged" AutoPostBack="true" />
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

                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">
                                                    &Uuml;berwiesen:&nbsp;</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:CheckBox ID="chkTransferred" runat="server" OnCheckedChanged="chkTransferred_CheckedChanged" AutoPostBack="true" />
                                            </td>
                                        </tr>

                                        <tr id="trTransferredDate" runat="server">
                                            <td nowrap class="EditCaption">
                                                <div align="right"><strong>&Uuml;berwiesen am</strong>:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox runat="server" ID="txtTransferDate" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="12" MaxLength="10" />
                                                <ajax:MaskedEditExtender ID="mmeTransferDate" runat="server" Century="2000" TargetControlID="txtTransferDate" Mask="99/99/9999" MessageValidatorTip="true"
                                                    OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" MaskType="Date" ErrorTooltipEnabled="True" />
                                                <ajax:MaskedEditValidator ID="mevTransferDate" runat="server" ControlExtender="mmeTransferDate" ControlToValidate="txtTransferDate" InvalidValueMessage="Datum is ung&uuml;ltig!"
                                                    Display="Dynamic" InvalidValueBlurredMessage="*" />
                                                <ajax:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtTransferDate" PopupButtonID="cbTransferDate" />
                                                <asp:ImageButton runat="Server" ID="cbTransferDate" ImageUrl="~/v2/intranet/images/CalendarHS.png" AlternateText="Click to show calendar" />
                                                (tt.mm.jjjj)
                                            </td>
                                        </tr>
                                        <tr id="trTransferredAmount" runat="server">
                                            <td nowrap class="EditCaption">
                                                <div align="right"><strong>
                                                    &Uuml;berweisungsbetrag</strong>:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox runat="server" ID="txtTransferredAmount" class="docText" onfocus="this.style.backgroundColor='#DFF4FF'" onblur="this.style.backgroundColor=''" size="15"
                                                    MaxLength="15" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" class="tblDataAll">
                                                <ctl:message ID="ctlMessage" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditData" colspan="2">
                                                <strong>Buchungen</strong>:<br />
                                                <asp:GridView ID="gvAppliedTo" runat="server" AllowSorting="false" AutoGenerateColumns="False" CellPadding="2" BorderStyle="Groove" Width="100%" ShowFooter="true" >
                                                    <RowStyle />
                                                    <Columns>
                                                        <asp:BoundField HeaderText="Buchung #" DataField="InvoiceID" SortExpression="InvoiceID" ItemStyle-HorizontalAlign="Center"/>
                                                        <asp:BoundField HeaderText="Datum" DataField="InvoiceDate" SortExpression="InvoiceDate" ItemStyle-HorizontalAlign="Center"/>
                                                        <asp:BoundField HeaderText="Text" DataField="InvoiceDescription" SortExpression="InvoiceDescription"  FooterText="Total:" FooterStyle-Font-Bold="True" FooterStyle-HorizontalAlign="Right"/>
                                                        <asp:TemplateField HeaderText="Buchung" FooterStyle-Font-Bold="True"  ItemStyle-HorizontalAlign="Right"  FooterStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <%# HTBUtilities.HTBUtils.FormatCurrency(decimal.Parse(Eval("AppliedAmount").ToString()))  %>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <%# HTBUtilities.HTBUtils.FormatCurrency(GetTotalApplied()) %>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField HeaderText="Betrag" DataField="InvoiceAmount" SortExpression="InvoiceAmount"  ItemStyle-HorizontalAlign="Right"/>
                                                        <asp:BoundField HeaderText="Saldo"  DataField="InvoiceBalance" SortExpression="InvoiceBalance"  ItemStyle-HorizontalAlign="Right" />
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr id="trInstallmentsApplied" runat="server">
                                            <td class="tblDataAll"  colspan="2">
                                                <strong>Applied Raten</strong>:<br />
                                                <asp:GridView ID="gvInstallmentsApplied" runat="server" AutoGenerateColumns="False" HtmlEncode="false" CellPadding="2" CellSpacing="2" BorderStyle="Inset"
                                                    Width="100%">
                                                    <AlternatingRowStyle BackColor="AntiqueWhite" />
                                                    <Columns>
                                                        <asp:BoundField HeaderText="Rate Datum" DataField="RateDueDate" SortExpression="RateDueDate" >
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="Betrag" DataField="RateAmount" SortExpression="RateAmount" >
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="Zahlung Datum" DataField="RateAppliedDate" SortExpression="ReceivedDate" >
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="Zahlung Betrag" DataField="RateAppliedAmount" SortExpression="ReceivedAmount" >
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="Ofen" DataField="RateBalance" SortExpression="RateBalance" >
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        </asp:BoundField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>

                                        <tr id="trInstallmentsGrid" runat="server">
                                            <td class="tblDataAll"  colspan="2">
                                                <strong>Raten</strong>:<br />
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

                                                        <asp:BoundField HeaderText="Saldo" DataField="RateBalance" SortExpression="RateBalance" ItemStyle-HorizontalAlign="Right" />
                                                        
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
                                            <td colspan="2" class="tblFooter1">
                                                <div align="right">
                                                    <asp:Button runat="server" ID="btnSubmit" class="btnSave" title="Speichern" Text="Speichern" OnClick="btnSubmit_Click" />
                                                    <asp:Button runat="server" ID="btnCancel" class="btnCancel" title="Schlie&szlig;en" Text="Schlie&szlig;en" OnClick="btnCancel_Click" />

                                                    <asp:Button runat="server" ID="btnApply" class="btnAction" title="Aufteilen" Text="Aufteilen" onclick="btnApply_Click"  />
                                                    <asp:Button runat="server" ID="btnUnApply" class="btnAction" title="R&uuml;ckaufteilen" Text="R&uuml;ckaufteilen" onclick="btnUnApply_Click"  />
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
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
    <ctl:footer ID="ftr" runat="server" />
</body>
</html>
