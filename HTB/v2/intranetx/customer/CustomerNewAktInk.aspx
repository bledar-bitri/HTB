<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerNewAktInk.aspx.cs" Inherits="HTB.v2.intranetx.customer.CustomerNewAktInk" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>HTB.ASP [ Neuer Inkassoakt ]</title>
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
    <ctl:headerNoMenu ID="hdr" runat="server" />
    <form id="form1" runat="server">
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
                            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="Customer.aspx">Portal</asp:HyperLink> | <a href="AktenInk.aspx">Inkassosakten</a> | Neuer Akt | <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="javascript:window.print()">Drucken</asp:HyperLink>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <br />
    <asp:UpdatePanel ID="updPanel1" runat="server">
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
                                <td colspan="2" >
                                    <ctl:message ID="ctlMessage" runat="server" />
                                </td>
                            </tr>

                            <tr>
                                <td colspan="2" class="tblHeader" title="Adressdaten">
                                    Schuldner
                                </td>
                            </tr>
                            <tr>
                                <td class="EditCaption">
                                    <div align="right"><strong>Schuldner Typ:</strong></div>
                                </td>
                                <td class="EditData">
                                    <asp:DropDownList runat="server" ID="ddlGegnerType" class="docText" />
                                </td>
                            </tr>
                            <tr>
                                <td class="EditCaption">
                                    <div align="right">
                                        Titel:</div>
                                </td>
                                <td class="EditData">
                                    <asp:TextBox runat="server" ID="txtAnrede" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="50" MaxLength="100" />
                                </td>
                            </tr>
                            <tr>
                                <td class="EditCaption">
                                    <div align="right"><strong>Nachname / Firma:</strong></div>
                                </td>
                                <td class="EditData">
                                    <asp:TextBox runat="server" ID="txtName1" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="50" MaxLength="100" />
                                </td>
                            </tr>
                            <tr>
                                <td class="EditCaption">
                                    <div align="right">Vorname:</div>
                                </td>
                                <td class="EditData">
                                    <asp:TextBox runat="server" ID="txtName2" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="50" MaxLength="100" />
                                </td>
                            </tr>
                            <tr>
                                <td class="EditCaption">
                                    <div align="right">
                                        Alias:</div>
                                </td>
                                <td class="EditData">
                                    <asp:TextBox runat="server" ID="txtName3" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="50" MaxLength="100" />
                                </td>
                            </tr>
                            <tr>
                                <td class="EditCaption">
                                    <div align="right">
                                        Strasse:</div>
                                </td>
                                <td class="EditData">
                                    <asp:TextBox runat="server" ID="txtStrasse" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="50"
                                        MaxLength="100" />
                                </td>
                            </tr>
                            <tr>
                                <td nowrap class="EditCaption">
                                    <div align="right">
                                        LKZ / PLZ / Ort:</div>
                                </td>
                                <td class="EditData">
                                    <asp:TextBox runat="server" ID="txtZIPPrefix" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" value="A" size="3"
                                        MaxLength="3" />
                                    -
                                    <asp:TextBox runat="server" ID="txtZIP" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="12" MaxLength="12" />
                                    <asp:TextBox runat="server" ID="txtOrt" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="50" MaxLength="50" />
                                </td>
                            </tr>
                            <tr runat="server" id="trTelefon">
                                <td valign="middle" class="EditCaption">
                                    <div align="right">Telefon:&nbsp;</div>
                                </td>
                                <td class="EditData">
                                    <table id="tblPhone" runat="server">
                                
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td nowrap class="EditCaption">
                                    <div align="right">
                                        Fax:</div>
                                </td>
                                <td class="EditData">
                                    +
                                    <asp:TextBox runat="server" ID="txtFaxCountry" class="docText" value="43" size="5" MaxLength="5" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" />
                                    (
                                    <asp:TextBox runat="server" ID="txtFaxCity" class="docText" size="6" MaxLength="10" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" />
                                    )
                                    <asp:TextBox runat="server" ID="txtFax" class="docText" size="15" MaxLength="50" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" />
                                </td>
                            </tr>     
                            <tr>
                                <td nowrap class="EditCaption">
                                    <div align="right">
                                        E-Mail:</div>
                                </td>
                                <td class="EditData">
                                    <asp:TextBox runat="server" ID="txtEmail" class="docText" size="50" MaxLength="50" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" />
                                </td>
                            </tr>
                            <tr>
                                <td class="EditCaption">
                                    <div align="right">
                                        Geburtsdatum:</div>
                                </td>
                                <td class="EditData">
                                    <asp:TextBox runat="server" ID="txtGebDat" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="10" MaxLength="10" />
                                    <ajax:MaskedEditExtender ID="MaskedEditExtender2" runat="server" Century="2000" TargetControlID="txtGebDat" Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" MaskType="Date" ErrorTooltipEnabled="True" />
                                    <ajax:MaskedEditValidator ID="MaskedEditValidator2" runat="server" ControlExtender="Datum_MaskedEditExtender" ControlToValidate="txtGebDat" InvalidValueMessage="Datum is ung&uuml;ltig!" Display="Dynamic" InvalidValueBlurredMessage="*"  />
                                    <ajax:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtGebDat" PopupButtonID="GebDatum_CalendarButton" />
                                    <asp:ImageButton runat="Server" ID="GebDatum_CalendarButton" ImageUrl="~/v2/intranet/images/CalendarHS.png" AlternateText="Click to show calendar" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" >
                                &nbsp;
                                </td>
                            </tr>
                            
                            <tr>
                                <td colspan="2" class="tblHeader" title="Forderung">
                                    Forderung
                                </td>
                            </tr>
                            <tr runat="server" ID="trClientSB">
                                <td align="right" class="EditCaption">
                                    Sachbearbeiter:
                                </td>
                                <td width="846" class="EditData">
                                    <span class="docText">
                                        <asp:DropDownList runat="server" ID="ddlClientSB"/>
                                    </span>
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
                                <td align="right" class="EditCaption">
                                    Referenznummer:
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
                                    Forderung:
                                </td>
                                <td class="EditData">
                                    <asp:TextBox runat="server" ID="txtForderung" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor=''" size="15" MaxLength="20" />
                                    &euro;
                                </td>
                            </tr>
                            <tr>
                                <td align="right" class="EditCaption">
                                    Kosten:
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
                                <td valign="top" align="right" class="EditCaption">
                                    Bemerkungen:
                                </td>
                                <td class="EditData">
                                    <asp:TextBox runat="server" ID="txtMemo" TextMode="MultiLine" Columns="80" Rows="8" class="docText" onFocus="this.style.backgroundColor='#DFF4FF';" onBlur="this.style.backgroundColor='';" />
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
            <asp:HiddenField runat="server" ID="hdnNumberOfPhones"/>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
    <ctl:footer ID="ftr" runat="server" />
</body>
</html>
