<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditGegner.aspx.cs" Inherits="HTB.v2.intranetx.gegner.EditGegner" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>HTB.ASP [ Neuer Gegner ]</title>
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
    <script src="../../intranet/globalcode/lib.js" type="text/javascript" language="javascript"></script>
</head>
<body>
    <ctl:headerNoMenu ID="header" runat="server" />
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" EnablePartialRendering="true" EnableScriptLocalization="true" />
   <%-- <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        function BeginRequestHandler(sender, args) {
            document.getElementById('<%= btnSubmit.ClientID %>').innerText = "Processing";
            document.getElementById('<%= btnSubmit.ClientID %>').disabled = true;
        }
    </script>--%>
    <asp:UpdatePanel ID="updPanel1" runat="server">
        <%--<Triggers>
           <asp:AsyncPostBackTrigger ControlID="txtLastZIP" EventName="TextChanged" />
        </Triggers>--%>
        <ContentTemplate>
            <table width="100%" border="0" cellspacing="0" cellpadding="1">
                <tr>
                    <td bgcolor="#000000">
                        <table width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
                            <tr>
                                <td>
                                    <p>
                                        &nbsp;</p>
                                    <table border="0" align="center" cellpadding="3" cellspacing="0">
                                        <tr>
                                            <td colspan="2" class="tblHeader" title="GEGNER NEU">
                                                GEGNER
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <ctl:message ID="ctlMessage" runat="server"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <img name="" src="" width="1" height="1" alt="" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" class="tblHeader" title="Adressdaten">
                                                Adressdaten
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right"><strong>Gegner Typ:</strong></div>
                                            </td>
                                            <td class="EditData">
                                                <asp:DropDownList runat="server" ID="ddlGegnerType" class="docText" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">
                                                    Titel:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:Label runat="server" ID="lblAnrede" class="docText"  />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">
                                                    <strong>Nachname / Firma</strong>:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:Label runat="server" ID="lblName1" class="docText" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">
                                                    <strong>Vorname</strong>:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:Label runat="server" ID="lblName2" class="docText" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">
                                                    Alias:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:Label runat="server" ID="lblName3" class="docText" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">
                                                    Strasse:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:Label runat="server" ID="lblStrasse" class="docText" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td nowrap class="EditCaption">
                                                <div align="right">
                                                    LKZ / PLZ / Ort:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:Label runat="server" ID="lblZIPPrefix" class="docText" />
                                                -
                                                <asp:Label runat="server" ID="lblZIP" class="docText" />
                                                <asp:Label runat="server" ID="lblOrt" class="docText" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" class="tblFooter1">
                                                <div align="right">
                                                    <asp:Button ID="btnSubmit" runat="server" class="btnSave" title="Speichern " Text="Speichern" onclick="btnSubmit_Click" />
                                                    <asp:Button ID="btnCancel" runat="server" class="btnCancel" title="Abbrechen" Text="Abbrechen" onclick="btnCancel_Click" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                    <p>
                                        &nbsp;</p>
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
