using System;
using SimpleTCP;
using System.Net;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;
using System.Linq;

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
        public List<BlankOrderScanSession> _ScanSession;

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
            try
            {
                String Temp = e.MessageString.Replace("\u00a0", "").Replace(" ","");
                String[] ValidationDataMatrix = Temp.Split('_');
                if (CheckedUniqueList(Temp))
                {
                    if (ValidationDataMatrix.Length <= 3)
                    {
                        throw new Exception("В QR менее 3 полей");
                    }
                    if (!FindedOrder(ValidationDataMatrix, Temp))
                    {
                        throw new Exception("Ошибка определения чертежей в бланке заказа");
                    }
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
        private bool FindedOrder(String[] ValidationDataMatrix, String QRBlankOrder)
        {
            try
            {
                _ScanSession.Add(new BlankOrderScanSession(true, QRBlankOrder));
                ValidationDataMatrix[1] = ReplaceNumber(ValidationDataMatrix[1]);
                Status?.Invoke(QRBlankOrder);
                for (int i = 4; i < ValidationDataMatrix.Length; i++)
                {
                    if (Added)
                    {
                        if (SystemArgs.RequestLinq.CheckedOrderAndStatus(ValidationDataMatrix[1], ValidationDataMatrix[i]))
                        {
                            _ScanSession[_ScanSession.Count - 1]._Order.Add(new BlankOrderScanSession.NumberAndList(ValidationDataMatrix[1], ValidationDataMatrix[i], 1));
                        }
                        else if (SystemArgs.RequestLinq.CheckedOrderAndStatusForUpdate(ValidationDataMatrix[1], ValidationDataMatrix[i]))
                        {
                            _ScanSession[_ScanSession.Count - 1]._Order.Add(new BlankOrderScanSession.NumberAndList(ValidationDataMatrix[1], ValidationDataMatrix[i], 0));
                        }
                        else
                        {
                            _ScanSession[_ScanSession.Count - 1]._Order.Add(new BlankOrderScanSession.NumberAndList(ValidationDataMatrix[1], ValidationDataMatrix[i], -1));
                        }
                    }
                    else
                    {
                        if (SystemArgs.RequestLinq.CheckedOrderAndStatus(ValidationDataMatrix[1], ValidationDataMatrix[i]) && SystemArgs.RequestLinq.FindedOrdersInAddBlankOrder(QRBlankOrder, ValidationDataMatrix[1], ValidationDataMatrix[i]))
                        {
                            _ScanSession[_ScanSession.Count - 1]._Order.Add(new BlankOrderScanSession.NumberAndList(ValidationDataMatrix[1], ValidationDataMatrix[i], 1));
                        }
                        else if (SystemArgs.RequestLinq.CheckedOrderAndStatusForUpdate(ValidationDataMatrix[1], ValidationDataMatrix[i]) && SystemArgs.RequestLinq.FindedOrdersInAddBlankOrder(QRBlankOrder, ValidationDataMatrix[1], ValidationDataMatrix[i]))
                        {
                            _ScanSession[_ScanSession.Count - 1]._Order.Add(new BlankOrderScanSession.NumberAndList(ValidationDataMatrix[1], ValidationDataMatrix[i], 0));
                        }
                        else
                        {
                            _ScanSession[_ScanSession.Count - 1]._Order.Add(new BlankOrderScanSession.NumberAndList(ValidationDataMatrix[1], ValidationDataMatrix[i], -1));
                        }
                    }

                    if ((_ScanSession[_ScanSession.Count - 1]._Order.Where(p => p._Finded == -1).Count() == 0) && (_ScanSession[_ScanSession.Count - 1]._Order.Where(p => p._Finded == 1).Count() > 0))
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
