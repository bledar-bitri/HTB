<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OverdueList.aspx.cs" Inherits="HTB.v2.intranetx.aktenint.OverdueList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>HTB.ASP [ &Uuml;berf&auml;llige Akte]</title>
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
        .style2
        {
            color: #FF0000;
            font-weight: bold;
        }
        
        OPTION.dis
        {
            background-color: white;
            color: #999999;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <table width="100%" border="0" cellpadding="1" cellspacing="0" bgcolor="#000000">
        <tr>
            <td>
                <table width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
                    <tr>
                        <td class="smallText">
                            <a href="../../intranet/intranet.asp">Intranet</a> | <a href="../../intranet/mydata.asp">Meine Daten</a> | <a href="../../intranet/akten.asp">Akten</a> | <a href="../aktenint/aktenint.asp">
                                Interventionsakte</a> | &Uuml;berf&auml;llige Akte
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table width="100%" border="0" cellspacing="0" cellpadding="1">
        <tr>
            <td bgcolor="#000000">
                <table width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
                    <tr>
                        <td>
                            <table width="100%" border="0" cellspacing="0" cellpadding="1">
                                <tr>
                                    <td class="tblFunctionBar">
                                        <table width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
                                            <tr>
                                                <td colspan="2">
                                                    <span class="style2">Liste &uuml;berf&auml;lliger Akte f&uuml;r Interventionen generieren </span>
                                                </td>
                                                <td width="442">
                                                    <div align="right">
                                                        <input name="Submit5" type="button" class="smallText" title="Zur&uuml;ck zum Men&uuml;." onclick="MM_goToURL('parent','aktenint.asp');return document.MM_returnValue"
                                                            value="Zur&uuml;ck">
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3" valign="middle">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3" valign="middle">
                                                    <table width="100%" border="0" cellspacing="0" cellpadding="3">
                                                        <tr>
                                                            <td width="200" valign="top" class="docText">
                                                                <div align="right" class="smallText">
                                                                    Sachbearbeiter :&nbsp;</div>
                                                            </td>
                                                            <td>
                                                                <span class="docText">
                                                                    <asp:Listbox ID="lbUsers" runat="server" class="smallText" />
                                                                </span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top" class="smallText">
                                                                <div align="right">
                                                                    Wiedervorlage:&nbsp;
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <select name="lstWiedervorlage" class="smallText" id="lstWiedervorlage">
                                                                    <option value="999" selected>*** Alle ***</option>
                                                                    <option value="1">Ja</option>
                                                                    <option value="0">Nein</option>
                                                                </select>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3" valign="middle">
                                                    <hr>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" valign="middle">
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <div align="right">
                                                        <input name="Submit" type="submit" class="smallText" title=".. weiter .." value="Weiter &gt;&gt;">
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
</body>
</html>
