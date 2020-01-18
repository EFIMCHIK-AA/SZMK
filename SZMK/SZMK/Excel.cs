using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace SZMK
{
    public class Excel
    {
        public Boolean CreateAndExportActs(List<OrderScanSession> ScanSession,Boolean Added)
        {
            SaveFileDialog SaveAct = new SaveFileDialog();
            String date = DateTime.Now.ToString();
            date = date.Replace(".", "_");
            date = date.Replace(":", "_");
            SaveAct.FileName = "Акты от " + date;
            FileInfo fInfoSrcUnique = new FileInfo(SystemArgs.Path.TemplateActUniquePath);
            FileInfo fInfoSrcNoUnique = new FileInfo(SystemArgs.Path.TemplateActNoUniquePath);
            String Status = (from p in SystemArgs.Statuses
                             where p.IDPosition == SystemArgs.User.GetPosition().ID
                             select p.Name).Single();

            if (SaveAct.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    String UniqueFileName = "";
                    String NoUniqueFileName = "";
                    if (!Added)
                    {
                        UniqueFileName = SaveAct.FileName + @"\Акт от " + date + " найденных чертежей.xlsx";
                        NoUniqueFileName = SaveAct.FileName + @"\Акт от " + date + " не найденных чертежей.xlsx";
                    }
                    else
                    {
                        UniqueFileName = SaveAct.FileName + @"\Акт от " + date + " уникальных чертежей.xlsx";
                        NoUniqueFileName = SaveAct.FileName + @"\Акт от " + date + " не уникальных чертежей.xlsx";
                    }
                    Directory.CreateDirectory(SaveAct.FileName.Replace(".xlsx", ""));
                    new ExcelPackage(fInfoSrcUnique).File.CopyTo(UniqueFileName);
                    new ExcelPackage(fInfoSrcNoUnique).File.CopyTo(NoUniqueFileName);

                    ExcelPackage wbUnique = new ExcelPackage(new System.IO.FileInfo(UniqueFileName));
                    ExcelWorksheet wsUnique = wbUnique.Workbook.Worksheets[1];

                    ExcelPackage wbNoUnique = new ExcelPackage(new System.IO.FileInfo(NoUniqueFileName));
                    ExcelWorksheet wsNoUnique = wbNoUnique.Workbook.Worksheets[1];

                    if (SaveAct.FileName.IndexOf(@":\") != -1)
                    {
                        if (!Added)
                        {
                            wsUnique.Cells[1, 1].Value = "Акт найденных чертежей";
                            wsNoUnique.Cells[1, 1].Value = "Акт не найденных чертежей";
                        }
                        for (Int32 i = 0; i < ScanSession.Count; i++)
                        {
                            String[] SplitDataMatrix = ScanSession[i].DataMatrix.Split('_');
                            if (ScanSession[i].Unique)
                            {
                                int rowCntActUnique = wsUnique.Dimension.End.Row;
                                wsUnique.Cells[rowCntActUnique + 1, 1].Value = SplitDataMatrix[0];
                                wsUnique.Cells[rowCntActUnique + 1, 2].Value = Convert.ToInt64(SplitDataMatrix[1]);
                                wsUnique.Cells[rowCntActUnique + 1, 3].Value = SplitDataMatrix[2];
                                wsUnique.Cells[rowCntActUnique + 1, 4].Value = SplitDataMatrix[3];
                                wsUnique.Cells[rowCntActUnique + 1, 5].Value = Convert.ToDouble(SplitDataMatrix[4]);
                                wsUnique.Cells[rowCntActUnique + 1, 6].Value = Convert.ToDouble(SplitDataMatrix[5]);
                                wsUnique.Cells[rowCntActUnique + 1, 7].Value = DateTime.Now.ToString();
                                wsUnique.Cells[rowCntActUnique + 1, 8].Value = Status;
                                wsUnique.Cells[rowCntActUnique + 1, 9].Value = SystemArgs.User.Surname + " " + SystemArgs.User.Name + " " + SystemArgs.User.MiddleName;
                            }
                            else
                            {
                                int rowCntActNoUnique = wsNoUnique.Dimension.End.Row;
                                wsNoUnique.Cells[rowCntActNoUnique + 1, 1].Value = SplitDataMatrix[0];
                                wsNoUnique.Cells[rowCntActNoUnique + 1, 2].Value = Convert.ToInt64(SplitDataMatrix[1]);
                                wsNoUnique.Cells[rowCntActNoUnique + 1, 3].Value = SplitDataMatrix[2];
                                wsNoUnique.Cells[rowCntActNoUnique + 1, 4].Value = SplitDataMatrix[3];
                                wsNoUnique.Cells[rowCntActNoUnique + 1, 5].Value = Convert.ToDouble(SplitDataMatrix[4]);
                                wsNoUnique.Cells[rowCntActNoUnique + 1, 6].Value = Convert.ToDouble(SplitDataMatrix[5]);
                                wsNoUnique.Cells[rowCntActNoUnique + 1, 7].Value = DateTime.Now.ToString();
                                wsNoUnique.Cells[rowCntActNoUnique + 1, 8].Value = Status;
                                wsNoUnique.Cells[rowCntActNoUnique + 1, 9].Value = SystemArgs.User.Surname + " " + SystemArgs.User.Name + " " + SystemArgs.User.MiddleName;
                            }
                        }
                        int lastline = wsUnique.Dimension.End.Row;
                        wsUnique.Cells["A2:P" + lastline].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        wsUnique.Cells["A2:P" + lastline].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        wsUnique.Cells["A2:P" + lastline].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        wsUnique.Cells["A2:P" + lastline].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        wsUnique.Cells[lastline + 2, 6].Value = "Принял";
                        wsUnique.Cells[lastline + 3, 6].Value = "Сдал";
                        wsUnique.Cells[lastline + 2, 8].Value = "______________";
                        wsUnique.Cells[lastline + 3, 8].Value = "______________";
                        wsUnique.Cells[lastline + 2, 9].Value = SystemArgs.User.Surname + " " + SystemArgs.User.Name + " " + SystemArgs.User.MiddleName;
                        wsUnique.Cells[lastline + 3, 9].Value = "/______________/";
                        wbUnique.Save();
                        lastline = wsNoUnique.Dimension.End.Row;
                        wsNoUnique.Cells["A2:P" + lastline].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        wsNoUnique.Cells["A2:P" + lastline].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        wsNoUnique.Cells["A2:P" + lastline].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        wsNoUnique.Cells["A2:P" + lastline].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        wsNoUnique.Cells[lastline + 2, 6].Value = "Принял";
                        wsNoUnique.Cells[lastline + 3, 6].Value = "Сдал";
                        wsNoUnique.Cells[lastline + 2, 8].Value = "______________";
                        wsNoUnique.Cells[lastline + 3, 8].Value = "______________";
                        wsNoUnique.Cells[lastline + 2, 9].Value = SystemArgs.User.Surname + " " + SystemArgs.User.Name + " " + SystemArgs.User.MiddleName;
                        wsNoUnique.Cells[lastline + 3, 9].Value = "/______________/";
                        wbNoUnique.Save();
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool ReportOrderOfDate(DateTime First,DateTime Second)
        {
            SaveFileDialog SaveReport = new SaveFileDialog();
            String date = DateTime.Now.ToString();
            date = date.Replace(".", "_");
            date = date.Replace(":", "_");
            SaveReport.FileName = "Отчет за выбранный период от " + date;
            SaveReport.Filter = "Excel Files .xlsx|*.xlsx";

            System.IO.FileInfo fInfoSrcUnique = new System.IO.FileInfo(SystemArgs.Path.TemplateReportOrderOfDatePath);

            if (SaveReport.ShowDialog() == DialogResult.OK)
            {
                var WBcopy = new ExcelPackage(fInfoSrcUnique).File.CopyTo(SaveReport.FileName);

                try
                {
                    List<StatusOfUser> Statuses = new List<StatusOfUser>();
                    SystemArgs.Request.GetAllStatusOfUser(Statuses);
                    ExcelPackage WB = new ExcelPackage(new System.IO.FileInfo(SaveReport.FileName));
                    ExcelWorksheet WS = WB.Workbook.Worksheets[1];
                    var rowCntReport = WS.Dimension.End.Row;

                    if (SaveReport.FileName.IndexOf(@":\") != -1)
                    {
                        List<Order> Report = SystemArgs.Orders.Where(p => (p.DateCreate >= First) && (p.DateCreate <= Second)).ToList();
                        for (Int32 i = 0; i < Report.Count; i++)
                        {
                            List<StatusOfUser> OrderStatuses = (from p in Statuses
                                                                where p.IDOrder == Report[i].ID
                                                                orderby p.IDStatus
                                                                select p).ToList();
                            WS.Cells[i + rowCntReport + 1, 1].Value = Report[i].Number;
                            if (Report[i].BlankOrder.QR.Split('_').Length > 3)
                            {
                                WS.Cells[i + rowCntReport + 1, 2].Value = Report[i].BlankOrder.QR.Split('_')[1];
                            }
                            else
                            {
                                WS.Cells[i + rowCntReport + 1, 2].Value = Report[i].BlankOrder.QR;
                            }
                            WS.Cells[i + rowCntReport + 1, 3].Value = Report[i].List.ToString();
                            WS.Cells[i + rowCntReport + 1, 4].Value = Report[i].Mark;
                            WS.Cells[i + rowCntReport + 1, 5].Value = Report[i].Executor;
                            WS.Cells[i + rowCntReport + 1, 6].Value = Report[i].Lenght.ToString();
                            WS.Cells[i + rowCntReport + 1, 7].Value = Report[i].Weight.ToString();
                            WS.Cells[i + rowCntReport + 1, 8].Value = Report[i].Status.Name;
                            Int32 Count = 0;
                            for (int j = 0; j < OrderStatuses.Count; j++)
                            {
                                User Temp = (from p in SystemArgs.Users
                                             where p.ID == OrderStatuses[j].IDUser
                                             select p).Single();
                                WS.Cells[i + rowCntReport + 1, 9+Count].Value = Temp.Surname + " " + Temp.Name.First() + ". " + Temp.MiddleName.First()+".";
                                WS.Cells[i + rowCntReport + 1, 10+Count].Value = OrderStatuses[j].DateCreate.ToString();
                                Count = Count + 2;
                            }
                        }
                        int last = WS.Dimension.End.Row;
                        Double Sum = Report.Sum(p=>p.Weight);
                        WS.Cells[last + 1, 1].Value = "Итого";
                        WS.Cells[last + 1, 7].Value = Sum;
                        last = WS.Dimension.End.Row;
                        WS.Cells["A2:P" + last].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        WS.Cells["A2:P" + last].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        WS.Cells["A2:P" + last].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        WS.Cells["A2:P" + last].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        WS.Cells[last + 2, 14].Value = "Принял";
                        WS.Cells[last + 3, 14].Value = "Сдал";
                        WS.Cells[last + 2, 15].Value = "______________";
                        WS.Cells[last + 3, 15].Value = "______________";
                        WS.Cells[last + 2, 16].Value = SystemArgs.User.Surname + " " + SystemArgs.User.Name + " " + SystemArgs.User.MiddleName;
                        WS.Cells[last + 3, 16].Value = "/______________/";
                        WS.Cells["A2:P"+ WS.Dimension.End.Row.ToString()].AutoFitColumns();
                        WS.Cells["A2:P" + WS.Dimension.End.Row.ToString()].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        WB.Save();
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool AddToRegistry(Order Order)
        {
            try
            {
                System.IO.FileInfo fInfoSrcTemplateRegistry = new System.IO.FileInfo(SystemArgs.Path.TemplateRegistry);
                System.IO.FileInfo fInfoSrcRegistry = new System.IO.FileInfo(SystemArgs.ClientProgram.RegistryPath + @"\Реестр.xlsx");
                if (!File.Exists(fInfoSrcRegistry.ToString()))
                {
                    var WBcopy = new ExcelPackage(fInfoSrcTemplateRegistry).File.CopyTo(fInfoSrcRegistry.ToString());
                }
                ExcelPackage WB = new ExcelPackage(new System.IO.FileInfo(fInfoSrcRegistry.ToString()));
                ExcelWorksheet WS = WB.Workbook.Worksheets[1];
                var rowCntReport = WS.Dimension.End.Row;
                WS.Cells[rowCntReport + 1, 1].Value = Order.Number;
                if (Order.BlankOrder.QR.Split('_').Length > 3)
                {
                    WS.Cells[rowCntReport + 1, 2].Value = Order.BlankOrder.QR.Split('_')[1];
                }
                else
                {
                    WS.Cells[rowCntReport + 1, 2].Value = Order.BlankOrder.QR;
                }
                WS.Cells[rowCntReport + 1, 3].Value = Order.List.ToString();
                WS.Cells[rowCntReport + 1, 4].Value = Order.Mark;
                WS.Cells[rowCntReport + 1, 5].Value = Order.Executor;
                WS.Cells[rowCntReport + 1, 6].Value = Order.Lenght.ToString();
                WS.Cells[rowCntReport + 1, 7].Value = Order.Weight.ToString();
                WS.Cells[rowCntReport + 1, 8].Value = Order.Status.Name;
                WS.Cells["A2:H" + WS.Dimension.End.Row.ToString()].AutoFitColumns();
                WS.Cells["A2:H" + WS.Dimension.End.Row.ToString()].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                WB.Save();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
