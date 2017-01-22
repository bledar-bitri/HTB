<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UEList.aspx.cs" Inherits="HTB.v2.intranetx.aktenint.UEList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>HTB.ASP [ Überweisungslisten ]</title>
    <link href="../../intranet/styles/htbTablet.css" rel="stylesheet" type="text/css"/>
    <style type="text/css">
        body {
	        margin-left: 5px;
	        margin-top: 5px;
	        margin-right: 5px;
	        margin-bottom: 5px;
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
        OPTION.dis
        {
            background-color: white;
            color: #999999;
        }
    </style>
</head>
<body>
    <ctl:headerNoMenu ID="header" runat="server" />
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" EnablePartialRendering="true" EnableScriptLocalization="true" />
            <script type="text/javascript">
                Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
                function BeginRequestHandler(sender, args) {
                    document.getElementById('<%= btnSubmit.ClientID %>').innerText = "Processing";
                    document.getElementById('<%= btnSubmit.ClientID %>').disabled = true;
                    document.getElementById('<%= btnGo.ClientID %>').innerText = "Processing";
                    document.getElementById('<%= btnGo.ClientID %>').disabled = true;
                }
        </script>
    <asp:UpdatePanel ID="updPanel1" runat="server">
        <ContentTemplate>
            <table width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
                <tr>
                    <td>
                        <table border="0" align="center" cellpadding="3" cellspacing="0">
                            <tr>
                                <td class="tblHeader">
                                    &Uuml;BERWEISUNGSLISTEN
                                </td>
                            </tr>
                            <tr>
                                <td class="tblData1">
                                    <ctl:message ID="ctlMessage" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tblData1">
                                    <table width="100%"><tr>
                                    <td>
                                    <asp:DropDownList runat="server" ID="ddlUsers" />
                                    </td>
                                    </tr>
                                    <tr><td>&nbsp;</td></tr>
                                    <tr><td>
                                    <asp:CheckBox ID="chkShowPassedTransfers" runat="server" Text="Berreits &uuml;berwiesen"/>
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                    vom: 
                                    <asp:TextBox runat="server" ID="txtDateFrom" MaxLength="10" ToolTip="Date of contact" ValidationGroup="DateFromVG" onFocus="this.style.backgroundColor='#DFF4FF'" onBlur="this.style.backgroundColor=''" size="10" />
                                    <ajax:MaskedEditExtender ID="DateFrom_MaskedEditExtender" runat="server" Century="2000" TargetControlID="txtDateFrom" Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left" ErrorTooltipEnabled="True" />
                                    <ajax:MaskedEditValidator ID="DateFrom_MaskedEditValidator" runat="server" ControlExtender="DateFrom_MaskedEditExtender" ControlToValidate="txtDateFrom" EmptyValueMessage="Date of contact is required" InvalidValueMessage="DateFrom is ung&uuml;ltig!" Display="Dynamic" EmptyValueBlurredText="*" InvalidValueBlurredMessage="*" ValidationGroup="DateFromVG" />
                                    <ajax:CalendarExtender ID="DateFrom_CalendarExtender" runat="server" TargetControlID="txtDateFrom" PopupButtonID="DateFrom_CalendarButton" />
                                    <asp:ImageButton runat="Server" ID="DateFrom_CalendarButton" ImageUrl="~/v2/intranet/images/CalendarHS.png" AlternateText="Click to show calendar" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;bis:&nbsp;&nbsp;
                                    <asp:TextBox runat="server" ID="txtDateTo" MaxLength="10" ToolTip="Date of contact" ValidationGroup="DateToVG" onFocus="this.style.backgroundColor='#DFF4FF'" onBlur="this.style.backgroundColor=''" size="10" />
                                    <ajax:MaskedEditExtender ID="DateTo_MaskedEditExtender" runat="server" Century="2000" TargetControlID="txtDateTo" Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left" ErrorTooltipEnabled="True" />
                                    <ajax:MaskedEditValidator ID="DateTo_MaskedEditValidator" runat="server" ControlExtender="DateTo_MaskedEditExtender" ControlToValidate="txtDateTo" EmptyValueMessage="Date of contact is required" InvalidValueMessage="DateTo is ung&uuml;ltig!" Display="Dynamic" EmptyValueBlurredText="*" InvalidValueBlurredMessage="*" ValidationGroup="DateToVG" />
                                    <ajax:CalendarExtender ID="DateTo_CalendarExtender" runat="server" TargetControlID="txtDateTo" PopupButtonID="DateTo_CalendarButton" />
                                    <asp:ImageButton runat="Server" ID="DateTo_CalendarButton" ImageUrl="~/v2/intranet/images/CalendarHS.png" AlternateText="Click to show calendar" />
                                    </td>
                                    </tr>
                                    <tr><td>&nbsp;</td></tr>
                                    <tr>
                                    <td>
                                    <asp:CheckBox ID="chkShowCollectionFees" runat="server" Text="Kosten zeigen"/>
                                    </td></tr>
                                    <td align="right">
                                    <asp:Button ID="btnGo" runat="server" class="btnSave" Text="   GO   " OnClick="btnGo_Click"/>
                                    </td>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tblDataAll">
                                    <asp:GridView ID="gvTransfers" runat="server" AllowSorting="false" AutoGenerateColumns="False"
                                        CellPadding="2" CellSpacing="2" BorderStyle="Inset" Width="100%" AlternatingRowStyle-BackColor="AntiqueWhite" ShowFooter="true"
                                        EmptyDataText="Keine &Uuml;berweisungen vorhanden"
                                        >
                                        <RowStyle />
                                        <Columns>
                                            <%--Hidden Column--%>
                                            <asp:TemplateField HeaderText="ID" Visible="false" > 
                                                <ItemTemplate>
                                                    <asp:Label id="lblPosID" runat="server" Text='<%# Eval("PosId")%>' /><br/>
                                                    <asp:Label id="lblAktID" runat="server" Text='<%# Eval("AktId")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:BoundField HeaderText="Bemerkung" DataField="PosCaption" SortExpression="PosCaption" ItemStyle-HorizontalAlign="Left"  HtmlEncode="false"/>
                                            <asp:BoundField HeaderText="DateFrom (Kassiert)" DataField="PosDate" SortExpression="PosDate" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField HeaderText="Schuldner" DataField="GegnerName" SortExpression="GegnerName" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="Aussendienst" DataField="UserName" SortExpression="UserName" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="Memo" DataField="Memo" SortExpression="Memo" ItemStyle-HorizontalAlign="Left" HtmlEncode="false" />
                                            
                                            <asp:BoundField HeaderText="Auftraggeber<BR/>Emf&auml;nger" DataField="AuftraggeberName" SortExpression="AuftraggeberName" HtmlEncode="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-BackColor="LightGreen" />
                                            <asp:BoundField HeaderText="Kontonummer<BR/>Emf&auml;nger" DataField="TransferToBankAccount" SortExpression="TransferToBankAccount" ItemStyle-HorizontalAlign="Center" HtmlEncode="false" HeaderStyle-BackColor="LightGreen" />
                                            <asp:BoundField HeaderText="Aktenzeichen<BR/>Verwendungszweck" DataField="AktAZ" SortExpression="AktAZ" ItemStyle-HorizontalAlign="Left" HtmlEncode="false" HeaderStyle-BackColor="LightGreen"/>
                                            
                                            <asp:TemplateField HeaderText="Betrag  <br/> (zu &Uuml;berweisen)" HeaderStyle-BackColor="LightGreen">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTransferAmmount" runat="server" Width="80px" Text='<%#Eval("PosAmount") %>' style="text-align: right"/>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                                <FooterTemplate>
                                                    <%= GetTotalToTransferAmountString()%>
                                                </FooterTemplate>
                                                <FooterStyle HorizontalAlign="Center" Font-Bold="true"/>
                                            </asp:TemplateField>
                                            
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td class="EditData" align="right">
                                    <asp:Button ID="btnSubmit" runat="server" class="btnSave" Text="&Uuml;berwiesen" OnClick="btnSubmit_Click" />
                                </td>
                            </tr>
                            <tr id="trTransferred" runat="server">
                                <td class="tblDataAll">
                                    <strong>Berreits &uuml;berwiesen:</strong>
                                    <br/>
                                    <asp:GridView ID="gvTransferred" runat="server" AllowSorting="false" AutoGenerateColumns="False"
                                        CellPadding="2" CellSpacing="2" BorderStyle="Inset" Width="100%" 
                                        AlternatingRowStyle-BackColor="PaleTurquoise" ShowFooter="true"
                                        >
                                        <RowStyle />
                                        <Columns>
                                            <%--Hidden Column--%>
                                            <asp:TemplateField HeaderText="ID" Visible="false" > 
                                                <ItemTemplate>
                                                    <asp:Label id="lblPosID" runat="server" Text='<%# Eval("PosId")%>' /><br/>
                                                    <asp:Label id="lblAktID" runat="server" Text='<%# Eval("AktId")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:BoundField HeaderText="Bemerkung" DataField="PosCaption" SortExpression="PosCaption" ItemStyle-HorizontalAlign="Left"  HtmlEncode="false"/>
                                            <asp:BoundField HeaderText="DateFrom (Kassiert)" DataField="PosDate" SortExpression="PosDate" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField HeaderText="Schuldner" DataField="GegnerName" SortExpression="GegnerName" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="Aussendienst" DataField="UserName" SortExpression="UserName" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="Memo" DataField="Memo" SortExpression="Memo" ItemStyle-HorizontalAlign="Left" HtmlEncode="false" />
                                            
                                            <asp:BoundField HeaderText="Auftraggeber<BR/>Emf&auml;nger" DataField="AuftraggeberName" SortExpression="AuftraggeberName" HtmlEncode="false" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="Kontonummer<BR/>Emf&auml;nger" DataField="TransferToBankAccount" SortExpression="TransferToBankAccount" ItemStyle-HorizontalAlign="Center" HtmlEncode="false"/>
                                            <asp:BoundField HeaderText="Aktenzeichen<BR/>Verwendungszweck" DataField="AktAZ" SortExpression="AktAZ" ItemStyle-HorizontalAlign="Left" HtmlEncode="false" />
                                            
                                            <asp:TemplateField HeaderText="Betrag  <br/> (zu &Uuml;berweisen)" >
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTransferAmmount" runat="server" Width="80px" Text='<%#Eval("PosAmount") %>' style="text-align: right"/>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                                <FooterTemplate>
                                                    <%= GetTotalTransferredAmountString()%>
                                                </FooterTemplate>
                                                <FooterStyle HorizontalAlign="Center" Font-Bold="true"/>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="&Uuml;berwiesen am" DataField="TransferredDate" SortExpression="TransferredDate" ItemStyle-HorizontalAlign="Center" HtmlEncode="false" />
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                           
                            <tr id="trCollection" runat="server">
                                <td class="tblDataAll">
                                    <table width="100%"><tr><td>
                                    <strong>Kosten:</strong>
                                    <br/>
                                    <asp:GridView ID="gvCollection" runat="server" AllowSorting="false" AutoGenerateColumns="False"
                                        CellPadding="2" CellSpacing="2" BorderStyle="Inset" Width="100%" 
                                        AlternatingRowStyle-BackColor="AntiqueWhite" ShowFooter="true"
                                        EmptyDataText="Keine Kosten vorhanden"
                                        >
                                        <RowStyle />
                                        <Columns>
                                            <%--Hidden Column--%>
                                            <asp:TemplateField HeaderText="ID" Visible="false" > 
                                                <ItemTemplate>
                                                    <asp:Label id="lblPosID" runat="server" Text='<%# Eval("PosId")%>' /><br/>
                                                    <asp:Label id="lblAktID" runat="server" Text='<%# Eval("AktId")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:BoundField HeaderText="Bemerkung" DataField="PosCaption" SortExpression="PosCaption" ItemStyle-HorizontalAlign="Left"  HtmlEncode="false"/>
                                            <asp:BoundField HeaderText="DateFrom (Kassiert)" DataField="PosDate" SortExpression="PosDate" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField HeaderText="Schuldner" DataField="GegnerName" SortExpression="GegnerName" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="Aussendienst" DataField="UserName" SortExpression="UserName" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="Memo" DataField="Memo" SortExpression="Memo" ItemStyle-HorizontalAlign="Left" HtmlEncode="false" />
                                            
                                            <asp:BoundField HeaderText="Auftraggeber<BR/>Emf&auml;nger" DataField="AuftraggeberName" SortExpression="AuftraggeberName" HtmlEncode="false" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="Kontonummer<BR/>Emf&auml;nger" DataField="TransferToBankAccount" SortExpression="TransferToBankAccount" ItemStyle-HorizontalAlign="Center" HtmlEncode="false"/>
                                            <asp:BoundField HeaderText="Aktenzeichen<BR/>Verwendungszweck" DataField="AktAZ" SortExpression="AktAZ" ItemStyle-HorizontalAlign="Left" HtmlEncode="false" />
                                            
                                            <asp:TemplateField HeaderText="Betrag  <br/> (zu &Uuml;berweisen)" >
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTransferAmmount" runat="server" Width="80px" Text='<%#Eval("PosAmount") %>' style="text-align: right"/>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                                <FooterTemplate>
                                                    <%= GetTotalCollectionAmountString()%>
                                                </FooterTemplate>
                                                <FooterStyle HorizontalAlign="Center" Font-Bold="true"/>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Empfangen">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkIsReceived" runat="server"/>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    </td></tr>
                                    <tr><td align="right"><asp:Button ID="btnCollectionReceived" runat="server" class="btnSave" Text="Kosten Empfangen" OnClick="btnCollectionReceived_Click" /></td></tr>
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
