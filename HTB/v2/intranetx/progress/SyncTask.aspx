<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SyncTask.aspx.cs" Inherits="HTB.v2.intranetx.progress.SyncTask" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script type="text/javascript">
    function DisableButtons() {
        // Clear
        var results = document.getElementById("Results");
        results.innerHTML = "";

        var button1 = document.getElementById("Button1");
        button1.disabled = true;
        var textbox1 = document.getElementById("TextBox1");
        textbox1.disabled = true;

        var progress = document.getElementById("ProgressBar");
        progress.style.display = "";
        return false;
    }
</script>


<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Synchronous Execution</title>
</head>
<body>
    <form id="form1" runat="server" onsubmit="DisableButtons()" submitdisabledcontrols="true">
    <div>
    By clicking the button below you start a potentially lengthy server task. 
    For the purposes of this demo, the task consists of waiting for up to 10 seconds.
    <hr />
        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>        
        <asp:Button UseSubmitBehavior="false" ID="Button1" runat="server" Text="Start task..." />
        <br />
        <hr />
        <h1>RESULTS</h1>
        
        <div id="ProgressBar" style="display:none; font-weight: bold; font-size: 12pt; color: navy; font-family: Verdana; background-color: #ffff99;">
        <img alt="" src="images/indicator.gif"  />
        <span id="Msg">Please, wait ... </span>
        </div>
        
         <asp:Label ID="Results" runat="server" Text=""></asp:Label>
       </div>
    </form>
</body>
</html>