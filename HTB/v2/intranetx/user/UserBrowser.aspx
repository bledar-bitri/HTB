<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserBrowser.aspx.cs" Inherits="HTB.v2.intranetx.klienten.UserBrowser" %>

<%@ Register TagPrefix="ctl" TagName="ctlBrowserUser" Src="~/v2/intranetx/global_files/CtlBrowserUser.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <base target=_self> <%--so that it posts back on iteself instead of a new window --%>
    <title>Klienten</title>
    <link href="../../intranet/styles/inksys.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        body {
	        background-image: url(../images/osxback.gif);
        }
        a:link {
	        color: #CC0000;
	        text-decoration: none;
        }
        a:visited {
	        color: #CC0000;
	        text-decoration: none;
        }
        a:hover {
	        color: #CC0000;
	        text-decoration: underline;
        }
        a:active {
	        color: #CC0000;
	        text-decoration: none;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table  width="80%"  align="center">
            <tr>
            <td>
                <ctl:ctlBrowserUser ID="ctlBrowser" runat="server" />
            </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
