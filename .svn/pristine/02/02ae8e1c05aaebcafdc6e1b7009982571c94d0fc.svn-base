<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MissingBeleg.aspx.cs" Inherits="HTB.v2.intranetx.kassablock.MissingBeleg" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>HTB.ASP [ Fehlende Belege ]</title>
    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
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
        
        OPTION.dis
        {
            background-color: white;
            color: #999999;
        }
    </style>
</head>
<body>
    <ctl:header ID="header" runat="server" />
    <form id="form1" runat="server">
    <table width="100%" border="0" cellpadding="1" cellspacing="0" bgcolor="#000000">
      <tr>
        <td><table width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
            <tr>
              <td class="smallText">
                <a href="../../intranet/intranet/intranet.asp">Intranet</a> 
              | <a href="../../intranet/intranet/mydata.asp">Meine Daten</a> 
              | <a href="../../intranet/kassablock/kassablock.asp"> Kassabl&ouml;cke </a>
              | Fehlende Belege
              </td>
            </tr>
        </table></td>
      </tr>
    </table>

    <table width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
        <tr>
            <td>
                <table border="0" align="center" cellpadding="3" cellspacing="0">
                    <tr>
                        <td class="tblHeader">
                            FEHLENDE BELEGE
                        </td>
                    </tr>
                    <tr>
                        <td class="tblDataAll">
                            <ctl:message ID="ctlMessage" runat="server"/>
                        </td>
                    </tr>
                    <tr>
                        <td class="tblDataAll">
                            <asp:GridView ID="gvBelege" runat="server" 
                                AllowSorting="false" AutoGenerateColumns="False" CellPadding="2" 
                                CellSpacing="2" BorderStyle="Inset" Width="100%" AlternatingRowStyle-BackColor="AntiqueWhite" 
                                EmptyDataText="Keine fehlende Belege!" >
                                <RowStyle />
                                <Columns>
                                    <asp:TemplateField HeaderText="UserID" ItemStyle-HorizontalAlign="Center" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMissingUserID" runat="server" Text='<%# Eval("KbMissUser")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Benutzer" DataField="User" SortExpression="User" ItemStyle-HorizontalAlign="Center" />
                                    <asp:TemplateField HeaderText="Beleg Nr."  ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMissingBelegNr" runat="server" Text='<%# Eval("KbMissNr")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Datum" DataField="KbMissDate" SortExpression="KbMissDate" ItemStyle-HorizontalAlign="Center" />
                                    <asp:TemplateField HeaderText="Empfangen">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkReceived" runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td class="EditData" align="center">
                            <asp:Button ID="btnSubmit" runat="server" class="btnSave" Text="Speichern" OnClick="Submit_Click" />&nbsp;&nbsp;&nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
    <ctl:footer ID="ftr" runat="server" />
</body>
</html>
