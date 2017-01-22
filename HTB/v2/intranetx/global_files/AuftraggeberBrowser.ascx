<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AuftraggeberBrowser.ascx.cs" Inherits="HTB.v2.intranetx.global_files.AuftraggeberBrowser" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<ajax:ModalPopupExtender ID="MPE" runat="server"
    TargetControlID="btnAG"
    PopupControlID="panEdit"
    BackgroundCssClass="modalBackground" 
    DropShadow="true" 
    PopupDragHandleControlID="panEdit" />

<asp:Panel ID="panEdit" runat="server" >
    <h1>
        Edit</h1>
    <table width="100%" border="0" cellspacing="0" cellpadding="1" bgcolor="#000000">
        <tr>
            <td class="tblHeader">
                W&auml;hlen Sie einen Auftraggeber
            </td>
        </tr>
    </table>
    <br />
    <table width="100%" border="0" cellspacing="0" cellpadding="1" bgcolor="#000000">
        <tr>
            <td>
                <table width="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF">
                    <tr>
                        <td>
                            <form action="" method="get" name="form1">
                            <table width="100%" border="0" cellspacing="2" cellpadding="2">
                                <tr>
                                    <td class="docText">
                                        Name:
                                    </td>
                                    <td class="docText">
                                        <asp:TextBox ID="txtName" runat="server" class="docText" size="35" />
                                    </td>
                                    <td class="docText">
                                        &nbsp;
                                    </td>
                                    <td class="docText" width="1">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="docText">
                                        &nbsp;
                                    </td>
                                    <td class="docText">
                                        &nbsp;
                                    </td>
                                    <td class="docText">
                                        <div align="right">
                                            <input type="submit" name="Submit2" value="Suchen" class="docText">
                                        </div>
                                    </td>
                                    <td class="docText" width="1">
                                        <div align="right">
                                            <input type="button" name="Submit23" value="Abbrechen" class="docText" onclick="window.close();">
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            </form>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <br />
    <table width="100%" border="0" cellspacing="0" cellpadding="4">
        <tr>
        <td>
            <asp:GridView ID="gvAG" runat="server" AllowSorting="false" AutoGenerateColumns="False" CellPadding="2" CellSpacing="2" BorderStyle="Inset" Width="100%" AlternatingRowStyle-BackColor="AntiqueWhite">
                <RowStyle />
                <Columns>
                    <%--Hidden Column--%>
                    <asp:TemplateField HeaderText="ID" Visible="false"> 
                        <ItemTemplate>
                            <asp:Label id="lblInstallmentId" runat="server" Text='<%# Eval("ID")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField HeaderText="ID" DataField="ID" SortExpression="ID" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField HeaderText="Name" DataField="Name" SortExpression="Name" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField HeaderText="Ort" DataField="Address" SortExpression="Address" ItemStyle-HorizontalAlign="Left" />
                </Columns>
            </asp:GridView>
        </td>
        </tr>  
    </table>
</asp:Panel>
<asp:Button ID="btnAG" runat="server" class="smallText" title="?" Text="? [F11]" />