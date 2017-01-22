using System.Data.SqlClient;
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace HTB.v2.intranetx.progress.atlas
{
    
    public class CoreDataServiceCommands
    {
        public static string ConnectionString = "SERVER=.\\deuexpress;DATABASE=Northwind;UID=ecp;Password=ecp;";
        public static string Cmd_AvailableYears = "SELECT DISTINCT YEAR(orderdate) AS [Year]  FROM orders ORDER BY [Year] ASC";
        public static string Cmd_SalesEmployees = "SELECT e.lastname AS Employee, SUM(price) AS Sales FROM (SELECT o.employeeid, od.orderid, SUM(od.quantity*od.unitprice) AS price FROM Orders o, [Order Details] od WHERE Year(o.orderdate) = @year AND od.orderid=o.orderid GROUP BY o.employeeid, od.orderid ) AS t1 INNER JOIN Employees e ON t1.employeeid=e.employeeid GROUP BY t1.employeeid, e.lastname";
        public static string Cmd_OrdersByEmployee = "SELECT Orders.OrderID, Orders.OrderDate, \"Order Subtotals\".Subtotal FROM Orders INNER JOIN \"Order Subtotals\" ON Orders.OrderID = \"Order Subtotals\".OrderID WHERE Year(Orders.OrderDate)=@year AND Orders.EmployeeID=@empID";
        public static string Cmd_SalesByEmployee = "SELECT SUM(Amount) FROM (SELECT o.ShippedDate, o.OrderID, \"Order Subtotals\".Subtotal AS Amount FROM Orders o INNER JOIN \"Order Subtotals\" ON o.OrderID = \"Order Subtotals\".OrderID WHERE Year(o.OrderDate)=@year AND o.employeeid=@empID) t1";
        public static string Cmd_GetEmployees = "SELECT employeeid, firstname, lastname FROM employees";
    }


    public class CoreDataService
    {

        // Return all the years for which there's an order
        public DataTable GetAvailableYears()
        {
            SqlDataAdapter adapter = new SqlDataAdapter(CoreDataServiceCommands.Cmd_AvailableYears, CoreDataServiceCommands.ConnectionString);
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }


        // Return the total sales for a given employee in the specified year
        public DataTable GetOrdersByEmployee(int empID, int year)
        {
            SqlDataAdapter adapter = new SqlDataAdapter(CoreDataServiceCommands.Cmd_OrdersByEmployee, CoreDataServiceCommands.ConnectionString);
            adapter.SelectCommand.Parameters.AddWithValue("@empID", empID);
            adapter.SelectCommand.Parameters.AddWithValue("@year", year);

            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }


        // Return the total sales for all employees in the specified year
        public DataTable GetSalesEmployees(int year)
        {
            SqlDataAdapter adapter = new SqlDataAdapter(CoreDataServiceCommands.Cmd_SalesEmployees, CoreDataServiceCommands.ConnectionString);
            adapter.SelectCommand.Parameters.AddWithValue("@year", year);

            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }


        // Return the total sales for a given employee in the specified year
        public decimal GetSalesByEmployee(int empID, int year)
        {

            SqlDataAdapter adapter = new SqlDataAdapter(CoreDataServiceCommands.Cmd_SalesByEmployee, CoreDataServiceCommands.ConnectionString);
            adapter.SelectCommand.Parameters.AddWithValue("@empID", empID);
            adapter.SelectCommand.Parameters.AddWithValue("@year", year);

            DataTable table = new DataTable();
            adapter.Fill(table);
            return (decimal)table.Rows[0][0];
        }


        // Return all employees  
        public DataTable GetEmployees()
        {
            SqlDataAdapter adapter = new SqlDataAdapter(CoreDataServiceCommands.Cmd_GetEmployees, CoreDataServiceCommands.ConnectionString);

            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }

    }

}