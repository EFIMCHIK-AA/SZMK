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
        public RegistrationUser_F(User TempUser)
        {
            this.TempUser = TempUser;
            InitializeComponent();
        }

        private User TempUser;

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

        private void RegistrationUser_F_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
            {
                try
                {
                    if (String.IsNullOrEmpty(DataReg_TB.Text))
                    {
                        DataReg_TB.Focus();
                        throw new Exception("Необходимо указать дату регистрации пользователя");
                    }

                    if (String.IsNullOrEmpty(Name_TB.Text))
                    {
                        Name_TB.Focus();
                        throw new Exception("Необходимо указать имя пользователя");
                    }

                    if (String.IsNullOrEmpty(Surname_TB.Text))
                    {
                        Surname_TB.Focus();
                        throw new Exception("Необходимо указать фамилию пользователя");
                    }

                    if (String.IsNullOrEmpty(MiddleName_TB.Text))
                    {
                        MiddleName_TB.Focus();
                        throw new Exception("Необходимо указать отчество пользователя");
                    }

                    if (String.IsNullOrEmpty(DOB_MTB.Text))
                    {
                        DOB_MTB.Focus();
                        throw new Exception("Необходимо указать пользователя");
                    }

                    DateTime Temp = Convert.ToDateTime(DOB_MTB.Text, DOB_MTB.Culture);

                    if (Position_CB.SelectedIndex < 0)
                    {
                        Position_CB.Focus();
                        throw new Exception("Необходимо выбрать должность сотрудника");
                    }

                    if (String.IsNullOrEmpty(Login_TB.Text))
                    {
                        Login_TB.Focus();
                        throw new Exception("Необходимо указать логин пользователя");
                    }

                    List<User> TempList = SystemArgs.Users;

                    if(TempUser != null)
                    {
                        TempList.Remove(TempUser);
                    }

                    for (Int32 i = 0; i < TempList.Count(); i++)
                    {
                        if (TempList[i].Login == Login_TB.Text.Trim())
                        {
                            Login_TB.Focus();
                            throw new Exception("Пользователь с указанным логином уже существует");
                        }
                    }

                    if (String.IsNullOrEmpty(HashPassword_TB.Text))
                    {
                        HashPassword_TB.Focus();
                        throw new Exception("Необходимо указать пароль пользователя");
                    }

                    if(HashPassword_TB.Text.Length < 8)
                    {
                        HashPassword_TB.Focus();
                        throw new Exception("Длина пароля должна быть 8 и более символов");
                    }
                }
                catch (FormatException)
                {
                    MessageBox.Show("Указан неверный формат даты рождения", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    e.Cancel = true;
                }
                catch (Exception E)
                {
                    MessageBox.Show(E.Message, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    e.Cancel = true;
                }
            }
        }

        private void RegistrationUser_F_Load(object sender, EventArgs e)
        {
            HashPassword_TB.UseSystemPasswordChar = true;
        }

        private void CheckPass_CB_CheckedChanged(object sender, EventArgs e)
        {
            HashPassword_TB.UseSystemPasswordChar = !HashPassword_TB.UseSystemPasswordChar;
        }
    }
}
