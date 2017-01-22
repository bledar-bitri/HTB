<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AktenInk.aspx.cs" Inherits="HTB.v2.intranetx.customer.AktenInk" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>HTB.ASP [CollectionInvoiceakte]</title>
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
                            <asp:HyperLink runat="server" NavigateUrl="Customer.aspx">Kundenportal</asp:HyperLink> | CollectionInvoiceakte | <asp:HyperLink runat="server" NavigateUrl="javascript:window.print()">Drucken</asp:HyperLink>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <br />
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" EnablePartialRendering="true" EnableScriptLocalization="true" >
        <Scripts>
            <asp:ScriptReference Path="~/v2/intranetx/scripts/AjaxHacks.js" />
        </Scripts>   
    </asp:ScriptManager>
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
                                                                <a href="Customer.aspx" class="smallText" title="Zur&uuml;ck zum Hauptmen&uuml;" >Zur&uuml;ck</a>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="10%">
                                                            <span class="smallText">&nbsp;&nbsp;Schuldner:</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtGegnerName" runat="server" class="docText" size="65" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span class="smallText">&nbsp;&nbsp;AZ:</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtAZ" runat="server" class="docText" size="35" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span class="smallText">&nbsp;&nbsp;Rechnungsnummer:</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtInvoiceNumber" runat="server" class="docText" size="35" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span class="smallText">&nbsp;&nbsp;Status:</span>
                                                        </td>
                                                        <td>
                                                            <div align="left">
                                                                <asp:DropDownList ID="ddlAktStatus" runat="server" />
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnSubmit" runat="server" Text="Suchen" class="btnSave" OnClick="btnSubmit_Click" />
                                                            &nbsp;&nbsp;&nbsp;&nbsp;
                                                            <asp:HyperLink ID="newAkt" runat="server" Text="Neuer CollectionInvoiceakt" NavigateUrl="~/v2/intranetx/customer/CustomerNewAktInk.aspx"/>
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
                                                CollectionInvoiceakte - Forderungs&uuml;bersicht f&uuml;r
                                                <asp:Label ID="lblKlientInfo" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="12">
                                                <ctl:message ID="ctlMessage" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tblCol1">
                                                <asp:GridView ID="gvAkts" DataKeyNames="AktID" DataSourceID="ObjectDataSource1" runat="server" AllowPaging="true" PageSize="10" AllowSorting="true" AutoGenerateColumns="False"
                                                    CellPadding="2" CellSpacing="1" BorderStyle="Inset" Width="100%" OnDataBound="gvAkts_DataBound">
                                                    <RowStyle BackColor="#EFF3FB" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAktId" runat="server" Text='<%# Eval("AktId")%>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ItemStyle-VerticalAlign="Top">
                                                            <ItemTemplate>
                                                                <div class="gridData">
                                                                    <%--<asp:LinkButton ID="btnShowDirectPay" runat="server" class="smallText" OnClick="btnShowDirectPay_Click" CommandArgument='<%# Eval("RowId")%>'>
                                                                        <asp:Image ID="imgDZ" runat="server" ImageUrl="~/v2/intranet/images/Accounting.png"  AlternateText="Direktzahlung"/>
                                                                    </asp:LinkButton>
                                                                    &nbsp;
                                                                    <asp:LinkButton ID="btnShowEMail" runat="server" class="smallText" OnClick="btnShowEMail_Click" CommandArgument='<%# Eval("RowId")%>'>
                                                                        <asp:Image ID="imgEmail" runat="server" ImageUrl="~/v2/intranet/images/kuvert16.gif"  AlternateText="Email ECP"/>
                                                                    </asp:LinkButton>
--%>
                                                                    <asp:ImageButton ID="btnShowDirectPay" runat="server" class="smallText" OnClick="btnShowDirectPay_Click" CommandArgument='<%# Eval("RowId")%>' ImageUrl="~/v2/intranet/images/Accounting.png"  AlternateText="Direktzahlung"/>
                                                                    &nbsp;
                                                                    <asp:ImageButton ID="btnShowEMail" runat="server" class="smallText" OnClick="btnShowEMail_Click" CommandArgument='<%# Eval("RowId")%>' ImageUrl="~/v2/intranet/images/kuvert16.gif"  AlternateText="Email ECP"/>
                                                                    <br />
                                                                    <asp:UpdatePanel ID="pnlDirectPay" runat="server" Visible="false">
                                                                        <ContentTemplate>
                                                                            <table cellpadding="3" cellspacing="0">
                                                                                <tr>
                                                                                    <td colspan="2">
                                                                                        <img name="" src="" width="2" height="2" alt="">
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td colspan="2" class="tblHeader">
                                                                                        Zahlung
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td colspan="2" class="tblDataAllTransparent">
                                                                                        <ctl:message ID="cltPaymentMessage" runat="server" Visible="false" />
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <tr>
                                                                                        <td class="EditCaptionTopLine">
                                                                                            <div align="right">Zahlungseingangsdatum:&nbsp;</div>
                                                                                        </td>
                                                                                        <td class="EditDataTopLine">
                                                                                            <asp:TextBox runat="server" ID="txtPaymentDate" MaxLength="10" ToolTip="Rate Datum" Text='<%# DateTime.Now.ToShortDateString()%>' ValidationGroup="DateVG" onFocus="this.style.backgroundColor='#DFF4FF'"
                                                                                                onBlur="this.style.backgroundColor=''" size="10" />
                                                                                            <ajax:MaskedEditExtender ID="Datum_MaskedEditExtender" runat="server" Century="2000" TargetControlID="txtPaymentDate" Mask="99/99/9999" MessageValidatorTip="true"
                                                                                                OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" MaskType="Date" ErrorTooltipEnabled="True" />
                                                                                            <ajax:MaskedEditValidator ID="Datum_MaskedEditValidator" runat="server" ControlExtender="Datum_MaskedEditExtender" ControlToValidate="txtPaymentDate" InvalidValueMessage="Datum is ung&uuml;ltig!"
                                                                                                Display="Dynamic" InvalidValueBlurredMessage="*" ValidationGroup="DateVG" />
                                                                                            <ajax:CalendarExtender ID="Datum_CalendarExtender" runat="server" TargetControlID="txtPaymentDate" PopupButtonID="Datum_CalendarButton" />
                                                                                            <asp:ImageButton runat="Server" ID="Datum_CalendarButton" ImageUrl="~/v2/intranet/images/CalendarHS.png" AlternateText="Click to show calendar" />
                                                                                        </td>
                                                                                    </tr>
<%--                                                                                    <tr>--%>
<%--                                                                                        <td valign="middle" class="EditCaption">--%>
<%--                                                                                            <div align="right">--%>
<%--                                                                                                Buchungstext:&nbsp;</div>--%>
<%--                                                                                        </td>--%>
<%--                                                                                        <td class="EditData">--%>
<%--                                                                                            <asp:TextBox runat="server" ID="txtPaymentText" type="text" class="smallText" size="70" MaxLength="255" />--%>
<%--                                                                                        </td>--%>
<%--                                                                                    </tr>--%>
                                                                                    <tr>
                                                                                        <td valign="middle" class="EditCaption">
                                                                                            <div align="right">
                                                                                                Summe:&nbsp;</div>
                                                                                        </td>
                                                                                        <td class="EditData">
                                                                                            <asp:TextBox runat="server" ID="txtAmount" class="smallText" onFocus="this.style.backgroundColor='#DFF4FF';" onBlur="this.style.backgroundColor='';" size="12" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td valign="top" class="EditCaption">
                                                                                            <div align="right">
                                                                                                Bemerkungen an E.C.P. AD:&nbsp;</div>
                                                                                        </td>
                                                                                        <td class="EditData">
                                                                                            <asp:TextBox runat="server" ID="txtMemo" TextMode="MultiLine" Columns="70" Rows="5" class="smallText" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td colspan="2">
                                                                                            <img name="" src="" width="2" height="2" alt="">
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            &nbsp;
                                                                                        </td>
                                                                                        <td>
                                                                                            <div align="right">
                                                                                                <asp:Button ID="btnSendDirectPay" runat="server" class="btnSave" Text="Absenden" OnClick="btnSendDirectPay_Click" CommandArgument='<%# Eval("RowId")%>' />
                                                                                                <asp:Button ID="btnHideDirectPay" runat="server" class="btnCancel" Text="Abbrechen" OnClick="btnHideDirectPay_Click" CommandArgument='<%# Eval("RowId")%>' />
                                                                                            </div>
                                                                                        </td>
                                                                                    </tr>
                                                                            </table>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                    <asp:UpdateProgress ID="progresDirectPay" runat="server" AssociatedUpdatePanelID="pnlDirectPay">
                                                                        <ProgressTemplate>
                                                                            &nbsp;<br />
                                                                            <div class="smallText">
                                                                                Direktzahlung läuft... bitte warten!</div>
                                                                            <br />
                                                                            &nbsp;
                                                                        </ProgressTemplate>
                                                                    </asp:UpdateProgress>
                                                                    <asp:UpdatePanel ID="pnlEMail" runat="server" Visible="false">
                                                                        <ContentTemplate>
                                                                            <table cellpadding="3" cellspacing="0">
                                                                                <tr>
                                                                                    <td colspan="2">
                                                                                        <img name="" src="" width="2" height="2" alt="">
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td colspan="2" class="tblHeader">
                                                                                        BENACHRICHTIGUNG AN E.C.P. INKASSO KG
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td colspan="2" class="tblDataAllTransparent">
                                                                                        <ctl:message ID="cltEMailMessage" runat="server" Visible="false" />
                                                                                    </td>
                                                                                </tr>
<%--                                                                                <tr>--%>
<%--                                                                                    <td valign="middle" class="EditCaption">--%>
<%--                                                                                        <div align="right">--%>
<%--                                                                                            Name:&nbsp;</div>--%>
<%--                                                                                    </td>--%>
<%--                                                                                    <td class="EditData">--%>
<%--                                                                                        <asp:TextBox runat="server" ID="txtFromName" type="text" class="smallText" size="70" MaxLength="255" />--%>
<%--                                                                                    </td>--%>
<%--                                                                                </tr>--%>
<%--                                                                                <tr>--%>
<%--                                                                                    <td valign="middle" class="EditCaption">--%>
<%--                                                                                        <div align="right">--%>
<%--                                                                                            Ihre Emailadresse:&nbsp;</div>--%>
<%--                                                                                    </td>--%>
<%--                                                                                    <td class="EditData">--%>
<%--                                                                                        <asp:TextBox runat="server" ID="txtEMailFrom" type="text" class="smallText" size="70" MaxLength="255" />--%>
<%--                                                                                    </td>--%>
<%--                                                                                </tr>--%>
<%--                                                                                <tr>--%>
<%--                                                                                    <td valign="middle" class="EditCaption">--%>
<%--                                                                                        <div align="right">--%>
<%--                                                                                            Betreff:&nbsp;</div>--%>
<%--                                                                                    </td>--%>
<%--                                                                                    <td class="EditData">--%>
<%--                                                                                        <asp:TextBox runat="server" ID="txtSubject" type="text" class="smallText" size="70" MaxLength="255" />--%>
<%--                                                                                    </td>--%>
<%--                                                                                </tr>--%>
                                                                                <tr>
                                                                                    <td valign="top" class="EditCaptionTopLine">
                                                                                        <div align="right">
                                                                                            Text:&nbsp;</div>
                                                                                    </td>
                                                                                    <td class="EditDataTopLine">
                                                                                        <asp:TextBox runat="server" ID="txtEmailBody" TextMode="MultiLine" Columns="70" Rows="5" class="smallText" />
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td colspan="2">
                                                                                        <img name="" src="" width="2" height="2" alt="">
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <div align="right">
                                                                                            <asp:Button ID="btnSendEMail" runat="server" class="btnSave" Text="Absenden" OnClick="btnSendEMail_Click" CommandArgument='<%# Eval("RowId")%>' />
                                                                                            <asp:Button ID="btnHideEMail" runat="server" class="btnCancel" Text="Abbrechen" OnClick="btnHideEMail_Click" CommandArgument='<%# Eval("RowId")%>' />
                                                                                        </div>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                    <asp:UpdateProgress ID="progressEmail" runat="server" AssociatedUpdatePanelID="pnlEMail">
                                                                        <ProgressTemplate>
                                                                            &nbsp;<br />
                                                                            <div class="smallText">
                                                                                Email läuft... bitte warten!</div>
                                                                            <br />
                                                                            &nbsp;
                                                                        </ProgressTemplate>
                                                                    </asp:UpdateProgress>
                                                                    <asp:Label ID="lblDirectPaymentInfo" runat="server" />
                                                                    <br />
                                                                    <asp:Label ID="lblEmailInfo" runat="server" />
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="AZ" SortExpression="AktId">
                                                            <ItemTemplate>
                                                                <div class="gridData">
                                                                    <%# Eval("AktIdLink")%>
                                                                </div>
                                                                <span class="style3">[<%# Eval("AktAZ")%>]</span>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Rechnungsnummer" SortExpression="AktKunde">
                                                            <ItemTemplate>
                                                                <div class="gridData">
                                                                    <%# Eval("AktKunde")%></div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Erf. Datum" SortExpression="AktEnteredDate">
                                                            <ItemTemplate>
                                                                <div class="gridData">
                                                                    <%#  ((DateTime)Eval("AktEnteredDate")).ToShortDateString()%></div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Sachbearbeiter" SortExpression="ClientSBName">
                                                            <ItemTemplate>
                                                                <div class="gridData">&nbsp;
                                                                    <%# Eval("ClientSBName")%></div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Schuldner" SortExpression="GegnerName">
                                                            <ItemTemplate>
                                                                <div class="gridData">
                                                                    <%# Eval("GegnerInfo")%></div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Status" SortExpression="AktStatusCaption">
                                                            <ItemTemplate>
                                                                <div class="gridData">
                                                                    <%# Eval("AktStatusCaption")%><br/>
                                                                    <%# Eval ("AktSecondaryStatusCaption")%>
                                                                    </div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                                    <HeaderStyle CssClass="tblCol1" Font-Bold="True" ForeColor="#000000" />
                                                    <AlternatingRowStyle BackColor="White" />
                                                </asp:GridView>
                                                <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
                                                    SelectMethod="GetAllAkts" 
                                                    EnablePaging="true" 
                                                    TypeName="HTB.v2.intranetx.customer.AktenInk" 
                                                    StartRowIndexParameterName="startIndex"
                                                    MaximumRowsParameterName="pageSize" 
                                                    SortParameterName="sortBy" 
                                                    SelectCountMethod="GetTotalAktsCount" />
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                    <table width="100%" border="0" cellpadding="1" cellspacing="0" bgcolor="#000000">
                                        <tr>
                                            <td>
                                                <table width="100%" border="0" cellpadding="4" cellspacing="0" bgcolor="#FFFFFF">
                                                    <tr>
                                                        <td>
                                                            <span class="docText">
                                                                <asp:Label ID="lblTotalAktsMsg" runat="server" /></span>
                                                        </td>
                                                    </tr>
                                                </table>
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
