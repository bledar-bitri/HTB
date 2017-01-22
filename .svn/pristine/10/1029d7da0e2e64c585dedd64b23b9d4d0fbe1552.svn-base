<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InteverntionStatistic.aspx.cs" Inherits="HTB.v2.intranetx.customer.InteverntionStatistic" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting"
    TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>HTB.ASP [ Inkassostatistiken ]</title>
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
    <form id="form1" runat="server" defaultbutton="btnSubmit">
    <table width="100%" border="0" cellpadding="1" cellspacing="0" bgcolor="#000000">
        <tr>
            <td>
                <table width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
                    <tr>
                        <td class="smallText">
                            <asp:HyperLink runat="server" NavigateUrl="Customer.aspx">Kundenportal</asp:HyperLink> 
                            | Interventionsstatistik 
                            | <asp:HyperLink runat="server" NavigateUrl="javascript:window.print()">Drucken </asp:HyperLink>
<%--                            | <asp:LinkButton ID="btnXL" runat="server" CssClass="btnExport" Text=" Xls " OnClick="btnXL_Click" />
                            | <asp:LinkButton ID="btnXL2" runat="server" CssClass="btnExport" Text=" Xls2 " OnClick="btnXL2_Click" />--%>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <br />
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
                                                            <span class="smallText">&nbsp;&nbsp;Akttyp:</span>
                                                        </td>
                                                        <td>
                                                            <div align="left">
                                                                <asp:DropDownList ID="ddlAktType" runat="server" />
                                                            </div>
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
                                                Interventionsstatistik - Forderungs&uuml;bersicht f&uuml;r
                                                <asp:Label ID="lblAGInfo" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="12">
                                                <ctl:message ID="ctlMessage" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                    <table border="0" align="center" cellpadding="3" cellspacing="0" class="tblDataAll">
                                        <tr>
                                            <td colspan="2" valign="top" class="tblHeader" colspan="2">
                                                Status aller Auftr&auml;ge
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tblData1" colspan="2">
                                                <asp:GridView ID="gvAllCases" runat="server" AllowSorting="true" AutoGenerateColumns="False" CellPadding="2" CellSpacing="1" ShowFooter="True" BorderStyle="Inset"
                                                    Width="100%" OnSorting="gvAllCases_Sorting">
                                                    <RowStyle BackColor="#EFF3FB" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="" SortExpression="Description" FooterStyle-HorizontalAlign="Right" HeaderStyle-Width="500px">
                                                            <ItemTemplate>
                                                                <div class="gridData">
                                                                    <%# Eval("Description")%>
                                                                </div>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <div class="gridData">
                                                                    <strong>Insgesamt:</strong></div>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Anzahl" SortExpression="Akts" FooterStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <div class="gridData" align="right">
                                                                    <%# Eval("Akts")%></div>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <div class="gridData">
                                                                    <strong>
                                                                        <%=GetTotalAkts()%></strong></div>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Anteil" SortExpression="PercentAkt" FooterStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <div class="gridData" align="right">
                                                                    <%# Eval("Percent") %></div>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <div class="gridData">
                                                                    <strong>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;100,00&nbsp;&#37;</strong></div>
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
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" valign="top" class="tblHeader" colspan="2">
                                                Status Auftr&auml;ge in Bearbeitung
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tblData1" colspan="2">
                                                <asp:GridView ID="gvInProgress" runat="server" AllowSorting="true" AutoGenerateColumns="False" CellPadding="2" CellSpacing="1" ShowFooter="True" BorderStyle="Inset"
                                                    Width="100%" OnSorting="gvInProgress_Sorting">
                                                    <RowStyle BackColor="#EFF3FB" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="" SortExpression="Description" FooterStyle-HorizontalAlign="Right" HeaderStyle-Width="500px">
                                                            <ItemTemplate>
                                                                <div class="gridData">
                                                                    <%# Eval("Description")%>
                                                                </div>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <div class="gridData">
                                                                    <strong>Insgesamt:</strong></div>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Anzahl" SortExpression="Akts" FooterStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <div class="gridData" align="right">
                                                                    <%# Eval("Akts")%></div>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <div class="gridData">
                                                                    <strong>
                                                                        <%=GetTotalInProgress()%></strong></div>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Anteil" SortExpression="PercentAkt" FooterStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <div class="gridData" align="right">
                                                                    <%# Eval("Percent") %></div>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <div class="gridData">
                                                                    <strong>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<%=GetTotalInProgressPct()%></strong></div>
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
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" class="tblHeader" colspan="2">
                                                Status Auftr&auml;ge erledigt
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tblData1" colspan="2">
                                                <asp:GridView ID="gvFinished" runat="server" AllowSorting="true" AutoGenerateColumns="False" CellPadding="2" CellSpacing="1" ShowFooter="True" BorderStyle="Inset"
                                                    Width="100%" OnSorting="gvFinished_Sorting">
                                                    <RowStyle BackColor="#EFF3FB" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="" SortExpression="Description" FooterStyle-HorizontalAlign="Right" HeaderStyle-Width="500px">
                                                            <ItemTemplate>
                                                                <div class="gridData">
                                                                    <%# Eval("Description")%>
                                                                </div>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <div class="gridData">
                                                                    <strong>Insgesamt:</strong></div>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Anzahl" SortExpression="Akts" FooterStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <div class="gridData" align="right">
                                                                    <%# Eval("Akts")%></div>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <div class="gridData">
                                                                    <strong>
                                                                        <%=GetTotalFinished()%></strong></div>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Anteil" SortExpression="PercentAkt" FooterStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <div class="gridData" align="right">
                                                                    <%# Eval("Percent") %></div>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <div class="gridData">
                                                                    <strong>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<%=GetTotalFinishedPct()%></strong></div>
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
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" class="tblHeader" colspan="2">
                                                Status Auftr&auml;ge erfolgreich erledigt
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tblData1">
                                                <asp:GridView ID="gvSuccessActions" runat="server" AllowSorting="true" AutoGenerateColumns="False" CellPadding="2" CellSpacing="1" ShowFooter="True" BorderStyle="Inset"
                                                    Width="100%" OnSorting="gvSuccessActions_Sorting">
                                                    <RowStyle BackColor="#EFF3FB" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="" SortExpression="Description" FooterStyle-HorizontalAlign="Right" HeaderStyle-Width="500px">
                                                            <ItemTemplate>
                                                                <div class="gridData">
                                                                    <%# Eval("Description")%>
                                                                </div>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <div class="gridData">
                                                                    <strong>Insgesamt:</strong></div>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Anzahl" SortExpression="Akts" FooterStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <div class="gridData" align="right">
                                                                    <%# Eval("Akts")%></div>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <div class="gridData">
                                                                    <strong>
                                                                        <%=GetTotalSuccessfull()%></strong></div>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Anteil" SortExpression="PercentAkt" FooterStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <div class="gridData" align="right">
                                                                    <%# Eval("Percent") %></div>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <div class="gridData">
                                                                    <strong>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<%=GetTotalSuccessfullPct()%></strong></div>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <FooterStyle BackColor="#d6f5a7" Font-Bold="True" ForeColor="White" />
                                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                                    <HeaderStyle CssClass="tblCol1" Font-Bold="True" ForeColor="#000000" />
                                                    <AlternatingRowStyle BackColor="White" />
                                                </asp:GridView>
                                            </td>
                                            <td class="tblDataAll">
                                                <asp:Chart ID="chrtSuccess" runat="server" Palette="BrightPastel" BackColor="WhiteSmoke" Height="170px" BorderDashStyle="Solid" BackSecondaryColor="White"
                                                    BackGradientStyle="TopBottom" BorderWidth="2" BorderColor="26, 59, 105"  EnableViewState="true">
                                                    
                                                    <BorderSkin SkinStyle="Emboss"></BorderSkin>
                                                    <Series>
                                                        <asp:Series Name="Series1" BorderColor="180, 26, 59, 105" Color="220, 65, 140, 240" />
                                                    </Series>
                                                    <ChartAreas>
                                                        <asp:ChartArea Name="ChartArea1" BorderColor="64, 64, 64, 64" BackSecondaryColor="Transparent" BackColor="Transparent" ShadowColor="Transparent" BorderWidth="0">
                                                            <AxisY LineColor="64, 64, 64, 64">
                                                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                                                <MajorGrid LineColor="64, 64, 64, 64" />
                                                            </AxisY>
                                                            <AxisX Interval="1" LineColor="64, 64, 64, 64">
                                                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                                                <MajorGrid LineColor="64, 64, 64, 64" />
                                                            </AxisX>
                                                        </asp:ChartArea>
                                                    </ChartAreas>
                                                </asp:Chart>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" class="tblHeader" colspan="2">
                                                Status Auftr&auml;ge erfolglos erledigt
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tblData1">
                                                <asp:GridView ID="gvUnSuccessActions" runat="server" AllowSorting="true" AutoGenerateColumns="False" CellPadding="2" CellSpacing="1" ShowFooter="True" BorderStyle="Inset"
                                                    Width="100%" OnSorting="gvUnSuccessActions_Sorting">
                                                    <RowStyle BackColor="#EFF3FB" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="" SortExpression="Description" FooterStyle-HorizontalAlign="Right" HeaderStyle-Width="500px">
                                                            <ItemTemplate>
                                                                <div class="gridData">
                                                                    <%# Eval("Description")%>
                                                                </div>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <div class="gridData">
                                                                    <strong>Insgesamt:</strong></div>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Anzahl" SortExpression="Akts" FooterStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <div class="gridData" align="right">
                                                                    <%# Eval("Akts")%></div>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <div class="gridData">
                                                                    <strong>
                                                                        <%=GetTotalUnSuccessfull()%></strong></div>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Anteil" SortExpression="PercentAkt" FooterStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <div class="gridData" align="right">
                                                                    <%# Eval("Percent") %></div>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <div class="gridData">
                                                                    <strong>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<%=GetTotalUnSuccessfullPct()%></strong></div>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <FooterStyle BackColor="#d6f5a7" Font-Bold="True" ForeColor="White" />
                                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                                    <HeaderStyle CssClass="tblCol1" Font-Bold="True" ForeColor="#000000" />
                                                    <AlternatingRowStyle BackColor="White" />
                                                </asp:GridView>
                                            </td>
                                            <td class="tblDataAll">
                                                <asp:Chart ID="chrtUnSuccess" runat="server" Palette="BrightPastel" BackColor="WhiteSmoke" 
                                                    Height="240px" BorderDashStyle="Solid" BackSecondaryColor="White"
                                                    BackGradientStyle="TopBottom" BorderWidth="2" BorderColor="26, 59, 105"  EnableViewState="true">
                                                    
                                                    <BorderSkin SkinStyle="Emboss"></BorderSkin>
                                                    <Series>
                                                        <asp:Series Name="Series1" BorderColor="180, 26, 59, 105" Color="220, 65, 140, 240" />
                                                    </Series>
                                                    <ChartAreas>
                                                        <asp:ChartArea Name="ChartArea1" BorderColor="64, 64, 64, 64" BackSecondaryColor="Transparent" BackColor="Transparent" ShadowColor="Transparent" BorderWidth="0">
                                                            <AxisY LineColor="64, 64, 64, 64">
                                                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                                                <MajorGrid LineColor="64, 64, 64, 64" />
                                                            </AxisY>
                                                            <AxisX Interval="1" LineColor="64, 64, 64, 64">
                                                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                                                <MajorGrid LineColor="64, 64, 64, 64" />
                                                            </AxisX>
                                                        </asp:ChartArea>
                                                    </ChartAreas>
                                                </asp:Chart>
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
