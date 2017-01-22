<%@ Page Language="C#" Culture="de-DE" AutoEventWireup="true" CodeBehind="AuftragReceipt.aspx.cs" Inherits="HTB.v2.intranetx.klienten.AuftragReceipt" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>HTB.ASP [ Auftragsbestaetigung drucken ]</title>
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
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" EnablePartialRendering="true" EnableScriptLocalization="true" />
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
                                    <td colspan="2" class="tblHeader" title="FORDERUNGSAUFSTELLUNG DRUCKEN">
                                        AUFTRAGSBESTÄTIGUNG DRUCKEN
                                    </td>
                                </tr>
                                <tr>
                                    <td class="EditCaption">
                                        <div align="right">Klient:</div>
                                    </td>
                                    <td class="EditData">
                                        <asp:Label ID="lblKlient" runat="server" Enabled="false" class="docText" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="EditCaption">
                                        <div align="right">Ansprech:</div>
                                    </td>
                                    <td class="EditData">
                                        <asp:Label ID="lblAnsprech" runat="server" Enabled="false" class="docText" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <img name="" src="" width="1" height="1" alt="">
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" class="EditCaption">
                                        <div align="right">Akt Datum vom:</div>
                                    </td>
                                    <td class="EditData">
                                        <asp:TextBox ID="txtFromDate" runat="server" class="docText" />
                                        <ajax:MaskedEditExtender ID="Datum_MaskedEditExtender" runat="server" Century="2000" TargetControlID="txtFromDate" Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" MaskType="Date" ErrorTooltipEnabled="True" />
                                        <ajax:MaskedEditValidator ID="Datum_MaskedEditValidator" runat="server" ControlExtender="Datum_MaskedEditExtender" ControlToValidate="txtFromDate" InvalidValueMessage="Datum is ung&uuml;ltig!" Display="Dynamic" InvalidValueBlurredMessage="*"  />
                                        <ajax:CalendarExtender ID="Datum_CalendarExtender" runat="server" TargetControlID="txtFromDate" PopupButtonID="FromDate_CalendarButton" />
                                        <asp:ImageButton runat="Server" ID="FromDate_CalendarButton" ImageUrl="~/v2/intranet/images/CalendarHS.png" AlternateText="Click to show calendar" />
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" class="EditCaption">
                                        <div align="right">Akt Datum bis:</div>
                                    </td>
                                    <td class="EditData">
                                        <asp:TextBox ID="txtToDate" runat="server" class="docText" />
                                        <ajax:MaskedEditExtender ID="MaskedEditExtender1" runat="server" Century="2000" TargetControlID="txtToDate" Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" MaskType="Date" ErrorTooltipEnabled="True" />
                                        <ajax:MaskedEditValidator ID="MaskedEditValidator1" runat="server" ControlExtender="Datum_MaskedEditExtender" ControlToValidate="txtToDate" InvalidValueMessage="Datum is ung&uuml;ltig!" Display="Dynamic" InvalidValueBlurredMessage="*"  />
                                        <ajax:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtToDate" PopupButtonID="ToDate_CalendarButton" />
                                        <asp:ImageButton runat="Server" ID="ToDate_CalendarButton" ImageUrl="~/v2/intranet/images/CalendarHS.png" AlternateText="Click to show calendar" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <img name="" src="" width="1" height="1" alt="">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="EditCaption">
                                        <div align="right">Klient Sachbearbeiter:</div>
                                    </td>
                                    <td class="EditData">
                                        <asp:DropDownList ID="ddlKlientSB" runat="server" class="docText"/>&nbsp;&nbsp;
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
                            <p>&nbsp;</p>
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
