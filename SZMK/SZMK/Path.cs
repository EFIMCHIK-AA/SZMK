using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZMK
{
    public class Path
    {
        private readonly String _ConnectDBPath;
        private readonly String _ConnectApplicationPath;
        private readonly String _ArchivePath;
        private readonly String _RegistryPath;
        private readonly String _LogPath;
        private readonly String _ProgramPath;
        private readonly String _DirectoryProgramPath;

        public Path() 
        {
            _ConnectDBPath = $@"Connect\DataBase\connect.conf"; //Параметры подключения к базе данных
            _ConnectApplicationPath = $@"Connect\Application\connect.conf"; //Параметры подключения к приложению
            _ArchivePath = $@"Path\Archive.df";//Путь к архиву
            _RegistryPath = $@"Path\Registry.df";//Путь к реестру
            _ProgramPath = $@"Path\Program\Program.df";//Путь к программе распознавания
            _LogPath = $@"Log";//Путь к директории хранения логов
            _DirectoryProgramPath = $@"Path\Program\Directory.df";//Путь к директории темповых файлов для распознвания
        }

        public String DirectoryProgramPath
        {
            get
            {
                return _DirectoryProgramPath;
            }
        }

        public String ProgramPath
        {
            get
            {
                return _ProgramPath;
            }
        }

        public String ConnectDBPath
        {
            get
            {
                return _ConnectDBPath;
            }
        }

        public String LogPath
        {
            get
            {
                return _LogPath;
            }
        }

        public String ConnectApplicationPath
        {
            get
            {
                return _ConnectApplicationPath;
            }
        }

        public String ArchivePath
        {
            get
            {
                return _ArchivePath;
            }
        }

        public String RegistryPath
        {
            get
            {
                return _RegistryPath;
            }
        }
    }
}
