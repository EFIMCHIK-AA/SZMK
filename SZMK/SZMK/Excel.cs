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
                                wsUnique.Cells[rowCntActUnique + 1, 7].Value = DateTime.Now.ToShortDateString();
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
                                wsNoUnique.Cells[rowCntActNoUnique + 1, 7].Value = DateTime.Now.ToShortDateString();
                                wsNoUnique.Cells[rowCntActNoUnique + 1, 8].Value = Status;
                                wsNoUnique.Cells[rowCntActNoUnique + 1, 9].Value = SystemArgs.User.Surname + " " + SystemArgs.User.Name + " " + SystemArgs.User.MiddleName;
                            }
                        }
                        int lastline = wsUnique.Dimension.End.Row;
                        wsUnique.Cells[lastline + 2, 6].Value = "Принял";
                        wsUnique.Cells[lastline + 3, 6].Value = "Сдал";
                        wsUnique.Cells[lastline + 2, 8].Value = "______________";
                        wsUnique.Cells[lastline + 3, 8].Value = "______________";
                        wsUnique.Cells[lastline + 2, 9].Value = SystemArgs.User.Surname + " " + SystemArgs.User.Name + " " + SystemArgs.User.MiddleName;
                        wsUnique.Cells[lastline + 3, 9].Value = "/______________/";
                        wbUnique.Save();
                        lastline = wsNoUnique.Dimension.End.Row;
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
                    ExcelPackage WB = new ExcelPackage(new System.IO.FileInfo(SaveReport.FileName));
                    ExcelWorksheet WS = WB.Workbook.Worksheets[1];
                    var rowCntReport = WS.Dimension.End.Row;

                    if (SaveReport.FileName.IndexOf(@":\") != -1)
                    {
                        List<Order> Report = SystemArgs.Orders.Where(p => (p.DateCreate >= First) && (p.DateCreate <= Second)).ToList();
                        for (Int32 i = 0; i < Report.Count; i++)
                        {
                            WS.Cells[i + rowCntReport + 1, 1].Value = Report[i].Number;
                            WS.Cells[i + rowCntReport + 1, 2].Value = Report[i].List.ToString();
                            WS.Cells[i + rowCntReport + 1, 3].Value = Report[i].Mark;
                            WS.Cells[i + rowCntReport + 1, 4].Value = Report[i].Executor;
                            WS.Cells[i + rowCntReport + 1, 5].Value = Report[i].Lenght.ToString();
                            WS.Cells[i + rowCntReport + 1, 6].Value = Report[i].Weight.ToString();
                            WS.Cells[i + rowCntReport + 1, 7].Value = Report[i].BlankOrder.QR;
                            WS.Cells[i + rowCntReport + 1, 8].Value = Report[i].Status.Name;
                            WS.Cells[i + rowCntReport + 1, 9].Value = Report[i].User.Login;
                            WS.Cells[i + rowCntReport + 1, 10].Value = Report[i].DateCreate.ToShortDateString().ToString();
                        }
                        int last = WS.Dimension.End.Row;
                        WS.Cells[last + 2, 8].Value = "Принял";
                        WS.Cells[last + 3, 8].Value = "Сдал";
                        WS.Cells[last + 2, 9].Value = "______________";
                        WS.Cells[last + 3, 9].Value = "______________";
                        WS.Cells[last + 2, 10].Value = SystemArgs.User.Surname + " " + SystemArgs.User.Name + " " + SystemArgs.User.MiddleName;
                        WS.Cells[last + 3, 10].Value = "/______________/";
                        WS.Cells["A2:J"+ WS.Dimension.End.Row.ToString()].AutoFitColumns();
                        WS.Cells["A2:J" + WS.Dimension.End.Row.ToString()].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
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
    }
}
