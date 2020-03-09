using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZMK
{
    public class AboutProgram
    {
        private String _Version;
        private DateTime _DateUpdate;
        private List<String> _DiscriptionsUpdate;

        public AboutProgram()
        {
            if (CheckFile())
            {
                _DiscriptionsUpdate = new List<string>();
                if (!GetInformations())
                {
                    throw new Exception("Ошибка при получении информации о программе");
                }
            }
            else
            {
                throw new Exception("Файл информации о программе не найден");
            }
        }
        public String Version
        {
            get
            {
                return _Version;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _Version = value;
                }
            }
        }
        public DateTime DateUpdate
        {
            get
            {
                return _DateUpdate;
            }
            set
            {
                if (value != null)
                {
                    _DateUpdate = value;
                }
            }
        }
        public String this[Int32 Index]
        {
            get
            {
                return _DiscriptionsUpdate[Index];
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _DiscriptionsUpdate[Index] = value;
                }
            }
        }
        public List<String> GetDiscriptionsUpdate()
        {
            return _DiscriptionsUpdate;
        }
        public bool GetInformations()
        {
            try
            {
                if (!File.Exists(SystemArgs.Path.AboutProgram))
                {
                    throw new Exception();
                }

                using (StreamReader sr = new StreamReader(File.Open(SystemArgs.Path.AboutProgram, FileMode.Open)))
                {
                    _Version = sr.ReadLine();
                    _DateUpdate = Convert.ToDateTime(sr.ReadLine());
                    for(int i = 0; !sr.EndOfStream; i++)
                    {
                        _DiscriptionsUpdate.Add(sr.ReadLine());
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool SetInformations()
        {
            try
            {
                String Dir = SystemArgs.Path.GetDirectory(SystemArgs.Path.AboutProgram);

                if (!Directory.Exists(Dir))
                {
                    Directory.CreateDirectory(Dir);
                }

                using (StreamWriter sw = new StreamWriter(File.Open(SystemArgs.Path.AboutProgram, FileMode.Create)))
                {
                    sw.WriteLine(_Version);
                    sw.WriteLine(_DateUpdate.ToString());
                    for(int i =0; i<_DiscriptionsUpdate.Count(); i++)
                    {
                        sw.WriteLine(_DiscriptionsUpdate[i]);
                    }
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
            if (!File.Exists(SystemArgs.Path.AboutProgram))
            {
                return false;
            }

            return true;
        }
    }
}
