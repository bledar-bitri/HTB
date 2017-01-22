<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProvFix.aspx.cs" Inherits="HTB.v2.intranetx.aktenintprovfix.ProvFix" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>HTB.ASP [ Provisionskontrolle ]</title>
    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
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
        
        OPTION.dis
        {
            background-color: white;
            color: #999999;
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
                            <a href="../../intranet/intranet/intranet.asp">Intranet</a> | <a href="../../intranet/intranet/mydata.asp">Meine Daten</a> | Provisionskontrolle
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <br />
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" EnablePartialRendering="true" EnableScriptLocalization="true" AsyncPostBackTimeOut="500"/>
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        function BeginRequestHandler(sender, args) {
            document.getElementById('<%= btnSubmit.ClientID %>').innerText = "Processing";
            document.getElementById('<%= btnSubmit.ClientID %>').disabled = true;
            document.getElementById('<%= btnShowBadProv.ClientID %>').innerText = "Processing";
            document.getElementById('<%= btnShowBadProv.ClientID %>').disabled = true;
            
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
                                    <table border="0" align="center" cellpadding="3" cellspacing="0">
                                        <tr>
                                            <td class="tblHeader" title="Provisionskontrolle">
                                                Provisionskontrolle
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tblDataAll">
                                                <ctl:message ID="ctlMessage" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tblDataAll">
                                                <table border="0" align="center" cellpadding="3" cellspacing="0">
                                                    <tr>
                                                        <td class="EditCaption">
                                                            <div align="right"><strong>Datum von</strong>:&nbsp;</div>
                                                        </td>
                                                        <td class="EditData" valign="bottom">
                                                            <asp:TextBox ID="txtDateStart" runat="server" MaxLength="10" ToolTip="Zahlungsdatum" onFocus="this.style.backgroundColor='#DFF4FF'" onBlur="this.style.backgroundColor=''"
                                                                size="10" />
                                                            <ajax:MaskedEditExtender ID="Datum_MaskedEditExtender" runat="server" Century="2000" TargetControlID="txtDateStart" Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                                                OnInvalidCssClass="MaskedEditError" MaskType="Date" ErrorTooltipEnabled="True" />
                                                            <ajax:MaskedEditValidator ID="Datum_MaskedEditValidator" runat="server" ControlExtender="Datum_MaskedEditExtender" ControlToValidate="txtDateStart" InvalidValueMessage="Datum is ung&uuml;ltig!"
                                                                Display="Dynamic" InvalidValueBlurredMessage="*" />
                                                            <ajax:CalendarExtender ID="Datum_CalendarExtender" runat="server" TargetControlID="txtDateStart" PopupButtonID="Datum_CalendarButton" />
                                                            <asp:ImageButton runat="Server" ID="Datum_CalendarButton" ImageUrl="~/v2/intranet/images/CalendarHS.png" AlternateText="Click to show calendar" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="EditCaption">
                                                            <div align="right"><strong>Datum bis</strong>:&nbsp;</div>
                                                        </td>
                                                        <td class="EditData" valign="bottom">
                                                            <asp:TextBox ID="txtDateEnd" runat="server" MaxLength="10" ToolTip="Zahlungsdatum" onFocus="this.style.backgroundColor='#DFF4FF'" onBlur="this.style.backgroundColor=''"
                                                                size="10" />
                                                            <ajax:MaskedEditExtender ID="Datum_MEE" runat="server" Century="2000" TargetControlID="txtDateEnd" Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                                                OnInvalidCssClass="MaskedEditError" MaskType="Date" ErrorTooltipEnabled="True" />
                                                            <ajax:MaskedEditValidator ID="Datum_MEV" runat="server" ControlExtender="Datum_MEE" ControlToValidate="txtDateEnd" InvalidValueMessage="Datum is ung&uuml;ltig!"
                                                                Display="Dynamic" InvalidValueBlurredMessage="*" />
                                                            <ajax:CalendarExtender ID="Datum_CE" runat="server" TargetControlID="txtDateEnd" PopupButtonID="Datum_CB" />
                                                            <asp:ImageButton runat="Server" ID="Datum_CB" ImageUrl="~/v2/intranet/images/CalendarHS.png" AlternateText="Click to show calendar" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="EditCaption">
                                                            <div align="right"><strong>Show Invalid Provision</strong>:&nbsp;</div>
                                                        </td>
                                                        <td class="EditData" valign="bottom">
                                                            <asp:CheckBox ID="chkShowInvalidProvision" runat="server"/>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="EditCaption">
                                                            <div align="right"><strong>Show Invalid Price</strong>:&nbsp;</div>
                                                        </td>
                                                        <td class="EditData" valign="bottom">
                                                            <asp:CheckBox ID="chkShowInvalidPrice" runat="server"/>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tblCol1"  colspan="2">
                                                            <asp:GridView ID="gvBadProv" DataSourceID="ObjectDataSource1" runat="server" AutoGenerateColumns="False"
                                                                CellPadding="2" CellSpacing="1" BorderStyle="Inset" Width="100%" 
                                                                OnDataBound="gvBadProv_DataBound" >
                                                                <RowStyle BackColor="#EFF3FB" />
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="ID" SortExpression="ActionIdLink">
                                                                        <ItemTemplate>
                                                                            <div class="gridData">
                                                                                <%# Eval("ActionIdLink")%></div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Benutzer" SortExpression="UserName">
                                                                        <ItemTemplate>  
                                                                            <%# Eval("UserName")%>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Auftraggeber" SortExpression="AuftraggeberName">
                                                                        <ItemTemplate>
                                                                            <div class="gridData"><%# Eval("AuftraggeberName")%></div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Akttyp" SortExpression="AktTypeCaption">
                                                                        <ItemTemplate>
                                                                            <div class="gridData">
                                                                                <%# Eval("AktTypeCaption")%></div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Aktion" SortExpression="ActionTypeCaption">
                                                                        <ItemTemplate>
                                                                            <div class="gridData">
                                                                                <%# Eval("ActionTypeCaption")%></div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Betrag" SortExpression="CollectedAmount">
                                                                        <ItemTemplate>
                                                                            <div class="gridData">
                                                                                <%# HTBUtilities.HTBUtils.FormatCurrency((double)Eval("CollectedAmount"))%></div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="ECP Price" SortExpression="ActionPrice">
                                                                        <ItemTemplate>
                                                                            <div class="gridData">
                                                                                <%# HTBUtilities.HTBUtils.FormatCurrency((double)Eval("ActionPrice"))%></div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                
                                                                    <asp:TemplateField HeaderText="Provision" SortExpression="ActionProvision">
                                                                        <ItemTemplate>
                                                                            <div class="gridData">
                                                                                <%# HTBUtilities.HTBUtils.FormatCurrency((double)Eval("ActionProvision"))%></div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    
                                                                    <asp:TemplateField HeaderText="Description" SortExpression="ErrorDescription">
                                                                        <ItemTemplate>
                                                                            <div class="gridData">
                                                                                <%# Eval("ErrorDescription")%></div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    

                                                                    <asp:TemplateField HeaderText="Neu ECP Price" SortExpression="CalculatedPrice">
                                                                        <ItemTemplate>
                                                                            <div class="gridData">
                                                                                <%# HTBUtilities.HTBUtils.FormatCurrency((double)Eval("CalculatedPrice"))%></div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                     <asp:TemplateField HeaderText="Neu Provision" SortExpression="CalculatedProvision">
                                                                        <ItemTemplate>
                                                                            <div class="gridData">
                                                                                <%# HTBUtilities.HTBUtils.FormatCurrency((double)Eval("CalculatedProvision"))%></div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                                                <HeaderStyle CssClass="tblCol1"  Font-Bold="True" ForeColor="#000000"/>
                                                                <AlternatingRowStyle BackColor="White" />
                                                            </asp:GridView>
                                                            <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetInvalidActions" EnablePaging="true" TypeName="HTB.v2.intranetx.aktenintprovfix.ProvFix"
                                                                SelectCountMethod="GetTotalActionsCount"></asp:ObjectDataSource>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tblFooter1">
                                                <div align="right">
                                                    <asp:Button ID="btnShowBadProv" runat="server" class="btnSave" title="Show Invalid Provisionen" Text="Show Invalid Provisionen" OnClick="btnShowBadProv_Click" />
                                                    <asp:Button ID="btnSubmit" runat="server" class="btnSave" title="Fix" Text="Fix All" OnClick="btnSubmit_Click" />
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
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
