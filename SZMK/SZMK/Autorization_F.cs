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
    public partial class Autorization_F : Form
    {
        public Autorization_F()
        {
            InitializeComponent();
        }

        private void Enter_B_Click(object sender, EventArgs e)
        {
            String CurrUser = Login_CB.SelectedItem.ToString();
            String CurrPassword = Password_TB.Text.Trim();
        }

        private void Autorization_F_Load(object sender, EventArgs e)
        {

        }
    }
}
