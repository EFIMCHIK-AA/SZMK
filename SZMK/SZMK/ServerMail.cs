using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZMK
{
    public class ServerMail
    {
        private String _NameWho;
        private String _SMTP;
        private String _Port;
        private String _Name;
        private String _Login;
        private String _Password;
        private bool _SSL;

        public ServerMail()
        {
            if (CheckFile())
            {
                if (!GetParametersConnect())
                {
                    throw new Exception("Ошибка при получении параметров подключения к почтовому серверу");
                }
            }
            else
            {
                throw new Exception("Файл подключения к почтовому серверу не найден");
            }
        }

        public bool SSL
        {
            get
            {
                return _SSL;
            }

            set
            {
                _SSL = value;
            }
        }

        public String Login
        {
            get
            {
                return _Login;
            }

            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _Login = value;
                }
            }
        }

        public String SMTP
        {
            get
            {
                return _SMTP;
            }

            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _SMTP = value;
                }
            }
        }

        public String NameWho
        {
            get
            {
                return _NameWho;
            }

            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _NameWho = value;
                }
            }
        }

        public String Name
        {
            get
            {
                return _Name;
            }

            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _Name = value;
                }
            }
        }

        public String Port
        {
            get
            {
                return _Port;
            }

            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _Port = value;
                }
            }
        }

        public String Password
        {
            get
            {
                return _Password;
            }

            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _Password = value;
                }
            }
        }

        public bool GetParametersConnect()
        {
            try
            {
                if (!File.Exists(SystemArgs.Path.ConnectServerMails))
                {
                    throw new Exception();
                }

                using (StreamReader sr = new StreamReader(File.Open(SystemArgs.Path.ConnectServerMails, FileMode.Open)))
                {
                    _NameWho = sr.ReadLine();
                    _SMTP = sr.ReadLine();
                    _Port = sr.ReadLine();
                    _Name = sr.ReadLine();
                    _Login = sr.ReadLine();
                    String SSL = sr.ReadLine();

                    if(SSL.ToLower() == "true")
                    {
                        _SSL = true;
                    }
                    else
                    {
                        _SSL = false;
                    }

                    _Password = Encryption.DecryptRSA(sr.ReadLine());
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool SetParametersConnect()
        {
            try
            {
                String Dir = SystemArgs.Path.GetDirectory(SystemArgs.Path.ConnectServerMails);

                if (!Directory.Exists(Dir))
                {
                    Directory.CreateDirectory(Dir);
                }

                using (StreamWriter sw = new StreamWriter(File.Open(SystemArgs.Path.ConnectServerMails, FileMode.Create)))
                {
                    sw.WriteLine(_NameWho);
                    sw.WriteLine(_SMTP);
                    sw.WriteLine(_Port);
                    sw.WriteLine(_Name);
                    sw.WriteLine(_Login);

                    if(_SSL)
                    {
                        sw.WriteLine("true");
                    }
                    else
                    {
                        sw.WriteLine("false");
                    }

                    sw.WriteLine(Encryption.EncryptRSA(_Password));
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool CheckFile()
        {
            if (!File.Exists(SystemArgs.Path.ConnectServerMails))
            {
                return false;
            }

            return true;
        }

        public bool CheckConnect(String Email,String Name,String Server,Int32 Port,String Login,String Password)
        {
            try
            {
                MailAddress from = new MailAddress(Email, Name);
                MailAddress to = new MailAddress("rakrachok99@mail.ru");
                MailMessage m = new MailMessage(from, to);
                m.Subject = "Тест";
                m.Body = "<h2>Письмо-тест работы smtp-клиента</h2>";
                m.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient(Server, Port);
                smtp.Credentials = new NetworkCredential(Login, Password);
                smtp.EnableSsl = true;
                smtp.Send(m);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public void SendMail()
        {
            try
            {
                MailMessage m = new MailMessage();
                m.From = new MailAddress(NameWho,Name);
                if (SystemArgs.Mails.Count != 0)
                {
                    for(int i=0;i<SystemArgs.Mails.Count;i++)
                    {
                        m.To.Add(new MailAddress(SystemArgs.Mails[i].MailAddress));
                    }
                }
                else
                {
                    throw new Exception("Отсутсвуют адреса для отправки"); 
                }
                m.Subject = "Деталировка отсутствует от " + DateTime.Now.ToString();
                m.Body = CreateMessage();
                m.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient(SMTP, Convert.ToInt32(Port));
                smtp.Credentials = new NetworkCredential(Login, Password);
                smtp.EnableSsl = true;
                smtp.Send(m);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public String CreateMessage()
        {
            try
            {
                String Message = $"<table border=\"1\">" +
                                    $"<tr>" +
                                    $"<td> № заказа</td>" +
                                    $"<td> № листа</td>" +
                                    $"<td> Фамилия разработчика</td>" +
                                    $"<td> № детали</td>" +
                                    $"</tr>";
                for (int i = 0; i < SystemArgs.UnLoadSpecific.Specifics.Count; i++)
                {
                    if (!SystemArgs.UnLoadSpecific.Specifics[i].Finded)
                    {
                        Message += $"<tr>" +
                                    $"<td> {SystemArgs.UnLoadSpecific.Specifics[i].Number}</td>" +
                                    $"<td> {SystemArgs.UnLoadSpecific.Specifics[i].List.ToString()}</td>" +
                                    $"<td> {SystemArgs.UnLoadSpecific.Specifics[i].Executor}</td>" +
                                    $"<td> {SystemArgs.UnLoadSpecific.Specifics[i].NumberSpecific.ToString()}</td>" +
                                    $"</tr>";
                    }
                }
                Message += $"</table>";
                return Message;
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
