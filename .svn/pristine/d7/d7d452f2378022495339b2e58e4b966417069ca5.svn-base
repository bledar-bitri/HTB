<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ActionProv.aspx.cs" Inherits="HTB.v2.intranetx.aktenintprovfix.ActionProv" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>HTB.ASP [ Aktion Fixe Provisionss&auml;tze ]</title>
    <link href="../../intranet/styles/inksys.css" rel="stylesheet" type="text/css" />
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
                            <a href="../../intranet/intranet/intranet.asp">Intranet</a> | <a href="../../intranet/intranet/mydata.asp">Meine Daten</a> | Fixe Provisionss&auml;tze
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <br/>
    <form id="form1" runat="server">
    <table width="100%" border="0" cellspacing="0" cellpadding="1">
        <tr>
            <td bgcolor="#000000">
                <table width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
                    <tr>
                        <td>
                            <table width="100%" border="0" cellpadding="1" cellspacing="3">
                                <tr>
                                    <td background="../../intranet/images/hdbackblue.gif" class="tblFunctionBar">
                                        <table width="100%" border="0" cellpadding="3" cellspacing="0" background="../../intranet/images/hdbackblue.gif" bgcolor="#FFFFFF">
                                            <tr>
                                                <td width="10%">
                                                    <span class="docText"><strong class="smallText">Filter:</strong></span>
                                                </td>
                                                <td width="71%">
                                                    <span class="smallText"><asp:Label ID="lblAuftraggeber" runat="server" Text="Auftraggeber:" />&nbsp;</span><span class="docText">
                                                        <asp:DropDownList ID="ddlAuftraggeber" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAuftraggeber_SelectedIndexChanged" />
                                                    </span><span class="smallText">&nbsp;&nbsp;&nbsp;<asp:Label ID="lblAktType" runat="server" Text="Akt Typ:" /></span><span class="docText">
                                                        <asp:DropDownList ID="ddlAktType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAktType_SelectedIndexChanged" />
                                                    </span>
                                                    <span class="smallText">&nbsp;&nbsp;&nbsp;<asp:Label ID="lblUser" runat="server" Text="Benutzer:" /></span><span class="docText">
                                                        <asp:DropDownList ID="ddlUser" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlUser_SelectedIndexChanged" />
                                                    </span>
                                                    &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblMessage" runat="server" />
                                                </td>
                                                <td width="1%">
                                                    &nbsp;
                                                </td>
                                                <td width="10%">
                                                    <div align="right">
                                                        <input name="Submit5" type="button" class="btnStandard" title="Zur&uuml;ck zum Men&uuml; Meine Daten." onclick="MM_goToURL('parent','../intranet/mydata.asp');return document.MM_returnValue"
                                                            value="Zur&uuml;ck" />
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <table width="100%" border="0" cellpadding="1" cellspacing="3">
                                <tr>
                                    <td background="../../intranet/images/hdbackyellow.gif" class="tblFunctionBar">
                                        <table width="100%" border="0" cellpadding="3" cellspacing="0" background="../../intranet/images/hdbackyellow.gif" bgcolor="#FFFFFF">
                                            <tr>
                                                <td width="10%">
                                                    <span class="docText"><strong class="smallText">Aktionen:</strong></span>
                                                </td>
                                                <td width="71%">
                                                    <asp:Button ID="btnShowActions" runat="server" class="btnStandard" title="Hier k&ouml;nnen Sie einen neuen Provisionssatz hinfügen." Text="Provisionssatz Hinfügen" onclick="btnShowActions_Click" />
                                                    &nbsp;
                                                   <%-- <input name="Submit7" type="button" class="smallText" value="Drucken" onclick="window.open('popPrintPDF.asp?hidTableName=&amp;Titel=Fixe%20Provisonen&amp;ColumnNr=4');"
                                                        title="Erstellt eine Provisionsliste im PDF Format mit den aktuellen Filter- & Sortiereinstellungen." />
                                                    <input name="btnExportTo" type="button" class="smallText" id="btnExportTo" value="Exportieren nach" onclick="window.open('../global_forms/getDBfields.asp?hidTableName=&amp;lstExportTo=');"
                                                        title="Provisionsliste exportieren." />
                                                    <asp:DropDownList ID="ddlExportTo" runat="server" />
                                                    &nbsp;--%>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <table width="100%" border="0" cellpadding="3" cellspacing="0">
                                <tr>
                                    <td class="tblHeader" title="FIXE PROVISIONSS&Auml;TZE ">
                                        FIXE PROVISIONSS&Auml;TZE
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5" class="EditData">
                                        &nbsp;
                                        <asp:Panel ID="panActionProv" runat="server">
                                            <asp:GridView ID="gvActionProv" runat="server" AllowSorting="false" AutoGenerateColumns="False" CellPadding="2" CellSpacing="2" BorderStyle="Inset" Width="100%"
                                                AlternatingRowStyle-BackColor="AntiqueWhite" OnDataBound="gvActionProv_DataBound">
                                                <RowStyle />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="ID">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblActionID" runat="server" Text='<%# Eval("ActionID")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="Tätigkeit" DataField="ActionCaption" SortExpression="ActionCaption" ItemStyle-HorizontalAlign="Left" />
                                                    <asp:TemplateField HeaderText="Preis (Eingang)">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtPrice" runat="server" Text='<%#Eval("Price") %>' onFocus="this.style.backgroundColor='#DFF4FF'" onBlur="this.style.backgroundColor=''"
                                                                Style="text-align: right" />&euro;
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Provision Betrag">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtProvAmount" runat="server" Text='<%#Eval("ProvAmount") %>' onFocus="this.style.backgroundColor='#DFF4FF'" onBlur="this.style.backgroundColor=''"
                                                                Style="text-align: right" />&euro;
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Provision Betrag<br/>Ohne Kassierung">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtProvAmountForZero" runat="server" Width="80px" Text='<%#Eval("ProvAmountForZero") %>' onFocus="this.style.backgroundColor='#DFF4FF'" onBlur="this.style.backgroundColor=''"
                                                                Style="text-align: right" />&euro;
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Provision Procent<br/>Vom Kassierung">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtProvPct" runat="server" Width="80px" Text='<%#Eval("ProvPct") %>' onFocus="this.style.backgroundColor='#DFF4FF'" onBlur="this.style.backgroundColor=''"
                                                                Style="text-align: right" />&#37;
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Erfolgsprovisionsgroupe">
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="ddlHonorarGroup" runat="server" onFocus="this.style.backgroundColor='#DFF4FF'" onBlur="this.style.backgroundColor=''"
                                                                Style="text-align: right" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                     <%--Hidden Column--%>
                                                    <asp:TemplateField HeaderText="HonorarGrpID" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblHonorarGrpId" runat="server" Text='<%# Eval("HonorarGrpId")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </asp:Panel>
                                        <asp:Panel ID="panActions" runat="server" Visible="false">
                                            <asp:GridView ID="gvActions" runat="server" AllowSorting="false" AutoGenerateColumns="False" CellPadding="2" CellSpacing="2" BorderStyle="Inset"
                                             Width="100%" AlternatingRowStyle-BackColor="AntiqueWhite" OnDataBound="gvActions_DataBound">
                                                <RowStyle />
                                                <Columns>
                                                    <%--Hidden Column--%>
                                                    <asp:TemplateField HeaderText="ID">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblActionID" runat="server" Text='<%# Eval("ActionID")%>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
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
                                                    <asp:TemplateField HeaderText="Aktionen">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblActionCaption" runat="server" Text='<%# Eval("ActionCaption")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tblFooter1">
                                        <asp:Label ID="lblRowCount" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tblFooter1" colspan="3">
                                        <div align="right">
                                            <asp:Button ID="btnSubmit" runat="server" class="btnSave" Text="Speichern" title="Speichern" OnClick="btnSubmit_Click" />
                                            <asp:Button ID="btnCancel" runat="server" class="btnCancel" title="Abbrechen" Text="Abbrechen" OnClick="btnCancel_Click" />
                                            <asp:Button ID="btnAddSelectedProv" runat="server" class="btnSave" Text="Hinfügen >>" title="Hinfügen" onclick="btnAddSelectedProv_Click"  />
                                            <asp:Button ID="btnCancelSelectedProv" runat="server" class="btnCancel" Text="Abbrechen" title="Abbrechen" onclick="btnCancelSelectedProv_Click" />
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
