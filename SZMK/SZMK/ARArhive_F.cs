using System;
using System.Drawing;
using System.Threading;
using Npgsql;
using System.IO;
using Equin.ApplicationFramework;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace SZMK
{
    public partial class ARArhive_F : Form
    {
        public ARArhive_F()
        {
            InitializeComponent();
        }

        BindingListView<Order> View;

        private void ARArhive_F_Load(object sender, EventArgs e)
        {
            try
            {
                Order_DGV.AutoGenerateColumns = false;
                Order_DGV.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Order_DGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                Load_F Dialog = new Load_F();
                Dialog.Show();

                SystemArgs.ByteScout = new ByteScout();
                SystemArgs.ClientProgram = new ClientProgram();
                SystemArgs.MobileApplication = new MobileApplication();
                SystemArgs.Orders = new List<Order>();
                SystemArgs.BlankOrders = new List<BlankOrder>();
                SystemArgs.Statuses = new List<Status>();
                SystemArgs.BlankOrderOfOrders = new List<BlankOrderOfOrder>();
                SystemArgs.StatusOfOrders = new List<StatusOfOrder>();
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
                RefreshOrder();
            }
        }

        private void ChangeOrder_TSB_Click(object sender, EventArgs e)
        {
            if (ChangeOrder())
            {
                RefreshOrder();
            }
        }

        private void DeleteOrder_TSB_Click(object sender, EventArgs e)
        {
            if (DeleteOrder())
            {
                RefreshOrder();
            }
        }

        private void AddOrder_TSM_Click(object sender, EventArgs e)
        {
            if (AddOrder())
            {
                RefreshOrder();
            }
        }

        private void ChangeOrder_TSM_Click(object sender, EventArgs e)
        {
            if (ChangeOrder())
            {
                RefreshOrder();
            }
        }

        private void DeleteOrder_TSM_Click(object sender, EventArgs e)
        {
            if (DeleteOrder())
            {
                RefreshOrder();
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
            RefreshOrder();
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
                ARDecode_F Dialog = new ARDecode_F();

                if (SystemArgs.ByteScout.CheckConnect())
                {
                    Dialog.ServerStatus_TB.Text = "Подключено";
                    Dialog.ServerStatus_TB.BackColor = Color.FromArgb(233, 245, 255);
                    Dialog.Status_TB.AppendText($"Ожидание распознования" + Environment.NewLine);

                    if (Dialog.ShowDialog() == DialogResult.OK)
                    {
                        ARDecodeReport_F Report = new ARDecodeReport_F();

                        for (int i = 0; i < SystemArgs.ByteScout.GetDecodeSession().Count; i++)
                        {
                            if (SystemArgs.ByteScout[i].Unique)
                            {
                                Int64 PositionID = SystemArgs.User.GetPosition().ID;

                                Status TempStatus = (from p in SystemArgs.Statuses
                                                     where p.IDPosition == PositionID
                                                     select p).Single();

                                Order Temp = SystemArgs.Orders.Where(p => p.DataMatrix == SystemArgs.ByteScout[i].DataMatrix).Single();
                                Order NewOrder = Temp;

                                NewOrder.Status = TempStatus;
                                NewOrder.User = SystemArgs.User;

                                if (SystemArgs.Excel.AddToRegistry(NewOrder))
                                {
                                    if (SystemArgs.Request.InsertStatus(NewOrder))
                                    {
                                        SystemArgs.Orders.Remove(Temp);
                                        SystemArgs.Orders.Add(NewOrder);

                                        Report.Report_DGV.Rows.Add();
                                        Report.Report_DGV[0, Report.Report_DGV.Rows.Count - 1].Value = NewOrder.DataMatrix;
                                        Report.Report_DGV[1, Report.Report_DGV.Rows.Count - 1].Value = CopyFileToArhive(NewOrder.DataMatrix, Dialog.FileNames[i]);
                                    }
                                    else
                                    {
                                        MessageBox.Show("Ошибка при добавлении в базу данных статуса для: " + SystemArgs.ByteScout[i].DataMatrix, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Ошибка добавления данных в реестр, перемещение и обновление статуса " + SystemArgs.ByteScout[i].DataMatrix + " не будет произведено", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            }
                        }
                        if (SystemArgs.ByteScout.GetDecodeSession().Where(p => p.Unique).Count() != 0)
                        {
                            Report.ShowDialog();
                        }
                    }

                    SystemArgs.ByteScout.ClearData();
                    return true;
                }
                else
                {
                    SystemArgs.ByteScout.ClearData();
                    throw new Exception("Ошибка при подключении к серверу распознавнаия");
                }
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

        }
        private String CopyFileToArhive(String DataMatrix, String FileName)
        {
            try
            {
                String[] Temp = DataMatrix.Split('_');
                String TextReport = "";
                if (Directory.Exists($@"{SystemArgs.ClientProgram.ArchivePath}\{Temp[0]}"))
                {
                    if (!File.Exists($@"{SystemArgs.ClientProgram.ArchivePath}\{Temp[0]}\{DataMatrix}.tiff"))
                    {
                        File.Copy(FileName, $@"{SystemArgs.ClientProgram.ArchivePath}\{Temp[0]}\{DataMatrix}.tiff");
                        TextReport = $"Файл {DataMatrix}.tiff помещен в директорию {Temp[0]}" + Environment.NewLine;
                        try
                        {
                            File.Delete(FileName);
                        }
                        catch
                        {
                            MessageBox.Show("Ошибка доступа к файлу по пути " + FileName, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        }

                    }
                    else
                    {
                        TextReport = $"Файл {DataMatrix}.tiff уже сущетвует в директории {Temp[0]}" + Environment.NewLine;
                    }
                }
                else
                {
                    Directory.CreateDirectory($@"{SystemArgs.ClientProgram.ArchivePath}\{Temp[0]}");

                    if (!File.Exists($@"{SystemArgs.ClientProgram.ArchivePath}\{Temp[0]}\{DataMatrix}.tiff"))
                    {
                        File.Copy(FileName, $@"{SystemArgs.ClientProgram.ArchivePath}\{Temp[0]}\{DataMatrix}.tiff");
                        TextReport = $"Директория {Temp[0]} создана. Файл {DataMatrix}.tiff помещен в директорию" + Environment.NewLine;
                        try
                        {
                            File.Delete(FileName);
                        }
                        catch
                        {
                            MessageBox.Show("Ошибка доступа к файлу по пути " + FileName, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        }
                    }
                    else
                    {
                        TextReport = $"Файл {DataMatrix}.tiff уже сущетвует в директории {Temp[0]}" + Environment.NewLine;
                    }
                }
                return TextReport;
            }
            catch
            {
                throw new Exception("Ошибка перемещения файлов по директории");
            }
        }
        private bool ChangeOrder()
        {
            try
            {
                if (Order_DGV.CurrentCell.RowIndex >= 0)
                {
                    Order Temp = (Order)View[Order_DGV.CurrentCell.RowIndex];
                    ARChangeOrder_F Dialog = new ARChangeOrder_F(Temp);

                    Dialog.Executor_TB.Text = Temp.Executor;
                    Dialog.Number_TB.Text = Temp.Number;
                    Dialog.List_TB.Text = Temp.List.ToString();
                    Dialog.Mark_TB.Text = Temp.Mark;
                    Dialog.Lenght_TB.Text = Temp.Lenght.ToString();
                    List<Status> TempStatuses = new List<Status>();
                    TempStatuses.Add(Temp.Status);
                    Dialog.Weight_TB.Text = Temp.Weight.ToString();
                    if (Temp.Status.ID != SystemArgs.Statuses.Min(p => p.ID))
                    {
                        TempStatuses.Add(SystemArgs.Statuses.Where(p => p.ID == Temp.Status.ID - 1).Single());
                    }
                    Dialog.Status_CB.DataSource = TempStatuses;

                    if (Dialog.ShowDialog() == DialogResult.OK)
                    {
                        String NewDataMatrix = Dialog.Number_TB.Text + "_" + Dialog.List_TB.Text + "_" + Dialog.Mark_TB.Text + "_" + Dialog.Executor_TB.Text + "_" + Dialog.Lenght_TB.Text + "_" + Dialog.Weight_TB.Text;
                        Order NewOrder = new Order(Temp.ID, NewDataMatrix, Temp.DateCreate, Dialog.Number_TB.Text, Dialog.Executor_TB.Text, Dialog.List_TB.Text, Dialog.Mark_TB.Text, Convert.ToDouble(Dialog.Lenght_TB.Text), Convert.ToDouble(Dialog.Weight_TB.Text), SystemArgs.Statuses.Where(p => p == (Status)Dialog.Status_CB.SelectedItem).Single(), Temp.User, Temp.BlankOrder, Temp.Canceled);
                        if (SystemArgs.Request.UpdateOrder(NewOrder))
                        {
                            if (Dialog.Status_CB.SelectedIndex != 0)
                            {
                                SystemArgs.Request.DeleteStatus(Temp);
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
            try
            {
                if (FilterCB_TSB.SelectedIndex >= 0)
                {
                    Int32 Index = FilterCB_TSB.SelectedIndex;

                    switch(Index)
                    {
                        case 1:
                            View = new BindingListView<Order>(List.Where(p => !p.Canceled).ToList());

                            Order_DGV.DataSource = null;
                            Order_DGV.DataSource = View;

                            CountOrder_TB.Text = View.Count.ToString();

                            VisibleButton(false);
                            break;
                        case 2:
                            View = new BindingListView<Order>(List.Where(p => p.Canceled).ToList());

                            Order_DGV.DataSource = null;
                            Order_DGV.DataSource = View;

                            CountOrder_TB.Text = View.Count.ToString();

                            VisibleButton(false);
                            if (View.Count > 0)
                            {
                                CanceledOrder_TSB.Text = "Восстановить";
                                CanceledOrder_TSB.Visible = true;
                            }

                            break;
                        default:
                            View = new BindingListView<Order>(List.Where(p => p.Status.IDPosition == SystemArgs.User.GetPosition().ID && !p.Canceled).ToList());

                            Order_DGV.DataSource = null;
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

                            ForgetOrder();
                            break;         
                    }
                }      
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ForgetOrder()
        {
            for (int i = 0; i < Order_DGV.RowCount; i++)
            {
                if ((DateTime.Now - Convert.ToDateTime(Order_DGV[0, i].Value)).TotalDays >= SystemArgs.ClientProgram.VisualRow_N2)
                {
                    Order_DGV.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(236, 0, 6);
                }
                else if ((DateTime.Now - Convert.ToDateTime(Order_DGV[0, i].Value)).TotalDays >= SystemArgs.ClientProgram.VisualRow_N1)
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

        private void ARArhive_F_FormClosing(object sender, FormClosingEventArgs e)
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
            ResetSearch();
            RefreshOrder();
        }

        private void ItemsFilter()
        {
            FilterCB_TSB.Items.Add("Текущий статус");
            FilterCB_TSB.Items.Add("Все статусы");
            FilterCB_TSB.Items.Add("Аннулированные");
            FilterCB_TSB.SelectedIndex = 0;
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
                ARSearchParam_F Dialog = new ARSearchParam_F();

                List<BlankOrder> BlankOrders = new List<BlankOrder>();

                BlankOrders.Add(new BlankOrder());
                BlankOrders.AddRange(SystemArgs.BlankOrders);
                Dialog.BlankOrder_CB.DataSource = BlankOrders;

                List<Status> Statuses = new List<Status>();

                Statuses.Add(new Status(-1, 0, "Не задан"));
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
                ARReportOrderOfDate_F Dialog = new ARReportOrderOfDate_F();
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
            }
            else
            {
                ChangeOrder_TSB.Visible = false;
                DeleteOrder_TSB.Visible = false;
                ChangeOrder_TSM.Visible = false;
                DeleteOrder_TSM.Visible = false;
                CanceledOrder_TSB.Visible = false;
            }
        }
        private void Selection(Order Temp, bool flag)
        {
            if (flag)
            {
                DateCreate_TB.Text = Temp.DateCreate.ToString();
                Executor_TB.Text = Temp.Executor;
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
        private bool RefreshOrder()
        {
            List<Order> Temp = null;

            try
            {
                Temp = new List<Order>(SystemArgs.Orders);

                SystemArgs.Orders.Clear();
                SystemArgs.Statuses.Clear();
                SystemArgs.BlankOrders.Clear();
                SystemArgs.StatusOfOrders.Clear();
                SystemArgs.BlankOrderOfOrders.Clear();

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
            catch (Exception E)
            {
                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Order_DGV_Sorted(object sender, EventArgs e)
        {
            try
            {
                ForgetOrder();
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
                ADSettingsMobileApp_F Dialog = new ADSettingsMobileApp_F();

                if (SystemArgs.MobileApplication.GetParametersConnect())
                {
                    String MyIP = Dns.GetHostAddresses(Dns.GetHostName())[0].ToString();

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

        private void CanceledOrder_TSB_Click(object sender, EventArgs e)
        {
            try
            {
                if(MessageBox.Show("Изменить статус аннулирования чертежа?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (Order_DGV.CurrentCell.RowIndex >= 0)
                    {
                        Order Temp = (Order)View[Order_DGV.CurrentCell.RowIndex];

                        Temp.Canceled = !Temp.Canceled;

                        if (SystemArgs.Request.CanceledOrder(Temp))
                        {
                            Display(SystemArgs.Orders);
                        }
                        else
                        {
                            throw new Exception("Произошла ошибка при аннулировании чертежа");
                        }
                    }
                }
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
