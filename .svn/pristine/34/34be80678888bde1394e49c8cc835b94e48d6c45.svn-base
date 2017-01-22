<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditDealer.aspx.cs" Inherits="HTB.v2.intranetx.dealer.EditDealer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>HTB.ASP [ Handler ]</title>
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
    </style>
</head>
<body>
    <ctl:headerNoMenu ID="header" runat="server" />
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
        <Triggers>
           <asp:AsyncPostBackTrigger ControlID="txtAutoDealerPLZ" EventName="TextChanged" />
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
                                            <td colspan="2" class="tblHeader" title="Handler">
                                                HANDLER
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <ctl:message ID="ctlMessage" runat="server"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaptionTopLine">
                                                <div align="right">
                                                    Handler Nr.:
                                                </div>
                                            </td>
                                            <td class="EditDataTopLine">
                                                <asp:Label runat="server" ID="lblAutoDealerID" Text="(wird autom. vergeben)"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">
                                                    <strong>Firma</strong>:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox runat="server" ID="txtAutoDealerName" class="docText" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">Strasse:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox runat="server" ID="txtAutoDealerStrasse" class="docText" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td nowrap class="EditCaption">
                                                <div align="right">LKZ / PLZ / Ort:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox runat="server" ID="txtAutoDealerLKZ" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" value="A" size="3" MaxLength="3" />
                                                -
                                                <asp:TextBox runat="server" ID="txtAutoDealerPLZ" AutoPostBack="true" OnTextChanged="txtAutoDealerPLZ_TextChanged" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="12" MaxLength="12" />
                                                <asp:TextBox runat="server" ID="txtAutoDealerOrt" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="50" MaxLength="50" />
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
                                            <td nowrap class="EditCaption">
                                                <div align="right">E-Mail:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox runat="server" ID="txtEmail" class="docText" size="50" MaxLength="50" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td nowrap class="EditCaption">
                                                <div align="right">Web:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox runat="server" ID="txtAutoDealerWeb" class="docText" size="50" MaxLength="50" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" class="tblFooter1">
                                                <div align="right">
                                                    <asp:Button ID="btnSubmit" runat="server" class="btnSave" title="Speichern " Text="Speichern" onclick="btnSubmit_Click" />
                                                    <asp:Button ID="btnCancel" runat="server" class="btnCancel" title="Abbrechen" Text="Abbrechen" onclick="btnCancel_Click" />
                                                    <asp:Button ID="btnAddNew" runat="server" class="btnAction" title="Neu Handler" Text="Neu Handler" onclick="btnAddNew_Click" />
                                                </div>
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
</body>
</html>
