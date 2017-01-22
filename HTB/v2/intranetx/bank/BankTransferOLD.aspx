<%@ Page Language="C#" Culture="de-DE" AutoEventWireup="true" CodeBehind="BankTransferOLD.aspx.cs" Inherits="HTB.v2.intranetx.bank.BankTransferOLD" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>HTB.ASP [ Überweisungslisten ]</title>
    <link href="../../intranet/styles/inksys.css" rel="stylesheet" type="text/css"/>
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

    OPTION.dis{background-color:white; color:#999999}

    </style>
</head>
<body>
    <ctl:header ID="header" runat="server" />
    <form id="form1" runat="server">
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
        <ContentTemplate>
            <table width="100%" border="0" cellpadding="1" cellspacing="0" bgcolor="#000000">
                <tr>
                    <td>
                        <table width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
                            <tr>
                                <td class="smallText">
                                    <a href="/v2/intranet/intranet/intranet.asp">Intranet</a> |
                                    <a href="/v2/intranet/intranet/mydata.asp"> Meine Daten</a> | 
                                    <a href="Bank.aspx">Bank</a> | &Uuml;berweisungslisten
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <br>
            <table width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
                <tr>
                    <td>
                        <table border="0" align="center" cellpadding="3" cellspacing="0">
                            <tr>
                                <td class="tblHeader">
                                    INKASSO &Uuml;BERWEISUNG
                                </td>
                            </tr>
                            <tr>
                                <td class="tblData1">
                                    <asp:Label ID="lblMessage" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tblDataAll">
                                    <asp:GridView ID="gvTransfers" runat="server" AllowSorting="false" AutoGenerateColumns="False"
                                        CellPadding="2" CellSpacing="2" BorderStyle="Inset" Width="100%" AlternatingRowStyle-BackColor="AntiqueWhite" ShowFooter="true">
                                        <RowStyle />
                                        <Columns>
                                            <%--Hidden Column--%>
                                            <asp:TemplateField HeaderText="ID" Visible="false"> 
                                                <ItemTemplate>
                                                    <asp:Label id="lblInvoiceId" runat="server" Text='<%# Eval("InvoiceID")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:HyperLinkField HeaderText="Buchung #" DataTextField="InvoiceID" DataNavigateUrlFields="InvoiceID"
                                                ItemStyle-HorizontalAlign="Center" DataTextFormatString="&lt;a href=javascript:MM_openBrWindow('/v2/intranetx/aktenink/ShowInvoice.aspx?InvId={0}','popSetAction','menubar=yes,scrollbars=yes,resizable=yes,width=800,height=800,top=10');&gt;{0}&lt;/a&gt;"
                                                SortExpression="InvoiceID" />
                                            <asp:BoundField HeaderText="Aktenzeichen" DataField="AktNumber" SortExpression="AktNumber"
                                                ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField HeaderText="Bemerkung" DataField="Note" SortExpression="Note"
                                                ItemStyle-HorizontalAlign="Left"  HtmlEncode="false"/>
                                            <asp:BoundField HeaderText="Datum (Eingang)" DataField="PaymentReceivedDate" SortExpression="PaymentReceivedDate"
                                                ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField HeaderText="Betrag (Eingang)" DataField="PaymentAmount" SortExpression="PaymentAmount"
                                                ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField HeaderText="Differenz (ECP)" DataField="CollectionAmount" SortExpression="CollectionAmount"
                                                ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField HeaderText="Name Schuldner" DataField="NameSchuldner" SortExpression="NameSchuldner"
                                                ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="Name Kunde" DataField="ClientName" SortExpression="ClientName"
                                                ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="Kontonummer Kunde" DataField="BankAccountNumber" SortExpression="BankAccountNumber"
                                                ItemStyle-HorizontalAlign="Center" HtmlEncode="false" />
                                            <asp:TemplateField HeaderText="Datum (Ausgang)" FooterText="Summe:">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtTransferDate" runat="server" MaxLength="10" ToolTip="&Uuml;berweisungs Datum" Text='<%#Eval("TransferDate") %>'
                                                        ValidationGroup="DateVG" onFocus="this.style.backgroundColor='#DFF4FF'" onBlur="this.style.backgroundColor=''"
                                                        size="10" />
                                                    <ajax:MaskedEditExtender id="Datum_MaskedEditExtender" runat="server" century="2000"
                                                        targetcontrolid="txtTransferDate" mask="99/99/9999" messagevalidatortip="true" onfocuscssclass="MaskedEditFocus"
                                                        oninvalidcssclass="MaskedEditError" masktype="Date" errortooltipenabled="True" />
                                                    <ajax:MaskedEditValidator id="Datum_MaskedEditValidator" runat="server" controlextender="Datum_MaskedEditExtender"
                                                        controltovalidate="txtTransferDate" invalidvaluemessage="Datum is ung&uuml;ltig!" display="Dynamic"
                                                        invalidvalueblurredmessage="*" validationgroup="DateVG" />
                                                    <ajax:CalendarExtender id="Datum_CalendarExtender" runat="server" targetcontrolid="txtTransferDate"
                                                        popupbuttonid="Datum_CalendarButton" />
                                                    <asp:ImageButton runat="Server" ID="Datum_CalendarButton" ImageUrl="~/v2/intranet/images/CalendarHS.png"
                                                        AlternateText="Click to show calendar" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                                <FooterStyle HorizontalAlign="Right" Font-Bold="true"/>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Betrag (Ausgang)">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtTransferAmmount" runat="server" Width="80px" Text='<%#Eval("TransferAmount") %>' onFocus="this.style.backgroundColor='#DFF4FF'" onBlur="this.style.backgroundColor=''" style="text-align: right"/>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                                <FooterTemplate>
                                                    <%= GetTotalAmountString() %>
                                                </FooterTemplate>
                                                <FooterStyle HorizontalAlign="Center" Font-Bold="true"/>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="&Uuml;berwiesen">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkTransferred" runat="server"/>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td class="EditData" align="center">
                                    <asp:Button ID="btnSubmit" runat="server" class="btnSave" Text="Speichern" OnClick="Submit_Click" />&nbsp;&nbsp;&nbsp;
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
