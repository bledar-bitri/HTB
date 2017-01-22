<%@ Page Language="C#" Culture="de-DE" AutoEventWireup="true" CodeBehind="Protocol.aspx.cs" Inherits="HTB.intranetx.aktenint.Protocol"%>

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

<%--                                    <tr id="trBlank2" runat="server">
                                        <td colspan="2" nowrap>
                                            <img name="" src="" width="1" height="1" alt="">
                                        </td>
                                    </tr>                                    --%>
                                    <tr id="trBesuch1" runat="server">
                                       <td class="EditCaption">
                                            <div align="right">1. Besuch:</div>
                                        </td>
                                        <td class="EditData">
                                            <asp:TextBox runat="server" ID="txtBesuch1" MaxLength="10" ToolTip="Besuch Datum" ValidationGroup="DatumVG"
                                                onFocus="this.style.backgroundColor='#DFF4FF'" onBlur="this.style.backgroundColor=''"
                                                size="10" />
                                            <ajax:MaskedEditExtender ID="txtBesuch1_MaskedEditExtender" runat="server" Century="2000"
                                                TargetControlID="txtBesuch1" Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                                OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left"
                                                ErrorTooltipEnabled="True" />
                                            <ajax:MaskedEditValidator ID="txtBesuch1_MaskedEditValidator" runat="server" ControlExtender="txtBesuch1_MaskedEditExtender"
                                                ControlToValidate="txtBesuch1" EmptyValueMessage="Date of contact is required" InvalidValueMessage="Besuch Datum is ung&uuml;ltig!"
                                                Display="Dynamic" EmptyValueBlurredText="*" InvalidValueBlurredMessage="*" ValidationGroup="DatumVG" />
                                            <ajax:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtBesuch1" PopupButtonID="txtBesuch1_CalendarButton" />
                                            <asp:ImageButton runat="Server" ID="txtBesuch1_CalendarButton" ImageUrl="~/v2/intranet/images/CalendarHS.png" AlternateText="Click to show calendar" />

                                        </td>
                                    </tr>
                                    <tr id="trBesuch2" runat="server">
                                       <td class="EditCaption">
                                            <div align="right">2. Besuch:</div>
                                        </td>
                                        <td class="EditData">
                                            <asp:TextBox runat="server" ID="txtBesuch2" MaxLength="10" ToolTip="Besuch Datum" ValidationGroup="DatumVG"
                                                onFocus="this.style.backgroundColor='#DFF4FF'" onBlur="this.style.backgroundColor=''"
                                                size="10" />
                                            <ajax:MaskedEditExtender ID="txtBesuch2_MaskedEditExtender" runat="server" Century="2000"
                                                TargetControlID="txtBesuch2" Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                                OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left"
                                                ErrorTooltipEnabled="True" />
                                            <ajax:MaskedEditValidator ID="txtBesuch2_MaskedEditValidator" runat="server" ControlExtender="txtBesuch2_MaskedEditExtender"
                                                ControlToValidate="txtBesuch2" EmptyValueMessage="Date of contact is required" InvalidValueMessage="Besuch Datum is ung&uuml;ltig!"
                                                Display="Dynamic" EmptyValueBlurredText="*" InvalidValueBlurredMessage="*" ValidationGroup="DatumVG" />
                                            <ajax:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtBesuch2" PopupButtonID="txtBesuch2_CalendarButton" />
                                            <asp:ImageButton runat="Server" ID="txtBesuch2_CalendarButton" ImageUrl="~/v2/intranet/images/CalendarHS.png" AlternateText="Click to show calendar" />

                                        </td>
                                    </tr>
                                    <tr id="trBesuch3" runat="server">
                                       <td class="EditCaption">
                                            <div align="right">3. Besuch:</div>
                                        </td>
                                        <td class="EditData">
                                            <asp:TextBox runat="server" ID="txtBesuch3" MaxLength="10" ToolTip="Besuch Datum" ValidationGroup="DatumVG"
                                                onFocus="this.style.backgroundColor='#DFF4FF'" onBlur="this.style.backgroundColor=''"
                                                size="10" />
                                            <ajax:MaskedEditExtender ID="txtBesuch3_MaskedEditExtender" runat="server" Century="2000"
                                                TargetControlID="txtBesuch3" Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                                OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left"
                                                ErrorTooltipEnabled="True" />
                                            <ajax:MaskedEditValidator ID="txtBesuch3_MaskedEditValidator" runat="server" ControlExtender="txtBesuch3_MaskedEditExtender"
                                                ControlToValidate="txtBesuch3" EmptyValueMessage="Date of contact is required" InvalidValueMessage="Besuch Datum is ung&uuml;ltig!"
                                                Display="Dynamic" EmptyValueBlurredText="*" InvalidValueBlurredMessage="*" ValidationGroup="DatumVG" />
                                            <ajax:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtBesuch3" PopupButtonID="txtBesuch3_CalendarButton" />
                                            <asp:ImageButton runat="Server" ID="txtBesuch3_CalendarButton" ImageUrl="~/v2/intranet/images/CalendarHS.png" AlternateText="Click to show calendar" />

                                        </td>
                                    </tr>
                                    <tr id="trBesuch4" runat="server">
                                       <td class="EditCaption">
                                            <div align="right">4. Besuch:</div>
                                        </td>
                                        <td class="EditData">
                                            <asp:TextBox runat="server" ID="txtBesuch4" MaxLength="10" ToolTip="Besuch Datum" ValidationGroup="DatumVG"
                                                onFocus="this.style.backgroundColor='#DFF4FF'" onBlur="this.style.backgroundColor=''"
                                                size="10" />
                                            <ajax:MaskedEditExtender ID="txtBesuch4_MaskedEditExtender" runat="server" Century="2000"
                                                TargetControlID="txtBesuch4" Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                                OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left"
                                                ErrorTooltipEnabled="True" />
                                            <ajax:MaskedEditValidator ID="txtBesuch4_MaskedEditValidator" runat="server" ControlExtender="txtBesuch4_MaskedEditExtender"
                                                ControlToValidate="txtBesuch4" EmptyValueMessage="Date of contact is required" InvalidValueMessage="Besuch Datum is ung&uuml;ltig!"
                                                Display="Dynamic" EmptyValueBlurredText="*" InvalidValueBlurredMessage="*" ValidationGroup="DatumVG" />
                                            <ajax:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="txtBesuch4" PopupButtonID="txtBesuch4_CalendarButton" />
                                            <asp:ImageButton runat="Server" ID="txtBesuch4_CalendarButton" ImageUrl="~/v2/intranet/images/CalendarHS.png" AlternateText="Click to show calendar" />

                                        </td>
                                    </tr>
                                    <tr id="trBesuch5" runat="server">
                                       <td class="EditCaption">
                                            <div align="right">5. Besuch:</div>
                                        </td>
                                        <td class="EditData">
                                            <asp:TextBox runat="server" ID="txtBesuch5" MaxLength="10" ToolTip="Besuch Datum" ValidationGroup="DatumVG"
                                                onFocus="this.style.backgroundColor='#DFF4FF'" onBlur="this.style.backgroundColor=''"
                                                size="10" />
                                            <ajax:MaskedEditExtender ID="txtBesuch5_MaskedEditExtender" runat="server" Century="2000"
                                                TargetControlID="txtBesuch5" Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                                OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left"
                                                ErrorTooltipEnabled="True" />
                                            <ajax:MaskedEditValidator ID="txtBesuch5_MaskedEditValidator" runat="server" ControlExtender="txtBesuch5_MaskedEditExtender"
                                                ControlToValidate="txtBesuch5" EmptyValueMessage="Date of contact is required" InvalidValueMessage="Besuch Datum is ung&uuml;ltig!"
                                                Display="Dynamic" EmptyValueBlurredText="*" InvalidValueBlurredMessage="*" ValidationGroup="DatumVG" />
                                            <ajax:CalendarExtender ID="CalendarExtender5" runat="server" TargetControlID="txtBesuch5" PopupButtonID="txtBesuch5_CalendarButton" />
                                            <asp:ImageButton runat="Server" ID="txtBesuch5_CalendarButton" ImageUrl="~/v2/intranet/images/CalendarHS.png" AlternateText="Click to show calendar" />

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

                                    <tr id="trForderungBarKassiert" runat="server">
                                        <td class="EditCaption">
                                            <div align="right"><nobr>Forderung Bar kassiert &euro;:</nobr></div>
                                        </td>
                                        <td class="EditData">
                                            <asp:TextBox id="txtForderungBarKassiert" runat="server" type="text" class="docText" onfocus="this.style.backgroundColor='#DFF4FF'" onblur="this.style.backgroundColor=''" size="15" maxlength="10"/>
                                            
                                            &nbsp;&nbsp;&nbsp;&nbsp;
                                            Forderung &uuml;berwiesen AG am:
                                            <asp:TextBox runat="server" ID="txtForderungUberwiesenAm" MaxLength="10" ToolTip="Besuch Datum" ValidationGroup="DatumVG" onFocus="this.style.backgroundColor='#DFF4FF'" onBlur="this.style.backgroundColor=''" size="10" />
                                            <ajax:MaskedEditExtender ID="txtForderungUberwiesenAm_MaskedEditExtender" runat="server" Century="2000" TargetControlID="txtForderungUberwiesenAm" Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left" ErrorTooltipEnabled="True" />
                                            <ajax:MaskedEditValidator ID="txtForderungUberwiesenAm_MaskedEditValidator" runat="server" ControlExtender="txtForderungUberwiesenAm_MaskedEditExtender" ControlToValidate="txtForderungUberwiesenAm" EmptyValueMessage="Date of contact is required" InvalidValueMessage="Besuch Datum is ung&uuml;ltig!" Display="Dynamic" EmptyValueBlurredText="*" InvalidValueBlurredMessage="*" ValidationGroup="DatumVG" />
                                            <ajax:CalendarExtender ID="CalendarExtender7" runat="server" TargetControlID="txtForderungUberwiesenAm" PopupButtonID="txtForderungUberwiesenAm_CalendarButton" />
                                            <asp:ImageButton runat="Server" ID="txtForderungUberwiesenAm_CalendarButton" ImageUrl="~/v2/intranet/images/CalendarHS.png" AlternateText="Click to show calendar" />
                                        </td>
                                    </tr>
                                    <tr id="trVersicherungBarKassiert" runat="server">
                                        <td class="EditCaption">
                                            <div align="right"><nobr>Versicherung Bar kassiert &euro;:</nobr></div>
                                        </td>
                                        <td class="EditData">
                                            <asp:TextBox id="txtVersicherungBarKassiert" runat="server" type="text" class="docText" onfocus="this.style.backgroundColor='#DFF4FF'" onblur="this.style.backgroundColor=''" size="15" maxlength="10"/>
                                            &nbsp;&nbsp;&nbsp;&nbsp;
                                            &Uuml;berwiesen am:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:TextBox runat="server" ID="txtVersicherungUberwiesenAm" MaxLength="10" ToolTip="Besuch Datum" ValidationGroup="VersicherungVg"
                                                onFocus="this.style.backgroundColor='#DFF4FF'" onBlur="this.style.backgroundColor=''"
                                                size="10" />
                                            <ajax:MaskedEditExtender ID="txtVersicherungUberwiesenAm_MaskedEditExtender" runat="server" Century="2000"
                                                TargetControlID="txtVersicherungUberwiesenAm" Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                                OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left"
                                                ErrorTooltipEnabled="True" />
                                            <ajax:MaskedEditValidator ID="txtVersicherungUberwiesenAm_MaskedEditValidator" runat="server" ControlExtender="txtVersicherungUberwiesenAm_MaskedEditExtender"
                                                ControlToValidate="txtVersicherungUberwiesenAm" EmptyValueMessage="Date of contact is required" InvalidValueMessage="Besuch Datum is ung&uuml;ltig!"
                                                Display="Dynamic" EmptyValueBlurredText="*" InvalidValueBlurredMessage="*" ValidationGroup="VersicherungVG" />
                                            <ajax:CalendarExtender ID="CalendarExtender6" runat="server" TargetControlID="txtVersicherungUberwiesenAm" PopupButtonID="txtVersicherungUberwiesenAm_CalendarButton" />
                                            <asp:ImageButton runat="Server" ID="txtVersicherungUberwiesenAm_CalendarButton" ImageUrl="~/v2/intranet/images/CalendarHS.png" AlternateText="Click to show calendar" />
                                        </td>
                                    </tr>
                                    <tr id="trKostenBarKassiert" runat="server">
                                        <td class="EditCaption">
                                            <div align="right"><nobr>Kosten Bar kassiert &euro;:</nobr></div>
                                        </td>
                                        <td class="EditData">
                                            <asp:TextBox id="txtKostenBarKassiert" runat="server" type="text" class="docText" onfocus="this.style.backgroundColor='#DFF4FF'" onblur="this.style.backgroundColor=''" size="15" maxlength="10"/>
                                            
                                            &nbsp;&nbsp;&nbsp;&nbsp;
                                            Kosten &uuml;berwiesen E.C.P. am:
                                            <asp:TextBox runat="server" ID="txtKostenUberwiesenAm" MaxLength="10" ToolTip="Datum" ValidationGroup="DatumVG"
                                                onFocus="this.style.backgroundColor='#DFF4FF'" onBlur="this.style.backgroundColor=''"
                                                size="10" />
                                            <ajax:MaskedEditExtender ID="txtKostenUberwiesenAm_MaskedEditExtender" runat="server" Century="2000"
                                                TargetControlID="txtKostenUberwiesenAm" Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                                OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left"
                                                ErrorTooltipEnabled="True" />
                                            <ajax:MaskedEditValidator ID="txtKostenUberwiesenAm_MaskedEditValidator" runat="server" ControlExtender="txtKostenUberwiesenAm_MaskedEditExtender"
                                                ControlToValidate="txtKostenUberwiesenAm" EmptyValueMessage="Date of contact is required" InvalidValueMessage="Besuch Datum is ung&uuml;ltig!"
                                                Display="Dynamic" EmptyValueBlurredText="*" InvalidValueBlurredMessage="*" ValidationGroup="DatumVG" />
                                            <ajax:CalendarExtender ID="CalendarExtender8" runat="server" TargetControlID="txtKostenUberwiesenAm" PopupButtonID="txtKostenUberwiesenAm_CalendarButton" />
                                            <asp:ImageButton runat="Server" ID="txtKostenUberwiesenAm_CalendarButton" ImageUrl="~/v2/intranet/images/CalendarHS.png" AlternateText="Click to show calendar" />
                                        </td>
                                    </tr>
                                    <tr id="trDirektzahlung" runat="server">
                                        <td class="EditCaption">
                                            <div align="right"><nobr>Direktzahlung an AG &euro;:</nobr></div>
                                        </td>
                                        <td class="EditData">
                                            <asp:TextBox id="txtDirektzahlung" runat="server" type="text" class="docText" onfocus="this.style.backgroundColor='#DFF4FF'" onblur="this.style.backgroundColor=''" size="15" maxlength="10"/>
                                            &nbsp;&nbsp;&nbsp;&nbsp;
                                            Direktzahlung am:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:TextBox runat="server" ID="txtDirektzahlungAm" MaxLength="10" ToolTip="Datum" ValidationGroup="DatumVG"
                                                onFocus="this.style.backgroundColor='#DFF4FF'" onBlur="this.style.backgroundColor=''"
                                                size="10" />
                                            <ajax:MaskedEditExtender ID="txtDirektzahlungAm_MaskedEditExtender" runat="server" Century="2000"
                                                TargetControlID="txtDirektzahlungAm" Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                                OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left"
                                                ErrorTooltipEnabled="True" />
                                            <ajax:MaskedEditValidator ID="txtDirektzahlungAm_MaskedEditValidator" runat="server" ControlExtender="txtDirektzahlungAm_MaskedEditExtender"
                                                ControlToValidate="txtDirektzahlungAm" InvalidValueMessage="Datum is ung&uuml;ltig!"
                                                Display="Dynamic" EmptyValueBlurredText="*" InvalidValueBlurredMessage="*" ValidationGroup="DatumVG" />
                                            <ajax:CalendarExtender ID="txtDirektzahlungAm_CalendarExtender" runat="server" TargetControlID="txtDirektzahlungAm" PopupButtonID="txtDirektzahlungAm_CalendarButton" />
                                            <asp:ImageButton runat="Server" ID="txtDirektzahlungAm_CalendarButton" ImageUrl="~/v2/intranet/images/CalendarHS.png" AlternateText="Click to show calendar" />
                                        </td>
                                    </tr>
                                    <tr id="trDirektzahlungVersicherung" runat="server">
                                        <td class="EditCaption">
                                            <div align="right"><nobr>Direktzahlung an Versicherung &euro;:</nobr></div>
                                        </td>
                                        <td class="EditData">
                                            <asp:TextBox id="txtDirektzahlungVersicherung" runat="server" type="text" class="docText" onfocus="this.style.backgroundColor='#DFF4FF'" onblur="this.style.backgroundColor=''" size="15" maxlength="10"/>
                                            &nbsp;&nbsp;&nbsp;&nbsp;
                                            Direktzahlung an Versicherung am:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:TextBox runat="server" ID="txtDirektzahlungVersicherungAm" MaxLength="10" ToolTip="Datum" ValidationGroup="DatumVG"
                                                onFocus="this.style.backgroundColor='#DFF4FF'" onBlur="this.style.backgroundColor=''"
                                                size="10" />
                                            <ajax:MaskedEditExtender ID="txtDirektzahlungVersicherungAm_MaskedEditExtender" runat="server" Century="2000"
                                                TargetControlID="txtDirektzahlungVersicherungAm" Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                                OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left"
                                                ErrorTooltipEnabled="True" />
                                            <ajax:MaskedEditValidator ID="txtDirektzahlungVersicherungAm_MaskedEditValidator" runat="server" ControlExtender="txtDirektzahlungVersicherungAm_MaskedEditExtender"
                                                ControlToValidate="txtDirektzahlungVersicherungAm" InvalidValueMessage="Datum is ung&uuml;ltig!"
                                                Display="Dynamic" EmptyValueBlurredText="*" InvalidValueBlurredMessage="*" ValidationGroup="DatumVG" />
                                            <ajax:CalendarExtender ID="txtDirektzahlungVersicherungAm_CalendarExtender" runat="server" TargetControlID="txtDirektzahlungVersicherungAm" PopupButtonID="txtDirektzahlungVersicherungAm_CalendarButton" />
                                            <asp:ImageButton runat="Server" ID="txtDirektzahlungVersicherungAm_CalendarButton1" ImageUrl="~/v2/intranet/images/CalendarHS.png" AlternateText="Click to show calendar" />
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
                                    <tr id="trZusatzkostenTreibstoff" runat="server">
                                        <td class="EditCaption">
                                            <div align="right">Zusatzkosten Treibstoff &euro;:</div>
                                        </td>
                                        <td class="EditData">
                                            <asp:TextBox id="txtZusatzkostenTreibstoff" runat="server" type="text" class="docText" onfocus="this.style.backgroundColor='#DFF4FF'" onblur="this.style.backgroundColor=''" size="15" maxlength="10"/>
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
                                    <tr id="trZusatzkostenSonstige" runat="server">
                                        <td class="EditCaption">
                                            <div align="right">Sonstige Zusatzkosten &euro;:</div>
                                        </td>
                                        <td class="EditData">
                                            <asp:TextBox id="txtZusatzkostenSonstige" runat="server" type="text" class="docText" onfocus="this.style.backgroundColor='#DFF4FF'" onblur="this.style.backgroundColor=''" size="15" maxlength="10"/>
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
                                    <tr id="trHandler" runat="server">
                                        <td class="EditCaption">
                                            <div align="right">H&auml;ndler:</div>
                                        </td>
                                        <td class="EditData">
                                            <asp:TextBox id="txtHandler" runat="server" type="text" class="docText" onfocus="this.style.backgroundColor='#DFF4FF'" onblur="this.style.backgroundColor=''" size="35" maxlength="100"/>
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
                                    <tr id="trBeifahrer" runat="server">
                                        <td class="EditCaption">
                                            <div align="right">Name vom Beifahrer:</div>
                                        </td>
                                        <td class="EditData">
                                            <asp:TextBox id="txtBeifahrer" runat="server" type="text" class="docText" onfocus="this.style.backgroundColor='#DFF4FF'" onblur="this.style.backgroundColor=''" size="35" maxlength="100"/>
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
                                    <br>
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
