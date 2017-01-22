<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AuftraggeberExtension.aspx.cs" Inherits="HTB.v2.intranetx.auftraggeber.AuftraggeberExtension" %>

<%@ Register TagPrefix="hdr" TagName="header" Src="~/v2/intranetx/global_files/intranet_header_no_menu_no_session_validation.ascx" %>
<%@ Register TagPrefix="ftr" TagName="footer" Src="~/v2/intranetx/global_files/intranet_footer.ascx" %>
<%@ Register TagPrefix="ctl" TagName="message" Src="~/v2/intranetx/global_files/CtlMessage.ascx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title><asp:Literal ID="litTitle" runat="server" /> </title>
    <link href="../../intranet/styles/inksys.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        body {
	        margin-left: 5px;
	        margin-top: 5px;
	        margin-right: 5px;
	        margin-bottom: 5px;
	        background-image: url(../../intranet/images/osxback.gif);
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
        .style2 {	color: #FF0000;
	        font-weight: bold;
        }
    </style>
</head>
<body>
    <hdr:header ID="hdr" runat="server" />
    <form id="form1" runat="server">
        <table width="100%" border="0" cellspacing="0" cellpadding="1">
        <tr>
            <td bgcolor="#000000">
                <table width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
                    <tr>
                        <td>
                            <table border="0" align="center" cellpadding="3" cellspacing="0">
                                <tr>
                                    <td colspan="2" class="tblHeader">
                                        <asp:Label ID="lblHeader" runat="server"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                       <ctl:message ID="ctlMessage" runat="server"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" class="tblHeader" title="Adressdaten">
                                        Verl&auml;ngerungsanfrage
                                    </td>
                                </tr>
                                <tr>
                                    <td class="EditCaption">
                                        <div align="right">Auftraggeber:</div>
                                    </td>
                                    <td class="EditData">
                                        <asp:Label runat="server" ID="lblAuftraggeber" class="docText" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="EditCaption">
                                        <div align="right">Sachbearbeiter(in):</div>
                                    </td>
                                    <td class="EditData">
                                        <asp:Label runat="server" ID="lblAGSB" class="docText" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="EditData" colspan="2">&nbsp;
                                       <asp:GridView ID="gvExtension" runat="server" AllowSorting="false" AutoGenerateColumns="False"
                                        CellPadding="2" CellSpacing="2"  Width="100%" AlternatingRowStyle-BackColor="AntiqueWhite" EmptyDataText="Es gipt keine neue Anfragen">
                                        <RowStyle />
                                        <Columns>
                                            <%--Hidden Column--%>
                                            <asp:TemplateField HeaderText="ID" Visible="false"> 
                                                <ItemTemplate>
                                                    <asp:Label id="lblExtId" runat="server" Text='<%# Eval("ExtId")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="Akt" DataField="AktAZ" SortExpression="AktAZ" ItemStyle-HorizontalAlign="Center"/>
                                            <asp:BoundField HeaderText="Schuldner" DataField="Gegner" SortExpression="Gegner" HtmlEncode="false" />
                                            <asp:BoundField HeaderText="Betrag" DataField="TotalDue" SortExpression="TotalDue" />
                                            <asp:BoundField HeaderText="Grund" DataField="Reason" SortExpression="Reason" />
                                            <asp:BoundField HeaderText="Termin" DataField="CrrDueDate" SortExpression="CrrDueDate"  ItemStyle-HorizontalAlign="Center"/>
                                            <asp:BoundField HeaderText="Verl&auml;ngerungstage" DataField="ExtenssionDays" SortExpression="ExtenssionDays" HtmlEncode="false"  ItemStyle-HorizontalAlign="Right"/>
                                            <asp:BoundField HeaderText="Termin neu" DataField="NextDueDate" SortExpression="NextDueDate"  ItemStyle-HorizontalAlign="Center"/>
                                            <asp:TemplateField HeaderText="genehmigt">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkApprove" runat="server" Checked='<%#Eval("Aprove") %>' onFocus="this.style.backgroundColor='#DFF4FF'" onBlur="this.style.backgroundColor=''" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="nicht genehmigt">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkDeny" runat="server" Checked='<%#Eval("Deny") %>' onFocus="this.style.backgroundColor='#DFF4FF'" onBlur="this.style.backgroundColor=''" />&nbsp;&nbsp;&nbsp;
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Memo">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtMemo" runat="server" Width="300px" Text='<%#Eval("Memo") %>' onFocus="this.style.backgroundColor='#DFF4FF'" onBlur="this.style.backgroundColor=''" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" class="tblFooter1">
                                        <div align="right">
                                             <asp:Button ID="btnSubmit" runat="server" class="btnSave" Text="Speichern" title="Speichern" OnClick="btnSubmit_Click" />
                                             <asp:Button ID="btnCancel2" runat="server" class="btnCancel" title="Abbrechen" Text="Abbrechen" OnClick="btnCancel_Click" />
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
    </form>
    <ctl:footer ID="ftr" runat="server" />
</body>
</html>
