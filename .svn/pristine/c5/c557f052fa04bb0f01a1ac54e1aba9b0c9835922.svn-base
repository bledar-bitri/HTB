using System.Threading;
using System.Data;

namespace HTB.v2.intranetx.progress.atlas
{
    
    public class SalesData
    {
        public string Employee;
        public decimal Amount;
    }

    public class SalesDataCollection : System.Collections.ObjectModel.Collection<SalesData>
    {
    }

    public class FakeDataService
    {

        private string _taskID;

        private CoreDataService coreDataService = new CoreDataService();
        public FakeDataService()
        {
        }

        public FakeDataService(string taskID)
        {
            _taskID = taskID;
        }

        // Return all the years for which there's an order
        public DataTable GetAvailableYears()
        {
            return coreDataService.GetAvailableYears();
        }


        // Return the total sales for a given employee in the specified year
        public DataTable GetOrdersByEmployee(int empID, int year)
        {
            if (empID < 1)
            {
                return null;
            }
            Thread.Sleep(5000);
            return coreDataService.GetOrdersByEmployee(empID, year);
        }


        // Return the total sales for all employees in the specified year
        public SalesDataCollection GetSalesEmployees(int year)
        {
            TaskHelpers.ClearStatus(_taskID);
            DataTable employees = GetEmployees();
            //Dim sales As DataTable = CreateSalesTable()
            SalesDataCollection sales = new SalesDataCollection();

            foreach (DataRow row in employees.Rows)
            {
                int id = (int)row["employeeid"];
                string empName = (string)row["lastname"];

                // Report progress here
                TaskHelpers.UpdateStatus(_taskID, string.Format("Processing employee: <b>{0}</b>", empName));

                decimal amount = GetSalesByEmployee(id, year);
                SalesData salesRow = new SalesData();

                salesRow.Employee = empName;
                salesRow.Amount = amount;

                sales.Add(salesRow);
            }

            TaskHelpers.ClearStatus(_taskID);
            return sales;
        }


        // Return the total sales for a given employee in the specified year
        public decimal GetSalesByEmployee(int empID, int year)
        {
            Thread.Sleep(1000);
            return coreDataService.GetSalesByEmployee(empID, year);
        }


        // Return all employees  
        public DataTable GetEmployees()
        {
            return coreDataService.GetEmployees();
        }


        #region "Helpers"
        protected DataTable CreateSalesTable()
        {
            DataTable sales = new DataTable();
            sales.Columns.Add("Employee", typeof(string));
            sales.Columns.Add("Sales", typeof(decimal));
            return sales;
        }
        #endregion
    }

}