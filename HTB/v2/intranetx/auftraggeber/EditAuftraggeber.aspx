﻿<%@ Page Language="C#"  Culture="de-DE" AutoEventWireup="true" CodeBehind="EditAuftraggeber.aspx.cs" Inherits="HTB.v2.intranetx.auftraggeber.EditAuftraggeber" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><asp:Literal ID="litTitle" runat="server" /> </title>
    <link href="../../intranet/styles/inksys.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        body {
	        margin-left: 5px;
	        margin-top: 5px;
	        margin-right: 5px;
	        margin-bottom: 5px;
	        background-image: url(../../intranet/images/osxback.gif);
        }
        a:link {
	        color: #CC0000;
        }
        a:visited {
	        color: #CC0000;
        }
        a:hover {
	        color: #CC0000;
        }
        a:active {
	        color: #CC0000;
        }
        .style2 {	color: #FF0000;
	        font-weight: bold;
        }
    </style>
</head>
<body>
    <ctl:header ID="hdr" runat="server" />
    <br/>
        <table width="100%" border="0" cellpadding="1" cellspacing="0" bgcolor="#000000">
          <tr>
            <td><table width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
                <tr>
                  <td class="smallText"><a href="../../intranet/intranet/intranet.asp">Intranet</a> | <a href="../../intranet/auftraggeber/auftraggeber.asp">Auftraggeber</a> | <asp:Label ID="lblNavTitle" runat="server" /></td>
                </tr>
            </table></td>
          </tr>
        </table>
     <br/>
    <form id="form1" runat="server">
    <table width="100%" border="0" cellspacing="0" cellpadding="1">
        <tr>
            <td bgcolor="#000000">
                <table width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
                    <tr>
                        <td>
                            <table border="0" align="center" cellpadding="3" cellspacing="0">
                                <tr>
                                    <td colspan="2" class="tblHeader">
                                        <asp:Label ID="lblHeader" runat="server"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <img name="" src="" width="1" height="1" alt=""/>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" class="tblHeader" title="Adressdaten">
                                        Adressdaten
                                    </td>
                                </tr>
                                <tr>
                                    <td class="EditCaption">
                                        <div align="right">
                                            Anrede:</div>
                                    </td>
                                    <td class="EditData">
                                        <asp:TextBox runat="server" ID="txtAnrede" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="50" maxlength="100" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="EditCaption">
                                        <div align="right">Name:</div>
                                    </td>
                                    <td class="EditData">
                                        <asp:TextBox runat="server" ID="txtName1" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="50" maxlength="100" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="EditCaption">
                                        &nbsp;
                                    </td>
                                    <td class="EditData">
                                        <asp:TextBox runat="server" ID="txtName2" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="50" maxlength="100" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="EditCaption">
                                        <div align="right">
                                            &nbsp;&nbsp;</div>
                                    </td>
                                    <td class="EditData">
                                        <asp:TextBox runat="server" ID="txtName3" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="50" maxlength="100" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="EditCaption">
                                        <div align="right">Strasse:</div>
                                    </td>
                                    <td class="EditData">
                                        <asp:TextBox runat="server" ID="txtStrasse" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="50" maxlength="100" />
                                    </td>
                                </tr>
                                <tr>
                                    <td nowrap class="EditCaption">
                                        <div align="right">
                                            LKZ / PLZ / Ort:</div>
                                    </td>
                                    <td class="EditData">
                                        <asp:TextBox runat="server" ID="txtZIPPrefix" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="3" maxlength="3" />
                                        <asp:TextBox runat="server" ID="txtZIP" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="12" maxlength="12" />
                                        <asp:TextBox runat="server" ID="txtOrt" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="50" maxlength="50" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="EditCaption">
                                        <div align="right">
                                            Land:</div>
                                    </td>
                                    <td class="EditData">
                                        <asp:DropDownList ID="ddlState" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <img name="" src="" width="1" height="1" alt="" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" class="tblHeader" title="Kommunikationsdaten">
                                        Kommunikationsdaten
                                    </td>
                                </tr>
                                <tr>
                                    <td class="EditCaption">
                                        <div align="right">Telefon:</div>
                                    </td>
                                    <td class="EditData">
                                        +
                                        <asp:TextBox runat="server" ID="txtPhoneCountry" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="5" maxlength="5" />
                                        (
                                        <asp:TextBox runat="server" ID="txtPhoneCity" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="10" maxlength="10" />
                                        )
                                        <asp:TextBox runat="server" ID="txtPhone" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="30" maxlength="50" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="EditCaption">
                                        <div align="right">Fax:</div>
                                    </td>
                                    <td class="EditData">
                                        +
                                        <asp:TextBox runat="server" ID="txtFaxCountry" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="5" maxlength="5"/>
                                        (
                                        <asp:TextBox runat="server" ID="txtFaxCity" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="10" maxlength="10"/>
                                        )
                                        <asp:TextBox runat="server" ID="txtFax" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="30" maxlength="50"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="EditCaption">
                                        <div align="right">E-Mail:</div>
                                    </td>
                                    <td class="EditData">
                                        <asp:TextBox runat="server" ID="txtemail" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="50" maxlength="100" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="EditCaption">
                                        <div align="right">
                                            Auftragsbest&auml;tigung senden:
                                        </div>
                                    </td>
                                    <td class="EditData">
                                        <asp:CheckBox ID="chksendconfirmation" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="EditCaption">
                                        <div align="right">
                                            Mercedes-Benz Protokoll senden:
                                        </div>
                                    </td>
                                    <td class="EditData">
                                        <asp:CheckBox ID="chkUseMerzedesProtocol" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="EditCaption">
                                        <div align="right">Web:</div>
                                    </td>
                                    <td class="EditData">
                                        <asp:TextBox runat="server" ID="txtweb" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="50" maxlength="100" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="EditCaption">
                                        <div align="right">
                                            Bankverbindung:</div>
                                    </td>
                                    <td class="EditData">
                                        <label>
                                            <asp:DropDownList ID="ddlAGBank" runat="server" />
                                        </label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <img name="" src="" width="1" height="1" alt=""/>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" class="tblHeader" title="Sonstiges">
                                        Intervention Kosten
                                    </td>
                                </tr>
                                <tr>
                                    <td class="EditCaption">
                                        <div align="right">Interventionskosten (Eingang):</div>
                                    </td>
                                    <td class="EditData">
                                        <label>
                                            <asp:TextBox runat="server" ID="txtInterventionsKosten" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="20" maxlength="20" />
                                            &euro;
                                        </label>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" class="EditCaption">
                                        <div align="right"> <br />Akt Typ <br />Interventionskosten:</div>
                                    </td>
                                    <td class="EditData">&nbsp;
                                       <asp:GridView ID="gvKosten" runat="server" AllowSorting="false" AutoGenerateColumns="False"
                                        CellPadding="2" CellSpacing="2" BorderStyle="Inset" Width="100%" AlternatingRowStyle-BackColor="AntiqueWhite">
                                        <RowStyle />
                                        <Columns>
                                            <%--Hidden Column--%>
                                            <asp:TemplateField HeaderText="ID" Visible="false"> 
                                                <ItemTemplate>
                                                    <asp:Label id="lblAktTypeId" runat="server" Text='<%# Eval("AktTypeId")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="Akt Typ" DataField="AktType" SortExpression="AktType" />
                                            <asp:TemplateField HeaderText="Kosten<br/>(Eingang)">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtAmount" runat="server" Width="80px" Text='<%#Eval("Amount") %>' onFocus="this.style.backgroundColor='#DFF4FF'" onBlur="this.style.backgroundColor=''" style="text-align: right"/>&euro;
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Provision Betrag<br/>(Ausendienst)">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtProvAmount" runat="server" Width="80px" Text='<%#Eval("ProvAmount") %>' onFocus="this.style.backgroundColor='#DFF4FF'" onBlur="this.style.backgroundColor=''" style="text-align: right"/>&euro;
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Provision Betrag<br/>Ohne Kassierung<br/>(Ausendienst)">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtProvAmountForZero" runat="server" Width="80px" Text='<%#Eval("ProvAmountForZero") %>' onFocus="this.style.backgroundColor='#DFF4FF'" onBlur="this.style.backgroundColor=''" style="text-align: right"/>&euro;
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Provision Procent<br/>Vom Kassierung<br/>(Ausendienst)">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtProvPct" runat="server" Width="80px" Text='<%#Eval("ProvPct") %>' onFocus="this.style.backgroundColor='#DFF4FF'" onBlur="this.style.backgroundColor=''" style="text-align: right"/>&#37;
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <img name="" src="" width="1" height="1" alt="" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" class="tblHeader" title="Sonstiges">
                                        Sonstiges
                                    </td>
                                </tr>
                                <tr>
                                    <td class="EditCaption">
                                        <div align="right">
                                            Interventionshonorar:</div>
                                    </td>
                                    <td class="EditData">
                                        <asp:TextBox runat="server" ID="txtInterventionshonorar" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="20" maxlength="20" />
                                        &euro;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="EditCaption">
                                        <div align="right">Interventionsprovision:</div>
                                    </td>
                                    <td class="EditData">
                                        <label>
                                            <asp:TextBox runat="server" ID="txtIntProv" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="20" maxlength="20" />
                                            &euro;
                                        </label>
                                    </td>
                                </tr>
                                <%--<tr>
                                    <td class="EditCaption">
                                        <div align="right">Interventionsprovision 2:</div>
                                    </td>
                                    <td class="EditData">
                                        <label>
                                            <asp:TextBox runat="server" ID="txtIntProv2" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="20" maxlength="20" />
                                            &euro;
                                        </label>
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td class="EditCaption">
                                        <div align="right">Hinterlegung f&uuml;r Intervention drucken:
                                        </div>
                                    </td>
                                    <td class="EditData">
                                        <asp:CheckBox ID="chkHl" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="EditCaption">
                                        <div align="right">
                                            Provisionsabzug AD:
                                        </div>
                                    </td>
                                    <td class="EditData">
                                        <asp:CheckBox ID="chkProv" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <img name="" src="" width="1" height="1" alt="" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" class="tblHeader" title="Bemerkungen">
                                        Bemerkungen
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" class="EditCaption">
                                        <div align="right">
                                            Beschreibung:</div>
                                    </td>
                                    <td class="EditData">
                                        <asp:TextBox runat="server" ID="txtMemo" TextMode="MultiLine" Columns="80" Rows="8" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" />
                                        <br/>
                                        <input name="Submit3" type="button" class="btnStandard" value="Zeitstempel" title="Hier können Sie den Zeitstempel setzten." onclick="GBInsertTimeStamp(document.forms(0).txtMemo,'<%=Session["MM_VorName"] + " " + Session["MM_NachName"] + " " + DateTime.Now %>');">
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" class="tblFooter1">
                                        <div align="right">
                                             <asp:Button ID="btnSubmit" runat="server" class="btnSave" Text="Speichern" title="Speichern" OnClick="btnSubmit_Click" />
                                             <asp:Button ID="btnCancel2" runat="server" class="btnCancel" title="Abbrechen" Text="Abbrechen" OnClick="btnCancel_Click" />
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
    </form>
    <ctl:footer ID="ftr" runat="server" />
</body>
</html>