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
        private readonly String _DirectoryModelsPath;
        private readonly String _ConnectServerMails;
        private readonly String _TemplateActUniquePath;
        private readonly String _TemplateActNoUniquePath;
        private readonly String _TemplateReportOrderOfDatePath;
        private readonly String _TemplateRegistryPath;
        private readonly String _TestFileApplicationPath;
        private readonly String _ConnectProgramPath;


        public Path() 
        {
            _ConnectDBPath = $@"Connect\DataBase\connect.conf"; //Параметры подключения к базе данных
            _ConnectApplicationPath = $@"Connect\Application\connect.conf"; //Параметры подключения к приложению
            _ConnectProgramPath = $@"Path\Program\connect.df"; //Параметры подключения к программе распознавания
            _TestFileApplicationPath = $@"Connect\Application\test.png";//Путь к тестовому файлу для проверки подключения к программе по распознованию
            _ArchivePath = $@"Path\Archive.df";//Путь к архиву
            _RegistryPath = $@"Path\Registry.df";//Путь к реестру
            _ProgramPath = $@"Path\Program\Program.df";//Путь к программе распознавания
            _DirectoryProgramPath = $@"Path\Program\Directory.df";//Путь к директории темповых файлов для распознвания
            _LogPath = $@"Log"; //Путь к директории хранения логов
            _DirectoryModelsPath = $@"Path\Models.df"; //Директория выгрузки
            _ConnectServerMails = $@"Connect\Mails\connect.conf"; //Конфигурация почтового сервера
            _TemplateActUniquePath = $@"Templates\ActTemplateUnique.xlsx";//Путь до шаблона акта уникальных чертежей
            _TemplateActNoUniquePath = $@"Templates\ActTemplateNoUnique.xlsx";//Путь до шаблона акта не уникальных чертежей
            _TemplateReportOrderOfDatePath = $@"Templates\ReportOrderOfDateTemplate.xlsx";//Путь до шаблона реестра
            _TemplateRegistryPath = $@"Templates\RegistryTemplate.xlsx";//Путь до шаблона реестра
        }

        public String DirectoryProgramPath
        {
            get
            {
                return _DirectoryProgramPath;
            }
        }

        public String ConnectProgramPath
        {
            get
            {
                return _ConnectProgramPath;
            }
        }

        public String ConnectServerMails
        {
            get
            {
                return _ConnectServerMails;
            }
        }

        public String DirectoryModelsPath
        {
            get
            {
                return _DirectoryModelsPath;
            }
        }

        public String GetDirectory(String Path)
        {
            String[] Temp = Path.Split('\\');
            String Directory = String.Empty;

            for (Int32 i = 0; i < Temp.Length - 1; i++)
            {
                Directory += Temp[i];
            }

            return Directory;
        }

        public String GetFileName(String Path)
        {
            String[] Temp = Path.Split('\\');
            String FileName = String.Empty;
            FileName += Temp[Temp.Length-1];
            return FileName;
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
        public String TestFileApplicationPath
        {
            get
            {
                return _TestFileApplicationPath;
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
        public String TemplateActUniquePath
        {
            get
            {
                return _TemplateActUniquePath;
            }
        }
        public String TemplateActNoUniquePath
        {
            get
            {
                return _TemplateActNoUniquePath;
            }
        }
        public String TemplateReportOrderOfDatePath
        {
            get
            {
                return _TemplateReportOrderOfDatePath;
            }
        }
        public String TemplateRegistry
        {
            get
            {
                return _TemplateRegistryPath;
            }
        }
    }
}
