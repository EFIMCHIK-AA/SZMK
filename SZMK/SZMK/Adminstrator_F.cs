using System;
using System.Globalization;
using Equin.ApplicationFramework;
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
    public partial class Adminstrator_F : Form
    {
        public Adminstrator_F()
        {
            InitializeComponent();
        }

        private void Display(List<User> List)
        {
            BindingListView<User> Display = new BindingListView<User>(List);
        }

        private void Add_B_Click(object sender, EventArgs e)
        {

        }

        private void анализToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void Adminstrator_F_Load(object sender, EventArgs e)
        {

        }

        private void Users_DGV_SelectionChanged(object sender, EventArgs e)
        {
            if(Users_DGV.CurrentCell != null)
            {
                User Temp = SystemArgs.Users[Users_DGV.CurrentCell.RowIndex];

                Name_TB.Text = Temp.Name;
                Surname_TB.Text = Temp.Surname;
                MiddleName_TB.Text = Temp.MiddleName;
                DOB_TB.Text = Temp.DateOfBirth.ToShortDateString();
                Position_TB.Text = Temp.GetPosition().Name;

                ID_TB.Text = Temp.ID.ToString();
                DataReg_TB.Text = Temp.DateCreate.ToShortDateString();
                Admin_TB.Text = Temp.Admin.Name;
                Login_TB.Text = Temp.Login;
                HashPassword_TB.Text = Temp.HashPassword;
            }
        }

        private bool AddUser()
        {
            RegistrationUser_F Dialog = new RegistrationUser_F();
            DateTime DateCreate = DateTime.Now;
            Dialog.DataReg_TB.Text = DateCreate.ToShortDateString();
            Dialog.Position_CB.DataSource = SystemArgs.Positions;

            if(Dialog.ShowDialog() == DialogResult.OK)
            {
                Int64 Index = -1; //Получить уникальный индекс из базы данных
                //Записать данные в базу данных
                User Temp = new User(Index, Dialog.Name_TB.Text, Dialog.MiddleName_TB.Text, Dialog.Surname_TB.Text, DateCreate,
                                    Convert.ToDateTime(Dialog.DOB_MTB.Text, Dialog.DOB_MTB.Culture), SystemArgs.Positions[Dialog.Position_CB.SelectedIndex].ID,
                                    new List<Int64>(), SystemArgs.User, Dialog.Login_TB.Text, Hash.GetSHA256(Dialog.HashPassword_TB.Text));
                SystemArgs.Users.Add(Temp);
                return true;
            }
            else
            {
                return false;
            }
        }

        private void Add_TSB_Click(object sender, EventArgs e)
        {
            if(AddUser())
            {
                Display(SystemArgs.Users);
            }
        }
    }
}
