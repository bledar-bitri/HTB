<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditLawyer.aspx.cs" Inherits="HTB.v2.intranetx.lawyer.EditLawyer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>HTB.ASP [ Rechtsanwalt ]</title>
    <link href="../../intranet/styles/inksys.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        body
        {
            margin-left: 5px;
            margin-top: 5px;
            margin-right: 5px;
            margin-bottom: 5px;
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
    </style>
</head>
<body>
    <ctl:header ID="header" runat="server" />
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
                                                RECHTSANWALT
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <ctl:message ID="ctlMessage" runat="server"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaptionTopLine">
                                                <div align="right">Geschlecht:</div>
                                            </td>
                                            <td class="EditDataTopLine">
                                                <asp:DropDownList runat="server" ID="ddlGender" class="docText" >
                                                    <asp:ListItem Text="Herr" Value="1" />
                                                    <asp:ListItem Text="Frau" Value="2" />
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaptionTopLine">
                                                <div align="right">Anrede:</div>
                                            </td>
                                            <td class="EditDataTopLine">
                                                <asp:TextBox runat="server" ID="txtAnrede"  onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="50"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">Vorname:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox runat="server" ID="txtName2"  onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="50"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">Nachname:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox runat="server" ID="txtName1"  onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="50"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">Strasse:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox runat="server" ID="txtStreet"  onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="50"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">LKZ / PLZ / Ort:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox runat="server" ID="txtZipPrefix"  onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="3" Text="A"></asp:TextBox>
                                                -
                                                <asp:TextBox runat="server" ID="txtZip"  onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="12"></asp:TextBox>
                                                <asp:TextBox runat="server" ID="txtCity"  onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="50"></asp:TextBox>
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
                                            <td nowrap class="EditCaption">
                                                <div align="right">Fax:</div>
                                            </td>
                                            <td class="EditData">
                                                +
                                                <asp:TextBox runat="server" ID="txtFaxCountry" class="docText" value="43" size="5" MaxLength="5" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" />
                                                (
                                                <asp:TextBox runat="server" ID="txtFaxCity" class="docText" size="10" MaxLength="10" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" />
                                                )
                                                <asp:TextBox runat="server" ID="txtFax" class="docText" size="45" MaxLength="50" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">E-Mail:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox runat="server" ID="txtEmail"  onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="50"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" class="tblFooter1">
                                                <div align="right">
                                                    <asp:Button ID="btnSubmit" runat="server" class="btnSave" title="Speichern " Text="Speichern" onclick="btnSubmit_Click" />
                                                    <%--<asp:Button ID="btnCancel" runat="server" class="btnCancel" title="Abbrechen" Text="Fertig" onclick="btnCancel_Click" />--%>
                                                    <asp:Button ID="btnAddNew" runat="server" class="btnAction" title="Neu Rechtsanwalt" Text="Neu Rechtsanwalt" onclick="btnAddNew_Click" />
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
                                                        <asp:BoundField  HeaderText="Name" DataField="Name" SortExpression="Name">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField  HeaderText="Adresse" DataField="Address" SortExpression="Address" HtmlEncode="false">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField  HeaderText="Telefon" DataField="Phone" SortExpression="Phone">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField  HeaderText="Fax" DataField="Fax" SortExpression="Fax">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField  HeaderText="E-Mail" DataField="EMail" SortExpression="EMail">
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
