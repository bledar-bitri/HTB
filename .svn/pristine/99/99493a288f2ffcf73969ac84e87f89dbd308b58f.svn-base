<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CollectedByAussendienst.aspx.cs" Inherits="HTB.v2.intranetx.customer.CollectedByAussendienst" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>HTB.ASP [ Einnahmen ]</title>
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
    </style>
</head>
<body>
    <ctl:headerNoMenu ID="hdr" runat="server" />
    <table width="100%" border="0" cellpadding="1" cellspacing="0" bgcolor="#000000">
        <tr>
            <td>
                <table width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
                    <tr>
                        <td class="smallText">
                            <asp:HyperLink runat="server" NavigateUrl="Customer.aspx">Kundenportal</asp:HyperLink> | Interventionsstatistik | <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="javascript:window.print()">Drucken</asp:HyperLink>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <br />
    <form id="form1" runat="server" defaultbutton="btnSubmit">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" EnablePartialRendering="true" EnableScriptLocalization="true" />
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        function BeginRequestHandler(sender, args) {
            document.getElementById('<%= btnSubmit.ClientID %>').innerText = "Bitte warten!";
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
                                    <table width="100%" border="0" cellspacing="0" cellpadding="1">
                                        <tr>
                                            <td class="tblFunctionBar">
                                            </td>
                                        </tr>
                                    </table>
                                    <table width="100%" border="0" cellpadding="1" cellspacing="0">
                                        <tr>
                                            <td class="tblFunctionBar">
                                                <table width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#a3cdbd">
                                                    <tr>
                                                        <td>
                                                            <span class="smallText"><strong>Filter</strong></span>
                                                        </td>
                                                        <td>
                                                            <div align="right">
                                                                <a href="Customer.aspx" class="smallText" title="Zur&uuml;ck zum Hauptmen&uuml;">Zur&uuml;ck</a>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="10%">
                                                            <span class="smallText">&nbsp;&nbsp;&Uuml;bergabedatum:</span>
                                                        </td>
                                                        <td>
                                                            von:
                                                            <asp:TextBox ID="txtDateStart" runat="server" class="smallText" size="15" MaxLength="10" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" />
                                                            <ajax:MaskedEditExtender ID="Datum_MaskedEditExtender1" runat="server" Century="2000" TargetControlID="txtDateStart" Mask="99/99/9999" MessageValidatorTip="true"
                                                                OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" MaskType="Date" ErrorTooltipEnabled="True" />
                                                            <ajax:MaskedEditValidator ID="Datum_MaskedEditValidator1" runat="server" ControlExtender="Datum_MaskedEditExtender1" ControlToValidate="txtDateStart" InvalidValueMessage="Datum is ung&uuml;ltig!"
                                                                Display="Dynamic" InvalidValueBlurredMessage="*" />
                                                            <ajax:CalendarExtender ID="Datum_CalendarExtender1" runat="server" TargetControlID="txtDateStart" PopupButtonID="Datum_CalendarButton1" />
                                                            <asp:ImageButton runat="Server" ID="Datum_CalendarButton1" ImageUrl="~/v2/intranet/images/CalendarHS.png" AlternateText="Click to show calendar" />
                                                            &nbsp;&nbsp;bis:
                                                            <asp:TextBox ID="txtDateEnd" runat="server" class="smallText" size="15" MaxLength="10" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" />
                                                            <ajax:MaskedEditExtender ID="Datum_MaskedEditExtender2" runat="server" Century="2000" TargetControlID="txtDateEnd" Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                                                OnInvalidCssClass="MaskedEditError" MaskType="Date" ErrorTooltipEnabled="True" />
                                                            <ajax:MaskedEditValidator ID="Datum_MaskedEditValidator2" runat="server" ControlExtender="Datum_MaskedEditExtender2" ControlToValidate="txtDateEnd" InvalidValueMessage="Datum is ung&uuml;ltig!"
                                                                Display="Dynamic" InvalidValueBlurredMessage="*" />
                                                            <ajax:CalendarExtender ID="Datum_CalendarExtender2" runat="server" TargetControlID="txtDateEnd" PopupButtonID="Datum_CalendarButton2" />
                                                            <asp:ImageButton runat="Server" ID="Datum_CalendarButton2" ImageUrl="~/v2/intranet/images/CalendarHS.png" AlternateText="Click to show calendar" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnSubmit" runat="server" CssClass="btnSave" Text=" Go " OnClick="btnSubmit_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                    <table width="100%" border="0" cellpadding="3" cellspacing="0">
                                        <tr>
                                            <td colspan="12" class="tblHeader">
                                                Einnahmen
                                                <asp:Label ID="lblAGInfo" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="12">
                                                <ctl:message ID="ctlMessage" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                    <table border="0" align="center" cellpadding="0" cellspacing="0" >
                                        <tr>
                                            <td>
                                                <table border="0" align="center" cellpadding="3" cellspacing="0" class="tblDataAll">
                                                    <tr>
                                                        <td colspan="2" valign="top" class="tblHeader">
                                                           Einnahmen
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tblData1" colspan="2">
                                                            <asp:GridView ID="gvDetail" runat="server" AllowSorting="true" AutoGenerateColumns="False" CellPadding="2" CellSpacing="1" ShowFooter="True" BorderStyle="Inset"
                                                                Width="100%" OnSorting="gvDetail_Sorting">
                                                                <RowStyle BackColor="#EFF3FB" />
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Datum" SortExpression="CollectedDate" FooterStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <div class="gridData">
                                                                                <%# Eval("CollectedDate")%></div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="AZ" SortExpression="AktId" ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <div class="gridData">
                                                                                 <%# Eval("AktId")%><br />
                                                                                [<%# Eval("AktAZ")%>]
                                                                            </div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    
                                                                    <asp:TemplateField HeaderText="Aussendienst" SortExpression="CollectorName">
                                                                        <ItemTemplate>
                                                                            <div class="gridData">
                                                                                <%# Eval("CollectorName")%></div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Gegnername" SortExpression="GegnerName">
                                                                        <ItemTemplate>
                                                                            <div class="gridData">
                                                                                <%# Eval("GegnerName")%></div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    
                                                                    <asp:TemplateField HeaderText="Beleg" SortExpression="CollectedBillNumber" FooterStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                                                        <ItemTemplate>
                                                                            <div class="gridData" align="right">
                                                                                <%# Eval("CollectedBillNumber")%></div>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <div class="gridData"><strong>Insgesamt:</strong></div>
                                                                        </FooterTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Einnahmen" SortExpression="CollectedAmount" FooterStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                                                        <ItemTemplate>
                                                                            <div class="gridData" align="right">
                                                                                <%# Eval("CollectedAmount")%></div>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <div class="gridData">
                                                                                <strong><%=GetTotalAmount()%></strong>
                                                                            </div>
                                                                        </FooterTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <FooterStyle BackColor="#d6f5a7" Font-Bold="True" ForeColor="White" />
                                                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                                                <HeaderStyle CssClass="tblCol1" Font-Bold="True" ForeColor="#000000" />
                                                                <AlternatingRowStyle BackColor="White" />
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            </td> </tr> </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
    <ctl:footer ID="ftr" runat="server" />
</body>
</html>
