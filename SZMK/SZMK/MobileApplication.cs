using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZMK
{
    public class MobileApplication
    {
        String _Port;
        String _IP;

        public MobileApplication()
        {
            if(!GetParametersConnect())
            {
                throw new Exception("Ошибка при получении параметров подключения к мобильному приложению");
            }
        }

        public bool GetParametersConnect()
        {
            try
            {
                if(!File.Exists(SystemArgs.Path.ConnectApplicationPath))
                {
                    throw new Exception();
                }

                using (StreamReader sr = new StreamReader(File.Open(SystemArgs.Path.ConnectApplicationPath, FileMode.Open)))
                {
                    _Port = sr.ReadLine();
                    _IP = sr.ReadLine();
                }

                return true;
            }
            catch(Exception)
            {
                return false;  
            }
        }

        public bool SetParametersConnect()
        {
            try
            {
                String Dir = SystemArgs.Path.GetDirectory(SystemArgs.Path.ConnectApplicationPath);

                if (!Directory.Exists(Dir))
                {
                    Directory.CreateDirectory(Dir);
                }

                using (StreamWriter sw = new StreamWriter(File.Open(SystemArgs.Path.ConnectApplicationPath, FileMode.Create)))
                {
                    sw.WriteLine(_Port);
                    sw.WriteLine(_IP);
                }

                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        public bool CheckFile()
        {
            if (!File.Exists(SystemArgs.Path.ConnectApplicationPath))
            {
                return false;
            }

            return true;
        }

        public String IP
        {
            get
            {
                return _IP;
            }

            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _IP = value;
                }
            }
        }

        public String Port
        {
            get
            {
                return _Port;
            }

            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _Port = value;
                }
            }
        }
    }
}
