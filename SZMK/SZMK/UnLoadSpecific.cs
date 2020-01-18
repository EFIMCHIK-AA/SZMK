using System;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;
using System.Xml.Linq;
using System.Linq;
using System.Text;

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
            public String FindedView
            {
                get
                {
                    if (_Finded)
                    {
                        return "Найдено";
                    }
                    else
                    {
                        return "Не найдено";
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
        struct Unloading
        {
            public String _List;
            public String _Detail;

            public Unloading(String List, String Detail)
            {
                _List = List;
                _Detail = Detail;
            }
        }
        public void ChekedUnloading(List<OrderScanSession> ScanSession)
        {
            SystemArgs.UnLoadSpecific.Specifics.Clear();
            for (int i = 0; i < ScanSession.Count; i++)
            {
                String[] SplitDataMatrix = ScanSession[i].DataMatrix.Split('_');
                String pathSpecific = SystemArgs.ClientProgram.ModelsPath;
                if (Directory.Exists(pathSpecific + "\\" + SplitDataMatrix[0]))
                {
                    pathSpecific = pathSpecific + "\\" + SplitDataMatrix[0];
                }
                else if (Directory.Exists(pathSpecific + "\\" + SplitDataMatrix[0].Remove(SplitDataMatrix[0].IndexOf('('), SplitDataMatrix[0].Length - SplitDataMatrix[0].IndexOf('(')) + @"\" + SplitDataMatrix[0]))
                {
                    pathSpecific = pathSpecific + "\\" + SplitDataMatrix[0].Remove(SplitDataMatrix[0].IndexOf('('), SplitDataMatrix[0].Length - SplitDataMatrix[0].IndexOf('(')) + @"\" + SplitDataMatrix[0];
                }
                else
                {
                    throw new Exception("Папки с номером заказа " + SplitDataMatrix[0] + " не существует");
                }
                if (File.Exists(pathSpecific + @"\Отчеты\#Для выгрузки.xml"))
                {
                    Boolean flag = false;
                    String List = "";
                    String Detail = "";
                    List<Unloading> Temp = new List<Unloading>();
                    XDocument doc = XDocument.Load(pathSpecific + @"\Отчеты\#Для выгрузки.xml");
                    foreach (XElement el in doc.Element("Export").Elements("Сборка"))
                    {
                        foreach (XElement xml in el.Elements("Деталь"))
                        {
                            List = el.Element("Лист").Value.Trim();
                            Detail = xml.Element("Позиция_детали").Value.Trim();
                            Temp.Add(new Unloading(List, Detail));
                        }
                    }
                    for (int j = 0; j < Temp.Count; j++)
                    {
                        if (Temp[j]._List == SplitDataMatrix[1])
                        {
                            if (Temp[j]._Detail != null)
                            {
                                if (File.Exists(pathSpecific + @"\Чертежи\Детали PDF\" + "Дет." + Temp[j]._Detail + ".pdf"))
                                {
                                    Specifics.Add(new Specific(SplitDataMatrix[0], Convert.ToInt64(SplitDataMatrix[1]), SplitDataMatrix[3], Convert.ToInt64(Temp[j]._Detail), true));
                                }
                                else
                                {
                                    Specifics.Add(new Specific(SplitDataMatrix[0], Convert.ToInt64(SplitDataMatrix[1]), SplitDataMatrix[3], Convert.ToInt64(Temp[j]._Detail), false));
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
