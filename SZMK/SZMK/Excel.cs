using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OfficeOpenXml;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace SZMK
{
    public class Excel
    {
        public Boolean CreateAndExportActs(List<ScanSession> ScanSession)
        {
            SaveFileDialog SaveAct = new SaveFileDialog();
            String date = DateTime.Now.ToString();
            date = date.Replace(".", "_");
            date = date.Replace(":", "_");
            SaveAct.FileName = "Акты от " + date;
            FileInfo fInfoSrcUnique = new FileInfo(SystemArgs.Path.TemplateActUniquePath);
            FileInfo fInfoSrcNoUnique = new FileInfo(SystemArgs.Path.TemplateActNoUniquePath);

            if (SaveAct.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Directory.CreateDirectory(SaveAct.FileName.Replace(".xlsx", ""));
                    new ExcelPackage(fInfoSrcUnique).File.CopyTo(SaveAct.FileName + @"\Акт от " + date + " уникальных чертежей.xlsx");
                    new ExcelPackage(fInfoSrcNoUnique).File.CopyTo(SaveAct.FileName + @"\Акт от " + date + " не уникальных чертежей.xlsx");

                    ExcelPackage wbUnique = new ExcelPackage(new System.IO.FileInfo(SaveAct.FileName + @"\Акт от " + date + " уникальных чертежей.xlsx"));
                    ExcelWorksheet wsUnique = wbUnique.Workbook.Worksheets[1];
                    var rowCntAct = wsUnique.Dimension.End.Row;

                    ExcelPackage wbNoUnique = new ExcelPackage(new System.IO.FileInfo(SaveAct.FileName + @"\Акт от " + date + " не уникальных чертежей.xlsx"));
                    ExcelWorksheet wsNoUnique = wbNoUnique.Workbook.Worksheets[1];
                    rowCntAct = wsNoUnique.Dimension.End.Row;

                    if (SaveAct.FileName.IndexOf(@":\") != -1)
                    {
                        for (Int32 i = 0; i < ScanSession.Count; i++)
                        {
                            String[] SplitDataMatrix = ScanSession[i].DataMatrix.Split('_');
                            if (ScanSession[i].Unique)
                            {
                                wsUnique.Cells[i + rowCntAct + 1, 1].Value = SplitDataMatrix[0];
                                wsUnique.Cells[i + rowCntAct + 1, 2].Value = Convert.ToInt64(SplitDataMatrix[1]);
                                wsUnique.Cells[i + rowCntAct + 1, 3].Value = SplitDataMatrix[2];
                                wsUnique.Cells[i + rowCntAct + 1, 4].Value = SplitDataMatrix[3];
                                wsUnique.Cells[i + rowCntAct + 1, 5].Value = Convert.ToDouble(SplitDataMatrix[4]);
                                wsUnique.Cells[i + rowCntAct + 1, 6].Value = Convert.ToDouble(SplitDataMatrix[5]);
                                wsUnique.Cells[i + rowCntAct + 1, 7].Value = DateTime.Now.ToShortDateString();
                                wsUnique.Cells[i + rowCntAct + 1, 8].Value = "Добавлены начальником групп КБ";
                                wsUnique.Cells[i + rowCntAct + 1, 9].Value = SystemArgs.User.Surname + " " + SystemArgs.User.Name + " " + SystemArgs.User.MiddleName;
                            }
                            else
                            {
                                wsNoUnique.Cells[i + rowCntAct + 1, 1].Value = SplitDataMatrix[0];
                                wsNoUnique.Cells[i + rowCntAct + 1, 2].Value = Convert.ToInt64(SplitDataMatrix[1]);
                                wsNoUnique.Cells[i + rowCntAct + 1, 3].Value = SplitDataMatrix[2];
                                wsNoUnique.Cells[i + rowCntAct + 1, 4].Value = SplitDataMatrix[3];
                                wsNoUnique.Cells[i + rowCntAct + 1, 5].Value = Convert.ToDouble(SplitDataMatrix[4]);
                                wsNoUnique.Cells[i + rowCntAct + 1, 6].Value = Convert.ToDouble(SplitDataMatrix[5]);
                                wsNoUnique.Cells[i + rowCntAct + 1, 7].Value = DateTime.Now.ToShortDateString();
                                wsNoUnique.Cells[i + rowCntAct + 1, 8].Value = "Добавлены начальником групп КБ";
                                wsNoUnique.Cells[i + rowCntAct + 1, 9].Value = SystemArgs.User.Surname + " " + SystemArgs.User.Name + " " + SystemArgs.User.MiddleName;
                            }
                        }
                        int lastline = wsUnique.Dimension.End.Row;
                        wsUnique.Cells[lastline + 2, 6].Value = "Принял";
                        wsUnique.Cells[lastline + 3, 6].Value = "Сдал";
                        wsUnique.Cells[lastline + 2, 8].Value = "______________";
                        wsUnique.Cells[lastline + 3, 8].Value = "______________";
                        wsUnique.Cells[lastline + 2, 9].Value = SystemArgs.User.Surname+" "+ SystemArgs.User.Name+" "+ SystemArgs.User.MiddleName;
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
                catch(Exception e)
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
