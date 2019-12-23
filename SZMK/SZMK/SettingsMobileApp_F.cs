﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SZMK
{
    public partial class SettingsMobileApp_F : Form
    {
        public SettingsMobileApp_F()
        {
            InitializeComponent();
        }

        private void Generate_B_Click(object sender, EventArgs e)
        {
            try
            {
                if(String.IsNullOrEmpty(IP_TB.Text))
                {
                    IP_TB.Focus();
                    throw new Exception("Необходимо ввести IP - адрес");
                }

                if (String.IsNullOrEmpty(Port_TB.Text))
                {
                    Port_TB.Focus();
                    throw new Exception("Необходимо ввести порт");
                }

                Int32 Port = Convert.ToInt32(Port_TB.Text);

                if ((Port >= 48654 && Port <= 48999)||( Port >= 49152 && Port <= 65535))
                {
                    Port_TB.Focus();
                    throw new Exception("Необходимо использовать порты в диапазоне [48654..48999] или [49152..65535]");
                }

                SystemArgs.MobileApplication.IP = IP_TB.Text.Trim();
                SystemArgs.MobileApplication.Port = Port_TB.Text.Trim();

                if (SystemArgs.MobileApplication.SetParametersConnect())
                {
                    Zen.Barcode.CodeQrBarcodeDraw QrCode = Zen.Barcode.BarcodeDrawFactory.CodeQr;
                    QR_PB.Image = QrCode.Draw($"{SystemArgs.MobileApplication.IP}_{SystemArgs.MobileApplication.Port}", 100);
                }
            }
            catch (FormatException)
            {
                Port_TB.Focus();
                MessageBox.Show("Порт подключения должен состоять из целых цифр", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
