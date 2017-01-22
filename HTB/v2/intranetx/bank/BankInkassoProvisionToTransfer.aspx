<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="BankInkassoProvisionToTransfer.aspx.cs" Inherits="HTB.v2.intranetx.bank.BankInkassoProvisionToTransfer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>HTB.ASP [ Konto Monatlicher Status ]</title>
    <style type="text/css">
        body
        {
            margin-left: 5px;
            margin-top: 5px;
            margin-right: 5px;
            margin-bottom: 5px;
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
        .style1
        {
            color: #FF0000;
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
                            <a href="/v2/intranet/intranet/intranet.asp">Intranet</a> |
                            <a href="/v2/intranet/intranet/mydata.asp"> Meine Daten</a> | 
                            <a href="Bank.aspx">Bank</a> | Inkassoprovisionen | 
                            <asp:HyperLink runat="server" NavigateUrl="javascript:window.print()">Drucken</asp:HyperLink>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" EnablePartialRendering="true" EnableScriptLocalization="true" />
    <asp:UpdatePanel ID="updPanel1" runat="server">
        <ContentTemplate>
            <table width="100%" border="0" cellspacing="0" bgcolor="#FFFFFF">
                <tr>
                    <td>
                        <table border="0" align="center" cellpadding="3" cellspacing="0">
                            <tr>
                                <td colspan="2" class="tblHeader" title="Inkassoprovisionen">
                                    Inkassoprovisionen
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <ctl:message ID="ctlMessage" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table border="0" align="center" cellpadding="3" cellspacing="0">
                                        <tr>
                                            <td class="EditCaptionTopLine">
                                                <div align="right">
                                                    <strong>Datum</strong>:&nbsp;</div>
                                            </td>
                                            <td class="EditDataTopLine">
                                                <asp:Label runat="server" ID="lblFrom" Text="&nbsp;vom&nbsp;"/>
                                                <asp:TextBox ID="txtDateFrom" runat="server" MaxLength="10" ToolTip="Datum" onFocus="this.style.backgroundColor='#DFF4FF'"
                                                    onBlur="this.style.backgroundColor=''" size="10" />
                                                <ajax:MaskedEditExtender ID="Datum_MaskedEditExtender" runat="server" Century="2000" TargetControlID="txtDateFrom" Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                                    OnInvalidCssClass="MaskedEditError" MaskType="Date" ErrorTooltipEnabled="True" />
                                                <ajax:MaskedEditValidator ID="Datum_MaskedEditValidator" runat="server" ControlExtender="Datum_MaskedEditExtender" ControlToValidate="txtDateFrom" InvalidValueMessage="Datum is ung&uuml;ltig!"
                                                    Display="Dynamic" InvalidValueBlurredMessage="*" />
                                                <ajax:CalendarExtender ID="Datum_CalendarExtender" runat="server" TargetControlID="txtDateFrom" PopupButtonID="Datum_CalendarButton" />
                                                <asp:ImageButton runat="Server" ID="Datum_CalendarButton" ImageUrl="~/v2/intranet/images/CalendarHS.png" AlternateText="Click to show calendar" />
                                                
                                                <asp:Label runat="server" ID="lblTo" Text="&nbsp;&nbsp;&nbsp;bis&nbsp;"/>

                                                <asp:TextBox ID="txtDateTo" runat="server" MaxLength="10" ToolTip="Datum" onFocus="this.style.backgroundColor='#DFF4FF'"
                                                    onBlur="this.style.backgroundColor=''" size="10" />
                                                <ajax:MaskedEditExtender ID="Datum_MaskedEditExtender2" runat="server" Century="2000" TargetControlID="txtDateTo" Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                                    OnInvalidCssClass="MaskedEditError" MaskType="Date" ErrorTooltipEnabled="True" />
                                                <ajax:MaskedEditValidator ID="Datum_MaskedEditValidator2" runat="server" ControlExtender="Datum_MaskedEditExtender2" ControlToValidate="txtDateTo" InvalidValueMessage="Datum is ung&uuml;ltig!"
                                                    Display="Dynamic" InvalidValueBlurredMessage="*" />
                                                <ajax:CalendarExtender ID="Datum_CalendarExtender2" runat="server" TargetControlID="txtDateTo" PopupButtonID="Datum_CalendarButton2" />
                                                <asp:ImageButton runat="Server" ID="Datum_CalendarButton2" ImageUrl="~/v2/intranet/images/CalendarHS.png" AlternateText="Click to show calendar" />
                                            </td>
                                        </tr>
                                        <tr id="trEmpty1" runat="server" visible="false">
                                            <td colspan="2" class="EditData">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">
                                                    &nbsp;</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:Button ID="btnCalculate" runat="server" class="btnSave" title="Go" Text="Go" OnClick="btnCalculate_Click" />
                                                <asp:LinkButton ID="btnNew" runat="server" title="Neu" Text="Neu" OnClick="btnNew_Click" Visible="false" />
                                                <asp:LinkButton ID="btnSave" runat="server" title="Speichern" Text="Speichern" OnClick="btnSave_Click" Visible="false" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table border="0" align="center" cellpadding="3" cellspacing="0">
                                        <tr>
                                            <td valign="top" class="dataTable">
                                                <asp:GridView ID="gvProvisionSummary" runat="server" AllowSorting="false" AutoGenerateColumns="False" CellPadding="2" CellSpacing="2" BorderStyle="Inset" Width="100%"
                                                    AlternatingRowStyle-BackColor="AntiqueWhite" BorderWidth="1" BorderColor="black" ShowFooter="true" BackColor="white" CssClass="docText">
                                                    <RowStyle />
                                                    <Columns>
                                                        <asp:BoundField DataField="UserName" HeaderText="Aussendienst" />
                                                        <asp:TemplateField HeaderText="Provision">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblProvision" runat="server" Text='<%# Eval("AktIntActionProvision")%>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                            <FooterTemplate>
                                                                <strong>
                                                                    <%= GetTotalProvision()%>
                                                                </strong>
                                                            </FooterTemplate>
                                                            <FooterStyle CssClass="tblDataAll" HorizontalAlign="Right" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" class="dataTable">
                                                <asp:GridView ID="gvProvisionDetail" runat="server" AllowSorting="false" AutoGenerateColumns="False" CellPadding="2" CellSpacing="2" BorderStyle="Inset" Width="100%"
                                                    AlternatingRowStyle-BackColor="AntiqueWhite" BorderWidth="1" BorderColor="black" ShowFooter="true" BackColor="white" CssClass="docText">
                                                    <RowStyle />
                                                    <Columns>
                                                        <asp:BoundField DataField="UserName" HeaderText="Aussendienst" />
                                                        <asp:BoundField DataField="CustInkAktID" HeaderText="Inkassoaktenzahl" />
                                                        <asp:BoundField DataField="AktIntID" HeaderText="Interventionsaktenzahl" />
                                                        <asp:BoundField DataField="AktIntActionDate" HeaderText="Aktionsdatum" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:TemplateField HeaderText="Provision">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblProvision" runat="server" Text='<%# Eval("AktIntActionProvision")%>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                            <FooterTemplate>
                                                                <strong>
                                                                    <%= GetTotalProvision()%>
                                                                </strong>
                                                            </FooterTemplate>
                                                            <FooterStyle CssClass="tblDataAll" HorizontalAlign="Right" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" class="tblFooter1">
                                                <div align="right">
                                                    <asp:Button ID="btnCancel" runat="server" class="btnCancel" title="Fertig" Text="Fertig" OnClick="btnCancel_Click" />
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
            <asp:HiddenField runat="server" ID="hdnStart"/>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
    <ctl:footer ID="ftr" runat="server" />
</body>
</html>
