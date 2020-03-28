using Equin.ApplicationFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SZMK
{
    public partial class Chief_PDO_F : Form
    {
        public Chief_PDO_F()
        {
            InitializeComponent();
        }

        BindingListView<Order> View;

        private void Chief_PDO_F_Load(object sender, EventArgs e)
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

        private void ReportDate_TSM_Click(object sender, EventArgs e)
        {
            if (ReportOrderOfDate())
            {
                MessageBox.Show("Отчет успешно сформирован и сохранен", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void SelectionReport_TSM_Click(object sender, EventArgs e)
        {
            try
            {
                List<Order> Report = new List<Order>();
                if (Order_DGV.CurrentCell.RowIndex > 0)
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
                    throw new Exception("Необходимо выбрать объекты");
                }
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshStatus_B_Click(object sender, EventArgs e)
        {
            try
            {
                RefreshOrderAsync();
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                DisplayAsync(Result);
            }
        }

        private bool ChangeOrder()
        {
            try
            {
                if (Order_DGV.CurrentCell.RowIndex >= 0 && Order_DGV.SelectedRows.Count == 1)
                {
                    Order Temp = (Order)View[Order_DGV.CurrentCell.RowIndex];
                    Chief_PDO_ChangeOrder_F Dialog = new Chief_PDO_ChangeOrder_F(Temp);

                    Dialog.Executor_TB.Text = Temp.Executor;
                    Dialog.Number_TB.Text = Temp.Number;
                    Dialog.List_TB.Text = Temp.List.ToString();
                    Dialog.Mark_TB.Text = Temp.Mark;
                    Dialog.Lenght_TB.Text = Temp.Lenght.ToString();
                    List<Status> TempStatuses = new List<Status>
                    {
                        Temp.Status
                    };
                    Dialog.Weight_TB.Text = Temp.Weight.ToString();

                    TempStatuses.AddRange(SystemArgs.Statuses);

                    Dialog.Status_CB.DataSource = TempStatuses;

                    if (Dialog.ShowDialog() == DialogResult.OK)
                    {
                        String NewDataMatrix = Dialog.Number_TB.Text + "_" + Dialog.List_TB.Text + "_" + Dialog.Mark_TB.Text + "_" + Dialog.Executor_TB.Text + "_" + Dialog.Lenght_TB.Text + "_" + Dialog.Weight_TB.Text;
                        Order NewOrder;
                        if (Temp.Status!= SystemArgs.Statuses.Where(p => p == (Status)Dialog.Status_CB.SelectedItem).Single())
                        {
                            NewOrder = new Order(Temp.ID, NewDataMatrix, Temp.DateCreate, Dialog.Number_TB.Text, Dialog.Executor_TB.Text,Temp.ExecutorWork, Dialog.List_TB.Text, Dialog.Mark_TB.Text, Convert.ToDouble(Dialog.Lenght_TB.Text), Convert.ToDouble(Dialog.Weight_TB.Text), SystemArgs.Statuses.Where(p => p == (Status)Dialog.Status_CB.SelectedItem).Single(), DateTime.Now, SystemArgs.User, Temp.BlankOrder, Temp.Canceled);
                        }
                        else
                        {
                            List<DateTime> StatusDate = SystemArgs.StatusOfOrders.Where(p => p.IDOrder == Temp.ID && p.IDStatus == SystemArgs.Statuses.Where(j => j == (Status)Dialog.Status_CB.SelectedItem).Single().ID).Select(p => p.DateCreate).ToList();
                            NewOrder = new Order(Temp.ID, NewDataMatrix, Temp.DateCreate, Dialog.Number_TB.Text, Dialog.Executor_TB.Text,Temp.ExecutorWork, Dialog.List_TB.Text, Dialog.Mark_TB.Text, Convert.ToDouble(Dialog.Lenght_TB.Text), Convert.ToDouble(Dialog.Weight_TB.Text), SystemArgs.Statuses.Where(p => p == (Status)Dialog.Status_CB.SelectedItem).Single(), StatusDate[0], Temp.User, Temp.BlankOrder, Temp.Canceled);
                        }

                        if (SystemArgs.Request.UpdateOrder(NewOrder))
                        {
                            if (Temp.Status.ID > SystemArgs.Statuses.Where(p => p == (Status)Dialog.Status_CB.SelectedItem).Single().ID)
                            {
                                SystemArgs.Request.DownGradeStatus(NewOrder);
                            }
                            else
                            {
                                int NewStatus = (int)NewOrder.Status.ID;
                                for(int i= (int)Temp.Status.ID + 1;i <= NewStatus; i++)
                                {
                                    NewOrder.Status = SystemArgs.Statuses.Where(p => p.ID == i).Single();
                                    if (!SystemArgs.Request.StatusExist(NewOrder))
                                    {
                                        SystemArgs.Request.InsertStatus(NewOrder);
                                    }
                                }
                            }
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
                BlankOrder_TB.Text = Temp.BlankOrder.QR;
                Status_TB.Text = Temp.Status.Name;
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
                BlankOrder_TB.Text = String.Empty;
                Status_TB.Text = String.Empty;
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
        private void LockedButtonForLoadData(bool flag)
        {
            ChangeOrder_TSB.Enabled = flag;
            DeleteOrder_TSB.Enabled = flag;
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

                DisplayAsync(SystemArgs.Orders);
            }
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
                    Result = SystemArgs.Orders;
                    if (Dialog.DateEnable_CB.Checked)
                    {
                        Result = Result.Where(p => (p.DateCreate >= Dialog.First_DP.Value.Date) && (p.DateCreate <= Dialog.Second_DP.Value.Date.AddSeconds(86399))).ToList();
                    }

                    if (Dialog.Executor_TB.Text.Trim() != String.Empty)
                    {
                        Result = Result.Where(p => p.Executor.IndexOf(Dialog.ExecutorWork_TB.Text.Trim()) != -1).ToList();
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
                AR_ReportOrderOfDate_F Dialog = new AR_ReportOrderOfDate_F();
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
        private void ItemsFilter()
        {
            FilterCB_TSB.Items.Add("Все статусы");
            FilterCB_TSB.Items.Add("Аннулированные");
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
                        case 0:
                            Order_DGV.Invoke((MethodInvoker)delegate ()
                            {
                                View.DataSource = List.Where(p => !p.Canceled).ToList();

                                Order_DGV.DataSource = View;

                                CountOrder_TB.Text = View.Count.ToString();

                                if (View.Count > 0)
                                {
                                    VisibleButton(true);
                                    CanceledOrder_TSB.Text = "Аннулировать";
                                }
                                else
                                {
                                    VisibleButton(false);
                                    CanceledOrder_TSB.Text = "Аннулировать";
                                }
                            });
                            break;
                        case 1:
                            Order_DGV.Invoke((MethodInvoker)delegate ()
                            {
                                View.DataSource = List.Where(p => p.Canceled).ToList();

                                Order_DGV.DataSource = View;

                                CountOrder_TB.Text = View.Count.ToString();

                                if (View.Count > 0)
                                {
                                    CanceledOrder_TSB.Text = "Восстановить";
                                    CanceledOrder_TSB.Visible = true;
                                }
                            });

                            break;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void VisibleButton(Boolean Enable)
        {
            if (Enable)
            {
                ChangeOrder_TSB.Visible = true;
                DeleteOrder_TSB.Visible = true;
                CanceledOrder_TSB.Visible = true;
                Report_TSM.Visible = true;
            }
            else
            {
                ChangeOrder_TSB.Visible = false;
                DeleteOrder_TSB.Visible = false;
                CanceledOrder_TSB.Visible = false;
                Report_TSM.Visible = false;
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

        private void Chief_PDO_F_FormClosing(object sender, FormClosingEventArgs e)
        {
            for (int i = 0; i < Order_DGV.Columns.Count; i++)
            {
                SystemArgs.SelectedColumn.GetDisplayIndex()[i] = Order_DGV.Columns[i].DisplayIndex;
                SystemArgs.SelectedColumn.GetFillWeight()[i] = Order_DGV.Columns[i].FillWeight;
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
            DisplayAsync(SystemArgs.Orders);
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

        private void ChangeStatuses_TSM_Click(object sender, EventArgs e)
        {
            try
            {
                List<Order> Selections = new List<Order>();
                if (Order_DGV.CurrentCell.RowIndex >= 0)
                {
                    Chief_PDO_ChangedStatuses_F Dialog = new Chief_PDO_ChangedStatuses_F();
                    Dialog.Statuses_CB.DataSource = SystemArgs.Statuses;
                    if(Dialog.ShowDialog() == DialogResult.OK)
                    {
                        for (int i = 0; i < Order_DGV.SelectedRows.Count; i++)
                        {
                            Order ChangedOrder = (Order)(View[Order_DGV.SelectedRows[i].Index]);
                            Status Temp = ChangedOrder.Status;
                            if (ChangedOrder.Status != SystemArgs.Statuses.Where(p => p == (Status)Dialog.Statuses_CB.SelectedItem).Single())
                            {
                                ChangedOrder.Status = (Status)Dialog.Statuses_CB.SelectedItem;
                                ChangedOrder.StatusDate = DateTime.Now;
                            }
                            else
                            {
                                List<DateTime> StatusDate = SystemArgs.StatusOfOrders.Where(p => p.IDOrder == ChangedOrder.ID && p.IDStatus == SystemArgs.Statuses.Where(j => j == (Status)Dialog.Statuses_CB.SelectedItem).Single().ID).Select(p => p.DateCreate).ToList();
                                ChangedOrder.Status = (Status)Dialog.Statuses_CB.SelectedItem;
                                ChangedOrder.StatusDate = StatusDate[0];
                            }

                            if (SystemArgs.Request.UpdateOrder(ChangedOrder))
                            {
                                if (Temp.ID > SystemArgs.Statuses.Where(p => p == (Status)Dialog.Statuses_CB.SelectedItem).Single().ID)
                                {
                                    SystemArgs.Request.DownGradeStatus(ChangedOrder);
                                }
                                else
                                {
                                    int NewStatus = (int)ChangedOrder.Status.ID;
                                    for (int j = (int)Temp.ID + 1; j <= NewStatus; j++)
                                    {
                                        ChangedOrder.Status = SystemArgs.Statuses.Where(p => p.ID == j).Single();
                                        if (!SystemArgs.Request.StatusExist(ChangedOrder))
                                        {
                                            SystemArgs.Request.InsertStatus(ChangedOrder);
                                        }
                                    }
                                }
                                SystemArgs.Orders.Remove((Order)(View[Order_DGV.SelectedRows[i].Index]));
                                SystemArgs.Orders.Add(ChangedOrder);
                            }
                            else
                            {
                                throw new Exception("Ошибка обновления данных черетежа");
                            }
                        }
                        MessageBox.Show("Статусы успешно изменены", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    throw new Exception("Необходимо выбрать объекты");
                }
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                for (int i = 0; i < SystemArgs.SelectedColumn.GetVisibels().Length; i++)
                {
                    Order_DGV.Columns[i].DisplayIndex = SystemArgs.SelectedColumn.GetDisplayIndex()[i];
                    Order_DGV.Columns[i].Visible = SystemArgs.SelectedColumn[i];
                    Order_DGV.Columns[i].FillWeight = SystemArgs.SelectedColumn.GetFillWeight()[i];
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

                Dialog.DataMatrix_CB.Checked = SystemArgs.SelectedColumn[0];
                Dialog.DateCreate_CB.Checked = SystemArgs.SelectedColumn[1];
                Dialog.Number_CB.Checked = SystemArgs.SelectedColumn[2];
                Dialog.Executor_CB.Checked = SystemArgs.SelectedColumn[3];
                Dialog.ExecutorWork_CB.Checked = SystemArgs.SelectedColumn[4];
                Dialog.List_CB.Checked = SystemArgs.SelectedColumn[5];
                Dialog.Mark_CB.Checked = SystemArgs.SelectedColumn[6];
                Dialog.Lenght_CB.Checked = SystemArgs.SelectedColumn[7];
                Dialog.Height_CB.Checked = SystemArgs.SelectedColumn[8];
                Dialog.Status_CB.Checked = SystemArgs.SelectedColumn[9];
                Dialog.User_CB.Checked = SystemArgs.SelectedColumn[10];
                Dialog.BlankOrder_CB.Checked = SystemArgs.SelectedColumn[11];
                Dialog.Cancelled_CB.Checked = SystemArgs.SelectedColumn[12];
                Dialog.StatusDate_CB.Checked = SystemArgs.SelectedColumn[13];

                if (Dialog.ShowDialog() == DialogResult.OK)
                {
                    SystemArgs.SelectedColumn[0] = Dialog.DataMatrix_CB.Checked;
                    SystemArgs.SelectedColumn[1] = Dialog.DateCreate_CB.Checked;
                    SystemArgs.SelectedColumn[2] = Dialog.Number_CB.Checked;
                    SystemArgs.SelectedColumn[3] = Dialog.Executor_CB.Checked;
                    SystemArgs.SelectedColumn[4] = Dialog.ExecutorWork_CB.Checked;
                    SystemArgs.SelectedColumn[5] = Dialog.List_CB.Checked;
                    SystemArgs.SelectedColumn[6] = Dialog.Mark_CB.Checked;
                    SystemArgs.SelectedColumn[7] = Dialog.Lenght_CB.Checked;
                    SystemArgs.SelectedColumn[8] = Dialog.Height_CB.Checked;
                    SystemArgs.SelectedColumn[9] = Dialog.Status_CB.Checked;
                    SystemArgs.SelectedColumn[10] = Dialog.User_CB.Checked;
                    SystemArgs.SelectedColumn[11] = Dialog.BlankOrder_CB.Checked;
                    SystemArgs.SelectedColumn[12] = Dialog.Cancelled_CB.Checked;
                    SystemArgs.SelectedColumn[13] = Dialog.StatusDate_CB.Checked;
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
    }
}
