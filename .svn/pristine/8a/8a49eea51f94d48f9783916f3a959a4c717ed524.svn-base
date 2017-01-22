<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SyncExecPanel.aspx.cs" Inherits="HTB.v2.intranetx.progress.atlas.SyncExecPanel" %>
<%@ Import Namespace="System.Web.Services" %>
<%@ Import Namespace="System.Data" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">

<script type="text/javascript">
    function UpdateMsg(empName) {
        var msg = document.getElementById("Msg");
        msg.innerHTML = "Please, wait while data for " + empName + " is retrieved ...";
    }
</script>


<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Atlas:: Sync Execution (using UpdatePanel)</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="True" />
        
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
                onchange="UpdateMsg(this.options[this.selectedIndex].text);"
                DataSourceID="EmployeesSource"
                DataTextField="lastname"
                DataValueField="employeeid">
            </asp:DropDownList>
            
            <hr />

            <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                <ContentTemplate>
                    <asp:Button ID="Button1" runat="server" Text="Load" OnClick="Button1_Click" />                
                    <asp:GridView DataSourceID="DataServiceSource" ID="GridView1" runat="server" AllowPaging="True" BackColor="White" BorderColor="White" BorderStyle="Ridge" BorderWidth="2px" CellPadding="3" CellSpacing="1" GridLines="None">
                        <FooterStyle BackColor="#C6C3C6" ForeColor="Black" />
                        <RowStyle BackColor="#DEDFDE" ForeColor="Black" />
                        <SelectedRowStyle BackColor="#9471DE" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#C6C3C6" ForeColor="Black" HorizontalAlign="Right" />
                        <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#E7E7FF" />
                    </asp:GridView>            
                    <small><asp:Label runat="server" ID="TimeServed"></asp:Label></small>
                </ContentTemplate>
            </asp:UpdatePanel>


            <asp:ObjectDataSource ID="DataServiceSource" runat="server" 
                TypeName="CuttingEdge.Samples.FakeDataService"
                SelectMethod="GetOrdersByEmployee">
                <SelectParameters>
                    <asp:ControlParameter ControlID="AvailableYears" Name="year" PropertyName="SelectedValue" /> 
                    <asp:ControlParameter ControlID="EmployeeList" Name="empid" PropertyName="SelectedValue" /> 
                </SelectParameters>
            </asp:ObjectDataSource>            
            
            <atlas:UpdateProgress id="progress1" runat="server">
               <ProgressTemplate>
                    <div style="font-weight: bold; font-size: 12pt; color: navy; font-family: Verdana; background-color: #ffff99;">
                       <img alt="" src="images/indicator.gif"  />
                       <span id="Msg">Please, wait ... </span>  
                    </div>    
               </ProgressTemplate>
            </atlas:UpdateProgress>
         </div>
    </form>

</body>
</html>
