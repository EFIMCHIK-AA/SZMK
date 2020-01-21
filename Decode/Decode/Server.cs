using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Decode
{
    public class Server
    {
        public String _Port;
        public delegate void LoadData(String Text);
        public event LoadData Load;
        static TcpListener listener;
        static int Index = 0;
        public bool Start()
        {
            try
            {
                if (CreateTempDirectory())
                {
                    listener = new TcpListener(IPAddress.Any, Convert.ToInt32(SystemArgs.Server._Port));
                    listener.Start();
                    ListeningAsync();
                    SystemArgs.PrintLog("Запущено слушание клиентов и найдена директория темповых файлов");
                    return true;
                }
                else
                {
                    SystemArgs.PrintLog("Ошибка создания директории темповых файлов");
                    return false;
                }
            }
            catch(Exception e)
            {
                SystemArgs.PrintLog(e.Message);
                return false;
            }
        }
        private async void ListeningAsync()
        {
            await Task.Run(() => Listening());
        }
        private void Listening()
        {
            try
            {
                while (true)
                {
                    String User="";
                    TcpClient client = listener.AcceptTcpClient();
                    using (NetworkStream inputStream = client.GetStream())
                    {
                        using (BinaryReader reader = new BinaryReader(inputStream))
                        {
                            if (reader.ReadBoolean())
                            {
                                Load?.Invoke("Была проведена проверка соединения пользователем "+reader.ReadString());
                                client.Close();
                                continue;
                            }
                            else
                            {
                                User = reader.ReadString();
                                Load?.Invoke("Был присоединен пользователь "+ User);
                            }
                            string OldFilename = reader.ReadString();
                            string FileName = reader.ReadString();
                            long lenght = reader.ReadInt64();
                            using (FileStream outputStream = File.Open(Path.Combine(SystemArgs.Path.TempFile, Index + ".png"), FileMode.Create))
                            {
                                long totalBytes = 0;
                                int readBytes = 0;
                                byte[] buffer = new byte[8192];

                                do
                                {
                                    readBytes = inputStream.Read(buffer, 0, buffer.Length);
                                    outputStream.Write(buffer, 0, readBytes);
                                    totalBytes += readBytes;
                                } while (client.Connected && totalBytes < lenght);
                                Load?.Invoke("Был получен файл: " + OldFilename);
                            }
                            String Data = SystemArgs.Decode.TIFF(SystemArgs.Path.TempFile + @"\" + Index + ".png");
                            Byte[] responseData = Encoding.UTF8.GetBytes(Data);
                            inputStream.Write(responseData, 0, responseData.Length);
                            Load?.Invoke("Были отправлены данные: " + Data);
                            Index++;
                        }
                    }
                    client.Close();
                    DirectoryInfo TempFiles = new DirectoryInfo(SystemArgs.Path.TempFile);
                    if (TempFiles.GetFiles().Length >= 50)
                    {
                        foreach (FileInfo File in TempFiles.GetFiles())
                        {
                            File.Delete();
                        }
                    }
                    Load?.Invoke("Был отключен пользователь " + User);
                }
            }
            catch(Exception e)
            {
                SystemArgs.PrintLog(e.Message);
                ListeningAsync();
            }
        }
        public bool GetParametersConnect()
        {
            try
            {
                if (!File.Exists(SystemArgs.Path.ConnectApplicationPath))
                {
                    throw new Exception();
                }

                using (StreamReader sr = new StreamReader(File.Open(SystemArgs.Path.ConnectApplicationPath, FileMode.Open)))
                {
                    _Port = sr.ReadLine();
                }

                return true;
            }
            catch (Exception e)
            {
                SystemArgs.PrintLog(e.Message);
                return false;
            }
        }

        public bool SetParametersConnect()
        {
            try
            {
                String Dir = SystemArgs.Path.GetDirectory(SystemArgs.Path.ConnectApplicationPath);

                if (!Directory.Exists(Dir))
                {
                    Directory.CreateDirectory(Dir);
                }

                using (StreamWriter sw = new StreamWriter(File.Open(SystemArgs.Path.ConnectApplicationPath, FileMode.Create)))
                {
                    sw.WriteLine(_Port);
                }

                return true;
            }
            catch (Exception e)
            {
                SystemArgs.PrintLog(e.Message);
                return false;
            }
        }
        public bool CreateTempDirectory()
        {
            try
            {
                String Dir = SystemArgs.Path.TempFile;

                if (!Directory.Exists(Dir))
                {
                    Directory.CreateDirectory(Dir);
                }

                return true;
            }
            catch (Exception e)
            {
                SystemArgs.PrintLog(e.Message);
                return false;
            }
        }
    }
}
