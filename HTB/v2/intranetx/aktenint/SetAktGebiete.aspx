<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SetAktGebiete.aspx.cs" Inherits="HTB.v2.intranetx.aktenint.SetAktGebiete" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>HTB [ Akt Gebiete ]</title>
</head>
<body>
<ctl:headerNoMenu ID="HeaderNoMenu" runat="server" />

    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" EnablePartialRendering="true" EnableScriptLocalization="true" />
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        function BeginRequestHandler(sender, args) {
            document.getElementById('<%= btnShowAkten.ClientID %>').innerText = "Processing";
            document.getElementById('<%= btnShowAkten.ClientID %>').disabled = true;
            
            if (document.getElementById('<%= btnProcessAkten.ClientID %>') != null) {
                document.getElementById('<%= btnProcessAkten.ClientID %>').innerText = "Processing";
                document.getElementById('<%= btnProcessAkten.ClientID %>').disabled = true;
            }
        }
    </script>
     <table width="100%" border="0" cellpadding="1" cellspacing="0" bgcolor="#000000">
        <tr>
            <td>
                <table width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
                    <tr>
                        <td class="smallText">
                            <a href="../../intranet/intranet/intranet.asp">Intranet</a> | <a href="../../intranet/intranet/mydata.asp">CollectionInvoice</a> | <a href="../../intranet/adgebiete/adgebiete.asp"> AD - Gebiete</a> | Akten &uuml;bernehmen
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <br />
    <asp:UpdatePanel ID="updPanel1" runat="server">
        <ContentTemplate>
            <table width="100%" border="0" cellspacing="0" cellpadding="1">
                <tr>
                    <td bgcolor="#000000">
                        <table width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
                            <tr>
                                <td colspan="2" class="tblHeader" title="INTERVENTION">
                                    Akten &uuml;bernehmen
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="tblDataAll">
                                    <ctl:message ID="ctlMessage" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="tblDataAll">
                                    <asp:Button runat="server" ID="btnShowAkten" Text="&Uuml;bernehmbare Akten zeigen"   OnClick="BtnShowAktenClicked" CssClass="btnStandard"/>
                                    <asp:Button runat="server" ID="btnProcessAkten" Text="&Uuml;bernemen"   OnClick="BtnProcessAktenClicked" CssClass="btnSave"/>
                                </td>
                            </tr>

                            <tr>
                                <td colspan="2" class="tblDataAll">
                                    <br/>
                                    <strong>Aktive Sachbearbeiter:</strong><br/><br />
                                    <asp:CheckBoxList id="chklst" runat="server" />
                                </td>
                            </tr>

                            <tr>
                                <td colspan="2" class="tblHeader" title="Akten">
                                    <asp:Panel ID="pnlAkte" runat="server">
                                        <asp:GridView ID="gvAkte" runat="server" AllowSorting="false" AutoGenerateColumns="False" CellPadding="2" BorderStyle="Groove" Width="100%" ShowFooter="true">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Akt" Visible="false"> 
                                                    <ItemTemplate>
                                                        <asp:Label id="lblAkt" runat="server" Text='<%# Eval("Akt")%>' />
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="tblDataAll"/>
                                                    <FooterStyle CssClass="tblDataAll"/>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Datum" DataField="EnterDate" SortExpression="EnterDate"><ItemStyle CssClass="tblDataAll"/><FooterStyle CssClass="tblDataAll"/></asp:BoundField>
                                                <asp:BoundField HeaderText="Termin" DataField="DueDate" SortExpression="DueDate" ><ItemStyle CssClass="tblDataAll"/><FooterStyle CssClass="tblDataAll"/></asp:BoundField>
                                                <asp:BoundField HeaderText="Klient" DataField="Client" SortExpression="Client" ><ItemStyle CssClass="tblDataAll"/><FooterStyle CssClass="tblDataAll"/></asp:BoundField>
                                                <asp:BoundField HeaderText="Gegner" DataField="Gegner" SortExpression="Gegner" HtmlEncode="false"><ItemStyle CssClass="tblDataAll"/><FooterStyle CssClass="tblDataAll"/></asp:BoundField>
                                                <asp:BoundField HeaderText="Auftraggeber" DataField="AG" SortExpression="AG" ><ItemStyle CssClass="tblDataAll"/><FooterStyle CssClass="tblDataAll"/></asp:BoundField>
                                                <asp:BoundField HeaderText="Sachbearbeiter" DataField="SB" SortExpression="SB" HtmlEncode="false"><ItemStyle CssClass="tblDataAll"/><FooterStyle CssClass="tblDataAll"/></asp:BoundField>
                                                <asp:TemplateField HeaderText="SB ID"> 
                                                    <ItemTemplate>
                                                        <asp:Label id="lblSbID" runat="server" Text='<%# Eval("SBID")%>' />
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="tblDataAll" HorizontalAlign="Right"/>
                                                    <FooterTemplate>
                                                        <strong>Total: <%=GetTotalAktenToFix().ToString()%> Akten</strong>
                                                    </FooterTemplate>
                                                    <FooterStyle CssClass="tblDataAll"/>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
