using System;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZMK
{
    public class Request
    {
        private String _ConnectString;
        
        public Request()
        {
            if(SystemArgs.DataBase != null)
            {
                _ConnectString = SystemArgs.DataBase.ToString();
            }
        }

        public bool GetAllMails()
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"SELECT \"ID\", \"DateCreate\", \"Name\", \"MidName\", \"SurName\", \"Mail\"" +
                                                           $"FROM public.\"AllMail\"", Connect))
                    {
                        using (var Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                SystemArgs.Mails.Add(new Mail(Reader.GetInt64(0), Reader.GetString(2), Reader.GetString(3), Reader.GetString(4), Reader.GetDateTime(1), Reader.GetString(5)));
                            }
                        }
                    }

                    Connect.Close();
                }

                return true;
            }
            catch
            {
                return false;
            }         
        }

        public bool GetAllPositions()
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"SELECT \"ID\", \"Name\"" +
                                                            "FROM public.\"Position\";", Connect))
                    {
                        using (var Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                SystemArgs.Positions.Add(new Position(Reader.GetInt64(0), Reader.GetString(1)));
                            }
                        }
                    }

                    Connect.Close();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool CheckConnect(String ConnectString)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"SELECT 1", Connect))
                    {
                        using (var Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                Connect.Close();
                                return true;
                            }
                        }
                    }

                    Connect.Close();
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        struct MailsOfUser
        {
            public Int64 ID_User;
            public Int64 ID_Mail;

            public MailsOfUser(Int64 IDUser, Int64 IDMail)
            {
                ID_Mail = IDMail;
                ID_User = IDUser;
            }
        }

        public bool GetAllUsers()
        {
            try
            {
                List<MailsOfUser> Temp = new List<MailsOfUser>(); //Письма всех пользователей

                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"SELECT \"ID_Mail\", \"ID_User\"" +
                                                            "FROM public.\"AddUserMail\";", Connect))
                    {
                        using (var Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                Temp.Add(new MailsOfUser(Reader.GetInt64(1), Reader.GetInt64(0)));
                            }
                        }
                    }

                    using (var Command = new NpgsqlCommand($"SELECT public.\"User\".\"ID\", public.\"User\".\"ID_Position\", public.\"User\".\"DateCreate\"," +
                                                                  $"public.\"User\".\"Name\", public.\"User\".\"MidName\", public.\"User\".\"SurName\", public.\"User\".\"DOB\"," +
                                                                  $"public.\"DataReg\".\"Login\",public.\"DataReg\".\"HashPass\"" +
                                                           "FROM public.\"User\", public.\"DataReg\"" +
                                                           "WHERE public.\"User\".\"ID\" = public.\"DataReg\".\"ID\";", Connect))
                    {
                        using (var Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                Int64 ID = Reader.GetInt64(0);

                                List<MailsOfUser> MailsID = (from p in Temp
                                                             where p.ID_User == ID
                                                             select p).ToList(); //Письма текущего пользователя

                                List<Mail> UserMails = new List<Mail>();

                                foreach(Mail Mail in SystemArgs.Mails)
                                {
                                    foreach(MailsOfUser MailsOfUser in MailsID)
                                    {
                                        if (Mail.ID == MailsOfUser.ID_Mail)
                                        {
                                            UserMails.Add(Mail);
                                        }
                                    }
                                }

                                SystemArgs.Users.Add(new User(ID, Reader.GetString(3), Reader.GetString(4),
                                Reader.GetString(5),Reader.GetDateTime(2), Reader.GetDateTime(6),Reader.GetInt64(1),UserMails,Reader.GetString(7), Reader.GetString(8)));
                            }
                        }
                    }

                    Connect.Close();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool AddUser(User User)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"INSERT INTO public.\"User\"(\"ID_Position\", \"DateCreate\", \"Name\", \"MidName\", \"SurName\", \"DOB\")" +
                                                            $"VALUES({User.GetPosition().ID}, '{User.DateCreate}', '{User.Name}', '{User.MiddleName}', '{User.Surname}', '{User.DateOfBirth}'); ", Connect))
                    {
                        Command.ExecuteNonQuery();
                    }

                    using (var Command = new NpgsqlCommand($"INSERT INTO public.\"DataReg\"(\"ID\", \"DateUpd\", \"Login\", \"HashPass\")" +
                                                            $"VALUES({User.ID}, '{User.DateCreate}', '{User.Login}', '{User.HashPassword}'); ", Connect))
                    {
                        Command.ExecuteNonQuery();
                    }

                    Connect.Close();
                }

                return true;
            }
            catch
            {
                return false;
            }       
        }

        public bool ChangeUser(User User)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"UPDATE public.\"User\"" +
                                                            $"SET \"ID_Position\" = {User.GetPosition().ID}, \"DateCreate\" = '{User.DateCreate}', \"Name\" = '{User.Name}', \"MidName\" = '{User.MiddleName}', \"SurName\" = '{User.Surname}', \"DOB\" = '{User.DateOfBirth}'" +
                                                            $"WHERE \"ID\" = {User.ID}; ", Connect))
                    {
                        Command.ExecuteNonQuery();
                    }

                    using (var Command = new NpgsqlCommand($"UPDATE public.\"DataReg\"" +
                                                            $"SET \"DateUpd\" = '{User.DateCreate}', \"Login\" = '{User.Login}', \"HashPass\" ='{User.HashPassword}'" +
                                                            $"WHERE \"ID\" = {User.ID}; ", Connect))
                    {
                        Command.ExecuteNonQuery();
                    }

                    Connect.Close();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteUser(User User)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"DELETE FROM public.\"User\"" +
                                                           $"WHERE \"ID\" = {User.ID}; ", Connect))
                    {
                        Command.ExecuteNonQuery();
                    }

                    Connect.Close();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteMail(Mail Mail)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"DELETE FROM public.\"AllMail\"" +
                                                           $"WHERE \"ID\" = {Mail.ID}; ", Connect))
                    {
                        Command.ExecuteNonQuery();
                    }

                    Connect.Close();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool ChangeMail(Mail Mail)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"UPDATE public.\"AllMail\"" +
                                                            $"SET \"DateCreate\" = '{Mail.DateCreate}', \"Name\" ='{Mail.Name}', \"MidName\" ='{Mail.MiddleName}', \"SurName\" ='{Mail.Surname}', \"Mail\" ='{Mail.MailAddress}'" +
                                                            $"WHERE \"ID\" = {Mail.ID}; ", Connect))
                    {
                        Command.ExecuteNonQuery();
                    }

                    Connect.Close();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool AddMail(Mail Mail)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"INSERT INTO public.\"AllMail\"(\"DateCreate\", \"Name\", \"MidName\", \"SurName\", \"Mail\")" +
                                                           $"VALUES('{Mail.DateCreate}', '{Mail.Name}', '{Mail.MiddleName}', '{Mail.Surname}', '{Mail.MailAddress}'); ", Connect))
                    {
                        Command.ExecuteNonQuery();
                    }

                    Connect.Close();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool InsertOrderDB(Order Order)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"INSERT INTO public.\"Orders\"(" +
                                                            "\"DateCreate\", \"DataMatrix\", \"Executor\", \"Number\", \"List\", \"Mark\", \"Lenght\", \"Weight\")" +
                                                            $"VALUES('{Order.DateCreate}', '{Order.DataMatrix}', '{Order.Executor}', '{Order.Number}', '{Order.List}', '{Order.Mark}', '{Order.Lenght}', '{Order.Weight}');", Connect))
                    {
                        Command.ExecuteNonQuery();
                    }

                    Connect.Close();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool InsertBlankOrder(Int64 IDOrder,Int64 IDBlankOrder)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"INSERT INTO public.\"AddBlank\"(\"DateCreate\", \"ID_BlankOrder\", \"ID_Order\") VALUES('{DateTime.Now}', '{IDBlankOrder}', '{IDOrder}'); ", Connect))
                    {
                        Command.ExecuteNonQuery();
                    }

                    Connect.Close();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool CheckedUniqueOrderDB(String DataMatrix)
        {
            Boolean flag = false;
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"SELECT Count(\"Orders\".\"DataMatrix\") FROM \"Orders\" WHERE \"DataMatrix\" = '{DataMatrix}'", Connect))
                    {
                        using (var reader = Command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (reader.GetInt64(0) == 0)
                                {
                                    flag = true;
                                }
                            }
                        }
                    }

                    Connect.Close();
                }
                return flag;
            }
            catch
            {
                return flag;
            }
        }
        public bool GetAllStatus()
        {
            try
            {

                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"SELECT \"ID\", \"ID_Position\", \"Name\" FROM public.\"Status\";", Connect))
                    {
                        using (var Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                SystemArgs.Statuses.Add(new Status(Reader.GetInt64(0),Reader.GetInt64(1), Reader.GetString(2)));
                            }
                        }
                    }

                    Connect.Close();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool GetAllBlankOrder()
        {
            try
            {

                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"SELECT \"ID\", \"DateCreate\", \"QR\" FROM public.\"BlankOrder\";", Connect))
                    {
                        using (var Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                SystemArgs.BlankOrders.Add(new BlankOrder(Reader.GetInt64(0), Reader.GetDateTime(1), Reader.GetString(2)));
                            }
                        }
                    }

                    Connect.Close();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool AddOrGetOrUpdateBlankOrder(BlankOrder BlankOrder,Int64 IDOrder)
        {
            try
            {
                Boolean flag = false;
                Int64 ID = -1;
                String TempQR = "";
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"SELECT \"ID\", \"DateCreate\", \"QR\" FROM public.\"BlankOrder\" WHERE \"BlankOrder\".\"QR\"='{BlankOrder.QR}';", Connect))
                    {
                        using (var Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                BlankOrder = new BlankOrder(Reader.GetInt64(0), Reader.GetDateTime(1), Reader.GetString(2));
                                TempQR = Reader.GetString(2);
                            }
                        }
                    }
                    if (TempQR != BlankOrder.QR)
                    {
                        using (var Command = new NpgsqlCommand($"SELECT \"ID_BlankOrder\" FROM public.\"AddBlank\" WHERE \"AddBlank\".\"ID_Order\"='{IDOrder}';", Connect))
                        {
                            using (var Reader = Command.ExecuteReader())
                            {
                                while (Reader.Read())
                                {
                                    flag = true;
                                    ID = Reader.GetInt64(0);
                                }
                            }
                        }
                        if (!flag)
                        {
                            using (var Command = new NpgsqlCommand($"INSERT INTO public.\"BlankOrder\"(\"DateCreate\", \"QR\") VALUES('{DateTime.Now}', '{BlankOrder.QR}'); ", Connect))
                            {
                                Command.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            using (var Command = new NpgsqlCommand($"UPDATE public.\"BlankOrder\" SET \"DateCreate\" = '{BlankOrder.DateCreate}', \"QR\" = '{BlankOrder.QR}' WHERE \"BlankOrder\".\"ID\" = '{ID}'; ", Connect))
                            {
                                Command.ExecuteNonQuery();
                            }
                        }
                    }

                    Connect.Close();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        struct StatusOfUser
        {
            public DateTime _DateCreate;
            public Int64 _IDStatus;
            public Int64 _IDOrder;
            public Int64 _IDUser;

            public StatusOfUser(DateTime DateCreate, Int64 IDStatus, Int64 IDOrder, Int64 IDUser)
            {
                _DateCreate = DateCreate;
                _IDStatus = IDStatus;
                _IDOrder = IDOrder;
                _IDUser = IDUser;
            }
        }
        struct BlankOrderofOrders
        {
            public DateTime _DateCreate;
            public Int64 _IDBlankOrder;
            public Int64 _IDOrder;

            public BlankOrderofOrders(DateTime DateCreate, Int64 IDBlankOrder, Int64 IDOrder)
            {
                _DateCreate = DateCreate;
                _IDBlankOrder = IDBlankOrder;
                _IDOrder = IDOrder;
            }
        }
        public bool GetAllOrders()
        {
            try
            {

                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    List<StatusOfUser> StatusOfUsers = new List<StatusOfUser>();

                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"SELECT \"DateCreate\", \"ID_Status\", \"ID_Order\", \"ID_User\" FROM public.\"AddStatus\";", Connect))
                    {
                        using (var Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                StatusOfUsers.Add(new StatusOfUser(Reader.GetDateTime(0), Reader.GetInt64(1),Reader.GetInt64(2), Reader.GetInt64(3)));
                            }
                        }
                    }
                    List<BlankOrderofOrders> BlankOrderofOrders = new List<BlankOrderofOrders>();

                    using (var Command = new NpgsqlCommand($"SELECT \"DateCreate\", \"ID_BlankOrder\", \"ID_Order\" FROM public.\"AddBlank\";", Connect))
                    {
                        using (var Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                BlankOrderofOrders.Add(new BlankOrderofOrders(Reader.GetDateTime(0), Reader.GetInt64(1), Reader.GetInt64(2)));
                            }
                        }
                    }

                    using (var Command = new NpgsqlCommand($"SELECT \"ID\", \"DateCreate\", \"DataMatrix\", \"Executor\", \"Number\", \"List\", \"Mark\", \"Lenght\", \"Weight\" FROM public.\"Orders\";", Connect))
                    {
                        using (var Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                Int64 ID = Reader.GetInt64(0);

                                List<StatusOfUser> StatusID = (from p in StatusOfUsers
                                                where p._IDOrder == ID
                                                             select p).ToList();
                                BlankOrderofOrders BlankOrderID = (from p in BlankOrderofOrders
                                                    where p._IDOrder == ID
                                                    select p).Single();
                                Int64 UserID = -1;
                                Int64 MaxIDStatus = -1;
                                foreach (Status item in SystemArgs.Statuses)
                                {
                                    foreach(StatusOfUser StatusOfUser in StatusID)
                                    {
                                        if (item.ID == StatusOfUser._IDStatus&&StatusOfUser._IDStatus>MaxIDStatus)
                                        {
                                            SystemArgs.Status = item;
                                            MaxIDStatus = StatusOfUser._IDStatus;
                                            UserID = StatusOfUser._IDUser;
                                        }
                                    }
                                }
                                User User = (from p in SystemArgs.Users
                                              where p.ID == UserID
                                              select p).Single();
                                foreach (BlankOrder item in SystemArgs.BlankOrders)
                                {
                                        if (BlankOrderID._IDBlankOrder == item.ID)
                                        {
                                            SystemArgs.BlankOrder = item;
                                        }
                                }
                                SystemArgs.Orders.Add(new Order(ID, Reader.GetString(2), Reader.GetDateTime(1), Reader.GetString(4), Reader.GetString(3), Reader.GetInt64(5), Reader.GetString(6), Reader.GetInt64(7), Reader.GetInt64(8),SystemArgs.Status,User,SystemArgs.BlankOrder));
                            }
                        }
                    }

                    Connect.Close();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }


    } 
}
