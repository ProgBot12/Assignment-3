using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assignment_3
{
    public partial class Form1 : Form
    {
        DataTable myDataTable = new DataTable();
        SqlConnection myConn = new SqlConnection();
        SqlTransaction myTransaction;
        public Form1()
        {
            InitializeComponent();
        }
        private void fillButton_Click(object sender, EventArgs e)
        {
          
            SqlConnection myConn = new SqlConnection();
            myConn.ConnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Emir Cokrlija\\Desktop\\DB\\PVFC.mdf; Integrated Security=True;Connect Timeout=30";
            myConn.Open();

            //create an SQL Command object
            SqlCommand myCommand = new SqlCommand();

            myCommand.CommandText = "Select * from Employee_T where EmployeeName = @name";

            myCommand.Parameters.Add("@name", SqlDbType.NChar, 20);
            myCommand.Parameters["@name"].Value = employeeNameTxtBox.Text;

            myCommand.Connection = myConn;

            //create an adapter
            SqlDataAdapter myAdapter = new SqlDataAdapter();
            myAdapter.SelectCommand = myCommand;

            //fill an internal table
            myAdapter.Fill(myDataTable);

            //bind the data to gui object
            dataGridView1.DataSource = myDataTable;
        }

        private void updateButton_Click(object sender, EventArgs e)
        {

            SqlConnection myConn = new SqlConnection();
            myConn.ConnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Emir Cokrlija\\Desktop\\DB\\PVFC.mdf; Integrated Security=True;Connect Timeout=30";
            myConn.Open();

            //initiate transaction with isolation level read uncommitted
            myTransaction = myConn.BeginTransaction(IsolationLevel.ReadUncommitted);

            //create update command
            SqlCommand updateCommand = new SqlCommand();
            updateCommand.Connection = myConn;
            updateCommand.Transaction = myTransaction;

            updateCommand.CommandText = "Update Employee_T set EmployeeName = @empname where EmployeeID " +
                "= @empId";
            updateCommand.Parameters.Add("@empname", SqlDbType.NVarChar, 50, "EmployeeName");
            updateCommand.Parameters.Add("@empId", SqlDbType.NVarChar, 50, "EmployeeID");

            SqlDataAdapter myAdapter = new SqlDataAdapter();
            myAdapter.UpdateCommand = updateCommand;

            myAdapter.ContinueUpdateOnError = true;

            try
            {
                myAdapter.Update(myDataTable);

            }
            catch(Exception ex)
            {
                myTransaction.Rollback();
                MessageBox.Show("Unable to update database.");
            }

        }

        private void insertButton_Click(object sender, EventArgs e)
        {
            SqlConnection myConn = new SqlConnection();
            myConn.ConnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Emir Cokrlija\\Desktop\\DB\\PVFC.mdf; Integrated Security=True;Connect Timeout=30";
            myConn.Open();

            SqlCommand insertCommand = new SqlCommand();
            insertCommand.Connection = myConn;

            insertCommand.CommandText = "Insert Into Employee_T Values (@empId,@empName,@empAddress," +
                "@empCity,@empState,@empZip,@empSupervisor,@empDateHired)";
            insertCommand.Parameters.Add("@empId", SqlDbType.NVarChar, 50, "EmployeeID");
            insertCommand.Parameters.Add("@empName", SqlDbType.NVarChar, 50, "EmployeeName");
            insertCommand.Parameters.Add("@empAddress", SqlDbType.NVarChar, 30, "EmployeeAddress");
            insertCommand.Parameters.Add("@empCity", SqlDbType.NVarChar, 20, "EmployeeCity");
            insertCommand.Parameters.Add("@empState", SqlDbType.NVarChar, 2, "EmployeeState");
            insertCommand.Parameters.Add("@empZip", SqlDbType.NVarChar, 9, "EmployeeZipCode");
            insertCommand.Parameters.Add("@empSupervisor", SqlDbType.NVarChar, 10, "EmployeeSupervisor");
            insertCommand.Parameters.Add("@empDateHired", SqlDbType.DateTime, 8, "EmployeeDateHired");

            SqlDataAdapter myAdapter = new SqlDataAdapter();
            myAdapter.InsertCommand = insertCommand;

            try
            {
                myAdapter.Update(myDataTable);
                MessageBox.Show("Data Inserted.");
            }
            catch(Exception ex)
            {
                MessageBox.Show("Unable to insert data.");
            }

        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            SqlConnection myConn = new SqlConnection();
            myConn.ConnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Emir Cokrlija\\Desktop\\DB\\PVFC.mdf; Integrated Security=True;Connect Timeout=30";
            myConn.Open();

            SqlCommand deleteCommand = new SqlCommand();
            deleteCommand.Connection = myConn;

            deleteCommand.CommandText = "Delete From Employee_T where EmployeeName = @empName";
            deleteCommand.Parameters.Add("@empName", SqlDbType.NVarChar, 25);
            deleteCommand.Parameters["@empName"].Value = employeeNameTxtBox.Text;

            SqlDataAdapter myAdapter = new SqlDataAdapter();
            myAdapter.DeleteCommand = deleteCommand;

            try
            {
                myAdapter.Update(myDataTable);
                MessageBox.Show("Data row deleted.");
            }catch(Exception ex)
            {
                MessageBox.Show("Unable to delete the row");
            }
            
        }

        private void commitButton_Click(object sender, EventArgs e)
        {
            myTransaction.Commit();
        }
    }
}
