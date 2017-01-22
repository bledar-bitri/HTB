<%@ Page Language="C#" Culture="de-DE" AutoEventWireup="true" CodeBehind="NewAction.aspx.cs" Inherits="HTB.v2.intranetx.aktenint.NewAction" %>

<%@ Register TagPrefix="ctl" TagName="installment" Src="~/v2/intranetx/global_files/CtlInstallment.ascx" %>
<%@ Register TagPrefix="ctl" TagName="installmentOld" Src="~/v2/intranetx/global_files/CtlInstallmentOld.ascx" %>
<%@ Register TagPrefix="ctl" TagName="extension" Src="~/v2/intranetx/global_files/CtlExtensionRequest.ascx" %>
<%@ Register TagPrefix="ctl" TagName="telAndEmail" Src="~/v2/intranetx/global_files/CtlTelAndEmailCollection.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>HTB.ASP [ Neue Aktion ]</title>
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
<body id="bdy" runat="server">
    <ctl:headerNoMenu ID="hdr" runat="server" />
    <form id="form1" runat="server" defaultbutton="btnSubmit">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" EnablePartialRendering="true" EnableScriptLocalization="true" />
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        function BeginRequestHandler(sender, args) {
            document.getElementById('<%= btnSubmit.ClientID %>').innerText = "Processing";
            document.getElementById('<%= btnSubmit.ClientID %>').disabled = true;
        }
    </script>
    <asp:UpdatePanel ID="updPanel1" runat="server">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="txtBetrag" EventName="TextChanged" />
        </Triggers>
        <ContentTemplate>
            <table width="100%" border="0" cellspacing="0" cellpadding="1">
                <tr>
                    <td bgcolor="#000000">
                        <table width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
                            <tr>
                                <td>
                                    <p>&nbsp;</p>
                                    <table border="0" align="center" cellpadding="3" cellspacing="0">
                                        <tr>
                                            <td colspan="2" class="tblHeader" title="NEUE AKTION ">
                                                NEUE AKTION
                                            </td>
                                        </tr>
                                        <tr id="trDate" runat="server">
                                            <td width="150" class="EditCaption">
                                                <div align="right">
                                                    Datum:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox runat="server" ID="txtDate" class="docText" onFocus="this.style.backgroundColor='#DFF4FF';" onBlur="this.style.backgroundColor='';" size="15" MaxLength="10" />
                                                <ajax:MaskedEditExtender ID="Date_MaskedEditExtender" runat="server" Century="2000" TargetControlID="txtDate" Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" MaskType="Date" ErrorTooltipEnabled="True" />
                                                <ajax:MaskedEditValidator ID="Date_MaskedEditValidator" runat="server" ControlExtender="Date_MaskedEditExtender" ControlToValidate="txtDate" InvalidValueMessage="Datum ist ung&uuml;ltig!" Display="Dynamic" InvalidValueBlurredMessage="* Ung&uuml;ltig! "  />
                                                <ajax:CalendarExtender ID="Date_CalendarExtender" runat="server" TargetControlID="txtDate" PopupButtonID="Date_CalendarButton" />
                                                <asp:ImageButton runat="Server" ID="Date_CalendarButton" ImageUrl="~/v2/intranet/images/CalendarHS.png" AlternateText="Click to show calendar" />
                                                <asp:DropDownList ID="ddlDate" runat="server" class="docText" onFocus="this.style.backgroundColor='#DFF4FF';" onBlur="this.style.backgroundColor='';" />
                                                Uhrzeit:
                                                <asp:TextBox runat="server" ID="txtTime" type="text" class="docText" size="15" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" />
                                                <ajax:MaskedEditExtender ID="MaskedEditExtender1" runat="server" Century="2000" TargetControlID="txtTime" Mask="99:99" MessageValidatorTip="true" MaskType="Time" ErrorTooltipEnabled="True" />
                                                <ajax:MaskedEditValidator ID="MaskedEditValidator1" runat="server" ControlExtender="MaskedEditExtender1" ControlToValidate="txtTime" InvalidValueMessage="Uhrzeit ist ung&uuml;ltig!" Display="Dynamic" InvalidValueBlurredMessage="* Ung&uuml;ltig! "/>
                                                (ss:mm)
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
                                                <asp:TextBox runat="server" ID="txtBetrag" class="docText" onFocus="this.style.backgroundColor='#DFF4FF';" onBlur="this.style.backgroundColor='';" 
                                                size="15" MaxLength="10" AutoPostBack="true" OnTextChanged="txtBetrag_TextChanged" />
                                                Beleg:&nbsp;&nbsp;
                                                <asp:TextBox runat="server" ID="txtBeleg"  class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" />
                                            </td>
                                        </tr>
                                        <tr id="trAuftraggeberSB" runat="server" Visible="false">
                                            <td class="EditCaption">
                                                <div align="right">lt. Sachbearbeiter (AG):</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox runat="server" ID="txtAuftraggeberSB" class="docText" onFocus="this.style.backgroundColor='#DFF4FF';" onBlur="this.style.backgroundColor='';" size="50" MaxLength="50" />
                                            </td>
                                        </tr>

                                        <tr>
                                            <td colspan="2" nowrap>
                                                <img name="" src="" width="1" height="1" alt="" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <ctl:message ID="ctlMessage" runat="server"/>
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
                                                <asp:Button runat="server" ID="btnSaveWithMissingBeleg" CssClass="btnSave" Text="Trotztem Speichern" onclick="btnSaveWithMissingBeleg_Click"/>
                                            </td>
                                        </tr>

                                        <tr id="trPrice" runat="server" visible="false">
                                            <td valign="middle" class="EditCaptionTopLine">
                                                <div align="right">Preis (ECP Eingang):</div>
                                            </td>
                                            <td class="EditDataTopLine">
                                                <asp:Label runat="server" ID="lblPrice" class="docText" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="middle" class="EditCaptionTopLine">
                                                <div align="right">
                                                    Provision:</div>
                                            </td>
                                            <td class="EditDataTopLine">
                                                <asp:TextBox runat="server" ID="txtProvision" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" />
                                                <asp:Label runat="server" ID="lblProvision" class="docText" ReadOnly="true" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="middle" class="EditCaptionTopLine">
                                                <div align="right">Memo:</div>
                                            </td>
                                            <td class="EditDataTopLine">
                                                <asp:TextBox ID="txtMemo" runat="server" class="docText" Columns="80" onblur="this.style.backgroundColor='';" onfocus="this.style.backgroundColor='#DFF4FF';" Rows="8" TextMode="MultiLine" />
                                            </td>
                                        </tr>

                                        <tr>
                                            <td colspan="2" class="tblFooter1">
                                                <div align="right">
                                                    <asp:Button ID="btnSubmit" runat="server" class="btnSave" title="Speichern" Text="Speichern" OnClick="btnSubmit_Click" />
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
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
    <ctl:footer ID="ftr" runat="server" />
</body>
</html>
