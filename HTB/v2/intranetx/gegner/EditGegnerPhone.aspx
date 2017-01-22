<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditGegnerPhone.aspx.cs" Inherits="HTB.v2.intranetx.gegner.EditGegnerPhone" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>HTB.ASP [ Gegner Telefon ]</title>
    <link href="../../intranet/styles/inksys.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <ctl:headerNoMenu ID="header" runat="server" />
    <form id="form1" runat="server">
     <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" EnablePartialRendering="true" EnableScriptLocalization="true" />
    <asp:UpdatePanel ID="updPanel1" runat="server">
        <ContentTemplate>
            <table width="100%" border="0" cellspacing="0" cellpadding="1">
                <tr>
                    <td bgcolor="#000000">
                        <table width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
                            <tr>
                                <td>
                                    <p>&nbsp;</p>
                                    <table border="0" align="center" cellpadding="3" cellspacing="0">
                                        <tr>
                                            <td colspan="2" class="tblHeader" title="GEGNER TELEFON">
                                                GEGNER TELEFON
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <ctl:message ID="ctlMessage" runat="server"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaptionTopLine">
                                                <div align="right"><strong>Telefon Typ:</strong></div>
                                            </td>
                                            <td class="EditDataTopLine">
                                                <asp:DropDownList runat="server" ID="ddlPhoneType" class="docText" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaptionTopLine">
                                                <div align="right"><strong>Beschreibung:</strong></div>
                                            </td>
                                            <td class="EditDataTopLine">
                                                <asp:TextBox runat="server" ID="txtDescription"  onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="80"></asp:TextBox>
                                            </td>
                                        </tr>
                                        
                                        <tr>
                                            <td nowrap class="EditCaption">
                                                <div align="right">Telefon:</div>
                                            </td>
                                            <td class="EditData">
                                                +
                                                <asp:TextBox runat="server" ID="txtPhoneCountry" class="docText" value="43" size="5" MaxLength="5" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" />
                                                (
                                                <asp:TextBox runat="server" ID="txtPhoneCity" class="docText" size="10" MaxLength="10" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" />
                                                )
                                                <asp:TextBox runat="server" ID="txtPhone" class="docText" size="45" MaxLength="50" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" class="tblFooter1">
                                                <div align="right">
                                                    <asp:Button ID="btnSubmit" runat="server" class="btnSave" title="Speichern " Text="Speichern" onclick="btnSubmit_Click" />
                                                    <asp:Button ID="btnCancel" runat="server" class="btnCancel" title="Abbrechen" Text="Fertig" onclick="btnCancel_Click" />
                                                    <asp:Button ID="btnAddNew" runat="server" class="btnAction" title="Neu Telefonnummer" Text="Neu Telefonnummer" onclick="btnAddNew_Click" />
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <img src="" width="1" height="1" alt="" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tblData1" colspan="2">
                                                <asp:GridView ID="gvGegnerPhones" runat="server" AutoGenerateColumns="False" BorderStyle="Groove" CellPadding="2" Width="100%">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <%# Eval("EditUrl") %>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                         <asp:BoundField  HeaderText="Typ" DataField="PhoneType" SortExpression="PhoneType">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField  HeaderText="Telefon" DataField="PhoneNumber" SortExpression="PhoneNumber">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField  HeaderText="Beschreibung" DataField="Description" SortExpression="Description">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                    <p>&nbsp;</p>
                                    <asp:HiddenField runat="server" ID="hdnIsAddNewClicked"/>
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
