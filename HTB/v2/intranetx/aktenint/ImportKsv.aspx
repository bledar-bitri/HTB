﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImportKsv.aspx.cs" Inherits="HTB.v2.intranetx.aktenint.ImportKsv" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>HTB.ASP [ Import Interventionsakte ]</title>
    <link href="../../intranet/styles/inksys.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        body
        {
            margin-left: 5px;
            margin-top: 5px;
            margin-right: 5px;
            margin-bottom: 5px;
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
        .style1
        {
            color: #FF0000;
        }
        OPTION.dis
        {
            background-color: white;
            color: #999999;
        }
    </style>
</head>
<body>
    <ctl:headerNoMenu runat="server"/>
    <form id="form1" runat="server" enctype="multipart/form-data">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" EnablePartialRendering="true" EnableScriptLocalization="true" />
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        function BeginRequestHandler(sender, args) {
            document.getElementById('<%= btnSubmit.ClientID %>').innerText = "Processing";
            document.getElementById('<%= btnSubmit.ClientID %>').disabled = true;
            document.getElementById('<%= btnCancel.ClientID %>').innerText = "Processing";
            document.getElementById('<%= btnCancel.ClientID %>').disabled = true;
        }
    </script>
    <table width="100%" border="0" cellpadding="1" cellspacing="0" bgcolor="#000000">
        <tr>
            <td>
                <table width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
                    <tr>
                        <td class="smallText">
                            <a href="../../intranet/intranet/intranet.asp">Intranet</a> | <a href="../../intranet/intranet/inkasso.asp">CollectionInvoice</a> | <a href="../../intranet/aktenint/aktenint.asp">
                                Interventionsakte (&Uuml;bersicht)</a> | Import Interventionsakte
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <br />
    
    <table width="100%" border="0" cellspacing="0" cellpadding="1">
        <tr>
            <td bgcolor="#000000">
                <table width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
                    <tr>
                        <td>
                            <table border="0" align="center" cellpadding="3" cellspacing="0">
                                <tr>
                                    <td colspan="2" class="tblHeader" title="INTERVENTION">
                                        IMPORT INTERVENTIONSAKTEN
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" class="tblDataAll">
                                        <ctl:message ID="ctlMessage" runat="server" />
                                    </td>
                                </tr>

                                <tr>
                                    <td class="EditCaption">
                                        <div align="right">Datei:</div>
                                    </td>
                                    <td class="tblDataAll">
                                        <INPUT ID="fileUpload" type="file" name="File1" runat="server" size="100"/>
                                    </td>
                                </tr>

                                <tr>
                                    <td class="EditCaption">
                                        <div align="right">Einlegsdatum:</div>
                                    </td>
                                    <td class="EditData">
                                        <asp:TextBox ID="txtEnterDate" runat="server" MaxLength="10" size="10" />
                                        <ajax:MaskedEditExtender ID="Datum_MaskedEditExtender" runat="server" Century="2000" TargetControlID="txtEnterDate" Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" MaskType="Date" ErrorTooltipEnabled="True" />
                                        <ajax:MaskedEditValidator ID="Datum_MaskedEditValidator" runat="server" ControlExtender="Datum_MaskedEditExtender" ControlToValidate="txtEnterDate" InvalidValueMessage="Datum is ung&uuml;ltig!" Display="Dynamic" InvalidValueBlurredMessage="*"  />
                                        <ajax:CalendarExtender ID="Datum_CalendarExtender" runat="server" TargetControlID="txtEnterDate" PopupButtonID="Datum_CalendarButton" />
                                        <asp:ImageButton runat="Server" ID="Datum_CalendarButton" ImageUrl="~/v2/intranet/images/CalendarHS.png" AlternateText="Click to show calendar" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="EditCaption">
                                        <div align="right">Termindatum:</div>
                                    </td>
                                    <td class="EditData">
                                        <asp:TextBox ID="txtDueDate" runat="server" MaxLength="10" size="10"/>
                                        <ajax:MaskedEditExtender ID="DueDate_MaskedEditExtender" runat="server" Century="2000" TargetControlID="txtDueDate" Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" MaskType="Date" ErrorTooltipEnabled="True" />
                                        <ajax:MaskedEditValidator ID="DueDate_MaskedEditValidator" runat="server" ControlExtender="DueDate_MaskedEditExtender" ControlToValidate="txtDueDate" InvalidValueMessage="Datum is ung&uuml;ltig!" Display="Dynamic" InvalidValueBlurredMessage="*"  />
                                        <ajax:CalendarExtender ID="DueDate_CalendarExtender" runat="server" TargetControlID="txtDueDate" PopupButtonID="DueDate_CalendarButton" />
                                        <asp:ImageButton runat="Server" ID="DueDate_CalendarButton" ImageUrl="~/v2/intranet/images/CalendarHS.png" AlternateText="Click to show calendar" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="EditCaption">
                                        <div align="right">Aussendienst:</div>
                                    </td>
                                    <td class="EditData">
                                        <asp:DropDownList ID="ddlSB" runat="server" class="docText" />
                                
                                    </td>
                                </tr>

                                <tr>
                                    <td class="EditCaption">
                                        <div align="right">Dubiose Akten:</div>
                                    </td>
                                    <td class="EditData">
                                        <asp:CheckBox ID="chkIsDub" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" class="tblFooter1">
                                        <div align="right">
                                            <asp:Button ID="btnSubmit" runat="server" class="btnSave" title="Speichern" Text="Speichern" OnClick="btnSubmit_Click" />
                                            <asp:Button ID="btnCancel" runat="server" class="btnCancel" title="Abbrechen" OnClick="btnCancel_Click" Text="Abbrechen" />
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
    <ctl:footer runat="server"/>
</body>
</html>
