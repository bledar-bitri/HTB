<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Zwischenbericht.aspx.cs" Inherits="HTB.v2.intranetx.global_forms.Zwischenbericht" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <title>HTB.ASP [ Zwischenbericht ]</title>
    <link href="../../intranet/styles/inksys.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        body
        {
            margin-left: 5px;
            margin-top: 5px;
            margin-right: 5px;
            margin-bottom: 5px;
            background-image: url(../../intranet/images/osxback.gif);
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
        
        .modalBackground
        {
            background-color: #CCCCCC;
            filter: alpha(opacity=40);
            opacity: 0.5;
        }
        
        .ModalWindow
        {
            border: solid1px#c0c0c0;
            background: #f0f0f0;
            padding: 0px10px10px10px;
            position: absolute;
            top: -1000px;
        }
    </style>
</head>
<body>
    <ctl:headerNoMenu ID="hdr" runat="server" />
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
                                    <td colspan="2" class="tblHeader" title="ZWISCHENBERICHT DRUCKEN">
                                        ZWISCHENBERICHT DRUCKEN
                                    </td>
                                </tr>
                                <tr>
                                    <td class="EditCaption">
                                        <div align="right">Akt Nr.:</div>
                                    </td>
                                    <td class="EditData">
                                        <asp:Label ID="lblAktNr" runat="server" Enabled="false" class="docText" />
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" class="EditCaption">
                                        <div align="right">AZ:</div>
                                    </td>
                                    <td class="EditData">
                                        <asp:Label ID="lblAZ" runat="server" class="docText" />
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" class="EditCaption">
                                        <div align="right">Auftraggeber:</div>
                                    </td>
                                    <td class="EditData">
                                        <asp:Label ID="lblAuftraggeber" runat="server" class="docText" />
                                        <br />
                                        <asp:Label ID="lblAuftraggeberName" runat="server" class="docText" />
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" class="EditCaption">
                                        <div align="right">Klient:</div>
                                    </td>
                                    <td class="EditData">
                                        <asp:Label ID="lblKlient" runat="server" class="docText" />
                                        <br />
                                        <asp:Label ID="lblKlientName" runat="server" class="docText" />
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" class="EditCaption">
                                        <div align="right">Gegner:</div>
                                    </td>
                                    <td class="EditData">
                                        <asp:Label ID="lblGegner" runat="server" class="docText" />
                                        <br/>
                                        <asp:Label ID="lblGegnerName" runat="server" class="docText" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tblCol1"  colspan="2">
                                        <asp:GridView ID="gvActions" DataSourceID="ObjectDataSource1" runat="server" AutoGenerateColumns="False"
                                            CellPadding="2" CellSpacing="1" BorderStyle="Inset" Width="100%" >
                                            <RowStyle BackColor="#EFF3FB" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblActionID" runat="server" Text='<%# Eval("ActionID")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Druck">
                                                    <ItemTemplate>
                                                        <div class="gridData">
                                                            <asp:CheckBox ID="chkSelected" runat="server" Checked="true" />
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Datum" >
                                                    <ItemTemplate>  
                                                        <%# ((DateTime)Eval("ActionDate")).ToShortDateString()%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Aktion">
                                                    <ItemTemplate>
                                                        <div class="gridData"><%# Eval("ActionCaption")%></div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Memo">
                                                    <ItemTemplate>
                                                        <div class="gridData">
                                                            <%# ((string)Eval("ActionMemo")).Replace(Environment.NewLine, "<br/>") %></div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                            <HeaderStyle CssClass="tblCol1"  Font-Bold="True" ForeColor="#000000"/>
                                            <AlternatingRowStyle BackColor="White" />
                                        </asp:GridView>
                                        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetActions" TypeName="HTB.v2.intranetx.global_forms.Zwischenbericht"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" class="EditCaption">
                                        <div align="right">Beschreibung:</div>
                                    </td>
                                    <td class="EditData">
                                        <asp:TextBox ID="txtMemo" runat="server" class="docText" Columns="80" onblur="this.style.backgroundColor='';" onfocus="this.style.backgroundColor='#DFF4FF';" Rows="8"
                                            TextMode="MultiLine" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <img name="" src="" width="1" height="1" alt="">
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" class="EditCaptionTopLine3">
                                        <table width="100%" border="1" align="center" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
                                            <tr>
                                                <td class="tblFooter1">
                                                    <div align="right">
                                                        <asp:Button ID="btnSubmit" runat="server" class="btnSave" OnClick="btnSubmit_Click" Text="Drucken" title="Drucken" />
                                                        <input type="button" id="btnCancel" class="btnCancel" title="Abbrechen" value="Abbrechen" onclick="window.close();" />
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <p>
                                &nbsp;</p>
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
