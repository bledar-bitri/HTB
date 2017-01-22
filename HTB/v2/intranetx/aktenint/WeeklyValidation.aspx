<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WeeklyValidation.aspx.cs" Inherits="HTB.v2.intranetx.aktenint.WeeklyValidation" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<title>HTB.ASP [ Montags Verrechnungskontrolle ]</title>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
<link href="/v2/intranet/styles/inksys.css" rel="stylesheet" type="text/css" />
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
</style>
<body>
    <form id="form1" runat="server">
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
                                    <td colspan="2" class="tblHeader" title="INTERVENTION">
                                        Montags Verrechnungskontrolle
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:GridView ID="gvErrors" runat="server" AllowSorting="false" AutoGenerateColumns="False" CellPadding="2" BorderStyle="Groove" Width="100%">
                                            <Columns>
                                                <asp:BoundField HeaderText="Akt" DataField="Akt" SortExpression="Akt"  />
                                                <asp:BoundField HeaderText="Fehler" DataField="ErrorDescription" SortExpression="ErrorDescription" />
                                            </Columns>
                                        </asp:GridView>
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
