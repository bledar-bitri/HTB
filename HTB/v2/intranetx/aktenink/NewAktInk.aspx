<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewAktInk.aspx.cs" Inherits="HTB.v2.intranetx.aktenink.NewAktInk" %>

<%@ Register TagPrefix="ctl" TagName="lookupKlient" Src="~/v2/intranetx/global_files/CtlLookupKlient.ascx" %>
<%@ Register TagPrefix="ctl" TagName="lookupGegner" Src="~/v2/intranetx/global_files/CtlLookupGegner.ascx" %>
<%@ Register TagPrefix="ctl" TagName="lookupUser" Src="~/v2/intranetx/global_files/CtlLookupUser.ascx" %>
<%@ Register TagPrefix="ctl" TagName="workflow" Src="~/v2/intranetx/global_files/CtlWorkflow.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>HTB.ASP [ Neuer CollectionInvoiceakt ]</title>
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
    </style>
    <script src="../../intranet/globalcode/lib.js" type="text/javascript" language="javascript"></script>
    <script src="/v2/intranetx/scripts/jquery-1.3.2.js" type="text/javascript"></script>
    <script src="/v2/intranetx/scripts/jquery.MultiFile.js" type="text/javascript"></script>
</head>
<body>
    <ctl:header ID="hdr" runat="server" />
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" EnablePartialRendering="true" EnableScriptLocalization="true" />
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        function BeginRequestHandler(sender, args) {
            document.getElementById('<%= btnSubmit.ClientID %>').innerText = "Processing";
            document.getElementById('<%= btnSubmit.ClientID %>').disabled = true;
            document.getElementById('<%= btnSubmit2.ClientID %>').innerText = "Processing";
            document.getElementById('<%= btnSubmit2.ClientID %>').disabled = true;
            document.getElementById('<%= btnCancel.ClientID %>').innerText = "Processing";
            document.getElementById('<%= btnCancel.ClientID %>').disabled = true;
            document.getElementById('<%= btnCancel2.ClientID %>').innerText = "Processing";
            document.getElementById('<%= btnCancel2.ClientID %>').disabled = true;
        }
    </script>
    <table width="100%" border="0" cellpadding="1" cellspacing="0" bgcolor="#000000">
        <tr>
            <td>
                <table width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
                    <tr>
                        <td class="smallText">
                            <a href="../../intranet/intranet/intranet.asp">Intranet</a> | <a href="../../intranet/intranet/inkasso.asp">CollectionInvoice</a> | <a href="../../intranet/aktenink/AktenStaff.asp">
                                CollectionInvoiceakten</a> | Neuer CollectionInvoiceakt
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <br />
    <asp:UpdatePanel ID="updPanel1" runat="server">
        <ContentTemplate>
            <ajax:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0">
                <ajax:TabPanel ID="TabPanel1" runat="server" HeaderText="Haupt Window">
                    <ContentTemplate>
                        <table width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
                            <tr>
                                <td>
                                    <table border="0" align="center" cellpadding="3" cellspacing="0">
                                        <tr>
                                            <td colspan="2" class="tblHeader" title="NEUER INKASSOAKT ">
                                                NEUER INKASSOAKT
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" class="tblDataAll">
                                                <ctl:message ID="ctlMessage" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="middle" align="right" class="EditCaption">
                                                Auftraggeber:
                                            </td>
                                            <td class="EditData">
                                                <asp:DropDownList ID="ddlAuftraggeber" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="middle" class="EditCaption">
                                                &nbsp;
                                            </td>
                                            <td class="EditData">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="middle" align="right" class="EditCaption">
                                                Klient:
                                            </td>
                                            <td width="846" class="EditData">
                                                <ctl:lookupKlient ID="ctlLookupKlient" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" align="right" class="EditCaption">
                                                &nbsp;
                                            </td>
                                            <td class="EditData">
                                                <asp:LinkButton ID="cmdClientSearch" runat="server" class="smallText" Text="suchen" OnClick="cmdClientSearch_Click" />&nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton ID="cmdClientNew" runat="server" class="smallText" Text="neu" OnClick="cmdClientNew_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" class="EditCaption">
                                                &nbsp;
                                            </td>
                                            <td class="EditData" id="tdKlientText">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="middle" align="right" class="EditCaption">
                                                Gegner:
                                            </td>
                                            <td class="EditData">
                                                <ctl:lookupGegner ID="ctlLookupGegner" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" align="right" class="EditCaption">
                                                &nbsp;
                                            </td>
                                            <td class="EditData">
                                                <asp:LinkButton ID="cmdGegnerSearch" runat="server" class="smallText" Text="suchen" OnClick="cmdGegnerSearch_Click" />&nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton ID="cmdGegnerNew" runat="server" class="smallText" Text="neu" OnClick="cmdGegnerNew_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                &nbsp;
                                            </td>
                                            <td class="EditData" id="tdGegnerText">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" class="EditCaption">
                                                Rechnungsnummer:
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox runat="server" ID="txtClientInvoiceNumber" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor=''" size="50" MaxLength="50" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                &nbsp;
                                            </td>
                                            <td class="EditData">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" class="EditCaption">
                                                Referenznummer Klient:
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox runat="server" ID="txtClientReferenceNumber" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor=''"
                                                    size="50" MaxLength="20" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" class="EditCaption">
                                                Rechnungsdatum:
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox ID="txtClientInvoiceDate" runat="server" MaxLength="10" ToolTip="Rechnungsdatum" onFocus="this.style.backgroundColor='#DFF4FF'" onBlur="this.style.backgroundColor=''"
                                                    size="10" />
                                                <ajax:MaskedEditExtender ID="Datum_MaskedEditExtender" runat="server" Century="2000" TargetControlID="txtClientInvoiceDate" Mask="99/99/9999" MaskType="Date" ErrorTooltipEnabled="True"
                                                    CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                    CultureTimePlaceholder="" Enabled="True" />
                                                <ajax:MaskedEditValidator ID="Datum_MaskedEditValidator" runat="server" ControlExtender="Datum_MaskedEditExtender" ControlToValidate="txtClientInvoiceDate" InvalidValueMessage="Datum is ung&uuml;ltig!"
                                                    Display="Dynamic" InvalidValueBlurredMessage="*" ErrorMessage="Datum_MaskedEditValidator" />
                                                <ajax:CalendarExtender ID="Datum_CalendarExtender" runat="server" TargetControlID="txtClientInvoiceDate" PopupButtonID="Datum_CalendarButton" Enabled="True" />
                                                <asp:ImageButton runat="server" ID="Datum_CalendarButton" ImageUrl="~/v2/intranet/images/CalendarHS.png" AlternateText="Click to show calendar" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" class="EditCaption">
                                                Forderung Klient:
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox runat="server" ID="txtForderung" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor=''" size="15" MaxLength="20" />
                                                &euro;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" class="EditCaption">
                                                Kosten Klient:
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox runat="server" ID="txtKosten" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor=''" size="15" MaxLength="20" />
                                                &euro;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                &nbsp;
                                            </td>
                                            <td class="EditData">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" class="EditCaption">
                                                Sachbearbeiter:
                                            </td>
                                            <td width="846" class="EditData">
                                                <span class="docText">
                                                    <ctl:lookupUser ID="ctlLookupUser" runat="server" />
                                                </span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" align="right" class="EditCaption">
                                                &nbsp;
                                            </td>
                                            <td class="EditData">
                                                <asp:LinkButton ID="cmdUserSearch" runat="server" class="smallText" Text="suchen" OnClick="cmdUserSearch_Click" />&nbsp;&nbsp;&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" class="EditCaption">
                                                Klient Sachbearbeiter:
                                            </td>
                                            <td width="846" class="EditData">
                                                <span class="docText">
                                                    <asp:DropDownList runat="server" ID="ddlClientSB"/>
                                                </span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" align="right" class="EditCaption">
                                                &nbsp;
                                            </td>
                                            <td class="EditData">
                                                <asp:LinkButton ID="cmdClientSBLoad" runat="server" class="smallText" Text="suchen" OnClick="cmdClientSBLoad_Click" />&nbsp;&nbsp;&nbsp;
                                            </td>
                                        </tr>
                                        
                                        <tr>
                                            <td valign="top" align="right" class="EditCaption">
                                                Bemerkungen:
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox runat="server" ID="txtMemo" TextMode="MultiLine" Columns="80" Rows="8" class="docText" onFocus="this.style.backgroundColor='#DFF4FF';" onBlur="this.style.backgroundColor='';" />
                                                <br />
                                                <input name="Submit3" type="button" class="btnStandard" value="Zeitstempel" title="Hier können Sie den Zeitstempel setzten." onclick="GBInsertTimeStamp(document.forms[0].TabContainer1_TabPanel1_txtMemo,'<%=Session["MM_VorName"] + " " + Session["MM_NachName"] + " " + DateTime.Now %>');">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" align="right" class="EditCaption">
                                                Dokumente:
                                            </td>
                                            <td class="EditData">
                                                <div>
                                                    <asp:FileUpload ID="FileUpload1" runat="server" class="multi" />
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <img name="" src="" width="1" height="1" alt="" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" class="EditCaptionTopLine3">
                                                <table width="100%" border="1" cellpadding="3" cellspacing="0">
                                                    <tr>
                                                        <td class="tblHeader">
                                                            <div align="right">
                                                                &nbsp;
                                                                <asp:Button ID="btnSubmit" runat="server" class="btnSave" title="Speichern" Text="Speichern" OnClick="btnSubmit_Click" />
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
                        </table>
                    </ContentTemplate>
                </ajax:TabPanel>
                <ajax:TabPanel ID="TabPanel2" runat="server" HeaderText="Workflow">
                    <ContentTemplate>
                        <table width="100%" border="0" cellspacing="0" cellpadding="1" bgcolor="#FFFFFF">
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
                                                 <table border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF" align="right" >
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
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
    <ctl:footer ID="ftr" runat="server" />
</body>
</html>
