<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="HTB.v2.intranetx.test.Test" %>
<%@ Register Src="~/v2/intranetx/global_files/CtlMessage.ascx" TagName="message" TagPrefix="ctl" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <ctl:header runat="server" ID="cltHeader"/>
    <form id="form1" runat="server">
        <table width="632" border="0" align="center" cellpadding="3" cellspacing="0">
            <tr>
                <td colspan="2" class="tblHeader">
                    Testen
                </td>
            </tr>
            <tr>
                <td width="10%" valign="top" class="EditCaption">
                    <div align="right"><strong>&nbsp;</strong></div>
                </td>
                <td class="EditData">
                    <ctl:message ID="ctlMessage" runat="server"/>
                </td>
            </tr>
            <tr>
                <td width="10%" valign="top" class="EditCaption">
                    <div align="right"><strong>Akt Nr.&nbsp;</strong>:</div>
                </td>
                <td class="EditData">
                    <asp:TextBox runat="server" ID="txtAktID"/>
                </td>
            </tr>
            <tr>
                <td width="10%" valign="top" class="EditCaption">
                    <div align="right"><strong>&nbsp;</strong></div>
                </td>
                <td class="EditData">
                 <asp:LinkButton runat="server" ID="btnTestLaywerPackage" Text="Test Lawyer Package" OnClick="btnTestLaywerPackage_Clicked"></asp:LinkButton>
                </td>
            </tr>
         </table>
    </form>
</body>
</html>
