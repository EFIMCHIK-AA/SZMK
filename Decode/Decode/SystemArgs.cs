using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decode
{
    static public class SystemArgs
    {
        static public Decode Decode;//Класс для распознования DataMatrix
        static public Server Server;//Класс сервера для получения и отправки данных
        static public PathProgramm Path;
        public static void PrintLog(String Message)
        {
            Log Temp = new Log(Message);
            Temp.Print();
        }
    }
}
