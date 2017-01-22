<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditInkAction.aspx.cs" Inherits="HTB.v2.intranetx.aktenink.EditInkAction" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>HTB.ASP [ CollectionInvoice - Buchung editieren ]</title>
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
    </style>
</head>
<body id="bdy" runat="server">
    <ctl:headerNoMenu runat="server" ID="hdr" />
    <form id="form1" runat="server">
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
            
        </Triggers>
        <ContentTemplate>
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
                                            <td colspan="2" class="tblHeader" title="INTERVENTION - Buchung editieren ">
                                                INKASSO - Aktion editieren
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" class="tblDataAll">
                                                <ctl:message ID="ctlMessage" runat="server" />
                                            </td>
                                        </tr>
                                         <tr>
                                            <td valign="top" class="EditCaption">
                                                <div align="right">
                                                    Re. Nr.:</div>
                                            </td>
                                            <td class="EditData">
                                                <strong><asp:Label runat="server" ID="lblActionId" /></strong>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">Akt Nr.:</div>
                                            </td>
                                            <td class="EditData">
                                                <strong><asp:Label runat="server" ID="lblAktId" /></strong>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" class="EditCaption">
                                                <div align="right">Datum:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox runat="server" ID="txtActionDate" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="12" MaxLength="10" />
                                                <ajax:MaskedEditExtender ID="ActionDate_MaskedEditExtender" runat="server" Century="2000" TargetControlID="txtActionDate" Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" MaskType="Date" ErrorTooltipEnabled="True" />
                                                <ajax:MaskedEditValidator ID="ActionDate_MaskedEditValidator" runat="server" ControlExtender="ActionDate_MaskedEditExtender" ControlToValidate="txtActionDate" InvalidValueMessage="Datum is ung&uuml;ltig!" Display="Dynamic" InvalidValueBlurredMessage="*" />
                                                <ajax:CalendarExtender ID="ActionDate_CalendarExtender" runat="server" TargetControlID="txtActionDate" PopupButtonID="ActionDate_CalendarButton" />
                                                <asp:ImageButton runat="Server" ID="ActionDate_CalendarButton" ImageUrl="~/v2/intranet/images/CalendarHS.png" AlternateText="Click to show calendar" />
                                                (tt.mm.jjjj)
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" class="EditCaption">
                                                <div align="right">Text:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox runat="server" ID="txtCaption" class="docText" onfocus="this.style.backgroundColor='#DFF4FF'; this.select();" onblur="this.style.backgroundColor=''" size="50" MaxLength="50" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" class="EditCaption">
                                                <div align="right">
                                                    Beschreibung (grund für die Enderung):</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox ID="txtMemo" runat="server" class="docText" Columns="80" onblur="this.style.backgroundColor='';" onfocus="this.style.backgroundColor='#DFF4FF';" Rows="8"
                                                    TextMode="MultiLine" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" class="tblFooter1">
                                                <div align="right">
                                                    <asp:Button runat="server" ID="btnSubmit" class="btnSave" title="Speichern" Text="Speichern" OnClick="btnSubmit_Click" />
                                                    <asp:Button runat="server" ID="btnCancel" class="btnCancel" title="Abbrechen" Text="Abbrechen" OnClick="btnCancel_Click" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                    <p>
                                        <br />
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
