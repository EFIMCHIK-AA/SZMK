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
    }
}
