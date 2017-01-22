<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Werber.aspx.cs" Inherits="HTB.v2.intranetx.werber.Werber" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>HTB.ASP [Werber]</title>
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
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true"
        EnablePartialRendering="true" EnableScriptLocalization="true" />
    <asp:UpdatePanel ID="updPanel1" runat="server">
        <ContentTemplate>
            <table width="100%" border="0" cellspacing="0" cellpadding="1">
                <tr>
                    <td bgcolor="#000000">
                        <table width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
                            <tr>
                                <td>
                                    <table width="100%" border="0" cellpadding="3" cellspacing="0">
                                        <tr>
                                            <td colspan="12">
                                                <ctl:message ID="ctlMessage" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="12" class="tblHeader">
                                                Inkassoakte - Provisions&uuml;bersicht
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tblData1">
                                                <asp:GridView ID="gvTransfers" runat="server" AllowSorting="false" AutoGenerateColumns="False"
                                                    CellPadding="2" CellSpacing="2" Width="100%" AlternatingRowStyle-BackColor="AntiqueWhite" ShowFooter="true">
                                                    <RowStyle />
                                                    <Columns>
                                                        <asp:BoundField HeaderText="Akt" DataField="AktID" />
                                                        <asp:BoundField HeaderText="Klient" DataField="Client" />
                                                        
                                                        <asp:TemplateField HeaderText="Datum">
                                                            <ItemTemplate>
                                                                <%#Eval("AktDate")%>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Aktstatus">
                                                            <ItemTemplate>
                                                                <%#Eval("AktStatus")%>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Schuldner">
                                                            <ItemTemplate>
                                                                <%#Eval("Gegner")%>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <FooterTemplate>
                                                                Total:&nbsp;&nbsp;&nbsp;<%= GetTotalCount(0) %>&nbsp;Akte
                                                            </FooterTemplate>
                                                            <FooterStyle HorizontalAlign="Right" Font-Bold="true"/>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Forderung">
                                                            <ItemTemplate>
                                                                <%#Eval("KlientAmount")%>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                            <FooterTemplate>
                                                                <%= GetTotalForderung() %>
                                                            </FooterTemplate>
                                                            <FooterStyle HorizontalAlign="Right" Font-Bold="true"/>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="ECP Kosten">
                                                            <ItemTemplate>
                                                                <%#Eval("ECPAmount") %>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                            <FooterTemplate>
                                                                <%= GetTotalEcpKostenString() %>
                                                            </FooterTemplate>
                                                            <FooterStyle HorizontalAlign="Right" Font-Bold="true"/>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Provision (gesch&aumltzt)">
                                                            <ItemTemplate>
                                                                <%#Eval("ProjectedProvision")%>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                            <FooterTemplate>
                                                                <%= GetTotalProjectedProvision() %>
                                                            </FooterTemplate>
                                                            <FooterStyle HorizontalAlign="Right" Font-Bold="true"/>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="ECP Kosten Kassiert">
                                                            <ItemTemplate>
                                                                <%#Eval("ECPAmountReceived")%>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                            <FooterTemplate>
                                                                <%= GetTotalEcpReceivedString() %>
                                                            </FooterTemplate>
                                                            <FooterStyle HorizontalAlign="Right" Font-Bold="true"/>
                                                        </asp:TemplateField>

                                                        
                                                        <asp:TemplateField HeaderText="Provision">
                                                            <ItemTemplate>
                                                                <%#Eval("Provision")%>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                            <FooterTemplate>
                                                                <%= GetTotalProvision() %>
                                                            </FooterTemplate>
                                                            <FooterStyle HorizontalAlign="Right" Font-Bold="true"/>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Provision (&Uumlberwiesen)">
                                                            <ItemTemplate>
                                                                <%#Eval("ProvisionTransferred")%>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                            <FooterTemplate>
                                                                <%= GetTotalProvisionTransferred() %>
                                                            </FooterTemplate>
                                                            <FooterStyle HorizontalAlign="Right" Font-Bold="true"/>
                                                        </asp:TemplateField>

                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="12">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tblHeader" colspan="12">
                                                Stornierte Akte
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tblData1">
                                                <asp:GridView ID="gvStorno" runat="server" AllowSorting="false" AutoGenerateColumns="False"
                                                    CellPadding="2" CellSpacing="2" Width="100%" AlternatingRowStyle-BackColor="AntiqueWhite" ShowFooter="true">
                                                    <RowStyle />
                                                    <Columns>
                                                        <asp:BoundField HeaderText="Akt" DataField="AktID" />
                                                        <asp:BoundField HeaderText="Klient" DataField="Client" />
                                                        
                                                        <asp:TemplateField HeaderText="Datum">
                                                            <ItemTemplate>
                                                                <%#Eval("AktDate")%>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Aktstatus">
                                                            <ItemTemplate>
                                                                <%#Eval("AktStatus")%>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Schuldner">
                                                            <ItemTemplate>
                                                                <%#Eval("Gegner")%>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <FooterTemplate>
                                                                Total:&nbsp;&nbsp;&nbsp;<%= GetTotalCount(1) %>&nbsp;Akte
                                                            </FooterTemplate>
                                                            <FooterStyle HorizontalAlign="Right" Font-Bold="true"/>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Forderung">
                                                            <ItemTemplate>
                                                                <%#Eval("KlientAmount")%>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                            <FooterTemplate>
                                                                <%= GetTotalForderung(true) %>
                                                            </FooterTemplate>
                                                            <FooterStyle HorizontalAlign="Right" Font-Bold="true"/>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="ECP Kosten">
                                                            <ItemTemplate>
                                                                <%#Eval("ECPAmount") %>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                            <FooterTemplate>
                                                                <%= GetTotalEcpKostenString(true) %>
                                                            </FooterTemplate>
                                                            <FooterStyle HorizontalAlign="Right" Font-Bold="true"/>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Provision (gesch&aumltzt)">
                                                            <ItemTemplate>
                                                                <%#Eval("ProjectedProvision")%>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                            <FooterTemplate>
                                                                <%= GetTotalProjectedProvision(true) %>
                                                            </FooterTemplate>
                                                            <FooterStyle HorizontalAlign="Right" Font-Bold="true"/>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="ECP Kosten Kassiert">
                                                            <ItemTemplate>
                                                                <%#Eval("ECPAmountReceived")%>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                            <FooterTemplate>
                                                                <%= GetTotalEcpReceivedString(true) %>
                                                            </FooterTemplate>
                                                            <FooterStyle HorizontalAlign="Right" Font-Bold="true"/>
                                                        </asp:TemplateField>

                                                        
                                                        <asp:TemplateField HeaderText="Provision">
                                                            <ItemTemplate>
                                                                <%#Eval("Provision")%>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                            <FooterTemplate>
                                                                <%= GetTotalProvision(true) %>
                                                            </FooterTemplate>
                                                            <FooterStyle HorizontalAlign="Right" Font-Bold="true"/>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Provision (&Uumlberwiesen)">
                                                            <ItemTemplate>
                                                                <%#Eval("ProvisionTransferred")%>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                            <FooterTemplate>
                                                                <%= GetTotalProvisionTransferred(true) %>
                                                            </FooterTemplate>
                                                            <FooterStyle HorizontalAlign="Right" Font-Bold="true"/>
                                                        </asp:TemplateField>

                                                    </Columns>
                                                </asp:GridView>
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
