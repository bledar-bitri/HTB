<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CtlInstallmentOld.ascx.cs" Inherits="HTB.v2.intranetx.global_files.CtlInstallmentOld" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<table width="100%" border="0" cellspacing="0" cellpadding="1">
    <tr>
        <td bgcolor="#000000">
            <table width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
                <tr>
                    <td>
                        <table  width="100%" border="0" align="center" cellpadding="3" cellspacing="0">
                            <tr>
                                <td colspan="2" class="tblHeader" title="RATENVEREINBARUNG">
                                    RATENVEREINBARUNG
                                </td>
                            </tr>
                            <tr id="trMessage" runat="server" visible="false">
                                <td  colspan="2" class="tblDataLeftBottomRight">
                                    <asp:Label ID="lblMessage" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="EditCaption">
                                    Gesamtforderung:
                                </td>
                                <td class="EditData">
                                    <asp:Label ID="lblTotal" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="EditCaption">
                                    <div align="right">
                                        Zahlungsweise:</div>
                                </td>
                                <td class="EditData">
                                    <asp:DropDownList ID="ddlZW" runat="server" class="docText" >
                                        <asp:ListItem Text="*** Bitte ausw&auml;hlen ***" Value="999" />
                                        <asp:ListItem Text="Erlagschein" Value="0" />
                                        <asp:ListItem Text="Pers&ouml;nliches CollectionInvoice" Value="1" />
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="EditCaption">
                                    <div align="right">
                                        Beginndatum:</div>
                                </td>
                                <td class="EditData">
                                    <asp:TextBox ID="txtDate" runat="server" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="15" MaxLength="10" />
                                    <ajax:MaskedEditExtender ID="Date_MaskedEditExtender" runat="server" Century="2000" TargetControlID="txtDate" Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" MaskType="Date" ErrorTooltipEnabled="True" />
                                    <ajax:MaskedEditValidator ID="Date_MaskedEditValidator" runat="server" ControlExtender="Date_MaskedEditExtender" ControlToValidate="txtDate" InvalidValueMessage="Datum is ung&uuml;ltig!" Display="Dynamic" InvalidValueBlurredMessage="*"  />
                                    <ajax:CalendarExtender ID="Date_CalendarExtender" runat="server" TargetControlID="txtDate" PopupButtonID="Date_CalendarButton" />
                                    <asp:ImageButton runat="Server" ID="Date_CalendarButton" ImageUrl="~/v2/intranet/images/CalendarHS.png" AlternateText="Click to show calendar" />
                                    (tt.mm.jjjj)
                                </td>
                            </tr>
                            <tr>
                                <td class="EditCaption">
                                    <div align="right">
                                        Betrag/Monat:</div>
                                </td>
                                <td class="EditData">
                                    <asp:TextBox runat="server" ID="txtBetrag" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="15" MaxLength="10" />
                                </td>
                            </tr>
                            <tr>
                                <td valign="middle" class="EditCaption">
                                    <div align="right">
                                        immer zum:</div>
                                </td>
                                <td class="EditData">
                                    <asp:TextBox runat="server" ID="txtDay" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="15" MaxLength="10" />
                                    . des Monates
                                </td>
                            </tr>
                            <tr>
                                <td valign="middle" class="EditCaption">
                                    <div align="right">
                                        Laufzeit:</div>
                                </td>
                                <td class="EditData">
                                    <asp:TextBox runat="server" ID="txtDuration" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="15" MaxLength="10" />
                                    Monate
                                </td>
                            </tr>
                            <tr>
                                <td valign="middle" class="EditCaption">
                                    <div align="right">
                                        Zahlungsrhythmus:&nbsp;</div>
                                </td>
                                <td class="EditData">
                                    <asp:TextBox runat="server" ID="txtPaymentRythm" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="40"
                                        MaxLength="40" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
