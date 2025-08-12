<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BingRouteActionsDisplay.aspx.cs" Inherits="HTB.v2.intranetx.routeplanner.BingRouteActionsDisplay" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="/v2/intranet/styles/inksys.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        body
        {
            margin-left: 5px;
            margin-top: 5px;
            margin-right: 5px;
            margin-bottom: 5px;
            background-image: url(../images/osxback.gif);
        }
        
        #mapDiv {
          position: fixed;
          right: 0px;
          top: 0px;
          width: 100%;
          height: 300px;
        }
        
        OPTION.dis
        {
            background-color: white;
            color: #999999;
        }
        .style1
        {
            color: #999999;
        }
        .style2
        {
            color: #CCCCCC;
        }
        .detailStyle1 
        {
            background-color: #E0FFFF;
        }
        .detailStyle 
        {
            background-color: #EFFFE0;
        }
    </style>
    
</head>
<body onload="GetMap();">
<script type="text/javascript" src="http://ecn.dev.virtualearth.net/mapcontrol/mapcontrol.ashx?v=7.0&&mkt=de-DE"></script>
    <script type="text/javascript">
          var map = null;

          function GetMap() {
              // Initialize the map
              map = new Microsoft.Maps.Map(
                  document.getElementById("mapDiv"), {
                      credentials: "<%= HTB.v2.intranetx.routeplanner.RoutePlanerManager.BingMapsKey %>",
                      mapTypeId: Microsoft.Maps.MapTypeId.road
                  });
              
            map.entities.clear();
            map.entities.clear();
            var offset = new Microsoft.Maps.Point(0, 5);
               
            <%= JsPushpins %>
            
            map.setView({ center: new Microsoft.Maps.Location(47.82115, 13.05845), zoom: 7 });
          }
          
          pushPinMouseOver = function (e) {

            var pin = e.target;
            if (pin != null && pin.Name != "") {
                var id = parseInt(pin.Name);
                var row = document.getElementById("CoordinateRow_" + id);
                row.style.backgroundColor = '#2dc6ee';
            }
        }
          pushPinMouseOut = function (e) {

            var pin = e.target;
            if (pin != null && pin.Name != "") {
                var id = parseInt(pin.Name);
                var row = document.getElementById("CoordinateRow_" + id);
                row.style.backgroundColor = '#ffffff';
            }
        }
    </script>
    <ctl:headerNoMenu ID="header" runat="server" />
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" EnablePartialRendering="true" EnableScriptLocalization="true" />
    <div style="position:absolute; left:0px; top:500px;">
    <table width="100%" border="0" cellspacing="0" cellpadding="1">
        <tr>
            <td bgcolor="#000000">
                <table width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
                    <tr>
                        <td valign="top">
                            <table>
                                <tr>
                                    <td>
                                        <table border="0" align="center" cellpadding="3" cellspacing="0">
                                            <tr>
                                                <td colspan="2" class="tblHeader" title="Aktionen">
                                                    Aktionen
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" class="tblDataAll">
                                                    <ctl:message ID="ctlMessage" runat="server" />
                                                </td>
                                            </tr>
                                            <tr id="tr1" runat="server">
                                                <td valign="top" class="EditCaption">
                                                    <div align="right">
                                                        Inkassant:</div>
                                                </td>
                                                <td class="EditData">
                                                    <asp:DropDownList ID="ddlUser" runat="server" />
                                                </td>
                                            </tr>
                                            <tr id="trDate" runat="server">
                                                <td class="EditCaption">
                                                    <div align="right">
                                                        <strong>Datum (vom)</strong>:&nbsp;</div>
                                                </td>
                                                <td class="EditData" valign="bottom">
                                                    <asp:TextBox ID="txtDateFrom" runat="server" MaxLength="10" ToolTip="Datum vom" onFocus="this.style.backgroundColor='#DFF4FF'" onBlur="this.style.backgroundColor=''"
                                                        size="10" />
                                                    <ajax:MaskedEditExtender ID="Datum_MaskedEditExtenderToFrom" runat="server" Century="2000" TargetControlID="txtDateFrom" Mask="99/99/9999" MessageValidatorTip="true"
                                                        OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" MaskType="Date" ErrorTooltipEnabled="True" />
                                                    <ajax:MaskedEditValidator ID="Datum_MaskedEditValidatorToFrom" runat="server" ControlExtender="Datum_MaskedEditExtenderToFrom" ControlToValidate="txtDateFrom" InvalidValueMessage="Datum is ung&uuml;ltig!"
                                                        Display="Dynamic" InvalidValueBlurredMessage="*" />
                                                    <ajax:CalendarExtender ID="Datum_CalendarExtenderToFrom" runat="server" TargetControlID="txtDateFrom" PopupButtonID="Datum_CalendarButtonToFrom" />
                                                    <asp:ImageButton runat="Server" ID="Datum_CalendarButtonToFrom" ImageUrl="~/v2/intranet/images/CalendarHS.png" AlternateText="Click to show calendar" />
                                                </td>
                                            </tr>
                                            <tr id="tr2" runat="server">
                                                <td class="EditCaption">
                                                    <div align="right">
                                                        <strong>Datum (bis)</strong>:&nbsp;</div>
                                                </td>
                                                <td class="EditData" valign="bottom">
                                                    <asp:TextBox ID="txtDateTo" runat="server" MaxLength="10" ToolTip="Datum bis" onFocus="this.style.backgroundColor='#DFF4FF'" onBlur="this.style.backgroundColor=''"
                                                        size="10" />
                                                    <ajax:MaskedEditExtender ID="Datum_MaskedEditExtenderTo" runat="server" Century="2000" TargetControlID="txtDateTo" Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                                        OnInvalidCssClass="MaskedEditError" MaskType="Date" ErrorTooltipEnabled="True" />
                                                    <ajax:MaskedEditValidator ID="Datum_MaskedEditValidatorTo" runat="server" ControlExtender="Datum_MaskedEditExtenderTo" ControlToValidate="txtDateTo" InvalidValueMessage="Datum is ung&uuml;ltig!"
                                                        Display="Dynamic" InvalidValueBlurredMessage="*" />
                                                    <ajax:CalendarExtender ID="Datum_CalendarExtenderTo" runat="server" TargetControlID="txtDateTo" PopupButtonID="Datum_CalendarButtonTo" />
                                                    <asp:ImageButton runat="Server" ID="Datum_CalendarButtonTo" ImageUrl="~/v2/intranet/images/CalendarHS.png" AlternateText="Click to show calendar" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tblFooter1" colspan="3">
                                                    <div align="right">
                                                        <asp:Button ID="btnSubmit" runat="server" class="btnSave" Text="GO" title="Speichern" OnClick="btnSubmit_Click" />
                                                        <asp:Button ID="btnCancel" runat="server" class="btnCancel" title="Abbrechen" Text="Abbrechen" OnClick="btnCancel_Click" />
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="EditData">
                                        <asp:GridView ID="gvSummary" 
                                        runat="server" 
                                        AutoGenerateColumns="False" 
                                        BorderStyle="Groove" 
                                        CellPadding="2"
                                        CellSpacing="2" 
                                        Width="100%" 
                                        ShowHeader="false"
                                        OnDataBound="gvSummary_RowCreated"
                                        >
                                            <Columns>
                                                <asp:TemplateField Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblIsDetail" Text='<%# Eval("IsDetail")%>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Text" HtmlEncode="false">
                                                    <ItemStyle HorizontalAlign="Left"/>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Value" HtmlEncode="false">
                                                    <ItemStyle HorizontalAlign="Right"/>
                                                </asp:BoundField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top" class="EditData">
                            <asp:GridView ID="gvCoordinates" runat="server" AutoGenerateColumns="False" BorderStyle="Groove" CellPadding="2" ShowFooter="True" Width="100%" OnRowCreated="gvCoordinates_RowCreated">
                                <Columns>
                                    <asp:BoundField DataField="ActionIndex" HeaderText="" HtmlEncode="false">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ActionDate" HeaderText="Datum" HtmlEncode="false">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ActionAddress" HeaderText="Aktionsadresse" HtmlEncode="false">
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ActionCaption" HeaderText="Aktion" HtmlEncode="false">
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="GegnerName" HeaderText="Schuldner" HtmlEncode="false">
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Schuldneradresse">
                                        <ItemTemplate>
                                            <%# Eval("GegnerAddress") %>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </div>
    <div id='mapDiv'/>
    </form>
    <ctl:footer ID="ftr" runat="server" />
</body>
</html>
