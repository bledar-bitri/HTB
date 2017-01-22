<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="intranet_header_no_menu_tablet.ascx.cs" Inherits="HTB.intranetx.global_files.intranet_header_no_menu_tablet" %>
<link href="/v2/intranet/styles/htbTablet.css" rel="stylesheet" type="text/css">
<script language="JavaScript" type="text/JavaScript">

<!--
    function MM_openBrWindow(theURL, winName, features) { //v2.0
        window.open(theURL, winName, features);
    }
    function MM_refreshParentAndClose() {
        if (window.opener && !window.opener.closed) {
            window.opener.location.reload();
        }
        window.close();
    }
//-->
</script>
<table id="hdrTable" runat="server" width="100%" border="0" cellpadding="1" cellspacing="0" bgcolor="#000000">
    <tr>
        <td>
            <table width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblTabletLinkStart" />
                        <img src="/v2/intranet/images/Logo1x1cm.png" height="52"/><img src="/v2/intranet/images/newheader.gif" height="52" />
                        <asp:Label runat="server" ID="lblTabletLinkEnd" />
                    </td>
                    <td id="tdUserInfo" runat="server" valign="top" class="headerText">
                        <div align="right" id="divUserInfo" runat="server">
                        </div>
                        <div align="right" id="divTime" runat="server">
                        </div>
                    </td>
                    <td valign="top" class="headerText">
                        <div align="center">
                        </div>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<br/>
