using System;
using SimpleTCP;
using Npgsql;
using System.Net;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZMK
{
    public class ServerMobileApp
    {
        SimpleTcpServer ServerTCP;
        public delegate void LoadData (List<ScanSession> ScanSession);
        public event LoadData  Load;
        public List<ScanSession> _ScanSession;

        public ServerMobileApp()
        {
            _ScanSession = new List<ScanSession>();
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
                if (SystemArgs.Request.CheckedUniqueOrderDB(e.MessageString))
                {
                    _ScanSession.Add(new ScanSession(e.MessageString, true));

                }
                else
                {
                    _ScanSession.Add(new ScanSession(e.MessageString,false));
                }
                Load?.Invoke(_ScanSession);
            }
        }
        private bool CheckedUniqueList(SimpleTCP.Message e)
        {
            foreach (var item in _ScanSession)
            {
                if (item._DateMatrix.Equals(e.MessageString))
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
