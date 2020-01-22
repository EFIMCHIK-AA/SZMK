using System;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;
using System.Xml.Linq;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SZMK
{
    /*Класс для определения выгрузки деталей с методом проверки а также структурой для хранения исполнителя деталей и все детали в ней*/
    public class UnLoadSpecific
    {
        public struct ExecutorMail
        {
            private String _Executor;
            public List<Specific> _Specifics;
            public ExecutorMail(String Executor)
            {
                if (!String.IsNullOrEmpty(Executor))
                {
                    _Executor = Executor;
                }
                else
                {
                    throw new Exception("Не задан исполнитель");
                }
                _Specifics = new List<Specific>();
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
        public List<ExecutorMail> ExecutorMails;
        public UnLoadSpecific()
        {
            ExecutorMails = new List<ExecutorMail>();
        }
        public void ChekedUnloading(List<OrderScanSession> ScanSession)
        {
            SystemArgs.UnLoadSpecific.ExecutorMails.Clear();
            for (int i = 0; i < ScanSession.Count; i++)
            {
                String[] SplitDataMatrix = ScanSession[i].DataMatrix.Split('_');
                String pathSpecific = SystemArgs.ClientProgram.ModelsPath;
                String[] directories = Directory.GetDirectories(pathSpecific);
                if (Directory.Exists(pathSpecific + "\\" + SplitDataMatrix[0]))
                {
                    pathSpecific = pathSpecific + "\\" + SplitDataMatrix[0];
                }
                else 
                {
                    foreach (var directory in directories)
                    {
                        if (directory.IndexOf(SplitDataMatrix[0].Remove(SplitDataMatrix[0].IndexOf('('), SplitDataMatrix[0].Length - SplitDataMatrix[0].IndexOf('('))) != -1)
                        {
                            pathSpecific = directory + "\\" + SplitDataMatrix[0];
                            break;
                        }
                    }
                }
                if(SystemArgs.ClientProgram.ModelsPath.Equals(pathSpecific))
                {
                    throw new Exception("Папки с номером заказа " + SplitDataMatrix[0] + " не существует");
                }
                if (File.Exists(pathSpecific + @"\Отчеты\#Для выгрузки.xml"))
                {
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
                        if (Temp[j]._List.Equals(SplitDataMatrix[1]))
                        {
                            if (Temp[j]._Detail != null)
                            {
                                if (File.Exists(pathSpecific + @"\Чертежи\Детали PDF\" + "Дет." + Temp[j]._Detail + ".pdf"))
                                {
                                    if(ExecutorMails.Where(p=>p.Executor.Equals(SplitDataMatrix[3])).Count()!=0)
                                    {
                                        foreach (var item in SystemArgs.UnLoadSpecific.ExecutorMails)
                                        {
                                            if (SplitDataMatrix[3].Equals(item.Executor))
                                            {
                                                item._Specifics.Add(new Specific(SplitDataMatrix[0], Temp[j]._List, Convert.ToInt64(Temp[j]._Detail), true));
                                            }
                                        }
                                    }
                                    else
                                    {
                                        ExecutorMails.Add(new ExecutorMail(SplitDataMatrix[3]));
                                        ExecutorMails[ExecutorMails.Count() - 1]._Specifics.Add(new Specific(SplitDataMatrix[0], Temp[j]._List,Convert.ToInt64(Temp[j]._Detail),true));
                                    }
                                }
                                else
                                {
                                    if (ExecutorMails.Where(p => p.Executor.Equals(SplitDataMatrix[3])).Count() != 0)
                                    {
                                        foreach (var item in ExecutorMails)
                                        {
                                            if (SplitDataMatrix[3].Equals(item.Executor))
                                            {
                                                item._Specifics.Add(new Specific(SplitDataMatrix[0], Temp[j]._List, Convert.ToInt64(Temp[j]._Detail), false));
                                            }
                                        }
                                    }
                                    else
                                    {
                                        ExecutorMails.Add(new ExecutorMail(SplitDataMatrix[3]));
                                        ExecutorMails[ExecutorMails.Count() - 1]._Specifics.Add(new Specific(SplitDataMatrix[0], Temp[j]._List, Convert.ToInt64(Temp[j]._Detail), false));
                                    }
                                }
                            }
                        }
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
