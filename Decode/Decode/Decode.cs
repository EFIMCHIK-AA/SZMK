using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using Bytescout.BarCodeReader;
using System.Threading.Tasks;

namespace Decode
{
    public class Decode
    {
        public String TIFF(string path)
        {
            try
            {
                using (Reader reader = new Reader())
                {
                    reader.BarcodeTypesToFind.DataMatrix = true;
                    FoundBarcode[] barcodes = reader.ReadFrom(path);
                    string cash = null;
                    foreach (FoundBarcode code in barcodes)
                    {
                        cash = code.Value;
                    }
                    if (cash != null)
                    {
                        var fromEncodind = Encoding.GetEncoding("ISO-8859-1");//из какой кодировки
                        var bytes = fromEncodind.GetBytes(cash);
                        var toEncoding = Encoding.GetEncoding(1251);//в какую кодировку
                        cash = toEncoding.GetString(bytes);
                        while (cash.IndexOf("<FNC1>") != -1)
                            cash = cash.Replace("<FNC1>", "и");
                        cash = cash.Remove(cash.IndexOf('>'), cash.IndexOf('<') - cash.IndexOf('>') + 1);
                        string[] check = cash.Split('_');
                        int list = Convert.ToInt32(check[1]);
                        cash = check[0] + "_" + list + "_" + check[2] + "_" + check[3] + "_" + check[4] + "_" + check[5];
                        return cash;
                    }
                    else
                    {
                        cash = "Не правильно";
                        return cash;
                    }

                }
            }
            catch (Exception e)
            {
                SystemArgs.PrintLog(e.Message);
                return e.Message;
            }
        }
    }
}
