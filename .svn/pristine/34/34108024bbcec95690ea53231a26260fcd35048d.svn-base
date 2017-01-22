<%@ Page Language="C#" Culture="de-DE" AutoEventWireup="true" CodeBehind="PrintAuftrag.aspx.cs" Inherits="HTB.v2.intranetx.aktenint.PrintAuftrag" %>
<%@ Import Namespace="HTBAktLayer" %>
<%@ Import Namespace="HTBExtras" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../intranet/styles/inksys.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style2
        {
            font-size: 14;
            font-weight: bold;
        }
        .style4
        {
            font-size: 14;
        }
        .Stil1
        {
            font-size: 9px;
        }
    </style>
    <script language="JavaScript" type="text/JavaScript">
        function CheckForClose(evt) {
            if (evt.keyCode == 27) {
                window.opener.document.location.reload();
                window.close();
            }
        }
    </script>
</head>
<body onkeypress="CheckForClose(event);">
    <% 
        for (int i = 0; i < aktenList.Count; i++)
        {
            var akt = (HTB.Database.Views.qryAktenInt)aktenList[i];
            SetStatusPrinted(akt.AktIntID);
            AktIntAmounts pa = AktInterventionUtils.GetAktAmounts(akt);
            //Akt = Hillinger
            if (akt.AuftraggeberID == 38)
            {

    %>
    <hr>
    <span class="docText">
        <br>
        <br>
        <br>
        <strong>A C H T U N G !!!</strong><br>
        <br>
        DIESEN AUFTRAG VON IDH ERHALTEN SIE PER MAIL VOM E.C.P. OFFICE ZUGESCHICKT.
        <br>
        FALLS SIE DIESEN BIS DATO ODER IN KÜRZE NICHT ERHALTEN, DANN BITTE WENDEN SIE SICH AN DIE OFFICE DAMEN DER FIRMA E.C.P.<br>
        MIT FREUNDLICHEM GRUSS,
        <br>
        IHR ADMINISTRATOR </span>
    <br>
    <br>
    <span class="docText"><strong>A U F T R A G</strong> <strong>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[AZ:<%=akt.AktIntAZ + ((akt.AktIntAktType == 6 || akt.AktIntAktType == 7) ? "&nbsp;&nbsp;&nbsp;AK: " + akt.AktIntIZ : "")%>]<% if (akt.AKTIntDub == 1)
                                                                                                                        { %>&nbsp;DUB!<% } %></strong><br/>
        Pers&ouml;nliche Intervention
        <br>
        <%=akt.UserVorname%>&nbsp;<%=akt.UserNachname%></span><br>
        <span class="docText">Abgabedatum:&nbsp;<strong><%=akt.AktIntTerminAD.ToShortDateString()%></strong><br/></span>
        <span class="docText">Schuldner:&nbsp;<strong><%=akt.GegnerLastName1%></strong><br/></span>
        <span class="docText">Akttyp:&nbsp;<strong><%=akt.AktTypeINTCaption%></strong><br /></span>
            <br/>
            <hr/>
            <%
//no pagebreake on the last sheet of the print
if (i < aktenList.Count - 1)
{
            %>
            <br style="page-break-before: always;"/>
            <%
}
        }
        else
        {
            //Akt <> Hillinger
            //1st Page standard for all cases
            %>
            <table width="100%" border="0" cellpadding="4" cellspacing="0" class="docText">
                <tr>
                    <td valign="top">
                        <strong>
                            <%=akt.AuftraggeberName1%>,
                            <%=akt.AuftraggeberName2%></strong><br>
                            <%=akt.AuftraggeberStrasse%>
                            <%=akt.AuftraggeberLKZ%>&nbsp;<%=akt.AuftraggeberPLZ%>&nbsp;<%=akt.AuftraggeberOrt%>
                    </td>
                </tr>
            </table>
            <hr>
            <p align="left">
                <span class="docText"><strong>A U F T R A G</strong> <strong>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[AZ:
                    <%=akt.AktIntAZ + ((akt.AktIntAktType == 6 || akt.AktIntAktType == 7) ? "&nbsp;&nbsp;&nbsp;AK: " + akt.AktIntIZ : "")%>]<% if (akt.AKTIntDub == 1)
                                         { %>&nbsp;DUB!<% } %></strong><br>
                    Pers&ouml;nliche Intervention
                    <br><%=akt.UserVorname%>&nbsp;<%=akt.UserNachname%></span><br>
                    <span class="docText">Abgabedatum:&nbsp;<strong><%=akt.AktIntTerminAD.ToShortDateString()%></strong><br /></span>
                    <span class="docText">Akttyp:&nbsp;<strong><%=akt.AktTypeINTCaption%></strong></span>
            </p>
            <p align="right" class="docText">
                Salzburg,
                <%=DateTime.Now.ToShortDateString()%></p>
            <table width="100%" border="0" cellpadding="0" cellspacing="2" class="docText">
                <tr>
                    <td>
                        <strong>Bearb. durch:</strong>
                    </td>
                    <td colspan="2">
                        Partner der E.C.P. European Car Protect KG, Schwarzparkstr. 15, A-5020&nbsp;Salzburg
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <strong>Auftraggeber:</strong>
                    </td>
                    <td colspan="2">
                        <%=akt.AuftraggeberName1%>&nbsp;<%=akt.AuftraggeberName2%>, Kontakt:
                        <%=akt.AKTIntAGSB%>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td width="144">
                        <strong>Klient:</strong>
                    </td>
                    <td colspan="2">
                        <%=akt.KlientName1%>
                    </td>
                </tr>
                <%if (akt.KlientName2.Trim() != "")
                  {%>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td colspan="2">
                        <%=akt.KlientName2%>
                    </td>
                </tr>
                <%} %>
                <%if (akt.KlientName3.Trim() != "")
                  { %>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td colspan="2">
                        <%=akt.KlientName3%>
                    </td>
                </tr>
                <%} %>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td colspan="2">
                        <%=akt.KlientStrasse%>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td colspan="2">
                        <%=akt.KlientLKZ%>-<%=akt.KlientPLZ%>&nbsp;<%=akt.KlientOrt%>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <strong>Gegner:</strong>
                    </td>
                    <td width="797">
                        <%=akt.GegnerLastName1%>
                    </td>
                    <td width="317">
                        <strong>Tel:&nbsp;</strong>
                        <%if (akt.GegnerPhone.Trim() != "")
                          {%>
                        <%=akt.GegnerPhoneCountry%><%=akt.GegnerPhoneCity%><%=akt.GegnerPhone%>
                        <%}
                          else
                          {%>
                        unbekannt!
                        <%} %>
                    </td>
                </tr>
                <%if (akt.GegnerLastName2.Trim() != "")
                  { %>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td colspan="2">
                        <%=akt.GegnerLastName2%>
                    </td>
                </tr>
                <%} %>
                <% if (akt.GegnerLastName3.Trim() != "")
                   {%>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td colspan="2">
                        <%=akt.GegnerLastName3%>
                    </td>
                </tr>
                <%} %>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td colspan="2">
                        <%=akt.GegnerLastStrasse%>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td colspan="2">
                        <%=akt.GegnerLastZipPrefix%>-<%=akt.GegnerLastZip%>&nbsp;<%=akt.GegnerLastOrt%>
                    </td>
                </tr>
                <%if ((akt.AktIntOriginalMemo.Trim() + akt.AKTIntMemo.Trim()) != "")
                  {%>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <strong>Info:</strong>
                    </td>
                    <td>
                        <% string memo = akt.AktIntOriginalMemo.Trim() + "<br/>" + akt.AKTIntMemo; %>
                        <%= memo.Length > 150 ? memo.Substring(0, 150) : memo %>
                    </td>
                    <td>
                        <strong>GebDat:</strong>
                        <%if (akt.GegnerGebDat.ToShortDateString() != "" && akt.GegnerGebDat.ToShortDateString() != "01.01.1900")
                          { %>
                        <%=akt.GegnerGebDat.ToShortDateString()%>
                        <%}
                          else
                          {%>
                        unbekannt!
                        <%}%>
                    </td>
                </tr>
                <%}%>
            </table>
            <hr>
            <p align="left">
            </p>
            <table width="100%" border="0" cellpadding="0" cellspacing="2" class="couriertext">
                <tr>
                    <td colspan="5">
                        <strong>Forderungsaufstellung:</strong>
                    </td>
                </tr>
                <%
                    foreach (AktIntForderungPrintLine pf in pa.ForderungList)
                    {
                %>
                <tr>
                    <td width="40">
                        &nbsp;
                    </td>
                    <td width="300">
                        <%=pf.Text %>
                </td>
                <td>
                    EUR
                </td>
                <td>
                    <div align="right">
                        <%= HTBUtilities.HTBUtils.FormatCurrencyNumber(pf.Amount)%></div>
                </td>
                <td>
                    <div align="right">
                    </div>
                </td>
                </tr>
                <%} %>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        inkl. Bearbeitungsgebühren
                    </td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>Zinsen</td>
                    <td>EUR</td>
                    <td><div align="right"><%=  HTBUtilities.HTBUtils.FormatCurrencyNumber(pa.Zinsen)%></div></td>
                    <td width="150"><div align="right"></div></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>CollectionInvoicekosten</td>
                    <td>EUR</td>
                    <td><div align="right"><%= HTBUtilities.HTBUtils.FormatCurrencyNumber(pa.InkassoKosten)%></div></td>
                    <td><div align="right"></div></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>20% MWSt aus Kosten</td>
                    <td>EUR</td>
                    <td><div align="right"><%= HTBUtilities.HTBUtils.FormatCurrencyNumber(pa.MWS)%></div></td>
                    <td><div align="right"></div></td>
                </tr>
                <%if (pa.Zahlungen < 0)
                  { %>
                  <tr>
                    <td>&nbsp;</td>
                    <td>Zahlungen</td>
                    <td>EUR</td>
                    <td><div align="right"><%= HTBUtilities.HTBUtils.FormatCurrencyNumber(pa.Zahlungen)%></div></td>
                    <td><div align="right"></div></td>
                  </tr>
                <%} %>
                <tr>
                    <td>&nbsp;</td>
                    <td colspan="3"><div align="right">---------------------------------------------------</div></td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td><strong>Forderung:</strong></td>
                    <td><strong>EUR</strong></td>
                    <td><div align="right"><%=  HTBUtilities.HTBUtils.FormatCurrencyNumber(pa.GetTotalLessWeggebuhr())%></div></td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>+ Weggeb&uuml;hr</td>
                    <td>EUR</td>
                    <td><div align="right"><%= HTBUtilities.HTBUtils.FormatCurrencyNumber(pa.Weggebuhr)%></div></td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td colspan="3"><div align="right">---------------------------------------------------</div></td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td><strong>Endsumme (inkl. Weggeb&uuml;hr)</strong></td>
                    <td><strong>EUR</strong></td>
                    <td><div align="right"><%= HTBUtilities.HTBUtils.FormatCurrencyNumber(pa.GetTotal())%></div></td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td colspan="3"><div align="right"><strong>=================================================== </strong></div>
                    </td><td>&nbsp;</td>
                </tr>
            </table>
            <div align="center">
                <strong>
                    <br>
                    <span class="Stil1">Wir bitten Sie den oben angef&uuml;hrten Betrag, dem Überbringer dieses Schreibens, gegen eine separate Quittung SOFORT zu bezahlen.</span></strong></div>
            <div align="center" class="Stil1">
                Bitte begleichen Sie Ihre offene Schuld, Sie vermeiden somit höhere Kosten und weitere Konsequenzen. Vereinbarungsgemäß wird mit der ersten Zahlung die Weggebühr, wie oben angeführt, in Abzug gebracht.
                Gleichzeitig setzen wir Sie davon in Kenntnis, dass wir nach den Bestimmungen des Datenschutzgesetzes die ermittelten Daten zwecks Gläubigerschutz und der Bonitätsprüfung in Anwendung bringen werden.</div>
            <br>
            <%
            if (!printRV && i < aktenList.Count - 1)
            {
            %>
                <br style="page-break-before: always;">
            <%
            }
            %>
            <%
            if (printRV)
                {
            %>
            <br style="page-break-before: always;">
            <%
                //2nd Page Ratenvereinbarungen nach AG

                //RV Infoscore
                switch (akt.AuftraggeberID)
                {
                    case 32:
            %>
            </div>
            <p class="docText" align="right">
                Infoscore austria gmbh<br />
                weyringergasse 1<br />
                1040 Wien<br />
                Tel: 0820/5008800</p>
            <p class="docText" align="left">
                <strong>Aktenzeichen</strong>:
                <%=akt.AktIntAZ%></p>
            <p class="docText" align="left">
                <strong>Forderungsh&ouml;he</strong>:
                <%= HTBUtilities.HTBUtils.FormatCurrencyNumber(pa.GetTotal())%>
                <strong>EUR</strong> <span class="smallText">(zuz&uuml;gl. Kosten dieser Vereinbarung)</span></p>
            <p class="docText" align="left">
                <strong>Forderung von</strong>:
                <%=akt.KlientName1%>&nbsp;<%=akt.KlientName2%></p>
            <p class="docText" align="left">
                &nbsp;</p>
            <p align="center" class="printText">
                <strong>R A T E N A N S U C H E N</strong></p>
            <p class="docText" align="left">
                Ich ersuche um Genehmigung des nachstehenden Ratenansuchens:</p>
            <table width="100%" border="0" cellpadding="1">
                <tr>
                    <td>
                        <p class="docText">
                            <strong>Ratenbetrag</strong> .............................. <strong>EUR</strong></p>
                    </td>
                    <td>
                        <p class="docText">
                            <strong>Zahlbar ab</strong> ..............................
                        </p>
                    </td>
                </tr>
            </table>
            <p class="printText" align="center">
                &nbsp;</p>
            <p class="printText" align="center">
                S E L B S T A U S K U N F T
            </p>
            <table width="100%" border="0" cellpadding="5" cellspacing="0">
                <tr>
                    <td colspan="3" class="tblDataHeader4">
                        <div align="center">
                            <strong>PERS&Ouml;NLICHE DATEN </strong>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="tblData1">
                        &nbsp;
                    </td>
                    <td class="tblData2">
                        <div align="center">
                            <strong>Zahlungspflichtiger</strong></div>
                    </td>
                    <td class="tblData3">
                        <div align="center">
                            <strong>Ehepartner/Lebensgef./B&uuml;rge</strong></div>
                    </td>
                </tr>
                <tr>
                    <td class="tblData1">
                        <strong>Vor- und Zuname
                            <br>
                            &nbsp; </strong>
                    </td>
                    <td class="tblData2">
                        &nbsp;
                    </td>
                    <td class="tblData3">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="tblData1">
                        <strong>Anschrift</strong><br>
                        <span class="smallText">(PLZ, Ort, Str.,Nr.)</span>
                    </td>
                    <td class="tblData2">
                        &nbsp;
                    </td>
                    <td class="tblData3">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="tblData1">
                        <strong>Geburtsdatum<br>
                            <span class="smallText">&nbsp;</span></strong>
                    </td>
                    <td class="tblData2">
                        &nbsp;
                    </td>
                    <td class="tblData3">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="tblData1">
                        <strong>Familienstand/Verh&auml;ltnis zu Schuldner
                            <br>
                            <span class="smallText">&nbsp;</span></strong>
                    </td>
                    <td class="tblData2">
                        &nbsp;
                    </td>
                    <td class="tblData3">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="tblData1">
                        <strong>SVA Nummer
                            <br>
                            <span class="smallText">&nbsp;</span></strong>
                    </td>
                    <td class="tblData2">
                        &nbsp;
                    </td>
                    <td class="tblData3">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="tblData1">
                        <strong>Beruf<br>
                            <span class="smallText">&nbsp;</span></strong>
                    </td>
                    <td class="tblData2">
                        &nbsp;
                    </td>
                    <td class="tblData3">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="tblData1">
                        <strong>Arbeitgeber/Auszuzahlende Stelle
                            <br>
                            <span class="smallText">&nbsp;</span></strong>
                    </td>
                    <td class="tblData2">
                        &nbsp;
                    </td>
                    <td class="tblData3">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="tblData1">
                        <strong>Telefon<br>
                            <span class="smallText">&nbsp;</span></strong>
                    </td>
                    <td class="tblData2">
                        &nbsp;
                    </td>
                    <td class="tblData3">
                        &nbsp;
                    </td>
                </tr>
            </table>
            <br>
            <span class="smallText">Ich erkl&auml;re hiermit die derzeitige Forderung (zzgl. weiterer Zinsen und inkassokosten) ausdr&uuml;cklich und unwiderruflich anzuerkennen und verpflichte mich die Gesamtforderung
                zu bezahlen.<br>
                Damit mein Ansuchen bearbeitet werden kann, best&auml;tige ich mit meiner Unterschrift, dass meine Angaben wahrheitsgem&auml;&szlig; und vollst&auml;ndig sind.<br>
            </span>
            <br>
            <br>
            <br>
            <table width="100%" border="0" cellpadding="1">
                <tr>
                    <td>
                        ________________________
                    </td>
                    <td>
                        ________________________
                    </td>
                </tr>
                <tr>
                    <td class="docText">
                        Ort, Datum
                    </td>
                    <td class="docText">
                        Unterschrift
                    </td>
                </tr>
            </table>
            <p align="left" class="docText">
                LAUFENDES INKASSO VEREINBART &nbsp;&nbsp;&nbsp;<span class="style5"> O</span></p>
            <%
                //no pagebreake on the last sheet of the print
                if (i < aktenList.Count - 1)
                {
            %>
            <br style="page-break-before: always;">
            <%
                }

                break;
                    //RV Hillinger (disabled)
                    case 99999:
            %>
            <table width="956" border="0">
                <tr>
                    <td>
                        <div align="right">
                            <strong>Aktenzahl</strong>:
                            <%=akt.AktIntAZ%></div>
                    </td>
                </tr>
                <tr>
                    <td>
                        An
                    </td>
                </tr>
                <tr>
                    <td>
                        CollectionInvoicedienst Hillinger GmbH
                    </td>
                </tr>
                <tr>
                    <td>
                        Linzer Stra&szlig;e 46 A
                    </td>
                </tr>
                <tr>
                    <td>
                        4810 Gmunden
                    </td>
                </tr>
                <tr>
                    <td>
                        Tel.: 07612/75800-0 (Fax-DW 19)
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <strong>Unser Auftraggeber</strong>:
                        <%=akt.KlientName1%>&nbsp;<%=akt.KlientName2%>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <strong>Anerkenntnis - Ratenzahlung - Stundung</strong>:
                    </td>
                </tr>
                <tr>
                    <td>
                        Forderung (Kapital, Zinsen und Kosten):
                        <%= HTBUtilities.HTBUtils.FormatCurrencyNumber(pa.GetTotal())%>
                        <strong>EUR</strong>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <div align="justify">
                            Da ich (wir) obige Gesamtforderung innerhalb der gesetzten Frist nicht bezahlen kann (k&ouml;nnen),<br>
                            unterbreite(n) ich (wir) folgenden Vorschlag:<br>
                            Obige Forderung ist zur G&auml;nze f&auml;llig und anerkenne(n) ich (wir) hiermit diese Forderung inkl. der<br>
                            aufgeschl&uuml;sselten Kosten It. Beilage sowie der aufgelaufenen Zinsen. Ich (wir) verpflichte(n) mich (uns) zur<br>
                            Bezahlung obiger Forderung zur ungeteilten Hand. Um gerichtliche Schritte zu vermeiden, trete(n) ich (wir)<br>
                            Ihnen hiermit zur Besicherung der obigen Forderung meine Gehalts-und Lohnanspr&uuml;che gegen meinen<br>
                            derzeitigen Arbeitgeber unwiderruflich, f&uuml;r den Fall des Zustandekommens einer Ratenvereinbarung aber<br>
                            bedingt mit Wirkung der F&auml;lligkeit, ab. Diese Abtretung gilt auch f&uuml;r Anspr&uuml;che gegen&uuml;ber allf&auml;lligen<br>
                            k&uuml;nftigen Arbeitgebern und f&uuml;r gegenw&auml;rtige und zuk&uuml;nftige Pensionsanspr&uuml;che.</div>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        Ich (wir) ersuche(n) um Zustimmung zur nachstehenden Ratenzahlung bzw. Stundung:
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <strong>A - Ratenzahlung *) monatliche Raten in H&ouml;he von EUR .....................................................<br>
                            Zahlungsbeginn der 1. Rate am :..............................................................................................</strong><br>
                        Terminsverlust tritt gem&auml;&szlig; umseitigen Bedingungen bei Verzug mit auch nur einer Rate ein.
                    </td>
                </tr>
                <tr>
                    <td>
                        oder
                    </td>
                </tr>
                <tr>
                    <td>
                        <strong>B - Stundung *) der gesamten Forderung bis zum:</strong><strong>..................................................................</strong>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        F&uuml;r die Dauer der Ratenzahlung/Stundung werden <strong>............... % Jahreszinsen </strong>monatlich kontokorrentm&auml;&szlig;ig
                    </td>
                </tr>
                <tr>
                    <td>
                        und die Kosten der Vereinbarung von <strong>............... EUR </strong>verrechnet. Zahlungen sind mit schuldbefreiender
                    </td>
                </tr>
                <tr>
                    <td>
                        Wirkung ausschlie&szlig;lich an IDH-CollectionInvoicedienst Hillinger in Gmunden zu leisten.
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        Ich (wir) habe(n) die umseitig aufgedruckten Bedingungen zur G&auml;nze gelesen und anerkenne(n) diese vollinhaltlich.
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        Ort und Datum<strong> ...........................................................................................</strong> Unterschrift(en)<strong> ...........................................................................................</strong>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        Bitte umseitig aufgedruckte Bedingungen genau lesen sowie umseitig richtig und vollst&auml;ndig ausf&uuml;llen und
                    </td>
                </tr>
                <tr>
                    <td>
                        unterschreiben, ansonsten der Raten-/Stundungsvorschlag nicht genehmigt werden kann.
                    </td>
                </tr>
                <tr>
                    <td>
                        Als Konsument haben Sie nach &sect;3 KSchG das Recht, innerhalb einer Woche nach Erhalt einer Kopie, von
                    </td>
                </tr>
                <tr>
                    <td>
                        dieser Vereinbarung schriftlich zur&uuml;ckzutreten.
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        *) Nichtzutreffendes bitte streichen.
                    </td>
                </tr>
            </table>
            <%
                //no pagebreake on the last sheet of the print
                if (i < aktenList.Count - 1)
                {
            %>
            <br style="page-break-before: always;">
            <%
                }
            %>
            <strong>Bedingungen für Ratenzahlung bzw. Stundung </strong>
            <br>
            <br>
            Das Zustandekommen der Ratenzahlungs-/Stundungsvereinbarung bedarf der schriftlichen Zustimmung
            <br>
            durch den Auftraggeber bzw. das CollectionInvoiceinstitut: CollectionInvoicedienst Hillinger GmbH, Linzer Straße 46 A, in Gmunden.
            <br>
            <br>
            Der umseitige Ratenzahlungs-jStundungsvorschlag berührt den Bestand und die Fälligkeit der Forderung
            <br>
            nicht. Sofern dem Ratenzahlungs-jStundungsvorschlag nicht innerhalb von 10 Tagen ab Einlangen bei der
            <br>
            CollectionInvoicedienst Hillinger GmbH schriftlich zugestimmt wird, gilt dieser als abgelehnt.
            <br>
            <br>
            Für die Betreibung der Forderung durch die CollectionInvoicedienst Hillinger GmbH sind monatliche Evidenzgebühren,
            <br>
            gestaffelt nach Forderungshöhe von 0,-bis 70,-EUR 2,--, bis 300,-EUR 4,--, bis 700,-EUR 6,--und darüber
            <br>
            EUR 10,--, sowie für den Kontoauszug monatlich ein Betrag von EUR 7,50 bis EUR 400,-und darüber EUR
            <br>
            12,50 zu leisten. Änderungen insbesondere Adressenänderungen sind der CollectionInvoicedienst Hillinger GmbH
            <br>
            unverzüglich bekannt zu geben, widrigenfalls Erhebungskosten verrechnet werden.
            <br>
            <br>
            Im Falle des Zahlungsverzuges verpflichte(n) ich (wir) mich (uns) zum Ersatz aller Kosten, Spesen und
            <br>
            Barauslagen, die Ihnen durch die zweckentsprechende Verfolgung Ihrer Ansprüche, insbesondere aus
            <br>
            diesem Vertragsverhältnis, entstehen. Ich (Wir) verpflichte(n) mich (uns), sämtliche Kosten zur ungeteilten
            <br>
            Hand mit dem umseitigen Zinssatz verzinst, zu bezahlen.
            <br>
            <br>
            Zahlungen werden vorerst auf Kosten und Zinsen verrechnet. Im Falle des Verzuges mit auch nur einer Rate
            <br>
            von mindestens 6 Wochen und Nachfristsetzung von 2 Wochen, ist die gesamte Forderung fällig (Terminsverlust).
            <br>
            <br>
            <strong>Bitte genau und wahrheitsgemäß ausfüllen</strong>:
            <br>
            <br>
            <table width="956" border="0">
                <tr>
                    <td width="609">
                        <strong>Vor - u. Zuname des Schuldners: ..............................................................................................</strong>
                    </td>
                    <td width="337">
                        <strong>geboren am: ................................</strong>
                    </td>
                </tr>
                <tr>
                    <td>
                        <strong>Vor-u. Zuname des Gattenteill Mitschuldners: .......................................................................</strong>
                    </td>
                    <td>
                        <strong>geboren am: ................................</strong>
                    </td>
                </tr>
            </table>
            <br>
            Ich anerkenne hiermit die umseitige Forderung und verpflichte mich, diese zur ungeteilten Hand mit dem Schuldner zu bezahlen.
            <br>
            <br>
            <table width="956" border="0">
                <tr>
                    <td>
                        Genaue Anschrift und Telefonnummer:
                    </td>
                </tr>
                <tr>
                    <td>
                        Name und Anschrift des Arbeitgebers:
                    </td>
                </tr>
                <tr>
                    <td>
                        monatliches Einkommen:
                    </td>
                </tr>
            </table>
            <br>
            Ich (wir) anerkenne(n) hiermit die umseitige Forderung zuzüglich Zinsen und Kosten, habe{n)
            <br>
            obige Bedingungen zur Gänze gelesen und bin (sil1d) damit voll inhaltlich einverstanden.
            <br>
            <br>
            <table width="956" border="0">
                <tr>
                    <td>
                        Ort und Datum<strong> ...........................................................................................</strong> Unterschrift(en)<strong> ...........................................................................................</strong>
                    </td>
                </tr>
            </table>
            <%
                //no pagebreake on the last sheet of the print
                if (i < aktenList.Count - 1)
                {
            %>
            <br style="page-break-before: always;">
            <%
                }
                break;
                    //RV Standard
                    default:
            %>
            <span class="docText">
                <%=akt.GegnerLastName1%><br>
                <%=akt.GegnerLastName2%><br>
                <%=akt.GegnerLastStrasse%><br>
                <%=akt.GegnerLastZipPrefix%>-<%=akt.GegnerLastZip%>&nbsp;<%=akt.GegnerLastOrt%><br>
                <p align="right" class="docText">
                    <br>
                    <%=akt.GegnerLastOrt%>, am _______________________</p>
                <%=akt.AuftraggeberName1%>
                <br>
                <%=akt.AuftraggeberName2%><br>
                <%=akt.AuftraggeberStrasse%><br>
                <%=akt.AuftraggeberLKZ%>-<%=akt.AuftraggeberPLZ%>&nbsp;<%=akt.AuftraggeberOrt%><br>
                <p align="center" class="Stil">
                    <strong>ANERKENNTNIS - RATENANSUCHEN - STUNDUNG</strong></p>
                <p align="center">
                    Aktenzeichen -
                    <%=akt.AktIntAZ%><br>
                    Forderung von -
                    <%=akt.KlientName1%></p>
                Sehr geehrte Damen und Herren,<br>
                da ich die Gesamtforderung aus dem Schreiben vom
                <%=DateTime.Now.ToShortDateString()%>
                in der H&ouml;he von
                <% = HTBUtilities.HTBUtils.FormatCurrencyNumber(pa.GetTotal())%>
                EUR<br>
                innerhalb der gesetzten Frist nicht bezahlen kann, unterbreite ich folgenden Vorschlag.<br>
                Ich anerkenne hiermit diese Forderung inkl. der aufgeschl&uuml;sselten Kosten lt. Beilage, sowie der<br>
                aufgelaufenen Zinsen und ersuche um Zustimmung zur nachstehenden Ratenzahlung bzw. Stundung:<br>
                <p>
                    <strong>A</strong> - monatliche und p&uuml;nktliche Ratenzahlung in der H&ouml;he von _______________ EUR<br>
                    mit dem Zahlungsbeginn der 1.Rate am _______________ (Datum).<br>
                    Eine Anzahlung &uuml;ber _______________ EUR wurden per heutigem Datum an
                    <br>
                    Herrn/Frau
                    <%=akt.UserVorname%>&nbsp;<%=akt.UserNachname%>
                    (Partner/in der Firma E.C.P.) gegen Aush&auml;ndigung einer Quittung bezahlt.</p>
                <p>
                    <strong>B</strong> - Stundung der gesamten Forderung bis zum _______________ (Datum).</p>
                <p align="center">
                    Diese Vereinbarung tritt vorbehaltlich der Zustimmung des Klienten in Kraft. Zahlungen werden vorerst auf Kosten und Zinsen verrechnet. Im Falle des Verzuges mit auch nur einer Rate von mindestens 6 Wochen
                    und Nachfristsetzung von 2 Wochen, ist die gesamte Forderung f&auml;llig (Terminverlust).</p>
                <strong>SELBSTAUSKUNFT</strong> (<em><strong>pers&ouml;nliche Daten </strong></em>)<br>
                <table width="100%" border="3" cellspacing="0" cellpadding="2">
                    <tr>
                        <td width="47%">
                            <div align="center">
                                <em><strong>Zahlungspflichtiger</strong></em></div>
                        </td>
                        <td width="53%">
                            <div align="center">
                                <em><strong>Ehepartner/Lebensgef&auml;hrte/B&uuml;rge</strong></em></div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Vor- und Zuname:
                        </td>
                        <td>
                            Vor- und Zuname:
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Anschrift:
                        </td>
                        <td>
                            Anschrift:
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Telefon:
                        </td>
                        <td>
                            Telefon:
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Geburtsdatum:
                        </td>
                        <td>
                            Geburtsdatum:
                        </td>
                    </tr>
                    <tr>
                        <td>
                            SVA Nummer:
                        </td>
                        <td>
                            SVA Nummer:
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Beruf:
                        </td>
                        <td>
                            Beruf:
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Arbeitgeber:
                        </td>
                        <td>
                            Arbeitgeber:
                        </td>
                    </tr>
                </table>
                <br>
                <p>
                    Ich erkläre hiermit die derzeitige Forderung (zzgl. weitere Zinsen und CollectionInvoicekosten) ausdrücklich und unwiderruflich anzuerkennen und verpflichte mich die Gesamtforderung zu bezahlen. Ich habe die aufgedruckten
                    Bedingungen zur G&auml;nze gelesen und anerkenne diese vollinhaltlich. Damit mein Ansuchen bearbeitet werden kann, bestätige ich mit meiner Unterschrift, dass meine Angaben wahrheitsgemäß und vollständig
                    sind.<br>
                </p>
                <p>
                    Mit freundlichen Gr&uuml;&szlig;en,<br>
                    <%=akt.GegnerLastName1%>&nbsp;<%=akt.GegnerLastName2%>
                    <p align="right" class="docText">
                        Unterschrift: Ehepartner/Lebensgefährte/Bürge
                    </p>
                    <%
                        //no pagebreake on the last sheet of the print
                        if (i < aktenList.Count - 1)
                        {
                    %>
                    <br style="page-break-before: always;">
                    <%
                        }
                //RV end
                //end select
                break;
                }
                }
                //HINTERLEGUNG
                    %>
                    <% if (akt.AuftraggeberHinterlegung == 1)
                       { 
                    %>
                    <br style="page-break-before: always;">
                    <%=akt.GegnerLastName1%>&nbsp;<%=akt.GegnerLastName2%><br>
                    <%=akt.GegnerLastStrasse%><br>
                    <%=akt.GegnerLastZipPrefix%>&nbsp;
                    <%=akt.GegnerLastZip%>
                    &nbsp;
                    <%=akt.GegnerLastOrt%><br>
                    <br>
                    <br>
                    <br>
                    <br>
                </p>
                <table width="100%" border="0" cellpadding="5" cellspacing="0" class="docText">
                    <tr>
                        <td>
                            Auftragsnummer
                        </td>
                        <td>
                            Nummer
                        </td>
                        <td>
                            Kontakt
                        </td>
                        <td>
                            <% = DateTime.Now.ToShortDateString()%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <strong>
                                <%=akt.AktIntAZ%></strong>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <%=akt.AKTIntAGSB%>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>
                <br>
                <br>
                Offene Forderung<br>
                <br>
                wir wurden mit dem CollectionInvoice folgender &uuml;berf&auml;lliger Forderung beauftragt:<br>
                <br>
                <table width="100%" border="0" cellpadding="0" cellspacing="2" class="couriertext">
                    <tr>
                        <td colspan="5">
                            <strong>Forderungsaufstellung:</strong>
                        </td>
                    </tr>
                    <%
                        foreach (AktIntForderungPrintLine pf in pa.ForderungList)
                        {
                    %>
                    <tr>
                        <td width="40">
                            &nbsp;
                        </td>
                        <td width="300">
                            <%=pf.Text %>
                        </td>
                        <td>
                            EUR
                        </td>
                        <td>
                            <div align="right">
                                <%= HTBUtilities.HTBUtils.FormatCurrencyNumber(pf.Amount)%></div>
                        </td>
                        <td>
                            <div align="right">
                            </div>
                        </td>
                    </tr>
                    <%} %>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            inkl. Bearbeitungsgebühren
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            Zinsen
                        </td>
                        <td>
                            EUR
                        </td>
                        <td>
                            <div align="right">
                                <%=  HTBUtilities.HTBUtils.FormatCurrencyNumber(pa.Zinsen)%></div>
                        </td>
                        <td width="150">
                            <div align="right">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            CollectionInvoicekosten
                        </td>
                        <td>
                            EUR
                        </td>
                        <td>
                            <div align="right">
                                <%= HTBUtilities.HTBUtils.FormatCurrencyNumber(pa.InkassoKosten)%>
                            </div>
                        </td>
                        <td>
                            <div align="right">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            20% MWSt aus Kosten
                        </td>
                        <td>
                            EUR
                        </td>
                        <td>
                            <div align="right">
                                <%= HTBUtilities.HTBUtils.FormatCurrencyNumber(pa.MWS)%>
                            </div>
                        </td>
                        <td>
                            <div align="right">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td colspan="3">
                            <div align="right">
                                ---------------------------------------------------</div>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <strong>Forderung:</strong>
                        </td>
                        <td>
                            <strong>EUR</strong>
                        </td>
                        <td>
                            <div align="right">
                                <%=  HTBUtilities.HTBUtils.FormatCurrencyNumber(pa.GetTotalLessWeggebuhr())%>
                            </div>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            + Weggeb&uuml;hr
                        </td>
                        <td>
                            EUR
                        </td>
                        <td>
                            <div align="right">
                                <%= HTBUtilities.HTBUtils.FormatCurrencyNumber(pa.Weggebuhr)%></div>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td colspan="3">
                            <div align="right">
                                ---------------------------------------------------</div>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <strong>Endsumme (inkl. Weggeb&uuml;hr)</strong>
                        </td>
                        <td>
                            <strong>EUR</strong>
                        </td>
                        <td>
                            <div align="right">
                                <%= HTBUtilities.HTBUtils.FormatCurrencyNumber(pa.GetTotal())%>
                            </div>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td colspan="3">
                            <div align="right">
                                <strong>=================================================== </strong>
                            </div>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>
                <p align="left">
                    <br>
                    <br>
                    <br>
                    Wir bitten Sie diesen Betrag dem &Uuml;berbringer dieses Schreibens gegen seperate Quittung s o f o r t zu bezahlen.
                    <br>
                    <br>
                    Begleichen Sie Ihre offene Schuld, Sie vermeiden damit h&ouml;here Kosten und weitere Kosequenzen.<br>
                    Vereinbarungsgem&auml;ss wird mit der ersten Zahlung die Weggeb&uuml;hr wie oben angef&uuml;hrt in Abzug gebracht.
                    <br>
                    Gleichzeitig setzen wir Sie, als Auftraggeber nach den Bestimmungen des Datenschutzgesetzes, davon in Kenntnis, dass wir zu Zwecken des Gl&auml;ubigerschutzes und der Bonit&auml;tsbeurteilung die ermitttelten
                    Daten in Anwendung bringen werden.<br>
                    <br>
                    <br>
                    Freundliche Gr&uuml;&szlig;e<br>
                    <br>
                    <%=akt.AuftraggeberName1%><br>
                    <%=akt.AuftraggeberName2%><br>
                    <% 
                        }
            //Hillinger global "end if" 
        }
    }
                    %>
</body>
</html>
