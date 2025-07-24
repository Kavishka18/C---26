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
using e_Shift_Management_Sytem.Controllers;


namespace e_Shift_Management_Sytem.Views
{
    public partial class Login: Form
    {
        
        private LoginController controller;

        public Login()
        {
            InitializeComponent();
            controller = new LoginController(this);
        }
       

        private void btn_login_Click(object sender, EventArgs e)
        {
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

            text_password.UseSystemPasswordChar = !showPasswords;
        }
    }
}

