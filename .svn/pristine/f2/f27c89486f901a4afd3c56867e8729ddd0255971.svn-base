<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="KlientBericht.aspx.cs" Inherits="HTB.v2.intranetx.klienten.KlientBericht" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <title>HTB.ASP [ Klientbericht ]</title>
    <link href="../../intranet/styles/inksys.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        body
        {
            margin-left: 5px;
            margin-top: 5px;
            margin-right: 5px;
            margin-bottom: 5px;
            background-image: url(../../intranet/images/osxback.gif);
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
        
        OPTION.dis
        {
            background-color: white;
            color: #999999;
        }
        
        .modalBackground
        {
            background-color: #CCCCCC;
            filter: alpha(opacity=40);
            opacity: 0.5;
        }
        
        .ModalWindow
        {
            border: solid1px#c0c0c0;
            background: #f0f0f0;
            padding: 0px10px10px10px;
            position: absolute;
            top: -1000px;
        }
    </style>
</head>
<body>
    <ctl:headerNoMenu ID="hdr" runat="server" />
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" EnablePartialRendering="true"  EnableScriptLocalization="true" />
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
                                    <td colspan="2" class="tblHeader" title="BERICHT DRUCKEN">
                                        BERICHT DRUCKEN
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" class="tblDataAll">
                                        <ctl:message ID="ctlMessage" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" class="EditCaption">
                                        <div align="right">Klient:</div>
                                    </td>
                                    <td class="EditData">
                                        <asp:Label ID="lblKlient" runat="server" class="docText" />
                                        <br />
                                        <asp:Label ID="lblKlientName" runat="server" class="docText" />
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" class="EditCaption">
                                        <div align="right">Datum erfasst:</div>
                                    </td>
                                    <td class="EditData">
                                        vom:
                                        <asp:TextBox ID="txtEnteredDateStart" runat="server" class="docText" MaxLength="10" onFocus="this.style.backgroundColor='#DFF4FF'" onBlur="this.style.backgroundColor=''" size="10"/>
                                        <ajax:MaskedEditExtender ID="Datum_MaskedEditExtender1" runat="server" Century="2000" TargetControlID="txtEnteredDateStart" Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" MaskType="Date" ErrorTooltipEnabled="True" />
                                        <ajax:MaskedEditValidator ID="Datum_MaskedEditValidator1" runat="server" ControlExtender="Datum_MaskedEditExtender1" ControlToValidate="txtEnteredDateStart" InvalidValueMessage="Datum is ung&uuml;ltig!" Display="Dynamic" InvalidValueBlurredMessage="*"  />
                                        <ajax:CalendarExtender ID="Datum_CalendarExtender1" runat="server" TargetControlID="txtEnteredDateStart" PopupButtonID="Datum_CalendarButton1" />
                                        <asp:ImageButton runat="Server" ID="Datum_CalendarButton1" ImageUrl="~/v2/intranet/images/CalendarHS.png" AlternateText="Click to show calendar" />
                                    
                                        &nbsp;&nbsp;&nbsp;bis:
                                        <asp:TextBox ID="txtEnteredDateEnd" runat="server" class="docText" MaxLength="10" onFocus="this.style.backgroundColor='#DFF4FF'" onBlur="this.style.backgroundColor=''" size="10"/>
                                        <ajax:MaskedEditExtender ID="Datum_MaskedEditExtender2" runat="server" Century="2000" TargetControlID="txtEnteredDateEnd" Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" MaskType="Date" ErrorTooltipEnabled="True" />
                                        <ajax:MaskedEditValidator ID="Datum_MaskedEditValidator2" runat="server" ControlExtender="Datum_MaskedEditExtender2" ControlToValidate="txtEnteredDateEnd" InvalidValueMessage="Datum is ung&uuml;ltig!" Display="Dynamic" InvalidValueBlurredMessage="*"  />
                                        <ajax:CalendarExtender ID="Datum_CalendarExtender2" runat="server" TargetControlID="txtEnteredDateEnd" PopupButtonID="Datum_CalendarButton2" />
                                        <asp:ImageButton runat="Server" ID="Datum_CalendarButton2" ImageUrl="~/v2/intranet/images/CalendarHS.png" AlternateText="Click to show calendar" />
                              
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" class="EditCaption">
                                        <div align="right">Datum letzte &Auml;nerung:</div>
                                    </td>
                                    <td class="EditData">
                                        vom:
                                        <asp:TextBox ID="txtUpdatedDateStart" runat="server" class="docText"  MaxLength="10" onFocus="this.style.backgroundColor='#DFF4FF'" onBlur="this.style.backgroundColor=''" size="10"/>
                                        <ajax:MaskedEditExtender ID="Datum_MaskedEditExtender3" runat="server" Century="2000" TargetControlID="txtUpdatedDateStart" Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" MaskType="Date" ErrorTooltipEnabled="True" />
                                        <ajax:MaskedEditValidator ID="Datum_MaskedEditValidator3" runat="server" ControlExtender="Datum_MaskedEditExtender3" ControlToValidate="txtUpdatedDateStart" InvalidValueMessage="Datum is ung&uuml;ltig!" Display="Dynamic" InvalidValueBlurredMessage="*"  />
                                        <ajax:CalendarExtender ID="Datum_CalendarExtender3" runat="server" TargetControlID="txtUpdatedDateStart" PopupButtonID="Datum_CalendarButton3" />
                                        <asp:ImageButton runat="Server" ID="Datum_CalendarButton3" ImageUrl="~/v2/intranet/images/CalendarHS.png" AlternateText="Click to show calendar" />
                                        &nbsp;&nbsp;&nbsp;bis:
                                        <asp:TextBox ID="txtUpdatedDateEnd" runat="server" class="docText"  MaxLength="10" onFocus="this.style.backgroundColor='#DFF4FF'" onBlur="this.style.backgroundColor=''" size="10"/>
                                        <ajax:MaskedEditExtender ID="Datum_MaskedEditExtender4" runat="server" Century="2000" TargetControlID="txtUpdatedDateEnd" Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" MaskType="Date" ErrorTooltipEnabled="True" />
                                        <ajax:MaskedEditValidator ID="Datum_MaskedEditValidator4" runat="server" ControlExtender="Datum_MaskedEditExtender4" ControlToValidate="txtUpdatedDateEnd" InvalidValueMessage="Datum is ung&uuml;ltig!" Display="Dynamic" InvalidValueBlurredMessage="*"  />
                                        <ajax:CalendarExtender ID="Datum_CalendarExtender4" runat="server" TargetControlID="txtUpdatedDateEnd" PopupButtonID="Datum_CalendarButton4" />
                                        <asp:ImageButton runat="Server" ID="Datum_CalendarButton4" ImageUrl="~/v2/intranet/images/CalendarHS.png" AlternateText="Click to show calendar" />
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" class="EditCaption">
                                        <div align="right">Keine stornierte Akten:</div>
                                    </td>
                                    <td class="EditData">
                                        <asp:CheckBox runat="server" ID="chkSkipStorno" Checked="true"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <img name="" src="" width="1" height="1" alt=""/>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" class="EditCaptionTopLine3">
                                        <table width="100%" border="1" align="center" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
                                            <tr>
                                                <td class="tblFooter1">
                                                    <div align="right">
                                                        <asp:Button ID="btnSubmit" runat="server" class="btnSave" OnClick="btnSubmit_Click" Text="Drucken" title="Drucken" />
                                                        <input type="button" id="btnCancel" class="btnCancel" title="Abbrechen" value="Abbrechen" onclick="window.close();" />
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <p>
                                &nbsp;</p>
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
