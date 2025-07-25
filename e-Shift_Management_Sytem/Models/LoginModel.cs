using e_Shift_Management_Sytem.DB;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace e_Shift_Management_Sytem.Models
{
    public class LoginModel
    {
        // Method that authenticates the user by checking the user credentials in the database
        public static bool AuthenticateUser(string userId, string password, out string userType)
        {
            userType = string.Empty;  // Initialize userType as empty
            try
            {
                // SQL query to check the user credentials
                string query = "SELECT * FROM loginUser WHERE userId = @userId AND password = @password";

                // Define parameters
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@userId", SqlDbType.Int) { Value = int.Parse(userId) },
                    new SqlParameter("@password", SqlDbType.NVarChar) { Value = password }
                };

                // Execute the query using the DatabaseConnection class
                DataTable dataTable = DatabaseConnection.ExecuteQuery(query, parameters);

                if (dataTable.Rows.Count == 1)
                {
                    userType = dataTable.Rows[0][2].ToString();  // Assuming the userType is in the third column (index 2)
                    return true;  // Return true indicating successful authentication
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
