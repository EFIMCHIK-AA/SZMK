
using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace SZMK
{
    public class ByteScout
    {
        private String _ProgramPath;
        private String _Port;
        private String _Server;
        public delegate void LoadData(List<OrderScanSession> ScanSession);
        public event LoadData Load;
        public delegate void FailData(String FileName);
        public event FailData Fail;
        public List<OrderScanSession> _DecodeSession;

        public ByteScout()
        {
            if (!GetParametersConnect())
            {
                throw new Exception("Ошибка при получении конфигурационных путей программы распознавания");
            }

            _DecodeSession = new List<OrderScanSession>();
        }

        public bool GetParametersConnect()
        {
            try
            {
                if (!File.Exists(SystemArgs.Path.ProgramPath))
                {
                    throw new Exception();
                }

                if (!File.Exists(SystemArgs.Path.ConnectProgramPath))
                {
                    throw new Exception();
                }

                using (StreamReader sr = new StreamReader(File.Open(SystemArgs.Path.ProgramPath, FileMode.Open)))
                {
                    _ProgramPath = sr.ReadLine();
                }

                using (StreamReader sr = new StreamReader(File.Open(SystemArgs.Path.ConnectProgramPath, FileMode.Open)))
                {
                    _Server = sr.ReadLine();
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
                String DirProg = SystemArgs.Path.GetDirectory(SystemArgs.Path.ProgramPath);

                if (!Directory.Exists(DirProg))
                {
                    Directory.CreateDirectory(DirProg);
                }

                String DirConnProgram = SystemArgs.Path.GetDirectory(SystemArgs.Path.ConnectProgramPath);

                if (!Directory.Exists(DirConnProgram))
                {
                    Directory.CreateDirectory(DirConnProgram);
                }

                using (StreamWriter sw = new StreamWriter(File.Open(SystemArgs.Path.ProgramPath, FileMode.Create)))
                {
                    sw.WriteLine(_ProgramPath);
                }

                using (StreamWriter sw = new StreamWriter(File.Open(SystemArgs.Path.ConnectProgramPath, FileMode.Create)))
                {
                    sw.WriteLine(_Server);
                    sw.WriteLine(_Port);
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
            if (!File.Exists(_ProgramPath))
            {
                return false;
            }

            return true;
        }

        public String ProgramPath
        {
            get
            {
                return _ProgramPath;
            }

            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _ProgramPath = value;
                }
            }
        }

        public String Server
        {
            get
            {
                return _Server;
            }

            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _Server = value;
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

        public void ClearData()
        {
            _DecodeSession.Clear();
        }
        public String SendAndRead(String FileName,String OldFileName)
        {
            TcpClient tcpClient = new TcpClient(_Server,Convert.ToInt32(_Port));
            String responseData = String.Empty;
            using (FileStream inputStream = File.OpenRead(FileName))
            {
                using (NetworkStream outputStream = tcpClient.GetStream())
                {
                    using (BinaryWriter writer = new BinaryWriter(outputStream))
                    {
                        bool CloseConnect = false;
                        long lenght = inputStream.Length;
                        long totalBytes = 0;
                        int readBytes = 0;
                        byte[] buffer = new byte[8192];
                        writer.Write(CloseConnect);
                        writer.Write(SystemArgs.User.Login);
                        writer.Write(OldFileName);
                        writer.Write(SystemArgs.Path.GetFileName(FileName));
                        writer.Write(lenght);
                        do
                        {
                            readBytes = inputStream.Read(buffer, 0, buffer.Length);
                            outputStream.Write(buffer, 0, readBytes);
                            totalBytes += readBytes;
                        } while (tcpClient.Connected && totalBytes < lenght);
                        Byte[] readingData = new Byte[256];
                        StringBuilder completeMessage = new StringBuilder();
                        int numberOfBytesRead = 0;
                        do
                        {
                            numberOfBytesRead = outputStream.Read(readingData, 0, readingData.Length);
                            completeMessage.AppendFormat("{0}", Encoding.UTF8.GetString(readingData, 0, numberOfBytesRead));
                        }
                        while (outputStream.DataAvailable);
                        responseData = completeMessage.ToString();
                        if (AddDecodeSession(responseData.Replace(" ","")))
                        {
                            Load?.Invoke(_DecodeSession);
                        }
                        else
                        {
                            Fail?.Invoke(OldFileName);
                        }

                    }
                }
            }
            tcpClient.Close();
            return responseData;
        }

        public bool CheckedUniqueList(String Message)
        {
            foreach (var item in _DecodeSession)
            {
                if (item.DataMatrix.Equals(Message))
                {
                    return false;
                }
            }

            return true;
        }

        public bool CheckConnect()
        {
            try
            {
                TcpClient tcpClient = new TcpClient(_Server, Convert.ToInt32(_Port));
                NetworkStream outputStream = tcpClient.GetStream();
                BinaryWriter writer = new BinaryWriter(outputStream);
                writer.Write(true);
                writer.Write(SystemArgs.User.Login);
                tcpClient.Close();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public String GetPathTempFile(String FileName, int Index)
        {
            Image myImage = Image.FromFile(FileName);
            Bitmap source = new Bitmap(myImage);
            Bitmap CroppedImage = source.Clone(new System.Drawing.Rectangle(source.Width / 2, source.Height / 2, source.Width / 2, source.Height / 2), source.PixelFormat);
            string path = @"TempFile\" + Index + ".jpg";
            CroppedImage = new Bitmap(CroppedImage, new Size(source.Width / 5, source.Height / 5));
            CroppedImage.Save(path);
            CroppedImage.Dispose();
            myImage.Dispose();
            return path;
        }

        private bool AddDecodeSession(String DataMatrix)
        {
            if (DataMatrix.Split('_').Length != 6)
            {
                return false;
            }
            else
            {
                if (CheckedUniqueList(DataMatrix))
                {
                    Int64 IDStatus = (from p in SystemArgs.Statuses
                                      where p.IDPosition == SystemArgs.User.GetPosition().ID
                                      select p.ID).Single();
                    if (SystemArgs.Request.CheckedStatusOrderDB(IDStatus,DataMatrix))
                    {
                        _DecodeSession.Add(new OrderScanSession(DataMatrix, true));
                    }
                    else
                    {
                        _DecodeSession.Add(new OrderScanSession(DataMatrix, false));
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
