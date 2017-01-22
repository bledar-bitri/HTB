<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditBooking.aspx.cs" Inherits="HTB.v2.intranetx.aktenint.EditBooking" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>HTB.ASP [ Buchung ]</title>
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
        </style>
</head>
<body>
    <ctl:headerNoMenu ID="hdr" runat="server" />
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true"
        EnableScriptLocalization="true" />
    <table width="100%"  border="0" cellspacing="0" cellpadding="1">
    <tr>
    <td bgcolor="#000000"><table width="100%"  border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
      <tr>
        <td><p>&nbsp;</p>        <table border="0" align="center" cellpadding="3" cellspacing="0">
          <tr>
            <td colspan="2" class="tblHeader" title="INTERVENTION - Neue Buchung ">INTERVENTION - Neue Buchung </td>
            </tr>
          
            <tr>
            <td nowrap class="EditCaption"><div align="right">&nbsp;</div></td>
            <td class="EditData">
                <ctl:message runat="server" ID="ctlMessage"/> 
            </td>
          </tr>
          
          <tr>
            <td class="EditCaption"><div align="right">Akt Nr.:</div></td>
                <td class="EditData">
                    <asp:Label ID="lblAktID" type="text" class="docText" runat="server"/>   &nbsp;
                </td>
          </tr>
          <tr>
            <td class="EditCaption">
                <div align="right">
                    Buchungsart:&nbsp;</div>
            </td>
            <td class="EditData">
                <asp:DropDownList ID="ddlInvoiceType" runat="server" />
            </td>
          </tr>
          <tr>
            <td valign="top" class="EditCaption"><div align="right">Re. Nr.:</div></td>
            <td class="EditData">
                <asp:TextBox class="docText" ID="txtPosNr" onfocus="this.style.backgroundColor='#DFF4FF'; this.select();" onblur="this.style.backgroundColor=''" size="50" maxlength="50" runat="server"/>
            </td>
          </tr>
          <tr>
            <td valign="top" class="EditCaption"><div align="right">Datum:</div></td>
            <td class="EditData">
                <asp:TextBox runat="server" ID="txtPosDate" class="docText"  onFocus="this.style.backgroundColor='#DFF4FF';" onBlur="this.style.backgroundColor='';" size="12" maxlength="10" />
                <ajax:MaskedEditExtender ID="PosDate_MaskedEditExtender" runat="server" Century="2000" TargetControlID="txtPosDate" Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left" ErrorTooltipEnabled="True" />
                <ajax:MaskedEditValidator ID="PosDate_MaskedEditValidator" runat="server" ControlExtender="PosDate_MaskedEditExtender" ControlToValidate="txtPosDate" EmptyValueMessage="Date of contact is required" InvalidValueMessage="Datum is ung&uuml;ltig!" Display="Dynamic" EmptyValueBlurredText="*" InvalidValueBlurredMessage="*" ValidationGroup="PosDateVG" />
                <ajax:CalendarExtender ID="PosDate_CalendarExtender" runat="server" TargetControlID="txtPosDate" PopupButtonID="PosDate_CalendarButton" />
                <asp:ImageButton ID="PosDate_CalendarButton" runat="Server" ImageUrl="~/v2/intranet/images/CalendarHS.png" AlternateText="Click to show calendar" />
            </td>
          </tr>
          <tr>
            <td valign="top" class="EditCaption"><div align="right">Text: </div></td>
            <td class="EditData">
            <asp:TextBox runat="server" ID="txtCaption" class="docText" onfocus="this.style.backgroundColor='#DFF4FF'; this.select();" onblur="this.style.backgroundColor=''" size="50" maxlength="50"/>
            </td>
          </tr>
          <tr>
            <td nowrap class="EditCaption"><div align="right">Betrag &euro;:</div></td>
            <td class="EditData">
                <asp:TextBox runat="server" type="text" ID="txtAmount" class="docText" onfocus="this.style.backgroundColor='#DFF4FF'; this.select();" onblur="this.style.backgroundColor=''" size="15" maxlength="15" /> 
            </td>
          </tr>
          <tr>
            <td nowrap class="EditCaption"><div align="right">F&auml;lligkeitsdatum: </div></td>
            <td class="EditData">
                <asp:TextBox runat="server" type="text" ID="txtDueDate" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="12" maxlength="10"/>
                <ajax:MaskedEditExtender ID="DueDate_MaskedEditExtender" runat="server" Century="2000" TargetControlID="txtDueDate" Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left" ErrorTooltipEnabled="True" />
                <ajax:MaskedEditValidator ID="DueDate_MaskedEditValidator" runat="server" ControlExtender="DueDate_MaskedEditExtender" ControlToValidate="txtDueDate" EmptyValueMessage="Date of contact is required" InvalidValueMessage="Datum is ung&uuml;ltig!" Display="Dynamic" EmptyValueBlurredText="*" InvalidValueBlurredMessage="*" ValidationGroup="DueDateVG" />
                <ajax:CalendarExtender ID="DueDate_CalendarExtender" runat="server" TargetControlID="txtDueDate" PopupButtonID="DueDate_CalendarButton" />
                <asp:ImageButton ID="DueDate_CalendarButton" runat="Server" ImageUrl="~/v2/intranet/images/CalendarHS.png" AlternateText="Click to show calendar" />
            </td>
          </tr>
          <tr>
            <td nowrap class="EditCaption"><div align="right">&Uuml;berweisungsdatum: </div></td>
            <td class="EditData">
                <asp:TextBox runat="server" type="text" ID="txtTrasnferDate" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="12" maxlength="10"/>
                <ajax:MaskedEditExtender ID="TrasnferDate_MaskedEditExtender" runat="server" Century="2000" TargetControlID="txtTrasnferDate" Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left" ErrorTooltipEnabled="True" />
                <ajax:MaskedEditValidator ID="TrasnferDate_MaskedEditValidator" runat="server" ControlExtender="TrasnferDate_MaskedEditExtender" ControlToValidate="txtTrasnferDate" EmptyValueMessage="Date of contact is required" InvalidValueMessage="Datum is ung&uuml;ltig!" Display="Dynamic" EmptyValueBlurredText="*" InvalidValueBlurredMessage="*" ValidationGroup="TrasnferDateVG" />
                <ajax:CalendarExtender ID="TrasnferDate_CalendarExtender" runat="server" TargetControlID="txtTrasnferDate" PopupButtonID="TrasnferDate_CalendarButton" />
                <asp:ImageButton ID="TrasnferDate_CalendarButton" runat="Server" ImageUrl="~/v2/intranet/images/CalendarHS.png" AlternateText="Click to show calendar" />

            </td>
          </tr>
          <tr>
            <td colspan="2" class="tblFooter1"><div align="right">
                <asp:Button runat="server" ID="btnSubmit" class="btnSave" Text="Speichern"  OnClick="btnSubmit_Click"/>
                <input name="Submit2" type="button" class="btnCancel" title="Abbrechen" onClick="window.opener.document.location.reload(); window.close();" value="Abbrechen">
            </div></td>
            </tr>
        </table>          
          <p><br>            
          </p>          </td>
      </tr>
    </table></td>
  </tr>
</table>
    </form>
</body>
</html>
