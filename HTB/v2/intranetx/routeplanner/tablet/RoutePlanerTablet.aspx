<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RoutePlanerTablet.aspx.cs" Inherits="HTB.v2.intranetx.routeplanner.tablet.RoutePlanerTablet" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>HTB.ASP [ Routenplaner ]</title>
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
        OPTION.dis
        {
            background-color: white;
            color: #999999;
        }
        
        .modalBackground
        {
            background-color: #CCCCCC;
            filter: alpha(opacity=40);
            opacity: 0.5;
        }
    </style>
    <script type="text/javascript">

        function UpdatePage(results, context) {
            StopProgress();
            document.location.href = "BingRouteDisplayTablet.aspx";
        }

        var _timerId;

        function StartProgress() {
            ClearProgress();
            $get('progressbar').style.display = '';
            $get('message').innerHTML = 'Processing, Please wait';
            _timerId = setInterval(UpdateStatus, 1000); // Poll the status at 1 Second interval
        }

        function UpdateStatus() {
            HTB.v2.intranetx.WS.WsProgress.GetBingRoutePlannerTaskStatus(
                                function (status) {
                                    if ((status == null) || (status.Progress == 100)) {
                                        clearInterval(_timerId);
                                        $get('message').innerHTML = 'Task Complete';
                                        $get('progressbar').style.display = 'none';
                                        document.location.href = "BingRouteDisplayTablet.aspx";
                                    }
                                    else {
                                        UpdateProgress(status.Progress, status.ProgressText, status.ProgressTextLong);
                                    }
                                }
                            );
        }

        function ClearProgress() {
            for (var i = 1; i <= 10; i++) {
                $get('bar' + i).style.backgroundColor = 'transparent';
            }
        }

        function UpdateProgress(progressPct, progressText, progressTextLong) {
            var barToFill = (progressPct / 10);
            var counter = 1;

            if (progressTextLong == null || progressTextLong == 'null')
                progressTextLong = '';

            $get('message').innerHTML = String.format(progressText + ' Progress {0}% complete.<br/>' + progressTextLong, progressPct);

            while (counter <= barToFill) {
                $get('bar' + counter).style.backgroundColor = '#0000cc';
                counter++;
            }
        }
    </script>
</head>
<body runat="server" id="bdy">
    <ctl:headerNoMenuTablet ID="header" runat="server" />
    <form id="form1" runat="server" defaultbutton="btnSubmit">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/v2/intranetx/WS/WsProgress.asmx" />
        </Services>
    </asp:ScriptManager>
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        function BeginRequestHandler(sender, args) {
            document.getElementById('<%= btnSubmit.ClientID %>').innerText = "Processing";
            document.getElementById('<%= btnSubmit.ClientID %>').disabled = true;
        }
    </script>
    <table width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
        <tr>
            <td class="tblDataAll" align="left">
                <table border="0" align="center" cellpadding="3" cellspacing="0">
                    <tr runat="server" id="trHeader">
                        <td colspan="2" class="tblHeader" title="Startadresse">
                            Routenplaner
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="tblDataAll">
                            <ctl:message runat="server" ID="ctlMessage" />
                        </td>
                    </tr>
                    <tr runat="server" id="trAD">
                        <td valign="top" class="EditCaption">
                            <div align="right">
                                Inkassant:&nbsp;</div>
                        </td>
                        <td class="EditData">
                            <asp:Label runat="server" ID="lblAD" class="docText" />
                            <asp:DropDownList runat="server" ID="ddlAD" class="docText" AutoPostBack="true" OnSelectedIndexChanged="ddlAD_SelectedIndexChanged" />
                        </td>
                    </tr>
                    <tr runat="server" id="tr1">
                        <td valign="top" class="EditCaption">
                            <div align="right">
                                Routenname:&nbsp;</div>
                        </td>
                        <td class="EditData">
                            <asp:TextBox runat="server" ID="txtRouteName" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor=''" size="60" />
                        </td>
                    </tr>
                    <tr runat="server" id="trAddress">
                        <td valign="top" class="EditCaption">
                            <div align="right">
                                Startadresse:&nbsp;</div>
                        </td>
                        <td class="EditData">
                            <asp:TextBox runat="server" ID="txtStartAddress" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor=''" size="60" />
                        </td>
                    </tr>
                    <tr runat="server" id="trPLZ">
                        <td valign="middle" class="EditCaption">
                            <div align="right">
                                PLZ:&nbsp;</div>
                        </td>
                        <td class="EditData">
                            <table id="tblPlz" runat="server">
                                
                            </table>
                        </td>
                    </tr>
                    <tr runat="server" id="trAktType">
                        <td valign="top" class="EditCaption">
                            <div align="right">
                                Akttyp:&nbsp;</div>
                        </td>
                        <td class="EditData">
                            <asp:DropDownList runat="server" ID="ddlAktType" class="docText" />
                        </td>
                    </tr>
                    <tr runat="server" id="trAktStatus">
                        <td valign="top" class="EditCaption">
                            <div align="right">
                                Aktstatus:&nbsp;</div>
                        </td>
                        <td class="EditData">
                            1 - In Bearbeitung
                        </td>
                    </tr>
                    <tr runat="server" id="trProgress">
                        <td colspan="2">
                            <div>
                                <div id="message" style="font-size: smaller">
                                </div>
                                <table id="progressbar" border="0" cellpadding="0" cellspacing="2" style="border: solid 1px #000000; display: none">
                                    <tbody>
                                        <tr>
                                            <td id="bar1">
                                                &nbsp; &nbsp;
                                            </td>
                                            <td id="bar2">
                                                &nbsp; &nbsp;
                                            </td>
                                            <td id="bar3">
                                                &nbsp; &nbsp;
                                            </td>
                                            <td id="bar4">
                                                &nbsp; &nbsp;
                                            </td>
                                            <td id="bar5">
                                                &nbsp; &nbsp;
                                            </td>
                                            <td id="bar6">
                                                &nbsp; &nbsp;
                                            </td>
                                            <td id="bar7">
                                                &nbsp; &nbsp;
                                            </td>
                                            <td id="bar8">
                                                &nbsp; &nbsp;
                                            </td>
                                            <td id="bar9">
                                                &nbsp; &nbsp;
                                            </td>
                                            <td id="bar10">
                                                &nbsp; &nbsp;
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr runat="server" id="trButtons">
                        <td colspan="2">
                            <br />
                            <asp:Button runat="server" ID="btnSubmit" Text="Route planen" class="btnSave" Height="30" OnClick="btnSubmit_Click" />
                            <br />
                            &nbsp;<br />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="tblDataAll" valign="top" align="left">
                <table border="0" align="center" cellpadding="3" cellspacing="0">
                    <tr runat="server" id="tr2">
                        <td colspan="2" class="tblHeader" title="Startadresse">
                            Gespeicherte Routen
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="gvRouten" runat="server" AutoGenerateColumns="False" CellPadding="2" BorderStyle="Groove" Width="100%" CssClass="tblDataAll">
                                <Columns>
                                    <asp:BoundField HeaderText="Inkassant" DataField="RouteUser" SortExpression="RouteUser" />
                                    <asp:BoundField HeaderText="Datum" DataField="RouteDate" SortExpression="RouteDate" />
                                    <asp:TemplateField HeaderText="Routenname">
                                        <ItemTemplate>
                                            <%# Eval("RouteNameLink")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <asp:HiddenField runat="server" ID="hdnNumberOfPlzRanges"/>
    </form>
    <ctl:footer ID="ftr" runat="server" />
</body>
</html>
