<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Customer.aspx.cs" Inherits="HTB.v2.intranetx.customer.Customer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>HTB.ASP [ Kundenportal ]</title>
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
    <ctl:headerNoMenu ID="hdr" runat="server" />
    <form id="form1" runat="server">
    <asp:Panel ID="pnlInkasso" runat="server" Visible="false">
        <table width="100%" border="0" cellspacing="0" cellpadding="1">
            <tr>
                <td bgcolor="#000000">
                    <table width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#000000">
                        <tr>
                            <td class="tblHeader">
                                <asp:Label ID="lblHeader" runat="server" />
                            </td>
                        </tr>
                    </table>
                    <table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF">
                        <tr>
                            <td>
                                <table width="100%" border="0" cellspacing="0" cellpadding="5">
                                    <tr>
                                        <td width="120" valign="top">
                                            <div align="center">
                                                <a href="AktenInk.aspx">
                                                    <img src="../../intranet/images/sc_Inkasso.gif" width="100" height="93" hspace="3" vspace="3" border="1" />
                                                </a></div>
                                        </td>
                                        <td valign="top">
                                            <span class="docText"><strong><a href="AktenInk.aspx">CollectionInvoice</a></strong></span><br />
                                            <span class="smallText">Hier &uuml;berpr&uuml;fen Sie den aktuellen Status Ihrer Auftr&auml;ge.<br />
                                                Ausserdem haben Sie die M&ouml;glichkeit mit unseren SachbearbeiterInnen Kontakt aufzunehmen und abgeschlossene Auftr&auml;ge in Ihr System zu &uuml;bernehmen.
                                            </span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="120" valign="top">
                                            <div align="center">
                                                <a href="TransferToCustomer.aspx">
                                                    <img src="../../intranet/images/sc_Transfer.gif" width="100" height="93" hspace="3" vspace="3" border="1" alt=""/>
                                                </a>
                                            </div>
                                        </td>
                                        <td valign="top">
                                            <span class="docText"><strong><a href="TransferToCustomer.aspx">&Uuml;berweisungsliste</a></strong></span><br />
                                            <span class="smallText">Hier &uuml;berpr&uuml;fen Sie die &Uuml;berweisungen.<br />

                                            </span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div align="center">
                                                <a href="InkassoStatistic.aspx">
                                                    <img src="../../intranet/images/sc_Statistik.gif" width="100" height="93" hspace="3" vspace="3" border="1" alt=""/>
                                                </a>
                                            </div>
                                        </td>
                                        <td valign="top">
                                            <span class="docText"><strong><a href="InkassoStatistic.aspx">CollectionInvoicestatistiken</a> </strong>
                                                <br />
                                                <span class="smallText">Hier sehen Sie Auftragsstatistiken, Verlaufsgrafiken sowie Zeitstatistiken. </span></span>
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr background="../../intranet/images/hdbackblue.gif">
                                        <td height="22" colspan="3" class="BoxHeaderBlue">
                                            <strong class="smallText">ALLGEMEINES</strong>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <div align="center">
                                                <a href="../../intranet/intranet/mysettings.asp">
                                                    <img src="../../intranet/images/sc_Settings.gif" width="100" height="75" hspace="3" vspace="3" border="1" alt=""/>
                                                </a>
                                            </div>
                                        </td>
                                        <td valign="top">
                                            <span class="docText"><strong><a href="../../intranet/intranet/mysettings.asp">Ihre Einstellungen </a></strong>
                                                <br />
                                                <span class="smallText">Hier warten Sie Ihre pers&ouml;nlichen Daten, &auml;ndern Ihr Passwort und konfigurieren Ihre Anzeigeoptionen. </span></span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnlIntervention" runat="server" Visible="false">
        <table width="100%" border="0" cellspacing="0" cellpadding="1">
            <tr>
                <td bgcolor="#000000">
                    <table width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#000000">
                        <tr>
                            <td class="tblHeader">
                                <asp:Label ID="lblHeaderIntervention" runat="server" />
                            </td>
                        </tr>
                    </table>
                    <table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF">
                        <tr>
                            <td>
                                <table width="100%" border="0" cellspacing="0" cellpadding="5">
                                    <tr background="../../intranet/images/hdbackblue.gif">
                                        <td height="22" colspan="3" class="BoxHeaderBlue">
                                            <strong>INTERVENTION</strong>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="120" valign="top">
                                            <div align="center">
                                                <a href="AktenInt.aspx">
                                                    <img src="../../intranet/images/sc_Inkasso.gif" width="100" height="93" hspace="3" vspace="3" border="1" alt=""/>
                                                </a>
                                            </div>
                                        </td>
                                        <td valign="top">
                                            <span class="docText"><strong><a href="AktenInt.aspx">Interventionen</a></strong></span><br />
                                            <span class="smallText">Hier &uuml;berpr&uuml;fen Sie den aktuellen Status Ihrer Auftr&auml;ge.<br />
                                                Ausserdem haben Sie die M&ouml;glichkeit mit unseren SachbearbeiterInnen Kontakt aufzunehmen und abgeschlossene Auftr&auml;ge in Ihr System zu &uuml;bernehmen.
                                            </span>
                                        </td>
                                    </tr>


                                    <tr>
                                        <td width="120" valign="top">
                                            <div align="center">
                                                <a href="CollectedByAussendienst.aspx">
                                                    <img src="../../intranet/images/sc_Transfer.gif" width="100" height="93" hspace="3" vspace="3" border="1" alt=""/>
                                                </a>
                                            </div>
                                        </td>
                                        <td valign="top">
                                            <span class="docText"><strong><a href="CollectedByAussendienst.aspx">Einnahmen</a></strong></span><br />
                                            <span class="smallText">Eine Aufstellung der Einnahmen.<br />

                                            </span>
                                        </td>
                                    </tr>



                                    <tr>
                                        <td>
                                            <div align="center">
                                                <a href="InteverntionStatistic.aspx">
                                                    <img src="../../intranet/images/sc_Statistik.gif" width="100" height="93" hspace="3" vspace="3" border="1" alt=""/>
                                                </a>
                                            </div>
                                        </td>
                                        <td valign="top">
                                            <span class="docText"><strong><a href="InteverntionStatistic.aspx">Interventionsstatistiken</a> </strong>
                                                <br>
                                                <span class="smallText">Hier sehen Sie Auftragsstatistiken, Verlaufsgrafiken sowie Zeitstatistiken. </span></span>
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>
                                            <div align="center">
                                                <a href="UbergabeneAktenInt.aspx">
                                                    <img src="../../intranet/images/ug_Statistik.gif" width="100" height="93" hspace="3" vspace="3" border="1" alt=""/>
                                                </a>
                                            </div>
                                        </td>
                                        <td valign="top">
                                            <span class="docText"><strong><a href="UbergabeneAktenInt.aspx">&Uuml;bergabestatistik</a> </strong>
                                                <br>
                                                <span class="smallText">Hier sehen Sie Ihre &Uuml;bergabestatistik. </span></span>
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>

                                    <tr background="../../intranet/images/hdbackblue.gif">
                                        <td height="22" colspan="3" class="BoxHeaderBlue">
                                            <strong class="smallText">ALLGEMEINES</strong>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <div align="center">
                                                <a href="../../intranet/intranet/mysettings.asp">
                                                    <img src="../../intranet/images/sc_Settings.gif" width="100" height="75" hspace="3" vspace="3" border="1" alt="" />
                                                </a>
                                            </div>
                                        </td>
                                        <td valign="top">
                                            <span class="docText"><strong><a href="../../intranet/intranet/mysettings.asp">Ihre Einstellungen </a></strong>
                                                <br />
                                                <span class="smallText">Hier warten Sie Ihre pers&ouml;nlichen Daten, &auml;ndern Ihr Passwort und konfigurieren Ihre Anzeigeoptionen. </span></span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </asp:Panel>
    </form>
    <ctl:footer ID="ftr" runat="server" />
</body>
</html>
