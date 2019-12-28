using System;
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
    }
}
