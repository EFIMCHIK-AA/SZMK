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
        private readonly String _ConnectMobilePath;
        private readonly String _ConnectServerMails;
        private readonly String _ConnectDecodePath;

        private readonly String _ArchivePath;
        private readonly String _LogPath;
        private readonly String _DirectoryModelsPath;


        private readonly String _CheckMarksPath;
        private readonly String _VisualRowPath;
        private readonly String _VisualColumnsPath;
        private readonly String _WebCamDevicePath;
        private readonly String _ConfigProgramParh;


        private readonly String _TemplateReportPastTimeofDate;
        private readonly String _TemplateActUniquePath;
        private readonly String _TemplateActNoUniquePath;
        private readonly String _TemplateReportOrderOfDatePath;


        private readonly String _AboutProgram;


        public Path() 
        {


            _ConnectDBPath = $@"Connect\DataBase\connect.conf"; //Параметры подключения к базе данных
            _ConnectMobilePath = $@"Connect\Mobile\connect.conf"; //Параметры подключения к приложению
            _ConnectDecodePath = $@"Connect\Decode\connect.conf"; //Параметры подключения к программе распознавания
            _ConnectServerMails = $@"Connect\Mails\connect.conf"; //Конфигурация почтового сервера


            _ArchivePath = $@"Program\Path\Archive.df";//Путь к архиву
            _LogPath = $@"Log"; //Путь к директории хранения логов
            _DirectoryModelsPath = $@"Program\Path\Models.df"; //Директория выгрузки


            _TemplateActUniquePath = $@"Templates\ActTemplateUnique.xlsx";//Путь до шаблона акта уникальных чертежей
            _TemplateActNoUniquePath = $@"Templates\ActTemplateNoUnique.xlsx";//Путь до шаблона акта не уникальных чертежей
            _TemplateReportOrderOfDatePath = $@"Templates\ReportOrderOfDateTemplate.xlsx";//Путь до шаблона отчета по дате
            _TemplateReportPastTimeofDate = $@"Templates\ReportPastTimeofDateTemplate.xlsx";//Путь до шаблона отчета по времени


            _WebCamDevicePath = $@"Program\Settings\WebCamDevice.conf";//Путь до выбора девайса с камерой
            _CheckMarksPath = $@"Program\Settings\MarksCheck.df"; // Путь до файла с проверкой строки с маркой на строчные буквы
            _VisualColumnsPath = $@"Program\Settings\ColumnSetting.conf"; //Путь до файла с параметрами отображения столбцов
            _VisualRowPath = $@"Program\Settings\VisualRow.df"; // Путь до файла с параметра для визуализации просрочивания чертежей
            _ConfigProgramParh = $@"Program\Settings\Config.conf"; //Путь до файла с настройкой конфигурации ПО


            _AboutProgram = $@"Program\AboutProgram.xml"; //Путь до файла с информацией о программе
        }
        #region//Пути до параметров подключения к различным ПО и БД
        public String ConnectProgramPath
        {
            get
            {
                return _ConnectDecodePath;
            }
        }
        public String ConnectMobilePath
        {
            get
            {
                return _ConnectMobilePath;
            }
        }
        public String ConnectServerMails
        {
            get
            {
                return _ConnectServerMails;
            }
        }
        public String ConnectDBPath
        {
            get
            {
                return _ConnectDBPath;
            }
        }
        #endregion

        #region//Пути до конфигов программы
        public String CheckMarksPath
        {
            get
            {
                return _CheckMarksPath;
            }
        }

        public String VisualRowPath
        {
            get
            {
                return _VisualRowPath;
            }
        }

        public String VisualColumnsPath
        {
            get
            {
                return _VisualColumnsPath;
            }
        }

        public String WebCamDevice
        {
            get
            {
                return _WebCamDevicePath;
            }
        }

        public String ConfigProgram
        {
            get
            {
                return _ConfigProgramParh;
            }
        }
        #endregion

        #region//Методы получения директории и имени файла
        public String GetDirectory(String Path)
        {
            String[] Temp = Path.Split('\\');
            String Directory = String.Empty;

            for (Int32 i = 0; i < Temp.Length - 1; i++)
            {
                Directory += Temp[i] + @"\";
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
        #endregion

        #region//Пути до файлов
        public String LogPath
        {
            get
            {
                return _LogPath;
            }
        }

        public String ArchivePath
        {
            get
            {
                return _ArchivePath;
            }
        }
        public String DirectoryModelsPath
        {
            get
            {
                return _DirectoryModelsPath;
            }
        }
        #endregion

        #region//Пути до шаблонов
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
        public String TemplateReportPastTimeofDate
        {
            get
            {
                return _TemplateReportPastTimeofDate;
            }
        }
        #endregion

        #region//Путь до файла информации о программе
        public String AboutProgram
        {
            get
            {
                return _AboutProgram;
            }
        }
        #endregion
    }
}
