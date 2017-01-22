<%@ Page Language="C#" Culture="de-DE" AutoEventWireup="true" CodeBehind="AktInkRatePayment.aspx.cs" Inherits="HTB.v2.intranetx.aktenink.AktInkRatePayment" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>HTB.ASP [ &Uuml;berweisungslisten ]</title>
    <link href="../../intranet/styles/inksys.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
    <!--
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

    -->
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
                                    <a href="../../intranet/intranet/intranet.asp">Intranet</a> | <a href="../../intranet/intranet/mydata.asp">
                                        Meine Daten</a> | <a href="../../intranet/intranet/akten.asp">Akten</a> | <a href="../../intranet/aktenink/AktenStaff.asp">
                                            Inkassoakten</a> | &Uuml;berweisungslisten
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
                                    RATEN ZAHLUNGEN
                                </td>
                            </tr>
                            <tr>
                                <td class="tblData1">
                                    <asp:Label ID="lblMessage" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tblData1">
                                    <asp:GridView ID="gvTransfers" runat="server" AllowSorting="false" AutoGenerateColumns="False"
                                        CellPadding="2" CellSpacing="2" BorderStyle="Inset" Width="100%" AlternatingRowStyle-BackColor="AntiqueWhite">
                                        <RowStyle />
                                        <Columns>
                                            <%--Hidden Column--%>
                                            <asp:TemplateField HeaderText="ID" Visible="false"> 
                                                <ItemTemplate>
                                                    <asp:Label id="lblInvoiceId" runat="server" Text='<%# Eval("RateID")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:BoundField HeaderText="Aktenzeichen" DataField="AktNumber" SortExpression="AktNumber" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField HeaderText="Rate Datum" DataField="RateDueDate" SortExpression="RateDueDate" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField HeaderText="Betrag" DataField="RateAmount" SortExpression="RateAmount" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField HeaderText="Name Schuldner" DataField="NameSchuldner" SortExpression="NameSchuldner" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="Name Kunde" DataField="ClientName" SortExpression="ClientName" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="Kontonummer Kunde" DataField="BankAccountNumber" SortExpression="BankAccountNumber" ItemStyle-HorizontalAlign="Center" />

                                            <asp:TemplateField HeaderText="Datum (Eingang)">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtReceivedDate" runat="server" MaxLength="10" ToolTip="Rate Datum" Text='<%#Eval("ReceivedDate") %>'
                                                        ValidationGroup="DateVG" onFocus="this.style.backgroundColor='#DFF4FF'" onBlur="this.style.backgroundColor=''"
                                                        size="10" />
                                                    <ajax:maskededitextender id="Datum_MaskedEditExtender" runat="server" century="2000"
                                                        targetcontrolid="txtReceivedDate" mask="99/99/9999" messagevalidatortip="true" onfocuscssclass="MaskedEditFocus"
                                                        oninvalidcssclass="MaskedEditError" masktype="Date" errortooltipenabled="True" />
                                                    <ajax:maskededitvalidator id="Datum_MaskedEditValidator" runat="server" controlextender="Datum_MaskedEditExtender"
                                                        controltovalidate="txtReceivedDate" invalidvaluemessage="Datum is ung&uuml;ltig!" display="Dynamic"
                                                        invalidvalueblurredmessage="*" validationgroup="DateVG" />
                                                    <ajax:calendarextender id="Datum_CalendarExtender" runat="server" targetcontrolid="txtReceivedDate"
                                                        popupbuttonid="Datum_CalendarButton" />
                                                    <asp:ImageButton runat="Server" ID="Datum_CalendarButton" ImageUrl="~/v2/intranet/images/CalendarHS.png"
                                                        AlternateText="Click to show calendar" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            
                                            <asp:BoundField HeaderText="Verschoben bis" DataField="PostponeTillDate" SortExpression="PostponeTillDate" >
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>

                                            <asp:BoundField HeaderText="Verschiebungsgrund" DataField="PostponeReason" SortExpression="PostponeReason" HtmlEncode="false"/>
                                            <asp:BoundField HeaderText="Verschoben vom" DataField="PostponeBy" SortExpression="PostponeBy" HtmlEncode="false"/>

                                            <asp:TemplateField HeaderText="Betrag (Eingang)">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtReceivedAmount" runat="server" Width="80px" Text='<%#Eval("ReceivedAmount") %>' onFocus="this.style.backgroundColor='#DFF4FF'" onBlur="this.style.backgroundColor=''" style="text-align: right"/>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            
                                            <asp:TemplateField HeaderText="Received">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkReceived" runat="server"/>
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
