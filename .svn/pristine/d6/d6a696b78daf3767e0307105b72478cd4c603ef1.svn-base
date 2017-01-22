<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CtlInstallment.ascx.cs" Inherits="HTB.v2.intranetx.global_files.CtlInstallment" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:UpdatePanel ID="updPanel1" runat="server">
    <ContentTemplate>
        <table width="100%" border="0" cellspacing="0" cellpadding="1">
            <tr>
                <td bgcolor="#000000">
                    <table width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
                        <tr>
                            <td>
                                <table width="100%" border="0" cellpadding="3" cellspacing="0">
                                    <tr>
                                        <td colspan="2" class="tblHeader">
                                            <asp:Label ID="lblHeader" runat="server"/>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="trInterest">
                                        <td nowrap="nowrap" class="EditCaption">
                                            J&auml;rliche Zinsenprocent
                                        </td>
                                        <td class="EditData">
                                            <asp:TextBox runat="server" ID="txtInterestPct" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor=''" Visible="false"/>
                                            <asp:Label runat="server" ID="lblInterestPct" />
                                            <asp:LinkButton runat="server" ID="btnChangeInterestPct" OnClick="btnChangeInterestPct_Clicked" Text="&Auml;ndern"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td nowrap="nowrap" class="EditCaption">
                                            Datum /Zeit:&nbsp;
                                        </td>
                                        <td class="EditData">
                                            <%= DateTime.Now %>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="EditCaption">
                                            Aktion:&nbsp;
                                        </td>
                                        <td class="EditData">
                                            Ratenvereinbarung
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <img name="" src="" width="1" height="1" alt="" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="EditCaptionTopLine">
                                            Offene Forderung:
                                        </td>
                                        <td class="EditDataTopLine">
                                            <strong>
                                                <asp:Label ID="lblBalance" runat="server" /></strong>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="EditCaption">
                                            Hauptforderung:
                                        </td>
                                        <td class="EditData">
                                            <asp:Label ID="lblOriginalAmount" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="EditCaptionTopLine">
                                            Zahlungsweise:
                                        </td>
                                        <td class="EditDataTopLine">
                                            <asp:DropDownList runat="server" ID="ddlPaymentType">
                                                <asp:ListItem Value="0" Text="Erlagschein" />
                                                <asp:ListItem Value="1" Text="Pers&ouml;nliches CollectionInvoice" />
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="EditCaption">
                                            Startdatum:
                                        </td>
                                        <td class="EditData">
                                            <asp:TextBox runat="server" ID="txtStartDate" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor=''" />
                                            <ajax:MaskedEditExtender ID="Date_MaskedEditExtender" runat="server" Century="2000" TargetControlID="txtStartDate" Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" MaskType="Date" ErrorTooltipEnabled="True" />
                                            <ajax:MaskedEditValidator ID="Date_MaskedEditValidator" runat="server" ControlExtender="Date_MaskedEditExtender" ControlToValidate="txtStartDate" InvalidValueMessage="Datum is ung&uuml;ltig!" Display="Dynamic" InvalidValueBlurredMessage="*"  />
                                            <ajax:CalendarExtender ID="Date_CalendarExtender" runat="server" TargetControlID="txtStartDate" PopupButtonID="Date_CalendarButton" />
                                            <asp:ImageButton runat="Server" ID="Date_CalendarButton" ImageUrl="~/v2/intranet/images/CalendarHS.png" AlternateText="Click to show calendar" />
                                            <asp:HiddenField runat="server" ID="hidStartDatum" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="EditCaption">
                                            Anzahlung:
                                        </td>
                                        <td class="EditData">
                                            <asp:Label runat="server" ID="lblTotalPaid" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="EditCaption">
                                            &nbsp;
                                        </td>
                                        <td class="EditData">
                                            <asp:Label ID="lblMessage" runat="server" Text="&nbsp;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td nowrap="nowrap" class="EditCaption">
                                            Ratenperiod:
                                        </td>
                                        <td class="EditData">
                                            <asp:DropDownList runat="server" ID="ddlInstallmentPeriod">
                                                <asp:ListItem Value="monthly" Text="Jedes Monat" />
                                                <asp:ListItem Value="weekly" Text="Jede Woche" />
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td nowrap="nowrap" class="EditCaption">
                                            Anzahl der Raten:
                                        </td>
                                        <td class="EditData">
                                            <asp:TextBox runat="server" ID="txtNumberOfInstallments" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor=''" value="0" />
                                            <asp:Button runat="server" ID="calcRVNoRates" class="btnStandard" Text="RV berechnen" OnClick="calcRVNoRates_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="EditCaption">
                                            Ratenh&ouml;he:
                                        </td>
                                        <td class="EditData">
                                            <asp:TextBox runat="server" ID="txtInstallmentAmount" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor=''" value="0" />
                                            <asp:Button ID="btnCalcRVRate" runat="server" class="btnStandard" Text="RV berechnen" OnClick="btnCalcRVRate_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="EditCaption">
                                            Letzte Rate:
                                        </td>
                                        <td class="EditData">
                                            <asp:Label runat="server" ID="lblLastInstallment" Text="&nbsp;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="EditCaption">
                                            &nbsp;
                                        </td>
                                        <td class="EditData">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="EditCaption">
                                            Enddatum:
                                        </td>
                                        <td class="EditData">
                                            <asp:Label runat="server" ID="lblEndDatum" Text="&nbsp;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="EditCaption">
                                            Zinsen bis Enddatum:
                                        </td>
                                        <td class="EditData">
                                            <asp:Label runat="server" ID="lblTotalInterest" Text="&nbsp;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="EditCaption">
                                            &nbsp;
                                        </td>
                                        <td class="EditData">
                                            &nbsp;
                                            <asp:GridView ID="gvInstallments" runat="server" AllowSorting="false" AutoGenerateColumns="False" CellPadding="2" BorderStyle="Groove" Width="75%">
                                                <Columns>
                                                    <asp:BoundField HeaderText="Datum" DataField="InstallmentDate" SortExpression="InstallmentDate" ItemStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField HeaderText="Betrag" DataField="InstallmentAmount" SortExpression="InstallmentAmount" ItemStyle-HorizontalAlign="Right" />
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <asp:HiddenField runat="server" ID="hdnCurrentPayment"/>
    </ContentTemplate>
</asp:UpdatePanel>
