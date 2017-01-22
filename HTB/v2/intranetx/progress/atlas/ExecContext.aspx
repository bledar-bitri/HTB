<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExecContext.aspx.cs" Inherits="HTB.v2.intranetx.progress.atlas.ExecContext" %>
<%@ Import Namespace="System.Web.Services" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">

<script type="text/javascript">
    var timerID;
    var globalTaskID;
    function ShowProgress() {
        PageMethods.UpdateStatus(globalTaskID, onUpdateProgress);
        EnableStatusBar(true);

        timerID = window.setTimeout("ShowProgress()", 1000);
    }

    function StopProgress() {
        window.clearTimeout(timerID);
        EnableStatusBar(false);
    }

    function EnableStatusBar(enabled) {
        var label = document.getElementById("Msg");
        if (enabled)
            label.style.display = '';
        else
            label.style.display = 'none';
    }

    function onUpdateProgress(result) {
        var label = document.getElementById("Msg");
        label.innerHTML = result;
    }

    function StartTask() {
        var years = document.getElementById("AvailableYears");
        var year = years.options[years.selectedIndex].value;

        EnableUI(false);

        globalTaskID = GetGuid();
        ShowProgress();
        PageMethods.GetSalesEmployees(globalTaskID, year, onTaskCompleted);
    }

    function EnableUI(enabled) {
        var btn = document.getElementById("StartButton");
        var years = document.getElementById("AvailableYears");
        var grid = document.getElementById("grid");
        if (enabled) {
            years.disabled = false;
            btn.disabled = false;
            grid.disabled = false;
        }
        else {
            years.disabled = true;
            btn.disabled = true;
            grid.disabled = true;
        }
    }

    function onTaskCompleted(result) {
        StopProgress();
        EnableUI(true);
        var grid = document.getElementById("grid");
        grid.innerHTML = result;
    }

    function GetGuid() {
        var ranNum = Math.floor(Math.random() * 10000000);
        return ranNum;
    }

</script>

    <script type="text/C#" runat="server">
        [WebMethod()]
        [System.Web.Script.Services.ScriptMethod]
        public static string UpdateStatus(string taskID)
        {
            return UpdateStatusInternal(taskID);
        }

        [WebMethod()]
        [System.Web.Script.Services.ScriptMethod]
        public static string GetSalesEmployees(string taskID, int year)
        {
            return GetSalesEmployeesInternal(taskID, year);
        }

    </script>


<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Atlas:: Context-sensitive</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"  EnablePageMethods="true" EnablePartialRendering="true"/>
        
      
        <div>
            <asp:ObjectDataSource ID="AvailableYearsSource" runat="server" 
                TypeName="HTB.v2.intranetx.progress.atlas.CoreDataService"
                SelectMethod="GetAvailableYears">
            </asp:ObjectDataSource>            
            <asp:DropDownList ID="AvailableYears" runat="server" 
                DataSourceID="AvailableYearsSource"
                DataValueField="Year">
            </asp:DropDownList>
            
            <hr />
           
            <input type="button" name="StartButton" value="Employees Sales Report" onclick="StartTask()" />                

            <asp:Label runat="server" ID="Msg" ForeColor="Red" />            

            <div id="grid">
            </div>

         </div>

    </form>

</body>
</html>
