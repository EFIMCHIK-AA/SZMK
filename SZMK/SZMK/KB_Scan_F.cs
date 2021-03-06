﻿using System;
using SimpleTCP;
using System.Net;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using OfficeOpenXml;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SZMK
{
    public partial class KB_Scan_F : Form
    {
        public KB_Scan_F()
        {
            InitializeComponent();
        }
        private void Scan_F_Load(object sender, EventArgs e)
        {
            Scan_DGV.AutoGenerateColumns = false;
            Scan_DGV.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            if (SystemArgs.ClientProgram.UsingWebCam)
            {
                ViewWeb_PB.SizeMode = PictureBoxSizeMode.Zoom;
                SystemArgs.WebcamScanOrder.LoadResult += LoadToDGV;
                SystemArgs.WebcamScanOrder.LoadFrame += LoadFrame;
            }
            else
            {
                SystemArgs.ServerMobileAppOrder.Load += LoadToDGV;
            }
            EnableButton(false);
        }

        private void CheckedUnloading_TSM_Click(object sender, EventArgs e)
        {
            List<OrderScanSession> Temp;
            if (SystemArgs.ClientProgram.UsingWebCam)
            {
                Temp = SystemArgs.WebcamScanOrder.GetScanSessions();
            }
            else
            {
                Temp = SystemArgs.ServerMobileAppOrder.GetScanSessions();
            }
            if (Temp.Count != 0)
            {
                try
                {
                    if (SystemArgs.UnLoadSpecific.SearchFileUnloading(Temp.Select(p => p.DataMatrix).ToList()))
                    {
                        if (SystemArgs.UnLoadSpecific.ExecutorMails.Count != 0)
                        {
                            KB_ScanUnloadSpecific Dialog = new KB_ScanUnloadSpecific();
                            Dialog.ShowDialog();
                        }
                        else
                        {
                            MessageBox.Show("При проверки выгрузки не было найдено ни одного совпадения номера заказа с листом", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                catch (Exception E)
                {
                    MessageBox.Show("Файл был указан не верно или не хватило прав доступа к файлу", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    SystemArgs.PrintLog(E.ToString());
                    return;
                }
            }
            else
            {
                MessageBox.Show("Невозможно проверить выгрузку, нет данных", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void CreateAct_TSM_Click(object sender, EventArgs e)
        {
            List<OrderScanSession> Temp;
            if (SystemArgs.ClientProgram.UsingWebCam)
            {
                Temp = SystemArgs.WebcamScanOrder.GetScanSessions();
            }
            else
            {
                Temp = SystemArgs.ServerMobileAppOrder.GetScanSessions();
            }
            if (Temp.Count != 0)
            {
                if (SystemArgs.Excel.CreateAndExportActsKB(Temp))
                {
                    MessageBox.Show("Акты успешно сформированы и сохранены", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                };
            }
            else
            {
                MessageBox.Show("Невозможно сформировать акт, нет данных", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

        }

        private void KBScan_F_FormClosing(object sender, FormClosingEventArgs e)
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
        private void ClosedServer()
        {
            if (SystemArgs.ClientProgram.UsingWebCam)
            {
                if (SystemArgs.WebcamScanOrder.Stop())
                {
                    Status_TB.AppendText($"Выключение камеры" + Environment.NewLine);
                    SystemArgs.WebcamScanOrder.LoadResult -= LoadToDGV;
                    SystemArgs.WebcamScanOrder.LoadFrame -= LoadFrame;
                }
            }
            else
            {
                if (SystemArgs.ServerMobileAppOrder.Stop())
                {
                    Status_TB.AppendText($"Закрытие сервера" + Environment.NewLine);
                    SystemArgs.ServerMobileAppOrder.Load -= LoadToDGV;
                }
            }
        }
        private void LoadFrame(Bitmap Frame)
        {
           ViewWeb_PB.Image = Frame;
        }

        private void LoadToDGV(List<OrderScanSession> ScanSessions)
        {
            Scan_DGV.Invoke((MethodInvoker)delegate ()
            {
                SessionCount_TB.Text = ScanSessions.Count().ToString();
                LoadStatusOperation(ScanSessions[ScanSessions.Count - 1].DataMatrix);
                Scan_DGV.Rows.Add();
                Scan_DGV[0, Scan_DGV.Rows.Count - 1].Value = ScanSessions[ScanSessions.Count - 1].DataMatrix;
                if (ScanSessions[ScanSessions.Count - 1].Unique==2)
                {
                    Scan_DGV[1, Scan_DGV.Rows.Count - 1].Value = "Уникален";
                    Scan_DGV[1, Scan_DGV.Rows.Count - 1].Style.BackColor = Color.Lime;
                    Scan_DGV[2, Scan_DGV.Rows.Count - 1].Value = ScanSessions[ScanSessions.Count - 1].Discription;
                    Scan_DGV[2, Scan_DGV.Rows.Count - 1].Style.BackColor = Color.Lime;
                }
                else if (ScanSessions[ScanSessions.Count - 1].Unique == 1)
                {
                    Scan_DGV[1, Scan_DGV.Rows.Count - 1].Value = "Обновление";
                    Scan_DGV[1, Scan_DGV.Rows.Count - 1].Style.BackColor = Color.Orange;
                    Scan_DGV[2, Scan_DGV.Rows.Count - 1].Value = ScanSessions[ScanSessions.Count-1].Discription;
                    Scan_DGV[2, Scan_DGV.Rows.Count - 1].Style.BackColor = Color.Orange;
                }
                else
                {
                    Scan_DGV[1, Scan_DGV.Rows.Count - 1].Value = "Не уникален";
                    Scan_DGV[1, Scan_DGV.Rows.Count - 1].Style.BackColor = Color.Red;
                    Scan_DGV[2, Scan_DGV.Rows.Count - 1].Value = ScanSessions[ScanSessions.Count - 1].Discription;
                    Scan_DGV[2, Scan_DGV.Rows.Count - 1].Style.BackColor = Color.Red;
                }
            });
        }
        private void LoadStatusOperation(String DataMatrix)
        {
            Status_TB.AppendText(DataMatrix + Environment.NewLine);
        }

        private void Scan_DGV_SelectionChanged(object sender, EventArgs e)
        {
            Scan_DGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            if (Scan_DGV.Rows.Count > 0)
            {
                EnableButton(true);
            }
            else
            {
                EnableButton(false);
            }
        }

        private void Scan_DGV_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            e.CellStyle.SelectionBackColor = Color.FromArgb(112, 238, 226);
            e.CellStyle.SelectionForeColor = Color.Black;
        }
        private void EnableButton(Boolean flag)
        {
            if (flag)
            {
                Add_B.Enabled = true;
                CreateAct_TSM.Enabled = true;
                CheckedUnloading_TSM.Enabled = true;
            }
            else
            {
                Add_B.Enabled = false;
                CreateAct_TSM.Enabled = false;
                CheckedUnloading_TSM.Enabled = false;
            }
        }
    }
}
