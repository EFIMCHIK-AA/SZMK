using System;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
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
                if (DeleteOrdersUser(User))
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
                }
                else
                {
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool DeleteOrdersUser(User User)
        {
            try
            {
                List<Int64> Temp = (from p in GetAllIdOrder(User.ID)
                                    select p).Distinct().ToList();
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();
                    foreach (Int64 ID in Temp)
                    {
                        using (var Command = new NpgsqlCommand($"DELETE FROM public.\"Orders\" WHERE \"Orders\".\"ID\"='{ID}'; ", Connect))
                        {
                            Command.ExecuteNonQuery();
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
        private List<Int64> GetAllIdOrder(Int64 UserID)
        {
            try
            {
                List<Int64> Temp = new List<Int64>();
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"SELECT \"ID_Order\" FROM public.\"AddStatus\" WHERE \"AddStatus\".\"ID_User\" = '{UserID}'; ", Connect))
                    {
                        using (var Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                Temp.Add(Reader.GetInt64(0));
                            }
                        }
                    }

                    Connect.Close();
                }

                return Temp;
            }
            catch
            {
                throw new Exception("Ошибка получения всех чертежей которые были связаны с данным пользователем");
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
        public bool InsertStatus(Order Order)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"INSERT INTO public.\"AddStatus\" (\"DateCreate\", \"ID_Status\", \"ID_Order\", \"ID_User\") VALUES('{Order.DateCreate}', '{Order.Status.ID}', '{Order.ID}', '{Order.User.ID}'); ", Connect))
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
        public bool DeleteStatus(Order Order)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"DELETE FROM public.\"AddStatus\" WHERE \"ID_Status\" = '{Order.Status.ID}' AND \"ID_Order\" = '{Order.ID}'; ", Connect))
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
        Int64 IndexBlankOrder = -1;
        public bool CompareBlankOrder(List<Order> Orders, String QR)
        {
            try
            {
                if (SelectBlankOrder(QR))
                {
                    if (InsertBlankOrderOfOrders(Orders))
                    {
                        return true;
                    }
                }
                else if (GetOldIDBlankOrder(Orders))
                {
                    if (UpdateBlankOrder(QR))
                    {
                        if (InsertBlankOrderOfOrders(Orders))
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    if (InsertBlankOrder(QR))
                    {
                        if (SelectBlankOrder(QR))
                        {
                            if (InsertBlankOrderOfOrders(Orders))
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        private bool SelectOrderInBlankOrder(Int64 IDOrder)
        {
            Boolean flag = false;
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"SELECT COUNT(\"AddBlank\".\"ID_Order\") AS \"Count\" FROM \"AddBlank\" WHERE \"ID_Order\"='{IDOrder}';", Connect))
                    {
                        using (var reader = Command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (reader.GetInt64(0) == 1)
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
        private bool SelectBlankOrder(String QR)
        {
            Boolean flag = false;
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"SELECT \"ID\" FROM public.\"BlankOrder\" WHERE \"BlankOrder\".\"QR\"='{QR}';", Connect))
                    {
                        using (var reader = Command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                IndexBlankOrder = reader.GetInt64(0);
                                flag = true;
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
        private bool FindedOrdersInAddBlankOrder(String QR,Int64 IDOrder)
        {
            Boolean flag = false;
            try
            {
                if (SelectBlankOrder(QR))
                {
                    using (var Connect = new NpgsqlConnection(_ConnectString))
                    {
                        Connect.Open();

                        using (var Command = new NpgsqlCommand($"SELECT COUNT(\"ID_BlankOrder\") FROM public.\"AddBlank\" WHERE \"ID_BlankOrder\"='{IndexBlankOrder}' AND \"ID_Order\"='{IDOrder}';", Connect))
                        {
                            using (var reader = Command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    if (reader.GetInt64(0) == 1)
                                    {
                                        flag = true;
                                    }
                                }
                            }
                        }

                        Connect.Close();
                    }
                }
                return flag;
            }
            catch
            {
                return flag;
            }
        }
        private bool InsertBlankOrder(String QR)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();
                    using (var Command = new NpgsqlCommand($"INSERT INTO public.\"BlankOrder\"(\"DateCreate\", \"QR\") VALUES('{DateTime.Now}', '{QR}'); ", Connect))
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
        private bool InsertBlankOrderOfOrders(List<Order> Orders)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();
                    foreach (Order order in Orders)
                    {
                        if (!SelectOrderInBlankOrder(order.ID))
                        {
                            using (var Command = new NpgsqlCommand($"INSERT INTO public.\"AddBlank\"(\"DateCreate\", \"ID_BlankOrder\", \"ID_Order\") VALUES('{DateTime.Now}', '{IndexBlankOrder}', '{order.ID}')", Connect))
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
        private bool GetOldIDBlankOrder(List<Order> Orders)
        {
            Boolean flag = false;
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();
                    foreach (Order order in Orders)
                    {
                        using (var Command = new NpgsqlCommand($"SELECT \"ID_BlankOrder\"FROM public.\"AddBlank\" WHERE \"AddBlank\".\"ID_Order\"='{order.ID}';", Connect))
                        {
                            using (var reader = Command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    IndexBlankOrder = reader.GetInt64(0);
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
        private bool UpdateBlankOrder(String QR)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"UPDATE public.\"BlankOrder\" SET \"QR\" = '{QR}' WHERE \"BlankOrder\".\"ID\" = '{IndexBlankOrder}'; ", Connect))
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
        public bool InsertOrder(Order Order)
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
        public bool DeleteOrder(Order Order)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"DELETE FROM public.\"Orders\" WHERE \"ID\" = '{Order.ID}'; ", Connect))
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
        public bool UpdateOrder(Order Order)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"UPDATE public.\"Orders\" SET \"DataMatrix\" = '{Order.DataMatrix}', \"Executor\" = '{Order.Executor}', \"Number\" = '{Order.Number}', \"List\" = '{Order.List}', \"Mark\" = '{Order.Mark}', \"Lenght\" = '{Order.Lenght}', \"Weight\" = '{Order.Weight}' WHERE \"ID\" = '{Order.ID}'; ", Connect))
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
        public bool GetAllOrders()
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    List<StatusOfUser> StatusOfUsers = new List<StatusOfUser>();
                    List<BlankOrderOfOrders> BlankOrderofOrders = new List<BlankOrderOfOrders>();
                    if (GetAllStatusOfUser(StatusOfUsers) && GetAllBlankOrderofOrders(BlankOrderofOrders))
                    {
                        Connect.Open();

                        using (var Command = new NpgsqlCommand($"SELECT \"ID\", \"DateCreate\", \"DataMatrix\", \"Executor\", \"Number\", \"List\", \"Mark\", \"Lenght\", \"Weight\" FROM public.\"Orders\";", Connect))
                        {
                            using (var Reader = Command.ExecuteReader())
                            {
                                while (Reader.Read())
                                {
                                    Int64 ID = Reader.GetInt64(0);

                                    List<StatusOfUser> StatusID = (from p in StatusOfUsers
                                                                   where p.IDOrder == ID
                                                                   select p).ToList();
                                    Int64 UserID = -1;
                                    Int64 MaxIDStatus = -1;
                                    Status TempStatus = new Status();
                                    foreach (Status item in SystemArgs.Statuses)
                                    {
                                        foreach (StatusOfUser StatusOfUser in StatusID)
                                        {
                                            if (item.ID == StatusOfUser.IDStatus && StatusOfUser.IDStatus > MaxIDStatus)
                                            {
                                                TempStatus = item;
                                                MaxIDStatus = StatusOfUser.IDStatus;
                                                UserID = StatusOfUser.IDUser;
                                            }
                                        }
                                    }
                                    User TempUser = (from p in SystemArgs.Users
                                                     where p.ID == UserID
                                                     select p).Single();
                                    BlankOrder TempBlank = new BlankOrder();
                                    if (BlankOrderofOrders.Count > 0)
                                    {
                                        List<BlankOrderOfOrders> BlankOrderID = (from p in BlankOrderofOrders
                                                                                 where p.IDOrder == ID
                                                                                 select p).ToList();
                                        if (BlankOrderID.Count() > 0)
                                        {
                                            foreach (BlankOrderOfOrders ID_Blank in BlankOrderID)
                                            {
                                                foreach (BlankOrder item in SystemArgs.BlankOrders)
                                                {
                                                    if (ID_Blank._IDBlankOrder == item.ID)
                                                    {
                                                        TempBlank = item;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    SystemArgs.Orders.Add(new Order(ID, Reader.GetString(2), Reader.GetDateTime(1), Reader.GetString(4), Reader.GetString(3), Convert.ToInt64(Reader.GetString(5)), Reader.GetString(6), Convert.ToDouble(Reader.GetString(7)), Convert.ToDouble(Reader.GetString(8)), TempStatus, TempUser, TempBlank));
                                }
                            }
                        }

                        Connect.Close();
                    }
                    else
                    {
                        return false;
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        private bool GetAllBlankOrderofOrders(List<BlankOrderOfOrders> BlankOrderofOrders)
        {
            try
            {

                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"SELECT \"DateCreate\", \"ID_BlankOrder\", \"ID_Order\" FROM public.\"AddBlank\";", Connect))
                    {
                        using (var Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                BlankOrderofOrders.Add(new BlankOrderOfOrders(Reader.GetDateTime(0), Reader.GetInt64(1), Reader.GetInt64(2)));
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
        public bool GetAllStatusOfUser(List<StatusOfUser> StatusOfUsers)
        {
            try
            {

                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"SELECT \"DateCreate\", \"ID_Status\", \"ID_Order\", \"ID_User\" FROM public.\"AddStatus\";", Connect))
                    {
                        using (var Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                StatusOfUsers.Add(new StatusOfUser(Reader.GetDateTime(0), Reader.GetInt64(1), Reader.GetInt64(2), Reader.GetInt64(3)));
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
        public Int64 GetIDOrder(String DataMatrix)
        {
            Int64 ID = 0;
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"SELECT \"ID\" FROM public.\"Orders\" WHERE \"DataMatrix\"='{DataMatrix}';", Connect))
                    {
                        using (var reader = Command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ID = reader.GetInt64(0);
                            }
                        }
                    }

                    Connect.Close();
                }
                return ID;
            }
            catch
            {
                throw new Exception("Ошибка получения ID чертежа");
            }
        }
        public bool CheckedStatusOrderDB(Int64 IDStatus, String DataMatrix)
        {
            Int64 IDOrder = GetIDOrder(DataMatrix);
            Boolean flag = false;
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"SELECT MAX(\"ID_Status\") FROM public.\"AddStatus\" WHERE \"ID_Order\"='{IDOrder}';", Connect))
                    {
                        using (var reader = Command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (reader.GetInt64(0) == IDStatus-1)
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
        public bool CheckedExistenceOrderAndStatus(String Number,Int64 List)
        {
            Boolean flag = false;
            try
            {
                Int64 IDStatus = (from p in SystemArgs.Statuses
                                  where p.IDPosition == SystemArgs.User.GetPosition().ID
                                  select p.ID).Single();
                String DataMatrix = (from p in SystemArgs.Orders
                                     where (p.Number == Number) && (p.List == List)
                                     select p.DataMatrix).Single();
                if (CheckedStatusOrderDB(IDStatus, DataMatrix))
                {
                    using (var Connect = new NpgsqlConnection(_ConnectString))
                    {
                        Connect.Open();

                        using (var Command = new NpgsqlCommand($"SELECT Count(\"DataMatrix\") FROM public.\"Orders\" WHERE \"Number\"='{Number}' AND \"List\"='{List}';", Connect))
                        {
                            using (var reader = Command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    if (reader.GetInt64(0) == 1)
                                    {
                                        flag = true;
                                    }
                                }
                            }
                        }

                        Connect.Close();
                    }
                }
                return flag;
            }
            catch
            {
                return flag;
            }
        }
        public bool CheckedExistenceBlankOrderAndStatus(String Number, Int64 List,String QR)
        {
            Boolean flag = false;
            try
            {
                Int64 IDStatus = (from p in SystemArgs.Statuses
                                  where p.IDPosition == SystemArgs.User.GetPosition().ID
                                  select p.ID).Single();
                String DataMatrix = (from p in SystemArgs.Orders
                                     where (p.Number == Number) && (p.List == List)
                                     select p.DataMatrix).Single();
                if (CheckedStatusOrderDB(IDStatus, DataMatrix)&&FindedOrdersInAddBlankOrder(QR,GetIDOrder(DataMatrix)))
                {
                    using (var Connect = new NpgsqlConnection(_ConnectString))
                    {
                        Connect.Open();

                        using (var Command = new NpgsqlCommand($"SELECT Count(\"DataMatrix\") FROM public.\"Orders\" WHERE \"Number\"='{Number}' AND \"List\"='{List}';", Connect))
                        {
                            using (var reader = Command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    if (reader.GetInt64(0) == 1)
                                    {
                                        flag = true;
                                    }
                                }
                            }
                        }

                        Connect.Close();
                    }
                }
                return flag;
            }
            catch
            {
                return flag;
            }
        }
    } 
}
