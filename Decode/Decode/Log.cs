using System;
using System.IO;

namespace Decode
{
    class Log
    {
        private readonly String _Message; //Сообщение для записи
        private DateTime _CurrentDate;

        public Log(String StringData)
        {
            if (StringData.Trim() != "")
            {
                _Message = StringData;
            }

            _CurrentDate = DateTime.Now;

            if (!Directory.Exists($@"{SystemArgs.Path.LogPath}\{_CurrentDate.ToShortDateString()}"))
            {
                Directory.CreateDirectory($@"{SystemArgs.Path.LogPath}\{_CurrentDate.ToShortDateString()}");
            }

            if (!File.Exists($@"{SystemArgs.Path.LogPath}\{_CurrentDate.ToShortDateString()}\{_CurrentDate.ToShortDateString()}.log"))
            {
                using (FileStream fs = File.Create($@"{SystemArgs.Path.LogPath}\{_CurrentDate.ToShortDateString()}\{_CurrentDate.ToShortDateString()}.log")) { }
            }
        }

        public Log() : this("Нет информации") { }

        public void Print()
        {
            String TempString = $@"{_CurrentDate.ToString()} : {_Message}" + Environment.NewLine;

            File.AppendAllText($@"{SystemArgs.Path.LogPath}\{_CurrentDate.ToShortDateString()}\{_CurrentDate.ToShortDateString()}.log", TempString);
        }
    }
}
