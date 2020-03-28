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
    public partial class AVT_Autorization_F : Form
    {
        public AVT_Autorization_F()
        {
            InitializeComponent();
        }

        private bool UpdatePassword(User User)
        {
            String Password = String.Empty;

            AVT_ChangePassword_F Dialog = new AVT_ChangePassword_F();

            if (Dialog.ShowDialog() == DialogResult.OK)
            {
                Password = Hash.GetSHA256(Dialog.NewPassword_TB.Text.Trim());

                if (SystemArgs.Request.UpdatePasswordText(Password, User))
                {
                    if (SystemArgs.Request.UpdatePassword(User, true))
                    {
                        Int32 Index = SystemArgs.Users.FindIndex(p => p.ID == User.ID);

                        SystemArgs.Users[Index].UpdPassword = true;
                        SystemArgs.Users[Index].HashPassword = Password;

                        SystemArgs.User = null;

                        return true;
                    }
                    else
                    {
                        throw new Exception("Обнаружена ошибка при обновлении пароля");
                    }
                }
            }

            return false;
        }

        private void Enter_B_Click(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(Login_CB.Text.Trim()))
                {
                    User User = SearchUser(Login_CB.Text.Trim());

                    if (User != null)
                    {
                        String Password = Password_TB.Text.Trim();

                        if (ComparePassword(User, Password))
                        {
                            SystemArgs.User = User;

                            if(SystemArgs.User.UpdPassword)
                            {
                                Start(User);
                            }
                            else
                            {
                                if(UpdatePassword(SystemArgs.User))
                                {
                                    MessageBox.Show("Пароль успешно изменен!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                        }
                        else
                        {
                            throw new Exception("Обнаружена ошибка при вводе логина или пароля");
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
                        AD_Adminstrator_F Administrator = new AD_Adminstrator_F();
                        this.Hide();
                        Administrator.Show();
                        break;
                    case 2: //КБ
                        KB_F KB = new KB_F();
                        this.Hide();
                        KB.Show();
                        break;
                    case 3: //Архивариус
                        AR_Arhive_F Arhive = new AR_Arhive_F();
                        this.Hide();
                        Arhive.Show();
                        break;
                    case 4://Сотрудник ПДО
                        PDO_F PDO = new PDO_F();
                        this.Hide();
                        PDO.Show();
                        break;
                    case 5://Сотрудник ОПП
                        OPP_F OPP = new OPP_F();
                        this.Hide();
                        OPP.Show();
                        break;
                    case 6:
                        Chief_PDO_F Chief_PDO = new Chief_PDO_F();
                        this.Hide();
                        Chief_PDO.Show();
                        break;
                    default:
                        throw new Exception("Должности пользователя не существует");
                }
            }
        }

        private User SearchUser(String Login)
        {
            foreach (User Temp in SystemArgs.Users)
            {
                if (Temp.Login == Login)
                {
                    return Temp;
                }
            }

            return null;
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
                SystemArgs.Path = new Path(); //Системные пути
                SystemArgs.DataBase = new DataBase(); //Конфигурация базы данных
                SystemArgs.Request = new Request(); //Слой запросов к базе данных
                SystemArgs.RequestLinq = new RequestLinq(); //Слой запросов linq к полученным данным из БД
                SystemArgs.AboutProgram = new AboutProgram();

                if (SystemArgs.DataBase.CheckConnect(SystemArgs.DataBase.ToString()))
                {
                    Password_TB.UseSystemPasswordChar = true;

                    List<User> Users = new List<User>();
                    SystemArgs.Users = new List<User>(); //Список всех пользователей в программе
                    SystemArgs.Positions = new List<Position>(); //Общий список должностей
                    SystemArgs.Request.GetAllPositions();
                    SystemArgs.Mails = new List<Mail>(); //Общий список адресов почты
                    SystemArgs.Request.GetAllMails();
                    SystemArgs.Statuses = new List<Status>();
                    SystemArgs.Request.GetAllStatus();
                    SystemArgs.Request.GetAllUsers();

                    Users.Add(new User(-1, "Не выбрано", "Нет отчества", "Нет фамилии", DateTime.Now, 1, new List<Mail>(), "Не выбрано", "Нет хеша",true));
                    Users.AddRange(SystemArgs.Users);
                    Login_CB.DataSource = Users;
                }
                else
                {
                    if (MessageBox.Show("Отсутствует соединение с базой данных. Работа программного обеспечения приостановлена", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK)
                    {

                    }

                    Application.Exit();
                }      
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

        private void Autorization_F_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Enter_B.PerformClick();
            }
            else if(e.KeyCode == Keys.Escape)
            {
                Cancel_B.PerformClick();
            }
        }
    }
}
