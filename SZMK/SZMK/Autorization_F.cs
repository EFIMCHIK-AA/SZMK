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
            try
            {
                if (Login_CB.SelectedIndex > 0)
                {
                    User User = Login_CB.SelectedItem as User;

                    if (SearchUser(User))
                    {
                        String Password = Password_TB.Text.Trim();

                        if (ComparePassword(User, Password))
                        {
                            SystemArgs.User = User;

                            Start(User);
                        }
                        else
                        {
                            throw new Exception("Обнаружена ошибка при вводе пароля");
                        }
                    }
                    else
                    {
                        throw new Exception("Пользователь не существует");
                    }
                }
                else
                {
                    throw new Exception("Пользователь не выбран");
                }
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }    
        }

        private void Start(User User)
        {
            Position Position = User.GetPosition();

            if(Position != null)
            {
                Int64 IDPosition = Position.ID;

                switch(IDPosition)
                {
                    case 1: //Администратор
                        Adminstrator_F Administrator = new Adminstrator_F();
                        this.Hide();
                        Administrator.Show();
                        break;
                    case 2: //КБ
                        KB_F KB = new KB_F();
                        KB.Show();
                        break;
                    case 3: //Архивариус
                        break;
                    case 4://Сотрудник ПДО
                        break;
                    case 5:
                        break;
                    case 6:
                        break;
                    default:
                        throw new Exception("Должности пользователя не существует");
                }
            }
        }

        private bool SearchUser(User User)
        {
            if(User != null)
            {
                foreach (User Temp in SystemArgs.Users)
                {
                    if (Temp == User)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool ComparePassword(User User, String Password)
        {
            if (User.HashPassword == Hash.GetSHA256(Password))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void Autorization_F_Load(object sender, EventArgs e)
        {
            try
            {
                Password_TB.UseSystemPasswordChar = true;

                List<User> Users = new List<User>();
                SystemArgs.Users = new List<User>(); //Список всех пользователей в программе
                SystemArgs.Path = new Path(); //Системные пути
                SystemArgs.DataBase = new DataBase(); //Конфигурация базы данных
                SystemArgs.Request = new Request(); //Слой запросов к базе данных

                SystemArgs.Positions = new List<Position>(); //Общий список должностей
                SystemArgs.Request.GetAllPositions();
                SystemArgs.Mails = new List<Mail>(); //Общий список адресов почты
                SystemArgs.Request.GetAllMails();

                SystemArgs.Request.GetAllUsers();

                Users.Add(new User(-1, "Не выбрано", "Нет отчества", "Нет фамилии", DateTime.Now, DateTime.Now, 1, new List<Mail>(), "Не выбрано", "Нет хеша"));
                Users.AddRange(SystemArgs.Users);
                Login_CB.DataSource = Users;
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        private void CheckPass_CB_CheckedChanged(object sender, EventArgs e)
        {
            Password_TB.UseSystemPasswordChar = !Password_TB.UseSystemPasswordChar;
        }

        private void Autorization_F_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void Cancel_B_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите выйти?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                Application.Exit();
            }
        }
    }
}
