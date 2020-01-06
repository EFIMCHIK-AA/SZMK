using System;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZMK
{
    public class UnLoadSpecific
    {
        public struct Specific
        {
            private String _Number;
            private Int64 _List;
            private String _Executor;
            private Int64 _NumberSpecific;
            private Boolean _Finded;
            public Specific(String Number,Int64 List,String Executor,Int64 NumberSpecific,Boolean Finded)
            {
                if (!String.IsNullOrEmpty(Number))
                {
                    _Number = Number;
                }
                else
                {
                    throw new Exception("Не задан номер заказа");
                }
                if (List >= 0)
                {
                    _List = List;
                }
                else
                {
                    throw new Exception("Номер листа заказа меньше 0");
                }
                if (!String.IsNullOrEmpty(Executor))
                {
                    _Executor = Executor;
                }
                else
                {
                    throw new Exception("Не задан исполнитель");
                }
                if (NumberSpecific >= 0)
                {
                    _NumberSpecific = NumberSpecific;
                }
                else
                {
                    throw new Exception("Номер детали меньше 0");
                }
                _Finded = Finded;
            }
            public String Number
            {
                get
                {
                    return _Number;
                }
                set
                {
                    if (!String.IsNullOrEmpty(Number))
                    {
                        _Number = value;
                    }
                }
            }
            public Int64 List
            {
                get
                {
                    return _List;
                }
                set
                {
                    if (value>=0)
                    {
                        _List = value;
                    }
                }
            }
            public String Executor
            {
                get
                {
                    return _Executor;
                }
                set
                {
                    if (!String.IsNullOrEmpty(Executor))
                    {
                        _Executor = value;
                    }
                }
            }
            public Int64 NumberSpecific
            {
                get
                {
                    return _NumberSpecific;
                }
                set
                {
                    if (value >= 0)
                    {
                        _NumberSpecific = value;
                    }
                }
            }
            public Boolean Finded
            {
                get
                {
                    return _Finded;
                }
                set
                {
                    _Finded = value;
                }
            }
        }
        public List<Specific> Specifics;
        public UnLoadSpecific()
        {
            Specifics = new List<Specific>();
        }
        public void ChekedUnloading(List<OrderScanSession> ScanSession)
        {
            for (int i = 0; i < ScanSession.Count; i++)
            {
                String[] SplitDataMatrix = ScanSession[i].DataMatrix.Split('_');
                String pathSpecific = "";
                if (Directory.Exists(SystemArgs.Path.DirectoryModelsPath + SplitDataMatrix[0]))
                {
                    pathSpecific = SystemArgs.Path.DirectoryModelsPath + SplitDataMatrix[0];
                }
                else if (Directory.Exists(SystemArgs.Path.DirectoryModelsPath + SplitDataMatrix[0].Remove(SplitDataMatrix[0].IndexOf('('), SplitDataMatrix[0].Length - SplitDataMatrix[0].IndexOf('(')) + @"\" + SplitDataMatrix[0]))
                {
                    pathSpecific = SystemArgs.Path.DirectoryModelsPath + SplitDataMatrix[0].Remove(SplitDataMatrix[0].IndexOf('('), SplitDataMatrix[0].Length - SplitDataMatrix[0].IndexOf('(')) + @"\" + SplitDataMatrix[0];
                }
                else
                {
                    throw new Exception("Папки с номером заказа " + SplitDataMatrix[0] + " не существует");
                }
                if (File.Exists(pathSpecific + @"\Отчеты\#Для выгрузки.xls"))
                {
                    Boolean flag = false;
                    ExcelPackage WBChecked = new ExcelPackage(new System.IO.FileInfo(pathSpecific + @"\Отчеты\#Для выгрузки.xls"));
                    ExcelWorksheet WSChecked = WBChecked.Workbook.Worksheets[1];
                    for (int j = 0; j < WSChecked.Dimension.End.Row; i++)
                    {
                        if (WSChecked.Cells[j, 3].Value.ToString() == SplitDataMatrix[1])
                        {
                            if (WSChecked.Cells[j, 7].Value != null)
                            {
                                if (File.Exists(pathSpecific + @"\Чертежи\Детфли PDF\" + "Дет." + WSChecked.Cells[j, 7].Value.ToString() + ".pdf"))
                                {
                                    Specifics.Add(new Specific(SplitDataMatrix[0], Convert.ToInt64(SplitDataMatrix[1]), SplitDataMatrix[1], Convert.ToInt64(SplitDataMatrix[3]), true));
                                }
                                else
                                {
                                    Specifics.Add(new Specific(SplitDataMatrix[0], Convert.ToInt64(SplitDataMatrix[1]), SplitDataMatrix[1], Convert.ToInt64(SplitDataMatrix[3]), false));
                                    flag = true;
                                }
                            }
                        }
                    }
                    if (flag)
                    {
                        SystemArgs.ServerMail.SendMail();
                    }
                }
                else
                {
                    throw new Exception("Файла #Для выгрузки.xls не найдено по пути " + pathSpecific);
                }
            }
        }
    }
}
