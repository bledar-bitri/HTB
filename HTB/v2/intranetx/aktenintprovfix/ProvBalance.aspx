<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProvBalance.aspx.cs" Inherits="HTB.v2.intranetx.aktenintprovfix.ProvBalance" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>HTB.ASP [ Erfolgsprovisionschema ]</title>
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
    <script type="text/javascript">
        function switchViews(obj, row) {
            var div = document.getElementById(obj);
            var img = document.getElementById('img' + obj);

            if (div.style.display == "none") {
                div.style.display = "inline";
                if (row == 'alt') {
                    img.src = "../../intranet/images/expand_button_down.png";
                    mce_src = "../../intranet/images/expand_button_down.png";
                }
                else {
                    img.src = "../../intranet/images/expand_button_down.png";
                    mce_src = "../../intranet/images/expand_button_down.png";
                }
                img.alt = "Close to view other customers";
            }
            else {
                div.style.display = "none";
                if (row == 'alt') {
                    img.src = "../../intranet/images/expand_button.png";
                    mce_src = "../../intranet/images/expand_button.png";
                }
                else {
                    img.src = "../../intranet/images/expand_button.png";
                    mce_src = "../../intranet/images/expand_button.png";
                }
                img.alt = "Expand to show orders";
            }
        }
    </script>
</head>
<body>
    <ctl:header ID="hdr" runat="server" />
    <table width="100%" border="0" cellpadding="1" cellspacing="0" bgcolor="#000000">
        <tr>
            <td>
                <table width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
                    <tr>
                        <td class="smallText">
                            <a href="../../intranet/intranet/intranet.asp">Intranet</a> | <a href="../../intranet/intranet/mydata.asp">Meine Daten</a> | Provisionsbilanz
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
                                            <td colspan="2" class="tblHeader" title="PROVISIONSBILANZ">
                                                Provisionsbilanz
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" class="tblDataAll">
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
                                                        <td class="EditData" valign="bottom" colspan="3">
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
                                                        <td class="EditData" valign="bottom" colspan="3">
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
                                                        <td class="tblDataAll" valign="top" colspan="2">
                                                            &nbsp;
                                                            <asp:GridView ID="gvHeader" runat="server" AutoGenerateColumns="False" DataKeyNames="UserID" 
                                                                OnDataBound="gvHeaderGrid_DataBound" AllowPaging="True" PageSize="20" ShowFooter="true"  CssClass="tblDataAll">

                                                                <HeaderStyle CssClass="dataTable" />
                                                                <RowStyle CssClass="dataTable" />
                                                                <AlternatingRowStyle CssClass="dataTableAlt" />
                                                                <Columns>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <a href="javascript:switchViews('div<%# Eval("UserID") %>', 'one');">
                                                                                <img id="imgdiv<%# Eval("UserID") %>" alt="Click to show/hide orders" border="0" src="../../intranet/images/expand_button.png" />
                                                                            </a>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblUserID" runat="server" Text='<%# Eval("UserID")%>'/>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Total ECP" FooterStyle-Font-Bold="True" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Right">
                                                                        <ItemTemplate>
                                                                            <%# Eval("User")%>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:Label ID="lblTotal" runat="server" Text="Total:"/>
                                                                        </FooterTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Total ECP" FooterStyle-Font-Bold="True" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                                                        <ItemTemplate>
                                                                            <%# Eval("TotalPrice")%>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <%# HTBUtilities.HTBUtils.FormatCurrency(GetTotalPrice()) %>
                                                                        </FooterTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Total Provision" FooterStyle-Font-Bold="True" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                                                        <ItemTemplate>
                                                                            <%# Eval("TotalProv")%>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <%# HTBUtilities.HTBUtils.FormatCurrency(GetTotalProvision()) %>
                                                                        </FooterTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Einkommen" FooterStyle-Font-Bold="True" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDifference" runat="server" Text='<%# Eval("Difference") %>' ForeColor='<%# Eval("ForeColor") %>' />
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <%# HTBUtilities.HTBUtils.FormatCurrency(GetDifference()) %>
                                                                        </FooterTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Akten" FooterStyle-Font-Bold="True" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                                                        <ItemTemplate>
                                                                            <%# Eval("Count")%>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <%# GetTotalCount() %>
                                                                        </FooterTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            </td></tr>
                                                                            <tr>
                                                                                <td colspan="100%">
                                                                                    <div id="div<%# Eval("UserID") %>" style="display: none; position: relative; left: 25px;">
                                                                                        <asp:GridView ID="gvDetail" runat="server" Width="80%" AutoGenerateColumns="false" EmptyDataText="Keine Aktionen für diesen Benutzer.">
                                                                                            <HeaderStyle CssClass="dataTable" />
                                                                                            <AlternatingRowStyle CssClass="dataTableAlt" />
                                                                                            <RowStyle CssClass="dataTable" />
                                                                                            <Columns>
                                                                                                <asp:TemplateField HeaderText="Aktenzahl" ItemStyle-HorizontalAlign="Center">
                                                                                                    <ItemTemplate>
                                                                                                        <%# Eval("AktIdLink") %>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                                <asp:BoundField DataField="AktType" HeaderText="Akttyp" HtmlEncode="False" />
                                                                                                <asp:BoundField DataField="Aktion" HeaderText="Action" HtmlEncode="False" />
                                                                                                <asp:BoundField DataField="ActionDate" HeaderText="Datum" HtmlEncode="False" />
                                                                                                <asp:BoundField DataField="ActionPrice" HeaderText="Preis (ECP)" ItemStyle-HorizontalAlign="Right" HtmlEncode="False" />
                                                                                                <asp:BoundField DataField="ActionProv" HeaderText="Provision (AD)" ItemStyle-HorizontalAlign="Right" HtmlEncode="False" />
                                                                                                <asp:TemplateField HeaderText="Einkommen" ItemStyle-HorizontalAlign="Right">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblActionDifference" runat="server" Text='<%# Eval("ActionDifference") %>' ForeColor='<%# Eval("ForeColor") %>' />
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                            </Columns>
                                                                                        </asp:GridView>
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                </Columns>
                                                            </asp:GridView>
                                                        </td>

                                                        <td class="tblDataAll" valign="top" colspan="2">
                                                            &nbsp;
                                                            <asp:GridView ID="gvAGHeader" runat="server" AutoGenerateColumns="False" DataKeyNames="AuftraggeberID" 
                                                                OnDataBound="gvAGHeaderGrid_DataBound" AllowPaging="True" PageSize="20" ShowFooter="true" CssClass="tblDataAll">

                                                                <HeaderStyle CssClass="dataTable" />
                                                                <RowStyle CssClass="dataTable" />
                                                                <AlternatingRowStyle CssClass="dataTableAlt" />
                                                                <Columns>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <a href="javascript:switchViews('div<%# Eval("AuftraggeberID") %>', 'one');">
                                                                                <img id="imgdiv<%# Eval("AuftraggeberID") %>" alt="Click to show/hide orders" border="0" src="../../intranet/images/expand_button.png" />
                                                                            </a>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblAGID" runat="server" Text='<%# Eval("AuftraggeberID")%>'/>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Total ECP" FooterStyle-Font-Bold="True" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Right">
                                                                        <ItemTemplate>
                                                                            <%# Eval("Auftraggeber")%>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:Label ID="lblTotal" runat="server" Text="Total:"/>
                                                                        </FooterTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Total ECP" FooterStyle-Font-Bold="True" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                                                        <ItemTemplate>
                                                                            <%# Eval("TotalPrice")%>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <%# HTBUtilities.HTBUtils.FormatCurrency(GetTotalPrice()) %>
                                                                        </FooterTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Total Provision" FooterStyle-Font-Bold="True" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                                                        <ItemTemplate>
                                                                            <%# Eval("TotalProv")%>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <%# HTBUtilities.HTBUtils.FormatCurrency(GetTotalProvision()) %>
                                                                        </FooterTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Einkommen" FooterStyle-Font-Bold="True" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDifference" runat="server" Text='<%# Eval("Difference") %>' ForeColor='<%# Eval("ForeColor") %>' />
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <%# HTBUtilities.HTBUtils.FormatCurrency(GetAGDifference()) %>
                                                                        </FooterTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Akten" FooterStyle-Font-Bold="True" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                                                        <ItemTemplate>
                                                                            <%# Eval("Count")%>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <%# GetAGTotalCount() %>
                                                                        </FooterTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            </td></tr>
                                                                            <tr>
                                                                                <td colspan="100%">
                                                                                    <div id="div<%# Eval("AuftraggeberID") %>" style="display: none; position: relative; left: 25px;">
                                                                                        <asp:GridView ID="gvAGDetail" runat="server" Width="80%" AutoGenerateColumns="false" EmptyDataText="Keine Aktionen für diesen Benutzer.">
                                                                                            <HeaderStyle CssClass="dataTable" />
                                                                                            <AlternatingRowStyle CssClass="dataTableAlt" />
                                                                                            <RowStyle CssClass="dataTable" />
                                                                                            <Columns>
                                                                                                <asp:TemplateField HeaderText="Aktenzahl" ItemStyle-HorizontalAlign="Center">
                                                                                                    <ItemTemplate>
                                                                                                        <%# Eval("AktIdLink") %>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                                <asp:BoundField DataField="AktType" HeaderText="Akttyp" HtmlEncode="False" />
                                                                                                <asp:BoundField DataField="Aktion" HeaderText="Action" HtmlEncode="False" />
                                                                                                <asp:BoundField DataField="ActionDate" HeaderText="Datum" HtmlEncode="False" />
                                                                                                <asp:BoundField DataField="ActionPrice" HeaderText="Preis (ECP)" ItemStyle-HorizontalAlign="Right" HtmlEncode="False" />
                                                                                                <asp:BoundField DataField="ActionProv" HeaderText="Provision (AD)" ItemStyle-HorizontalAlign="Right" HtmlEncode="False" />
                                                                                                <asp:TemplateField HeaderText="Einkommen" ItemStyle-HorizontalAlign="Right">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblActionDifference" runat="server" Text='<%# Eval("ActionDifference") %>' ForeColor='<%# Eval("ForeColor") %>' />
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                            </Columns>
                                                                                        </asp:GridView>
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                </Columns>
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" class="tblFooter1">
                                                <div align="right">
                                                    <asp:Button ID="btnSubmit" runat="server" class="btnSave" title="Fertig" Text="Fertig" OnClick="btnSubmit_Click" />
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
    <ctl:footer ID="ftr" runat="server" />
</body>
</html>
