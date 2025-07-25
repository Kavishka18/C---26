using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using e_Shift_Management_Sytem.DB;
using e_Shift_Management_Sytem.Admin;
using e_Shift_Management_Sytem.Customer;
using System.Data.SqlClient;
using e_Shift_Management_Sytem.Controllers;  // Controller reference


namespace e_Shift_Management_Sytem.Views
{
    public partial class Login: Form
    {
        
        private LoginController controller;   // Reference to the controller

        // Constructor initializes the controller
        public Login()
        {
            InitializeComponent();
            controller = new LoginController(this); // Pass the view to the controller
        }

        // Event handler for the login button click event
        private void btn_login_Click(object sender, EventArgs e)
        {
            // Call the controller method to authenticate the user
            controller.LoginButtonClicked(text_userid.Text.Trim(), text_password.Text.Trim());
        }

        private void btn_Clear_Click(object sender, EventArgs e)
        {
            text_userid.Clear();
            text_password.Clear();

            text_userid.Focus();
        }

        private void checkBox_ShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            bool showPasswords = checkBox_ShowPassword.Checked;

            text_password.UseSystemPasswordChar = !showPasswords;   // Toggle password visibility
        }
    }
}

