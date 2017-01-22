<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HonorarSchema.aspx.cs" Inherits="HTB.v2.intranetx.aktenintprovfix.HonorarSchema" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
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
        function UpdateHonorarGroup() {
            var List = document.getElementById("ddlHonorarGroup");
            window.open("EditHonorarGroup.aspx?ID=" + List.options[List.selectedIndex].value + "&IsPopUp=Y", "_blank", "toolbar=no,menubar=no");
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
                            <a href="../../intranet/intranet/intranet.asp">Intranet</a> | <a href="../../intranet/intranet/mydata.asp">Meine Daten</a> | Erfolgsprovisionschema
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <br/>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" EnablePartialRendering="true" EnableScriptLocalization="true" />
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
                                    <td colspan="2" class="tblHeader" title="ERFOLGSPROVISIONSCHEMA EDITIEREN">
                                        ERFOLGSPROVISIONSCHEMA AKTIONEN EDITIEREN
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
                                    <td colspan="3" valign="middle">
                                        <asp:DropDownList ID="ddlHonorarGroup" runat="server" AutoPostBack="true" onselectedindexchanged="ddlHonorarGroup_SelectedIndexChanged"/>
                                        &nbsp;&nbsp;
                                        <a href="javascript:UpdateHonorarGroup();">Erfolgsprovisionsgroupenbeschreibung Editired</a>
                                        &nbsp;&nbsp;
                                        <a href="javascript:void(window.open('EditHonorarGroup.aspx?IsPopUp=Y','_blank','toolbar=no,menubar=no'))">New Erfolgsprovisionsgroupe</a>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tblDataAll" valign="top">
                                        &nbsp;
                                        <asp:GridView ID="gvHonorarGroupItems" runat="server" AllowSorting="false" AutoGenerateColumns="False" CellPadding="2" CellSpacing="2" BorderStyle="Inset" Width="100%" AlternatingRowStyle-BackColor="AntiqueWhite" >
                                            <RowStyle />
                                            <Columns>
                                                <%--Hidden Column--%>
                                                <asp:TemplateField HeaderText="ID" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblHonorarID" runat="server" Text='<%# Eval("HonorarID")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelected" runat="server" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Vom" DataField="From" SortExpression="From"  ItemStyle-HorizontalAlign="Right"/>
                                                <asp:BoundField HeaderText="Bis" DataField="To" SortExpression="To"  ItemStyle-HorizontalAlign="Right"/>
                                                <asp:BoundField HeaderText="Preis" DataField="Price" SortExpression="Price"  ItemStyle-HorizontalAlign="Right"/>
                                                <asp:BoundField HeaderText="Procent" DataField="Pct" SortExpression="Pct"  ItemStyle-HorizontalAlign="Right"/>
                                                <asp:BoundField HeaderText="Procent<br/>vom" DataField="PctOf" SortExpression="PctOf" ItemStyle-HorizontalAlign="Right" HtmlEncode="false"/>
                                                <asp:BoundField HeaderText="Maximum<br/>Preis" DataField="MaxPrice" SortExpression="Price" ItemStyle-HorizontalAlign="Right" HtmlEncode="false"/>
                                                <asp:BoundField HeaderText="Provision" DataField="ProvAmount" SortExpression="ProvAmount" ItemStyle-HorizontalAlign="Right"/>
                                                <asp:BoundField HeaderText="Provision<br/>Procent" DataField="ProvPct" SortExpression="ProvPct" ItemStyle-HorizontalAlign="Right" HtmlEncode="false"/>
                                                <asp:BoundField HeaderText="Provision<br/>Procent vom" DataField="ProvPctOf" SortExpression="ProvPctOf" ItemStyle-HorizontalAlign="Right" HtmlEncode="false"/>
                                                <asp:BoundField HeaderText="Maximum<br/>Provision" DataField="MaxProvAmount" SortExpression="MaxProvAmount" ItemStyle-HorizontalAlign="Right" HtmlEncode="false"/>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                    <td>
                                    <asp:Button ID="btnAdd" runat="server" Text="<<" onclick="btnAdd_Click" />
                                    <br />
                                    <asp:Button ID="btnRemove" runat="server" Text=">>" onclick="btnRemove_Click" />
                                    </td>
                                    <td class="tblDataAll" valign="top">
                                            <a href="javascript:void(window.open('EditHonorar.aspx?IsPopUp=Y','_blank','toolbar=no,menubar=no'))">
                                                Neu Erfolgsprovisionsdatasatz
                                            </a>
                                        <br />&nbsp;
                                        <asp:GridView ID="gvHonorarItems" runat="server" AllowSorting="false" AutoGenerateColumns="False" CellPadding="2" CellSpacing="2" BorderStyle="Inset" Width="100%" AlternatingRowStyle-BackColor="AntiqueWhite">
                                            <RowStyle />
                                            <Columns>
                                                <%--Hidden Column--%>
                                                <asp:TemplateField HeaderText="ID" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblHonorarID" runat="server" Text='<%# Eval("HonorarID")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelected" runat="server" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <a href="javascript:void(window.open('<%# Eval("EditPopupUrl")%>','_blank','toolbar=no,menubar=no'))">
                                                            <asp:Image ID="img" runat="server" ImageUrl='<%# Eval("EditUrl")  %>' BorderColor="White" />
                                                        </a>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Vom" DataField="From" SortExpression="From"  ItemStyle-HorizontalAlign="Right"/>
                                                <asp:BoundField HeaderText="Bis" DataField="To" SortExpression="To"  ItemStyle-HorizontalAlign="Right"/>
                                                <asp:BoundField HeaderText="Preis" DataField="Price" SortExpression="Price"  ItemStyle-HorizontalAlign="Right"/>
                                                <asp:BoundField HeaderText="Procent" DataField="Pct" SortExpression="Pct"  ItemStyle-HorizontalAlign="Right"/>
                                                <asp:BoundField HeaderText="Procent<br/>vom" DataField="PctOf" SortExpression="PctOf" ItemStyle-HorizontalAlign="Right" HtmlEncode="false"/>
                                                <asp:BoundField HeaderText="Maximum<br/>Preis" DataField="MaxPrice" SortExpression="Price" ItemStyle-HorizontalAlign="Right" HtmlEncode="false"/>
                                                <asp:BoundField HeaderText="Provision" DataField="ProvAmount" SortExpression="ProvAmount" ItemStyle-HorizontalAlign="Right"/>
                                                <asp:BoundField HeaderText="Provision<br/>Procent" DataField="ProvPct" SortExpression="ProvPct" ItemStyle-HorizontalAlign="Right" HtmlEncode="false"/>
                                                <asp:BoundField HeaderText="Provision<br/>Procent<br/>vom" DataField="ProvPctOf" SortExpression="ProvPctOf" ItemStyle-HorizontalAlign="Right" HtmlEncode="false"/>
                                                <asp:BoundField HeaderText="Maximum<br/>Provision" DataField="MaxProvAmount" SortExpression="MaxProvAmount" ItemStyle-HorizontalAlign="Right" HtmlEncode="false"/>
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
                                            <asp:Button ID="btnSubmit" runat="server" class="btnCancel" title="Fertig" Text="Fertig" onclick="btnCancel_Click" />
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
