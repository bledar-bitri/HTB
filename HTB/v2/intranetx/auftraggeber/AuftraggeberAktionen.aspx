<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AuftraggeberAktionen.aspx.cs" Inherits="HTB.v2.intranetx.auftraggeber.AuftraggeberAktionen" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>HTB.ASP [ Auftraggeber Aktionen ]</title>
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
                            <a href="../../intranet/intranet/intranet.asp">Intranet</a> | Auftraggeber Aktionen Editiren
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
                                    <td colspan="2" class="tblHeader" title="AUFTRAGGEBER AKTIONEN EDITIEREN">
                                        AUFTRAGGEBER AKTIONEN EDITIEREN
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <ctl:message ID="ctlMessage" runat="server"/>
                                    </td>
                                </tr>
                                <tr>
                                <td class="tblDataAll">
                                <table border="0" align="center" cellpadding="3" cellspacing="0">
                                <tr>
                                    <td>
                                        <span class="smallText"><asp:Label ID="lblAuftraggeber" runat="server" Text="Auftraggeber:" />&nbsp;</span><span class="docText">
                                        <asp:DropDownList ID="ddlAuftraggeber" runat="server" AutoPostBack="true" onselectedindexchanged="ddlAuftraggeber_SelectedIndexChanged"/>
                                        </span><span class="smallText">&nbsp;&nbsp;&nbsp;<asp:Label ID="lblAktType" runat="server" Text="Akt Typ:" /></span>
                                        <span class="docText"><asp:DropDownList ID="ddlAktType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAktType_SelectedIndexChanged" /></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tblDataAll" valign="top">
                                        &nbsp;
                                        <asp:GridView ID="gvAGActions" runat="server" AllowSorting="false" AutoGenerateColumns="False" CellPadding="2" CellSpacing="2" BorderStyle="Inset" Width="100%" AlternatingRowStyle-BackColor="AntiqueWhite" >
                                            <RowStyle />
                                            <Columns>
                                                <%--Hidden Column--%>
                                                <asp:TemplateField HeaderText="ID" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblActionID" runat="server" Text='<%# Eval("ActionID")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelected" runat="server" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Auftraggeber Aktionen" DataField="ActionCaption" SortExpression="ActionCaption" />
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
                                        <asp:GridView ID="gvActions" runat="server" AllowSorting="false" AutoGenerateColumns="False" CellPadding="2" CellSpacing="2" BorderStyle="Inset" Width="100%" AlternatingRowStyle-BackColor="AntiqueWhite">
                                            <RowStyle />
                                            <Columns>
                                                <%--Hidden Column--%>
                                                <asp:TemplateField HeaderText="ID" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblActionID" runat="server" Text='<%# Eval("ActionID")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelected" runat="server" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Andare Aktionen" DataField="ActionCaption" SortExpression="ActionCaption" />
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                    <td class="tblDataAll" valign="top">
                                        &nbsp;
                                        <asp:GridView ID="gvDefaultActions" runat="server" AllowSorting="false" AutoGenerateColumns="False" CellPadding="2" CellSpacing="2" BorderStyle="Inset" Width="100%" AlternatingRowStyle-BackColor="AntiqueWhite">
                                            <RowStyle />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Standard Aktionen<BR/>Wenn keine Aktion gipt werden die Standardaktionen benützt">
                                                <ItemTemplate>
                                                    </nobr><asp:Label ID="lblCaption" runat="server" Width="80px" Text='<%#Eval("ActionCaption") %>'/>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" Wrap="false" />
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
