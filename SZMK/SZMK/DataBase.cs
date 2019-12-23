using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZMK
{
    public class DataBase
    {
        private String _Name;
        private String _Owner;
        private String _Port;
        private String _IP;
        private String _Password;

        public DataBase()
        {
            if (File.Exists(SystemArgs.Path.ConnectDBPath))
            {
                using (StreamReader sr = new StreamReader(File.Open(SystemArgs.Path.ConnectDBPath, FileMode.Open)))
                {
                    _Name = sr.ReadLine();
                    _Owner = sr.ReadLine();
                    _IP = sr.ReadLine();
                    _Port = sr.ReadLine();
                    _Password = Encryption.DecryptRSA(sr.ReadLine());
                }
            }
            else
            {
                throw new Exception("Файл конфигурации подключения к базе данных не найден");
            }

            CheckParam();
        }

        public String Name
        {
            get
            {
                return _Name;
            }

            set
            {
                if(!String.IsNullOrEmpty(value))
                {
                    _Name = value;
                    SetParamDB();
                }
            }
        }

        public String Owner
        {
            get
            {
                return _Owner;
            }

            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _Owner = value;
                    SetParamDB();
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
                    SetParamDB();
                }
            }
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
                    SetParamDB();
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
                    SetParamDB();
                }
            }
        }

        public void CheckParam()
        {
            if(String.IsNullOrEmpty(_Name) || _Name.Trim() != "SZMK")
            {
                throw new Exception("Недействительное наименование базы данных");
            }

            if (String.IsNullOrEmpty(_Owner) || _Name.Trim() != "postgres")
            {
                throw new Exception("Недействительный владелец базы данных");
            }

            if (String.IsNullOrEmpty(_IP))
            {
                throw new Exception("Недействительный адрес базы данных");
            }

            if (String.IsNullOrEmpty(_Port))
            {
                throw new Exception("Недействительный порт базы данных");
            }

            if (String.IsNullOrEmpty(_Password))
            {
                throw new Exception("Недействительный пароль базы данных");
            }
        }

        private void SetParamDB()
        {
            if (File.Exists(SystemArgs.Path.ConnectDBPath))
            {
                using (StreamWriter sw = new StreamWriter(File.Open(SystemArgs.Path.ConnectDBPath, FileMode.Create)))
                {
                    sw.WriteLine(_Name);
                    sw.WriteLine(_Owner);
                    sw.WriteLine(_IP);
                    sw.WriteLine(_Port);
                    sw.WriteLine(Encryption.EncryptRSA(_Password));
                }
            }
            else
            {
                throw new Exception("Файл конфигурации подключения к базе данных не найден");
            }
        }

        private void CheckConnect()
        {
            //Описать подключение к базе данных
        }

        public override string ToString()
        {
            return $@"Server = {_IP}; Port = {_Port}; User Id = {_Owner}; Password = {_Password}; Database = {_Name};";
        }
    }
}
