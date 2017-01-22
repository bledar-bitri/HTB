<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CtlBrowserDealer.ascx.cs" Inherits="HTB.v2.intranetx.global_files.CtlBrowserDealer" %>


<asp:Panel id="pnlLookup" runat="server" DefaultButton="btnSubmit" >

    <table width="100%" border="0" cellspacing="0" cellpadding="1" bgcolor="#000000">
        <tr>
            <td class="tblHeader">
                W&auml;hlen Sie einen Handler
            </td>
        </tr>
    </table>
    <table width="100%" border="0" cellspacing="0" cellpadding="1" bgcolor="#000000">
        <tr>
            <td>
                <table width="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF">
                    <tr>
                        <td>
                            <table width="100%" border="0" cellspacing="2" cellpadding="2">
                                <tr>
                                    <td class="docText">
                                        Name:
                                    </td>
                                    <td class="docText">
                                        <asp:TextBox ID="txtDealerName" runat="server" class="docText" size="35" />
                                    </td>
                                    <td class="docText">
                                        <div align="right">
                                            <asp:Button ID="btnSubmit" runat="server" Text="Suchen" class="btnSave" OnClick="btnSubmit_Click" />
                                        </div>
                                    </td>
                                    <td class="docText" width="1">
                                        <div align="right">
                                            <asp:Button ID="btnCancel" runat="server" Text="Abbrechen" class="btnCancel" onclick="btnCancel_Click" />
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
    <table width="100%" border="0" cellspacing="0" cellpadding="4">
        <tr>
            <td class="tblCol1">
                <asp:GridView ID="gvDealer" DataKeyNames="AutoDealerID" DataSourceID="ObjectDataSource1" runat="server" AllowPaging="true" PageSize="20" AllowSorting="true" AutoGenerateColumns="False"
                    CellPadding="2" CellSpacing="1" BorderStyle="Inset" Width="100%" 
                    OnDataBound="gvGegner_DataBound" >
                    <RowStyle BackColor="#EFF3FB" />
                    <Columns>
                        <asp:TemplateField HeaderText="ID" SortExpression="AutoDealerID">
                            <ItemTemplate>
                                <div class="gridData">
                                    <%# Eval("AutoDealerID")%></div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Name" SortExpression="AutoDealerName">
                            <ItemTemplate>
                                <div class="gridData">
                                    <%# Eval("DealerNameLink")%>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Address" SortExpression="AutoDealerOrt">
                            <ItemTemplate>
                                <div class="gridData"><%# Eval("DealerAddress")%></div>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <HeaderStyle CssClass="tblCol1"  Font-Bold="True" ForeColor="#000000"/>
                    <AlternatingRowStyle BackColor="White" />
                </asp:GridView>
                <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetAllDealers" EnablePaging="true" TypeName="HTB.v2.intranetx.global_files.CtlBrowserDealer"
                    StartRowIndexParameterName="startIndex" MaximumRowsParameterName="pageSize" SortParameterName="sortBy" SelectCountMethod="GetTotalDealerCount"></asp:ObjectDataSource>
            </td>
        </tr>
    </table>
    <table border="0" width="50%" class="docText">
        <tr>
            <td width="23%" align="center">
            </td>
            <td width="31%" align="center">
            </td>
            <td width="23%" align="center">
            </td>
            <td width="23%" align="center">
            </td>
        </tr>
    </table>
    <span class="docText"></span>
    <br />
    <table width="100%" border="0" cellpadding="1" cellspacing="0" bgcolor="#000000">
        <tr>
            <td>
                <table width="100%" border="0" cellpadding="4" cellspacing="0" bgcolor="#FFFFFF">
                    <tr>
                        <td>
                            <span class="docText"><asp:Label ID="lblTotalDealerMsg" runat="server" /></span>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Panel>
