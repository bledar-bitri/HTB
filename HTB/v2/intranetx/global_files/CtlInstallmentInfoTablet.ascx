<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CtlInstallmentInfoTablet.ascx.cs" Inherits="HTB.v2.intranetx.global_files.CtlInstallmentInfoTablet" %>


<table align="center" border="0" cellpadding="3" cellspacing="0">
    <tr>
        <td colspan="2">
            <table align="center" width="100%" border="0" cellpadding="3" cellspacing="0">
                <tr>
                    <th class="tblHeader" colspan="2">
                        Zahlungspflichtiger
                    </th>
                    <th class="tblHeader" colspan="2">
                        Ehepartner/Lebensgef&auml;hrte/B&uuml;rge
                    </th>
                </tr>
                <tr>
                    <td class="EditCaption">
                        Vor-und Zuname:
                    </td>
                    <td class="EditData">
                        <asp:TextBox runat="server" ID="txtName" Width="250" Height="30" CssClass="tabletInput"/>
                    </td>
                    <td class="EditData">
                        <asp:TextBox runat="server" ID="txtNamePartner" Width="250" Height="30" CssClass="tabletInput"/>
                    </td>
                </tr>
                <tr>
                    <td class="EditCaption">
                        Anschrift:
                    </td>
                    <td class="EditData">
                        <asp:TextBox runat="server" ID="txtAddress" Width="300" Height="30" CssClass="tabletInput"/>
                    </td>
                    <td class="EditData" valign="middle">
                        &nbsp;&nbsp;
                        <asp:LinkButton runat="server" ID="lnkMoveAddress" OnClick="lnkMoveAddress_Click">
                            <asp:Image ID="imgMoveAddress" runat="server" ImageUrl="/v2/intranet/images/ForwardIcon.png" AlternateText="" />
                        </asp:LinkButton>
                        &nbsp;&nbsp;
                        <asp:TextBox runat="server" ID="txtAddressPartner" Width="300" Height="30" CssClass="tabletInput" />
                    </td>
                </tr>
                <tr>
                    <td class="EditCaption">
                        Telefon:
                    </td>
                    <td class="EditData">
                        <asp:TextBox runat="server" ID="txtPhoneCity" Height="30" CssClass="tabletInput" Width="40"/>
                        <asp:TextBox runat="server" ID="txtPhone" Height="30" CssClass="tabletInput"/>
                    </td>
                    <td class="EditData">
                        <asp:TextBox runat="server" ID="txtPhonePartner" Height="30" CssClass="tabletInput"/>
                    </td>
                </tr>
                <tr>
                    <td class="EditCaption">
                        Geburtsdatum:
                    </td>
                    <td class="EditData">
                        <asp:TextBox runat="server" ID="txtBirthday" Width="100" Height="30" CssClass="tabletInput"/>
                        <ajax:MaskedEditExtender ID="mee_Birthday" runat="server" Century="2000" TargetControlID="txtBirthday" Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" MaskType="Date" ErrorTooltipEnabled="True" />
                        <ajax:MaskedEditValidator ID="mev_Birthday" runat="server" ControlExtender="mee_Birthday" ControlToValidate="txtBirthday" InvalidValueMessage="Datum ist ung&uuml;ltig!" Display="Dynamic" InvalidValueBlurredMessage="*" />
                        <ajax:CalendarExtender ID="ce_Birthday" runat="server" TargetControlID="txtBirthday" PopupButtonID="imgBtnBirthday" />
                        <asp:ImageButton runat="server" ID="imgBtnBirthday" ImageUrl="~/v2/intranet/images/CalendarHS.png" AlternateText="Click to show calendar" Width="30" Height="30" />
                    </td>
                    <td class="EditData">
                        <asp:TextBox runat="server" ID="txtBirthdayPartner" Width="100" Height="30" CssClass="tabletInput"/>
                        <ajax:MaskedEditExtender ID="mee_BirthdayPartner" runat="server" Century="2000" TargetControlID="txtBirthdayPartner" Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" MaskType="Date" ErrorTooltipEnabled="True" />
                        <ajax:MaskedEditValidator ID="mev_BirthdayPartner" runat="server" ControlExtender="mee_BirthdayPartner" ControlToValidate="txtBirthdayPartner" InvalidValueMessage="Datum ist ung&uuml;ltig!" Display="Dynamic" InvalidValueBlurredMessage="*" />
                        <ajax:CalendarExtender ID="ce_BirthdayPartner" runat="server" TargetControlID="txtBirthdayPartner" PopupButtonID="imgBtnBirthdayPartner" />
                        <asp:ImageButton runat="server" ID="imgBtnBirthdayPartner" ImageUrl="~/v2/intranet/images/CalendarHS.png" AlternateText="Click to show calendar" Width="30" Height="30" />
                    </td>
                </tr>
                <tr>
                    <td class="EditCaption">
                        SVA Nummer:
                    </td>
                    <td class="EditData">
                        <asp:TextBox runat="server" ID="txtSva" Height="30" CssClass="tabletInput"/>
                    </td>
                    <td class="EditData">
                        <asp:TextBox runat="server" ID="txtSvaPartner" Height="30" CssClass="tabletInput"/>
                    </td>
                </tr>
                <tr>
                    <td class="EditCaption">
                        Beruf:
                    </td>
                    <td class="EditData">
                        <asp:TextBox runat="server" ID="txtJob" Width="200" Height="30" CssClass="tabletInput"/>
                    </td>
                    <td class="EditData">
                        <asp:TextBox runat="server" ID="txtJobPartner" Width="200" Height="30" CssClass="tabletInput"/>
                    </td>
                </tr>
                <tr>
                    <td class="EditCaption">
                        Arbeigeber:
                    </td>
                    <td class="EditData">
                        <asp:TextBox runat="server" ID="txtEmployer" Width="200" Height="30" CssClass="tabletInput"/>
                    </td>
                    <td class="EditData">
                        <asp:TextBox runat="server" ID="txtEmployerPartner" Width="200" Height="30" CssClass="tabletInput"/>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <br />
                        <asp:Button runat="server" ID="btnGeneratePdf" OnClick="btnGeneratePdf_Click" Text="Ratenansuchen PDF" Height="30" Font-Size="12pt" CssClass="btnSave" />
                        <br />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<asp:Label runat="server" Visible="false" ID="hdnAktId" />
<asp:Label runat="server" Visible="false" ID="hdnRateAmount" />
<asp:Label runat="server" Visible="false" ID="hdnFirstRate" />
<asp:Label runat="server" Visible="false" ID="hdnPaymentAmount" />
<asp:Label runat="server" Visible="false" ID="hdnLastRate" />
   
<asp:Label runat="server" Visible="false" ID="hdnGegnerId" />