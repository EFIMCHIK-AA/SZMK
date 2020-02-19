using System;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace SZMK
{
    /*Класс реализованный для выполненя запросов к базе данных, практически все запросы к базе реализованы в этом классе*/
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
                                                                  $"public.\"User\".\"Name\", public.\"User\".\"MidName\", public.\"User\".\"SurName\"," +
                                                                  $"public.\"DataReg\".\"Login\",public.\"DataReg\".\"HashPass\",public.\"DataReg\".\"UpdPassword\"" +
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
                                Reader.GetString(5),Reader.GetDateTime(2),Reader.GetInt64(1),UserMails,Reader.GetString(6), Reader.GetString(7), Reader.GetBoolean(8)));
                            }
                        }
                    }

                    Connect.Close();
                }

                return true;
            }
            catch(Exception E)
            {
                MessageBox.Show(E.Message);
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

                    using (var Command = new NpgsqlCommand($"INSERT INTO public.\"User\"(\"ID_Position\", \"DateCreate\", \"Name\", \"MidName\", \"SurName\")" +
                                                            $"VALUES({User.GetPosition().ID}, '{User.DateCreate}', '{User.Name}', '{User.MiddleName}', '{User.Surname}');", Connect))
                    {
                        Command.ExecuteNonQuery();
                    }

                    using (var Command = new NpgsqlCommand($"INSERT INTO public.\"DataReg\"(\"ID\", \"DateUpd\", \"Login\", \"HashPass\", \"UpdPassword\")" +
                                                            $"VALUES({User.ID}, '{User.DateCreate}', '{User.Login}', '{User.HashPassword}', {User.UpdPassword}); ", Connect))
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
                                                            $"SET \"ID_Position\" = {User.GetPosition().ID}, \"DateCreate\" = '{User.DateCreate}', \"Name\" = '{User.Name}', \"MidName\" = '{User.MiddleName}', \"SurName\" = '{User.Surname}' " +
                                                            $"WHERE \"ID\" = {User.ID}; ", Connect))
                    {
                        Command.ExecuteNonQuery();
                    }

                    using (var Command = new NpgsqlCommand($"UPDATE public.\"DataReg\"" +
                                                            $"SET \"DateUpd\" = '{User.DateCreate}', \"Login\" = '{User.Login}', \"HashPass\" ='{User.HashPassword}', \"UpdPassword\" = {User.UpdPassword} " +
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

        public Boolean UpdatePasswordText(String Password, User User)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"UPDATE public.\"DataReg\" " +
                                                            $"SET \"HashPass\" = '{Password}' " +
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

        public bool UpdatePassword(User User, Boolean Status)
        {
            try
            {

                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"UPDATE public.\"DataReg\" " +
                                                            $"SET \"UpdPassword\" = {Status} " +
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
                if (!StatusExist(Order))
                {
                    using (var Connect = new NpgsqlConnection(_ConnectString))
                    {
                        Connect.Open();

                        using (var Command = new NpgsqlCommand($"INSERT INTO public.\"AddStatus\" (\"DateCreate\", \"ID_Status\", \"ID_Order\", \"ID_User\") VALUES('{DateTime.Now}', '{Order.Status.ID}', '{Order.ID}', '{Order.User.ID}'); ", Connect))
                        {
                            Command.ExecuteNonQuery();
                        }

                        Connect.Close();
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        private bool StatusExist(Order Order)
        {
            Boolean flag = false;
            try
            {

                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"SELECT \"DateCreate\", \"ID_Status\", \"ID_Order\", \"ID_User\" FROM public.\"AddStatus\" WHERE \"ID_Status\"='{Order.Status.ID}' AND \"ID_Order\"='{Order.ID}';", Connect))
                    {
                        using (var Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                flag = true;
                            }
                        }
                    }

                    Connect.Close();
                }

                return flag;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
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
            catch (Exception E)
            {
                throw new Exception(E.ToString());
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
        public bool InsertBlankOrder(String QR)
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
        public bool InsertBlankOrderOfOrders(List<Order> Orders,String QR)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();
                    foreach (Order order in Orders)
                    {
                        if (!SystemArgs.RequestLinq.SelectOrderInBlankOrder(order.ID))
                        {
                            using (var Command = new NpgsqlCommand($"INSERT INTO public.\"AddBlank\"(\"DateCreate\", \"ID_BlankOrder\", \"ID_Order\") VALUES('{DateTime.Now}', '{SystemArgs.RequestLinq.GetIDBlankOrder(QR)}', '{order.ID}')", Connect))
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
        public bool UpdateBlankOrder(String QR, List<Order> Orders)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"UPDATE public.\"BlankOrder\" SET \"QR\" = '{QR}' WHERE \"BlankOrder\".\"ID\" = '{SystemArgs.RequestLinq.GetOldIDBlankOrder(Orders)}'; ", Connect))
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

        public  bool CanceledOrder(Order Order)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"UPDATE public.\"Orders\" SET \"Canceled\" = {Order.Canceled}" +
                                                           $" WHERE \"ID\" = {Order.ID};", Connect))
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
            catch (Exception E)
            {
                throw new Exception(E.ToString());
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
                                                            "\"DateCreate\", \"DataMatrix\", \"Executor\", \"Number\", \"List\", \"Mark\", \"Lenght\", \"Weight\", \"Canceled\" )" +
                                                            $"VALUES('{Order.DateCreate}', '{Order.DataMatrix}', '{Order.Executor}', '{Order.Number}', '{Order.List}', '{Order.Mark}', '{Order.Lenght}', '{Order.Weight}', {Order.Canceled});", Connect))
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

                    using (var Command = new NpgsqlCommand($"UPDATE public.\"Orders\" SET \"DataMatrix\" = '{Order.DataMatrix}', \"Executor\" = '{Order.Executor}', \"Number\" = '{Order.Number}', \"List\" = '{Order.List}', \"Mark\" = '{Order.Mark}', \"Lenght\" = '{Order.Lenght}', \"Weight\" = '{Order.Weight}', \"Canceled\" = '{Order.Canceled}' WHERE \"ID\" = '{Order.ID}'; ", Connect))
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
                    if (GetAllStatusOfUser() && GetAllBlankOrderofOrders())
                    {
                        Connect.Open();

                        using (var Command = new NpgsqlCommand($"SELECT \"ID\", \"DateCreate\", \"DataMatrix\", \"Executor\", \"Number\", \"List\", \"Mark\", \"Lenght\", \"Weight\", \"Canceled\"" +
                                                                $" FROM public.\"Orders\";", Connect))
                        {
                            using (var Reader = Command.ExecuteReader())
                            {
                                while (Reader.Read())
                                {
                                    Int64 ID = Reader.GetInt64(0);

                                    List<StatusOfOrder> StatusID = (from p in SystemArgs.StatusOfOrders
                                                                    where p.IDOrder == ID
                                                                   select p).ToList();
                                    Int64 UserID = -1;
                                    Int64 MaxIDStatus = -1;

                                    Status TempStatus = new Status();

                                    foreach (Status item in SystemArgs.Statuses)
                                    {
                                        foreach (StatusOfOrder StatusOfUser in StatusID)
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

                                    if (SystemArgs.BlankOrderOfOrders.Count > 0)
                                    {
                                        List<BlankOrderOfOrder> BlankOrderID = (from p in SystemArgs.BlankOrderOfOrders
                                                                                where p.IDOrder == ID
                                                                                 select p).ToList();
                                        if (BlankOrderID.Count() > 0)
                                        {
                                            foreach (BlankOrderOfOrder ID_Blank in BlankOrderID)
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
                                    List<DateTime> StatusDate = SystemArgs.StatusOfOrders.Where(p => p.IDOrder == ID && p.IDStatus == TempStatus.ID).Select(p => p.DateCreate).ToList();
                                    SystemArgs.Orders.Add(new Order(ID, Reader.GetString(2), Reader.GetDateTime(1), Reader.GetString(4), Reader.GetString(3), Reader.GetString(5), Reader.GetString(6), Convert.ToDouble(Reader.GetString(7)), Convert.ToDouble(Reader.GetString(8)), TempStatus,StatusDate[0], TempUser, TempBlank, Reader.GetBoolean(9)));
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
            catch (Exception E)
            {
                throw new Exception(E.ToString());
            }
        }
        private bool GetAllBlankOrderofOrders()
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
                                SystemArgs.BlankOrderOfOrders.Add(new BlankOrderOfOrder(Reader.GetDateTime(0), Reader.GetInt64(1), Reader.GetInt64(2)));
                            }
                        }
                    }

                    Connect.Close();
                }

                return true;
            }
            catch (Exception E)
            {
                throw new Exception(E.ToString());
            }
        }

        public bool GetAllStatusOfUser()
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
                                SystemArgs.StatusOfOrders.Add(new StatusOfOrder(Reader.GetDateTime(0), Reader.GetInt64(1), Reader.GetInt64(2), Reader.GetInt64(3)));
                            }
                        }
                    }

                    Connect.Close();
                }

                return true;
            }
            catch (Exception E)
            {
                throw new Exception(E.ToString());
            }
        }
    } 
}
