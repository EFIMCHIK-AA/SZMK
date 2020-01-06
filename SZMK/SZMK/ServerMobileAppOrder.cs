using System;
using SimpleTCP;
using System.Net;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;

namespace SZMK
{
    public class ServerMobileAppOrder
    {
        SimpleTcpServer ServerTCP;
        public delegate void LoadData (List<OrderScanSession> ScanSession);
        public event LoadData  Load;
        public List<OrderScanSession> _ScanSession;

        public ServerMobileAppOrder()
        {
            _ScanSession = new List<OrderScanSession>();
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
            if(ServerTCP.IsStarted)
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
                    if (ValidationDataMatrix.Length != 6)
                    {
                        throw new Exception("В DataMatrix менее 6 полей");
                    }
                    Int32 List = Convert.ToInt32(ValidationDataMatrix[1]);
                    if (SystemArgs.Request.CheckedUniqueOrderDB(e.MessageString))
                    {
                        _ScanSession.Add(new OrderScanSession(e.MessageString, true));

                    }
                    else
                    {
                        _ScanSession.Add(new OrderScanSession(e.MessageString, false));
                    }
                    Load?.Invoke(_ScanSession);
                }
                catch(FormatException)
                {
                    MessageBox.Show("Неверный формат DataMatrix, лист должен быть целым числом", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch(Exception E)
                {
                    MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private bool CheckedUniqueList(SimpleTCP.Message e)
        {
            foreach (var item in _ScanSession)
            {
                if (item.DataMatrix.Equals(e.MessageString))
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
    }
}
