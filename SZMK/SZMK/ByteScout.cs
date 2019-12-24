using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZMK
{
    public class ByteScout
    {
        private String _ProgramPath;
        private String _DirectoryProgramPath;

        public ByteScout()
        {
            if (!GetParametersConnect())
            {
                throw new Exception("Ошибка при получении конфигурационных путей программы распознавания");
            }
        }

        public bool GetParametersConnect()
        {
            try
            {
                if (!File.Exists(SystemArgs.Path.ProgramPath))
                {
                    throw new Exception();
                }

                if (!File.Exists(SystemArgs.Path.DirectoryProgramPath))
                {
                    throw new Exception();
                }

                using (StreamReader sr = new StreamReader(File.Open(SystemArgs.Path.ProgramPath, FileMode.Open)))
                {
                    _ProgramPath = sr.ReadLine();
                }

                using (StreamReader sr = new StreamReader(File.Open(SystemArgs.Path.DirectoryProgramPath, FileMode.Open)))
                {
                    _DirectoryProgramPath = sr.ReadLine();
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
                String DirProg = SystemArgs.Path.GetDirectory(SystemArgs.Path.ProgramPath);

                if (!Directory.Exists(DirProg))
                {
                    Directory.CreateDirectory(DirProg);
                }

                String DirTempDirProgram = SystemArgs.Path.GetDirectory(SystemArgs.Path.DirectoryProgramPath);

                if (!Directory.Exists(DirTempDirProgram))
                {
                    Directory.CreateDirectory(DirTempDirProgram);
                }

                using (StreamWriter sw = new StreamWriter(File.Open(SystemArgs.Path.ProgramPath, FileMode.Create)))
                {
                    sw.WriteLine(_ProgramPath);
                }

                using (StreamWriter sw = new StreamWriter(File.Open(SystemArgs.Path.DirectoryProgramPath, FileMode.Create)))
                {
                    sw.WriteLine(_DirectoryProgramPath);
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
            if (!File.Exists(_ProgramPath))
            {
                return false;
            }

            if (!File.Exists(_DirectoryProgramPath))
            {
                return false;
            }

            return true;
        }

        public String ProgramPath
        {
            get
            {
                return _ProgramPath;
            }

            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _ProgramPath = value;
                }
            }
        }

        public String DirectoryProgramPath
        {
            get
            {
                return _DirectoryProgramPath;
            }

            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _DirectoryProgramPath = value;
                }
            }
        }
    }
}
