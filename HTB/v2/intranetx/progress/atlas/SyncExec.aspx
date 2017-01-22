<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SyncExec.aspx.cs" Inherits="HTB.v2.intranetx.progress.atlas.SyncExec" %>
<%@ Import Namespace="System.Web.Services" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">

    <script type="text/C#" runat="server">
        [WebMethod()]
        [System.Web.Script.Services.ScriptMethod]
        public static decimal GetSalesByEmployee(int id, int year)
        {
	        return GetSalesByEmployeeInternal(id, year);
        }
    </script>

    <script language="javascript" type="text/javascript">

        function findData() {
            var list1 = document.getElementById("AvailableYears")
            var year = list1.options[list1.selectedIndex].value;

            var list2 = document.getElementById("EmployeeList")
            var id = list2.options[list2.selectedIndex].value;

            var progress = document.getElementById("ProgressBar");
            progress.style.display = "";

            PageMethods.GetSalesByEmployee(id, year, onSearchComplete);
        }

        function onSearchComplete(results) {
            // Turn off progress
            var progress = document.getElementById("ProgressBar");
            progress.style.display = "none";

            // Update the UI
            alert(results);
        }

</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Atlas:: Sync Execution</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"  EnablePageMethods="true"/>
        
        <div>
            <asp:ObjectDataSource ID="AvailableYearsSource" runat="server" 
                TypeName="HTB.v2.intranetx.progress.atlas.CoreDataService"
                SelectMethod="GetAvailableYears">
            </asp:ObjectDataSource>            
            <asp:DropDownList ID="AvailableYears" runat="server" 
                DataSourceID="AvailableYearsSource"
                DataValueField="Year">
            </asp:DropDownList>        
        
            <asp:ObjectDataSource ID="EmployeesSource" runat="server" 
                TypeName="HTB.v2.intranetx.progress.atlas.CoreDataService"
                SelectMethod="GetEmployees">
            </asp:ObjectDataSource>            
            <asp:DropDownList ID="EmployeeList" runat="server" 
                DataSourceID="EmployeesSource"
                DataTextField="lastname"
                DataValueField="employeeid">
            </asp:DropDownList>
            
            <input id="Button1" type="button" value="Load" onclick="findData()"/>    
            
            <hr />
                        
            <div id="ProgressBar" style="display:none; font-weight: bold; font-size: 12pt; color: navy; font-family: Verdana; background-color: #ffff99;">
               <img alt="" src="images/indicator.gif"  />
               <span id="Msg">Please, wait ... </span>
            </div>    
        </div>
    </form>

</body>
</html>

