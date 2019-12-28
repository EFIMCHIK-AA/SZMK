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
            SystemArgs.Template = new Template();
            SystemArgs.ActExcel = new Excel();
            SystemArgs.UnLoadSpecific = new UnLoadSpecific();
            SystemArgs.ServerMobileApp.Load += LoadToDGV;
        }

        private void CheckedUnloading_TSM_Click(object sender, EventArgs e)
        {
            if(SystemArgs.ServerMobileApp._ScanSession.Count!=0)
                try
                {
                    SystemArgs.UnLoadSpecific.ChekedUnloading(SystemArgs.ServerMobileApp._ScanSession);
                    KBScanUnloadSpecific Dialog = new KBScanUnloadSpecific();
                    Dialog.Show();
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
            if (SystemArgs.ServerMobileApp._ScanSession.Count != 0)
            {
                if (SystemArgs.ActExcel.CreateAndExportActs(SystemArgs.ServerMobileApp._ScanSession))
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
            if (SystemArgs.ServerMobileApp.Stop())
            {
                Status_TB.AppendText($"Закрытие сервера" + Environment.NewLine);
                SystemArgs.ServerMobileApp.Load -= LoadToDGV;
            }
        }
        private void LoadToDGV(List<ScanSession> ScanSessions)
        {
            Scan_DGV.Invoke((MethodInvoker)delegate ()
            {
                Scan_DGV.DataSource = null;
                Scan_DGV.DataSource = ScanSessions;
                LoadStatusOperation(ScanSessions[ScanSessions.Count - 1].DataMatrix);
                for (int i = 0; i < Scan_DGV.Rows.Count; i++)
                {
                    if (Convert.ToBoolean(Scan_DGV[1, i].Value))
                    {
                        Scan_DGV[1, i].Style.BackColor = Color.Lime;
                    }
                    else
                    {
                        Scan_DGV[1, i].Style.BackColor = Color.Red;
                    }
                }
            });
        }
        private void LoadStatusOperation(String DataMatrix)
        {
            Status_TB.AppendText($"Отсканирован чертеж: " + DataMatrix + Environment.NewLine);
        }
    }
}
