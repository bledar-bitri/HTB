<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BankTransactions.aspx.cs" Inherits="HTB.v2.intranetx.bank.BankTransactions" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>HTB.ASP [ Konto Buchungen ]</title>
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
                            <a href="Bank.aspx">Bank</a> | Konto &Uuml;berweisungen | 
                            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="javascript:window.print()">Drucken</asp:HyperLink>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" EnablePartialRendering="true" EnableScriptLocalization="true" />
    <table width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
        <tr>
            <td class="tblDataAll" align="left">
                <table border="0" align="center" cellpadding="3" cellspacing="0">
                    <tr runat="server" id="trHeader">
                        <td colspan="2" class="tblHeader">
                            Bank Buchungen
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="tblDataAll">
                            <ctl:message runat="server" ID="ctlMessage" />
                        </td>
                    </tr>
                    <tr runat="server" id="trID" Visible="false">
                        <td  class="EditCaption">
                            <div align="right"><strong>Buchung #:</strong>&nbsp;</div>
                        </td>
                        <td class="EditData">
                            <strong><asp:Label runat="server" ID="lblID"></asp:Label> </strong>
                        </td>
                    </tr>
                    <tr>
                        <td  class="EditCaption">
                            <div align="right"><strong>Ist Einkommen:</strong>&nbsp;</div>
                        </td>
                        <td class="EditData">
                            <asp:CheckBox runat="server" ID="chkIsTransactionIncoming" AutoPostBack="true" OnCheckedChanged="chkIsTransactionIncoming_CheckedChanged"/>
                        </td>
                    </tr>
                    <tr>
                        <td  class="EditCaption">
                            <div align="right"><strong>Sender:</strong>&nbsp;</div>
                        </td>
                        <td class="EditData">
                            <strong><asp:label ID="lblFrom" runat="server" Text="ECP"></asp:label></strong>
                            <asp:TextBox runat="server" ID="txtFrom" class="docText" size="60" Visible="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="EditCaption">
                            <div align="right">
                                <strong>Datum</strong>:&nbsp;</div>
                        </td>
                        <td class="EditData">
                            <asp:TextBox ID="txtDate" runat="server" MaxLength="10" ToolTip="Datum"  size="10" />
                            <ajax:MaskedEditExtender ID="Datum_MaskedEditExtender" runat="server" Century="2000" TargetControlID="txtDate" Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" MaskType="Date" ErrorTooltipEnabled="True" />
                            <ajax:MaskedEditValidator ID="Datum_MaskedEditValidator" runat="server" ControlExtender="Datum_MaskedEditExtender" ControlToValidate="txtDate" InvalidValueMessage="Datum is ung&uuml;ltig!" Display="Dynamic" InvalidValueBlurredMessage="*" />
                            <ajax:CalendarExtender ID="Datum_CalendarExtender" runat="server" TargetControlID="txtDate" PopupButtonID="Datum_CalendarButton" />
                            <asp:ImageButton runat="Server" ID="Datum_CalendarButton" ImageUrl="~/v2/intranet/images/CalendarHS.png" AlternateText="Click to show calendar" />
                        </td>
                    </tr>
                    <tr>
                        <td class="EditCaption">
                            <div align="right"><strong>Empf&auml;nger:</strong>&nbsp;</div>
                        </td>
                        <td class="EditData">
                            <asp:TextBox runat="server" ID="txtTo" class="docText" size="60" />
                            <strong><asp:Label runat="server" ID="lblTo" class="docText" Visible="false" Text="ECP"/></strong>
                        </td>
                    </tr>
                    <tr>
                        <td class="EditCaption">
                            <div align="right"><strong>Beschreibung:&nbsp;</strong></div>
                        </td>
                        <td class="EditData">
                            <asp:TextBox runat="server" ID="txtDescription" class="docText" size="60" />
                        </td>
                    </tr>
                    <tr runat="server" id="tr4">
                        <td class="EditCaption">
                            <div align="right"><strong>Betrag:</strong>&nbsp;</div>
                        </td>
                        <td class="EditData">
                            <asp:TextBox runat="server" ID="txtAmount" class="docText" size="10" />
                        </td>
                    </tr>
                    <tr runat="server" id="trButtons">
                        <td colspan="2">
                            <br />
                            <asp:Button runat="server" ID="btnSubmit" Text="Speichern" class="btnSave" OnClick="btnSubmit_Click" />
                            <asp:Button runat="server" ID="btnNew" Text="Neue Buchung" class="btnAction" OnClick="btnNew_Click" />
                            <br />
                            &nbsp;<br />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="tblDataAll" valign="top" align="left">
                <table border="0" align="center" cellpadding="3" cellspacing="0">
                    <tr>
                        <td>
                            <asp:GridView ID="gvTransactions" runat="server" AutoGenerateColumns="False" CellPadding="2" BorderStyle="Groove" Width="100%" CssClass="tblDataAll">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <%# Eval("DeleteUrl")%>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <%# Eval("EditUrl")%>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Datum" DataField="Date" />
                                    <asp:BoundField HeaderText="Sender" DataField="From" />
                                    <asp:BoundField HeaderText="Emfenger" DataField="To" />
                                    <asp:BoundField HeaderText="Beschreibung" DataField="Description" />
                                    <asp:BoundField HeaderText="Betrag" DataField="Amount" ItemStyle-HorizontalAlign="Right"/>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <asp:HiddenField runat="server" ID="hdnNumberOfPlzRanges"/>
    </form>
</body>
</html>
