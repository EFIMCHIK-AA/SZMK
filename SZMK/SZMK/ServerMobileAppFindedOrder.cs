using SimpleTCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SZMK
{
    public class ServerMobileAppFindedOrder
    {
        SimpleTcpServer ServerTCP;
        public delegate void SearchData(String DataMatrix);
        public event SearchData Search;
        public bool Start()
        {
            ServerTCP = new SimpleTcpServer
            {
                Delimiter = 0x13
            };
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

            Search?.Invoke(Temp);
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
