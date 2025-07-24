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
using e_Shift_Management_Sytem.Admin;
using e_Shift_Management_Sytem.DB;
using e_Shift_Management_Sytem.Validation;

namespace e_Shift_Management_Sytem.Customer
{
    public partial class CustomerDetailsManagement: Form
    {
        public CustomerDetailsManagement()
        {
            InitializeComponent();
        }

        // Customer class to represent a customer object
        public class Customer
        {
            public int CustomerID { get; set; }
            public string CustomerName { get; set; }
            public string CustomerAddress { get; set; }
            public string ContactNumber { get; set; }
            public string Email { get; set; }
            public int UserId { get; set; }
        }


        private void Return_Click(object sender, EventArgs e)
        {
            userDashboard us = new userDashboard();
            us.Show();
            this.Hide();
        }

        // Linear Search method for searching customers by name
        private DataTable LinearSearchCustomer(string searchQuery)
        {
            //Get all customers from the database
            DataTable data = DatabaseConnection.ExecuteQuery("SELECT * FROM Customers");

            //Convert DataTable to array of Customer objects
            List<Customer> customers = new List<Customer>();
            foreach (DataRow row in data.Rows)
            {
                customers.Add(new Customer
                {
                    CustomerID = row["CustomerID"] == DBNull.Value ? 0 : Convert.ToInt32(row["CustomerID"]),
                    CustomerName = row["CustomerName"].ToString(),
                    CustomerAddress = row["CustomerAddress"].ToString(),
                    ContactNumber = row["ContactNumber"].ToString(),
                    Email = row["Email"].ToString(),
                    UserId = row["UserId"] == DBNull.Value ? 0 : Convert.ToInt32(row["UserId"])
                });
            }

            //Create a new DataTable for search results
            DataTable searchResults = data.Clone(); // Create a new empty DataTable with the same schema

            //Linear search on the array of customers
            foreach (var customer in customers)
            {
                if (customer.CustomerName.ToLower().Contains(searchQuery.ToLower())) // Case-insensitive search for matching CustomerName
                {
                    // If match is found, add the row to the result DataTable
                    DataRow newRow = searchResults.NewRow();
                    newRow["CustomerID"] = customer.CustomerID;
                    newRow["CustomerName"] = customer.CustomerName;
                    newRow["CustomerAddress"] = customer.CustomerAddress;
                    newRow["ContactNumber"] = customer.ContactNumber;
                    newRow["Email"] = customer.Email;
                    newRow["UserId"] = customer.UserId;
                    searchResults.Rows.Add(newRow);
                }
            }

            return searchResults;  // Return the filtered results

        }

        private void LoadCustomers()
        {
            int currentUserId = int.Parse(text_userid.Text.Trim()); // Or however you track the logged-in user's ID.

            string query = "SELECT * FROM Customers WHERE userId = @userId";
            SqlParameter[] parameters = new SqlParameter[]
            {
        new SqlParameter("@userId", SqlDbType.Int) { Value = currentUserId }
            };

            DataTable data = DatabaseConnection.ExecuteQuery(query, parameters);
            dataGridViewCustomers.DataSource = data;
        }

        private void btnAddCustomer_Click(object sender, EventArgs e)
        {
            ErrorProvider errorProvider = new ErrorProvider();

            try
            {
                // Validate fields
                if (!RegexValidation.IsValidText(txtCustomerName.Text))
                {
                    RegexValidation.SetError(txtCustomerName, "Customer Name is required", errorProvider);
                    return;
                }
                else
                {
                    RegexValidation.ClearError(txtCustomerName, errorProvider);
                }

                if (!RegexValidation.IsValidEmail(txtEmail.Text))
                {
                    RegexValidation.SetError(txtEmail, "Invalid Email", errorProvider);
                    return;
                }
                else
                {
                    RegexValidation.ClearError(txtEmail, errorProvider);
                }

                if (!RegexValidation.IsValidContactNumber(txtContactNumber.Text))
                {
                    RegexValidation.SetError(txtContactNumber, "Invalid Contact Number", errorProvider);
                    return;
                }
                else
                {
                    RegexValidation.ClearError(txtContactNumber, errorProvider);
                }

                // Assuming the employee user ID is available
                int currentUserId = int.Parse(text_userid.Text.Trim());

                string query = "INSERT INTO Customers (CustomerName, CustomerAddress, ContactNumber, Email, userId) " +
                               "VALUES (@CustomerName, @CustomerAddress, @ContactNumber, @Email, @userId)";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@CustomerName", SqlDbType.NVarChar) { Value = txtCustomerName.Text },
                    new SqlParameter("@CustomerAddress", SqlDbType.NVarChar) { Value = txtCustomerAddress.Text },
                    new SqlParameter("@ContactNumber", SqlDbType.NVarChar) { Value = txtContactNumber.Text },
                    new SqlParameter("@Email", SqlDbType.NVarChar) { Value = txtEmail.Text },
                    new SqlParameter("@userId", SqlDbType.Int) { Value = currentUserId }
                };

                DatabaseConnection.ExecuteNonQuery(query, parameters);
                MessageBox.Show("Customer added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadCustomers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding customer: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEditCustomer_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "UPDATE Customers SET CustomerName = @CustomerName, CustomerAddress = @CustomerAddress, " +
                               "ContactNumber = @ContactNumber, Email = @Email WHERE CustomerID = @CustomerID";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@CustomerName", SqlDbType.NVarChar) { Value = txtCustomerName.Text },
                    new SqlParameter("@CustomerAddress", SqlDbType.NVarChar) { Value = txtCustomerAddress.Text },
                    new SqlParameter("@ContactNumber", SqlDbType.NVarChar) { Value = txtContactNumber.Text },
                    new SqlParameter("@Email", SqlDbType.NVarChar) { Value = txtEmail.Text },
                    new SqlParameter("@CustomerID", SqlDbType.Int) { Value = int.Parse(txtCustomerID.Text) }
                };

                DatabaseConnection.ExecuteNonQuery(query, parameters);
                MessageBox.Show("Customer updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadCustomers();  // Reload the data grid
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating customer: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeleteCustomer_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "DELETE FROM Customers WHERE CustomerID = @CustomerID";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@CustomerID", SqlDbType.Int) { Value = int.Parse(txtCustomerID.Text) }
                };

                DatabaseConnection.ExecuteNonQuery(query, parameters);
                MessageBox.Show("Customer deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadCustomers();  // Reload the data grid
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting customer: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Search for customers by name
        private void btnSearchCustomer_Click(object sender, EventArgs e)
        {
            try
            {
                string searchQuery = txtSearchCustomerName.Text.Trim(); // User input

                // Check if the search term is empty
                if (string.IsNullOrEmpty(searchQuery))
                {
                    MessageBox.Show("Please enter a search term.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Perform linear search to find matching customers
                DataTable result = LinearSearchCustomer(searchQuery);

                // Display the search results in the DataGridView
                if (result.Rows.Count > 0)
                {
                    dataGridViewCustomers.DataSource = result; // Display search results
                }
                else
                {
                    MessageBox.Show("No customers found.", "Search Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error searching for customers: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button_clear_Click(object sender, EventArgs e)
        {
            txtCustomerName.Clear();
            txtCustomerAddress.Clear();
            txtContactNumber.Clear();
            txtEmail.Clear();
            txtCustomerID.Clear();
            txtSearchCustomerName.Clear();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            adminDashboard ad = new adminDashboard();
            ad.Show();
            this.Hide();

        }
    }
}
