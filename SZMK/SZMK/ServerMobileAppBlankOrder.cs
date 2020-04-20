using System;
using SimpleTCP;
using System.Net;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;

namespace SZMK
{
    /*Данный класс описывает серверную часть для получения данных после сканирования бланка заказа, 
     * также в нем реализованы проверки на уникальность полученных данных и вызываются проверки для определения нахождения чертежа или его отсутсвия*/
    public class ServerMobileAppBlankOrder
    {
        SimpleTcpServer ServerTCP;
        private Boolean _Added;
        public delegate void LoadData(List<BlankOrderScanSession> ScanSession);
        public event LoadData Load;
        public delegate void LoadStatus(String QRBlankOrder);
        public event LoadStatus Status;
        private List<BlankOrderScanSession> _ScanSession;

        public ServerMobileAppBlankOrder(Boolean Added)
        {
            _Added = Added;
            _ScanSession = new List<BlankOrderScanSession>();
        }
        public Boolean Added
        {
            get
            {
                return _Added;
            }
            set
            {
                _Added = value;
            }
        }
        public BlankOrderScanSession this[Int32 Index]
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
        public List<BlankOrderScanSession> GetScanSessions()
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
            IPAddress ip = IPAddress.Parse(Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString());
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
            try
            {
                String Temp = e.MessageString.Replace("\u00a0", "").Replace(" ","");
                String[] ValidationDataMatrix = Temp.Split('_');
                if(ValidationDataMatrix.Length >= 4)
                {
                    Regex regex = new Regex(@"\d*-\d*-\d*");
                    MatchCollection matches = regex.Matches(ValidationDataMatrix[1]);

                    if (matches.Count > 0)
                    {
                        Temp = ValidationDataMatrix[0] + "_СЗМК";
                        for (int i = 1; i < ValidationDataMatrix.Length; i++)
                        {
                            Temp += "_" + ValidationDataMatrix[i];
                        }
                        ValidationDataMatrix = Temp.Split('_');
                    }

                    if (CheckedUniqueList(Temp))
                    {
                        _ScanSession.Add(new BlankOrderScanSession(true, Temp));

                        if (Added)
                        {
                            if (ValidationDataMatrix[0] != "ПО")
                            {
                                if (ValidationDataMatrix[1] != "СЗМК")
                                {
                                    if (!FindedOrderBlankProvider(ValidationDataMatrix, Temp))
                                    {
                                        throw new Exception("Ошибка определения чертежей в бланке поставщику");
                                    }
                                }
                                else
                                {
                                    if (!FindedOrderBlankOrder(ValidationDataMatrix, Temp))
                                    {
                                        throw new Exception("Ошибка определения чертежей в бланке заказа");
                                    }
                                }
                            }
                            else
                            {
                                if (!FindedOrderCreditOrder(ValidationDataMatrix, Temp))
                                {
                                    throw new Exception("Ошибка определения чертежей в приходном ордере");
                                }
                            }
                        }
                        else
                        {
                            if (!FindedBlankOrder_OPP(ValidationDataMatrix, Temp))
                            {
                                throw new Exception("Ошибка определения чертежей в бланке заказа");
                            }
                        }
                    }
                }
                else
                {
                    throw new Exception("В QR менее 4 полей");
                }

            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private bool CheckedUniqueList(String Temp)
        {
            foreach (var item in _ScanSession)
            {
                if (item.QRBlankOrder.Equals(Temp))
                {
                    return false;
                }
            }
            return true;
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
        private bool FindedOrderCreditOrder(String[] ValidationDataMatrix, String QRBlankOrder)
        {
            try
            {
                ValidationDataMatrix[2] = ReplaceNumber(ValidationDataMatrix[2]);
                Status?.Invoke(QRBlankOrder);
                for (int i = 5; i < ValidationDataMatrix.Length; i++)
                {
                    if (SystemArgs.Request.CheckedOrder(ValidationDataMatrix[2], ValidationDataMatrix[i]) && SystemArgs.Request.CheckedStatusOrderDB(ValidationDataMatrix[2], ValidationDataMatrix[i]) == SystemArgs.User.StatusesUser[0].ID && SystemArgs.Request.CheckedExecutorWork(ValidationDataMatrix[2],ValidationDataMatrix[i],QRBlankOrder))
                    {
                        _ScanSession[_ScanSession.Count - 1].GetNumberAndLists().Add(new BlankOrderScanSession.NumberAndList(ValidationDataMatrix[2], ValidationDataMatrix[i], 1));
                    }
                    else if (SystemArgs.Request.CheckedStatusOrderDB(ValidationDataMatrix[2], ValidationDataMatrix[i]) >= SystemArgs.User.StatusesUser[1].ID && SystemArgs.Request.CheckedExecutorWork(ValidationDataMatrix[2], ValidationDataMatrix[i], QRBlankOrder))
                    {
                        _ScanSession[_ScanSession.Count - 1].GetNumberAndLists().Add(new BlankOrderScanSession.NumberAndList(ValidationDataMatrix[2], ValidationDataMatrix[i], 0));
                    }
                    else
                    {
                        _ScanSession[_ScanSession.Count - 1].GetNumberAndLists().Add(new BlankOrderScanSession.NumberAndList(ValidationDataMatrix[2], ValidationDataMatrix[i], -1));
                    }

                    if ((_ScanSession[_ScanSession.Count - 1].GetNumberAndLists().Where(p => p.Finded == -1).Count() == 0) && (_ScanSession[_ScanSession.Count - 1].GetNumberAndLists().Where(p => p.Finded == 0).Count() == 0))
                    {
                        _ScanSession[_ScanSession.Count - 1].Added = true;
                    }
                    else
                    {
                        _ScanSession[_ScanSession.Count - 1].Added = false;
                    }
                }
                Load?.Invoke(_ScanSession);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool FindedOrderBlankProvider(String[] ValidationDataMatrix, String QRBlankOrder)
        {
            try
            {
                ValidationDataMatrix[2] = ReplaceNumber(ValidationDataMatrix[2]);
                Status?.Invoke(QRBlankOrder);
                for (int i = 5; i < ValidationDataMatrix.Length; i++)
                {
                    if (SystemArgs.Request.CheckedOrder(ValidationDataMatrix[2], ValidationDataMatrix[i]) && (SystemArgs.Request.CheckedStatusOrderDB(ValidationDataMatrix[2], ValidationDataMatrix[i]) == SystemArgs.User.StatusesUser[0].ID - 1 || SystemArgs.Request.CheckedStatusOrderDB(ValidationDataMatrix[2], ValidationDataMatrix[i]) == SystemArgs.User.StatusesUser[1].ID))
                    {
                        _ScanSession[_ScanSession.Count - 1].GetNumberAndLists().Add(new BlankOrderScanSession.NumberAndList(ValidationDataMatrix[2], ValidationDataMatrix[i], 1));
                    }
                    else if (SystemArgs.Request.CheckedStatusOrderDB(ValidationDataMatrix[2], ValidationDataMatrix[i]) >= SystemArgs.User.StatusesUser[0].ID)
                    {
                        _ScanSession[_ScanSession.Count - 1].GetNumberAndLists().Add(new BlankOrderScanSession.NumberAndList(ValidationDataMatrix[2], ValidationDataMatrix[i], 0));
                    }
                    else
                    {
                        _ScanSession[_ScanSession.Count - 1].GetNumberAndLists().Add(new BlankOrderScanSession.NumberAndList(ValidationDataMatrix[2], ValidationDataMatrix[i], -1));
                    }

                    if ((_ScanSession[_ScanSession.Count - 1].GetNumberAndLists().Where(p => p.Finded == -1).Count() == 0) && (_ScanSession[_ScanSession.Count - 1].GetNumberAndLists().Where(p => p.Finded == 0).Count() == 0))
                    {
                        _ScanSession[_ScanSession.Count - 1].Added = true;
                    }
                    else
                    {
                        _ScanSession[_ScanSession.Count - 1].Added = false;
                    }
                }
                Load?.Invoke(_ScanSession);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool FindedOrderBlankOrder(String[] ValidationDataMatrix, String QRBlankOrder)
        {
            try
            {
                ValidationDataMatrix[2] = ReplaceNumber(ValidationDataMatrix[2]);
                Status?.Invoke(QRBlankOrder);

                for (int i = 5; i < ValidationDataMatrix.Length; i++)
                {
                    if (SystemArgs.Request.CheckedOrder(ValidationDataMatrix[2], ValidationDataMatrix[i]) && (SystemArgs.Request.CheckedStatusOrderDB(ValidationDataMatrix[2], ValidationDataMatrix[i]) == SystemArgs.User.StatusesUser[0].ID - 1 || SystemArgs.Request.CheckedStatusOrderDB(ValidationDataMatrix[2], ValidationDataMatrix[i]) == SystemArgs.User.StatusesUser[1].ID))
                    {
                        _ScanSession[_ScanSession.Count - 1].GetNumberAndLists().Add(new BlankOrderScanSession.NumberAndList(ValidationDataMatrix[2], ValidationDataMatrix[i], 1));
                    }
                    else if (SystemArgs.Request.CheckedStatusOrderDB(ValidationDataMatrix[2], ValidationDataMatrix[i]) >= SystemArgs.User.StatusesUser[2].ID)
                    {
                        _ScanSession[_ScanSession.Count - 1].GetNumberAndLists().Add(new BlankOrderScanSession.NumberAndList(ValidationDataMatrix[2], ValidationDataMatrix[i], 0));
                    }
                    else
                    {
                        _ScanSession[_ScanSession.Count - 1].GetNumberAndLists().Add(new BlankOrderScanSession.NumberAndList(ValidationDataMatrix[2], ValidationDataMatrix[i], -1));
                    }

                    if ((_ScanSession[_ScanSession.Count - 1].GetNumberAndLists().Where(p => p.Finded == -1).Count() == 0))
                    {
                        _ScanSession[_ScanSession.Count - 1].Added = true;
                    }
                    else
                    {
                        _ScanSession[_ScanSession.Count - 1].Added = false;
                    }
                }
                Load?.Invoke(_ScanSession);
                return true;
            }
            catch
            {
                return false;
            }
        }
        private bool FindedBlankOrder_OPP(String[] ValidationDataMatrix, String QRBlankOrder)
        {
            try
            {
                ValidationDataMatrix[2] = ReplaceNumber(ValidationDataMatrix[2]);
                Status?.Invoke(QRBlankOrder);

                for (int i = 5; i < ValidationDataMatrix.Length; i++)
                {
                    if (SystemArgs.Request.FindedOrdersInAddBlankOrder(QRBlankOrder, ValidationDataMatrix[2], ValidationDataMatrix[i]))
                    {
                        if (SystemArgs.Request.CheckedOrder(ValidationDataMatrix[2], ValidationDataMatrix[i]) && SystemArgs.Request.CheckedStatusOrderDB(ValidationDataMatrix[2], ValidationDataMatrix[i]) == SystemArgs.User.StatusesUser[0].ID - 1)
                        {
                            _ScanSession[_ScanSession.Count - 1].GetNumberAndLists().Add(new BlankOrderScanSession.NumberAndList(ValidationDataMatrix[2], ValidationDataMatrix[i], 1));
                        }
                        else if (SystemArgs.Request.CheckedStatusOrderDB(ValidationDataMatrix[2], ValidationDataMatrix[i]) > SystemArgs.User.StatusesUser[0].ID - 1)
                        {
                            _ScanSession[_ScanSession.Count - 1].GetNumberAndLists().Add(new BlankOrderScanSession.NumberAndList(ValidationDataMatrix[2], ValidationDataMatrix[i], 0));
                        }
                        else
                        {
                            _ScanSession[_ScanSession.Count - 1].GetNumberAndLists().Add(new BlankOrderScanSession.NumberAndList(ValidationDataMatrix[2], ValidationDataMatrix[i], -1));
                        }
                    }
                    else
                    {
                        _ScanSession[_ScanSession.Count - 1].GetNumberAndLists().Add(new BlankOrderScanSession.NumberAndList(ValidationDataMatrix[2], ValidationDataMatrix[i], -1));
                    }

                    if ((_ScanSession[_ScanSession.Count - 1].GetNumberAndLists().Where(p => p.Finded == -1).Count() == 0) && (_ScanSession[_ScanSession.Count - 1].GetNumberAndLists().Where(p => p.Finded == 1).Count() > 0))
                    {
                        _ScanSession[_ScanSession.Count - 1].Added = true;
                    }
                    else
                    {
                        _ScanSession[_ScanSession.Count - 1].Added = false;
                    }
                }
                Load?.Invoke(_ScanSession);
                return true;
            }
            catch
            {
                return false;
            }
        }
        private String ReplaceNumber(String Number)
        {
            Number = Number.Remove(Number.IndexOf('-'), Number.Remove(0, Number.IndexOf('-') + 1).IndexOf('-') + 1);//Удаляю подстроку с шаблоном -*- отставляя полседнее -
            Number = Number.Replace('-', '(') + ")";//Заменяю - на скобочку ( и добавляю )
            return Number;
        }
    }
}
