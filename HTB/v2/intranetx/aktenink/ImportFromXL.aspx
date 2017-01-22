<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImportFromXL.aspx.cs" Inherits="HTB.v2.intranetx.aktenink.ImportFromXL" %>
<%@ Register Src="~/v2/intranetx/global_files/CtlMessage.ascx" TagName="message" TagPrefix="ctl" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>HTB.ASP [ Import ECP CollectionInvoiceakte ]</title>
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
        .style1
        {
            color: #FF0000;
        }
        OPTION.dis
        {
            background-color: white;
            color: #999999;
        }
    </style>
</head>
<body>
    <ctl:headerNoMenu runat="server"/>
    <form id="form1" runat="server" enctype="multipart/form-data">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" EnablePartialRendering="true" EnableScriptLocalization="true" />
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        function BeginRequestHandler(sender, args) {
            document.getElementById('<%= btnSubmit.ClientID %>').innerText = "Processing";
            document.getElementById('<%= btnSubmit.ClientID %>').disabled = true;
            document.getElementById('<%= btnCancel.ClientID %>').innerText = "Processing";
            document.getElementById('<%= btnCancel.ClientID %>').disabled = true;
        }
    </script>
    <table width="100%" border="0" cellpadding="1" cellspacing="0" bgcolor="#000000">
        <tr>
            <td>
                <table width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
                    <tr>
                        <td class="smallText">
                            <a href="../../intranet/intranet/intranet.asp">Intranet</a> | <a href="../../intranet/intranet/inkasso.asp">CollectionInvoice</a> | <a href="../../intranet/aktenink/AktenStaff.asp">
                                CollectionInvoiceakte (&Uuml;bersicht)</a> | Import CollectionInvoiceakte
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <br />
    
    <table width="100%" border="0" cellspacing="0" cellpadding="1">
        <tr>
            <td bgcolor="#000000">
                <table width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
                    <tr>
                        <td>
                            <table border="0" align="center" cellpadding="3" cellspacing="0">
                                <tr>
                                    <td colspan="2" class="tblHeader" title="INTERVENTION">
                                        IMPORT INKASSOAKTEN
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" class="tblDataAll">
                                        <ctl:message ID="ctlMessage" runat="server" />
                                    </td>
                                </tr>

                                <tr>
                                    <td class="EditCaption">
                                        <div align="right">Klient:</div>
                                    </td>
                                    <td class="EditData">
                                        <asp:DropDownList ID="ddlKlient" runat="server" class="docText" AutoPostBack="True" OnSelectedIndexChanged="btnLoadKlientSB_Click"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="EditCaption">
                                        <div align="right">Klient Sachbearbeiter:</div>
                                    </td>
                                    <td class="EditData">
                                        <asp:DropDownList ID="ddlKlientSB" runat="server" class="docText"/>&nbsp;&nbsp;
                                        <asp:Button runat="server" ID="btnLoadKlientSB" OnClick="btnLoadKlientSB_Click" Text="Laden"/>
                                    </td>
                                </tr>
                                
                                 <tr>
                                    <td class="EditCaption">
                                        <div align="right">Datei:</div>
                                    </td>
                                    <td class="tblDataAll">
                                        <INPUT ID="fileUpload" type="file" name="File1" runat="server" size="100"/>
                                    </td>
                                </tr>

                                <tr>
                                    <td colspan="2" class="tblFooter1">
                                        <div align="right">
                                            <asp:Button ID="btnSubmit" runat="server" class="btnSave" title="Speichern" Text="Speichern" OnClick="btnSubmit_Click" />
                                            <asp:Button ID="btnCancel" runat="server" class="btnCancel" title="Abbrechen" OnClick="btnCancel_Click" Text="Abbrechen" />
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
    <ctl:footer runat="server"/>
</body>
</html>
