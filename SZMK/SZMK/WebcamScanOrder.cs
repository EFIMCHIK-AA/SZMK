using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using ZXing;

namespace SZMK
{
    public class WebcamScanOrder
    {
        private VideoCaptureDevice videoSource;
        private BarcodeReader reader;
        private String Device;
        delegate void SetStringDelegate(String parameter);
        public delegate void LoadData(List<OrderScanSession> ScanSession);
        public event LoadData LoadResult;
        public delegate void LoadVideo(Bitmap Frame);
        public event LoadVideo LoadFrame;
        private List<OrderScanSession> _ScanSession;

        public WebcamScanOrder()
        {
            try
            {
                GetDevice();
                _ScanSession = new List<OrderScanSession>();
            }
            catch(Exception E)
            {
                SystemArgs.PrintLog(E.ToString());
                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public bool GetDevice()
        {
            try
            {
                XDocument doc = XDocument.Load(SystemArgs.Path.WebCamDevice);
                Device = doc.Element("Device").Value;
                return true;
            }
            catch(Exception E)
            {
                SystemArgs.PrintLog(E.ToString());
                throw new Exception(E.Message);
            }
        }

        public bool Start()
        {
            try
            {
                reader = new BarcodeReader();
                reader.Options.PossibleFormats = new List<BarcodeFormat>();
                reader.Options.PossibleFormats.Add(ZXing.BarcodeFormat.QR_CODE);
                reader.Options.PossibleFormats.Add(ZXing.BarcodeFormat.DATA_MATRIX);

                videoSource = new VideoCaptureDevice(Device);
                videoSource.NewFrame += new NewFrameEventHandler(video_NewFrame);
                videoSource.Start();
                if (videoSource.IsRunning)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception E)
            {
                SystemArgs.PrintLog(E.ToString());
                return false;
            }
        }
        private void video_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {
                Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();
                LoadFrame?.Invoke(bitmap);

                Result result = reader.Decode((Bitmap)eventArgs.Frame.Clone());
                if (result != null)
                {
                    SetResult(result.Text);
                }
            }
            catch
            {

            }

        }
        void SetResult(string result)
        {
            var fromEncodind = Encoding.GetEncoding("ISO-8859-1");//из какой кодировки
            var bytes = fromEncodind.GetBytes(result);
            var toEncoding = Encoding.GetEncoding(1251);//в какую кодировку

            result = toEncoding.GetString(bytes);

            while (result.IndexOf("<FNC1>") != -1)
            {
                result = result.Replace("<FNC1>", "и");
            }

            if (CheckedUniqueList(result))
            {
                try
                {
                    String Temp = result.Replace(" ", "");
                    String ReplaceMark = "";

                    String[] ValidationDataMatrix = Temp.Split('_');
                    String[] ExistingCharaterEnglish = new String[] { "A", "a", "B", "C", "c", "E", "e", "H", "K", "M", "O", "o", "P", "p", "T" };
                    String[] ExistingCharaterRussia = new String[] { "А", "а", "В", "С", "с", "Е", "е", "Н", "К", "М", "О", "о", "Р", "р", "Т" };

                    if (ValidationDataMatrix.Length != 6)
                    {
                        throw new Exception("В DataMatrix менее 6 полей");
                    }

                    for (int i = 0; i < ExistingCharaterRussia.Length; i++)
                    {
                        ReplaceMark = ValidationDataMatrix[2].Replace(ExistingCharaterRussia[i], ExistingCharaterEnglish[i]);
                    }

                    String[] Splitter = ValidationDataMatrix[1].Split('и');

                    while (Splitter[0][0] == '0')
                    {
                        Splitter[0] = Splitter[0].Remove(0, 1);
                    }

                    if (Splitter.Length != 1)
                    {
                        ValidationDataMatrix[1] = Splitter[0] + "и" + Splitter[1];
                    }
                    else
                    {
                        ValidationDataMatrix[1] = Splitter[0];
                    }

                    Temp = ValidationDataMatrix[0] + "_" + ValidationDataMatrix[1] + "_" + ReplaceMark + "_" + ValidationDataMatrix[3] + "_" + ValidationDataMatrix[4].Replace(".", ",") + "_" + ValidationDataMatrix[5].Replace(".", ",");

                    Int32 IndexException = SystemArgs.RequestLinq.CheckedNumberAndList(ValidationDataMatrix[0], ValidationDataMatrix[1], Temp);

                    switch (IndexException)
                    {
                        case 0:
                            if (SystemArgs.Request.CheckedNumberAndMark(ValidationDataMatrix[0], ReplaceMark))
                            {
                                if (SystemArgs.ClientProgram.CheckMarks)
                                {
                                    if (CheckedLowerRegistery(ReplaceMark))
                                    {
                                        _ScanSession.Add(new OrderScanSession(Temp, 2, "-"));
                                    }
                                    else
                                    {
                                        _ScanSession.Add(new OrderScanSession(Temp, 0, $"Наименование марки «{ReplaceMark}» не допускается"));
                                    }
                                }
                                else
                                {
                                    _ScanSession.Add(new OrderScanSession(Temp, 2, "-"));
                                }
                            }
                            else
                            {
                                _ScanSession.Add(new OrderScanSession(Temp, 0, $"В заказе {ValidationDataMatrix[0]}, марка {ReplaceMark} уже существует."));
                            }

                            LoadResult?.Invoke(_ScanSession);
                            break;
                        case 1:
                            _ScanSession.Add(new OrderScanSession(Temp, 0, $"В заказе {ValidationDataMatrix[0]}, номер листа {ValidationDataMatrix[1]} уже существует."));
                            LoadResult?.Invoke(_ScanSession);
                            break;
                        case 2:
                            if (SystemArgs.ClientProgram.CheckMarks)
                            {
                                if (CheckedLowerRegistery(ReplaceMark))
                                {
                                    _ScanSession.Add(new OrderScanSession(Temp, 2, "-"));
                                }
                                else
                                {
                                    _ScanSession.Add(new OrderScanSession(Temp, 0, $"Наименование марки «{ReplaceMark}» не допускается"));
                                }
                            }
                            else
                            {
                                _ScanSession.Add(new OrderScanSession(Temp, 2, "-"));
                            }

                            LoadResult?.Invoke(_ScanSession);
                            break;
                        case 3:
                            if (SystemArgs.ClientProgram.CheckMarks)
                            {
                                if (CheckedLowerRegistery(ReplaceMark))
                                {
                                    _ScanSession.Add(new OrderScanSession(Temp, 1, "-"));
                                }
                                else
                                {
                                    _ScanSession.Add(new OrderScanSession(Temp, 0, $"Наименование марки «{ReplaceMark}» не допускается"));
                                }
                            }
                            else
                            {
                                _ScanSession.Add(new OrderScanSession(Temp, 1, "-"));
                            }

                            LoadResult?.Invoke(_ScanSession);
                            break;
                    }
                }
                catch (Exception E)
                {
                    SystemArgs.PrintLog(E.ToString());
                    MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private bool CheckedLowerRegistery(String Mark)
        {
            String LowerCharacters = "абвгдеёжзийклмнопрстуфхцчшщъыьэюяabcdefghijklmnopqrstuvwxyz";
            for (int i = 0; i < Mark.Length; i++)
            {
                if (LowerCharacters.IndexOf(Mark[i]) != -1)
                {
                    return false;
                }
            }
            return true;

        }
        List<String> UniqueList = new List<string>();
        private bool CheckedUniqueList(String Temp)
        {
            if (UniqueList.Contains(Temp))
            {
                return false;
            }
            else
            {
                UniqueList.Add(Temp);
                return true;
            }
        }
        public bool Stop()
        {
            try
            {

                if (videoSource != null && videoSource.IsRunning)
                {
                    videoSource.SignalToStop();
                    videoSource.WaitForStop();
                    videoSource = null;
                }
                return true;
            }
            catch (Exception E)
            {
                SystemArgs.PrintLog(E.ToString());
                return false;
            }
        }
        public List<OrderScanSession> GetScanSessions()
        {
            return _ScanSession;
        }
    }
}
