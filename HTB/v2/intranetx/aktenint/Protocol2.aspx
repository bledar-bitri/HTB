<%@ Page Language="C#" Culture="de-DE" AutoEventWireup="true" CodeBehind="Protocol2.aspx.cs" Inherits="HTB.intranetx.aktenint.Protocol2"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>HTB [ Protokol ]</title>
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
    </style>
    <link rel="stylesheet" type="text/css" media="all" href="../../../includes/wdg/calendar/calendar-win2k-1.css" title="win2k-1" />
    <link href="../../../includes/skins/mxkollection3.css" rel="stylesheet" type="text/css" media="all" />
</head>
<body id="bdy" runat="server">
    <ctl:headerNoMenu ID="header" runat="server" />
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true"
        EnableScriptLocalization="true" />
    <asp:Panel ID="panTop" runat="server">
        <table id="tblAll" runat="server" width="100%" border="0" cellspacing="0" cellpadding="1">
            <tr>
                <td bgcolor="#000000">
                    <table id="tblMain" width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
                        <tr>
                            <td>
                                <table id="tblControls" runat="server" border="0" align="center" cellpadding="3" cellspacing="0">
                                    <tr>
                                        <td colspan="2" class="tblHeader" title="PROTOKOL ">
                                            PROTOKOL
                                        </td>
                                    </tr>
                                    <tr id="tr1" runat="server">
                                        <td class="EditCaption">
                                            <div align="right"><strong>Auftraggeber</strong>:</div>
                                        </td>
                                        <td class="EditData">
                                            <asp:Label runat="server" ID="lblAuftraggeber"/>&nbsp;
                                        </td>
                                    </tr>
                                    <tr id="trVerrechnungsart" runat="server">
                                        <td class="EditCaption">
                                            <div align="right"><strong>Verrechnungsart</strong>:</div>
                                        </td>
                                        <td class="EditData">
                                            <asp:Label runat="server" ID="lblVerrechnungsart"/>
                                        </td>
                                    </tr>
                                    <tr id="trBlankLine1" runat="server">
                                        <td colspan="2" nowrap>
                                            <ctl:message ID="ctlMessage" runat="server" />
                                        </td>
                                    </tr>
                                    <tr id="trRechnungNr" runat="server">
                                        <td class="EditCaptionTopLine">
                                            <div align="right">Rechnungs Nr. ECP:</div>
                                        </td>
                                        <td class="EditDataTopLine">
                                            <asp:TextBox id="txtRechnungsNr" runat="server" type="text" class="docText" onfocus="this.style.backgroundColor='#DFF4FF'" onblur="this.style.backgroundColor=''" size="25" maxlength="10"/>
                                        </td>
                                    </tr>
                                    <tr id="trDatum" runat="server">
                                        <td width="150" class="EditCaptionTopLine">
                                            <div align="right">Datum:</div>
                                        </td>
                                        <td class="EditDataTopLine">
                                            <asp:TextBox runat="server" ID="txtDatum" MaxLength="10" ToolTip="Date of contact" ValidationGroup="DatumVG" onFocus="this.style.backgroundColor='#DFF4FF'" onBlur="this.style.backgroundColor=''" size="10" />
                                            <ajax:MaskedEditExtender ID="Datum_MaskedEditExtender" runat="server" Century="2000"
                                                TargetControlID="txtDatum" Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                                OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left"
                                                ErrorTooltipEnabled="True" />
                                            <ajax:MaskedEditValidator ID="Datum_MaskedEditValidator" runat="server" ControlExtender="Datum_MaskedEditExtender"
                                                ControlToValidate="txtDatum" EmptyValueMessage="Date of contact is required" InvalidValueMessage="Datum is ung&uuml;ltig!"
                                                Display="Dynamic" EmptyValueBlurredText="*" InvalidValueBlurredMessage="*" ValidationGroup="DatumVG" />
                                            <ajax:CalendarExtender ID="Datum_CalendarExtender" runat="server" TargetControlID="txtDatum"
                                                PopupButtonID="Datum_CalendarButton" />
                                            <asp:ImageButton runat="Server" ID="Datum_CalendarButton" ImageUrl="~/v2/intranet/images/CalendarHS.png"
                                                AlternateText="Click to show calendar" /><%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="Datum" ErrorMessage="Please supply a date" ValidationGroup="CallSummaryDialogVG" Display="Dynamic"></asp:RequiredFieldValidator>--%>
                                            &nbsp;&nbsp;Uhrzeit:
                                            <asp:TextBox ID="txtUhrzeit" runat="server" class="docText" ValidationGroup="DatumVG"
                                                onFocus="this.style.backgroundColor='#DFF4FF'" onBlur="this.style.backgroundColor=''"
                                                value="00:00" size="10" MaxLength="5" />
                                            <ajax:MaskedEditExtender ID="Uhrzeit_MaskedEditExtender" runat="server" TargetControlID="txtUhrzeit"
                                                Mask="99:99" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError"
                                                MaskType="Time" AcceptAMPM="True" ErrorTooltipEnabled="True" />
                                            <ajax:MaskedEditValidator ID="Uhrzeit_MaskedEditValidator" runat="server" ControlExtender="Uhrzeit_MaskedEditExtender"
                                                ControlToValidate="txtUhrzeit" IsValidEmpty="True" InvalidValueMessage="Uhrzeit format ist ung&uuml;ltig!"
                                                Display="Dynamic" TooltipMessage="Time of contact" EmptyValueBlurredText="*"
                                                InvalidValueBlurredMessage="*" ValidationGroup="DatumVG" ErrorMessage="Bitte supply a time" />
                                            (ss:mm)
                                        </td>
                                    </tr>

                                    <tr id="trOrt" runat="server">
                                        <td class="EditCaption">
                                            <div align="right">Ort der &Uuml;bernahme:</div>
                                        </td>
                                        <td class="EditData">
                                            <asp:TextBox id="txtOrtDerUbernahme" runat="server" type="text" class="docText" onfocus="this.style.backgroundColor='#DFF4FF'" onblur="this.style.backgroundColor=''" size="25" maxlength="10"/>
                                        </td>
                                    </tr>

                                    <tr id="trKZ" runat="server">
                                        <td valign="middle" class="EditCaption">
                                            <div align="right">KZ:</div>
                                        </td>
                                        <td class="EditData">
                                            <asp:DropDownList ID="ddlKZ" runat="server" class="docText" >
                                                <asp:ListItem Text="*** bitte auswählen ***" Value="-1"></asp:ListItem>
                                                <asp:ListItem Text="am KFZ" Value="am KFZ"></asp:ListItem>
                                                <asp:ListItem Text="abgenommen" Value="abgenommen"></asp:ListItem>
                                                <asp:ListItem Text="abgemeldet" Value="abgemeldet"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="trKzStuck" runat="server">
                                        <td class="EditCaption">
                                            <div align="right">Anzahl KZ (st&uuml;ck):</div>
                                        </td>
                                        <td class="EditData">
                                            <asp:TextBox id="txtAnzahlKz" runat="server" type="text" class="docText" onfocus="this.style.backgroundColor='#DFF4FF'" onblur="this.style.backgroundColor=''" size="20" maxlength="10"/>
                                        </td>
                                    </tr>
                                    <tr id="trKzFromEcpToAg" runat="server">
                                        <td class="EditCaption">
                                            <div align="right">KZ von ECP abgenommen und dem AG &uuml;bergeben:</div>
                                        </td>
                                        <td class="EditData">
                                            <asp:DropDownList ID="ddlKzFromEcpToAg" runat="server" class="docText" >
                                                <asp:ListItem Text="*** bitte auswählen ***" Value="-1"></asp:ListItem>
                                                <asp:ListItem Text="Ja" Value="Ja"></asp:ListItem>
                                                <asp:ListItem Text="Nein" Value="Nein"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="trZulassung" runat="server">
                                        <td valign="middle" class="EditCaption">
                                            <div align="right"><nobr>&Uuml;bernommen mit Zulassung:</nobr></div>
                                        </td>
                                        <td class="EditData">
                                            <asp:DropDownList ID="ddlZulassung" runat="server" class="docText" >
                                                <asp:ListItem Text="*** bitte auswählen ***" Value="-1"></asp:ListItem>
                                                <asp:ListItem Text="Ja" Value="Ja"></asp:ListItem>
                                                <asp:ListItem Text="Nein" Value="Nein"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="trServiceheft" runat="server">
                                        <td class="EditCaption">
                                            <div align="right">Serviceheft:</div>
                                        </td>
                                        <td class="EditData">
                                            <asp:DropDownList ID="ddlServiceheft" runat="server" class="docText" />
                                        </td>
                                    </tr>
                                    <tr id="trAnzahlSchlussel" runat="server">
                                        <td class="EditCaption">
                                            <div align="right">Anzahl Schl&uuml;ssel:</div>
                                        </td>
                                        <td class="EditData">
                                            <asp:TextBox id="txtAnzahlSchlussel" runat="server" type="text" class="docText" onfocus="this.style.backgroundColor='#DFF4FF'" onblur="this.style.backgroundColor=''" size="15" maxlength="10"/>
                                        </td>
                                    </tr>
                                    <tr id="trMasterKey" runat="server">
                                        <td class="EditCaption">
                                            <div align="right">KZ von ECP abgenommen und dem AG &uuml;bergeben:</div>
                                        </td>
                                        <td class="EditData">
                                            <asp:DropDownList ID="ddlMasterKey" runat="server" class="docText" >
                                                <asp:ListItem Text="*** bitte auswählen ***" Value="-1"></asp:ListItem>
                                                <asp:ListItem Text="Ja" Value="Ja"></asp:ListItem>
                                                <asp:ListItem Text="Nein" Value="Nein"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="trKMStand" runat="server">
                                        <td class="EditCaption">
                                            <div align="right">KM - Stand laut Tacho:</div>
                                        </td>
                                        <td class="EditData">
                                            <asp:TextBox id="txtKMStand" runat="server" type="text" class="docText" onfocus="this.style.backgroundColor='#DFF4FF'" onblur="this.style.backgroundColor=''" size="20" maxlength="10"/>
                                        </td>
                                    </tr>
                                    <tr id="trSchaden" runat="server">
                                        <td class="EditCaption">
                                            <div align="right">Sichtbare SCH&Auml;DEN:</div>
                                        </td>
                                        <td class="EditData">
                                            <asp:TextBox id="txtSchaden" runat="server" type="text" class="docText" onfocus="this.style.backgroundColor='#DFF4FF'" onblur="this.style.backgroundColor=''" Rows="5" Columns="70" TextMode="MultiLine"/>
                                        </td>
                                    </tr>

                                    <tr id="trErweiterterBericht" runat="server">
                                        <td class="EditCaption">
                                            <div align="right">Erweiterter Bericht:</div>
                                        </td>
                                        <td class="EditData">
                                            <asp:TextBox id="txtErweiterterBericht" runat="server" type="text" class="docText" onfocus="this.style.backgroundColor='#DFF4FF'" onblur="this.style.backgroundColor=''" Rows="5" Columns="70" TextMode="MultiLine"/>
                                        </td>
                                    </tr>

                                    <tr id="trAbschleppdienst" runat="server">
                                        <td valign="middle" class="EditCaption">
                                            <div align="right">Anschleppdienst:</div>
                                        </td>
                                        <td class="EditData">
                                            <asp:DropDownList ID="ddlAbschleppdienst" runat="server" class="docText" >
                                                <asp:ListItem Text="*** bitte auswählen ***" Value="-1"></asp:ListItem>
                                                <asp:ListItem Text="Ja" Value="Ja"></asp:ListItem>
                                                <asp:ListItem Text="Nein" Value="Nein"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="trAbschleppdienstName" runat="server">
                                        <td class="EditCaption">
                                            <div align="right">Name Abschleppdienst:</div>
                                        </td>
                                        <td class="EditData">
                                            <asp:TextBox id="txtAbschleppdienstName" runat="server" type="text" class="docText" onfocus="this.style.backgroundColor='#DFF4FF'" onblur="this.style.backgroundColor=''" size="35" maxlength="100"/>
                                        </td>
                                    </tr>
                                    <tr id="trAbschleppdienstGrund" runat="server">
                                        <td class="EditCaption">
                                            <div align="right">Abschleppdienst Grund:</div>
                                        </td>
                                        <td class="EditData">
                                            <asp:TextBox id="txtAbschleppdienstGrund" runat="server" type="text" class="docText" onfocus="this.style.backgroundColor='#DFF4FF'" onblur="this.style.backgroundColor=''" size="35" maxlength="100"/>
                                        </td>
                                    </tr>
                                    
                                    <tr id="trAbchleppdienstKosten" runat="server">
                                        <td class="EditCaption">
                                            <div align="right">Abschleppdienstkosten &euro;:</div>
                                        </td>
                                        <td class="EditData">
                                            <asp:TextBox id="txtAbchleppdienstKosten" runat="server" type="text" class="docText" onfocus="this.style.backgroundColor='#DFF4FF'" onblur="this.style.backgroundColor=''" size="15" maxlength="10"/>
                                        </td>
                                    </tr>
                                    <tr id="trPannendienstKosten" runat="server">
                                        <td class="EditCaption">
                                            <div align="right">Pannendienstkosten &euro;:</div>
                                        </td>
                                        <td class="EditData">
                                            <asp:TextBox id="txtPannendienstKosten" runat="server" type="text" class="docText" onfocus="this.style.backgroundColor='#DFF4FF'" onblur="this.style.backgroundColor=''" size="15" maxlength="10"/>
                                        </td>
                                    </tr>
                                    <tr id="trZusatzkostenVignette" runat="server">
                                        <td class="EditCaption">
                                            <div align="right">Zusatzkosten Vignette &euro;:</div>
                                        </td>
                                        <td class="EditData">
                                            <asp:TextBox id="txtZusatzkostenVignette" runat="server" type="text" class="docText" onfocus="this.style.backgroundColor='#DFF4FF'" onblur="this.style.backgroundColor=''" size="15" maxlength="10"/>
                                        </td>
                                    </tr>
                                    <tr id="trMaut" runat="server">
                                        <td class="EditCaption">
                                            <div align="right">Mautgeb&uuml;hren &euro;:</div>
                                        </td>
                                        <td class="EditData">
                                            <asp:TextBox id="txtMaut" runat="server" type="text" class="docText" onfocus="this.style.backgroundColor='#DFF4FF'" onblur="this.style.backgroundColor=''" size="15" maxlength="10"/>
                                        </td>
                                    </tr>
                                    <tr id="trStandGebuhr" runat="server">
                                        <td class="EditCaption">
                                            <div align="right">Standgeb&uuml;hren &euro;:</div>
                                        </td>
                                        <td class="EditData">
                                            <asp:TextBox id="txtStandGebuhr" runat="server" type="text" class="docText" onfocus="this.style.backgroundColor='#DFF4FF'" onblur="this.style.backgroundColor=''" size="15" maxlength="10"/>
                                        </td>
                                    </tr>
                                    <tr id="trZusatzkostenTreibstoff" runat="server">
                                        <td class="EditCaption">
                                            <div align="right">Zusatzkosten Treibstoff &euro;:</div>
                                        </td>
                                        <td class="EditData">
                                            <asp:TextBox id="txtZusatzkostenTreibstoff" runat="server" type="text" class="docText" onfocus="this.style.backgroundColor='#DFF4FF'" onblur="this.style.backgroundColor=''" size="15" maxlength="10"/>
                                        </td>
                                    </tr>
                                    <tr id="trKostenEcp" runat="server">
                                        <td class="EditCaption">
                                            <div align="right">Reparaturkosten ECP &euro;:</div>
                                        </td>
                                        <td class="EditData">
                                            <asp:TextBox id="txtKostenEcp" runat="server" type="text" class="docText" onfocus="this.style.backgroundColor='#DFF4FF'" onblur="this.style.backgroundColor=''" size="15" maxlength="10"/>
                                        </td>
                                    </tr>
                                    <tr id="trUberstellungsdistanz" runat="server">
                                        <td class="EditCaption">
                                            <div align="right"><nobr>Tats&auml;chliche &Uuml;berstellungsdistanz:</nobr></div>
                                        </td>
                                        <td class="EditData">
                                            <asp:TextBox id="txtUberstellungsdistanz" runat="server" type="text" class="docText" onfocus="this.style.backgroundColor='#DFF4FF'" onblur="this.style.backgroundColor=''" size="25" maxlength="10"/>
                                        </td>
                                    </tr>
                                    
                                    <tr id="trPolizeiInformiert" runat="server">
                                        <td valign="middle" class="EditCaption">
                                            <div align="right">Polizei Informiert:</div>
                                        </td>
                                        <td class="EditData">
                                            <asp:DropDownList ID="ddlPolizeiInformiert" runat="server" class="docText" >
                                                <asp:ListItem Text="*** bitte auswählen ***" Value="-1"></asp:ListItem>
                                                <asp:ListItem Text="Ja" Value="Ja"></asp:ListItem>
                                                <asp:ListItem Text="Nein" Value="Nein"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="trPolizeiDienststelle" runat="server">
                                        <td class="EditCaption">
                                            <div align="right">Name vom Beifahrer:</div>
                                        </td>
                                        <td class="EditData">
                                            <asp:TextBox id="txtPolizeiDienststelle" runat="server" type="text" class="docText" onfocus="this.style.backgroundColor='#DFF4FF'" onblur="this.style.backgroundColor=''" size="35" maxlength="100"/>
                                        </td>
                                    </tr>
                                    
                                    <tr id="trBeifahrer" runat="server">
                                        <td class="EditCaption">
                                            <div align="right">Name vom Beifahrer:</div>
                                        </td>
                                        <td class="EditData">
                                            <asp:TextBox id="txtBeifahrer" runat="server" type="text" class="docText" onfocus="this.style.backgroundColor='#DFF4FF'" onblur="this.style.backgroundColor=''" size="35" maxlength="100"/>
                                        </td>
                                    </tr>
                                    
                                    <tr id="trHandler" runat="server">
                                        <td class="EditCaption">
                                            <div align="right">H&auml;ndler:</div>
                                        </td>
                                        <td class="EditData">
                                            <asp:TextBox id="txtHandler" runat="server" type="text" class="docText" onfocus="this.style.backgroundColor='#DFF4FF'" onblur="this.style.backgroundColor=''" size="35" maxlength="100"/>
                                        </td>
                                    </tr>
                                    
                                    <tr id="trUbernommenVon" runat="server">
                                        <td class="EditCaption">
                                            <div align="right">&Uuml;bernommen von:</div>
                                        </td>
                                        <td class="EditData">
                                            <asp:TextBox id="txtUbernommenVon" runat="server" type="text" class="docText" onfocus="this.style.backgroundColor='#DFF4FF'" onblur="this.style.backgroundColor=''" size="35" maxlength="100"/>
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td colspan="2" class="tblFooter1">
                                            <div align="right">
                                                <asp:Button class="btnSave" title="Speichern" OnClick="btnSave_Click" Text="Speichern" runat="server"/>
                                                <asp:Button class="btnSave" runat="server" ID="btnSaveAndGeneratePDF" OnClick="btnSaveAndGeneratePDF_Click" Text="Speichern und PDF Generieren" />
                                                <input name="Submit2" type="button" class="btnCancel" title="Abbrechen" onclick="window.opener.document.location.reload(); window.close();" value="Schliessen">
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <p>
                                    <br/>
                                </p>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </asp:Panel>
    </form>
</body>
</html>
