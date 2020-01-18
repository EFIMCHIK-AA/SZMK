﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZMK
{
    public class ClientProgram
    {
        private String _RegistryPath;
        private String _ArchivePath;
        private String _ModelsPath;
        private Boolean _CheckMarks;
        private (Int32, Int32) _VisualRow; // (n1,n2)

        public ClientProgram()
        {
            if(!GetParametersConnect())
            {
                throw new Exception("Ошибка при получении конфигурационных путей клиентского приложения");
            }
        }

        public bool GetParametersConnect()
        {
            try
            {
                if(!File.Exists(SystemArgs.Path.ArchivePath))
                {
                    throw new Exception();
                }

                if (!File.Exists(SystemArgs.Path.RegistryPath))
                {
                    throw new Exception();
                }

                if (!File.Exists(SystemArgs.Path.DirectoryModelsPath))
                {
                    throw new Exception();
                }

                if (!File.Exists(SystemArgs.Path.CheckMarksPath))
                {
                    throw new Exception();
                }

                using (StreamReader sr = new StreamReader(File.Open(SystemArgs.Path.ArchivePath, FileMode.Open)))
                {
                    _ArchivePath = sr.ReadLine();
                }

                using (StreamReader sr = new StreamReader(File.Open(SystemArgs.Path.RegistryPath, FileMode.Open)))
                {
                    _RegistryPath = sr.ReadLine();
                }

                using (StreamReader sr = new StreamReader(File.Open(SystemArgs.Path.DirectoryModelsPath, FileMode.Open)))
                {
                    _ModelsPath = sr.ReadLine();
                }

                using (StreamReader sr = new StreamReader(File.Open(SystemArgs.Path.CheckMarksPath, FileMode.Open)))
                {
                    String Temp = sr.ReadLine();

                    if(Temp.ToLower() != "true")
                    {
                        _CheckMarks = false;
                    }
                    else
                    {
                        _CheckMarks = true;
                    }
                }

                using (StreamReader sr = new StreamReader(File.Open(SystemArgs.Path.VisualRowPath, FileMode.Open)))
                {
                    _VisualRow.Item1 = Convert.ToInt32(sr.ReadLine()); // n1
                    _VisualRow.Item2 = Convert.ToInt32(sr.ReadLine()); // n2
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
                String DirArchive = SystemArgs.Path.GetDirectory(SystemArgs.Path.ArchivePath);

                if (!Directory.Exists(DirArchive))
                {
                    Directory.CreateDirectory(DirArchive);
                }

                String DirRegistry = SystemArgs.Path.GetDirectory(SystemArgs.Path.RegistryPath);

                if (!Directory.Exists(DirRegistry))
                {
                    Directory.CreateDirectory(DirRegistry);
                }

                String DirModels = SystemArgs.Path.GetDirectory(SystemArgs.Path.DirectoryModelsPath);

                if (!Directory.Exists(DirModels))
                {
                    Directory.CreateDirectory(DirModels);
                }

                String DirMarks = SystemArgs.Path.GetDirectory(SystemArgs.Path.CheckMarksPath);

                if (!Directory.Exists(DirMarks))
                {
                    Directory.CreateDirectory(DirMarks);
                }

                String DirVisualRow = SystemArgs.Path.GetDirectory(SystemArgs.Path.VisualRowPath);

                if (!Directory.Exists(DirVisualRow))
                {
                    Directory.CreateDirectory(DirVisualRow);
                }

                using (StreamWriter sw = new StreamWriter(File.Open(SystemArgs.Path.ArchivePath, FileMode.Create)))
                {
                    sw.WriteLine(_ArchivePath);
                }

                using (StreamWriter sw = new StreamWriter(File.Open(SystemArgs.Path.RegistryPath, FileMode.Create)))
                {
                    sw.WriteLine(_RegistryPath);
                }

                using (StreamWriter sw = new StreamWriter(File.Open(SystemArgs.Path.DirectoryModelsPath, FileMode.Create)))
                {
                    sw.WriteLine(_ModelsPath);
                }

                using (StreamWriter sw = new StreamWriter(File.Open(SystemArgs.Path.CheckMarksPath, FileMode.Create)))
                {
                    if(_CheckMarks)
                    {
                        sw.WriteLine("true");
                    }
                    else
                    {
                        sw.WriteLine("false");
                    }
                }

                using (StreamWriter sw = new StreamWriter(File.Open(SystemArgs.Path.VisualRowPath, FileMode.Create)))
                {
                    sw.WriteLine(_VisualRow.Item1);// n1
                    sw.WriteLine(_VisualRow.Item2);// n2
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
            if (!Directory.Exists(_ArchivePath))
            {
                return false;
            }

            if (!Directory.Exists(_RegistryPath))
            {
                return false;
            }

            if (!Directory.Exists(_ModelsPath))
            {
                return false;
            }

            return true;
        }

        public String ArchivePath
        {
            get
            {
                return _ArchivePath;
            }

            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _ArchivePath = value;
                }
            }
        }

        public String ModelsPath
        {
            get
            {
                return _ModelsPath;
            }

            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _ModelsPath = value;
                }
            }
        }

        public String RegistryPath
        {
            get
            {
                return _RegistryPath;
            }

            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _RegistryPath = value;
                }
            }
        }

        public Boolean CheckMarks
        {
            get
            {
                return _CheckMarks;
            }

            set
            {
                _CheckMarks = value;
            }
        }

        public Int32 VisualRow_N1
        {
            get
            {
                return _VisualRow.Item1;
            }

            set
            {
                _VisualRow.Item1 = value;
            }
        }

        public Int32 VisualRow_N2
        {
            get
            {
                return _VisualRow.Item2;
            }

            set
            {
                _VisualRow.Item2 = value;
            }
        }
    }
}
