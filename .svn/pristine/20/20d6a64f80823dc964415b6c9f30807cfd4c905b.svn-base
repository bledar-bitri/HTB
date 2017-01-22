<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserAktionen.aspx.cs" Inherits="HTB.v2.intranetx.user.UserAktionen" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>HTB.ASP [ Benutzer Aktionen ]</title>
    <style type="text/css">
        body
        {
            margin-left: 5px;
            margin-top: 5px;
            margin-right: 5px;
            margin-bottom: 5px;
            background-image: url(../../intranet/images/osxback.gif);
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
        
        OPTION.dis
        {
            background-color: white;
            color: #999999;
        }
        .style1
        {
            color: #999999;
        }
        .style2
        {
            color: #CCCCCC;
        }
    </style>
    <script type="text/javascript">
        function ChangeCheckBoxState(id, checkState) {
            var cb = document.getElementById(id);
            if (cb != null)
                cb.checked = checkState;
        }

        function ChangeAllActionCheckBoxStates(checkState) {
            // Toggles through all of the checkboxes defined in the CheckBoxIDs array
            // and updates their value to the checkState input parameter
            if (ActionCheckBoxIDs != null) {
                for (var i = 0; i < ActionCheckBoxIDs.length; i++)
                    ChangeCheckBoxState(ActionCheckBoxIDs[i], checkState);
            }
        }
        function ChangeAllUserActionCheckBoxStates(checkState) {
            // Toggles through all of the checkboxes defined in the CheckBoxIDs array
            // and updates their value to the checkState input parameter
            if (UserActionCheckBoxIDs != null) {
                for (var i = 0; i < UserActionCheckBoxIDs.length; i++)
                    ChangeCheckBoxState(UserActionCheckBoxIDs[i], checkState);
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
                            <a href="../../intranet/intranet/intranet.asp">Intranet</a> | Benutzer Aktionen Editiren
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <form id="form1" runat="server">
    <table width="100%" border="0" cellspacing="0" cellpadding="1">
        <tr>
            <td bgcolor="#000000">
                <table width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
                    <tr>
                        <td>
                            <table border="0" align="center" cellpadding="3" cellspacing="0">
                                <tr>
                                    <td colspan="2" class="tblHeader" title="BENUTZER AKTIONEN EDITIEREN">
                                        BENUTZER AKTIONEN EDITIEREN
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <img name="" src="" width="1" height="1" alt="" />
                                    </td>
                                </tr>
                                <tr>
                                <td class="tblDataAll">
                                <table border="0" align="center" cellpadding="3" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="ddlUser" runat="server" AutoPostBack="true" onselectedindexchanged="ddlUser_SelectedIndexChanged"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tblDataAll" valign="top">
                                        &nbsp;
                                        <asp:GridView ID="gvUserActions" runat="server" AllowSorting="false" AutoGenerateColumns="False" CellPadding="2" CellSpacing="2" BorderStyle="Inset" Width="100%" 
                                        AlternatingRowStyle-BackColor="AntiqueWhite" OnDataBound="gvUserActions_DataBound">
                                            <RowStyle />
                                            <Columns>
                                                <%--Hidden Column--%>
                                                <asp:TemplateField HeaderText="ID" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblActionID" runat="server" Text='<%# Eval("ActionID")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="">
                                                     <HeaderTemplate>
                                                         <asp:CheckBox runat="server" ID="chkUserActionHeader" />
                                                     </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelected" runat="server" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Benutzer Aktionen" DataField="ActionCaption" SortExpression="ActionCaption" />
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                    <td>
                                    <asp:Button ID="btnAddActions" runat="server" Text="<<" onclick="btnAddActions_Click" />
                                    <br />
                                    <asp:Button ID="btnRemoveActions" runat="server" Text=">>" onclick="btnRemoveActions_Click" />
                                    </td>
                                    <td class="tblDataAll" valign="top">
                                        &nbsp;
                                        <asp:GridView ID="gvActions" runat="server" AllowSorting="false" AutoGenerateColumns="False" CellPadding="2" CellSpacing="2" 
                                        BorderStyle="Inset" Width="100%" AlternatingRowStyle-BackColor="AntiqueWhite" OnDataBound="gvActions_DataBound">
                                            <RowStyle />
                                            <Columns>
                                                <%--Hidden Column--%>
                                                <asp:TemplateField HeaderText="ID" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblActionID" runat="server" Text='<%# Eval("ActionID")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="">
                                                    <HeaderTemplate>
                                                         <asp:CheckBox runat="server" ID="chkActionHeader" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelected" runat="server" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Aktionen" DataField="ActionCaption" SortExpression="ActionCaption" />
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
                                            <asp:Button ID="btnCancel" runat="server" class="btnCancel" title="Fertig" Text="Fertig" onclick="btnCancel_Click" />
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
