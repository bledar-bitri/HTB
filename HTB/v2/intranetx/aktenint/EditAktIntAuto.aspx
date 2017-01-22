﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditAktIntAuto.aspx.cs" Inherits="HTB.v2.intranetx.aktenint.EditAktIntAuto" %>

<%@ Register TagPrefix="ctl" TagName="lookupGegner" Src="~/v2/intranetx/global_files/CtlLookupGegner.ascx" %>
<%@ Register TagPrefix="ctl" TagName="lookupAuftraggeber" Src="~/v2/intranetx/global_files/CtlLookupAuftraggeber.ascx" %>
<%@ Register TagPrefix="ctl" TagName="lookupDealer" Src="~/v2/intranetx/global_files/CtlLookupDealer.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>HTB.ASP [ Intervention editieren ]</title>
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
    <script src="/v2/intranetx/scripts/jquery-1.3.2.js" type="text/javascript"></script>
    <script src="/v2/intranetx/scripts/jquery.MultiFile.js" type="text/javascript"></script>
</head>
<body>
    <ctl:header ID="header" runat="server" />
    <table width="100%" border="0" cellpadding="1" cellspacing="0" bgcolor="#000000">
        <tr>
            <td>
                <table width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
                    <tr>
                        <td class="smallText">
                            <a href="../../intranet/intranet/intranet.asp">Intranet</a> | <a href="../../intranet/intranet/inkasso.asp">CollectionInvoice</a> | <a href="../../intranet/aktenint/aktenint.asp">
                                Interventionsakte (&Uuml;bersicht)</a> | Auto Interventionsakt
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <br />
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" EnablePartialRendering="true" EnableScriptLocalization="true" />
    <script type="text/javascript">
//        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
<%--        function BeginRequestHandler(sender, args) {--%>
<%--            document.getElementById('<%= btnSubmit.ClientID %>').innerText = "Processing";--%>
<%--            document.getElementById('<%= btnSubmit.ClientID %>').disabled = true;--%>
<%--        }--%>
        </script>
        
            <table width="100%" border="0" cellspacing="0" cellpadding="1">
                <tr>
                    <td bgcolor="#000000">k
                        <table width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
                            <tr>
                                <td>
                                    <p>&nbsp;</p>
                                        <asp:UpdatePanel ID="updPanel1" runat="server">
                                        <Triggers>
                                           <asp:AsyncPostBackTrigger ControlID="txtAZ" EventName="TextChanged" />
                                        </Triggers>
                                        <ContentTemplate>
                                    <table border="0" align="center" cellpadding="3" cellspacing="0">
                                        <tr>
                                            <td colspan="2" class="tblHeader" title="NEUE AUTOINTERVENTION">
                                                NEUE AUTOINTERVENTION
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <ctl:message ID="ctlMessage" runat="server"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaptionTopLine">
                                                <div align="right">
                                                    Akt Nr.:
                                                </div>
                                            </td>
                                            <td class="EditDataTopLine">
                                                <asp:Label runat="server" ID="lblAktNr" Text="(wird autom. vergeben)"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">Akt Typ:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:DropDownList runat="server" ID="ddlAktType" AutoPostBack="true" OnSelectedIndexChanged="ddlAktType_SelectionChanged"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" class="EditCaption">
                                                <div align="right">Vertragsnummer:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox runat="server" ID="txtAZ" AutoPostBack="true" OnTextChanged="txtAZ_TextChanged" class="docText" onfocus="this.style.backgroundColor='#DFF4FF'; this.select();" onblur="this.style.backgroundColor='';" size="25" maxlength="50" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" class="EditCaption">
                                                <div align="right">Vertragsart:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox runat="server" ID="txtAktIntAutoVertragArt" class="docText" onfocus="this.style.backgroundColor='#DFF4FF'; this.select();" onblur="this.style.backgroundColor='';" size="45" maxlength="50" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" class="EditCaption">
                                                <div align="right">Auftraggeber:</div>
                                            </td>
                                            <td class="EditData">
                                                <ctl:lookupAuftraggeber ID="ctlLookupAuftraggeber" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" class="EditCaption">
                                                <div align="right">Gegner:</div>
                                            </td>
                                            <td class="EditData">
                                                <ctl:lookupGegner ID="ctlLookupGegner" runat="server" Category="Gegner"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" align="right" class="EditCaption">
                                                &nbsp;
                                            </td>
                                            <td class="EditData">
                                                <asp:LinkButton ID="cmdGegnerSearch" runat="server" class="smallText" Text="suchen" OnClick="cmdGegnerSearch_Click" />&nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton ID="cmdGegnerNew" runat="server" class="smallText" Text="neu" OnClick="cmdGegnerNew_Click" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton ID="cmdGegnerPhone" runat="server" class="smallText" Text="telefon" OnClick="cmdGegnerPhone_Click" />&nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton ID="cmdGegnerAddress" runat="server" class="smallText" Text="adresse" OnClick="cmdGegnerAddress_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" class="EditCaption">
                                                <div align="right">Mitverpflichteter:</div>
                                            </td>
                                            <td class="EditData">
                                                <ctl:lookupGegner ID="ctlLookupGegner2" runat="server" Category="Gegner_2"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" align="right" class="EditCaption">
                                                &nbsp;
                                            </td>
                                            <td class="EditData">
                                                <asp:LinkButton ID="cmdGegner2Search" runat="server" class="smallText" Text="suchen" OnClick="cmdGegner2Search_Click" />&nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton ID="cmdGegner2New" runat="server" class="smallText" Text="neu" OnClick="cmdGegner2New_Click" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton ID="cmdGegner2Phone" runat="server" class="smallText" Text="telefon" OnClick="cmdGegner2Phone_Click" />&nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton ID="cmdGegner2Address" runat="server" class="smallText" Text="adresse" OnClick="cmdGegner2Address_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" class="EditCaption">
                                                <div align="right">Sicherstellung H&auml;ndler:</div>
                                            </td>
                                            <td class="EditData">
                                                <ctl:lookupDealer ID="ctlLookupDealer" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" align="right" class="EditCaption">
                                                &nbsp;
                                            </td>
                                            <td class="EditData">
                                                <asp:LinkButton ID="cmdDealerSearch" runat="server" class="smallText" Text="suchen" OnClick="cmdDealerSearch_Click" />&nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton ID="cmdDealerNew" runat="server" class="smallText" Text="neu" OnClick="cmdDealerNew_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td nowrap class="EditCaption">
                                                <div align="right">
                                                    Inkassant:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:DropDownList runat="server" ID="ddlSB"/>
                                                <asp:LinkButton ID="cmdSearchSB" runat="server" class="smallText" Text="suchen" OnClick="cmdSearchSB_Click" title="Hier k&ouml;nnen Sie den zust&auml;ndigen Inkassant suchen." />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td nowrap class="EditCaption">
                                                <div align="right">Termin:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox runat="server" ID="txtTermin" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="12" maxlength="10" />
                                                (tt.mm.jjjj)
                                            </td>
                                        </tr>
                                        <tr>
                                            <td nowrap class="EditCaption">
                                                <div align="right">SB Auftraggeber Geschlecht:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:DropDownList runat="server" ID="ddlAktIntAgSbType">
                                                    <asp:ListItem Value="0" Text="*** bitte auswählen ***"/>
                                                    <asp:ListItem Value="1" Text="Herr"/>
                                                    <asp:ListItem Value="2" Text="Frau"/>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td nowrap class="EditCaption">
                                                <div align="right">
                                                    SB Auftraggeber:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox runat="server" ID="txtAktIntAGSB" class="docText" onfocus="this.style.backgroundColor='#DFF4FF'; this.select();" onblur="this.style.backgroundColor=''" size="35" maxlength="100"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td nowrap class="EditCaption">
                                                <div align="right">
                                                    SB Auftraggeber EMail:
                                                </div>
                                            </td>
                                            <td class="EditData">
                                               <asp:TextBox runat="server" ID="txtAktIntKSVEMail" class="docText" onfocus="this.style.backgroundColor='#DFF4FF'; this.select();" onblur="this.style.backgroundColor=''" size="35" maxlength="100"/>
                                            </td>
                                        </tr>
                                        <tr id="trStatus" runat="server">
                                            <td nowrap class="EditCaption">
                                                <div align="right">Status:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:DropDownList ID="ddlStatus" runat="server" class="docText">
                                                    <asp:ListItem Value="0" Text="0 - Neu Erfasst" />
                                                    <asp:ListItem Value="1" Text="1 - In Bearbeitung" />
                                                    <asp:ListItem Value="2" Text="2 - Abgegeben" />
                                                    <asp:ListItem Value="3" Text="3 - Fertig" />
                                                    <asp:ListItem Value="4" Text="4 - Abgeschlossen" />
                                                    <asp:ListItem Value="5" Text="5 - Abgeschlossene Altakte" />
                                                    <asp:ListItem Value="8" Text="8 - Unter Schwellwert" />
                                                    <asp:ListItem Value="9" Text="9 - Wartet auf Bonitätsprüfung" />
                                                    <asp:ListItem Value="10" Text="10 - Storno aufgrund Bonität" />
                                                    <asp:ListItem Value="11" Text="11 - Sofortklage" />
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <img src="" width="1" height="1" alt="" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" class="tblHeader2" title="Auto">
                                                Kraftfahrzeug
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">KFZ-Typ:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:DropDownList runat="server" ID="ddlAutoType"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">KFZ:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox runat="server" ID="txtAktIntAutoName" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="70" maxlength="100" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">Fahrgestell Nummer:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox runat="server" ID="txtAktIntAutoIdNr" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="25" maxlength="50" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">Erstzulassung:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox runat="server" ID="txtAktIntAutoFirstRegistrationDate" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="12" maxlength="10" />
                                                (tt.mm.jjjj)
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">Pol. Kennzeichen:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox runat="server" ID="txtAktIntAutoKZ" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="12" maxlength="20" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">Farbe:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox runat="server" ID="txtAktIntAutoColor" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="50" maxlength="100" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <img src="" width="1" height="1" alt="" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" class="tblHeader2" title="Forderungen">
                                                Forderungen
                                            </td>
                                        </tr>
                                        <tr id="trAktIntMissingInstallments" runat="server">
                                            <td nowrap class="EditCaption">
                                                <div align="right">
                                                    Anzahl derder offenen raten:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox runat="server" ID="txtAktIntMissingInstallments" class="docText" onfocus="this.style.backgroundColor='#DFF4FF'; this.select();" onblur="this.style.backgroundColor=''" size="10" maxlength="5"/>

                                            </td>
                                        </tr>
                                        <tr runat="server" id="trOpenedAmount">
                                            <td nowrap class="EditCaption">
                                                <div align="right">Offene Leasingrate:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox runat="server" ID="txtAmountOpened" class="docText" onfocus="this.style.backgroundColor='#DFF4FF'; this.select();" onblur="this.style.backgroundColor=''" size="15" maxlength="15" />
                                                &euro;
                                            </td>
                                        </tr>
                                        <tr runat="server" id="trAgKosten">
                                            <td nowrap class="EditCaption">
                                                <div align="right">
                                                    Auftraggeber Kosten:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox runat="server" ID="txtAgKosten" class="docText" onfocus="this.style.backgroundColor='#DFF4FF'; this.select();" onblur="this.style.backgroundColor=''" size="15" maxlength="15" />
                                                &euro;
                                            </td>
                                        </tr>
                                        <tr runat="server" id="trInsuranceAmount">
                                            <td nowrap class="EditCaption">
                                                <div align="right">
                                                    Versicherung:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox runat="server" ID="txtInsuranceAmount" class="docText" onfocus="this.style.backgroundColor='#DFF4FF'; this.select();" onblur="this.style.backgroundColor=''" size="15" maxlength="15" />
                                                &euro;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td nowrap class="EditCaption">
                                                <div align="right">
                                                    Sicherstellungsgeb&uuml;hr:</div>
                                            </td>
                                            <td class="EditData">
                                               <asp:TextBox runat="server" ID="txtAktIntKosten" class="docText" onfocus="this.style.backgroundColor='#DFF4FF'; this.select();" onblur="this.style.backgroundColor=''" size="15" maxlength="15"/>
                                                &euro;
                                                <asp:LinkButton ID="cmdGetCollectionAmount" runat="server" class="smallText" Text="suchen" OnClick="cmdGetCollectionAmount_Click" title="Hier k&ouml;nnen Sie die Sicherstellungsgeb&uuml;hr suchen." />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <img src="" width="1" height="1" alt="" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" class="tblHeader2" title="Forderungen">
                                                Versicherung
                                            </td>
                                        </tr>
                                        <tr>
                                            <td nowrap class="EditCaption">
                                                <div align="right">Versicherungsname:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox runat="server" ID="txtAktIntInsuranceName" class="docText" onfocus="this.style.backgroundColor='#DFF4FF'; this.select();" onblur="this.style.backgroundColor=''" size="35" maxlength="100"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td nowrap class="EditCaption">
                                                <div align="right">Versicherungstelefon:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox runat="server" ID="txtAktIntInsurancePhone" class="docText" onfocus="this.style.backgroundColor='#DFF4FF'; this.select();" onblur="this.style.backgroundColor=''" size="35" maxlength="100"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">Polizenummer:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox runat="server" ID="txtAktIntInsuranceAccount" class="docText" onfocus="this.style.backgroundColor='#DFF4FF'; this.select();" onblur="this.style.backgroundColor=''" size="35" maxlength="100"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">Versicherungsbankname:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox runat="server" ID="txtAktIntInsuranceBankName" class="docText" onfocus="this.style.backgroundColor='#DFF4FF'; this.select();" onblur="this.style.backgroundColor=''" size="35" maxlength="100"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">VersicherungsBLZ:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox runat="server" ID="txtAktIntInsuranceBLZ" class="docText" onfocus="this.style.backgroundColor='#DFF4FF'; this.select();" onblur="this.style.backgroundColor=''" size="35" maxlength="100"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">Versicherungskonto:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox runat="server" ID="txtAktIntInsuranceKtoNr" class="docText" onfocus="this.style.backgroundColor='#DFF4FF'; this.select();" onblur="this.style.backgroundColor=''" size="35" maxlength="100"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <img src="" width="1" height="1" alt="" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" class="EditCaptionTopLine">
                                                <div align="right">Beschreibung:&nbsp;</div>
                                            </td>
                                            <td class="EditDataTopLine">
                                                <asp:TextBox runat="server" ID="txtMemo" Columns="80" Rows="8" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" TextMode="MultiLine"/>
                                                <br/>
                                                <asp:Button runat="server" ID="cmdTimeStamp" class="smallText" Text="Zeitstempel" title="Hier können Sie den Zeitstempel setzten." />
                                            </td>
                                        </tr>
                                        <tr  id="trEditOnly" runat="server">
                                            <td colspan="2" class="EditCaptionTopLine3">
                                                <table width="100%" border="1" align="center" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
                                                    <tr>
                                                        <td class="tblHeader" title="Buchungen">
                                                            Buchungen
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <table width="100%" cellpadding="4" cellspacing="0">
                                                                <tr>
                                                                    <td class="tblData1">
                                                                        <asp:GridView ID="gvInvoices" runat="server" AllowSorting="false" AutoGenerateColumns="False" CellPadding="2" BorderStyle="Groove" Width="100%" ShowFooter="true">
                                                                            <RowStyle />
                                                                            <Columns>
                                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                                                    <ItemTemplate>
                                                                                        <a href="javascript:void(window.open('<%# Eval("DeletePopupUrl")%>','_blank','toolbar=no,scrollbars=yes,resizable=yes,width=800,height=800'))">
                                                                                            <asp:Image ID="img" runat="server" ImageUrl='<%# Eval("DeleteUrl")  %>' BorderColor="White" />
                                                                                        </a>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                                                    <ItemTemplate>
                                                                                        <a href="javascript:void(window.open('<%# Eval("EditPopupUrl")%>','_blank','toolbar=no,scrollbars=yes,resizable=yes,width=800,height=800'))">
                                                                                            <asp:Image ID="img" runat="server" ImageUrl='<%# Eval("EditUrl")  %>' BorderColor="White" />
                                                                                        </a>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:HyperLinkField HeaderText="Nr." DataTextField="InvoiceID" DataNavigateUrlFields="InvoiceID" ItemStyle-HorizontalAlign="Right" DataTextFormatString="&lt;a href=javascript:MM_openBrWindow('/v2/intranetx/aktenink/ShowInvoice.aspx?InvId={0}','popSetAction','menubar=yes,scrollbars=yes,resizable=yes,width=800,height=800,top=10');&gt;{0}&lt;/a&gt;"
                                                                                    SortExpression="InvoiceID" />
                                                                                <asp:BoundField HeaderText="Nr." DataField="PosInvoiceID" SortExpression="InvoiceID" ItemStyle-HorizontalAlign="Left" />
                                                                                <asp:BoundField HeaderText="Datum" DataField="InvoiceDate" SortExpression="InvoiceDate" ItemStyle-HorizontalAlign="Center" />
                                                                                <asp:BoundField HeaderText="Text" DataField="InvoiceDescription" SortExpression="InvoiceDescription" FooterText="Forderung:" FooterStyle-Font-Bold="True" FooterStyle-HorizontalAlign="Right" />
                                                                                <asp:TemplateField HeaderText="Betrag" FooterStyle-Font-Bold="True" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                                                                    <ItemTemplate>
                                                                                        <%# Eval("InvoiceAmount") %>
                                                                                    </ItemTemplate>
                                                                                    <FooterTemplate>
                                                                                        <%# HTBUtilities.HTBUtils.FormatCurrency(GetTotalDue()) %>
                                                                                    </FooterTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:BoundField HeaderText="Fällig" DataField="DueDate" SortExpression="DueDate" ItemStyle-HorizontalAlign="Center" />
                                                                            </Columns>
                                                                        </asp:GridView>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td id="tdNewBuchung" runat="server">
                                                                        <div align="right">
                                                                            <input name="Submit4" type="button" class="smallText" title="Hier k&ouml;nnen Sie eine neue Buchung abstellen." onclick="MM_openBrWindow('/v2/intranetx/aktenint/EditBooking.aspx?IntAkt=<%=GetAkt().AktIntID %>','popWindow','menubar=yes,scrollbars=yes,resizable=yes,width=800,height=800')"
                                                                                value="Neue Buchung">
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tblHeader" title="Dokumente">
                                                            Dokumente
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <table width="100%" border="0" cellspacing="0" cellpadding="4">
                                                                <tr>
                                                                    <td valign="top" class="tblData1">
                                                                        <asp:GridView ID="gvDocs" runat="server" AllowSorting="false" AutoGenerateColumns="False" CellPadding="2" BorderStyle="Groove" Width="100%" ShowFooter="true">
                                                                            <RowStyle />
                                                                            <Columns>
                                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                                                    <ItemTemplate>
                                                                                        <a href="javascript:void(window.open('<%# Eval("DeletePopupUrl")%>','_blank','toolbar=no,menubar=no'))">
                                                                                            <asp:Image ID="img" runat="server" ImageUrl='<%# Eval("DeleteUrl")  %>' BorderColor="White" />
                                                                                        </a>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:BoundField HeaderText="Datum" DataField="DokCreationTimeStamp" SortExpression="DokCreationTimeStamp" ItemStyle-HorizontalAlign="Left" />
                                                                                <asp:BoundField HeaderText="Typ" DataField="DokTypeCaption" SortExpression="DokTypeCaption" ItemStyle-HorizontalAlign="Left" />
                                                                                <asp:BoundField HeaderText="Bezeichnung" DataField="DokCaption" SortExpression="DokCaption" ItemStyle-HorizontalAlign="Left" />
                                                                                <asp:TemplateField HeaderText="Anlage" ItemStyle-HorizontalAlign="Left">
                                                                                    <ItemTemplate>
                                                                                        <a href="javascript:void(window.open('../../intranet/documents/files/<%# Eval("DokAttachment")  %>','popSetAction','menubar=yes,scrollbars=yes,resizable=yes,width=800,height=800,top=10'))">
                                                                                            <%# Eval("DokAttachment")  %>
                                                                                        </a>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                        </asp:GridView>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <div align="right">
                                                                <br>
                                                                <input name="Submit6" type="button" class="smallText" title="Hier k&ouml;nnen Sie ein neues Dokument anlegen." onclick="MM_openBrWindow('../../intranet/documents/newaktintdok.asp?ADAktTyp=3&AktID=<%=GetAkt().AktIntID %>&GegnerID=<%=GetGegnerID()%>&KlientID=<%=GetKlientID() %>','newDok','menubar=yes,scrollbars=yes,resizable=yes,width=800,height=600')"
                                                                    value="Neues Dokument">
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                    </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>

                            <tr id="trFileUpload" runat="server">
                                <td>
                                    <table border="0" align="center" cellpadding="3" cellspacing="0">
                                        <tr>
                                            <td valign="top" align="right" class="EditCaptionTopLine">
                                                Dokumente:
                                            </td>
                                            <td class="EditDataTopLine">
                                                <div>
                                                    <asp:FileUpload ID="FileUpload1" runat="server" class="multi" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                           </tr>
                           <tr>
                           <td>
                           <table border="0" align="center" cellpadding="3" cellspacing="0">
                           <tr>
                            <td colspan="2" class="tblFooter1">
                                <div align="right">
                                    <asp:Button runat="server" ID="btnSubmit" class="btnSave" title="Speichern" onclick="btnSubmit_Click" Text="Speichern"/>
                                    <asp:Button runat="server" ID="btnCancel" class="btnCancel" title="Abbrechen" onclick="btnCancel_Click" Text="Abbrechen"/>
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
