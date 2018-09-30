<%@ Page Language="C#" Culture="de-DE" AutoEventWireup="true" CodeBehind="AuftragTablet.aspx.cs" Inherits="HTB.v2.intranetx.aktenint.AuftragTablet" %>

<%@ Register TagPrefix="ctl" TagName="installment" Src="~/v2/intranetx/global_files/CtlInstallmentTablet.ascx" %>
<%@ Register TagPrefix="ctl" TagName="installmentOld" Src="~/v2/intranetx/global_files/CtlInstallmentOldTablet.ascx" %>
<%@ Register TagPrefix="ctl" TagName="extension" Src="~/v2/intranetx/global_files/CtlExtensionRequest.ascx" %>
<%@ Register TagPrefix="ctl" TagName="telAndEmail" Src="~/v2/intranetx/global_files/CtlTelAndEmailCollection.ascx" %>

<%@ Import Namespace="HTBUtilities" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>HTB.ASP [ Auftrag ]</title>
    <style type="text/css">
        .style2
        {
            font-size: 14;
            font-weight: bold;
        }
        .style4
        {
            font-size: 13pt;
        }
        .Stil1
        {
            font-size: 9px;
        }
        
        .ajax__myTab .ajax__tab_header
        {
            font-family: verdana,tahoma,helvetica;
            font-size: 18px;
            border-bottom: solid 1px #999999;
        }
        
        .ajax__myTab .ajax__tab_outer
        {
            padding-right: 4px;
            height: 30px;
            background-color: #C0C0C0;
            margin-right: 2px;
            border-right: solid 1px #666666;
            border-top: solid 1px #aaaaaa;
        }
        
        .ajax__myTab .ajax__tab_inner
        {
            padding-left: 3px;
            background-color: #C0C0C0;
        }
        
        .ajax__myTab .ajax__tab_tab
        {
            height: 18px;
            padding: 4px;
            margin: 0;
        }
        
        .ajax__myTab .ajax__tab_hover .ajax__tab_outer
        {
            background-color: #cccccc;
        }
        
        .ajax__myTab .ajax__tab_hover .ajax__tab_inner
        {
            background-color: #cccccc;
        }
        
        .ajax__myTab .ajax__tab_hover .ajax__tab_tab
        {
        }
        
        .ajax__myTab .ajax__tab_active .ajax__tab_outer
        {
            background-color: #fff;
            border-left: solid 1px #999999;
        }
        
        .ajax__myTab .ajax__tab_active .ajax__tab_inner
        {
            background-color: #fff;
        }
        
        .ajax__myTab .ajax__tab_active .ajax__tab_tab
        {
        }
        
        .ajax__myTab .ajax__tab_body
        {
            font-family: verdana,tahoma,helvetica;
            font-size: 10pt;
            border: 1px solid #999999;
            border-top: 0;
            padding: 8px;
            background-color: #ffffff;
        }
        
        .MyPopupCalendar .ajax__calendar_container
        {
            border: 1px solid #646464;
            background-color: #ffffff;
            color: #000000;
            padding: 4px;
            margin: 4px;
        }
        
        .MyPopupCalendar .ajax__calendar_footer
        {
            border-top: 1px solid #f5f5f5;
            padding: 3px;
            margin: 3px;
        }
        .MyPopupCalendar .ajax__calendar_dayname
        {
            border-bottom: 1px solid #f5f5f5;
            padding: 0;
            margin: 0;
        }
        
        .MyPopupCalendar .ajax__calendar_day
        {
            border: 1px solid #ffffff;
            padding: 0;
            margin: 0;
            width: 300;
        }
        .MyPopupCalendar .ajax__calendar_month
        {
            border: 1px solid #ffffff;
        }
        .MyPopupCalendar .ajax__calendar_year
        {
            border: 1px solid #ffffff;
        }
        
        .MyPopupCalendar .ajax__calendar_active .ajax__calendar_day
        {
            background-color: #edf9ff;
            border-color: #0066cc;
            color: #0066cc;
        }
        .MyPopupCalendar .ajax__calendar_active .ajax__calendar_month
        {
            background-color: #edf9ff;
            border-color: #0066cc;
            color: #0066cc;
        }
        .MyPopupCalendar .ajax__calendar_active .ajax__calendar_year
        {
            background-color: #edf9ff;
            border-color: #0066cc;
            color: #0066cc;
        }
        
        .MyPopupCalendar .ajax__calendar_other .ajax__calendar_day
        {
            background-color: #ffffff;
            border-color: #ffffff;
            color: #646464;
        }
        .MyPopupCalendar .ajax__calendar_other .ajax__calendar_year
        {
            background-color: #ffffff;
            border-color: #ffffff;
            color: #646464;
        }
        
        .MyPopupCalendar .ajax__calendar_hover .ajax__calendar_day
        {
            background-color: #edf9ff;
            border-color: #daf2fc;
            color: #0066cc;
        }
        .MyPopupCalendar .ajax__calendar_hover .ajax__calendar_month
        {
            background-color: #edf9ff;
            border-color: #daf2fc;
            color: #0066cc;
        }
        .MyPopupCalendar .ajax__calendar_hover .ajax__calendar_year
        {
            background-color: #edf9ff;
            border-color: #daf2fc;
            color: #0066cc;
        }
        
        .MyPopupCalendar .ajax__calendar_hover .ajax__calendar_title
        {
            color: #0066cc;
        }
        .MyPopupCalendar .ajax__calendar_hover .ajax__calendar_today
        {
            color: #0066cc;
        }
    </style>
</head>
<body>
    <ctl:headerNoMenuTablet runat="server" />
    <form runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" EnablePartialRendering="true" EnableScriptLocalization="true" />
    <asp:UpdatePanel runat="server">
    <ContentTemplate>
    <ajax:TabContainer ID="tabContainerMain" runat="server" ActiveTabIndex="0" CssClass="ajax__myTab" AutoPostBack="true" OnActiveTabChanged="tabContainerMain_ActiveTabChanged">
        <ajax:TabPanel ID="tabAkt" runat="server" HeaderText="  Akt  ">
            <ContentTemplate>
                <table width="100%" border="0" cellpadding="4" cellspacing="0" class="style4">
                    <tr>
                        <td align="left">
                            <asp:LinkButton runat="server" ID="lnkPrevious" OnClick="lnkPrevious_Click">
                                <asp:Image ID="Image1" runat="server" ImageUrl="/v2/intranet/images/ButtonPreviousIcon.png" AlternateText="Vorherige Akt" /></asp:LinkButton>&nbsp;
                        </td>
                        <td>
                        </td>
                        <td align="right">
                            <asp:LinkButton runat="server" ID="lnkNext" OnClick="lnkNext_Click">
                                <asp:Image ID="Image2" runat="server" ImageUrl="/v2/intranet/images/ButtonNextIcon.png" AlternateText="Nechste Akt" /></asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" colspan="2">
                            <strong>
                                <asp:Label runat="server" ID="lblAuftraggeberName1" />,
                                <asp:Label runat="server" ID="lblAuftraggeberName2" /></strong><br />
                            <asp:Label runat="server" ID="lblAuftraggeberStrasse" />
                            <asp:Label runat="server" ID="lblAuftraggeberLKZ" />&nbsp;<asp:Label runat="server" ID="lblAuftraggeberPLZ" />&nbsp;<asp:Label runat="server" ID="lblAuftraggeberOrt" />
                        </td>
                        <td align="right">
                            Salzburg,
                            <asp:Label runat="server" ID="lblDate" />
                        </td>
                    </tr>
                </table>
                <hr />
                <p align="left" class="style4">
                    <span><strong>A U F T R A G</strong> <strong>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[AZ:<asp:Label runat="server" ID="lblAktIntAZ" />] [ID:<asp:Label runat="server" ID="lblAktIntID" />]</strong><br>
                        Pers&ouml;nliche Intervention
                        <br />
                        <asp:Label runat="server" ID="lblUserVorname" />&nbsp;<asp:Label runat="server" ID="lblUserNachname" /></span><br />
                    <span>Abgabedatum:&nbsp;<strong><asp:Label runat="server" ID="lblAktIntTerminAD" /></strong><br />
                    </span><span>Akttyp:&nbsp;<strong><asp:Label runat="server" ID="lblAktTypeINTCaption" /></strong></span>
                </p>
                <table width="100%" border="0" cellpadding="0" cellspacing="2" class="style4">
                    <tr>
                        <td>
                            <strong>Bearb. durch:</strong>
                        </td>
                        <td colspan="3">
                            Partner der E.C.P. European Car Protect KG, Loigerstr. 89, A-5071&nbsp;Wals
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <strong>Auftraggeber:</strong>
                        </td>
                        <td colspan="2">
                            <asp:Label runat="server" ID="lblAuftraggeberName1_2" />&nbsp;<asp:Label runat="server" ID="lblAuftraggeberName2_2" />, Kontakt:
                            <asp:Label runat="server" ID="lblAKTIntAGSB" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <strong>Klient:</strong>
                        </td>
                        <td colspan="2">
                            <asp:Label runat="server" ID="lblKlientInfo" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <strong>Gegner:</strong>
                        </td>
                        <td valign="top">
                            <asp:Label runat="server" ID="lblGegnerInfo" />
                        </td>
                        <td valign="top" align="right">
                            <strong>Tel:&nbsp;</strong>
                            <asp:Label runat="server" ID="lblGegnerPhone" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <strong>Info:</strong>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblAKTIntMemo" />
                        </td>
                        <td align="right">
                            <strong>GebDat:</strong>
                            <asp:Label runat="server" ID="lblGegnerGebDat" />
                        </td>
                    </tr>
                </table>
                <br />
                <table width="100%" border="0" cellpadding="0" cellspacing="2" class="couriertext">
                    <tr>
                        <td>
                            <strong><span class="style4">Forderungsaufstellung:</span></strong>
                        </td>
                    </tr>
                    <tr>
                        <td class="tblData1">
                            <asp:GridView ID="gvFA" runat="server" AllowSorting="false" AutoGenerateColumns="False" CellPadding="2" BorderStyle="Groove" Width="100%" ShowFooter="true" ShowHeader="false">
                                <RowStyle Font-Size="11" />
                                <FooterStyle Font-Size="12" />
                                <Columns>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Left" FooterText="Endsumme (inkl. Weggeb&uuml;hr):">
                                        <ItemTemplate>
                                            <%# Eval("Description")%>
                                        </ItemTemplate>
                                        <FooterStyle Font-Bold="True" HorizontalAlign="Left" Wrap="False" />
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <%# Eval("Amount")%>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <%# HTBUtils.FormatCurrency(Pa.GetTotal()) %>
                                        </FooterTemplate>
                                        <FooterStyle Font-Bold="True" HorizontalAlign="Right" Wrap="False" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
                <div align="center">
                    <strong>
                        <br>
                        <span class="Stil1">Wir bitten Sie den oben angef&uuml;hrten Betrag, dem Überbringer dieses Schreibens, gegen eine separate Quittung SOFORT zu bezahlen.</span></strong></div>
                <div align="center" class="Stil1">
                    Bitte begleichen Sie Ihre offene Schuld, Sie vermeiden somit höhere Kosten und weitere Konsequenzen. Vereinbarungsgemäß wird mit der ersten Zahlung die Weggebühr,
                    wie oben angeführt, in Abzug gebracht. Gleichzeitig setzen wir Sie davon in Kenntnis, dass wir nach den Bestimmungen des Datenschutzgesetzes die ermittelten Daten
                    zwecks Gläubigerschutz und der Bonitätsprüfung in Anwendung bringen werden.</div>
                <br />
                <asp:HiddenField runat="server" ID="hdnPageIndex" />
            </ContentTemplate>
        </ajax:TabPanel>
        <ajax:TabPanel runat="server" ID="tabDocuments" HeaderText="Dokumente">
            <ContentTemplate>
                [AZ:<asp:Label runat="server" ID="lblAktIntAZ_Doc" />] [ID:<asp:Label runat="server" ID="lblAktIntID_Doc" />]
                <br/>
                <asp:GridView ID="gvDocs" runat="server" AllowSorting="false" AutoGenerateColumns="False" CellPadding="2" BorderStyle="Groove" Width="100%" ShowFooter="true" CssClass="dataTable">
                    <RowStyle Height="50" Font-Size="14pt" />
                    <Columns>
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
            </ContentTemplate>
        </ajax:TabPanel>
        <ajax:TabPanel ID="tabAction" runat="server" HeaderText="Aktionen">
            <ContentTemplate>
                <asp:Label runat="server" ID="Label1" Visible="false"></asp:Label>
                <table width="100%" border="0" cellpadding="4" cellspacing="0" class="style4">
                    <tr>
                        <td colspan="2">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            [AZ:<asp:Label runat="server" ID="lblAktIntAZ_Action" />] [ID:<asp:Label runat="server" ID="lblAktIntID_Action" />]
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <strong>
                                <asp:Label runat="server" ID="lblAuftraggeberName1_4" />,
                                <asp:Label runat="server" ID="lblAuftraggeberName2_4" />
                            </strong>
                            <br />
                            <asp:Label runat="server" ID="lblAuftraggeberStrasse_4" />
                            <asp:Label runat="server" ID="lblAuftraggeberLKZ_4" />&nbsp;<asp:Label runat="server" ID="lblAuftraggeberPLZ_4" />&nbsp;<asp:Label runat="server" ID="lblAuftraggeberOrt_4" />
                        </td>
                        <td>
                            <asp:HyperLink runat="server" ID="lnkUploadPics">
                                <asp:Image ID="Image3" runat="server" ImageUrl="/v2/intranet/images/IPadUpload.png" AlternateText="Upload Bilder" />
                            </asp:HyperLink>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblGegnerInfo_2" />
                        </td>
                    </tr>
                </table>
                <hr />
                <asp:UpdatePanel ID="updPanel1" runat="server">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="txtBetrag" EventName="TextChanged" />
                    </Triggers>
                    <ContentTemplate>
                        <table width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
                            <tr runat="server" ID="trActionsGrid">
                                <td>
                                    <table border="0" align="center" cellpadding="3" cellspacing="0">
                                        <tr>
                                            <td colspan="2" class="tblHeader" title="NEUE AKTION ">
                                                AKTIONEN
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditData">
                                                <asp:GridView ID="gvExistingActions" 
                                                    runat="server" 
                                                    AllowSorting="false" 
                                                    AutoGenerateColumns="False" 
                                                    CellPadding="2" 
                                                    BorderStyle="Groove" 
                                                    Width="100%" 
                                                    ShowFooter="true" 
                                                    CssClass="dataTable"
                                                    OnRowCommand="gvExistingActions_RowCommand"
                                                    >
                                                    <RowStyle Height="50" Font-Size="14pt" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblActionID" Text='<%# Eval("ActionID")%>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="">
                                                            <ItemTemplate>
                                                                <%# Eval("DeleteUrl")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField HeaderText="Datum/Uhrzeit" DataField="ActionDate" SortExpression="ActionDate" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:ButtonField ButtonType="Link" DataTextField="ActionCaption" CommandName="ShowAction"/>
                                                        <asp:TemplateField HeaderText="">
                                                            <ItemTemplate>
                                                                <%# Eval("PrintReceiptUrl")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table border="0" align="center" cellpadding="3" cellspacing="0">
                                        <tr>
                                            <td colspan="2" class="tblHeader" title="NEUE AKTION ">
                                                NEUE AKTION
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">
                                                    <strong>Aktion</strong>:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:DropDownList ID="ddlAktion" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAktion_SelectedIndexChanged" class="docText" />
                                            </td>
                                        </tr>
                                        <tr id="trBetrag" runat="server">
                                            <td class="EditCaption">
                                                <div align="right">
                                                    Betrag:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox runat="server" ID="txtBetrag" Height="30" class="docText" size="15" MaxLength="10" AutoPostBack="true" OnTextChanged="txtBetrag_TextChanged" />
                                                <asp:Label runat="server" ID="lblBeleg"  Text="Beleg:"/>&nbsp;&nbsp;
                                                <asp:TextBox runat="server" ID="txtBeleg"  Height="30" class="docText" />
                                                <br/>
                                                <asp:HyperLink runat="server" ID="lnkPrintReceipt" Text="Beleg Drucken" Visible="false"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" nowrap>
                                                <img name="" src="" width="1" height="1" alt="" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <ctl:message ID="ctlMessage" runat="server" />
                                            </td>
                                        </tr>
                                        <tr id="trExtra" runat="server" visible="false">
                                            <td colspan="2">
                                                <ctl:installment ID="ctlInstallment" runat="server" Visible="false" />
                                                <ctl:installmentOld ID="ctlInstallmentOld" runat="server" Visible="false" />
                                                <ctl:extension ID="ctlExtension" runat="server" Visible="false" />
                                                <ctl:telAndEmail ID="ctlTelAndEmail" runat="server" Visible="false" />
                                            </td>
                                        </tr>
                                        <tr id="trExtraBlank" runat="server" visible="false">
                                            <td colspan="2" nowrap>
                                                <img name="" src="" width="1" height="1" alt="" />
                                            </td>
                                        </tr>
                                        <tr id="trSaveWithMissingBeleg" runat="server" visible="false">
                                            <td class="tblDataAll" align="center" colspan="2">
                                                <asp:Button runat="server" ID="btnSaveWithMissingBeleg" CssClass="btnSave" Text="Trotztem Speichern" onclick="btnSaveWithMissingBeleg_Click" Height="30"/>
                                            </td>
                                        </tr>
                                        <tr id="trPrice" runat="server" visible="false">
                                            <td valign="middle" class="EditCaptionTopLine">
                                                <div align="right">
                                                    Preis (ECP Eingang):</div>
                                            </td>
                                            <td class="EditDataTopLine">
                                                <asp:Label runat="server" ID="lblPrice" class="docText" />
                                            </td>
                                        </tr>
                                        <tr runat="server" id="trProvision" Visible="false">
                                            <td valign="middle" class="EditCaptionTopLine">
                                                <div align="right">Provision:</div>
                                            </td>
                                            <td class="EditDataTopLine">
                                                <asp:TextBox runat="server" ID="txtProvision" Height="30" class="input_text" />
                                                <asp:Label runat="server" ID="lblProvision" Height="30" class="tabletInput" ReadOnly="true" />
                                            </td>
                                        </tr>
                                         <tr>
                                            <td class="tblDataAll" colspan="2">
                                                <div class="tblHeader">Information zu dieser Intervention (f&uuml;r Klienten sichtbar):</div>
                                                <br/>
                                                <asp:TextBox ID="txtMemo" runat="server" TextMode="MultiLine" Columns="80" Rows="6" class="docText" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" class="tblFooter1">
                                                <div align="right">
                                                    <asp:Button ID="btnSaveAction" runat="server" class="btnSave" title="Speichern" Text="Speichern und Abgeben" OnClick="btnSaveAction_Click" Height="30" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </ContentTemplate>
        </ajax:TabPanel>
    </ajax:TabContainer>
    </ContentTemplate>
    </asp:UpdatePanel>
    </form>
    <ctl:footer runat="server" />
</body>
</html>
