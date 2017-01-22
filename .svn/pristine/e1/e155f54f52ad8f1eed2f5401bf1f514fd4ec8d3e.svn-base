<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewGegner.aspx.cs" Inherits="HTB.v2.intranetx.gegner.NewGegner" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>HTB.ASP [ Neuer Gegner ]</title>
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
    <script src="../../intranet/globalcode/lib.js" type="text/javascript" language="javascript"></script>
</head>
<body>
    <ctl:header ID="header" runat="server" />
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
           <asp:AsyncPostBackTrigger ControlID="txtZIP" EventName="TextChanged" />
           <asp:PostBackTrigger ControlID="btnDeltaVista" />
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
                                            <td colspan="2" class="tblHeader" title="GEGNER NEU">
                                                GEGNER NEU
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <ctl:message ID="ctlMessage" runat="server"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <img name="" src="" width="1" height="1" alt="" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" class="tblHeader" title="Adressdaten">
                                                Adressdaten
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right"><strong>Gegner Typ:</strong></div>
                                            </td>
                                            <td class="EditData">
                                                <asp:DropDownList runat="server" ID="ddlGegnerType" class="docText" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">
                                                    Titel:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox runat="server" ID="txtAnrede" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="50" MaxLength="100" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">
                                                    <strong>Nachname / Firma</strong>:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox runat="server" ID="txtName1" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="50" MaxLength="100" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">
                                                    <strong>Vorname</strong>:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox runat="server" ID="txtName2" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="50" MaxLength="100" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">
                                                    Alias:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox runat="server" ID="txtName3" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="50" MaxLength="100" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">
                                                    Strasse:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox runat="server" ID="txtStrasse" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="50"
                                                    MaxLength="100" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td nowrap class="EditCaption">
                                                <div align="right">
                                                    LKZ / PLZ / Ort:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox runat="server" ID="txtZIPPrefix" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" value="A" size="3"
                                                    MaxLength="3" />
                                                -
                                                <asp:TextBox runat="server" ID="txtZIP" AutoPostBack="true" OnTextChanged="txtZip_TextChanged" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="12" MaxLength="12" />
                                                <asp:TextBox runat="server" ID="txtOrt" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="50" MaxLength="50" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td nowrap class="EditCaption">
                                            </td>
                                            <td class="EditData">
                                                <asp:LinkButton runat="server" ID="btnDeltaVista" OnClick="btnDeltaVista_Clicked" Text="Deltavista"></asp:LinkButton>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td colspan="2" nowrap>
                                                <img name="" src="" width="1" height="1" alt="" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" nowrap class="tblHeader">
                                                Kommunikation
                                            </td>
                                        </tr>
                                        <%--<tr>
                                            <td nowrap class="EditCaption">
                                                <div align="right">
                                                    Telefon:</div>
                                            </td>
                                            <td class="EditData">
                                                +
                                                <asp:TextBox runat="server" ID="txtPhoneCountry" class="docText" value="43" size="5" MaxLength="5" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" />
                                                (
                                                <asp:TextBox runat="server" ID="txtPhoneCity" class="docText" size="10" MaxLength="10" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" />
                                                )
                                                <asp:TextBox runat="server" ID="txtPhone" class="docText" size="45" MaxLength="50" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" />
                                            </td>
                                        </tr>--%>
                                        <tr runat="server" id="trTelefon">
                                            <td valign="middle" class="EditCaption">
                                                <div align="right">Telefon:&nbsp;</div>
                                            </td>
                                            <td class="EditData">
                                                <table id="tblPhone" runat="server">
                                
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td nowrap class="EditCaption">
                                                <div align="right">
                                                    Fax:</div>
                                            </td>
                                            <td class="EditData">
                                                +
                                                <asp:TextBox runat="server" ID="txtFaxCountry" class="docText" value="43" size="5" MaxLength="5" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" />
                                                (
                                                <asp:TextBox runat="server" ID="txtFaxCity" class="docText" size="10" MaxLength="10" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" />
                                                )
                                                <asp:TextBox runat="server" ID="txtFax" class="docText" size="45" MaxLength="50" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" />
                                            </td>
                                        </tr>

                                        
                                        <tr>
                                            <td nowrap class="EditCaption">
                                                <div align="right">
                                                    E-Mail:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox runat="server" ID="txtEmail" class="docText" size="50" MaxLength="50" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">
                                                    Ansprech Anrede:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox runat="server" ID="txtAnsprechAnrede" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="50" MaxLength="100" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">
                                                    Ansprech Nachname:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox runat="server" ID="txtAnsprech" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="50" MaxLength="100" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">
                                                    Ansprech Vorname:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox runat="server" ID="txtAnsprechVorname" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="50" MaxLength="100" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <img name="" src="" width="1" height="1" alt="">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" class="tblHeader" title="Verm&ouml;gensverzeichnis">
                                                Verm&ouml;gensverzeichnis
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">
                                                    VVZ Abgelegt:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:CheckBox runat="server" ID="chkVVZ" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">
                                                    Datum:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox runat="server" ID="txtVVZDatum" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="12" MaxLength="10" />
                                                <ajax:MaskedEditExtender ID="Datum_MaskedEditExtender" runat="server" Century="2000" TargetControlID="txtVVZDatum" Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" MaskType="Date" ErrorTooltipEnabled="True" />
                                                <ajax:MaskedEditValidator ID="Datum_MaskedEditValidator" runat="server" ControlExtender="Datum_MaskedEditExtender" ControlToValidate="txtVVZDatum" InvalidValueMessage="Datum is ung&uuml;ltig!" Display="Dynamic" InvalidValueBlurredMessage="*"  />
                                                <ajax:CalendarExtender ID="Datum_CalendarExtender" runat="server" TargetControlID="txtVVZDatum" PopupButtonID="VVZDatum_CalendarButton" />
                                                <asp:ImageButton runat="Server" ID="VVZDatum_CalendarButton" ImageUrl="~/v2/intranet/images/CalendarHS.png" AlternateText="Click to show calendar" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <img name="" src="" width="1" height="1" alt="" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" class="tblHeader" title="GebDat / Bemerkungen">
                                                GebDat / Bemerkungen
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EditCaption">
                                                <div align="right">
                                                    Geburtsdatum:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox runat="server" ID="txtGebDat" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor='';" size="12" MaxLength="10" />
                                                <ajax:MaskedEditExtender ID="MaskedEditExtender1" runat="server" Century="2000" TargetControlID="txtGebDat" Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" MaskType="Date" ErrorTooltipEnabled="True" />
                                                <ajax:MaskedEditValidator ID="MaskedEditValidator1" runat="server" ControlExtender="Datum_MaskedEditExtender" ControlToValidate="txtGebDat" InvalidValueMessage="Datum is ung&uuml;ltig!" Display="Dynamic" InvalidValueBlurredMessage="*"  />
                                                <ajax:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtGebDat" PopupButtonID="GebDatum_CalendarButton" />
                                                <asp:ImageButton runat="Server" ID="GebDatum_CalendarButton" ImageUrl="~/v2/intranet/images/CalendarHS.png" AlternateText="Click to show calendar" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" class="EditCaption">
                                                <div align="right">
                                                    Beschreibung:</div>
                                            </td>
                                            <td class="EditData">
                                                <asp:TextBox runat="server" ID="txtMemo" TextMode="MultiLine" Columns="80" Rows="8" class="docText" onFocus="this.style.backgroundColor='#DFF4FF';" onBlur="this.style.backgroundColor='';" />
                                                <br>
                                                <input name="Submit3" type="button" class="btnStandard" value="Zeitstempel" title="Hier können Sie den Zeitstempel setzten." onclick="GBInsertTimeStamp(document.forms(0).txtMemo,'<%=Session["MM_VorName"] + " " + Session["MM_NachName"] + " " + DateTime.Now %>');">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" class="tblFooter1">
                                                <div align="right">
                                                    <asp:Button ID="btnSubmit" runat="server" class="btnSave" title="Speichern " Text="Speichern" onclick="btnSubmit_Click" />
                                                    <asp:Button ID="btnCancel" runat="server" class="btnCancel" title="Abbrechen" Text="Abbrechen" onclick="btnCancel_Click" />
                                                </div>
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
            <asp:HiddenField runat="server" ID="hdnNumberOfPhones"/>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
    <ctl:footer ID="ftr" runat="server" />
</body>
</html>
