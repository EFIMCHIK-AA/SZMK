﻿using System;
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
using System.Net;
using System.IO;
using System.Threading;
using Npgsql;

namespace SZMK
{
    public partial class AD_Adminstrator_F : Form
    {
        public AD_Adminstrator_F()
        {
            InitializeComponent();
        }

        BindingListView<User> View;

        private void Display(List<User> List)
        {
            View = new BindingListView<User>(List);
            Users_DGV.DataSource = View;
            Count_TB.Text = View.Count.ToString();
        }

        private void SetStatus(TextBox Textbox, Color Color, Boolean Flag)
        {
            if(Flag)
            {
                Textbox.Text = $"Успешно установлено <{DateTime.Now}>";
                Textbox.BackColor = Color;
            }
            else
            {
                Textbox.Text = $"Не установлено <{DateTime.Now}>";
                Textbox.BackColor = Color;
            }
        }

        private void GetStatus()
        {
            if (SystemArgs.DataBase.CheckConnect(SystemArgs.DataBase.ToString()))
            {
                SetStatus(Connect_TB, Color.Lime, true);
            }
            else
            {
                SetStatus(Connect_TB, Color.Red, false);
            }

            if (SystemArgs.MobileApplication.CheckFile())
            {
                SetStatus(StatusMobile_TB, Color.Lime, true);
            }
            else
            {
                SetStatus(StatusMobile_TB, Color.Red, false);
            }

            if (SystemArgs.ClientProgram.CheckFile())
            {
                SetStatus(StatusConf_TB, Color.Lime, true);
            }
            else
            {
                SetStatus(StatusConf_TB, Color.Red, false);
            }

            if(SystemArgs.ByteScout.CheckConnect())
            {
                SetStatus(StatusServer_TB, Color.Lime, true);
            }
            else
            {
                SetStatus(StatusServer_TB, Color.Red, false);
            }

            if (SystemArgs.ServerMail.CheckFile())
            {
                SetStatus(StatusMail_TB, Color.Lime, true);
            }
            else
            {
                SetStatus(StatusMail_TB, Color.Red, false);
            }
        }

        private void Adminstrator_F_Load(object sender, EventArgs e)
        {
            try
            {
                Users_DGV.AutoGenerateColumns = false;
                Users_DGV.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                Load_F Dialog = new Load_F();

                Dialog.Show();

                SystemArgs.MobileApplication = new MobileApplication(); //Конфигурация мобильного приложения
                SystemArgs.ClientProgram = new ClientProgram(); // Конфигурация клиентского программного обеспечения
                SystemArgs.ByteScout = new ByteScout(); // Конфигурация программы распознавания
                SystemArgs.ServerMail = new ServerMail(); // Конфигурация почтового сервера

                GetStatus();

                Thread.Sleep(1300);

                Dialog.Close();

                if (SystemArgs.Users.Count() <= 0)
                {
                    Change_TSB.Enabled = false;
                    Delete_TSB.Enabled = false;
                }

                Display(SystemArgs.Users);
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Selection(User Temp, bool flag)
        {
            if(flag)
            {
                Users_DGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect; //Выделение строки
                Change_TSB.Enabled = true;
                Delete_TSB.Enabled = true;

                Surname_TB.Text = Temp.Name;
                Name_TB.Text = Temp.Surname;
                MiddleName_TB.Text = Temp.MiddleName;
                Position_TB.Text = Temp.GetPosition().Name;
                ID_TB.Text = Temp.ID.ToString();
                DataReg_TB.Text = Temp.DateCreate.ToShortDateString();
                Login_TB.Text = Temp.Login;
                HashPassword_TB.Text = Temp.HashPassword;
            }
            else
            {
                Change_TSB.Enabled = false;
                Delete_TSB.Enabled = false;

                Surname_TB.Text = String.Empty;
                Name_TB.Text = String.Empty;
                MiddleName_TB.Text = String.Empty;
                Position_TB.Text = String.Empty;
                ID_TB.Text = String.Empty;
                DataReg_TB.Text = String.Empty;
                Login_TB.Text = String.Empty;
                HashPassword_TB.Text = String.Empty;
            }

        }

        private void Users_DGV_SelectionChanged(object sender, EventArgs e)
        {
            if (Users_DGV.CurrentCell != null && Users_DGV.CurrentCell.RowIndex < View.Count())
            {
                User Temp = (User)View[Users_DGV.CurrentCell.RowIndex];

                Selection(Temp,true);
            }
            else
            {
                Selection(null, false);
            }
        }

        private bool AddUser()
        {
            try
            {
                AD_RegistrationUser_F Dialog = new AD_RegistrationUser_F(null);

                DateTime DateCreate = DateTime.Now;
                Dialog.DataReg_TB.Text = DateCreate.ToShortDateString();
                Dialog.Position_CB.DataSource = SystemArgs.Positions;

                if (Dialog.ShowDialog() == DialogResult.OK)
                {
                    Int64 Index = -1;

                    using (var Connect = new NpgsqlConnection(SystemArgs.DataBase.ToString()))
                    {
                        Connect.Open();

                        using (var Command = new NpgsqlCommand($"SELECT last_value FROM \"User_ID_seq\"", Connect))
                        {
                            using (var Reader = Command.ExecuteReader())
                            {
                                while (Reader.Read())
                                {
                                    Index = Reader.GetInt64(0);
                                }
                            }
                        }
                    }

                    Boolean UpdatePassword = true;

                    if(Dialog.ChanageNext_CB.Checked)
                    {
                        UpdatePassword = false;
                    }

                    User Temp = new User(Index + 1, Dialog.Name_TB.Text, Dialog.MiddleName_TB.Text, Dialog.Surname_TB.Text , DateCreate, SystemArgs.Positions[Dialog.Position_CB.SelectedIndex].ID,
                                        new List<Mail>(), Dialog.Login_TB.Text, Hash.GetSHA256(Dialog.HashPassword_TB.Text), UpdatePassword);

                    if(SystemArgs.Request.AddUser(Temp))
                    {
                        SystemArgs.Users.Add(Temp);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void Add_TSB_Click(object sender, EventArgs e)
        {
            if (AddUser())
            {
                Display(SystemArgs.Users);
            }
        }

        private bool ChangeUser()
        {
            try
            {
                if (Users_DGV.CurrentCell.RowIndex >= 0)
                {
                    User Temp = (User)View[Users_DGV.CurrentCell.RowIndex];

                    if (Temp.Login == "ROOT")
                    {
                        MessageBox.Show("Изменение пользователя ROOT запрещено", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    if (Temp.Login == SystemArgs.User.Login)
                    {
                        MessageBox.Show("Изменение своего пользователя запрещено", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    AD_RegistrationUser_F Dialog = new AD_RegistrationUser_F(Temp)
                    {
                        Text = "Измененте параметров пользователя",
                    };

                    Dialog.DataReg_TB.Text = Temp.DateCreate.ToShortDateString();
                    Dialog.Surname_TB.Text = Temp.Name;
                    Dialog.MiddleName_TB.Text = Temp.MiddleName;
                    Dialog.Name_TB.Text = Temp.Surname;
                    Dialog.Position_CB.DataSource = SystemArgs.Positions;

                    for (Int32 i = 0; i < SystemArgs.Positions.Count; i++)
                    {
                        if (Temp.GetPosition().ID == SystemArgs.Positions[i].ID)
                        {
                            Dialog.Position_CB.SelectedIndex = i;
                        }
                    }

                    Dialog.label2.Text = "Укажите новые данные";
                    Dialog.Login_TB.Text = Temp.Login;
                    Dialog.HashPassword_TB.Text = String.Empty;

                    if (Dialog.ShowDialog() == DialogResult.OK)
                    {
                        Boolean UpdatePassword = true;

                        if (Dialog.ChanageNext_CB.Checked)
                        {
                            UpdatePassword = false;
                        }

                        User NewUser = new User(Temp.ID, Dialog.Surname_TB.Text, Dialog.MiddleName_TB.Text, Dialog.Name_TB.Text, Temp.DateCreate,
                                                SystemArgs.Positions[Dialog.Position_CB.SelectedIndex].ID,
                                            Temp.Mails, Dialog.Login_TB.Text, Hash.GetSHA256(Dialog.HashPassword_TB.Text.Trim()), UpdatePassword);

                        if(SystemArgs.Request.ChangeUser(NewUser))
                        {
                            SystemArgs.Users.Remove(Temp);
                            SystemArgs.Users.Add(NewUser);

                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    throw new Exception("Необходимо выбрать объект");
                }
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void Change_TSB_Click(object sender, EventArgs e)
        {
            if (ChangeUser())
            {
                Display(SystemArgs.Users);
            }
        }

        private bool DeleteUser()
        {
            try
            {
                if (Users_DGV.CurrentCell.RowIndex >= 0)
                {
                    User Temp = (User)View[Users_DGV.CurrentCell.RowIndex];

                    if (Temp.Login == "ROOT")
                    {
                        MessageBox.Show("Удаление пользователя ROOT запрещено", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    if (Temp.Login == SystemArgs.User.Login)
                    {
                        MessageBox.Show("Удаление своего пользователя запрещено", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    if (MessageBox.Show("Вы действительно хотите удалить пользователя?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                    {
                        if(SystemArgs.Request.DeleteUser(Temp))
                        {
                            SystemArgs.Users.Remove(Temp);
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    throw new Exception("Необходимо выбрать объект");
                }
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void Delete_TSB_Click(object sender, EventArgs e)
        {
            if (DeleteUser())
            {
                Display(SystemArgs.Users);

                if(SystemArgs.Users.Count <= 0)
                {
                    Delete_TSB.Enabled = false;
                    Change_TSB.Enabled = false;
                }
            }
        }

        private List<User> ResultSearch(String TextSearch)
        {
            List<User> Result = new List<User>();

            if (!String.IsNullOrEmpty(TextSearch))
            {
                foreach (User Temp in SystemArgs.Users)
                {
                    if (Temp.SearchString().IndexOf(TextSearch) != -1)
                    {
                        Result.Add(Temp);
                    }
                }
            }

            SystemArgs.PrintLog("Перебор значений по заданным параметрам успешно завершен");

            return Result;
        }



        private bool Search()
        {
            try
            {
                if (!String.IsNullOrEmpty(Search_TSTB.Text))
                {
                    String SearchText = Search_TSTB.Text.Trim();

                    Result = ResultSearch(SearchText);

                    if (Result.Count <= 0)
                    {
                        Search_TSTB.Focus();
                        MessageBox.Show("Поиск не дал результатов", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        SystemArgs.PrintLog("Количество объектов по параметрам поиска 0");
                        return false;
                    }

                    return true;
                }
                else
                {
                    ResetSearch();
                    SystemArgs.PrintLog("Получено пустое значение параметра поиска");
                    return false;
                }
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        List<User> Result;

        private void ResetSearch()
        {
            if(Result != null)
            {
                Search_TSTB.Text = String.Empty;

                Result.Clear();
            }
        }

        private void Search_TSB_Click(object sender, EventArgs e)
        {
            if (Search())
            {
                if (Result != null)
                {
                    Display(Result);
                }
            }
        }

        private void Reset_TSB_Click(object sender, EventArgs e)
        {
            ResetSearch();
            Display(SystemArgs.Users);
        }

        private bool SearchParam()
        {
            try
            {
                AD_SearchParamUsers_F Dialog = new AD_SearchParamUsers_F();

                List<Position> Positions = new List<Position>();

                Positions.Add(new Position(-1, "Не выбрано"));
                Positions.AddRange(SystemArgs.Positions);
                Dialog.Position_CB.DataSource = Positions;

                if (Dialog.ShowDialog() == DialogResult.OK)
                {
                    Result = SystemArgs.Users;

                    if (Dialog.Name_TB.Text.Trim() != String.Empty)
                    {
                        Result = Result.Where(p => p.Name == Dialog.Name_TB.Text.Trim()).ToList();
                    }

                    if (Dialog.MiddleName_TB.Text.Trim() != String.Empty)
                    {
                        Result = Result.Where(p => p.MiddleName == Dialog.MiddleName_TB.Text.Trim()).ToList();
                    }

                    if (Dialog.Surname_TB.Text.Trim() != String.Empty)
                    {
                        Result = Result.Where(p => p.Surname == Dialog.Surname_TB.Text.Trim()).ToList();
                    }

                    if (Dialog.Position_CB.SelectedIndex > 0)
                    {
                        Result = Result.Where(p => p.GetPosition() == (Position)Dialog.Position_CB.SelectedItem).ToList();
                    }

                    if (Dialog.ID_TB.Text.Trim() != String.Empty)
                    {
                        Result = Result.Where(p => p.ID == Convert.ToInt64(Dialog.ID_TB.Text.Trim())).ToList();
                    }

                    if (Dialog.Login_TB.Text.Trim() != String.Empty)
                    {
                        Result = Result.Where(p => p.Surname == Dialog.Login_TB.Text.Trim()).ToList();
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void SearchParam_TSB_Click(object sender, EventArgs e)
        {
            if (SearchParam())
            {
                Display(Result);
            }
        }

        private void MobileSettings_TSMB_Click(object sender, EventArgs e)
        {
            try
            {
                AD_SettingsMobileApp_F Dialog = new AD_SettingsMobileApp_F();

                if (SystemArgs.MobileApplication.GetParametersConnect())
                {
                    String MyIP = Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString();

                    Dialog.IP_TB.Text = MyIP;
                    Dialog.Port_TB.Text = SystemArgs.MobileApplication.Port;

                    Zen.Barcode.CodeQrBarcodeDraw QrCode = Zen.Barcode.BarcodeDrawFactory.CodeQr;
                    Dialog.QR_PB.Image = QrCode.Draw($"{MyIP}_{SystemArgs.MobileApplication.Port}_{SystemArgs.DataBase.Name}_{SystemArgs.DataBase.Owner}_{SystemArgs.DataBase.Password}_{SystemArgs.DataBase.IP}_{SystemArgs.DataBase.Port}", 100);
                }

                if (Dialog.ShowDialog() == DialogResult.OK)
                {

                }
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DataBase_TSMB_Click(object sender, EventArgs e)
        {
            try
            {
                AD_SettingsDataBase_F Dialog = new AD_SettingsDataBase_F();

                if (SystemArgs.DataBase.GetParametersConnect())
                {
                    Dialog.Server_TB.Text = SystemArgs.DataBase.IP;
                    Dialog.Owner_TB.Text = SystemArgs.DataBase.Owner;
                    Dialog.Port_TB.Text = SystemArgs.DataBase.Port;
                    Dialog.Name_TB.Text = SystemArgs.DataBase.Name;
                    Dialog.Password_TB.Text = SystemArgs.DataBase.Password;
                }

                if (Dialog.ShowDialog() == DialogResult.OK)
                {

                }
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void настройкиToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                AD_SettingsProgram_F Dialog = new AD_SettingsProgram_F();

                if (SystemArgs.ClientProgram.GetParametersConnect())
                {
                    Dialog.CheckMarks_CB.Checked = SystemArgs.ClientProgram.CheckMarks;
                    Dialog.N1_NUD.Value = SystemArgs.ClientProgram.VisualRow_N1;
                    Dialog.N2_NUD.Value = SystemArgs.ClientProgram.VisualRow_N2;
                }

                if (Dialog.ShowDialog() == DialogResult.OK)
                {

                }
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void настройкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                AD_SettingsByteScout_F Dialog = new AD_SettingsByteScout_F();

                if (SystemArgs.ByteScout.GetParametersConnect())
                {
                    Dialog.IP_TB.Text = SystemArgs.ByteScout.Server;
                    Dialog.Port_TB.Text = SystemArgs.ByteScout.Port;
                }

                if (Dialog.ShowDialog() == DialogResult.OK)
                {

                }
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Mail_TSMB_Click(object sender, EventArgs e)
        {
            try
            {
                AD_SettingsMails_F Dialog = new AD_SettingsMails_F();

                Dialog.Mails_DGV.AutoGenerateColumns = false;

                if(Dialog.ShowDialog() == DialogResult.OK)
                {

                }
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Menu_MS_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void Adminstrator_F_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void Users_DGV_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            e.CellStyle.SelectionBackColor = Color.FromArgb(112, 238, 226);
            e.CellStyle.SelectionForeColor = Color.Black;
        }

        private void RefreshStatus_B_Click(object sender, EventArgs e)
        {
            List<User> Temp = null;

            try
            {
                Temp = new List<User>(SystemArgs.Users);

                SystemArgs.Users.Clear();

                SystemArgs.Request.GetAllUsers();

                Display(SystemArgs.Users);
            }
            catch (Exception E)
            {
                if(Temp != null)
                {
                    Display(Temp);
                }

                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Adminstrator_F_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && ActiveControl == Search_TSTB.Control)
            {
                Search_TSB.PerformClick();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                if (MessageBox.Show("Вы дейстивтельно хотите выйти?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    Application.Exit();
                }
            }
        }

        private void AboutProgram_TSM_Click(object sender, EventArgs e)
        {
            ALL_AboutProgram_F Dialog = new ALL_AboutProgram_F();
            if(Dialog.ShowDialog() == DialogResult.OK)
            {

            }
        }

        private void SearchOrderScan_TSM_Click(object sender, EventArgs e)
        {

        }
    }
}
