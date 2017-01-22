<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IntranetTablet.aspx.cs" Inherits="HTB.v2.intranetx.intranet.tablet.IntranetTablet" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>HTB.ASP [ Intranet ]</title>
    <link href="/v2/intranet/styles/htbTablet.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <ctl:headerNoMenuTablet runat="server" />
    <form id="form1" runat="server">
    <table width="100%" border="0" cellspacing="0" cellpadding="1">
        <tr>
            <td bgcolor="#000000">
                <table width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#000000">
                    <tr>
                        <td height="22" background="/v2/intranet/images/hdbackblue.gif" class="BoxHeader" title="Sie k&ouml;nnen aus vorgeschlagenen Ordnern w&auml;hlen.">
                            &Uuml;BERSICHT
                        </td>
                    </tr>
                </table>
                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF">
                    <tr>
                        <td valign="top">
                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td colspan="6">
                                        <table width="100%" border="0" cellspacing="0" cellpadding="4">
                                            <tr>
                                                <td>
                                                    <div align="center" class="smallText">
                                                        <a href="/v2/intranet/kalender/monat.asp">
                                                            <img src="/v2/intranet/images/ic_cal48.gif" width="100" height="100" border="0" title="Hier k&ouml;nnen Sie Ihren Kalender ansehen und verwalten."></a><br>
                                                        <a href="/v2/intranet/kalender/monat.asp" title="Hier k&ouml;nnen Sie Ihren Kalender ansehen und verwalten.">Mein Kalender</a>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div align="center">
                                                        <a href="/v2/intranet/aktenint/aktenint.asp">
                                                            <img src="/v2/intranet/images/bf_Interventionen_Small.gif" width="100" height="100" border="0"><br />
                                                            <span class="smallText">Interventionen</span></a></div>
                                                </td>
                                                <td>
                                                    <div align="center">
                                                        <a href="/v2/intranetx/routeplanter/bingmaps/tablet/BingRoutePlanerTablet.aspx">
                                                            <img src="/v2/intranet/images/RouteIcon.jpg" alt="" width="100" height="100" border="0" /><br />
                                                            <span class="smallText">Routenplaner</span></a>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <div align="center" class="smallText">
                                                        <a href="/v2/intranetx/aktenint/tablet/ProvisionAbrechnungTablet.aspx">
                                                            <img src="/v2/intranet/images/ic_provisionen48.gif" width="100" height="100" border="0" title="Hier k&ouml;nnen Sie Ihren Provisionen ansehen."></a><br>
                                                        <a href="/v2/intranetx/aktenint/tablet/ProvisionAbrechnungTablet.aspx" title="Hier k&ouml;nnen Sie Ihren Provisionen ansehen.">Meine Provisionen</a>
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
            </td>
        </tr>
    </table>
    </form>
    <ctl:footer runat="server" />
</body>
</html>
