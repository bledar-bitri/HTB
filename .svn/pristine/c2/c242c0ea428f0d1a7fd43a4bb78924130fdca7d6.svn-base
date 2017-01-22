<%@ Page Language="C#" Culture="de-DE" AutoEventWireup="true" CodeBehind="ShowInvoice.aspx.cs" Inherits="HTB.v2.intranetx.aktenink.ShowInvoice" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>HTB.ASP [ Inkassoakt Buchung ]</title>
</head>
<body>
    <ctl:headerNoMenu ID="header" runat="server" />
    <form id="form1" runat="server">
    <table width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
        <tr>
            <td>
                <table width="632" border="0" align="center" cellpadding="3" cellspacing="0">
                    <tr>
                        <td colspan="2" class="tblHeader">
                            INKASSOAKT Buchung
                        </td>
                    </tr>
                    <tr>
                        <td width="10%" valign="top" nowrap class="EditCaption">
                            <div align="right">
                                <strong>Akt Nr.&nbsp;</strong>:</div>
                        </td>
                        <td class="EditData">
                            <%=qryInkassoakt.CustInkAktID%>
                            <%
                                if (qryInkassoakt.CustInkAktOldID != "")
                                {
                            %>
                            [<%= qryInkassoakt.CustInkAktOldID %>]
                            <%
                                }
                            %>
                            <br>
                            <span class="style3">
                                <%
                                    if (!string.IsNullOrEmpty(qryInkassoakt.CustInkAktGothiaNr))
                                    {
                                %>
                                <%=qryInkassoakt.CustInkAktGothiaNr%>
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
                            <%=qryInkassoakt.CustInkAktEnterDate.ToShortDateString()%>
                        </td>
                    </tr>
                    <tr>
                        <td width="10%" valign="top" nowrap class="EditCaption">
                            <div align="right">
                                Auftraggeber:&nbsp;</div>
                        </td>
                        <td class="EditData">
                            <strong>
                                <%=qryInkassoakt.AuftraggeberName1%></strong>&nbsp;<%=qryInkassoakt.AuftraggeberName2%><br>
                            <%=qryInkassoakt.AuftraggeberStrasse%><br>
                            <%=qryInkassoakt.AuftraggeberLKZ%>&nbsp;<%=qryInkassoakt.AuftraggeberPLZ%>&nbsp;<%=qryInkassoakt.AuftraggeberOrt%>
                        </td>
                    </tr>
                    <tr>
                        <td width="10%" valign="top" nowrap class="EditCaption">
                            <div align="right">
                                Klient:&nbsp;</div>
                        </td>
                        <td class="EditData">
                            <strong>
                                <%=qryInkassoakt.KlientName1%></strong>&nbsp;<%=qryInkassoakt.KlientName2%><br>
                            <%=qryInkassoakt.KlientStrasse%><br>
                            <%=qryInkassoakt.KlientLKZ%>&nbsp;<%=qryInkassoakt.KlientPLZ%>&nbsp;<%=qryInkassoakt.KlientOrt%>
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
                                    <%=qryInkassoakt.GegnerLastName1%></strong>,&nbsp;<%=qryInkassoakt.GegnerLastName2%><br>
                                <%=qryInkassoakt.GegnerLastStrasse%><br>
                                <%=qryInkassoakt.GegnerLastZipPrefix%>&nbsp;<%=qryInkassoakt.GegnerLastZip%>&nbsp;<%=qryInkassoakt.GegnerLastOrt%></p>
                            <p>
                                &nbsp;</p>
                        </td>
                    </tr>
                    <tr>
                        <td width="10%" class="EditCaption">
                            <div align="right">
                                <strong>Rechnungsnummer&nbsp;[Kunde]</strong>:&nbsp;</div>
                        </td>
                        <td class="EditData">
                            <asp:Label ID="lblCustInkAktKunde" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="EditCaption">
                            <div align="right">
                                <strong>Rechnungsdatum&nbsp;[Kunde]</strong>:&nbsp;</div>
                        </td>
                        <td class="EditData">
                            <%=qryInkassoakt.CustInkAktInvoiceDate.ToShortDateString()%>
                        </td>
                    </tr>
                    <tr>
                        <td class="EditCaption">
                            &nbsp;
                        </td>
                        <td class="EditData">
                            <asp:Label ID="lblMessage" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="EditCaption">
                            <div align="right">
                                <strong>Buchungsart</strong>:&nbsp;</div>
                        </td>
                        <td class="EditData">
                            <strong><asp:Label ID="lblInvoiceType" runat="server" /></strong>
                        </td>
                    </tr>
                    <tr>
                        <td class="EditCaption">
                            <div align="right">
                                <strong>Buchungsdatum</strong>:&nbsp;</div>
                        </td>
                        <td class="EditData" valign="bottom">
                            <asp:Label ID="lblInvoiceDate" runat="server" />
                        </td>
                    </tr>
                    <tr id="trPaymentReceivedDate" runat="server" visible="false">
                        <td class="EditCaption">
                            <div align="right">
                                <strong>Eingang Datum</strong>:&nbsp;</div>
                        </td>
                        <td class="EditData" valign="bottom">
                            <asp:Label ID="lblPaymentReceivedDate" runat="server" />
                        </td>
                    </tr>
                    <tr id="trInvoiceAmountNetto" runat="server" visible="false">
                        <td class="EditCaption">
                            <div align="right">
                                <strong>Betrag Netto</strong>:&nbsp;</div>
                        </td>
                        <td class="EditData">
                            <asp:Label ID="lblInvoiceAmountNetto" runat="server" />
                        </td>
                    </tr>
                    <tr id="trTax" runat="server" visible="false">
                        <td class="EditCaption">
                            <div align="right">
                                <strong>MWS</strong>:&nbsp;</div>
                        </td>
                        <td class="EditData">
                            <asp:Label ID="lblTax" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="EditCaption">
                            <div align="right">
                                <strong>Betrag</strong>:&nbsp;</div>
                        </td>
                        <td class="EditData">
                            <strong><asp:Label ID="lblInvoiceAmount" runat="server" /></strong>
                        </td>
                    </tr>
                    <tr>
                        <td class="EditCaption">
                            <div align="right">
                                <strong>Saldo</strong>:&nbsp;</div>
                        </td>
                        <td class="EditData">
                            <strong><asp:Label ID="lblInvoiceBalance" runat="server" /></strong>
                        </td>
                    </tr>
                    <tr>
                        <td class="EditData" colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr id="trInvoicePaymentTransferToClientDate" runat="server" visible="false">
                        <td class="EditCaption">
                            <div align="right"><strong>Ausgang&nbsp;[an&nbsp;Kunde]&nbsp;Datum</strong>:&nbsp;</div>
                        </td>
                        <td class="EditData" valign="bottom">
                            <asp:Label ID="lblInvoicePaymentTransferToClientDate" runat="server" />
                        </td>
                    </tr>
                    <tr id="trInvoicePaymentTransferToClientAmount" runat="server" visible="false">
                        <td class="EditCaption">
                            <div align="right">
                                <strong>Ausgang&nbsp;[an&nbsp;Kunde]&nbsp;Betrag</strong>:&nbsp;</div>
                        </td>
                        <td class="EditData">
                            <asp:Label ID="lblInvoicePaymentTransferToClientAmount" runat="server" />
                        </td>
                    </tr>
                    <tr id="trComment" runat="server" visible="false">
                        <td class="EditCaption">
                            <div align="right">
                                <strong>Beschreibung</strong>:&nbsp;</div>
                        </td>
                        <td class="EditData">
                            <asp:Label ID="lblComment" runat="server" TextMode="MultiLine" />
                        </td>
                    </tr>
                    <tr>
                        <td class="EditData" colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="EditData" colspan="2">
                            <strong>Buchungen</strong>:<br />
                            <asp:GridView ID="gvAppliedTo" runat="server" AllowSorting="false" AutoGenerateColumns="False" CellPadding="2" BorderStyle="Groove" Width="100%" ShowFooter="true" >
                                <RowStyle />
                                <Columns>
                                    <asp:HyperLinkField HeaderText="Buchung #" DataTextField="InvoiceID" DataNavigateUrlFields="InvoiceID" ItemStyle-HorizontalAlign="Center"
                                        DataNavigateUrlFormatString="~/v2/intranetx/aktenink/ShowInvoice.aspx?InvId={0}"
                                        SortExpression="InvoiceID" />
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
                    <tr>
                        <td class="EditData" colspan="2" align="right">
                            <input name="btnClose" id="btnClose" type="button" class="btnCancel" value="Schlie&szlig;en" onclick="window.close();" />&nbsp;&nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
    <ctl:footer ID="ftr" runat="server" />
</body>
</html>
