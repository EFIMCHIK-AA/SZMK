﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Decode
{
    public partial class Decode_F : Form
    {
        public Decode_F()
        {
            InitializeComponent();
        }

        private void Decode_F_Load(object sender, EventArgs e)
        {
            SystemArgs.Decode = new Decode();
            SystemArgs.Path = new PathProgramm();
            SystemArgs.Server = new Server();
            if (SystemArgs.Server.GetParametersConnect())
            {
                if (SystemArgs.Server.Start())
                {
                    SystemArgs.Server.Load += AppendToTB;
                }
                else
                {
                    MessageBox.Show("Сервер не был запущен", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Не удалось прочитать данные для запуска сервера", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Setting_TSM_Click(object sender, EventArgs e)
        {
            Setting_F Dialog = new Setting_F();
            Dialog.IP_TB.Text = Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString();
            Dialog.Port_TB.Text = SystemArgs.Server._Port;
            if (Dialog.ShowDialog() == DialogResult.OK)
            {
                
            }
        }
        private void AppendToTB(String Text)
        {
            Status_TB.Invoke((MethodInvoker)delegate ()
            {
                Status_TB.AppendText(Text+Environment.NewLine);
            });
        }
        private void Closing(object sender, FormClosingEventArgs e)
        {
            notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
            notifyIcon1.BalloonTipText = "Нажмите, чтобы отобразить окно";
            notifyIcon1.BalloonTipTitle = "Подсказка";
            notifyIcon1.ShowBalloonTip(12);
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            e.Cancel = true;
        }

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            this.ShowInTaskbar = true;
            this.WindowState = FormWindowState.Normal;
        }

        private void Exit_TSM_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}