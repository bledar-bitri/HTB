<%@ Page Language="C#" Culture="de-DE" AutoEventWireup="true" CodeBehind="ShowAktInk.aspx.cs" Inherits="HTB.v2.intranetx.customer.ShowAktInk" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>HTB.ASP [ Inkassoakt ]</title>
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
        .style1
        {
            color: #FF0000;
        }
        .style3
        {
            color: #999999;
        }
        OPTION.dis
        {
            background-color: white;
            color: #999999;
        }
    </style>
    <script src="../../intranet/globalcode/lib.js" type="text/javascript" language="javascript"></script>
    <script src="/v2/intranetx/scripts/jquery-1.3.2.js" type="text/javascript"></script>
    <script src="/v2/intranetx/scripts/jquery.MultiFile.js" type="text/javascript"></script>
</head>
<body id="bdy" runat="server">
    <ctl:headerNoMenu ID="header" runat="server" />
    <table width="100%" border="0" cellpadding="1" cellspacing="0" bgcolor="#000000">
        <tr>
            <td>
                <table width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
                    <tr>
                        <td class="smallText">
                            <asp:HyperLink runat="server" NavigateUrl="Customer.aspx">Portal</asp:HyperLink> | <a href="AktenInk.aspx">Inkassosakten</a> | Akt | <asp:HyperLink runat="server" NavigateUrl="javascript:window.print()">Drucken</asp:HyperLink>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <form runat="server" id="form2" method="post" action="">
    <table width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
        <tr>
            <td>
                <p>
                    &nbsp;</p>
                <table width="632" border="0" align="center" cellpadding="3" cellspacing="0">
                    <tr>
                        <td colspan="2" class="tblHeader">
                            INKASSOAKT
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" >
                            <ctl:message ID="ctlMessage" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td width="10%" valign="top" class="EditCaptionTopLine">
                            <div align="right">
                                <strong>Akt Nr.&nbsp;</strong>:</div>
                        </td>
                        <td class="EditDataTopLine">
                            <asp:Label ID="lblAktNumber" runat="server" />
                            <br />
                            <span class="style3">
                                <asp:Label ID="lblGothiaNr" runat="server" />
                            </span>
                        </td>
                    </tr>
                    <tr>
                        <td width="10%" class="EditCaption">
                            <div align="right">
                                Akt Anlagedatum:&nbsp;</div>
                        </td>
                        <td class="EditData">
                            <asp:Label ID="lblAktEnteredDate" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td width="10%" valign="top" class="EditCaption">
                            <div align="right">
                                Klient:&nbsp;</div>
                        </td>
                        <td class="EditData">
                            <asp:Label ID="lblKlient" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td width="10%" valign="top" class="EditCaption">
                            <div align="right">
                                Gegner:&nbsp;</div>
                        </td>
                        <td class="EditData">
                            <p>
                                <asp:Label ID="lblGegner" runat="server" /></p>
                        </td>
                    </tr>
                    <tr>
                        <td width="10%" class="EditCaption">
                            <div align="right">
                                <strong>Rechnungsnummer</strong>:&nbsp;</div>
                        </td>
                        <td class="EditData">
                            <asp:Label ID="lblCustInkAktKunde" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="EditCaption">
                            <div align="right">
                                <strong>Rechnungsdatum</strong>:&nbsp;</div>
                        </td>
                        <td class="EditData">
                            <asp:Label ID="lblInvoiceDate" runat="server" />
                        </td>
                    </tr>
                    <tr runat="server" id="trClientSB">
                        <td align="right" class="EditCaption">
                            Sachbearbeiter:
                        </td>
                        <td class="EditData">
                            <span class="docText">
                                <asp:DropDownList runat="server" ID="ddlClientSB"/>
                            </span>
                        </td>
                    </tr>
                </table>
                <br />
                <table width="632" border="0" align="center" cellpadding="3" cellspacing="0">
                    <tr>
                        <td colspan="4">
                            <img name="" src="" width="1" height="1" alt="" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" class="EditCaptionTopLine3">
                            <table width="100%" border="1" cellpadding="3" cellspacing="0">
                                <tr>
                                    <td class="tblHeader" title="Forderungsaufstellung">
                                        Forderungsaufstellung
                                    </td>
                                </tr>
                                <tr>
                                    <td title="Betrag">
                                        <table width="100%" border="0" cellspacing="0" cellpadding="3" id="BUebersicht">
                                            <tr>
                                                <td colspan="5" class="tblCol1Sel">
                                                    <div align="right">
                                                        Betr&auml;ge</div>
                                                </td>
                                                <td class="tblCol3Sel">
                                                    <div align="right">
                                                        Zahlungen (inkl. DZ,RT)</div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="20" class="tblData1">
                                                    &nbsp;
                                                </td>
                                                <td width="20" class="tblData2">
                                                    <strong>Forderung</strong> (Klient)
                                                </td>
                                                <td class="tblData2">
                                                    <div align="center">
                                                        EUR</div>
                                                </td>
                                                <td class="tblData2">
                                                    &nbsp;
                                                </td>
                                                <td class="tblData2">
                                                    <div align="right">
                                                        <asp:Label ID="lblForderung" runat="server" /></div>
                                                </td>
                                                <td class="tblData3">
                                                    <div align="right">
                                                        &nbsp;<asp:Label ID="lblZahlungen" runat="server" /></div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tblData1">
                                                    &nbsp;
                                                </td>
                                                <td class="tblData2">
                                                    <strong>Mahnspesen</strong> (Klient)
                                                </td>
                                                <td class="tblData2">
                                                    <div align="center">
                                                        EUR</div>
                                                </td>
                                                <td class="tblData2">
                                                    &nbsp;
                                                </td>
                                                <td class="tblData2">
                                                    <div align="right">
                                                        <asp:Label ID="lblKostenKlientSumme" runat="server" /></div>
                                                </td>
                                                <td class="tblData3">
                                                    <div align="right">
                                                        &nbsp;<asp:Label ID="lblKostenKlientZahlungen" runat="server" /></div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6" class="tblData1">
                                                    <img name="" src="" width="1" height="1" alt="">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6">
                                                    <div align="right">
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <img name="" src="" width="1" height="1" alt="">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" class="EditCaptionTopLine3">
                            <table width="100%" border="1" cellpadding="3" cellspacing="0">
                                <tr>
                                    <td class="tblHeader" title="Bereits &uuml;berwiesen">
                                        Bereits / &Uuml;berwiesen
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table width="100%" border="0" cellspacing="0" cellpadding="3" id="Buchung">
                                            <tr>
                                                <td class="tblData1">
                                                    <asp:GridView ID="gvInvoices" runat="server" AutoGenerateColumns="False" CellPadding="2" BorderStyle="Groove" Width="100%" ShowFooter="True">
                                                        <Columns>
                                                            <asp:BoundField HeaderText="Nr." DataField="InvoiceID" SortExpression="InvoiceID" ItemStyle-HorizontalAlign="Center" />
                                                            <asp:BoundField HeaderText="Überweisen<br/>am" DataField="TransferDate" SortExpression="TransferDate" HtmlEncode="False">
                                                                <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="Überweisen<br/>Betrag">
                                                                <ItemTemplate>
                                                                    <%# Eval("TransferAmount")%>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <%# HTBUtilities.HTBUtils.FormatCurrency(GetTotalTransferred())%>
                                                                </FooterTemplate>
                                                                <FooterStyle Font-Bold="True" HorizontalAlign="Right" Wrap="False" />
                                                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                                            </asp:TemplateField>
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
                    <tr>
                        <td colspan="4">
                            <img name="" src="" width="1" height="1" alt="" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" class="EditCaptionTopLine3">
                            <table width="100%" border="1" cellpadding="3" cellspacing="0">
                                <tr>
                                    <td class="tblHeader" title="Bearbeitungsverlauf">
                                        Bearbeitungsverlauf
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tblData2" align="center" valign="middle" colspan="2">
                                        <asp:GridView ID="gvActions" DataSourceID="ObjectDataSource1" runat="server" AutoGenerateColumns="False" CellPadding="2" CellSpacing="1" BorderStyle="Inset" Width="100%">
                                            <RowStyle BackColor="#EFF3FB" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Datum">
                                                    <ItemTemplate>
                                                        <%# ((DateTime)Eval("ActionDate")).ToShortDateString()%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Aktion" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <div class="gridData">
                                                            <%# Eval("ActionCaption")%></div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Memo" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <div class="gridData">
                                                            <%# ((string)Eval("ActionMemo")).Replace(Environment.NewLine, "<br/>") %></div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                            <HeaderStyle CssClass="tblCol1" Font-Bold="True" ForeColor="#000000" />
                                            <AlternatingRowStyle BackColor="White" />
                                        </asp:GridView>
                                        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetActions" TypeName="HTB.v2.intranetx.customer.ShowAktInk" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <img name="" src="" width="1" height="1" alt="">
                        </td>
                    </tr>
                    <tr>
                        <td class="EditCaptionTopLine3" colspan="4">
                            <table border="1" cellpadding="3" cellspacing="0" width="100%">
                                <tr>
                                    <td title="Dokumente">
                                        <table id="doks" border="0" cellpadding="3" cellspacing="0" width="100%">
                                            <tr>
                                               <td class="tblHeader" title="Bearbeitungsverlauf">
                                                    Dokumente
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tblData1">
                                                    <asp:GridView ID="gvDocs" runat="server" AutoGenerateColumns="False" BorderStyle="Groove" CellPadding="2" ShowFooter="True" Width="100%">
                                                        <Columns>
                                                            <asp:BoundField DataField="DokChangeDate" HeaderText="Erfasst" SortExpression="DokChangeDate">
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="DokTypeCaption" HeaderText="Typ" SortExpression="DokTypeCaption">
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="DokCaption" HeaderText="Bezeichnung" SortExpression="DokCaption">
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="Datei">
                                                                <ItemTemplate>
                                                                    <%# Eval("DokAttachment") %>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <div align="right">
                                                        <asp:FileUpload ID="FileUpload1" runat="server" class="multi" />
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <ctl:message ID="ctlMessageBottom" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" class="EditCaptionTopLine3">
                            <table width="100%" border="1" cellpadding="3" cellspacing="0">
                                <tr>
                                    <td colspan="4" class="tblHeader">
                                        <div align="right">
                                            <asp:Button ID="btnSave" runat="server" class="btnSave" OnClick="btnSubmit_Click" Text="Speichern" title="Speichern" />
                                            &nbsp;
                                            <asp:Button ID="btnCancel" runat="server" class="btnCancel" title="Abbrechen" Text="Abbrechen" OnClick="btnCancel_Click" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                </table>
            </td>
        </tr>
    </table>
    </form>
    <ctl:footer ID="footer1" runat="server" />
</body>
</html>
