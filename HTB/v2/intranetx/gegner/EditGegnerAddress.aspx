<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditGegnerAddress.aspx.cs" Inherits="HTB.v2.intranetx.gegner.EditGegnerAddress" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>HTB.ASP [ Gegner Adresse ]</title>
    <link href="../../intranet/styles/inksys.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <ctl:headerNoMenu ID="header" runat="server" />
    <form id="form1" runat="server">
     <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" EnablePartialRendering="true" EnableScriptLocalization="true" />
    <asp:UpdatePanel ID="updPanel1" runat="server">
        <Triggers>
           <asp:AsyncPostBackTrigger ControlID="txtGAZip" EventName="TextChanged" />
        </Triggers>
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
                                            <td colspan="2" class="tblHeader" title="GEGNER ADRESSE">
                                                GEGNER ADRESSE
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <ctl:message ID="ctlMessage" runat="server"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaptionTopLine">
                                                <div align="right"><strong>AdressE Typ:</strong></div>
                                            </td>
                                            <td class="EditDataTopLine">
                                                <asp:DropDownList runat="server" ID="ddlGAType" class="docText" >
                                                    <asp:ListItem Value="1" Text="KL"/>
                                                    <asp:ListItem Value="2" Text="ZMR"/>
                                                    <asp:ListItem Value="3" Text="AE"/>
                                                </asp:DropDownList>
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
                                                <div align="right">Nachname / Firma:&nbsp;</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox runat="server" ID="txtGAName1" class="docText" size="50" maxlength="100" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td nowrap class="EditCaption">
                                                <div align="right">Vorname:&nbsp;</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox runat="server" ID="txtGAName2" class="docText" size="50" maxlength="100" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td nowrap class="EditCaption">
                                                <div align="right">Alias:&nbsp;</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox runat="server" ID="txtGAName3" class="docText" size="50" maxlength="100" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td nowrap class="EditCaption">
                                                <div align="right">Strasse:&nbsp;</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox runat="server" ID="txtGAStrasse" class="docText" size="50" maxlength="100" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td nowrap class="EditCaption">
                                                <div align="right">Land/PLZ/Ort:&nbsp;</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox runat="server" ID="txtGAZipPrefix" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="5" maxlength="3"/>
                                                <asp:TextBox runat="server" ID="txtGAZip" AutoPostBack="true" OnTextChanged="txtGAZip_TextChanged" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="12" maxlength="10"/>
                                                <asp:TextBox runat="server" ID="txtGAOrt" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="50" maxlength="50"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" class="tblFooter1">
                                                <div align="right">
                                                    <asp:Button ID="btnSubmit" runat="server" class="btnSave" title="Speichern " Text="Speichern" onclick="btnSubmit_Click" />
                                                    <asp:Button ID="btnCancel" runat="server" class="btnCancel" title="Abbrechen" Text="Fertig" onclick="btnCancel_Click" />
                                                    <asp:Button ID="btnAddNew" runat="server" class="btnAction" title="Neu Adresse" Text="Neu Adresse" onclick="btnAddNew_Click" />
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
                                                <asp:GridView ID="gvGegnerAddresses" runat="server" AutoGenerateColumns="False" BorderStyle="Groove" CellPadding="2" Width="100%">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <%# Eval("EditUrl") %>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField  HeaderText="Beschreibung" DataField="Description" SortExpression="Description" HtmlEncode="false"/>
                                                        <asp:BoundField  HeaderText="Nachname / Firma" DataField="Name1" SortExpression="Name2" HtmlEncode="false">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField  HeaderText="Vorname" DataField="Name2" SortExpression="Name2" HtmlEncode="false">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField  HeaderText="Typ" DataField="Type" SortExpression="Type" HtmlEncode="false">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField  HeaderText="Datum" DataField="Date" SortExpression="Date" HtmlEncode="false">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField  HeaderText="Adresse" DataField="Address" SortExpression="Address" HtmlEncode="false">
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
