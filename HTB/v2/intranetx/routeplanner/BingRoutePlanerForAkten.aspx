<%@ Page Language="C#" Culture="de-DE" AutoEventWireup="true" CodeBehind="BingRoutePlanerForAkten.aspx.cs" Inherits="HTB.v2.intranetx.routeplanner.BingRoutePlanerForAkten" %>

<%@ Register TagPrefix="progress" Namespace="HTB.v2.intranetx.progress" Assembly="HTB" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>HTB.ASP [ Routenplanner ]</title>
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
    </style>
    <script type="text/javascript">

        function UpdatePage(results, context) {
            StopProgress();
            document.location.href = "BingRouteDisplay.aspx";
        }

        var _timerId;

        function StartProgress() {
            ClearProgress();
            $get('progressbar').style.display = '';
            $get('message').innerHTML = 'Processing, Please wait';
            _timerId = setInterval(UpdateStatus, 1000); // Poll the status at 1 Second interval
        }

        function UpdateStatus() {
            HTB.v2.intranetx.WS.WsProgress.GetBingRoutePlannerForAktenTaskStatus(
                                function (status) {
                                    if ((status == null) || (status.Progress == 100)) {
                                        clearInterval(_timerId);
                                        $get('message').innerHTML = 'Task Complete';
                                        $get('progressbar').style.display = 'none';
                                        document.location.href = "BingRouteDisplay.aspx";
                                    }
                                    else {
                                        UpdateProgress(status.Progress, status.ProgressText);
                                    }
                                }
                            );
        }

        function ClearProgress() {
            for (var i = 1; i <= 10; i++) {
                $get('bar' + i).style.backgroundColor = 'transparent';
            }
        }

        function UpdateProgress(progressPct, progressText) {
            var barToFill = (progressPct / 10);
            var counter = 1;

            $get('message').innerHTML = String.format(progressText + ' Progress {0}% complete. ', progressPct);

            while (counter <= barToFill) {
                $get('bar' + counter).style.backgroundColor = '#0000cc';
                counter++;
            }
        }
    </script>
</head>
<body runat="server" id="bdy">
    <ctl:headerNoMenu ID="header" runat="server" />
    <form id="form1" runat="server" DefaultButton="btnSubmit">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/v2/intranetx/WS/WsProgress.asmx" />
        </Services>
    </asp:ScriptManager>
    
    <table width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
        <tr>
            <td class="tblDataAll">
                <table border="0" align="center" cellpadding="3" cellspacing="0">
                    <tr runat="server" id="trHeader">
                        <td colspan="2" class="tblHeader" title="Startadresse">
                            Startadresse
                        </td>
                    </tr>
                    <tr runat="server" id="trAddress">
                        <td valign="top" class="EditCaption">
                            <div align="right">Startadresse:&nbsp;</div>
                        </td>
                        <td class="EditData">
                            <asp:TextBox runat="server" ID="txtStartAddress" class="docText" onfocus="this.style.backgroundColor='#DFF4FF';" onblur="this.style.backgroundColor=''" size="100"  />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <ctl:message runat="server" ID="ctlMessage"/>
                        </td>
                    </tr>
                    <tr>
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
                    <tr><td colspan="2"><asp:Button runat="server" ID="btnSubmit" Text="Routeplanen" class="btnSave" onclick="btnSubmit_Click" /></td></tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
    <ctl:footer ID="ftr" runat="server" />
</body>
</html>
