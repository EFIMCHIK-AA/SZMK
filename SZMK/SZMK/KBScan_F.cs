using System;
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
    public partial class KBScan_F : Form
    {
        public KBScan_F()
        {
            InitializeComponent();
        }
        private void Scan_F_Load(object sender, EventArgs e)
        {
            Scan_DGV.AutoGenerateColumns = false;
            Scan_DGV.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            SystemArgs.UnLoadSpecific = new UnLoadSpecific();
            SystemArgs.ServerMail = new ServerMail();
            SystemArgs.ServerMobileAppOrder.Load += LoadToDGV;
        }

        private void CheckedUnloading_TSM_Click(object sender, EventArgs e)
        {
            if(SystemArgs.ServerMobileAppOrder._ScanSession.Count!=0)
                try
                {
                    SystemArgs.UnLoadSpecific.ChekedUnloading(SystemArgs.ServerMobileAppOrder._ScanSession);
                    KBScanUnloadSpecific Dialog = new KBScanUnloadSpecific();
                    Dialog.ShowDialog();
                }
                catch(Exception E)
                {
                    MessageBox.Show(E.Message, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            else
            {
                MessageBox.Show("Невозможно проверить выгрузку, нет данных", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void CreateAct_TSM_Click(object sender, EventArgs e)
        {
            if (SystemArgs.ServerMobileAppOrder._ScanSession.Count != 0)
            {
                if (SystemArgs.Excel.CreateAndExportActs(SystemArgs.ServerMobileAppOrder._ScanSession,true))
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
            if (SystemArgs.ServerMobileAppOrder.Stop())
            {
                Status_TB.AppendText($"Закрытие сервера" + Environment.NewLine);
                SystemArgs.ServerMobileAppOrder.Load -= LoadToDGV;
            }
        }
        private void LoadToDGV(List<OrderScanSession> ScanSessions)
        {
            Scan_DGV.Invoke((MethodInvoker)delegate ()
            {
                SessionCount_TB.Text = ScanSessions.Count().ToString();
                LoadStatusOperation(ScanSessions[ScanSessions.Count - 1].DataMatrix);
                Scan_DGV.Rows.Add();
                Scan_DGV[0, Scan_DGV.Rows.Count - 1].Value = ScanSessions[ScanSessions.Count - 1].DataMatrix;
                if (ScanSessions[ScanSessions.Count - 1].Unique)
                {
                    Scan_DGV[1, Scan_DGV.Rows.Count - 1].Value = "Уникален";
                    Scan_DGV[1, Scan_DGV.Rows.Count - 1].Style.BackColor = Color.Lime;
                }
                else
                {
                    Scan_DGV[1, Scan_DGV.Rows.Count - 1].Value = "Не уникален";
                    Scan_DGV[1, Scan_DGV.Rows.Count - 1].Style.BackColor = Color.Red;
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
        }

        private void Scan_DGV_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            e.CellStyle.SelectionBackColor = Color.FromArgb(112, 238, 226);
            e.CellStyle.SelectionForeColor = Color.Black;
        }
    }
}
