<%@ Page Language="C#" Culture="de-DE" AutoEventWireup="true" CodeBehind="WorkAktInt.aspx.cs" Inherits="HTB.v2.intranetx.aktenint.WorkAktInt" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>HTB.ASP [ Intervention buchen ]</title>
    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
    <link href="../../intranet/styles/inksys.css" rel="stylesheet" type="text/css" />
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
    <script src="../../intranet/globalcode/lib.js" type="text/javascript" language="javascript"></script>
</head>
<body>
    <ctl:headerNoMenu ID="hdr" runat="server" />
    <form id="form1" name="form1" runat="server">
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
                        <table width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
                            <tr>
                                <td>
                                    <p>
                                        &nbsp;</p>
                                    <table border="0" align="center" cellpadding="3" cellspacing="0">
                                        <tr>
                                            <td colspan="2" class="tblHeader" title="INTERVENTION">
                                                INTERVENTION&nbsp;<asp:Label ID="lblAktIntAZ" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" class="tblDataAll">
                                                <ctl:message ID="ctlMessage" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">
                                                    Auftraggeber:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:Label ID="lblAuftraggeberName1" runat="server" />&nbsp;<asp:Label ID="lblAuftraggeberName2" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" class="EditCaption">
                                                <div align="right">
                                                    Klient:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:Label ID="lblKlientName1" runat="server" /><br>
                                                <asp:Label ID="lblKlientName2" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" class="EditCaption">
                                                <div align="right">
                                                    Gegner:<br>
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;(ev. neue Adresse bekant?)</div>
                                            </td>
                                            <td class="EditData">
                                                <a href="#" onclick="MM_openBrWindow('../../intranet/gegner/editgegner.asp?pop=true&GegnerID=<%= aktInt.GegnerID %>','popWindow','menubar=yes,scrollbars=yes,resizable=yes,width=900,height=800');return false">
                                                    <asp:Label ID="lblGegnerName1" runat="server" />
                                                    <br>
                                                    <asp:Label ID="lblGegnerName2" runat="server" />
                                                </a>
                                                <br>
                                                <asp:Label ID="lblGegnerLastStrasse" runat="server" /><br>
                                                <asp:Label ID="lblGegnerLastZipPrefix" runat="server" />-<asp:Label ID="lblGegnerLastZip" runat="server" />&nbsp;<asp:Label ID="lblGegnerLastOrt" runat="server" />
                                            </td>
                                        </tr>
                                        <tr id="tr1" runat="server">
                                            <td valign="top" class="EditCaption">
                                                <div align="right">Inkassant:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:Label ID="lblInkassant" runat="server" />
                                            </td>
                                        </tr>
                                        <tr id="trInkassoNumber" runat="server">
                                            <td valign="top" class="EditCaption">
                                                <div align="right">
                                                    CollectionInvoice Akt:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:Label ID="lblInkassoAktNumber" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" nowrap class="EditCaption">
                                                <div align="right">
                                                    Forderungen:</div>
                                            </td>
                                            <td class="EditData">
                                                <table width="100%" border="0" cellspacing="0" cellpadding="4">
                                                    <% 
                                                        foreach (HTB.Database.tblAktenIntPos AktIntPos in posList)
                                                        {
                                                    %>
                                                    <tr>
                                                        <td>
                                                            <%=AktIntPos.AktIntPosCaption%>
                                                        </td>
                                                        <td>
                                                            EUR
                                                        </td>
                                                        <td>
                                                            <div align="right">
                                                                <%= AktIntPos.AktIntPosBetrag.ToString("N2") %></div>
                                                        </td>
                                                    </tr>
                                                    <% 
                                                        }
                                                    %>
                                                    <tr>
                                                        <td>
                                                            Zinsen
                                                        </td>
                                                        <td>
                                                            EUR
                                                        </td>
                                                        <td>
                                                            <div align="right">
                                                                <asp:Label ID="lblAKTIntZinsenBetrag" runat="server" /></div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            CollectionInvoicekosten
                                                        </td>
                                                        <td>
                                                            EUR
                                                        </td>
                                                        <td>
                                                            <div align="right">
                                                                <asp:Label ID="lblAKTIntKosten" runat="server" /></div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            20% MWSt aus Kosten
                                                        </td>
                                                        <td>
                                                            EUR
                                                        </td>
                                                        <td>
                                                            <div align="right">
                                                                <asp:Label ID="lblTax" runat="server" /></div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Zahlungen
                                                        </td>
                                                        <td>
                                                            EUR
                                                        </td>
                                                        <td>
                                                            <div align="right">
                                                                <asp:Label ID="lblZahlung" runat="server" /></div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3">
                                                            <hr>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <strong>Forderungen:</strong>
                                                        </td>
                                                        <td>
                                                            <strong>EUR</strong>
                                                        </td>
                                                        <td>
                                                            <div align="right">
                                                                <strong>
                                                                    <asp:Label ID="lblTotal" runat="server" />
                                                                </strong>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            + Weggeb&uuml;hr:
                                                        </td>
                                                        <td>
                                                            EUR
                                                        </td>
                                                        <td>
                                                            <div align="right">
                                                                <asp:Label ID="lblAktIntWeggebuehr" runat="server" /></div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3">
                                                            <hr>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span class="style1"><strong>Endsumme</strong>:</span>
                                                        </td>
                                                        <td>
                                                            <span class="style1"><strong>EUR</strong></span>
                                                        </td>
                                                        <td>
                                                            <div align="right" class="style1">
                                                                <strong>
                                                                    <asp:Label ID="lblTotalWithtWeggebuehr" runat="server" />
                                                                </strong>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" nowrap class="EditCaption">
                                                <div align="right">
                                                    Aktionen:</div>
                                            </td>
                                            <td class="EditData">
                                                <table width="100%" border="0" cellspacing="0" cellpadding="4">
                                                    <tr>
                                                        <td class="tblCol1Sel">
                                                            &nbsp;
                                                        </td>
                                                        <td class="tblCol2Sel">
                                                            &nbsp;
                                                        </td>
                                                        <td class="tblCol2Sel">
                                                            Datum/Uhrzeit
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
                                                        foreach (HTB.Database.Views.qryInktAktAction AktIntAction in actionsList)
                                                        {
                                                    %>
                                                    <tr>
                                                        <td class="tblData1">
                                                            <a href="#">
                                                                <img src="../../intranet/images/delete.gif" alt="Löscht diese Aktion" width="16" height="16" border="0" 
                                                                    onclick="MM_openBrWindow('../../intranet/global_forms/globaldelete.asp?strTable=tblAktenIntAction&frage=Sind%20Sie%20sicher,%20dass%20sie%20diese%20T&#228;tigkeit%20l&#246;schen%20wollen?&strTextField=AktIntActionID&strColumn=AktIntActionID&ID=<%=AktIntAction.AktIntActionID%>','popWindow','menubar=yes,scrollbars=yes,resizable=yes,width=300,height=300')"></a>
                                                        </td>
                                                        <td class="tblData2">
                                                            <a href="#">
                                                                <img src="../../intranet/images/edit.gif" alt="Ändert diese Aktion" width="16" height="16" border="0" title="&Auml;ndert diesen Datensatz." onclick="MM_openBrWindow('NewAction.aspx?INTID=<%=aktInt.AktIntID%>&AktionID=<%=AktIntAction.AktIntActionID%>&AG=<%=aktInt.AktIntAuftraggeber%>','popWindow','menubar=yes,scrollbars=yes,resizable=yes,width=800,height=600')"></a>
                                                        </td>
                                                        <td class="tblData2">
                                                            <%=AktIntAction.AktIntActionDate.ToShortDateString() %>&nbsp;/&nbsp;<%= AktIntAction.AktIntActionTime.ToShortTimeString() %>
                                                        </td>
                                                        <td class="tblData2">
                                                            <%=AktIntAction.AktIntActionTypeCaption %>
                                                            <%=string.IsNullOrEmpty(AktIntAction.AktIntActionMemo) ? "" : "<BR/> MEMO: " + AktIntAction.AktIntActionMemo.Replace("\n", "<BR/>")%>
                                                        </td>
                                                        <td class="tblData2">
                                                            <%=AktIntAction.AktIntActionBeleg %>&nbsp;
                                                        </td>
                                                        <td class="tblData2">
                                                            <div align="right">
                                                                <%= HTBUtilities.HTBUtils.FormatCurrency(AktIntAction.AktIntActionHonorar) %>
                                                            </div>
                                                        </td>
                                                        <td class="tblData2">
                                                            <div align="right">
                                                                <%= HTBUtilities.HTBUtils.FormatCurrency(AktIntAction.AktIntActionProvision) %>
                                                            </div>
                                                        </td>
                                                        <td class="tblData3">
                                                            <div align="right">
                                                                <%= HTBUtilities.HTBUtils.FormatCurrency(AktIntAction.AktIntActionBetrag) %>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <% 
                                                        }
                                                    %>
                                                    <tr>
                                                        <td>
                                                            <span class="style1"></span>
                                                        </td>
                                                        <td>
                                                            <span class="style1"></span>
                                                        </td>
                                                        <td>
                                                            <span class="style1"></span>
                                                        </td>
                                                        <td class="tblData1">
                                                            <span class="style1"><strong>Summe:</strong></span>
                                                        </td>
                                                        <td class="tblData2 style1">
                                                            &nbsp;
                                                        </td>
                                                        <td class="tblData2">
                                                            <div align="right" class="style1">
                                                                <strong>
                                                                    <asp:Label ID="lblHTotal" runat="server" /></strong></div>
                                                        </td>
                                                        <td class="tblData2">
                                                            <div align="right" class="style1">
                                                                <strong>
                                                                    <asp:Label ID="lblPTotal" runat="server" /></strong></div>
                                                        </td>
                                                        <td class="tblData3">
                                                            <div align="right" class="style1">
                                                                <strong>
                                                                    <asp:Label ID="lblBTotal" runat="server" /></strong></div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="8">
                                                            <div align="right">
                                                                <%if (DateTime.Now.DayOfWeek != DayOfWeek.Monday || (Session["MM_UserLevelLevel"] != null && Session["MM_UserLevelLevel"].ToString() == "255"))
                                                                  {%>
                                                                <%--<input name="Submit3" type="button" class="btnStandard" title="Hier k&ouml;nnen Sie einen Aktion abstellen." onclick="MM_openBrWindow('../../intranet/aktenint/newaktion.asp?AG=<%=aktInt.AktIntAuftraggeber%>&ID=<%=inkAktId%>','popWindow','menubar=yes,scrollbars=yes,resizable=yes,width=800,height=600')"
                                                                    value=" Neue Aktion ">--%>
                                                                <input name="Submit4" type="button" class="btnStandard" title="Hier k&ouml;nnen Sie einen Aktion abstellen." onclick="MM_openBrWindow('NewAction.aspx?AG=<%=aktInt.AktIntAuftraggeber%>&INTID=<%=aktId%>','popWindow','menubar=yes,scrollbars=yes,resizable=yes,width=800,height=600')"
                                                                    value=" Neue Aktion">
                                                                <%}%>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" nowrap class="EditCaption">
                                                <div align="right">
                                                    Ratenvereinbarung:</div>
                                            </td>
                                            <td class="EditData">
                                                &nbsp;
                                                <asp:Panel ID="pnlInterventionRate" runat="server" Visible="false">
                                                    <table width="100%" border="0" cellspacing="0" cellpadding="4">
                                                        <tr>
                                                            <td width="20" class="tblCol1Sel">
                                                                &nbsp;
                                                            </td>
                                                            <td width="20" class="tblCol2Sel">
                                                                &nbsp;
                                                            </td>
                                                            <td class="tblCol2Sel">
                                                                Zahlungsweise:
                                                            </td>
                                                            <td class="tblCol2Sel">
                                                                <div align="left">
                                                                    Betrag:</div>
                                                            </td>
                                                            <td class="tblCol2Sel">
                                                                Beginnend per:
                                                            </td>
                                                            <td class="tblCol2Sel">
                                                                Anzahl Monate:
                                                            </td>
                                                            <td class="tblCol3Sel">
                                                                Jeweils zum:
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="tblData1">
                                                                <div align="center">
                                                                    <a href="#">
                                                                        <img src="../../intranet/images/delete.gif" width="16" height="16" border="0" title="L&ouml;scht diesen Datensatz." onclick="MM_openBrWindow('../../intranet/aktenint/delrv.asp?ID=<%=aktId%>','popWindow','menubar=yes,scrollbars=yes,resizable=yes,width=200,height=200')"></a></div>
                                                            </td>
                                                            <td class="tblData2">
                                                                <div align="center">
                                                                    <a href="#" onclick="MM_openBrWindow('../../intranet/aktenint/newrv.asp?Summe=<%= GetTotalDue()%>&ID=<%=aktId%>','popWindow','menubar=yes,scrollbars=yes,resizable=yes,width=800,height=600')">
                                                                        <img src="../../intranet/images/arrow16.gif" width="16" height="16" border="0" title="Auf diesen Datensatz buchen.">
                                                                    </a>
                                                                </div>
                                                            </td>
                                                            <td class="tblData2">
                                                                <asp:Label ID="lblCollectionType" runat="server" />
                                                            </td>
                                                            <td class="tblData2">
                                                                <asp:Label ID="lblAktIntRVAmmount" runat="server" />&nbsp;
                                                            </td>
                                                            <td class="tblData2">
                                                                <asp:Label ID="lblAktIntRVStartDate" runat="server" />&nbsp;
                                                            </td>
                                                            <td class="tblData2">
                                                                <asp:Label ID="lblAktIntRVNoMonth" runat="server" />&nbsp;
                                                            </td>
                                                            <td class="tblData3">
                                                                <asp:Label ID="lblAktIntRVIntervallDay" runat="server" />.&nbsp;
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <asp:Label ID="lblZahlungsrhythmus" runat="server" />
                                                </asp:Panel>
                                                <asp:Panel ID="pnlInkassoRate" runat="server" Visible="false">
                                                    <asp:Label ID="lblPayType" runat="server" />
                                                    <asp:GridView ID="gvRates" runat="server" AllowSorting="false" AutoGenerateColumns="False" CellPadding="2" BorderStyle="Groove" Width="100%">
                                                        <Columns>
                                                            <asp:BoundField HeaderText="Datum" DataField="InstallmentDate" SortExpression="InstallmentDate" ItemStyle-HorizontalAlign="Center" />
                                                            <asp:BoundField HeaderText="Betrag" DataField="InstallmentAmount" SortExpression="InstallmentAmount" ItemStyle-HorizontalAlign="Right" />
                                                        </Columns>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                &nbsp;
                                            </td>
                                            <td class="EditData">
                                                <%--<asp:Panel ID="pnlBtnNewInstallment" runat="server" Visible="false">
                                                    <input name="Submit3" type="button" class="btnStandard" title="Hier k&ouml;nnen Sie eine Ratenvereinbarung abstellen." onclick="MM_openBrWindow('<%=GetInstallmentLink() %>','popWindow','menubar=yes,scrollbars=yes,resizable=yes,width=800,height=600')"
                                                        value="  Neue Ratenvereinbarung ">
                                                    &nbsp;&nbsp;&nbsp;
                                                </asp:Panel>--%>
                                                &nbsp;
                                                <asp:Panel ID="pnlBtnProtocol" runat="server" Visible="false">
                                                    <input name="btnProtocol" type="button" class="btnStandard" title="Hier k&ouml;nnen Sie einen Protokoll abstellen." onclick="MM_openBrWindow('Protocol2.aspx?AG=<%=aktInt.AktIntAuftraggeber%>&ID=<%=aktId%>','popWindow','menubar=yes,scrollbars=yes,resizable=yes,width=800,height=600')"
                                                        value=" Protokoll ">
                                                </asp:Panel>
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
                                                                                        <a href="javascript:void(window.open('<%# Eval("DeletePopupUrl")%>','_blank','toolbar=no,menubar=no'))">
                                                                                            <asp:Image ID="img" runat="server" ImageUrl='<%# Eval("DeleteUrl")  %>' BorderColor="White" />
                                                                                        </a>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                                                    <ItemTemplate>
                                                                                        <a href="javascript:void(window.open('<%# Eval("EditPopupUrl")%>','_blank','toolbar=no,menubar=no'))">
                                                                                            <asp:Image ID="img" runat="server" ImageUrl='<%# Eval("EditUrl")  %>' BorderColor="White" />
                                                                                        </a>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:HyperLinkField HeaderText="Nr." DataTextField="InvoiceID" DataNavigateUrlFields="InvoiceID" ItemStyle-HorizontalAlign="Right" DataTextFormatString="&lt;a href=javascript:MM_openBrWindow('/v2/intranetx/aktenink/ShowInvoice.aspx?InvId={0}','popSetAction','menubar=yes,scrollbars=yes,resizable=yes,width=800,height=800,top=10');&gt;{0}&lt;/a&gt;"
                                                                                    SortExpression="InvoiceID" />
                                                                                <asp:BoundField HeaderText="Buchungsart" DataField="PosInvoiceTypeCaption" SortExpression="PosInvoiceTypeCaption" ItemStyle-HorizontalAlign="Left" />
                                                                                
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
                                                                            <input name="Submit4" type="button" class="smallText" title="Hier k&ouml;nnen Sie eine neue Buchung abstellen." onclick="MM_openBrWindow('/v2/intranetx/aktenint/EditBooking.aspx?IntAkt=<%=GetAkt().AktIntID %>','popWindow','menubar=yes,scrollbars=yes,resizable=yes,width=800,height=600')"
                                                                                value="Neue Buchung">
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
                                            <td valign="top" nowrap class="EditCaption">
                                                <div align="right">
                                                    Dokumente:</div>
                                            </td>
                                            <td class="EditData">
                                                <table width="100%" border="0" cellspacing="0" cellpadding="4">
                                        </tr>
                                        <tr>
                                            <td class="tblDataAll">
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
                                            <td valign="top" align="right">
                                                <div align="right">
                                                    <input name="Submit6" type="button" class="btnStandard" title="Hier k&ouml;nnen Sie ein neues Dokument anlegen." onclick="MM_openBrWindow('../../intranet/documents/newaktintdok.asp?ADAktTyp=3&AktID=<%=aktId%>&GegnerID=<%=aktInt.GegnerID%>&KlientID=<%=aktInt.KlientID%>','newDok','menubar=yes,scrollbars=yes,resizable=yes,width=800,height=600')"
                                                        value="Neues Dokument">
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="trAktIntStatus" runat="server" visible="false">
                                <td class="EditCaption">
                                    <div align="right">
                                        Neuer Status:</div>
                                </td>
                                <td class="EditData">
                                    <asp:DropDownList ID="ddlNewStatus" runat="server" class="docText">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <%--<tr>
                                <td class="EditCaption">
                                    <div align="right" title="Wom wem wurde di Information erhalten?">Auskumftgeber:</div>
                                </td>
                                <td class="EditData">
                                    <asp:TextBox ID="txtAuskumftgeber" runat="server" class="docText" size="45" onFocus="this.style.backgroundColor='#DFF4FF';" onBlur="this.style.backgroundColor='';" />
                                </td>
                            </tr>
                            <tr>
                                <td class="EditCaption">
                                    <div align="right">Arbeitgeber:</div>
                                </td>
                                <td class="EditData">
                                    <asp:TextBox ID="txtArbeitgeber" runat="server" class="docText" size="45" onFocus="this.style.backgroundColor='#DFF4FF';" onBlur="this.style.backgroundColor='';" />
                                </td>
                            </tr>
                            <tr>
                                <td class="EditCaption">
                                    <div align="right">Gegners Telefon:</div>
                                </td>
                                <td class="EditData">
                                    +
                                    <asp:TextBox runat="server" ID="txtPhoneCountry" class="docText" value="43" size="5" MaxLength="5" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" />
                                    (
                                    <asp:TextBox runat="server" ID="txtPhoneCity" class="docText" size="10" MaxLength="10" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" />
                                    )
                                    <asp:TextBox runat="server" ID="txtPhone" class="docText" size="45" MaxLength="50" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" />
                                </td>
                            </tr>
                            <tr>
                                <td class="EditCaption">
                                    <div align="right">Manatliches Netto Einkommen:</div>
                                </td>
                                <td class="EditData">
                                    <asp:TextBox ID="txtEinkommen" runat="server" class="docText" size="10" onFocus="this.style.backgroundColor='#DFF4FF';" onBlur="this.style.backgroundColor='';" />
                                </td>
                            </tr>
                            <tr>
                                <td class="EditCaption">
                                    <div align="right">Messeverwalter (Schuldnerberatungsstelle):</div>
                                </td>
                                <td class="EditData">
                                    <asp:TextBox ID="txtMesseverwalter" runat="server" class="docText" size="50" onFocus="this.style.backgroundColor='#DFF4FF';" onBlur="this.style.backgroundColor='';" />
                                </td>
                            </tr>--%>
                            <tr>
                                <td width="150" valign="top" class="EditCaption">
                                    <div align="right">
                                        Information zu dieser Intervention (f&uuml;r Klienten sichtbar):</div>
                                </td>
                                <td class="EditData">
                                    <strong>Auftraggeber Memo:</strong>
                                    <br/>
                                    <asp:Label runat="server" ID="lblOriginalMemo"/>
                                    <br/>
                                    <br/>
                                    <asp:TextBox ID="txtMemo" runat="server" TextMode="MultiLine" Columns="80" Rows="6" class="docText" onFocus="this.style.backgroundColor='#DFF4FF';" onBlur="this.style.backgroundColor='';" />
                                    <br/>
                                    <input name="Submit3" type="button" class="btnStandard" value="Zeitstempel" title="Hier können Sie den Zeitstempel setzten." onclick="GBInsertTimeStamp(document.forms(0).txtMemo,'<%=Session["MM_VorName"] + " " + Session["MM_NachName"] + " " + DateTime.Now %>');">
                                    <input name="Submit4" type="button" class="btnStandard" onclick="MM_openBrWindow('../../intranet/aktenint/sendmitteilung2.asp?ID=<%=aktId %>','sendmitteilung2','menubar=no,scrollbars=yes,resizable=no,width=500,height=400')"
                                        value="Zwischenbericht">
                                    <asp:DropDownList runat="server" ID="ddlUserEdit" class="docText" Visible="false">
                                        <asp:ListItem Value="B" Text="B - Beide" />
                                        <asp:ListItem Value="M" Text="M - Manfred" />
                                        <asp:ListItem Value="E" Text="E - Ernest" />
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="tblFooter1">
                                    <div align="right">
                                        <asp:Button ID="btnSubmit" runat="server" class="btnSave" title="Speichern" Text="Speichern" OnClick="btnSubmit_Click" />
                                        <asp:Button ID="btnCancel" runat="server" class="btnCancel" title="Abbrechen" OnClick="btnCancel_Click" Text="Abbrechen" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <p><br/></p>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
