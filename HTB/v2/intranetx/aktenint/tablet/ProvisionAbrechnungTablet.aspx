<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProvisionAbrechnungTablet.aspx.cs" Inherits="HTB.v2.intranetx.aktenint.tablet.ProvisionAbrechnungTablet" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>HTB [ Provisionen ]</title>
</head>
<body>
    <ctl:headerNoMenuTablet runat="server" />
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" EnablePartialRendering="true" EnableScriptLocalization="true" AsyncPostBackTimeout="500" />
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
                                                            
                                                            <strong>Datum&nbsp;&nbsp;&nbsp;&nbsp;von</strong>:&nbsp;
                                                            <asp:TextBox ID="txtDateStart" runat="server" MaxLength="10" ToolTip="Zahlungsdatum" onFocus="this.style.backgroundColor='#DFF4FF'" onBlur="this.style.backgroundColor=''" size="10" />
                                                            <ajax:MaskedEditExtender ID="Datum_MaskedEditExtender" runat="server" Century="2000" TargetControlID="txtDateStart" Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" MaskType="Date" ErrorTooltipEnabled="True" />
                                                            <ajax:MaskedEditValidator ID="Datum_MaskedEditValidator" runat="server" ControlExtender="Datum_MaskedEditExtender" ControlToValidate="txtDateStart" InvalidValueMessage="Datum is ung&uuml;ltig!" Display="Dynamic" InvalidValueBlurredMessage="*" />
                                                            <ajax:CalendarExtender ID="Datum_CalendarExtender" runat="server" TargetControlID="txtDateStart" PopupButtonID="Datum_CalendarButton" />
                                                            <asp:ImageButton runat="Server" ID="Datum_CalendarButton" ImageUrl="~/v2/intranet/images/CalendarHS.png" AlternateText="Click to show calendar" />
                                                        
                                                            &nbsp;&nbsp;&nbsp;<strong>bis</strong>:&nbsp;
                                                            <asp:TextBox ID="txtDateEnd" runat="server" MaxLength="10" ToolTip="Zahlungsdatum" onFocus="this.style.backgroundColor='#DFF4FF'" onBlur="this.style.backgroundColor=''" size="10" />
                                                            <ajax:MaskedEditExtender ID="Datum_MEE" runat="server" Century="2000" TargetControlID="txtDateEnd" Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" MaskType="Date" ErrorTooltipEnabled="True" />
                                                            <ajax:MaskedEditValidator ID="Datum_MEV" runat="server" ControlExtender="Datum_MEE" ControlToValidate="txtDateEnd" InvalidValueMessage="Datum is ung&uuml;ltig!" Display="Dynamic" InvalidValueBlurredMessage="*" />
                                                            <ajax:CalendarExtender ID="Datum_CE" runat="server" TargetControlID="txtDateEnd" PopupButtonID="Datum_CB" />
                                                            <asp:ImageButton runat="Server" ID="Datum_CB" ImageUrl="~/v2/intranet/images/CalendarHS.png" AlternateText="Click to show calendar" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tblDataAll" valign="top" colspan="2">
                                                            <asp:GridView ID="gvProvision" runat="server" Width="100%" 
                                                                AutoGenerateColumns="false" 
                                                                EmptyDataText="Keine Provisionen." 
                                                                ShowHeaderWhenEmpty="true" 
                                                                ShowFooter="true">
                                                                <HeaderStyle CssClass="dataTable" />
                                                                <AlternatingRowStyle CssClass="dataTableAlt" />
                                                                <RowStyle CssClass="dataTable" />
                                                                <Columns>
                                                                    <asp:BoundField DataField="Akt" HeaderText="Akt" HtmlEncode="False" />
                                                                    <asp:BoundField DataField="ActionDate" HeaderText="Datum" HtmlEncode="False" />
                                                                    <asp:BoundField DataField="ActionSB" HeaderText="SB" HtmlEncode="False" />
                                                                    <asp:BoundField DataField="Auftraggeber" HeaderText="Auftraggeber" HtmlEncode="False" />
                                                                    <asp:BoundField DataField="Gegner" HeaderText="Schuldner" HtmlEncode="False" />
                                                                    <asp:BoundField DataField="Action" HeaderText="Aktion" HtmlEncode="False" />
                                                                    <asp:BoundField DataField="Beleg" HeaderText="Beleg Nr." HtmlEncode="False" FooterText="Total:&nbsp;&nbsp;" >
                                                                        <FooterStyle Font-Bold="true" HorizontalAlign="Right"></FooterStyle>
                                                                    </asp:BoundField>
                                                                    <asp:TemplateField HeaderText="Provision" FooterStyle-Font-Bold="True" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                                                        <ItemTemplate>
                                                                            <%# Eval("Provision")%>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <%# GetTotalProvision() %>
                                                                        </FooterTemplate>
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
    <ctl:footer runat="server" />
</body>
</html>
