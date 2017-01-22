<%@ Page Language="C#" Culture="de-DE" AutoEventWireup="true" CodeBehind="EditAktInk.aspx.cs" Inherits="HTB.v2.intranetx.aktenink.EditAktInk" %>
<%@ Import Namespace="HTBExtras.KingBill" %>

<%@ Register TagPrefix="ctl" TagName="workflow" Src="~/v2/intranetx/global_files/CtlWorkflow.ascx" %>
<%@ Register Src="~/v2/intranetx/global_files/CtlMessage.ascx" TagName="message" TagPrefix="ctl" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>HTB.ASP [ CollectionInvoiceakt editieren ]</title>
    <link href="~/v2/intranet/styles/inksys.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
<!--

body {
	margin-left: 5px;
	margin-top: 5px;
	margin-right: 5px;
	margin-bottom: 5px;
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
.style1 {color: #FF0000}
.style3 {color: #999999}
OPTION.dis{background-color:white; color:#999999}

-->

</style>
    <script language="JavaScript" type="text/JavaScript">
        function MM_openBrWindow(theURL, winName, features) { //v2.0
            window.open(theURL, winName, features);
        }

        function MM_goToURL() { //v3.0
            var i, args = MM_goToURL.arguments; document.MM_returnValue = false;
            for (i = 0; i < (args.length - 1); i += 2) eval(args[i] + ".location='" + args[i + 1] + "'");
        }
    </script>
    <%--<script src="../../intranet/globalcode/lib.js" type="text/javascript" language="javascript"></script>--%>
</head>
<body id="bdy" runat="server">
    <ctl:headerNoMenu ID="header" runat="server" />
    <form runat="server" id="form2" method="post" action="">
    <table width="100%" border="0" cellpadding="1" cellspacing="0" bgcolor="#000000">
        <tr>
            <td>
                <table width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
                    <tr>
                        <td class="smallText">
                            <a href="../../intranet/intranet/intranet.asp">Intranet</a> <a href="../../intranet/intranet/inkasso.asp">CollectionInvoice</a> <a href="../../intranet/aktenink/AktenStaff.asp">
                                CollectionInvoiceakten (&Uuml;bersicht)</a> | CollectionInvoiceakt editieren
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" EnablePartialRendering="true" EnableScriptLocalization="true" />
    <ajax:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0">
        <ajax:TabPanel ID="TabPanel1" runat="server" HeaderText="Haupt Window">
            <ContentTemplate>
                <table width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
                    <tr>
                        <td>
                            <p>&nbsp;</p>
                            <table width="632" border="0" align="center" cellpadding="3" cellspacing="0">
                                <tr>
                                    <td colspan="2" class="tblHeader">
                                        INKASSOAKT Editieren
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" class="EditCaption">
                                        <div align="right">
                                            Aktionen:&nbsp;</div>
                                    </td>
                                    <td class="EditData">
                                        <%if (Permissions.GrantInkassoEdit)
                                          { %>
                                            <a href="#"><img src="../../intranet/images/ic_action16.gif" width="16" height="16" border="0" title="Aktion setzen!" onclick="MM_openBrWindow('../../intranet/aktenink/setInkAction.asp?ID=<%=QryInkassoakt.CustInkAktID%>','popSetAction','menubar=yes,scrollbars=yes,resizable=yes,width=800,height=800')" /></a>
                                        <%}
                                          else
                                          {%>
                                        <img src="../../intranet/images/ic_action16_dis.gif" width="16" height="16" border="0" title="Aktion" />
                                        <% } %>
                                        &nbsp;&nbsp;&nbsp;Bericht senden:
                                        <asp:DropDownList runat="server" ID="ddlSendBericht">
                                            <asp:ListItem Text="Ja" Value="1" />
                                            <asp:ListItem Text="Nein" Value="0" />
                                        </asp:DropDownList>
                                        <ctl:message runat="server" ID="ctlMessage"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="10%" valign="top" class="EditCaption">
                                        <div align="right">
                                            <strong>Akt Nr.&nbsp;</strong>:</div>
                                    </td>
                                    <td class="EditData">
                                        <%=QryInkassoakt.CustInkAktID%>
                                        <%
                                            if (QryInkassoakt.CustInkAktOldID != "")
                                            {
                                                Response.Write("[" + QryInkassoakt.CustInkAktOldID + "]");
                                            }
                                        %>
                                        <br />
                                        <span class="style3">
                                            <%
                                                if (!string.IsNullOrEmpty(QryInkassoakt.CustInkAktGothiaNr))
                                                    Response.Write(QryInkassoakt.CustInkAktGothiaNr);
                                            %>
                                        </span>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="10%" class="EditCaption">
                                        <div align="right">
                                            Akt Anlagedatum:&nbsp;</div>
                                    </td>
                                    <td class="EditData">
                                        <%=QryInkassoakt.CustInkAktEnterDate%>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="10%" valign="top" class="EditCaption">
                                        <div align="right">
                                            Auftraggeber:&nbsp;</div>
                                    </td>
                                    <td class="EditData">
                                        <strong>
                                            <%=QryInkassoakt.AuftraggeberName1%></strong>&nbsp;<%=QryInkassoakt.AuftraggeberName2%><br>
                                        <%=QryInkassoakt.AuftraggeberStrasse%><br>
                                        <%=QryInkassoakt.AuftraggeberLKZ%>&nbsp;<%=QryInkassoakt.AuftraggeberPLZ%>&nbsp;<%=QryInkassoakt.AuftraggeberOrt%>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="10%" valign="top" class="EditCaption">
                                        <div align="right">
                                            Klient:&nbsp;</div>
                                    </td>
                                    <td class="EditData">
                                        <asp:Label ID="lblKlient" runat="server" />
                                        <strong>
                                            <%=QryInkassoakt.KlientName1%></strong>&nbsp;<%=QryInkassoakt.KlientName2%><br>
                                        <%=QryInkassoakt.KlientStrasse%><br>
                                        <%=QryInkassoakt.KlientLKZ%>&nbsp;<%=QryInkassoakt.KlientPLZ%>&nbsp;<%=QryInkassoakt.KlientOrt%>
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
                                        <p>
                                            &nbsp;</p>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="10%" class="EditCaption">
                                        <div align="right">
                                            <strong>Kundennummer</strong>:&nbsp;</div>
                                    </td>
                                    <td class="EditData">
                                        <asp:Label ID="lblCustInkAktGothiaNr" runat="server" />
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
                                            Rechnungsdatum:&nbsp;</div>
                                    </td>
                                    <td class="EditData">
                                        <%=QryInkassoakt.CustInkAktInvoiceDate.ToShortDateString()%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="EditCaption">
                                        <div align="right">
                                            Akt&nbsp;Workflow&nbsp;Datum:&nbsp;</div>
                                    </td>
                                    <td class="EditData">
                                        <%=QryInkassoakt.CustInkAktNextWFLStep.ToShortDateString()%>
                                    </td>
                                </tr>
                                <tr runat="server" ID="trAussendienst" Visible="false">
                                    <td width="10%" class="EditCaption">
                                        <div align="right">
                                            Aussendienst:&nbsp;</div>
                                    </td>
                                    <td class="EditData">
                                        <asp:Label ID="lblAussendienst" runat="server" />
                                    </td>
                                </tr>
                                <tr runat="server" ID="trInterventionStatus" Visible="false">
                                    <td width="10%" class="EditCaption">
                                        <div align="right">
                                            Interventionsstatus:&nbsp;</div>
                                    </td>
                                    <td class="EditData">
                                        <asp:Label ID="lblInterventionStatus" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="EditCaption">
                                        <div align="right">Rechtsanwalt:</div>
                                    </td>
                                    <td class="EditData">
                                        <asp:DropDownList ID="ddlLawyer" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlLawyer_SelectedindexChagned"/>
                                        &nbsp;&nbsp;
                                        <asp:Label runat="server" ID="lblLawyerInfo" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" class="EditCaption">
                                        Klient Sachbearbeiter:
                                    </td>
                                    <td class="EditData">
                                        <span class="docText">
                                            <asp:DropDownList runat="server" ID="ddlClientSB"/>
                                        </span>
                                    </td>
                                </tr>
                                <asp:HiddenField ID="MM_update" runat="server" Value="form1" />
                                <asp:HiddenField ID="MM_recordId" runat="server" />
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
                                                <td class="tblHeader" title="Betragsübersicht">
                                                    <a href="javascript:show('btnDownBUebersicht'); hide('btnUpBUebersicht'); hide('BUebersicht'); setCookie('cBUebersicht',0,3);">
                                                        <img src="../../intranet/images/boxup2.gif" width="9" height="12" border="0" id="btnUpBUebersicht" style="display: inline;" /></a> <a href="javascript:show('btnUpBUebersicht'); hide('btnDownBUebersicht'); show('BUebersicht'); setCookie('cBUebersicht',1,3);">
                                                            <img src="../../intranet/images/boxdown2.gif" width="9" height="12" border="0" id="btnDownBUebersicht" style="display: none;" /></a> &nbsp;Betragsübersicht
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
                                                            <td class="tblData1">
                                                                &nbsp;
                                                            </td>
                                                            <td class="tblData2">
                                                                <strong>Zinsen</strong>
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
                                                                    <asp:Label ID="lblZinsen" runat="server" /></div>
                                                            </td>
                                                            <td class="tblData3">
                                                                <div align="right">
                                                                    &nbsp;<asp:Label ID="lblZinsenZahlungen" runat="server" /></div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="tblData1">
                                                                &nbsp;
                                                            </td>
                                                            <td class="tblData2">
                                                                <strong>ECP Zinsen</strong>
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
                                                                    <asp:Label ID="lblEcpZinsen" runat="server" /></div>
                                                            </td>
                                                            <td class="tblData3">
                                                                <div align="right">
                                                                    &nbsp;<asp:Label ID="lblEcpZinsenZahlungen" runat="server" /></div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="tblData1">
                                                                &nbsp;
                                                            </td>
                                                            <td class="tblData2">
                                                                <strong>Kosten</strong> (E.C.P.)
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
                                                                    <asp:Label ID="lblKostenSumme" runat="server" /></div>
                                                            </td>
                                                            <td class="tblData3">
                                                                <div align="right">
                                                                    &nbsp;<asp:Label ID="lblKostenZahlungen" runat="server" /></div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="tblData1">
                                                                &nbsp;
                                                            </td>
                                                            <td colspan="4" class="tblData2">
                                                                <hr>
                                                            </td>
                                                            <td class="tblData3">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="tblData1">
                                                                &nbsp;
                                                            </td>
                                                            <td class="tblData2">
                                                                &nbsp;
                                                            </td>
                                                            <td class="tblData2">
                                                                <div align="center">
                                                                    <strong>EUR</strong></div>
                                                            </td>
                                                            <td class="tblData2">
                                                                <div align="center">
                                                                    =</div>
                                                            </td>
                                                            <td class="tblData2">
                                                                <div align="right">
                                                                    <strong>
                                                                        <asp:Label ID="lblSumGesFortBUE" runat="server" /></strong></div>
                                                            </td>
                                                            <td class="tblData3">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="tblData1">
                                                                &nbsp;
                                                            </td>
                                                            <td class="tblData2">
                                                                <em>abz&uuml;gl. Zahlungen bis Dato</em>
                                                            </td>
                                                            <td class="tblData2">
                                                                <div align="center">
                                                                    EUR</div>
                                                            </td>
                                                            <td class="tblData2">
                                                                <div align="center">
                                                                    <strong>-</strong></div>
                                                            </td>
                                                            <td class="tblData2">
                                                                <div align="right">
                                                                    <asp:Label ID="lblSumGesZahlungenBUE" runat="server" /></div>
                                                            </td>
                                                            <td class="tblData3">
                                                                <div align="right">
                                                                    <asp:Label ID="lblUnappliedPay" runat="server" ForeColor="Green" /></div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="tblData1">
                                                                &nbsp;
                                                            </td>
                                                            <td colspan="4" class="tblData2">
                                                                <hr>
                                                            </td>
                                                            <td class="tblData3">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="tblData1">
                                                                &nbsp;
                                                            </td>
                                                            <td class="tblData2">
                                                                <strong class="style1">Saldo</strong> (per Dato)
                                                            </td>
                                                            <td class="tblData2">
                                                                <div align="center">
                                                                    <strong>EUR</strong></div>
                                                            </td>
                                                            <td class="tblData2">
                                                                <div align="center">
                                                                    <strong>=</strong></div>
                                                            </td>
                                                            <td class="tblData2">
                                                                <div align="right" class="style1">
                                                                    <strong>
                                                                        <asp:Label ID="lblSaldo" runat="server" /></strong></div>
                                                            </td>
                                                            <td class="tblData3">
                                                                &nbsp;
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
                                                <td class="tblHeader" title="Buchungen">
                                                    <a href="javascript:show('btnDownBuchung'); hide('btnUpBuchung'); hide('Buchung'); setCookie('cBuch',0,3)">
                                                        <img src="../../intranet/images/boxup2.gif" width="9" height="12" border="0" id="btnUpBuchung" style="display: inline;"></a> <a href="javascript:show('btnUpBuchung'); hide('btnDownBuchung'); show('Buchung'); setCookie('cBuch',1,3);">
                                                            <img src="../../intranet/images/boxdown2.gif" width="9" height="12" border="0" id="btnDownBuchung" style="display: none;"></a> &nbsp;Buchungen
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <table width="100%" border="0" cellspacing="0" cellpadding="3" id="Buchung">
                                                        <tr>
                                                            <td class="tblData1">
                                                                <asp:GridView ID="gvInvoices" runat="server" AutoGenerateColumns="False" CellPadding="2" BorderStyle="Groove" Width="100%" ShowFooter="True">
                                                                    <Columns>
                                                                        <asp:TemplateField>
                                                                            <ItemTemplate>
                                                                                <%# Eval("DeleteUrl")%>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField>
                                                                            <ItemTemplate>
                                                                                <%# Eval("EditUrl")%>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:HyperLinkField HeaderText="Nr." DataTextField="InvoiceID" DataNavigateUrlFields="InvoiceID" DataTextFormatString="&lt;a href=javascript:MM_openBrWindow('/v2/intranetx/aktenink/ShowInvoice.aspx?InvId={0}','popSetAction','menubar=yes,scrollbars=yes,resizable=yes,width=800,height=800,top=10');&gt;{0}&lt;/a&gt;"
                                                                            SortExpression="InvoiceID">
                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                        </asp:HyperLinkField>
                                                                        <asp:BoundField HeaderText="Datum" DataField="InvoiceDate" SortExpression="InvoiceDate">
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField HeaderText="Text" DataField="InvoiceDescription" SortExpression="InvoiceDescription" FooterText="Forderung:" HtmlEncode="False">
                                                                            <FooterStyle Font-Bold="True" HorizontalAlign="Right" Wrap="False" />
                                                                            <ItemStyle Wrap="False" />
                                                                        </asp:BoundField>
                                                                        <asp:TemplateField HeaderText="Betrag">
                                                                            <ItemTemplate>
                                                                                <%# Eval("InvoiceAmount") %>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <%# HTBUtilities.HTBUtils.FormatCurrency(GetTotalDue()) %>
                                                                            </FooterTemplate>
                                                                            <FooterStyle Font-Bold="True" HorizontalAlign="Right" Wrap="False" />
                                                                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Unapplied<br/>Zahlung">
                                                                            <ItemTemplate>
                                                                                <%# Eval("UnappliedPay")%>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <%# GetTotalUappliedPay()%>
                                                                            </FooterTemplate>
                                                                            <FooterStyle Font-Bold="True" HorizontalAlign="Right" ForeColor="Green" />
                                                                            <ItemStyle HorizontalAlign="Right" ForeColor="Green" />
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField HeaderText="Fällig" DataField="DueDate" SortExpression="DueDate">
                                                                            <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField HeaderText="Überweisen<br/>am" DataField="TransferDate" SortExpression="TransferDate" HtmlEncode="False">
                                                                            <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField HeaderText="Überweisen<br/>Betrag" DataField="TransferAmount" SortExpression="TransferAmount" HtmlEncode="False">
                                                                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                                                        </asp:BoundField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </td>
                                                        </tr>
                                                        <%
                                                            //if (permissions.GrantCollectionInvoiceBuchungNew)
                                                            { %>
                                                        <tr>
                                                            <td class="tblData1">
                                                                <div align="right">
                                                                    <strong>
                                                                        <asp:LinkButton runat="server" ID="lnkInterestDetail" OnClick="lnkInterestDetail_Click">Zinsendetail</asp:LinkButton></strong> <strong>
                                                                            <asp:LinkButton runat="server" ID="lnkInterestSummary" OnClick="lnkInterestSummary_Click">Zinsens&uuml;berblick</asp:LinkButton></strong> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                    <strong><a href="javascript:MM_openBrWindow('../global_forms/Zwischenbericht.aspx?ID=<%=Request.QueryString["ID"] %>','popSetAction','menubar=yes,scrollbars=yes,resizable=yes,width=800,height=800,top=10,screenY=10')">
                                                                        Zwischenbericht Drucken</a></strong> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <strong><a href="javascript:MM_openBrWindow('../global_forms/Forderungsaufstellung.aspx?ID=<%=Request.QueryString["ID"] %>','popSetAction','menubar=yes,scrollbars=yes,resizable=yes,width=800,height=800,top=10,screenY=10')">
                                                                            Brief Drucken</a></strong> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <strong><a href="javascript:MM_openBrWindow('AktInkPayment.aspx?ID=<%=Request.QueryString["ID"] %>','popSetAction','menubar=yes,scrollbars=yes,resizable=yes,width=800,height=800,top=10,screenY=10')">
                                                                                Neue Buchung</a></strong>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <%} %>
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
                                <tr id="trRaten" runat="server">
                                    <td colspan="4" class="EditCaptionTopLine3" runat="server">
                                        <table width="100%" border="1" cellpadding="3" cellspacing="0">
                                            <tr>
                                                <td class="tblHeader" title="Ratenvereinbarung">
                                                    <a href="javascript:show('btnDownBRaten'); hide('btnUpBRaten'); hide('BRaten'); setCookie('cBRaten',0,3);">
                                                        <img src="../../intranet/images/boxup2.gif" width="9" height="12" border="0" id="btnUpBRaten" style="display: inline;"></a> <a href="javascript:show('btnUpBRaten'); hide('btnDownBRaten'); show('BRaten'); setCookie('cBRaten',1,3);">
                                                            <img src="../../intranet/images/boxdown2.gif" width="9" height="12" border="0" id="btnDownBRaten" style="display: none;"></a> &nbsp;Ratenvereinbarung
                                                </td>
                                            </tr>
                                            <tr>
                                                <td title="Ratenvereinbarung" class="tblData1">
                                                    <table width="100%" border="0" cellspacing="0" cellpadding="3" id="BRaten">
                                                        <tr>
                                                            <td>
                                                                <asp:GridView ID="gvInstallments" runat="server" AutoGenerateColumns="False" HtmlEncode="false" CellPadding="2" CellSpacing="2" BorderStyle="Inset" Width="100%">
                                                                    <Columns>
                                                                        <asp:TemplateField>
                                                                            <ItemTemplate>
                                                                                <%# Eval("PostponePopupURL") %>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField HeaderText="Rate Datum" DataField="RateDueDate" SortExpression="RateDueDate">
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField HeaderText="Betrag" DataField="RateAmount" SortExpression="RateAmount" HtmlEncode="False">
                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField HeaderText="Zahlung Datum" DataField="ReceivedDate" SortExpression="ReceivedDate">
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField HeaderText="Zahlung Betrag" DataField="ReceivedAmount" SortExpression="ReceivedAmount" HtmlEncode="False">
                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField HeaderText="Offen" DataField="RateBalance" SortExpression="RateBalance" HtmlEncode="False">
                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField HeaderText="Verschoben bis" DataField="PostponeTillDate" SortExpression="PostponeTillDate">
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField HeaderText="Verschiebungsgrund" DataField="PostponeReason" SortExpression="PostponeReason" HtmlEncode="False" />
                                                                        <asp:BoundField HeaderText="Verschoben vom" DataField="PostponeBy" SortExpression="PostponeBy" HtmlEncode="False" />
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <div align="right">
                                                        <asp:LinkButton runat="server" OnClick="btnDeleteInstallmentPlan_Click" OnClientClick="return confirm('Wollen Sie diese Ratenvereinbarung wirklich l&ouml;schen?');">
                                                            <img style="BORDER-LEFT-WIDTH: 0px; BORDER-RIGHT-WIDTH: 0px; BORDER-TOP-COLOR: white; BORDER-BOTTOM-WIDTH: 0px; BORDER-LEFT-COLOR: white; BORDER-BOTTOM-COLOR: white; BORDER-RIGHT-COLOR: white; BORDER-TOP-WIDTH: 0px" alt="L&ouml;scht diese Ratenvereinbarung." src="../../intranet/images/delete2hover.gif" width="16" height="16"/>
                                                        </asp:LinkButton>
                                                    </div>
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
                                                <td class="tblHeader" title="Akt-Aktionen">
                                                    <a href="javascript:show('btnDownAktAktion'); hide('btnUpAktAktion'); hide('AktAktionen'); setCookie('cAktion',0,3);">
                                                        <img src="../../intranet/images/boxup2.gif" width="9" height="12" border="0" id="btnUpAktAktion" style="display: inline;" alt="" /></a><a href="javascript:show('btnUpAktAktion'); hide('btnDownAktAktion'); show('AktAktionen'); setCookie('cAktion',1,3);"><img
                                                            src="../../intranet/images/boxdown2.gif" width="9" height="12" border="0" id="btnDownAktAktion" style="display: none;" alt="" /></a>&nbsp;&nbsp;Akt-Aktionen
                                                </td>
                                            </tr>
                                            <tr>
                                                <td title="Akt-Aktionen">
                                                    <table id="AktAktionen" border="0" cellpadding="3" cellspacing="0" width="100%">
                                                        <tr>
                                                            <td class="tblCol1Sel" colspan="2">
                                                                &nbsp;
                                                            </td>
                                                            <td class="tblCol2Sel">
                                                                Datum
                                                            </td>
                                                            <td class="tblCol2Sel">
                                                                Typ
                                                            </td>
                                                            <td class="tblCol2Sel">
                                                                Aktion
                                                            </td>
                                                            <td class="tblCol2Sel">
                                                                Beleg
                                                            </td>
                                                            <td class="tblCol2Sel">
                                                                <div align="right">
                                                                    Erfolgshonorar</div>
                                                            </td>
                                                            <td class="tblCol2Sel">
                                                                <div align="right">
                                                                    Provision</div>
                                                            </td>
                                                            <td class="tblCol3Sel">
                                                                Betrag
                                                            </td>
                                                        </tr>
                                                        <% 
                                                            foreach (InkassoActionRecord aktion in AktionsList)
                                                            {
                                                        %>
                                                        <tr>
                                                            <td align="center" class="tblData2" valign="middle" width="20">
                                                                <%if (Permissions.GrantInkassoAktAktionDel && aktion.IsInkassoAction)
                                                                  { %><a href="javascript:void(window.open('<%=aktion.DeleteActionURL %>','_blank','toolbar=no,menubar=no'))">
                                                                      <asp:Image ID="img" runat="server" BorderColor="White" ImageUrl="../../intranet/images/delete2hover.gif" />
                                                                  </a>
                                                                <%}
                                                                  else
                                                                  { %>
                                                                <img src="../../intranet/images/delete2hover_dis.gif" width="16" height="16" alt="L&ouml;scht diesen Datensatz." />
                                                                <%} %>
                                                            </td>
                                                            <td align="center" class="tblData2" valign="middle" width="20">
                                                                <%if (Permissions.GrantInkassoAktAktionEdit && aktion.IsInkassoAction)
                                                                  { %><a href="javascript:void(window.open('<%=aktion.EditActionURL %>','_blank','toolbar=no,menubar=no'))">
                                                                      <asp:Image ID="Image1" runat="server" BorderColor="White" ImageUrl="../../intranet/images/edit.gif" />
                                                                  </a>
                                                                <%}
                                                                  else
                                                                  { %>
                                                                <img src="../../intranet/images/edit_dis.gif" width="16" height="16" alt="&Auml;ndert diesen Datensatz." />
                                                                <%} %>
                                                            </td>
                                                            <td class="tblData2">
                                                                <%= ((aktion.ActionDate != HTBUtilities.HTBUtils.DefaultDate) ? aktion.ActionDate.ToShortDateString() : "&nbsp;") + 
                                                                    (string.IsNullOrEmpty(aktion.ActionUser) ? "" : "<br>" + aktion.ActionUser) %>
                                                            </td>
                                                            <td class="tblData2">
                                                                <strong>[<%=aktion.ActionType%>]</strong>
                                                            </td>
                                                            <td class="tblData3">
                                                                <%=aktion.ActionCaption %>&nbsp;
                                                            </td>
                                                            <td class="tblData3">
                                                                <%=aktion.ActionBeleg %>&nbsp;
                                                            </td>
                                                            <td class="tblData3">
                                                                <%=aktion.ActionHonorar %>&nbsp;
                                                            </td>
                                                            <td class="tblData3">
                                                                <%=aktion.ActionProvision %>&nbsp;
                                                            </td>
                                                            <td class="tblData3">
                                                                <%=aktion.ActionBetrag%>&nbsp;
                                                            </td>
                                                        </tr>
                                                        <%if (!aktion.ActionMemo.Equals(string.Empty))
                                                          {%>
                                                        <tr>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td class="tblData3" colspan="7">
                                                                <%=aktion.ActionMemo%>&nbsp;
                                                            </td>
                                                        </tr>
                                                        <%
                                                            }
                                                            }
                                                        %>
                                                        <tr>
                                                            <td class="tblData3" colspan="9">
                                                                <br />
                                                                <strong><i>CollectionInvoicememo:</i></strong><br />
                                                                <asp:TextBox ID="txtMemo" runat="server" class="docText" Columns="80" onBlur="this.style.backgroundColor='';" onFocus="this.style.backgroundColor='#DFF4FF';" Rows="6"
                                                                    TextMode="MultiLine"></asp:TextBox>
                                                                <br />
                                                                <input class="btnStandard" name="Submit3" onclick='GBInsertTimeStamp(document.forms[0].TabContainer1_TabPanel1_txtMemo,&#039;<%=Session["MM_VorName"] + " " + Session["MM_NachName"] + " " + DateTime.Now %>&#039;);'
                                                                    title="Hier können Sie den Zeitstempel setzten." type="button" value="Zeitstempel"> </input>
                                                            </td>
                                                        </tr>
                                                        <%
                                                            if (HasIntervention())
                                                            {
                                                        %>
                                                        <tr>
                                                            <td class="tblData3" colspan="9">
                                                                <br />
                                                                <strong><i>Interventionsmemo:</i></strong><br />
                                                                <%=GetInverventionURL() %>&nbsp;<br />
                                                                <br />
                                                                <%=GetInterventionMemo() %>&nbsp;
                                                            </td>
                                                        </tr>
                                                        <%} %>

                                                        <%
                                                            if (HasMelde())
                                                            {
                                                        %>
                                                        <tr>
                                                            <td class="tblData3" colspan="9">
                                                                <br />
                                                                <strong><i>Meldebericht:</i></strong><br />
                                                                <%=GetMeldeResult()%>&nbsp;
                                                            </td>
                                                        </tr>
                                                        <%} %>

                                                        <tr>
                                                            <td colspan="10">
                                                                <div align="right">
                                                                    <%if (Permissions.GrantInkassoEdit)
                                                                      { %><a href="#">
                                                                          <img src="../../intranet/images/ic_action16.gif" width="16" height="16" border="0" title="Aktion setzen!" onclick="MM_openBrWindow('../../intranet/aktenink/setInkAction.asp?ID=<%=QryInkassoakt.CustInkAktID%>','popSetAction','menubar=yes,scrollbars=yes,resizable=yes,width=800,height=800')" />
                                                                      </a>
                                                                    <%}
                                                                      else
                                                                      {%>
                                                                    <img src="../../intranet/images/ic_action16_dis.gif" width="16" height="16" border="0" title="Aktion" />
                                                                    <% } %><%--&nbsp;&nbsp;&nbsp;<input name="Submit4" type="button" class="smallText" title="Hier k&ouml;nnen Sie eine neue Akt Aktion anlegen." onclick="MM_openBrWindow('<%=HTB.session_transfer.SessionTransfer.GetAspLink(Request, "aktenink/newaktion.asp", "") %>','newAktion','menubar=yes,scrollbars=yes,resizable=yes,width=700,height=500')"
                                                                        value="Neue Akt Aktion anlegen..." />--%>
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
                                        <img alt="" height="1" name="" src="" width="1"> </img>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="EditCaptionTopLine3" colspan="4">
                                        <table border="1" cellpadding="3" cellspacing="0" width="100%">
                                            <tr>
                                                <td class="tblHeader" title="Dokumente">
                                                    <% if (Request.Cookies["cDok"] != null && Request.Cookies["cDok"].ToString() == "0")
                                                       {%><a href="javascript:show('btnDowndoks'); hide('btnUpdoks'); hide('doks'); setCookie('cDok',0,3);">
                                                           <img id="btnUpdoks" border="0" height="12" src="../../intranet/images/boxup2.gif" style="display: none;" width="9"></img></a><a href="javascript:show('btnUpdoks'); hide('btnDowndoks'); show('doks'); setCookie('cDok',1,3);"><img
                                                               id="btnDowndoks" border="0" height="12" src="../../intranet/images/boxdown2.gif" style="display: inline;" width="9"></img></a>
                                                    <%}
                                                       else
                                                       {%><a href="javascript:show('btnDowndoks'); hide('btnUpdoks'); hide('doks'); setCookie('cDok',0,3);">
                                                           <img id="btnUpdoks" border="0" height="12" src="../../intranet/images/boxup2.gif" style="display: inline;" width="9"></img></a><a href="javascript:show('btnUpdoks'); hide('btnDowndoks'); show('doks'); setCookie('cDok',1,3);"><img
                                                               id="btnDowndoks" border="0" height="12" src="../../intranet/images/boxdown2.gif" style="display: none;" width="9"></img></a>
                                                    <%}%>&nbsp;Dokumente
                                                </td>
                                            </tr>
                                            <tr>
                                                <td title="Dokumente">
                                                    <table id="doks" border="0" cellpadding="3" cellspacing="0" width="100%">
                                                        <tr>
                                                            <td class="tblData1">
                                                                <asp:GridView ID="gvDocs" runat="server" AutoGenerateColumns="False" BorderStyle="Groove" CellPadding="2" ShowFooter="True" Width="100%">
                                                                    <Columns>
                                                                        <asp:TemplateField>
                                                                            <ItemTemplate>
                                                                                <%# Eval("DeleteUrl") %>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField DataField="DokChangeDate" HeaderText="Erfasst" SortExpression="DokChangeDate">
                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="DokTypeCaption" HeaderText="Typ" SortExpression="DokTypeCaption">
                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="DokCaption" HeaderText="Bezeichnung" SortExpression="DokCaption">
                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="DokUser" HeaderText="Benutzer" SortExpression="DokUser">
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
                                                                    <asp:Button ID="btnMahnung" runat="server" OnClick="btnMahnung_Click" Text="Neue Mahnung..." title="Hier können Sie ein neue Mahnung anlegen." />
                                                                    <input <% if (!Permissions.GrantInkassoDokumentNew) {%> disabled <%}%> class="smallText" name="Submit42" onclick="MM_openBrWindow('../../intranet/documents/newInkAktDok.asp?ADAktTyp=1&amp;AktID=<%=AktId %>&amp;GegnerID=<%=QryInkassoakt.CustInkAktGegner %>&amp;KlientID=<%=QryInkassoakt.KlientID %>','newDok','menubar=yes,scrollbars=yes,resizable=yes,width=700,height=700')"
                                                                        title="Hier können Sie ein neues Dokument anlegen." type="button" value="Neues Dokument anlegen..."> </input>
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
                                        <img alt="" height="1" name="" src="" width="1"> </img>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="EditCaptionTopLine3" colspan="4">
                                        <table border="1" cellpadding="3" cellspacing="0" width="100%">
                                            <tr>
                                                <td runat="server" id="tdShowDelete" class="tblHeader">
                                                    <asp:button runat="server" ID="btnShowDelete" CssClass="btnCancel" Text="Akt l&ouml;schen" OnClick="btnShowDelete_Click"/>

                                                </td>
                                                <td class="tblHeader" colspan="4">
                                                    <div align="right">
                                                        <asp:Button ID="btnSave" runat="server" class="btnSave" OnClick="btnSubmit_Click" Text="Speichern" title="Speichern" />
                                                        &nbsp;
                                                        <asp:Button ID="btnCancel" runat="server" class="btnCancel" OnClick="btnCancel_Click" Text="Abbrechen" title="Abbrechen" />
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr runat="server" id="trDelete" Visible="false">
                                    <td class="EditCaptionTopLine3" colspan="4">
                                    <table border="1" cellpadding="3" cellspacing="0" width="100%">
                                        <tr>
                                            <td class="warningMsg" colspan="2"><div class="warningText">Achtung! Wenn Sie den Button anklicken, wird der Akt entg&uuml;ltig gel&ouml;scht.</div> </td>
                                        </tr>
                                        <tr>
                                            <td class="tblHeader">
                                                <asp:button runat="server" ID="btnDelete" CssClass="btnCancel" Text="Akt endg&uuml;ltig l&ouml;schen" OnClick="btnDelete_Click"/>
                                            </td>
                                            <td class="tblHeader">
                                                <div align="right">
                                                    <asp:Button ID="btnCancelDelete" runat="server" class="btnSave" OnClick="btnCancelDelete_Click" Text="Nicht l&ouml;schen" title="Akt nicht l&ouml;schen" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                    </td>
                                </tr>
                                <caption>
                                    <p>
                                        &nbsp;</p>
                                </caption>
                    
                        </table>
                        </td> 
                    </tr> 
                </table>
            </ContentTemplate>
        </ajax:TabPanel>
        <ajax:TabPanel ID="TabPanel2" runat="server" HeaderText="Workflow">
            <ContentTemplate>
                <table width="100%" border="0" cellspacing="0" cellpadding="1" bgcolor="#FFFFFF" align="center">
                    <tr>
                        <td>
                            <table border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF" align="center">
                                <tr>
                                    <td align="right">
                                        <ctl:workflow runat="server" ID="ctlWorkflow" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF" align="right">
                                            <tr>
                                                <td class="tblHeader">
                                                    <div align="right">
                                                        <asp:Button ID="btnSubmit2" runat="server" class="btnSave" OnClick="btnSubmit_Click" Text="Speichern" title="Speichern" />
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
            </ContentTemplate>
        </ajax:TabPanel>
    </ajax:TabContainer>
    </form>
    <ctl:footer ID="ftr" runat="server" />
</body>
</html>
