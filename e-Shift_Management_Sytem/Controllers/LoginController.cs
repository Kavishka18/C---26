using System;
using System.Windows.Forms;
using e_Shift_Management_Sytem.Models;  // Model that handles business logic
using e_Shift_Management_Sytem.Admin;
using e_Shift_Management_Sytem.Customer;
using e_Shift_Management_Sytem.Views;


namespace e_Shift_Management_Sytem.Controllers
{
    public class LoginController
    {
        // Reference to the Login view
        private Login view;

        // Constructor that initializes the controller with the Login view
        public LoginController(Login loginView)
        {
            view = loginView;  // View initialization
        }

        // Method that is called when the login button is clicked
        public void LoginButtonClicked(string userId, string password)
        {
            string userType;  // Variable to hold the user type (Admin/User)

            // Authenticate the user by passing credentials to the LoginModel
            bool isAuthenticated = LoginModel.AuthenticateUser(userId, password, out userType);

            // Check if the user is authenticated
            if (isAuthenticated)
            {
                if (userType == "Admin")
                {
                    MessageBox.Show("Admin Login Successful.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    adminDashboard adminDashboard = new adminDashboard();
                    adminDashboard.Show();
                    view.Hide();
                }
                else if (userType == "User")
                {
                    MessageBox.Show("User Login Successful.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    userDashboard userDashboard = new userDashboard();
                    userDashboard.Show();
                    view.Hide();
                }
                else
                {
                    MessageBox.Show("Invalid user type.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Invalid details", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
            }
        }
    }
}
