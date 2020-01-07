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
    public partial class PDOScan_F : Form
    {
        public PDOScan_F()
        {
            InitializeComponent();
        }

        private void PDOScan_F_Load(object sender, EventArgs e)
        {
            Scan_DGV.AutoGenerateColumns = false;
            Scan_DGV.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            SystemArgs.UnLoadSpecific = new UnLoadSpecific();
            SystemArgs.ServerMobileAppBlankOrder.Load += LoadToDGV;
            SystemArgs.ServerMobileAppBlankOrder.Status += LoadStatusOperation;
        }
        private void ClosedServer()
        {
            if (SystemArgs.ServerMobileAppBlankOrder.Stop())
            {
                Status_TB.AppendText($"Закрытие сервера" + Environment.NewLine);
                SystemArgs.ServerMobileAppBlankOrder.Load -= LoadToDGV;
                SystemArgs.ServerMobileAppBlankOrder.Status -= LoadStatusOperation;
            }
        }
        private void LoadToDGV(List<BlankOrderScanSession> ScanSessions)
        {
            Scan_DGV.Invoke((MethodInvoker)delegate ()
            {
                Scan_DGV.Rows.Add();
                if (ScanSessions[ScanSessions.Count - 1].Added)
                {
                    Scan_DGV[0, Scan_DGV.Rows.Count - 1].Value = ScanSessions[ScanSessions.Count - 1].QRBlankOrder;
                    Scan_DGV[0, Scan_DGV.Rows.Count - 1].Style.BackColor = Color.Lime;
                    Scan_DGV[1, Scan_DGV.Rows.Count - 1].Value = "QR бланка заказа";
                    Scan_DGV[1, Scan_DGV.Rows.Count - 1].Style.BackColor = Color.AliceBlue;
                }
                else
                {
                    Scan_DGV[0, Scan_DGV.Rows.Count - 1].Value = ScanSessions[ScanSessions.Count - 1].QRBlankOrder;
                    Scan_DGV[0, Scan_DGV.Rows.Count - 1].Style.BackColor = Color.Red;
                    Scan_DGV[1, Scan_DGV.Rows.Count - 1].Value = "QR бланка заказа";
                    Scan_DGV[1, Scan_DGV.Rows.Count - 1].Style.BackColor = Color.AliceBlue;
                }
                foreach (BlankOrderScanSession.NumberAndList Order in ScanSessions[ScanSessions.Count - 1]._Order)
                {
                    Scan_DGV.Rows.Add();
                    String TextOrder = "Заказ №" + Order._Number + " Лист №" + Order._List.ToString();
                    Scan_DGV[0, Scan_DGV.Rows.Count - 1].Value = TextOrder;
                    if (Order._Finded)
                    {
                        Scan_DGV[1, Scan_DGV.Rows.Count - 1].Value = "Найден";
                        Scan_DGV[1, Scan_DGV.Rows.Count - 1].Style.BackColor = Color.Lime;
                    }
                    else
                    {
                        Scan_DGV[1, Scan_DGV.Rows.Count - 1].Value = "Не найден";
                        Scan_DGV[1, Scan_DGV.Rows.Count - 1].Style.BackColor = Color.Red;
                    }
                }
            });
        }
        private void LoadStatusOperation(String DataMatrix)
        {
            Status_TB.Invoke((MethodInvoker)delegate ()
            {
                Status_TB.AppendText("Получен: " + DataMatrix + Environment.NewLine);
            });
        }

        private void PDOScan_F_FormClosing(object sender, FormClosingEventArgs e)
        {
            ClosedServer();
        }

        private void Add_B_Click(object sender, EventArgs e)
        {
            ClosedServer();
        }

        private void Cancel_B_Click(object sender, EventArgs e)
        {
            ClosedServer();
        }

        private void Scan_DGV_SelectionChanged(object sender, EventArgs e)
        {
            Scan_DGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void Scan_DGV_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            e.CellStyle.SelectionBackColor = Color.FromArgb(112, 238, 226);
            e.CellStyle.SelectionForeColor = Color.Black;
        }
    }
}