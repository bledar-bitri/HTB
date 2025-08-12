<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BingRoutePrintRouteAddresses.aspx.cs" Inherits="HTB.v2.intranetx.routeplanner.BingRoutePrintRouteAddresses" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>HTB.ASP [ Reiseroute ]</title>
</head>
<body>
    <ctl:headerNoMenu runat="server"/>
    <form id="form1" runat="server">
    <table width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
        <tr>
            <td class="tblDataAll">
                <table border="0" align="center" cellpadding="3" cellspacing="0">
                    <tr>
                        <td colspan="2" class="tblHeader" title="Reiseroute">
                            Reiseroute
                        </td>
                    </tr>
                    <tr>
                        <td class="tblDataAll">
                            <asp:GridView ID="gvAddresses" runat="server" AutoGenerateColumns="False" CellPadding="2" BorderStyle="Groove" Width="100%">
                                <Columns>
                                    <asp:BoundField HeaderText="" DataField="Index" SortExpression="Index" HtmlEncode="false" >
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>

                                    <asp:TemplateField HeaderText="Akt"> 
                                        <ItemTemplate>
                                            <asp:Label id="lblId" runat="server" Text='<%# Eval("Akt")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Adresse" DataField="Address" SortExpression="Address" HtmlEncode="false" />
                                    <asp:BoundField HeaderText="Schuldner" DataField="Gegner" SortExpression="Gegner" HtmlEncode="false"/>
                                    <asp:BoundField HeaderText="Distance" DataField="Distance" SortExpression="Distance" HtmlEncode="false" >
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Zeit" DataField="Time" SortExpression="Time" HtmlEncode="false">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Distance<br/>bisher" DataField="TotalDistance" SortExpression="TotalDistance" HtmlEncode="false">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Zeit<br/>bisher" DataField="TotalTime" SortExpression="TotalTime" HtmlEncode="false">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Zeit<br/>beispielsweise" DataField="ExampleTime" SortExpression="ExampleTime" HtmlEncode="false">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                </Columns>
                            </asp:GridView>
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
