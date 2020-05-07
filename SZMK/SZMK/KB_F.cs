using System;
using System.Drawing;
using System.Threading;
using Npgsql;
using Equin.ApplicationFramework;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Reflection;
using AForge.Video.DirectShow;
using System.Xml.Linq;

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
                SetDoubleBuffered(Order_DGV);

                View = new BindingListView<Order>(new List<Order>());

                LoadData_PB.Image = Properties.Resources._239;
                LoadData_PB.SizeMode = PictureBoxSizeMode.CenterImage;

                Load_F Dialog = new Load_F();
                Dialog.Show();

                SystemArgs.MobileApplication = new MobileApplication();
                SystemArgs.ServerMail = new ServerMail();
                SystemArgs.UnLoadSpecific = new UnLoadSpecific();
                SystemArgs.ClientProgram = new ClientProgram();
                SystemArgs.Orders = new List<Order>();
                SystemArgs.BlankOrders = new List<BlankOrder>();
                SystemArgs.BlankOrderOfOrders = new List<BlankOrderOfOrder>();
                SystemArgs.StatusOfOrders = new List<StatusOfOrder>();
                SystemArgs.Excel = new Excel();
                SystemArgs.Template = new Template();
                SystemArgs.SelectedColumn = new SelectedColumn();

                ItemsFilter();
                SelectedColumnDGV();
                RefreshOrderAsync();

                Thread.Sleep(2000);

                Dialog.Close();
            }
            catch (Exception E)
            {
                if (MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK)
                {
                    Environment.Exit(0);
                }
            }
        }

        private async void AddOrder_TSB_Click(object sender, EventArgs e)
        {
            LoadData_PB.Visible = true;
            LockedButtonForLoadData(false);

            await Task.Run(() => RefreshOrder());

            LoadData_PB.Visible = false;

            if (AddOrder())
            {
                DisplayAsync(SystemArgs.Orders);
            }

            LockedButtonForLoadData(true);
        }

        private void ChangeOrder_TSB_Click(object sender, EventArgs e)
        {
            if (ChangeOrder())
            {
                DisplayAsync(SystemArgs.Orders);
            }
        }
        private void DeleteOrder_TSB_Click(object sender, EventArgs e)
        {
            if (DeleteOrder())
            {
                DisplayAsync(SystemArgs.Orders);
            }
        }

        private async void AddOrder_TSM_Click(object sender, EventArgs e)
        {
            LoadData_PB.Visible = true;
            LockedButtonForLoadData(false);

            await Task.Run(() => RefreshOrder());

            LoadData_PB.Visible = false;

            if (AddOrder())
            {
                DisplayAsync(SystemArgs.Orders);
            }

            LockedButtonForLoadData(true);
        }
        private void ChangeOrder_TSM_Click(object sender, EventArgs e)
        {
            if (ChangeOrder())
            {
                DisplayAsync(SystemArgs.Orders);
            }
        }

        private void DeleteOrder_TSM_Click(object sender, EventArgs e)
        {
            if (DeleteOrder())
            {
                DisplayAsync(SystemArgs.Orders);
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
                    DisplayAsync(Result);
                }
            }
        }

        private void Reset_TSB_Click(object sender, EventArgs e)
        {
            ResetSearch();
        }

        private void AdvancedSearch_TSB_Click(object sender, EventArgs e)
        {
            if (SearchParam())
            {
                ViewSearchAsync(Result);
            }
        }
        private Boolean AddOrder()
        {
            try
            {

                Int64 IndexOrder = -1;

                KB_Scan_F Dialog = new KB_Scan_F();
                if (SystemArgs.ClientProgram.UsingWebCam)
                {
                    SystemArgs.WebcamScanOrder = new WebcamScanOrder();
                    if (!SystemArgs.WebcamScanOrder.Start())
                    {
                        throw new Exception("Ошибка подключения вебкамеры");
                    }
                }
                else
                {
                    Dialog.Web_L.Visible = false;
                    Dialog.ViewWeb_PB.Visible = false;
                    Dialog.Main_TLP.SetRow(Dialog.Position_L, 2);
                    Dialog.Main_TLP.SetRow(Dialog.Scan_DGV, 3);
                    Dialog.Main_TLP.SetRowSpan(Dialog.Scan_DGV, 8);
                    Dialog.MaximumSize = new Size(Dialog.Width, Dialog.Height - 100);
                    Dialog.Scan_DGV.Height = Dialog.Scan_DGV.Height - 100;
                    Dialog.Status_TB.Height = Dialog.Status_TB.Height - 100;
                    SystemArgs.ServerMobileAppOrder = new ServerMobileAppOrder();//Сервер мобильного приложения
                    if (!SystemArgs.ServerMobileAppOrder.Start())
                    {
                        throw new Exception("Ошибка открытия сервера для получения данных с мобильного приложения");
                    }
                }
                Dialog.ServerStatus_TB.Text = "Запущен";
                Dialog.ServerStatus_TB.BackColor = Color.FromArgb(233, 245, 255);
                Dialog.Status_TB.AppendText($"Ожидание данных" + Environment.NewLine);

                if (Dialog.ShowDialog() == DialogResult.OK)
                {
                    List<OrderScanSession> ScanSession;
                    if (SystemArgs.ClientProgram.UsingWebCam)
                    {
                        ScanSession = SystemArgs.WebcamScanOrder.GetScanSessions();
                    }
                    else
                    {
                        ScanSession = SystemArgs.ServerMobileAppOrder.GetScanSessions();
                    }
                    for (int i = 0; i < ScanSession.Count; i++)
                    {
                        try
                        {
                            if (ScanSession[i].Unique == 2)
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

                                String[] SplitDataMatrix = ScanSession[i].DataMatrix.Split('_');

                                String[] ListCanceled = SplitDataMatrix[1].Split('и');

                                if (ListCanceled.Length != 1)
                                {
                                    List<Order> CanceledOrders = SystemArgs.Orders.Where(p => (p.List.IndexOf(ListCanceled[0] + "и") == 0 || p.List == ListCanceled[0]) && p.Number == SplitDataMatrix[0]).ToList();

                                    if (CanceledOrders.Count() >= 1)
                                    {
                                        for (Int32 j = 0; j < CanceledOrders.Count; j++)
                                        {
                                            CanceledOrders[j].Canceled = true;
                                            SystemArgs.Request.CanceledOrder(CanceledOrders[j]);
                                        }
                                    }
                                }

                                BlankOrder TempBlank = new BlankOrder();

                                Int64 PositionID = SystemArgs.User.GetPosition().ID;

                                Status TempStatus = (from p in SystemArgs.Statuses
                                                     where p.IDPosition == PositionID
                                                     select p).Single();

                                Order TempOrder = new Order(IndexOrder + 1, ScanSession[i].DataMatrix, DateTime.Now, SplitDataMatrix[0], SplitDataMatrix[3], "Исполнитель не определен", SplitDataMatrix[1], SplitDataMatrix[2], Convert.ToDouble(SplitDataMatrix[4]), Convert.ToDouble(SplitDataMatrix[5]), TempStatus, DateTime.Now, SystemArgs.User, TempBlank, false, false);

                                if (SystemArgs.Request.InsertOrder(TempOrder))
                                {
                                    SystemArgs.Orders.Add(TempOrder);
                                    if (!SystemArgs.Request.StatusExist(TempOrder))
                                    {
                                        SystemArgs.Request.InsertStatus(TempOrder);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Ошибка при добавлении в базу данных DataMatrix: " + ScanSession[i].DataMatrix, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }

                            }
                            else if (ScanSession[i].Unique == 1)
                            {
                                String[] SplitDataMatrix = ScanSession[i].DataMatrix.Split('_');
                                Order OldOrder = SystemArgs.Orders.Where(p => p.Number == SplitDataMatrix[0] && p.List == SplitDataMatrix[1]).Single();
                                Order UpdateOrder = OldOrder;
                                UpdateOrder.DataMatrix = ScanSession[i].DataMatrix;
                                UpdateOrder.Mark = SplitDataMatrix[2];
                                UpdateOrder.Executor = SplitDataMatrix[3];
                                UpdateOrder.Lenght = Convert.ToDouble(SplitDataMatrix[4]);
                                UpdateOrder.Weight = Convert.ToDouble(SplitDataMatrix[5]);
                                if (SystemArgs.Request.UpdateOrder(UpdateOrder))
                                {
                                    SystemArgs.Orders.Remove(OldOrder);
                                    SystemArgs.Orders.Add(UpdateOrder);
                                }
                                else
                                {
                                    MessageBox.Show("Ошибка при обновлении в базе данных DataMatrix: " + SystemArgs.ServerMobileAppOrder[i].DataMatrix, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            }
                        }
                        catch (Exception E)
                        {
                            ScanSession[i].Discription = E.Message;
                            ScanSession[i].Unique = 0;
                        }

                    }
                    List<OrderScanSession> Temp = ScanSession.Where(p => p.Unique == 0).ToList();
                    if (Temp.Count() > 0)
                    {
                        KB_NotAdded_F Report = new KB_NotAdded_F();
                        Report.Report_DGV.AutoGenerateColumns = false;
                        Report.Report_DGV.DataSource = Temp;
                        Report.CountOrder_TB.Text = ScanSession.Count() - Temp.Count() + "/" + ScanSession.Count();
                        Report.ShowDialog();

                    }

                    return true;
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
                if (Order_DGV.CurrentCell != null && Order_DGV.CurrentCell.RowIndex >= 0 && Order_DGV.SelectedRows.Count == 1)
                {
                    Order Temp = (Order)View[Order_DGV.CurrentCell.RowIndex];
                    KB_ChangeOrder_F Dialog = new KB_ChangeOrder_F(Temp);

                    Dialog.Executor_TB.Text = Temp.Executor;
                    Dialog.Number_TB.Text = Temp.Number;
                    Dialog.List_TB.Text = Temp.List.ToString();
                    Dialog.Mark_TB.Text = Temp.Mark;
                    Dialog.Lenght_TB.Text = Temp.Lenght.ToString();
                    Dialog.Weight_TB.Text = Temp.Weight.ToString();

                    if (Dialog.ShowDialog() == DialogResult.OK)
                    {
                        String NewDataMatrix = Dialog.Number_TB.Text + "_" + Dialog.List_TB.Text + "_" + Dialog.Mark_TB.Text + "_" + Dialog.Executor_TB.Text + "_" + Dialog.Lenght_TB.Text + "_" + Dialog.Weight_TB.Text;
                        List<DateTime> StatusDate = SystemArgs.StatusOfOrders.Where(p => p.IDOrder == Temp.ID && p.IDStatus == Temp.Status.ID).Select(p=>p.DateCreate).ToList();
                        Order NewOrder = new Order(Temp.ID, NewDataMatrix, Temp.DateCreate, Dialog.Number_TB.Text, Dialog.Executor_TB.Text,Temp.ExecutorWork, Dialog.List_TB.Text, Dialog.Mark_TB.Text, Convert.ToDouble(Dialog.Lenght_TB.Text), Convert.ToDouble(Dialog.Weight_TB.Text), Temp.Status,StatusDate[0], Temp.User, Temp.BlankOrder, Temp.Canceled,Temp.Finished);
                        
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
                    throw new Exception("Необходимо выбрать один объект");
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
                if (Order_DGV.CurrentCell.RowIndex >= 0 && Order_DGV.SelectedRows.Count == 1)
                {
                    Order Temp = (Order)View[Order_DGV.CurrentCell.RowIndex];

                    if (MessageBox.Show("Вы действительно хотите удалить чертеж?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
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
                    throw new Exception("Необходимо выбрать один объект");
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
            try
            {
                Int32 Index = 0;

                Order_DGV.Invoke((MethodInvoker)delegate ()
                {
                    Index = FilterCB_TSB.SelectedIndex;
                });

                if (Index >= 0)
                {
                    switch (Index)
                    {
                        case 1:
                            Order_DGV.Invoke((MethodInvoker)delegate ()
                            {
                                View.DataSource = null;
                                View.DataSource = List.Where(p => !p.Canceled&&!p.Finished).ToList();

                                Order_DGV.DataSource = View;

                                VisibleButton(false);
                                if (View.Count > 0)
                                {
                                    CountOrder_TB.Text = View.Count.ToString();
                                }
                                else
                                {
                                    CountOrder_TB.Text = "0";
                                    SelectedOrder_TB.Text = "0";
                                }
                            });
                            break;
                        case 2:
                            Order_DGV.Invoke((MethodInvoker)delegate ()
                            {
                                View.DataSource = null;
                                View.DataSource = List.Where(p => p.Canceled).ToList();

                                Order_DGV.DataSource = View;

                                VisibleButton(false);
                                if (View.Count > 0)
                                {
                                    CountOrder_TB.Text = View.Count.ToString();
                                    CanceledOrder_TSB.Text = "Восстановить";
                                    CanceledOrder_TSB.Visible = true;
                                }
                                else
                                {
                                    CountOrder_TB.Text = "0";
                                    SelectedOrder_TB.Text = "0";
                                }
                            });

                            break;
                        case 3:
                            Order_DGV.Invoke((MethodInvoker)delegate ()
                            {
                                View.DataSource = null;
                                View.DataSource = List.Where(p => p.Finished).ToList();

                                Order_DGV.DataSource = View;

                                VisibleButton(false);
                                if (View.Count > 0)
                                {
                                    CountOrder_TB.Text = View.Count.ToString();
                                }
                                else
                                {
                                    CountOrder_TB.Text = "0";
                                    SelectedOrder_TB.Text = "0";
                                }
                            });

                            break;
                        default:
                            Order_DGV.Invoke((MethodInvoker)delegate ()
                            {
                                View.DataSource = null;
                                View.DataSource = List.Where(p => p.Status.IDPosition == SystemArgs.User.GetPosition().ID && !p.Canceled&&!p.Finished).ToList();

                                Order_DGV.DataSource = View;

                                if (View.Count > 0)
                                {
                                    CountOrder_TB.Text = View.Count.ToString();
                                    VisibleButton(true);
                                    CanceledOrder_TSB.Text = "Аннулировать";
                                }
                                else
                                {
                                    CountOrder_TB.Text = "0";
                                    SelectedOrder_TB.Text = "0";
                                    VisibleButton(false);
                                    CanceledOrder_TSB.Text = "Аннулировать";
                                }

                                ForgetOrder();
                            });
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                SystemArgs.PrintLog(e.ToString());
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ForgetOrder()
        {
            for (int i = 0; i < Order_DGV.RowCount; i++)
            {
                if ((DateTime.Now - Convert.ToDateTime(Order_DGV["StatusDate", i].Value)).TotalDays >= SystemArgs.ClientProgram.VisualRow_N2)
                {
                    Order_DGV.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(236, 0, 6);
                }
                else if ((DateTime.Now - Convert.ToDateTime(Order_DGV["StatusDate", i].Value)).TotalDays >= SystemArgs.ClientProgram.VisualRow_N1)
                {
                    Order_DGV.Rows[i].DefaultCellStyle.BackColor = Color.Orange;
                }
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
            for (int i = 0; i < SystemArgs.SelectedColumn.GetColumns().Count; i++)
            {
                if (Order_DGV.Columns[i].Visible)
                {
                    SystemArgs.SelectedColumn[i].DisplayIndex = Order_DGV.Columns[SystemArgs.SelectedColumn[i].Name].DisplayIndex;
                    SystemArgs.SelectedColumn[i].FillWeight = Order_DGV.Columns[SystemArgs.SelectedColumn[i].Name].FillWeight;
                }
            }
            SystemArgs.SelectedColumn.SetParametrColumnDisplayIndex();
            SystemArgs.SelectedColumn.SetParametrColumnFillWeight();
            Application.Exit();
        }

        private void Order_DGV_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            e.CellStyle.SelectionBackColor = Color.FromArgb(112, 238, 226);
            e.CellStyle.SelectionForeColor = Color.Black;
        }

        private void FilterCB_TSB_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResetSearch();
        }

        private void ItemsFilter()
        {
            FilterCB_TSB.Items.Add("Текущий статус");
            FilterCB_TSB.Items.Add("Все статусы");
            FilterCB_TSB.Items.Add("Аннулированные");
            FilterCB_TSB.Items.Add("Завершенные");
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
                Result.Clear();
            }

            Search_TSTB.Text = String.Empty;

            DisplayAsync(SystemArgs.Orders);
        }

        private bool SearchParam()
        {
            try
            {
                Chief_PDO_SearchParam_F Dialog = new Chief_PDO_SearchParam_F();

                List<Status> Statuses = new List<Status>
                {
                    new Status(-1, 0, "Не задан")
                };
                Statuses.AddRange(SystemArgs.Statuses);
                Dialog.Status_CB.DataSource = Statuses;

                if (Dialog.ShowDialog() == DialogResult.OK)
                {
                    Result = SystemArgs.Orders.ToList();
                    if (Dialog.Finished_CB.Checked && Dialog.Number_TB.Text.Trim() == String.Empty && Dialog.List_TB.Text.Trim() == String.Empty)
                    {
                        if (MessageBox.Show("Вы уверены в выводе всех завершенных чертежей?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK)
                        {
                            Result = Result.Where(p => !p.Finished).ToList();
                        }
                    }
                    else if (!Dialog.Finished_CB.Checked)
                    {
                        Result = Result.Where(p => !p.Finished).ToList();
                    }
                    if (Dialog.DateEnable_CB.Checked && Dialog.Status_CB.SelectedIndex != 0)
                    {
                        Status Status = (Status)Dialog.Status_CB.SelectedItem;
                        var Orders = SystemArgs.StatusOfOrders.Where(p => p.DateCreate >= Dialog.First_DP.Value.Date && p.DateCreate <= Dialog.Second_DP.Value.Date.AddSeconds(86399) && p.IDStatus == Status.ID);
                        List<Order> Temp = new List<Order>();
                        foreach (var item in Orders)
                        {
                            List<Order> Order = Result.Where(p => p.ID == item.IDOrder).ToList();
                            if (Order.Count > 0)
                            {
                                Temp.Add(new Order(Order[0].ID, Order[0].DataMatrix, Order[0].DateCreate, Order[0].Number, Order[0].Executor, Order[0].ExecutorWork, Order[0].List, Order[0].Mark, Order[0].Lenght, Order[0].Weight, Order[0].Status, Order[0].StatusDate, Order[0].User, Order[0].BlankOrder, Order[0].Canceled, Order[0].Finished));
                            }
                        }
                        Result = Temp;
                    }
                    else if (Dialog.DateEnable_CB.Checked)
                    {
                        Result = Result.Where(p => (p.DateCreate >= Dialog.First_DP.Value.Date) && (p.DateCreate <= Dialog.Second_DP.Value.Date.AddSeconds(86399))).ToList();
                    }
                    else if (Dialog.Status_CB.SelectedIndex > 0)
                    {
                        Result = Result.Where(p => p.Status == (Status)Dialog.Status_CB.SelectedItem).ToList();
                    }

                    if (Dialog.Executor_TB.Text.Trim() != String.Empty)
                    {
                        Result = Result.Where(p => p.Executor.IndexOf(Dialog.Executor_TB.Text.Trim()) != -1).ToList();
                    }

                    if (Dialog.ExecutorWork_TB.Text.Trim() != String.Empty)
                    {
                        Result = Result.Where(p => p.ExecutorWork.IndexOf(Dialog.ExecutorWork_TB.Text.Trim()) != -1).ToList();
                    }

                    if (Dialog.Number_TB.Text.Trim() != String.Empty)
                    {
                        Result = Result.Where(p => p.Number.IndexOf(Dialog.Number_TB.Text.Trim()) != -1).ToList();
                    }

                    if (Dialog.List_TB.Text.Trim() != String.Empty)
                    {
                        Result = Result.Where(p => p.List.IndexOf(Dialog.List_TB.Text.Trim()) != -1).ToList();
                    }

                    if (Dialog.Mark_TB.Text.Trim() != String.Empty)
                    {
                        Result = Result.Where(p => p.Mark.IndexOf(Dialog.Mark_TB.Text.Trim()) != -1).ToList();
                    }

                    if (Dialog.Lenght_TB.Text.Trim() != String.Empty)
                    {
                        Result = Result.Where(p => p.Lenght.ToString().IndexOf(Dialog.Lenght_TB.Text.Trim()) != -1).ToList();
                    }

                    if (Dialog.Weight_TB.Text.Trim() != String.Empty)
                    {
                        Result = Result.Where(p => p.Weight.ToString().IndexOf(Dialog.Weight_TB.Text.Trim()) != -1).ToList();
                    }

                    if (Dialog.NumberBlankOrder_TB.Text.Trim() != String.Empty)
                    {
                        Result = Result.Where(p => p.BlankOrderView.IndexOf(Dialog.NumberBlankOrder_TB.Text.Trim()) != -1).ToList();
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
                KB_ReportOrderOfDate_F Dialog = new KB_ReportOrderOfDate_F();
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
        private void VisibleButton(Boolean Enable)
        {
            if (Enable)
            {
                ChangeOrder_TSB.Visible = true;
                DeleteOrder_TSB.Visible = true;
                ChangeOrder_TSM.Visible = true;
                DeleteOrder_TSM.Visible = true;
                CanceledOrder_TSB.Visible = true;
                Checked_TSM.Visible = true;
                SelectionReport_TSM.Enabled = true;

            }
            else
            {
                ChangeOrder_TSB.Visible = false;
                DeleteOrder_TSB.Visible = false;
                ChangeOrder_TSM.Visible = false;
                DeleteOrder_TSM.Visible = false;
                CanceledOrder_TSB.Visible = false;
                Checked_TSM.Visible = false;
                SelectionReport_TSM.Enabled = false;
            }
        }
        private void Selection(Order Temp, bool flag)
        {
            if (flag)
            {
                DateCreate_TB.Text = Temp.DateCreate.ToString();
                Executor_TB.Text = Temp.Executor;
                ExecutorWork_TB.Text = Temp.ExecutorWork;
                Number_TB.Text = Temp.Number;
                List_TB.Text = Temp.List.ToString();
                Mark_TB.Text = Temp.Mark;
                Lenght_TB.Text = Temp.Lenght.ToString();
                Weight_TB.Text = Temp.Weight.ToString();
                if (Temp.Canceled)
                {
                    Canceled_TB.BackColor = Color.Orange;
                    Canceled_TB.Text = "Да";
                }
                else
                {
                    Canceled_TB.BackColor = Color.Lime;
                    Canceled_TB.Text = "Нет";
                }
                if (Temp.Finished)
                {
                    Finished_TB.BackColor = Color.Orange;
                    Finished_TB.Text = "Да";
                }
                else
                {
                    Finished_TB.BackColor = Color.Lime;
                    Finished_TB.Text = "Нет";
                }
                BlankOrder_TB.Text = Temp.BlankOrder.QR;
                Status_TB.Text = Temp.Status.Name;
                SelectedOrder_TB.Text = Order_DGV.SelectedRows.Count.ToString();

            }
            else
            {
                DateCreate_TB.Text = String.Empty;
                Executor_TB.Text = String.Empty;
                ExecutorWork_TB.Text = String.Empty;
                Number_TB.Text = String.Empty;
                List_TB.Text = String.Empty;
                Mark_TB.Text = String.Empty;
                Lenght_TB.Text = String.Empty;
                Weight_TB.Text = String.Empty;
                Canceled_TB.BackColor = Color.FromArgb(233, 245, 255);
                Canceled_TB.Text = String.Empty;
                Finished_TB.BackColor = Color.FromArgb(233, 245, 255);
                Finished_TB.Text = String.Empty;
                BlankOrder_TB.Text = String.Empty;
                Status_TB.Text = String.Empty;
                SelectedOrder_TB.Text = "0";
            }

        }

        private async void RefreshOrderAsync()
        {
            LoadData_PB.Visible = true;
            LockedButtonForLoadData(false);

            await Task.Run(() => RefreshOrder());

            FilterCB_TSB.SelectedIndex = 0;

            LockedButtonForLoadData(true);

            LoadData_PB.Visible = false;
        }
        private async void DisplayAsync(List<Order> Orders)
        {
            LoadData_PB.Visible = true;
            LockedButtonForLoadData(false);

            await Task.Run(() => Display(Orders));

            LockedButtonForLoadData(true);

            LoadData_PB.Visible = false;
        }

        private async void ViewSearchAsync(List<Order> Orders)
        {
            LoadData_PB.Visible = true;
            LockedButtonForLoadData(false);

            await Task.Run(() => ViewSearch(Orders));

            LockedButtonForLoadData(true);

            LoadData_PB.Visible = false;
        }
        private void ViewSearch(List<Order> Orders)
        {
            try
            {
                Order_DGV.Invoke((MethodInvoker)delegate ()
                {
                    View.DataSource = null;
                    View.DataSource = Orders;

                    Order_DGV.DataSource = View;

                    VisibleButton(true);
                    CountOrder_TB.Text = View.Count.ToString();
                    CanceledOrder_TSB.Text = "Аннулировать";
                });
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LockedButtonForLoadData(bool flag)
        {
            AddOrder_TSB.Enabled = flag;
            AddOrder_TSM.Enabled = flag;
            ChangeOrder_TSB.Enabled = flag;
            ChangeOrder_TSM.Enabled = flag;
            DeleteOrder_TSB.Enabled = flag;
            DeleteOrder_TSM.Enabled = flag;
            Search_TSB.Enabled = flag;
            Reset_TSB.Enabled = flag;
            AdvancedSearch_TSB.Enabled = flag;
            FilterCB_TSB.Enabled = flag;
            RefreshStatus_B.Enabled = flag;
            CanceledOrder_TSB.Enabled = flag;
            ReportDate_TSM.Enabled = flag;
            SelectionReport_TSM.Enabled = flag;
            Time_Day_Report_TSM.Enabled = flag;
            Time_Week_Report_TSM.Enabled = flag;
            Time_Month_Report_TSM.Enabled = flag;
            Time_SelectionDate_Report_TSM.Enabled = flag;
        }

        private void RefreshOrder()
        {
            try
            {
                SystemArgs.Orders.Clear();
                SystemArgs.BlankOrders.Clear();
                SystemArgs.StatusOfOrders.Clear();
                SystemArgs.BlankOrderOfOrders.Clear();

                SystemArgs.Request.GetAllBlankOrder();
                SystemArgs.Request.GetAllOrders();

            }
            catch (Exception E)
            {
                SystemArgs.PrintLog(E.ToString());

                MessageBox.Show("Ошибка получения данных для обновления информации", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

                Environment.Exit(0);
            }
        }

        private void RefreshStatus_B_Click(object sender, EventArgs e)
        {

        }

        private void Order_DGV_Sorted(object sender, EventArgs e)
        {
            try
            {
                if (FilterCB_TSB.SelectedIndex == 0)
                {
                    ForgetOrder();
                }
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SettingMobile_TSM_Click(object sender, EventArgs e)
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
                    Dialog.QR_PB.Image = QrCode.Draw($"{MyIP}_{SystemArgs.MobileApplication.Port}", 100);
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

        private void SelectionReport_TSM_Click(object sender, EventArgs e)
        {
            try
            {
                List<Order> Report = new List<Order>();
                if (Order_DGV.CurrentCell != null && Order_DGV.CurrentCell.RowIndex > 0)
                {
                    for (int i = 0; i < Order_DGV.SelectedRows.Count; i++)
                    {
                        Report.Add((Order)(View[Order_DGV.SelectedRows[i].Index]));
                    }
                    if (SystemArgs.Excel.ReportOrderOfSelect(Report))
                    {
                        MessageBox.Show("Отчет успешно сформирован", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Ошибка формирования отчета", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    throw new Exception("Необходимо выбрать чертежи");
                }
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CheckedUnloading_TSM_Click(object sender, EventArgs e)
        {
            try
            {
                List<Order> Selection = new List<Order>();
                if (Order_DGV.CurrentCell.RowIndex >= 0)
                {
                    for (int i = 0; i < Order_DGV.SelectedRows.Count; i++)
                    {
                        Selection.Add((Order)(View[Order_DGV.SelectedRows[i].Index]));
                    }
                    if (SystemArgs.UnLoadSpecific.SearchFileUnloading(Selection.Select(p=>p.DataMatrix).ToList()))
                    {
                        if (SystemArgs.UnLoadSpecific.ExecutorMails.Count != 0)
                        {
                            KB_ScanUnloadSpecific Dialog = new KB_ScanUnloadSpecific();
                            Dialog.ShowDialog();
                        }
                        else
                        {
                            MessageBox.Show("При проверки выгрузки не было найдено ни одного совпадения номера заказа с листом", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                else
                {
                    throw new Exception("Необходимо выбрать объекты");
                }
            }
            catch (Exception E)
            {
                MessageBox.Show("Файл был указан не верно или не хватило прав доступа к файлу", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                SystemArgs.PrintLog(E.ToString());
            }
        }

        private void CanceledOrder_TSB_Click(object sender, EventArgs e)
        {
            try
            {
                if (Order_DGV.CurrentCell.RowIndex >= 0 && Order_DGV.SelectedRows.Count == 1)
                {
                    if (MessageBox.Show("Изменить статус аннулирования чертежа?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                    {
                        Order Temp = (Order)View[Order_DGV.CurrentCell.RowIndex];

                        Temp.Canceled = !Temp.Canceled;

                        if (SystemArgs.Request.CanceledOrder(Temp))
                        {
                            DisplayAsync(SystemArgs.Orders);
                        }
                        else
                        {
                            throw new Exception("Произошла ошибка при аннулировании чертежа");
                        }
                    }
                }
                else
                {
                    throw new Exception("Необходимо выбрать один объект");
                }
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Time_Day_Report_TSM_Click(object sender, EventArgs e)
        {
            ReportTimeofOrderPeriod(new TimeSpan(1, 0, 0, 1));
        }

        private void Time_Week_Report_TSM_Click(object sender, EventArgs e)
        {
            ReportTimeofOrderPeriod(new TimeSpan(7, 0, 0, 1));
        }

        private void Time_Month_Report_TSM_Click(object sender, EventArgs e)
        {
            ReportTimeofOrderPeriod(new TimeSpan(30, 0, 0, 1));
        }

        private void Time_SelectionDate_Report_TSM_Click(object sender, EventArgs e)
        {
            ReportTimeofOrder();
        }

        private void ReportTimeofOrderPeriod(object aInterval)
        {
            try
            {
                SaveFileDialog SaveReport = new SaveFileDialog();
                String date = DateTime.Now.ToString();
                date = date.Replace(".", "_");
                date = date.Replace(":", "_");
                SaveReport.FileName = "Отчет по времени за выбранный период от " + date;
                SaveReport.Filter = "Excel Files .xlsx|*.xlsx";
                if (SaveReport.ShowDialog() == DialogResult.OK)
                {
                    ALL_FormingReportForAllPosition_F FormingF = new ALL_FormingReportForAllPosition_F();
                    FormingF.Show();
                    List<StatusOfOrder> Report = SystemArgs.StatusOfOrders.Where(p => p.DateCreate <= DateTime.Now && p.DateCreate >= DateTime.Now.Subtract((TimeSpan)aInterval)).ToList();
                    Task<Boolean> task = ReportPastTimeAsync(Report, SaveReport.FileName);
                    task.ContinueWith(t =>
                    {
                        if (t.Result)
                        {
                            FormingF.Invoke((MethodInvoker)delegate ()
                            {
                                FormingF.Close();
                            });
                            MessageBox.Show("Отчет сформирован успешно", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            FormingF.Invoke((MethodInvoker)delegate ()
                            {
                                FormingF.Close();
                            });
                            MessageBox.Show("Ошибка фомирования отчета", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    });
                }
            }
            catch (Exception E)
            {
                SystemArgs.PrintLog(E.ToString());
                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ReportTimeofOrder()
        {
            try
            {
                AR_ReportOrderOfDate_F Dialog = new AR_ReportOrderOfDate_F();
                if (Dialog.ShowDialog() == DialogResult.OK)
                {
                    SaveFileDialog SaveReport = new SaveFileDialog();
                    String date = DateTime.Now.ToString();
                    date = date.Replace(".", "_");
                    date = date.Replace(":", "_");
                    SaveReport.FileName = "Отчет по времени за выбранный период от " + date;
                    SaveReport.Filter = "Excel Files .xlsx|*.xlsx";
                    if (SaveReport.ShowDialog() == DialogResult.OK)
                    {
                        ALL_FormingReportForAllPosition_F FormingF = new ALL_FormingReportForAllPosition_F();
                        FormingF.Show();
                        List<StatusOfOrder> Report = SystemArgs.StatusOfOrders.Where(p => p.DateCreate >= Dialog.First_MC.SelectionStart && p.DateCreate <= Dialog.Second_MC.SelectionStart).ToList();
                        Task<Boolean> task = ReportPastTimeAsync(Report, SaveReport.FileName);
                        task.ContinueWith(t =>
                        {
                            if (t.Result)
                            {
                                FormingF.Invoke((MethodInvoker)delegate ()
                                {
                                    FormingF.Close();
                                });
                                MessageBox.Show("Отчет сформирован успешно", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                FormingF.Invoke((MethodInvoker)delegate ()
                                {
                                    FormingF.Close();
                                });
                                MessageBox.Show("Ошибка фомирования отчета", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        });
                    }
                }
            }
            catch (Exception E)
            {
                SystemArgs.PrintLog(E.ToString());
                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async Task<Boolean> ReportPastTimeAsync(List<StatusOfOrder> Report, String filename)
        {
            return await Task.Run(() => SystemArgs.Excel.ReportPastTimeofDate(Report, filename));
        }

        private void AboutProgram_TSM_Click(object sender, EventArgs e)
        {
            ALL_AboutProgram_F Dialog = new ALL_AboutProgram_F();
            if (Dialog.ShowDialog() == DialogResult.OK)
            {

            }
        }
        public static void SetDoubleBuffered(Control control)
        {
            // set instance non-public property with name "DoubleBuffered" to true
            typeof(Control).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, control, new object[] { true });
        }
        private void SelectedColumnDGV()
        {
            try
            {
                List<Column> Temp = SystemArgs.SelectedColumn.GetColumns().OrderBy(p => p.DisplayIndex).ToList();
                for (int i = 0; i < Temp.Count(); i++)
                {
                    Order_DGV.Columns[Temp[i].Name].DisplayIndex = Temp[i].DisplayIndex;
                    Order_DGV.Columns[Temp[i].Name].Visible = Temp[i].Visible;
                    Order_DGV.Columns[Temp[i].Name].FillWeight = Temp[i].FillWeight;
                }
            }
            catch (Exception E)
            {
                SystemArgs.PrintLog(E.ToString());
                throw new Exception(E.Message);
            }
        }

        private void SelectedColumn_TSM_Click(object sender, EventArgs e)
        {
            try
            {
                AR_SelectedColumnDGV_F Dialog = new AR_SelectedColumnDGV_F();

                Dialog.DataMatrix_CB.Checked = SystemArgs.SelectedColumn[0].Visible;
                Dialog.DateCreate_CB.Checked = SystemArgs.SelectedColumn[1].Visible;
                Dialog.Number_CB.Checked = SystemArgs.SelectedColumn[2].Visible;
                Dialog.Executor_CB.Checked = SystemArgs.SelectedColumn[3].Visible;
                Dialog.ExecutorWork_CB.Checked = SystemArgs.SelectedColumn[4].Visible;
                Dialog.List_CB.Checked = SystemArgs.SelectedColumn[5].Visible;
                Dialog.Mark_CB.Checked = SystemArgs.SelectedColumn[6].Visible;
                Dialog.Lenght_CB.Checked = SystemArgs.SelectedColumn[7].Visible;
                Dialog.Height_CB.Checked = SystemArgs.SelectedColumn[8].Visible;
                Dialog.Status_CB.Checked = SystemArgs.SelectedColumn[9].Visible;
                Dialog.User_CB.Checked = SystemArgs.SelectedColumn[10].Visible;
                Dialog.BlankOrder_CB.Checked = SystemArgs.SelectedColumn[11].Visible;
                Dialog.Cancelled_CB.Checked = SystemArgs.SelectedColumn[12].Visible;
                Dialog.StatusDate_CB.Checked = SystemArgs.SelectedColumn[13].Visible;
                Dialog.Finished_CB.Checked = SystemArgs.SelectedColumn[14].Visible;

                if (Dialog.ShowDialog() == DialogResult.OK)
                {
                    SystemArgs.SelectedColumn[0].Visible = Dialog.DataMatrix_CB.Checked;
                    SystemArgs.SelectedColumn[1].Visible = Dialog.DateCreate_CB.Checked;
                    SystemArgs.SelectedColumn[2].Visible = Dialog.Number_CB.Checked;
                    SystemArgs.SelectedColumn[3].Visible = Dialog.Executor_CB.Checked;
                    SystemArgs.SelectedColumn[4].Visible = Dialog.ExecutorWork_CB.Checked;
                    SystemArgs.SelectedColumn[5].Visible = Dialog.List_CB.Checked;
                    SystemArgs.SelectedColumn[6].Visible = Dialog.Mark_CB.Checked;
                    SystemArgs.SelectedColumn[7].Visible = Dialog.Lenght_CB.Checked;
                    SystemArgs.SelectedColumn[8].Visible = Dialog.Height_CB.Checked;
                    SystemArgs.SelectedColumn[9].Visible = Dialog.Status_CB.Checked;
                    SystemArgs.SelectedColumn[10].Visible = Dialog.User_CB.Checked;
                    SystemArgs.SelectedColumn[11].Visible = Dialog.BlankOrder_CB.Checked;
                    SystemArgs.SelectedColumn[12].Visible = Dialog.Cancelled_CB.Checked;
                    SystemArgs.SelectedColumn[13].Visible = Dialog.StatusDate_CB.Checked;
                    SystemArgs.SelectedColumn[14].Visible = Dialog.Finished_CB.Checked;
                    SystemArgs.SelectedColumn.SetParametrColumnVisible();
                    MessageBox.Show("Настройки успешно сохранены", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    SelectedColumnDGV();
                }
            }
            catch (Exception E)
            {
                SystemArgs.PrintLog(E.ToString());
                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SettingWebcam_TSM_Click(object sender, EventArgs e)
        {
            try
            {
                KB_SettingWebcam_F Dialog = new KB_SettingWebcam_F();
                FilterInfoCollection videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                if (videoDevices.Count > 0)
                {
                    foreach (FilterInfo device in videoDevices)
                    {
                        Dialog.WB_LB.Items.Add(device.Name);
                    }
                    Dialog.WB_LB.SelectedIndex = 0;
                }
                if (Dialog.ShowDialog() == DialogResult.OK)
                {
                    XDocument doc = XDocument.Load(SystemArgs.Path.WebCamDevice);
                    doc.Element("Device").SetValue(videoDevices[Dialog.WB_LB.SelectedIndex].MonikerString);
                    doc.Save(SystemArgs.Path.WebCamDevice);
                    MessageBox.Show("Настройки успешно сохранены", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch(Exception E)
            {
                SystemArgs.PrintLog(E.ToString());
                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SettingConfig_TSM_Click(object sender, EventArgs e)
        {
            try
            {
                KB_SettingConfig_F Dialog = new KB_SettingConfig_F();

                if (SystemArgs.ClientProgram.GetParametersConnect())
                {
                    Dialog.ModelsPath_TB.Text = SystemArgs.ClientProgram.ModelsPath;
                    if (SystemArgs.ClientProgram.UsingWebCam)
                    {
                        Dialog.Web_RB.Checked = true;
                    }
                    else
                    {
                        Dialog.Mobile_RB.Checked = true;
                    }
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

        private void Order_DGV_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (Order_DGV.CurrentCell != null && Order_DGV.CurrentCell.RowIndex < View.Count())
                {
                    Order Temp = (Order)View[Order_DGV.CurrentCell.RowIndex];
                    KB_DetailedInformationOrder_F Dialog = new KB_DetailedInformationOrder_F();
                    List<StatusOfOrder> Statuses = SystemArgs.StatusOfOrders.Where(p => p.IDOrder == Temp.ID).OrderBy(p => p.DateCreate).ToList();
                    for (int i = 0; i < Statuses.Count; i++)
                    {
                        Dialog.Statuses_DGV.Rows.Add();
                        Dialog.Statuses_DGV[0, i].Value = SystemArgs.Statuses.Where(p => p.ID == Statuses[i].IDStatus).Select(p => p.Name).Single();
                        Dialog.Statuses_DGV[1, i].Value = Statuses[i].DateCreate;
                        User TempUser = SystemArgs.Users.Where(p => p.ID == Statuses[i].IDUser).Single();
                        Dialog.Statuses_DGV[2, i].Value = TempUser.Surname + " " + TempUser.Name.First() + "." + TempUser.MiddleName.First() + ".";
                    }
                    if (Dialog.ShowDialog() == DialogResult.OK)
                    {

                    }
                }
                else
                {
                    MessageBox.Show("Ошибка получения информации о чертеже", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch(Exception E)
            {
                SystemArgs.PrintLog(E.ToString());
                MessageBox.Show(E.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
