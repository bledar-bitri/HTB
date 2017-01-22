<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WorkReportInfoScore.aspx.cs" Inherits="HTB.v2.intranetx.aktenint.WorkReportInfoScore" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>HTB.ASP [ Infoscore Bericht ]</title>
    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
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
        .style2
        {
            color: #FF0000;
            font-weight: bold;
        }
        
        OPTION.dis
        {
            background-color: white;
            color: #999999;
        }
    </style>
</head>
<body>
    <ctl:header ID="hdr" runat="server" />
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" EnablePartialRendering="true" EnableScriptLocalization="true" />
    <table width="100%" border="0" cellspacing="0" cellpadding="1">
        <tr>
            <td bgcolor="#000000">
                <table width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
                    <tr>
                        <td>
                            <table border="0" align="center" cellpadding="3" cellspacing="0">
                                <tr>
                                    <td colspan="2" class="tblHeader" title="PROVISIONSBILANZ">
                                        Infoscorebericht
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <img name="" src="" width="1" height="1" alt="" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tblDataAll">
                                        <table border="0" align="center" cellpadding="3" cellspacing="0">
                                            <tr>
                                                <td class="EditCaption">
                                                    <div align="right">
                                                        <strong>Datum von</strong>:&nbsp;</div>
                                                </td>
                                                <td class="EditData" valign="bottom">
                                                    <asp:TextBox ID="txtDateStart" runat="server" MaxLength="10" ToolTip="Zahlungsdatum" onFocus="this.style.backgroundColor='#DFF4FF'" onBlur="this.style.backgroundColor=''"
                                                        size="10" />
                                                    <ajax:MaskedEditExtender ID="Datum_MaskedEditExtender" runat="server" Century="2000" TargetControlID="txtDateStart" Mask="99/99/9999" MessageValidatorTip="true"
                                                        OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" MaskType="Date" ErrorTooltipEnabled="True" />
                                                    <ajax:MaskedEditValidator ID="Datum_MaskedEditValidator" runat="server" ControlExtender="Datum_MaskedEditExtender" ControlToValidate="txtDateStart" InvalidValueMessage="Datum is ung&uuml;ltig!"
                                                        Display="Dynamic" InvalidValueBlurredMessage="*" />
                                                    <ajax:CalendarExtender ID="Datum_CalendarExtender" runat="server" TargetControlID="txtDateStart" PopupButtonID="Datum_CalendarButton" />
                                                    <asp:ImageButton runat="Server" ID="Datum_CalendarButton" ImageUrl="~/v2/intranet/images/CalendarHS.png" AlternateText="Click to show calendar" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="EditCaption">
                                                    <div align="right">
                                                        <strong>Datum Bis</strong>:&nbsp;</div>
                                                </td>
                                                <td class="EditData" valign="bottom">
                                                    <asp:TextBox ID="txtDateEnd" runat="server" MaxLength="10" ToolTip="Zahlungsdatum" onFocus="this.style.backgroundColor='#DFF4FF'" onBlur="this.style.backgroundColor=''"
                                                        size="10" />
                                                    <ajax:MaskedEditExtender ID="Datum_MEE" runat="server" Century="2000" TargetControlID="txtDateEnd" Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                                        OnInvalidCssClass="MaskedEditError" MaskType="Date" ErrorTooltipEnabled="True" />
                                                    <ajax:MaskedEditValidator ID="Datum_MEV" runat="server" ControlExtender="Datum_MEE" ControlToValidate="txtDateEnd" InvalidValueMessage="Datum is ung&uuml;ltig!"
                                                        Display="Dynamic" InvalidValueBlurredMessage="*" />
                                                    <ajax:CalendarExtender ID="Datum_CE" runat="server" TargetControlID="txtDateEnd" PopupButtonID="Datum_CB" />
                                                    <asp:ImageButton runat="Server" ID="Datum_CB" ImageUrl="~/v2/intranet/images/CalendarHS.png" AlternateText="Click to show calendar" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" class="tblFooter1">
                                        <div align="right">
                                            <asp:Button ID="btnSubmit" runat="server" class="btnSave" title="Download" Text="Download" OnClick="btnSubmit_Click" />
                                            <input name="Submit5" type="button" class="btnStandard" title="Zur&uuml;ck." onClick="javascript:history.go(-1)" value="Zur&uuml;ck">
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
    </form>
    <ctl:footer ID="ftr" runat="server" />
</body>
</html>
