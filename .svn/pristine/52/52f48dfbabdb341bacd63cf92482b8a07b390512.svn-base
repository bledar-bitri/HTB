<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BingRouteAddressFix.aspx.cs" Inherits="HTB.v2.intranetx.routeplanter.bingmaps.BingRouteAddressFix" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>HTB.ASP [ Address Repariern ]</title>
    <link href="/v2/intranet/styles/inksys.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        body
        {
            margin-left: 5px;
            margin-top: 5px;
            margin-right: 5px;
            margin-bottom: 5px;
            background-image: url(../images/osxback.gif);
        }
    </style>
    <script src="/v2/intranet/globalcode/lib.js" type="text/javascript" language="javascript"></script>
</head>
<body>
    <ctl:header runat="server" />
    <form id="form1" runat="server">
    <table width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
        <tr>
            <td class="tblDataAll">
                <table border="0" align="center" cellpadding="3" cellspacing="0">
                    <tr>
                        <td colspan="2" class="tblHeader" title="Schuldneraddressen">
                            Schuldneraddressen
                        </td>
                    </tr>
                    <tr>
                    <td colspan="2" class="dataTableAll">
                        <div class="smallText" style="color:Red">Folgende Adressen sind nicht gefunden.<br/>Bitte geben Sie die richtige Adresse ein.</div>
                    </td>
                    </tr>
                    <tr>
                        <td class="tblDataAll">
                            <asp:GridView ID="gvAddresses" runat="server" AutoGenerateColumns="False" CellPadding="2" BorderStyle="Groove" Width="100%">
                                <Columns>
                                    <asp:TemplateField HeaderText="Akt"> 
                                        <ItemTemplate>
                                            <asp:Label id="lblId" runat="server" Text='<%# Eval("Akt")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Adresse" DataField="Address" SortExpression="Address"/>
                                    <asp:BoundField HeaderText="Schuldner" DataField="Gegner" SortExpression="Gegner" HtmlEncode="false"/>
                                    <asp:TemplateField HeaderText="Alternative Adresse">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtReplacementAddress" runat="server" Width="280px" onFocus="this.style.backgroundColor='#DFF4FF'" onBlur="this.style.backgroundColor=''" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td class="EditData">
                            <asp:Button ID="btnSubmit" runat="server" class="btnSave" Text="Neue Route berechnen" OnClick="Submit_Click" />
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
