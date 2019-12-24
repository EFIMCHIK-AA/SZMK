using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SZMK
{
    public partial class RegistrationUser_F : Form
    {
        public RegistrationUser_F()
        {
            InitializeComponent();
        }

        private void Generate_B_Click(object sender, EventArgs e)
        {
            String Alfabet = "QqWwEeRrTtYyUuIiOoPpAaSsDdFfGgHhJjKkLlZzXxCcVvBbNnMm#1234567890!@#$%^&*-+";
            Random Generate = new Random();

            String Password = String.Empty;

            for(Int32 i = 0; i < 8; i++)
            {
                Password += Alfabet[Generate.Next(0, Alfabet.Length + 1)];
            }

            HashPassword_TB.Text = Password;
        }
    }
}
