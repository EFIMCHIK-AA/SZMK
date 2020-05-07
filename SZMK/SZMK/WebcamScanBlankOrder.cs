using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Threading;
using System.Xml.Linq;
using ZXing;

namespace SZMK
{
    public class WebcamScanBlankOrder
    {
        private Boolean _Added;
        private VideoCaptureDevice videoSource;
        private BarcodeReader reader;
        private String Device;
        public delegate void LoadData(List<BlankOrderScanSession> ScanSession);
        public event LoadData LoadResult;
        public delegate void LoadStatus(String QRBlankOrder);
        public event LoadStatus Status;
        public delegate void LoadVideo(Bitmap Frame);
        public event LoadVideo LoadFrame;
        private List<BlankOrderScanSession> _ScanSession;

        public WebcamScanBlankOrder(Boolean Added)
        {
            try
            {
                GetDevice();
                _Added = Added;
                _ScanSession = new List<BlankOrderScanSession>();
            }
            catch (Exception E)
            {
                SystemArgs.PrintLog(E.ToString());
                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public Boolean Added
        {
            get
            {
                return _Added;
            }
            set
            {
                _Added = value;
            }
        }
        public List<BlankOrderScanSession> GetScanSessions()
        {
            return _ScanSession;
        }
        public void ClearData()
        {
            _ScanSession.Clear();
        }

        public bool GetDevice()
        {
            try
            {
                XDocument doc = XDocument.Load(SystemArgs.Path.WebCamDevice);
                Device = doc.Element("Device").Value;
                return true;
            }
            catch (Exception E)
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
            catch (Exception E)
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
            try
            {
                String Temp = result.Replace("\u00a0", "").Replace(" ", "");
                String[] ValidationDataMatrix = Temp.Split('_');
                if (ValidationDataMatrix.Length >= 4)
                {
                    Regex regex = new Regex(@"\d*-\d*-\d*");
                    MatchCollection matches = regex.Matches(ValidationDataMatrix[1]);

                    if (matches.Count > 0)
                    {
                        Temp = ValidationDataMatrix[0] + "_СЗМК";
                        for (int i = 1; i < ValidationDataMatrix.Length; i++)
                        {
                            Temp += "_" + ValidationDataMatrix[i];
                        }
                        ValidationDataMatrix = Temp.Split('_');
                    }

                    if (CheckedUniqueList(Temp))
                    {
                        _ScanSession.Add(new BlankOrderScanSession(true, Temp));

                        if (Added)
                        {
                            if (ValidationDataMatrix[0] != "ПО")
                            {
                                if (ValidationDataMatrix[1] != "СЗМК" && ValidationDataMatrix[0] == "БЗ")
                                {
                                    if (!FindedOrderBlankProvider(ValidationDataMatrix, Temp))
                                    {
                                        throw new Exception("Ошибка определения чертежей в бланке поставщику");
                                    }
                                }
                                else if (ValidationDataMatrix[0] == "БЗ")
                                {
                                    if (!FindedOrderBlankOrder(ValidationDataMatrix, Temp))
                                    {
                                        throw new Exception("Ошибка определения чертежей в бланке заказа");
                                    }
                                }
                                else
                                {
                                    _ScanSession[_ScanSession.Count - 1].Added = false;
                                    throw new Exception("Неверный формат бланка заказа");
                                }
                            }
                            else
                            {
                                if (!FindedOrderCreditOrder(ValidationDataMatrix, Temp))
                                {
                                    throw new Exception("Ошибка определения чертежей в приходном ордере");
                                }
                            }
                        }
                        else if(ValidationDataMatrix[0] == "БЗ")
                        {
                            if (!FindedBlankOrder_OPP(ValidationDataMatrix, Temp))
                            {
                                throw new Exception("Ошибка определения чертежей в бланке заказа");
                            }
                        }
                        else
                        {
                            throw new Exception("Неверный формат бланка заказа");
                        }
                    }
                }
                else
                {
                    _ScanSession[_ScanSession.Count - 1].Added = false;
                    throw new Exception("В QR менее 4 полей");
                }

            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private bool CheckedUniqueList(String Temp)
        {
            foreach (var item in _ScanSession)
            {
                if (item.QRBlankOrder.Equals(Temp))
                {
                    return false;
                }
            }
            return true;
        }

        private bool FindedOrderCreditOrder(String[] ValidationDataMatrix, String QRBlankOrder)
        {
            try
            {
                ValidationDataMatrix[2] = ReplaceNumber(ValidationDataMatrix[2]);
                Status?.Invoke(QRBlankOrder);
                for (int i = 5; i < ValidationDataMatrix.Length; i++)
                {
                    if (SystemArgs.Request.CheckedOrder(ValidationDataMatrix[2], ValidationDataMatrix[i]) && SystemArgs.Request.CheckedStatusOrderDB(ValidationDataMatrix[2], ValidationDataMatrix[i]) == SystemArgs.User.StatusesUser[0].ID && SystemArgs.Request.CheckedExecutorWork(ValidationDataMatrix[2], ValidationDataMatrix[i], QRBlankOrder))
                    {
                        _ScanSession[_ScanSession.Count - 1].GetNumberAndLists().Add(new BlankOrderScanSession.NumberAndList(ValidationDataMatrix[2], ValidationDataMatrix[i], 1));
                    }
                    else if (SystemArgs.Request.CheckedStatusOrderDB(ValidationDataMatrix[2], ValidationDataMatrix[i]) >= SystemArgs.User.StatusesUser[1].ID && SystemArgs.Request.CheckedExecutorWork(ValidationDataMatrix[2], ValidationDataMatrix[i], QRBlankOrder))
                    {
                        _ScanSession[_ScanSession.Count - 1].GetNumberAndLists().Add(new BlankOrderScanSession.NumberAndList(ValidationDataMatrix[2], ValidationDataMatrix[i], 0));
                    }
                    else
                    {
                        _ScanSession[_ScanSession.Count - 1].GetNumberAndLists().Add(new BlankOrderScanSession.NumberAndList(ValidationDataMatrix[2], ValidationDataMatrix[i], -1));
                    }

                    if ((_ScanSession[_ScanSession.Count - 1].GetNumberAndLists().Where(p => p.Finded == -1).Count() == 0) && (_ScanSession[_ScanSession.Count - 1].GetNumberAndLists().Where(p => p.Finded == 0).Count() == 0))
                    {
                        _ScanSession[_ScanSession.Count - 1].Added = true;
                    }
                    else
                    {
                        _ScanSession[_ScanSession.Count - 1].Added = false;
                    }
                }
                LoadResult?.Invoke(_ScanSession);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool FindedOrderBlankProvider(String[] ValidationDataMatrix, String QRBlankOrder)
        {
            try
            {
                ValidationDataMatrix[2] = ReplaceNumber(ValidationDataMatrix[2]);
                Status?.Invoke(QRBlankOrder);
                for (int i = 5; i < ValidationDataMatrix.Length; i++)
                {
                    if (SystemArgs.Request.CheckedOrder(ValidationDataMatrix[2], ValidationDataMatrix[i]) && (SystemArgs.Request.CheckedStatusOrderDB(ValidationDataMatrix[2], ValidationDataMatrix[i]) == SystemArgs.User.StatusesUser[0].ID - 1 || SystemArgs.Request.CheckedStatusOrderDB(ValidationDataMatrix[2], ValidationDataMatrix[i]) == SystemArgs.User.StatusesUser[1].ID))
                    {
                        _ScanSession[_ScanSession.Count - 1].GetNumberAndLists().Add(new BlankOrderScanSession.NumberAndList(ValidationDataMatrix[2], ValidationDataMatrix[i], 1));
                    }
                    else if (SystemArgs.Request.CheckedStatusOrderDB(ValidationDataMatrix[2], ValidationDataMatrix[i]) >= SystemArgs.User.StatusesUser[0].ID)
                    {
                        _ScanSession[_ScanSession.Count - 1].GetNumberAndLists().Add(new BlankOrderScanSession.NumberAndList(ValidationDataMatrix[2], ValidationDataMatrix[i], 0));
                    }
                    else
                    {
                        _ScanSession[_ScanSession.Count - 1].GetNumberAndLists().Add(new BlankOrderScanSession.NumberAndList(ValidationDataMatrix[2], ValidationDataMatrix[i], -1));
                    }

                    if ((_ScanSession[_ScanSession.Count - 1].GetNumberAndLists().Where(p => p.Finded == -1).Count() == 0) && (_ScanSession[_ScanSession.Count - 1].GetNumberAndLists().Where(p => p.Finded == 0).Count() == 0))
                    {
                        _ScanSession[_ScanSession.Count - 1].Added = true;
                    }
                    else
                    {
                        _ScanSession[_ScanSession.Count - 1].Added = false;
                    }
                }
                LoadResult?.Invoke(_ScanSession);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool FindedOrderBlankOrder(String[] ValidationDataMatrix, String QRBlankOrder)
        {
            try
            {
                ValidationDataMatrix[2] = ReplaceNumber(ValidationDataMatrix[2]);
                Status?.Invoke(QRBlankOrder);

                for (int i = 5; i < ValidationDataMatrix.Length; i++)
                {
                    if (SystemArgs.Request.CheckedOrder(ValidationDataMatrix[2], ValidationDataMatrix[i]) && (SystemArgs.Request.CheckedStatusOrderDB(ValidationDataMatrix[2], ValidationDataMatrix[i]) == SystemArgs.User.StatusesUser[0].ID - 1 || SystemArgs.Request.CheckedStatusOrderDB(ValidationDataMatrix[2], ValidationDataMatrix[i]) == SystemArgs.User.StatusesUser[1].ID))
                    {
                        _ScanSession[_ScanSession.Count - 1].GetNumberAndLists().Add(new BlankOrderScanSession.NumberAndList(ValidationDataMatrix[2], ValidationDataMatrix[i], 1));
                    }
                    else if (SystemArgs.Request.CheckedStatusOrderDB(ValidationDataMatrix[2], ValidationDataMatrix[i]) >= SystemArgs.User.StatusesUser[2].ID)
                    {
                        _ScanSession[_ScanSession.Count - 1].GetNumberAndLists().Add(new BlankOrderScanSession.NumberAndList(ValidationDataMatrix[2], ValidationDataMatrix[i], 0));
                    }
                    else
                    {
                        _ScanSession[_ScanSession.Count - 1].GetNumberAndLists().Add(new BlankOrderScanSession.NumberAndList(ValidationDataMatrix[2], ValidationDataMatrix[i], -1));
                    }

                    if ((_ScanSession[_ScanSession.Count - 1].GetNumberAndLists().Where(p => p.Finded == -1).Count() == 0))
                    {
                        _ScanSession[_ScanSession.Count - 1].Added = true;
                    }
                    else
                    {
                        _ScanSession[_ScanSession.Count - 1].Added = false;
                    }
                }
                LoadResult?.Invoke(_ScanSession);
                return true;
            }
            catch
            {
                return false;
            }
        }
        private bool FindedBlankOrder_OPP(String[] ValidationDataMatrix, String QRBlankOrder)
        {
            try
            {
                ValidationDataMatrix[2] = ReplaceNumber(ValidationDataMatrix[2]);
                Status?.Invoke(QRBlankOrder);

                for (int i = 5; i < ValidationDataMatrix.Length; i++)
                {
                    if (SystemArgs.Request.FindedOrdersInAddBlankOrder(QRBlankOrder, ValidationDataMatrix[2], ValidationDataMatrix[i]))
                    {
                        if (SystemArgs.Request.CheckedOrder(ValidationDataMatrix[2], ValidationDataMatrix[i]) && SystemArgs.Request.CheckedStatusOrderDB(ValidationDataMatrix[2], ValidationDataMatrix[i]) == SystemArgs.User.StatusesUser[0].ID - 1)
                        {
                            _ScanSession[_ScanSession.Count - 1].GetNumberAndLists().Add(new BlankOrderScanSession.NumberAndList(ValidationDataMatrix[2], ValidationDataMatrix[i], 1));
                        }
                        else if (SystemArgs.Request.CheckedStatusOrderDB(ValidationDataMatrix[2], ValidationDataMatrix[i]) > SystemArgs.User.StatusesUser[0].ID - 1)
                        {
                            _ScanSession[_ScanSession.Count - 1].GetNumberAndLists().Add(new BlankOrderScanSession.NumberAndList(ValidationDataMatrix[2], ValidationDataMatrix[i], 0));
                        }
                        else
                        {
                            _ScanSession[_ScanSession.Count - 1].GetNumberAndLists().Add(new BlankOrderScanSession.NumberAndList(ValidationDataMatrix[2], ValidationDataMatrix[i], -1));
                        }
                    }
                    else
                    {
                        _ScanSession[_ScanSession.Count - 1].GetNumberAndLists().Add(new BlankOrderScanSession.NumberAndList(ValidationDataMatrix[2], ValidationDataMatrix[i], -1));
                    }

                    if ((_ScanSession[_ScanSession.Count - 1].GetNumberAndLists().Where(p => p.Finded == -1).Count() == 0) && (_ScanSession[_ScanSession.Count - 1].GetNumberAndLists().Where(p => p.Finded == 1).Count() > 0))
                    {
                        _ScanSession[_ScanSession.Count - 1].Added = true;
                    }
                    else
                    {
                        _ScanSession[_ScanSession.Count - 1].Added = false;
                    }
                }
                LoadResult?.Invoke(_ScanSession);
                return true;
            }
            catch
            {
                return false;
            }
        }
        private String ReplaceNumber(String Number)
        {
            Number = Number.Remove(Number.IndexOf('-'), Number.Remove(0, Number.IndexOf('-') + 1).IndexOf('-') + 1);//Удаляю подстроку с шаблоном -*- отставляя полседнее -
            Number = Number.Replace('-', '(') + ")";//Заменяю - на скобочку ( и добавляю )
            return Number;
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

    }
}
