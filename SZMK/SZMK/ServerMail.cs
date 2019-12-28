using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZMK
{
    public class ServerMail
    {
        private String _NameWho;
        private String _SMTP;
        private String _Port;
        private String _Name;
        private String _Login;
        private String _Password;
        private bool _SSL;

        public ServerMail()
        {
            if (CheckFile())
            {
                if (!GetParametersConnect())
                {
                    throw new Exception("Ошибка при получении параметров подключения к почтовому серверу");
                }
            }
            else
            {
                throw new Exception("Файл подключения к почтовому серверу не найден");
            }
        }

        public bool SSL
        {
            get
            {
                return _SSL;
            }

            set
            {
                _SSL = value;
            }
        }

        public String Login
        {
            get
            {
                return _Login;
            }

            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _Login = value;
                }
            }
        }

        public String SMTP
        {
            get
            {
                return _SMTP;
            }

            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _SMTP = value;
                }
            }
        }

        public String NameWho
        {
            get
            {
                return _NameWho;
            }

            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _NameWho = value;
                }
            }
        }

        public String Name
        {
            get
            {
                return _Name;
            }

            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _Name = value;
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

        public String Password
        {
            get
            {
                return _Password;
            }

            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _Password = value;
                }
            }
        }

        public bool GetParametersConnect()
        {
            try
            {
                if (!File.Exists(SystemArgs.Path.ConnectServreMails))
                {
                    throw new Exception();
                }

                using (StreamReader sr = new StreamReader(File.Open(SystemArgs.Path.ConnectServreMails, FileMode.Open)))
                {
                    _NameWho = sr.ReadLine();
                    _SMTP = sr.ReadLine();
                    _Port = sr.ReadLine();
                    _Name = sr.ReadLine();
                    _Login = sr.ReadLine();
                    String SSL = sr.ReadLine();

                    if(SSL.ToLower() == "true")
                    {
                        _SSL = true;
                    }
                    else
                    {
                        _SSL = false;
                    }

                    _Password = Encryption.DecryptRSA(sr.ReadLine());
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool SetParametersConnect()
        {
            try
            {
                String Dir = SystemArgs.Path.GetDirectory(SystemArgs.Path.ConnectServreMails);

                if (!Directory.Exists(Dir))
                {
                    Directory.CreateDirectory(Dir);
                }

                using (StreamWriter sw = new StreamWriter(File.Open(SystemArgs.Path.ConnectServreMails, FileMode.Create)))
                {
                    sw.WriteLine(_NameWho);
                    sw.WriteLine(_SMTP);
                    sw.WriteLine(_Port);
                    sw.WriteLine(_Name);
                    sw.WriteLine(_Login);

                    if(_SSL)
                    {
                        sw.WriteLine("true");
                    }
                    else
                    {
                        sw.WriteLine("false");
                    }

                    sw.WriteLine(Encryption.EncryptRSA(_Password));
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool CheckFile()
        {
            if (!File.Exists(SystemArgs.Path.ConnectServreMails))
            {
                return false;
            }

            return true;
        }

        public bool CheckConnect(String ConnectString)
        {
            return false; // Проверку подключения к севреру
        }
    }
}
