using System;
using System.Drawing;
using System.Threading;
using Npgsql;
using Equin.ApplicationFramework;
using System.Windows.Forms;
using System.Collections.Generic;

namespace SZMK
{
    public partial class KB_F : Form
    {
        public KB_F()
        {
            InitializeComponent();
        }
        BindingListView<Order> View;
        private void KB_F_Load(object sender, EventArgs e)
        {
            try
            {

                Load_F Dialog = new Load_F();

                Dialog.Show();

                SystemArgs.MobileApplication = new MobileApplication(); //Конфигурация мобильного приложения
                SystemArgs.Orders = new List<Order>();
                SystemArgs.BlankOrders = new List<BlankOrder>();
                SystemArgs.Statuses = new List<Status>();
                if (SystemArgs.Request.GetAllBlankOrder())
                {
                    if (SystemArgs.Request.GetAllStatus())
                    {
                        if (SystemArgs.Request.GetAllOrders())
                        {
                            Display(SystemArgs.Orders) ;
                        }
                        else
                        {
                            throw new Exception("Ошибка загрузки данных из базы");
                        }
                    }
                }

                Thread.Sleep(2000);

                Dialog.Close();
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddOrder_TSB_Click(object sender, EventArgs e)
        {
            AddOrder();
        }

        private void ChangeOrder_TSB_Click(object sender, EventArgs e)
        {
            ChangeOrder();
        }
        private void DeleteOrder_TSB_Click(object sender, EventArgs e)
        {
            DeleteOrder();
        }

        private void AddOrder_TSM_Click(object sender, EventArgs e)
        {
            if (AddOrder())
            {
                Display(SystemArgs.Orders);
            }
        }
        private void ChangeOrder_TSM_Click(object sender, EventArgs e)
        {
            ChangeOrder();
        }

        private void DeleteOrder_TSM_Click(object sender, EventArgs e)
        {
            DeleteOrder();
        }

        private void ReportDate_TSM_Click(object sender, EventArgs e)
        {

        }
        private void Exit_TSM_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Search_TSB_Click(object sender, EventArgs e)
        {
                                        
        }

        private void Reset_TSB_Click(object sender, EventArgs e)
        {

        }

        private void AdvancedSearch_TSB_Click(object sender, EventArgs e)
        {

        }
        private Boolean AddOrder()
        {
            try
            {
                SystemArgs.ServerMobileApp = new ServerMobileApp();//Сервер мобильного приложения
                Int64 IndexOrder = -1;
                Int64 IndexBlank = -1;
                KBScan_F Dialog = new KBScan_F();
                if (SystemArgs.ServerMobileApp.Start())
                {
                    Dialog.ServerStatus_TB.Text = "Запущен";
                    Dialog.ServerStatus_TB.BackColor = Color.FromArgb(233, 245, 255);
                    Dialog.Status_TB.AppendText($"Ожидание QR" + Environment.NewLine);
                    if (Dialog.ShowDialog() == DialogResult.OK)
                    {
                        for(int i = 0; i < SystemArgs.ServerMobileApp._ScanSession.Count; i++)
                        {
                            if (SystemArgs.ServerMobileApp._ScanSession[i]._Unique)
                            {
                                using (var Connect = new NpgsqlConnection(SystemArgs.DataBase.ToString()))
                                {
                                    Connect.Open();

                                    using (var Command = new NpgsqlCommand($"SELECT last_value FROM \"Orders_ID_seq\"", Connect))
                                    {
                                        using (var Reader = Command.ExecuteReader())
                                        {
                                            while (Reader.Read())
                                            {
                                                IndexOrder = Reader.GetInt64(0);
                                            }
                                        }
                                    }
                                    using (var Command = new NpgsqlCommand($"SELECT last_value FROM \"BlankOrder_ID_seq\"", Connect))
                                    {
                                        using (var Reader = Command.ExecuteReader())
                                        {
                                            while (Reader.Read())
                                            {
                                                IndexBlank = Reader.GetInt64(0);
                                            }
                                        }
                                    }
                                }
                                String[] SplitDataMatrix = SystemArgs.ServerMobileApp._ScanSession[i].DataMatrix.Split('_');
                                Order TempOrder = new Order(IndexOrder + 1, SystemArgs.ServerMobileApp._ScanSession[i].DataMatrix, DateTime.Now, SplitDataMatrix[0], SplitDataMatrix[3], Convert.ToInt64(SplitDataMatrix[1]), SplitDataMatrix[2], Convert.ToDouble(SplitDataMatrix[4]), Convert.ToDouble(SplitDataMatrix[5]),SystemArgs.Status,SystemArgs.User,SystemArgs.BlankOrder);
                                BlankOrder TempBlank = new BlankOrder(IndexBlank + 1,DateTime.Now,"Бланк заказа не задан");
                                if (SystemArgs.Request.InsertOrderDB(TempOrder))
                                {
                                    SystemArgs.Orders.Add(TempOrder);
                                    if(SystemArgs.Request.AddOrGetOrUpdateBlankOrder(TempBlank,IndexOrder+1))
                                    {
                                        SystemArgs.Request.InsertBlankOrder(IndexOrder + 1,TempBlank.ID);
                                        SystemArgs.BlankOrders.Add(TempBlank);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Ошибка при добавлении в базу данных DataMatrix: "+SystemArgs.ServerMobileApp._ScanSession[i].DataMatrix, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return false;
                                }

                            }
                        }
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    Dialog.ServerStatus_TB.Text = "Остановлен";
                    Dialog.ServerStatus_TB.BackColor = Color.Red;
                    return false;
                }
            }
            catch(Exception E)
            {
                MessageBox.Show(E.Message, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }
        private void ChangeOrder()
        {

        }
        private void DeleteOrder()
        {

        }
        private void Display(List<Order> List)
        {
            View = new BindingListView<Order>(List);
            Order_DGV.DataSource = View;
        }

        private void Order_DGV_SelectionChanged(object sender, EventArgs e)
        {

        }

        private void Order_DGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void KB_F_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
