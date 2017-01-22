<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContextSens.aspx.cs" Inherits="HTB.v2.intranetx.progress.ContextSens" %>
<%@ Register TagPrefix="x" Namespace="HTB.v2.intranetx.progress" Assembly="HTB" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script type="text/javascript">

    function UpdatePage(results, context) {
        StopProgress();
        var label = document.getElementById("Results");
        label.innerHTML = results;
        var button1 = document.getElementById("Button1");
        button1.disabled = false;
        var button2 = document.getElementById("Button2");
        button2.disabled = false;
    }
</script>


<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Lengthy Task--Context Sensitive</title>
</head>
<body>
    <form id="form1" runat="server">      
       <div>
            By clicking the button below you start a potentially lengthy server task. 
            For the purposes of this demo, the task consists of waiting for up to 10 seconds.
            <hr />
            <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>&nbsp;
            <input runat="server" id="Button1" type="button" value="Start task ..." />
            <input runat="server" id="Button2" type="button" value="Start another task ..." /><br />
            <hr />
            <h1>RESULTS</h1>

            <hr />
            <x:ProgressPanel runat="server" ID="ProgressPanel2" />
            <x:ProgressPanel runat="server" ID="ProgressPanel1" />
            <hr />
            <asp:Label ID="Results" runat="server" Text=""></asp:Label>
       </div>
    </form>
</body>
</html>
