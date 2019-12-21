﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZMK
{
    public static class SystemArgs
    {
        public static Autorization_F Autorization; // Линк на главную форму

        static public User User; // Пользователь, который зашел в систему
        static public Path Path; //Системные пути
        static public DataBase DataBase = new DataBase(); //Конфигурация базы данных

        static public List<Mail> Mails = new List<Mail>(); //Общий список адресов почты
        static public List<Position> Positions = new List<Position>(); //Общий список должностей
        static public List<User> Users = new List<User>(); //общий список пользователей в системе
        //static public List<Order> Orders; // Общий список заказов

        public static void PrintLog(String Message)
        {
            Log Temp = new Log(Message);
            Temp.Print();
        }
    }
}