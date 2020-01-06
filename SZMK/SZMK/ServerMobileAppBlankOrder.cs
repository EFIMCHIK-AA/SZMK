using System;
using SimpleTCP;
using System.Net;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;

namespace SZMK
{
    public class ServerMobileAppBlankOrder
    {
        SimpleTcpServer ServerTCP;
        public delegate void LoadData(List<BlankOrderScanSession> ScanSession);
        public event LoadData Load;
        public delegate void LoadStatus(String QRBlankOrder);
        public event LoadStatus Status;
        public List<BlankOrderScanSession> _ScanSession;

        public ServerMobileAppBlankOrder()
        {
            _ScanSession = new List<BlankOrderScanSession>();
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
            if (CheckedUniqueList(e))
            {
                try
                {
                    String[] ValidationDataMatrix = e.MessageString.Split('_');
                    if (ValidationDataMatrix.Length <= 3)
                    {
                        throw new Exception("В DataMatrix менее 6 полей");
                    }
                    FindedOrder(ValidationDataMatrix,e.MessageString);
                }
                catch (FormatException)
                {
                    MessageBox.Show("Неверный формат DataMatrix, лист должен быть целым числом", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception E)
                {
                    MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private bool CheckedUniqueList(SimpleTCP.Message e)
        {
            foreach (var item in _ScanSession)
            {
                if (item._QR.Equals(e.MessageString))
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
        private bool FindedOrder(String[] ValidationDataMatrix,String QRBlankOrder)
        {
            try
            {
                //Status?.Invoke(QRBlankOrder);
                for (int i = 4; i < ValidationDataMatrix.Length; i++)
                {
                    if (SystemArgs.Request.CheckedExistenceOrderAndStatus(ValidationDataMatrix[1], Convert.ToInt64(ValidationDataMatrix[i])))
                    {
                        _ScanSession.Add(new BlankOrderScanSession($"Заказ {ValidationDataMatrix[1]} Лист {ValidationDataMatrix[i]}", true, QRBlankOrder));
                    }
                    else
                    {
                        _ScanSession.Add(new BlankOrderScanSession($"Заказ {ValidationDataMatrix[1]} Лист {ValidationDataMatrix[i]}", false, QRBlankOrder));
                    }
                    Load?.Invoke(_ScanSession);
                }
                return true;
            }
            catch
            {
                throw new Exception("Ошибка определения чертежей в бланке заказа");
            }
        }
    }
}
