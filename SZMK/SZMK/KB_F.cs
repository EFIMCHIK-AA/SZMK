using System;
using System.Drawing;
using System.Threading;
using Npgsql;
using Equin.ApplicationFramework;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;

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
                Order_DGV.AutoGenerateColumns = false;
                Order_DGV.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Order_DGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                Load_F Dialog = new Load_F();
                Dialog.Show();
                SystemArgs.MobileApplication = new MobileApplication();
                SystemArgs.ClientProgram = new ClientProgram();
                SystemArgs.Orders = new List<Order>();
                SystemArgs.BlankOrders = new List<BlankOrder>();
                SystemArgs.Statuses = new List<Status>();
                SystemArgs.Excel = new Excel();
                SystemArgs.Template = new Template();
                ItemsFilter();
                RefreshOrder();
                Thread.Sleep(2000);

                Dialog.Close();
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void AddOrder_TSB_Click(object sender, EventArgs e)
        {
            if (AddOrder())
            {
                Display(SystemArgs.Orders);
            }
        }

        private void ChangeOrder_TSB_Click(object sender, EventArgs e)
        {
            if (ChangeOrder())
            {
                Display(SystemArgs.Orders);
            }
        }
        private void DeleteOrder_TSB_Click(object sender, EventArgs e)
        {
            if (DeleteOrder())
            {
                Display(SystemArgs.Orders);
            }
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
            if (ChangeOrder())
            {
                Display(SystemArgs.Orders);
            }
        }

        private void DeleteOrder_TSM_Click(object sender, EventArgs e)
        {
            if (DeleteOrder())
            {
                Display(SystemArgs.Orders);
            }
        }

        private void ReportDate_TSM_Click(object sender, EventArgs e)
        {
            if (ReportOrderOfDate())
            {
                MessageBox.Show("Отчет успешно сформирован и сохранен", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            Display(SystemArgs.Orders);
        }

        private void AdvancedSearch_TSB_Click(object sender, EventArgs e)
        {
            if (SearchParam())
            {
                Display(Result);
            }
        }
        private Boolean AddOrder()
        {
            try
            {
                SystemArgs.ServerMobileAppOrder = new ServerMobileAppOrder();//Сервер мобильного приложения
                Int64 IndexOrder = -1;
                KBScan_F Dialog = new KBScan_F();
                if (SystemArgs.ServerMobileAppOrder.Start())
                {
                    Dialog.ServerStatus_TB.Text = "Запущен";
                    Dialog.ServerStatus_TB.BackColor = Color.FromArgb(233, 245, 255);
                    Dialog.Status_TB.AppendText($"Ожидание данных" + Environment.NewLine);
                    if (Dialog.ShowDialog() == DialogResult.OK)
                    {
                        for(int i = 0; i < SystemArgs.ServerMobileAppOrder._ScanSession.Count; i++)
                        {
                            if (SystemArgs.ServerMobileAppOrder._ScanSession[i].Unique)
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
                                }
                                String[] SplitDataMatrix = SystemArgs.ServerMobileAppOrder._ScanSession[i].DataMatrix.Split('_');
                                BlankOrder TempBlank = new BlankOrder();
                                Int64 PositionID = SystemArgs.User.GetPosition().ID;
                                Status TempStatus = (from p in SystemArgs.Statuses
                                                    where p.IDPosition==PositionID
                                                    select p).Single();
                                Order TempOrder = new Order(IndexOrder + 1, SystemArgs.ServerMobileAppOrder._ScanSession[i].DataMatrix, DateTime.Now, SplitDataMatrix[0], SplitDataMatrix[3], Convert.ToInt64(SplitDataMatrix[1]), SplitDataMatrix[2], Convert.ToDouble(SplitDataMatrix[4].Replace('.', ',')), Convert.ToDouble(SplitDataMatrix[5].Replace('.', ',')), TempStatus, SystemArgs.User, TempBlank);
                                if (SystemArgs.Excel.AddToRegistry(TempOrder))
                                {
                                    if (SystemArgs.Request.InsertOrder(TempOrder))
                                    {
                                        SystemArgs.Orders.Add(TempOrder);
                                        SystemArgs.Request.InsertStatus(TempOrder);
                                    }
                                    else
                                    {
                                        MessageBox.Show("Ошибка при добавлении в базу данных DataMatrix: " + SystemArgs.ServerMobileAppOrder._ScanSession[i].DataMatrix, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Ошибка добавления данных в реестр, добавление в базу данных " +SystemArgs.ServerMobileAppOrder._ScanSession[i].DataMatrix+ " не будет произведено", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    return false;
                }
            }
            catch(Exception E)
            {
                MessageBox.Show(E.Message, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }
        private bool ChangeOrder()
        {
            try
            {
                if (Order_DGV.CurrentCell.RowIndex >= 0)
                {
                    Order Temp = (Order)View[Order_DGV.CurrentCell.RowIndex];
                    KBChangeOrder_F Dialog = new KBChangeOrder_F(Temp);

                    Dialog.Executor_TB.Text = Temp.Executor;
                    Dialog.Number_TB.Text = Temp.Number;
                    Dialog.List_TB.Text = Temp.List.ToString();
                    Dialog.Mark_TB.Text = Temp.Mark;
                    Dialog.Lenght_TB.Text = Temp.Lenght.ToString();
                    Dialog.Weight_TB.Text = Temp.Weight.ToString();

                    if (Dialog.ShowDialog() == DialogResult.OK)
                    {
                        String NewDataMatrix = Dialog.Number_TB.Text + "_" + Dialog.List_TB.Text + "_" + Dialog.Mark_TB.Text + "_" + Dialog.Executor_TB.Text + "_" + Dialog.Lenght_TB.Text + "_" + Dialog.Weight_TB.Text;
                        Order NewOrder = new Order(Temp.ID, NewDataMatrix, Temp.DateCreate, Dialog.Number_TB.Text, Dialog.Executor_TB.Text, Convert.ToInt64(Dialog.List_TB.Text), Dialog.Mark_TB.Text, Convert.ToDouble(Dialog.Lenght_TB.Text), Convert.ToDouble(Dialog.Weight_TB.Text), Temp.Status, Temp.User, Temp.BlankOrder);
                        if (SystemArgs.Request.UpdateOrder(NewOrder))
                        {
                            SystemArgs.Orders.Remove(Temp);
                            SystemArgs.Orders.Add(NewOrder);

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
                MessageBox.Show(E.Message, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }
        private bool DeleteOrder()
        {
            try
            {
                if (Order_DGV.CurrentCell.RowIndex >= 0)
                {
                    Order Temp = (Order)View[Order_DGV.CurrentCell.RowIndex];

                    if (MessageBox.Show("Вы действительно хотите удалить пользователя?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                    {
                        if (SystemArgs.Request.DeleteOrder(Temp))
                        {
                            SystemArgs.Orders.Remove(Temp);
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
        private void Display(List<Order> List)
        {
            if (Filter())
            {
                View = new BindingListView<Order>(List.Where(p=>p.Status.IDPosition==SystemArgs.User.GetPosition().ID).ToList());
                Order_DGV.DataSource = null;
                Order_DGV.DataSource = View;
                CountOrder_TB.Text = View.Count.ToString();
                if (View.Count > 0) 
                {
                    EnableButton(true);
                }
                else
                {
                    EnableButton(false);
                }
            }
            else
            {
                View = new BindingListView<Order>(List);
                Order_DGV.DataSource = null;
                Order_DGV.DataSource = View;
                CountOrder_TB.Text = View.Count.ToString();
                EnableButton(false);

            }
        }

        private void Order_DGV_SelectionChanged(object sender, EventArgs e)
        {
            if (Order_DGV.CurrentCell != null && Order_DGV.CurrentCell.RowIndex < View.Count())
            {
                Order Temp = (Order)View[Order_DGV.CurrentCell.RowIndex];

                Selection(Temp, true);
            }
            else
            {
                Selection(null, false);
            }
        }

        private void KB_F_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void Order_DGV_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            e.CellStyle.SelectionBackColor = Color.FromArgb(112, 238, 226);
            e.CellStyle.SelectionForeColor = Color.Black;
        }

        private void FilterCB_TSB_SelectedIndexChanged(object sender, EventArgs e)
        {
            Display(SystemArgs.Orders);
        }
        private void ItemsFilter()
        {
            FilterCB_TSB.Items.Add("Текущий статус");
            FilterCB_TSB.Items.Add("Все статусы");
            FilterCB_TSB.SelectedIndex = 0;
        }
        private bool Filter()
        {
            if (FilterCB_TSB.SelectedIndex == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private List<Order> ResultSearch(String TextSearch)
        {
            List<Order> Result = new List<Order>();

            if (!String.IsNullOrEmpty(TextSearch))
            {
                foreach (Order Temp in SystemArgs.Orders)
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

        List<Order> Result;

        private void ResetSearch()
        {
            if (Result != null)
            {
                Search_TSTB.Text = String.Empty;

                Result.Clear();
            }
        }
        private bool SearchParam()
        {
            try
            {
                KBSearchParam_F Dialog = new KBSearchParam_F();

                List<BlankOrder> BlankOrders = new List<BlankOrder>();

                BlankOrders.Add(new BlankOrder());
                BlankOrders.AddRange(SystemArgs.BlankOrders);
                Dialog.BlankOrder_CB.DataSource = BlankOrders;

                List<Status> Statuses = new List<Status>();

                Statuses.Add(new Status(-1, 0,"Не задан"));
                Statuses.AddRange(SystemArgs.Statuses);
                Dialog.Status_CB.DataSource = Statuses;

                if (Dialog.ShowDialog() == DialogResult.OK)
                {
                    Result = SystemArgs.Orders;

                    Result = Result.Where(p => (p.DateCreate >= Dialog.First_DP.Value.Date) && (p.DateCreate <= Dialog.Second_DP.Value.Date)).ToList();

                    if (Dialog.Executor_TB.Text.Trim() != String.Empty)
                    {
                        Result = Result.Where(p => p.Executor == Dialog.Executor_TB.Text.Trim()).ToList();
                    }

                    if (Dialog.Number_TB.Text.Trim() != String.Empty)
                    {
                        Result = Result.Where(p => p.Number == Dialog.Number_TB.Text.Trim()).ToList();
                    }

                    if (Dialog.List_TB.Text.Trim() != String.Empty)
                    {
                        Result = Result.Where(p => p.List.ToString() == Dialog.List_TB.Text.Trim()).ToList();
                    }

                    if (Dialog.Mark_TB.Text.Trim() != String.Empty)
                    {
                        Result = Result.Where(p => p.Mark == Dialog.Mark_TB.Text.Trim()).ToList();
                    }

                    if (Dialog.Lenght_TB.Text.Trim() != String.Empty)
                    {
                        Result = Result.Where(p => p.Lenght.ToString() == Dialog.Lenght_TB.Text.Trim()).ToList();
                    }

                    if (Dialog.Weight_TB.Text.Trim() != String.Empty)
                    {
                        Result = Result.Where(p => p.Weight.ToString() == Dialog.Weight_TB.Text.Trim()).ToList();
                    }
                    if (Dialog.BlankOrder_CB.SelectedIndex > 0)
                    {
                        Result = Result.Where(p => p.BlankOrder == (BlankOrder)Dialog.BlankOrder_CB.SelectedItem).ToList();
                    }
                    if (Dialog.Status_CB.SelectedIndex > 0)
                    {
                        Result = Result.Where(p => p.Status == (Status)Dialog.Status_CB.SelectedItem).ToList();
                    }
                    if (Dialog.User_CB.SelectedIndex > 0)
                    {
                        Result = Result.Where(p => p.User == (User)Dialog.User_CB.SelectedItem).ToList();
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
        private bool ReportOrderOfDate()
        {
            try
            {
                KBReportOrderOfDate_F Dialog = new KBReportOrderOfDate_F();
                if (Dialog.ShowDialog() == DialogResult.OK)
                {
                    if (SystemArgs.Excel.ReportOrderOfDate(Dialog.First_MC.SelectionStart, Dialog.Second_MC.SelectionStart))
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        private void EnableButton(Boolean Enable)
        {
            if (Enable)
            {
                ChangeOrder_TSB.Enabled = true;
                DeleteOrder_TSB.Enabled = true;
                ChangeOrder_TSM.Enabled = true;
                DeleteOrder_TSM.Enabled = true;
            }
            else
            {
                ChangeOrder_TSB.Enabled = false;
                DeleteOrder_TSB.Enabled = false;
                ChangeOrder_TSM.Enabled = false;
                DeleteOrder_TSM.Enabled = false;
            }
        }
        private void Selection(Order Temp, bool flag)
        {
            if (flag)
            {
                DateCreate_TB.Text = Temp.DateCreate.ToShortDateString();
                Executor_TB.Text = Temp.Executor;
                Number_TB.Text = Temp.Number;
                List_TB.Text = Temp.List.ToString();
                Mark_TB.Text = Temp.Mark;
                Lenght_TB.Text = Temp.Lenght.ToString();
                Weight_TB.Text = Temp.Weight.ToString();
                BlankOrder_TB.Text = Temp.BlankOrder.QR;
                Status_TB.Text = Temp.Status.Name;
            }
            else
            {
                DateCreate_TB.Text = String.Empty;
                Executor_TB.Text = String.Empty;
                Number_TB.Text = String.Empty;
                List_TB.Text = String.Empty;
                Mark_TB.Text = String.Empty;
                Lenght_TB.Text = String.Empty;
                Weight_TB.Text = String.Empty;
                BlankOrder_TB.Text = String.Empty;
                Status_TB.Text = String.Empty;
            }

        }
        private bool RefreshOrder()
        {
            List<Order> Temp = null;

            try
            {
                Temp = new List<Order>(SystemArgs.Orders);

                SystemArgs.Orders.Clear();
                SystemArgs.Statuses.Clear();
                SystemArgs.BlankOrders.Clear();
                SystemArgs.Request.GetAllBlankOrder();
                SystemArgs.Request.GetAllStatus();
                SystemArgs.Request.GetAllOrders();

                Display(SystemArgs.Orders);
                return true;
            }
            catch
            {
                if (Temp != null)
                {
                    Display(Temp);
                }
                throw;
            }
        }

        private void RefreshStatus_B_Click(object sender, EventArgs e)
        {
            try
            {
                RefreshOrder();
            }
            catch(Exception E)
            {
                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
