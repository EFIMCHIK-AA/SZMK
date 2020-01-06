using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZMK
{
    public static class SystemArgs
    {
        static public User User; // Пользователь, который зашел в систему
        static public Path Path; //Системные пути
        static public MobileApplication MobileApplication; //Конфигурация мобильного приложения
        static public ClientProgram ClientProgram; // Конфигурация клиентского программного обеспечения
        static public ByteScout ByteScout; // Конфигурация программы распознавания
        static public Request Request; //Слой запросов к базе данных
        static public DataBase DataBase; //Конфигурация базы данных
        static public Template Template;//Проверка шаблонов
        static public ServerMail ServerMail; //Конфигурация почтового сервера
        static public List<Mail> Mails; //Общий список адресов почты
        static public List<Position> Positions; //Общий список должностей
        static public List<User> Users; //общий список пользователей в системе
        static public List<Order> Orders; // Общий список заказов
        static public ServerMobileAppOrder ServerMobileAppOrder; //Сервер для получения данных с мобильного ПО чертежей
        static public ServerMobileAppBlankOrder ServerMobileAppBlankOrder; //Сервер для получения данных с мобильного ПО бланков заказов
        static public Excel Excel;//Формирование Актов при добавлении чертежей
        static public UnLoadSpecific UnLoadSpecific;//Проверка выгрузки деталей
        static public List<Status> Statuses;//Общий список возможных статусов
        static public List<BlankOrder> BlankOrders;//Общий список возможных бланков заказа

        public static void PrintLog(String Message)
        {
            Log Temp = new Log(Message);
            Temp.Print();
        }
    }
}
