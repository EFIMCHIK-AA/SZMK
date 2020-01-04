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
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
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
                    TcpClient client = listener.AcceptTcpClient();
                    using (NetworkStream inputStream = client.GetStream())
                    {
                        using (BinaryReader reader = new BinaryReader(inputStream))
                        {
                            string filename = reader.ReadString();
                            long lenght = reader.ReadInt64();
                            using (FileStream outputStream = File.Open(Path.Combine(SystemArgs.Path.TempFile, Index + ".png"), FileMode.Create))
                            {
                                long totalBytes = 0;
                                int readBytes = 0;
                                byte[] buffer = new byte[2048];

                                do
                                {
                                    readBytes = inputStream.Read(buffer, 0, buffer.Length);
                                    outputStream.Write(buffer, 0, readBytes);
                                    totalBytes += readBytes;
                                } while (client.Connected && totalBytes < lenght);
                                Load?.Invoke("Был получен файл: " + filename);
                            }
                            String Data = SystemArgs.Decode.TIFF(SystemArgs.Path.TempFile + @"\" + Index + ".png");
                            Byte[] responseData = Encoding.UTF8.GetBytes(Data);
                            inputStream.Write(responseData, 0, responseData.Length);
                            Load?.Invoke("Были отправлены данные: " + Data);
                            Index++;
                        }

                    }
                    client.Close();
                }
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
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
            catch (Exception)
            {
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
            catch (Exception)
            {
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
            catch (Exception)
            {
                return false;
            }
        }
    }
}
