<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginAndSearch.aspx.cs" Inherits="HTB.v2.intranetx.search.LoginAndSearch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server" />
<title>HTB.ASP [ Intervention buchen ]</title>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
<link href="../../intranet/styles/htbTablet.css" rel="stylesheet" type="text/css" />
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
<script language="JavaScript" type="text/JavaScript">
        function MM_openBrWindow(theURL,winName,features) { //v2.0
          window.open(theURL,winName,features);
        }

        function MM_goToURL() { //v3.0
          var i, args=MM_goToURL.arguments; document.MM_returnValue = false;
          for (i=0; i<(args.length-1); i+=2) eval(args[i]+".location='"+args[i+1]+"'");
        }

        function InsertTimeStamp(){	
          document.forms(0).txtMemo.focus();  
          document.forms(0).txtMemo.value = document.form1.txtMemo.value + "\n\n" + "[" + "<%= Session["MM_VorName"] %>" + " " + "<%= Session["MM_NachName"] %>" + " " + "<%= DateTime.Now %>"+ "]" + "\n";
          document.forms(0).txtMemo.focus();	
        }
</script>
</head>
<body>
    <form id="form1" runat="server">
    <ctl:header ID="hdr" runat="server" />
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" EnablePartialRendering="true" EnableScriptLocalization="true" />
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        function BeginRequestHandler(sender, args) {
            document.getElementById('<%= btnSubmit.ClientID %>').innerText = "Processing";
            document.getElementById('<%= btnSubmit.ClientID %>').disabled = true;
            document.getElementById('<%= btnCancel.ClientID %>').innerText = "Processing";
            document.getElementById('<%= btnCancel.ClientID %>').disabled = true;
        }
    </script>
    <table width="100%" border="0" cellpadding="1" cellspacing="0" bgcolor="#000000">
        <tr>
            <td>
                <table width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
                    <tr>
                        <td class="smallText">
                            <a href="../../intranet/intranet/intranet.asp">Intranet</a> | <a href="../../intranet/intranet/inkasso.asp">CollectionInvoice</a> | <a href="../../intranet/aktenint/aktenint.asp">
                                Interventionsakte (&Uuml;bersicht)</a> | Interventionsakt editieren
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <br />
    <asp:UpdatePanel ID="updPanel1" runat="server">
        <ContentTemplate>
            <table width="100%" border="0" cellspacing="0" cellpadding="1">
                <tr>
                    <td bgcolor="#000000">
                        <table width="100%" border="0" cellpadding="" cellspacing="0" bgcolor="#FFFFFF">
                            <tr>
                                <td colspan="2" class="tblHeader2" title="SUCHEN">
                                    SUCHEN
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table cellpadding="1" cellspacing="0" bgcolor="#000000">
                                        <tr>
                                            <td>
                                                <table bgcolor="#86dee7">
                                                    <tr>
                                                        <td>
                                                            <div align="right">
                                                                Zahl:</div>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtAkt" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <div align="right">
                                                                Name:</div>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtName" runat="server" Width="400" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <div align="right">Telefon:</div>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtPhone" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <div align="right">
                                                                <asp:Button ID="btnSubmit" runat="server" class="btnSave" title="Suchen" Text="Suchen" OnClick="btnSubmit_Click" />
                                                                <asp:Button ID="btnCancel" runat="server" class="btnCancel" title="Abbrechen" Text="Abbrechen" OnClick="btnCancel_Click" />
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
                                <td colspan="2" class="tblDataAll">
                                    <ctl:message ID="ctlMessage" runat="server" />
                                </td>
                            </tr>
                            <tr id="trHeaderGegner" runat="server">
                                <td colspan="2" class="tableRowHeader2">
                                    Schuldner:
                                    <asp:Label runat="server" ID="lblGegnerCount" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Button runat="server" ID="cmdPreviousGegner" text="<<" OnClick="cmdPreviousGegner_Click"/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Button runat="server" ID="cmdNextGegner" text=">>" OnClick="cmdNextGegner_Click"/>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:GridView ID="gvGegner" runat="server" AutoGenerateColumns="False" BorderStyle="Groove" CellPadding="2" Width="100%" SkinID="Professional" Font-Name="Verdana"
                                        Font-Size="10pt" HeaderStyle-BackColor="#444444" HeaderStyle-ForeColor="White" AlternatingRowStyle-BackColor="#dddddd" OnRowCreated="gvGegner_RowCreated">
                                        <Columns>
                                            <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" Visible="false" />
                                            <asp:BoundField DataField="OldID" HeaderText="OldID" SortExpression="OldID" Visible="false" />
                                            <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name">
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Strasse" HeaderText="Strasse" SortExpression="Strasse">
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Ort" HeaderText="Ort" SortExpression="Ort">
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="DOB" HeaderText="Geb. Dat." SortExpression="DOB">
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Phone" HeaderText="Telefon" SortExpression="Phone" HtmlEncode="false">
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="InterventionAkte" HeaderText="Int. Akte" SortExpression="InterventionAkte">
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CollectionInvoiceAkte" HeaderText="CollectionInvoice Akte" SortExpression="CollectionInvoiceAkte">
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CollectionInvoiceBalance" HeaderText="CollectionInvoice Saldo" SortExpression="CollectionInvoiceBalance">
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:BoundField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr id="tr1" runat="server">
                                <td colspan="2">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr id="trHeaderKlient" runat="server">
                                <td colspan="2" class="tableRowHeader2">
                                    Klienten:
                                    <asp:Label runat="server" ID="lblClientCount" />
                                     &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Button runat="server" ID="cmdPreviousClient" text="<<" OnClick="cmdPreviousClient_Click"/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Button runat="server" ID="cmdNextClient" text=">>" OnClick="cmdNextClient_Click"/>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:GridView ID="gvKlient" runat="server" AutoGenerateColumns="False" BorderStyle="Groove" CellPadding="2" Width="100%" SkinID="Professional" Font-Name="Verdana"
                                        Font-Size="10pt" HeaderStyle-BackColor="#444444" HeaderStyle-ForeColor="White" AlternatingRowStyle-BackColor="#dddddd" OnRowCreated="gvKlient_RowCreated">
                                        <Columns>
                                            <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" Visible="false" />
                                            <asp:BoundField DataField="OldID" HeaderText="OldID" SortExpression="OldID" Visible="false" />
                                            <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name">
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Strasse" HeaderText="Strasse" SortExpression="Strasse">
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Ort" HeaderText="Ort" SortExpression="Ort">
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Phone" HeaderText="Telefon" SortExpression="Phone" HtmlEncode="false">
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="InterventionAkte" HeaderText="Int. Akte">
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CollectionInvoiceAkte" HeaderText="CollectionInvoice Akte">
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CollectionInvoiceBalance" HeaderText="CollectionInvoice Saldo" HtmlEncode="false">
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:BoundField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr id="tr2" runat="server">
                                <td colspan="2">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr id="trHeaderAg" runat="server">
                                <td colspan="2" class="tableRowHeader2">
                                    Auftraggeber:<asp:Label runat="server" ID="lblAgCount" />
                                    <asp:Button runat="server" ID="cmdPreviousAg" text="<<" OnClick="cmdPreviousAg_Click"/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Button runat="server" ID="cmdNextAg" text=">>" OnClick="cmdNextAg_Click"/>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:GridView ID="gvAg" runat="server" AutoGenerateColumns="False" BorderStyle="Groove" CellPadding="2" Width="100%" SkinID="Professional" Font-Name="Verdana"
                                        Font-Size="10pt" HeaderStyle-BackColor="#444444" HeaderStyle-ForeColor="White" AlternatingRowStyle-BackColor="#dddddd" OnRowCreated="gvAG_RowCreated">
                                        <Columns>
                                            <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" Visible="false" />
                                            <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name">
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Strasse" HeaderText="Strasse" SortExpression="Strasse">
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Ort" HeaderText="Ort" SortExpression="Ort">
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Phone" HeaderText="Telefon" SortExpression="Phone" HtmlEncode="false">
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="InterventionAkte" HeaderText="Int. Akte">
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CollectionInvoiceAkte" HeaderText="CollectionInvoice Akte">
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CollectionInvoiceBalance" HeaderText="CollectionInvoice Saldo" HtmlEncode="false">
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:BoundField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>

                            <tr>
                                <td colspan="2" width="100%">
                                    <ajax:TabContainer ID="tabContainer1" runat="server" ActiveTabIndex="0" Width="100%">
                                        <ajax:TabPanel ID="tabMain" runat="server" HeaderText="Akten">
                                            <ContentTemplate>
                                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                    <ContentTemplate>
                                                        <table width="100%">
                                                            <tr id="trHeaderInkasso" runat="server">
                                                                <td class="tableRowHeader">
                                                                    CollectionInvoiceakte
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:GridView ID="gvInkasso" runat="server" AutoGenerateColumns="False" 
                                                                    BorderStyle="Groove" CellPadding="2" Width="100%" SkinID="Professional" Font-Name="Verdana"
                                                                        Font-Size="10pt" HeaderStyle-BackColor="#444444" HeaderStyle-ForeColor="White" 
                                                                        AlternatingRowStyle-BackColor="#dddddd" OnRowCreated="gvInkasso_RowCreated" ShowFooter="true">
                                                                        <Columns>
                                                                            <asp:BoundField DataField="ID" HeaderText="ID" />
                                                                            <asp:BoundField DataField="AZ" HeaderText="Rechnung Nr." />
                                                                            <asp:BoundField DataField="AktEnteredDate" HeaderText="Erf. Datum">
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="KlientName" HeaderText="Klient">
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="GegnerInfo" HeaderText="Schuldner" HtmlEncode="false">
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="AktStatus" HeaderText="Status" HtmlEncode="false">
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                            </asp:BoundField>
                                                                            <asp:TemplateField HeaderText="Forderung">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblTotalForderung" runat="server" Text='<%# Eval("Forderung")%>' />
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                                <FooterTemplate>
                                                                                    <strong>
                                                                                        <%= GetTotalInkassoForderung()%>
                                                                                    </strong>
                                                                                </FooterTemplate>
                                                                                <FooterStyle CssClass="tblDataAll" HorizontalAlign="Right" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Forderung<br/>Inkl. CollectionInvoicekosten">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblTotalCharges" runat="server" Text='<%# Eval("TotalCharges")%>' />
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                                <FooterTemplate>
                                                                                    <strong>
                                                                                        <%= GetTotalInkassoCharges()%>
                                                                                    </strong>
                                                                                </FooterTemplate>
                                                                                <FooterStyle CssClass="tblDataAll" HorizontalAlign="Right" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Bezahlt">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblPaid" runat="server" Text='<%# Eval("TotalPaid")%>' />
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                                <FooterTemplate>
                                                                                    <strong>
                                                                                        <%= GetTotalInkassoPaid()%>
                                                                                    </strong>
                                                                                </FooterTemplate>
                                                                                <FooterStyle CssClass="tblDataAll" HorizontalAlign="Right" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Saldo">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblBalance" runat="server" Text='<%# Eval("Balance")%>' />
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                                <FooterTemplate>
                                                                                    <strong>
                                                                                        <%= GetTotalInkassoBalance() %>
                                                                                    </strong>
                                                                                </FooterTemplate>
                                                                                <FooterStyle CssClass="tblDataAll" HorizontalAlign="Right" />
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </td>
                                                            </tr>
                                                            <tr id="trHeaderIntervention" runat="server">
                                                                <td colspan="2" class="tableRowHeader">
                                                                    Interventionsakte
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <asp:GridView ID="gvIntervention" runat="server" AutoGenerateColumns="False" BorderStyle="Groove" CellPadding="2" Width="100%" SkinID="Professional" Font-Name="Verdana"
                                                                        Font-Size="10pt" 
                                                                        HeaderStyle-BackColor="#444444" 
                                                                        HeaderStyle-ForeColor="White" 
                                                                        AlternatingRowStyle-BackColor="#dddddd" 
                                                                        OnRowCreated="gvIntervention_RowCreated"
                                                                        ShowFooter="true">
                                                                        <Columns>
                                                                            <asp:BoundField DataField="ID" HeaderText="ID" />
                                                                            <asp:BoundField DataField="AZ" HeaderText="AZ" />
                                                                            <asp:BoundField DataField="AktEnteredDate" HeaderText="Erf. Datum">
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="KlientName" HeaderText="Klient">
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="GegnerInfo" HeaderText="Schuldner" HtmlEncode="false">
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="AktStatus" HeaderText="Status" HtmlEncode="false">
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="AktCurStatus" HeaderText="L&auml;tzte Aktion" HtmlEncode="false">
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:BoundField>
                                                                            <asp:TemplateField HeaderText="Forderung<br/>Inkl. CollectionInvoicekosten">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblTotalCharges" runat="server" Text='<%# Eval("TotalCharges")%>' />
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                                <FooterTemplate>
                                                                                    <strong>
                                                                                        <%= GetTotalInterventionCharges()%>
                                                                                    </strong>
                                                                                </FooterTemplate>
                                                                                <FooterStyle CssClass="tblDataAll" HorizontalAlign="Right" />
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </ContentTemplate>
                                        </ajax:TabPanel>
                                        <ajax:TabPanel ID="tabAddress" runat="server" HeaderText="Adress(en) / Telefonnummer(n)" Width="100%">
                                            <ContentTemplate>
                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" Width="100%">
                                                    <ContentTemplate>
                                                        <table width="100%">
                                                            <tr>
                                                                <td>
                                                                    <asp:Label runat="server" ID="lblGegnerMemo"/>
                                                                </td>
                                                            </tr>
                                                            <tr id="trHeaderGegnerAddress" runat="server">
                                                                <td colspan="2" class="tableRowHeader">
                                                                    Adressen
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <asp:GridView ID="gvGegnerAddress" runat="server" AutoGenerateColumns="False" BorderStyle="Groove" CellPadding="2" Width="100%" SkinID="Professional" Font-Name="Verdana"
                                                                        Font-Size="10pt" HeaderStyle-BackColor="#444444" HeaderStyle-ForeColor="White" AlternatingRowStyle-BackColor="#dddddd" OnRowCreated="gvGegnerAddress_RowCreated"
                                                                        ShowFooter="true">
                                                                        <Columns>
                                                                            <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name">
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="Date" HeaderText="Datum">
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="Strasse" HeaderText="Strasse" SortExpression="Strasse">
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="Ort" HeaderText="Ort" SortExpression="Ort">
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:BoundField>
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </td>
                                                            </tr>
                                                            <tr id="trHeaderGegnerPhone" runat="server">
                                                                <td colspan="2" class="tableRowHeader">
                                                                    Telefonnummern
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <asp:GridView ID="gvGegnerPhone" runat="server" AutoGenerateColumns="False" BorderStyle="Groove" CellPadding="2" Width="100%" SkinID="Professional" Font-Name="Verdana"
                                                                        Font-Size="10pt" HeaderStyle-BackColor="#444444" HeaderStyle-ForeColor="White" AlternatingRowStyle-BackColor="#dddddd" OnRowCreated="gvGegnerPhone_RowCreated"
                                                                        ShowFooter="true">
                                                                        <Columns>
                                                                            <asp:BoundField DataField="PhoneType" HeaderText="Telefontyp" />
                                                                            <asp:BoundField DataField="PhoneCity" HeaderText="Vorwahl" />
                                                                            <asp:BoundField DataField="Phone" HeaderText="Nummer" />
                                                                            <asp:BoundField DataField="PhoneDate" HeaderText="Datum" />
                                                                            <asp:BoundField DataField="PhoneDescription" HeaderText="Memo" />
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </ContentTemplate>
                                        </ajax:TabPanel>
                                    </ajax:TabContainer>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <asp:HiddenField runat="server" ID="hdnGegnerIdx"/>
            <asp:HiddenField runat="server" ID="hdnClientIdx"/>
            <asp:HiddenField runat="server" ID="hdnAgIdx"/>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
