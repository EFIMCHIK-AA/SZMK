using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeneratePassword
{
    public partial class Main_F : Form
    {
        public Main_F()
        {
            InitializeComponent();
        }

        private (bool,String) Encrypt(String Password)
        {
            String EncPass = Ecnryption.EncryptRSA(Password);

            return (true, EncPass);
        }

        private void RefreshStatus_B_Click(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(Password_TB.Text))
                {
                    (bool, String) Temp = Encrypt(Password_TB.Text.Trim());

                    if (Temp.Item1)
                    {
                        Encryption_TB.Text = Temp.Item2;
                        Status_TB.Text = "Успешно";
                        Status_TB.BackColor = Color.Lime;
                    }
                    else
                    {
                        throw new Exception("Ошибка при попытке шифрования");
                    }
                }
                else
                {
                    Password_TB.Focus();
                    Encryption_TB.Clear();
                    throw new Exception("Необходимо указать последовательность символов");
                }
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Status_TB.Text = "Ошибка";
                Status_TB.BackColor = Color.Red;
            }
        }

        private (bool, String) Generate()
        {
            String Alfabet = "QqWwEeRrTtYyUuIiOoPpAaSsDdFfGgHhJjKkLlZzXxCcVvBbNnMm#1234567890!@#$%^&*-+";
            Random Generate = new Random();

            String Password = String.Empty;

            for (Int32 i = 0; i < 8; i++)
            {
                Password += Alfabet[Generate.Next(0, Alfabet.Length)];
            }

            return (true, Password);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                (bool, String) Temp = Generate();

                if (Temp.Item1)
                {
                    Password_TB.Text = Temp.Item2;

                    GeneratePass_B.PerformClick();

                    Status_TB.Text = "Успешно";
                    Status_TB.BackColor = Color.Lime;
                }
                else
                {
                    throw new Exception("Ошибка при попытке генерации");
                }
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Status_TB.Text = "Ошибка";
                Status_TB.BackColor = Color.Red;
            }
        }

        private void Main_F_Load(object sender, EventArgs e)
        {
            Status_TB.Text = "Ожидание генерации";
        }
    }
}
