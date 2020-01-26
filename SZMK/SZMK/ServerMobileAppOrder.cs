using System;
using SimpleTCP;
using System.Net;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;

namespace SZMK
{
    /*Данный класс описывает сервер для получения данных от клиентского мобильного приложения после сканирования чертежей, 
     а также реализует проверки на уникальность полученных данных, замену одинаковопишущихся букв на английский алфавит, проверки на формат полученных данных*/
    public class ServerMobileAppOrder
    {
        SimpleTcpServer ServerTCP;
        public delegate void LoadData(List<OrderScanSession> ScanSession);
        public event LoadData Load;
        private List<OrderScanSession> _ScanSession;

        public ServerMobileAppOrder()
        {
            _ScanSession = new List<OrderScanSession>();
        }
        public OrderScanSession this[Int32 Index]
        {
            get
            {
                return _ScanSession[Index];
            }
            set
            {
                if (value != null)
                {
                    _ScanSession[Index] = value;
                }
            }
        }
        public List<OrderScanSession> GetScanSessions()
        {
            return _ScanSession;
        }
        public void ClearData()
        {
            _ScanSession.Clear();
        }
        public bool Start()
        {
            ServerTCP = new SimpleTcpServer();
            ServerTCP.Delimiter = 0x13;
            ServerTCP.DataReceived += Server_DataReceived;
            ServerTCP.StringEncoder = Encoding.UTF8;
            IPAddress ip = IPAddress.Parse(Dns.GetHostAddresses(Dns.GetHostName())[0].ToString());
            ServerTCP.Start(ip, Convert.ToInt32(SystemArgs.MobileApplication.Port));
            if (ServerTCP.IsStarted)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void Server_DataReceived(object sender, SimpleTCP.Message e)
        {
            if (CheckedUniqueList(e.MessageString))
            {
                try
                {
                    String Temp = e.MessageString.Replace(" ", "");
                    String ReplaceMark = "";

                    String[] ValidationDataMatrix = Temp.Split('_');
                    String[] ExistingCharaterEnglish = new String[] { "A", "a", "B", "C", "c", "E", "e", "H", "K", "M", "O", "o", "P", "p", "T" };
                    String[] ExistingCharaterRussia = new String[] { "А", "а", "В", "С", "с", "Е", "е", "Н", "К", "М", "О", "о", "Р", "р", "Т" };

                    if (ValidationDataMatrix.Length != 6)
                    {
                        throw new Exception("В DataMatrix менее 6 полей");
                    }

                    for (int i = 0; i < ExistingCharaterRussia.Length; i++)
                    {
                        ReplaceMark = ValidationDataMatrix[2].Replace(ExistingCharaterRussia[i], ExistingCharaterEnglish[i]);
                    }

                    Temp = ValidationDataMatrix[0] + "_" + ValidationDataMatrix[1] + "_" + ReplaceMark + "_" + ValidationDataMatrix[3] + "_" + ValidationDataMatrix[4].Replace(".", ",") + "_" + ValidationDataMatrix[5].Replace(".", ",");

                    Int32 IndexException = SystemArgs.RequestLinq.CheckedNumberAndList(ValidationDataMatrix[0], ValidationDataMatrix[1]);

                    switch (IndexException)
                    {
                        case 0:
                            if (SystemArgs.RequestLinq.CheckedNumberAndMark(ValidationDataMatrix[0], ReplaceMark))
                            {
                                if (SystemArgs.ClientProgram.CheckMarks)
                                {
                                    if (CheckedLowerRegistery(ReplaceMark))
                                    {
                                        _ScanSession.Add(new OrderScanSession(Temp, true));
                                    }
                                    else
                                    {
                                        _ScanSession.Add(new OrderScanSession(Temp, false));
                                        MessageBox.Show($"Наименование марки «{ReplaceMark}» не допускается", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                                else
                                {
                                    _ScanSession.Add(new OrderScanSession(Temp, true));
                                }
                            }
                            else
                            {
                                _ScanSession.Add(new OrderScanSession(Temp, false));
                                MessageBox.Show($"В заказе {ValidationDataMatrix[0]}, марка {ReplaceMark} уже существует. Чертеж не добавлен.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }

                            Load?.Invoke(_ScanSession);
                            break;
                        case 1:
                            _ScanSession.Add(new OrderScanSession(Temp, false));
                            MessageBox.Show($"В заказе {ValidationDataMatrix[0]}, номер листа {ValidationDataMatrix[1]} уже существует. Чертеж не добавлен.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Load?.Invoke(_ScanSession);
                            break;
                        case 2:
                            if (SystemArgs.ClientProgram.CheckMarks)
                            {
                                if (CheckedLowerRegistery(ReplaceMark))
                                {
                                    _ScanSession.Add(new OrderScanSession(Temp, true));
                                }
                                else
                                {
                                    _ScanSession.Add(new OrderScanSession(Temp, false));
                                    MessageBox.Show($"Наименование марки «{ReplaceMark}» не допускается", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                _ScanSession.Add(new OrderScanSession(Temp, true));
                            }

                            Load?.Invoke(_ScanSession);
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception E)
                {
                    MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private bool CheckedLowerRegistery(String Mark)
        {
            String LowerCharacters = "абвгдеёжзийклмнопрстуфхцчшщъыьэюяabcdefghijklmnopqrstuvwxyz";
            for (int i = 0; i < Mark.Length; i++)
            {
                if (LowerCharacters.IndexOf(Mark[i])!=-1)
                {
                    return false;
                }
            }
            return true;

        }
        List<String> UniqueList = new List<string>();
        private bool CheckedUniqueList(String Temp)
        {
            if (UniqueList.Contains(Temp))
            {
                return false;
            }
            else
            {
                UniqueList.Add(Temp);
                return true;
            }
        }
        public bool Stop()
        {
            if (ServerTCP != null)
            {
                if (ServerTCP.IsStarted)
                {
                    ServerTCP.Stop();
                }
            }
            if (!ServerTCP.IsStarted)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
