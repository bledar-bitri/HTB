<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CtlWorkflow.ascx.cs" Inherits="HTB.v2.intranetx.global_files.CtlWorkflow" %>
<%@ Register TagPrefix="ctl" TagName="message" Src="~/v2/intranetx/global_files/CtlMessage.ascx" %>
<asp:UpdatePanel runat="server">
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="cmdLoadWFL" EventName="Click" />
    </Triggers>
    <ContentTemplate>
        <table border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
            <tr>
                <td colspan="3" class="tblHeader" title="INTERVENTION EDITIEREN " align="left">
                    WORKFLOW EDITIEREN
                </td>
            </tr>
            <tr runat="server" id="trLastAction">
                <td valign="middle" align="right" class="EditCaption">
                    Vorherige Aktion:
                </td>
                <td class="tblDataLeftBottomRight" align="left" colspan="2">
                    <strong><asp:Label ID="lblCurrentAction" runat="server" Text="&nbsp;" /></strong>
                </td>
            </tr>
            <tr runat="server" id="trNextAction">
                <td valign="middle" align="right" class="EditCaption">
                    Nächste Aktion:
                </td>
                <td class="tblDataLeftBottomRight" align="left" colspan="2">
                    <asp:DropDownList ID="ddlNextAction" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlNextAction_SelectedIndexChanged" />
                </td>
            </tr>
            <tr runat="server" id="trDate">
                <td valign="middle" align="right" class="EditCaption">
                    Nächste Aktionsdatum:
                </td>
                <td class="tblDataBottomRight" align="left" colspan="2">
                    <asp:TextBox ID="txtNextActionExecDate" runat="server" MaxLength="10" ToolTip="Nächste Aktionsdatum (wenn das Datum nicht ausgefuellt wird, dann wird das heutige Datum verwendet)"
                        onFocus="this.style.backgroundColor='#DFF4FF'" onBlur="this.style.backgroundColor=''" size="10" />
                    <ajax:MaskedEditExtender ID="Datum_MaskedEditExtender2" runat="server" Century="2000" TargetControlID="txtNextActionExecDate" Mask="99/99/9999" MessageValidatorTip="true"
                        OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" MaskType="Date" ErrorTooltipEnabled="True" />
                    <ajax:MaskedEditValidator ID="MaskedEditValidator1" runat="server" ControlExtender="Datum_MaskedEditExtender2" ControlToValidate="txtNextActionExecDate" InvalidValueMessage="Datum is ung&uuml;ltig!"
                        Display="Dynamic" InvalidValueBlurredMessage="*" />
                    <ajax:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtNextActionExecDate" PopupButtonID="Datum_CalendarButton2" />
                    <asp:ImageButton runat="Server" ID="Datum_CalendarButton2" ImageUrl="~/v2/intranet/images/CalendarHS.png" AlternateText="Click to show calendar" />
                    <asp:Label runat="server" ID="lblDateDescription" />
                </td>
            </tr>
            <tr runat="server" id="trMainStatus">
                <td valign="middle" align="right" class="EditCaption">
                    Hauptstatus:
                </td>
                <td class="tblDataBottomRight" align="left" colspan="2">
                    <asp:DropDownList ID="ddlAktMainStatus" runat="server" />
                </td>
            </tr>
            <tr runat="server" id="trSecondaryStatus">
                <td valign="middle" align="right" class="EditCaption">
                    Aktstatus:
                </td>
                <td class="tblDataBottomRight" align="left" colspan="2">
                    <asp:DropDownList ID="ddlAktCurrentStatus" runat="server" />
                </td>
            </tr>
            <tr runat="server" id="trStopWfl">
                <td valign="middle" align="right" class="EditCaption">
                    Workflow halten:
                </td>
                <td class="tblDataBottomRight" align="left" colspan="2">
                    <asp:CheckBox ID="chkStopWorkflow" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="4" class="tblDataAll" align="center">
                    <ctl:message ID="ctlMessage" runat="server" />
                </td>
            </tr>
            <tr runat="server" id="trLoadWflFromClient">
                <td valign="top" align="left" class="tblDataLeftBottomRight" colspan="4">
                    <asp:LinkButton ID="cmdLoadWFL" runat="server" class="smallText" Text="load vom Klient" OnClick="cmdLoadWFL_Click" />
                </td>
            </tr>
            <tr>
                <th class="tblHeader2" align="center">
                    &nbsp;Auswahl&nbsp;
                </th>
                <th class="tblHeader2" align="center">
                    &nbsp;Aktion&nbsp;
                </th>
                <th class="tblHeader2" align="center">
                    &nbsp;&nbsp;Dauer (Tage)&nbsp;&nbsp;
                </th>
            </tr>
            <tr>
                <td class="tblCol4Sel" align="center">
                    <asp:CheckBox ID="chk1Mah" runat="server"/>
                </td>
                <td class="tblData1" align="left">
                    &nbsp;&nbsp;&nbsp;Mahnung
                </td>
                <td class="tblDataBottomRight" align="center">
                    <asp:TextBox ID="txtDuration1Mah" runat="server" size="9" MaxLength="3" Style="text-align: right"/>
                </td>
            </tr>
            <tr>
                <td class="tblCol4Sel" align="center">
                    <asp:CheckBox ID="chkTelefonInk" runat="server" />
                </td>
                <td class="tblData1" align="left">
                    &nbsp;&nbsp;&nbsp;Telefoninkasso
                </td>
                <td class="tblDataBottomRight" align="center">
                    <asp:TextBox ID="txtDurationTelefon" runat="server" size="9" MaxLength="3" Style="text-align: right" />
                </td>
            </tr>
            <tr>
                <td class="tblCol4Sel" align="center">
                    <asp:CheckBox ID="chkIntervention" runat="server"/>
                </td>
                <td class="tblData1" align="left">
                    &nbsp;&nbsp;&nbsp;Intervention
                </td>
                <td class="tblDataBottomRight" align="center">
                    <asp:TextBox ID="txtDurationIntervention" runat="server" size="9" MaxLength="3" Style="text-align: right"/>
                </td>
            </tr>
            <tr>
                <td class="tblCol4Sel" align="center">
                    <asp:CheckBox ID="chkTelefonInkAfterInt" runat="server"/>
                </td>
                <td class="tblData1" align="left">
                    &nbsp;&nbsp;&nbsp;Telefoninkasso (nach Intervention)
                </td>
                <td class="tblDataBottomRight" align="center">
                    <asp:TextBox ID="txtDurationTelefonAfterInt" runat="server" size="9" MaxLength="3" Style="text-align: right"/>
                </td>
            </tr>
            <tr>
                <td class="tblCol4Sel" align="center">
                    <asp:CheckBox ID="chk2Mah" runat="server"/>
                </td>
                <td class="tblData1" align="left">
                    &nbsp;&nbsp;&nbsp;Mahnung
                </td>
                <td class="tblDataBottomRight" align="center">
                    <asp:TextBox ID="txtDuration2Mah" runat="server" size="9" MaxLength="3" Style="text-align: right"/>
                </td>
            </tr>
            <tr>
                <td class="tblCol4Sel" align="center">
                    <asp:CheckBox ID="chk3Mah" runat="server" />
                </td>
                <td class="tblData1" align="left">
                    &nbsp;&nbsp;&nbsp;Mahnung
                </td>
                <td class="tblDataBottomRight" align="center">
                    <asp:TextBox ID="txtDuration3Mah" runat="server" size="9" MaxLength="3" Style="text-align: right" />
                </td>
            </tr>
            <tr>
                <td class="tblCol4Sel" align="center">
                    <asp:CheckBox ID="chkRechtsanwalt" runat="server"/>
                </td>
                <td class="tblData1" align="left">
                    &nbsp;&nbsp;&nbsp;Rechtsanwalt
                </td>
                <td class="tblDataBottomRight" align="center">
                    <asp:TextBox ID="txtDurationRechtsanwalt" runat="server" size="9" MaxLength="3" Style="text-align: right"/>
                </td>
            </tr>
            <tr>
                <td class="tblCol4Sel" align="center">
                    <asp:CheckBox ID="chkRechtsanwaltErinerung" runat="server"/>
                </td>
                <td class="tblData1" align="left">
                    &nbsp;&nbsp;&nbsp;Erinerung EMail an Rechtsanwalt
                </td>
                <td class="tblDataBottomRight" align="center">
                    <asp:TextBox ID="txtDurationRechtsanwaltErinerung" runat="server" size="9" MaxLength="3" Style="text-align: right"/>
                </td>
            </tr>
            <tr>
                <td colspan="3" class="tblDataLeftRight" align="left">
                    <asp:LinkButton ID="btnUpdateStatus" runat="server" class="smallText" OnClick="btnUpdateStatus_Click" Text="Status speichern" title="Status speichern" Visible="false"/>
                </td>
            </tr>
            <tr>
                <th colspan="3" align="center" class="tblDataLeftBottomRight">
                    <b>Dienstleistung Intervention</b>
                </th>
            </tr>
            <tr>
                <td class="tblCol4Sel" align="center">
                    <asp:CheckBox ID="chkZMR" runat="server" Checked="true" />
                </td>
                <td class="tblData1" align="left">
                    &nbsp;&nbsp;&nbsp;ZMR Abfrage
                </td>
                <td class="tblDataBottomRight">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="tblCol4Sel" align="center">
                    <asp:CheckBox ID="chkAE" runat="server" Checked="true" />
                </td>
                <td class="tblData1" align="left">
                    &nbsp;&nbsp;&nbsp;AE
                </td>
                <td class="tblDataBottomRight">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="tblCol4Sel" align="center">
                    <asp:CheckBox ID="chkKFZ" runat="server" Checked="true" />
                </td>
                <td class="tblData1" align="left">
                    &nbsp;&nbsp;&nbsp;KFZ
                </td>
                <td class="tblDataBottomRight">
                    &nbsp;
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
