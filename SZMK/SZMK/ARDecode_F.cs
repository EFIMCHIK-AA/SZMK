using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace SZMK
{
    public partial class ARDecode_F : Form
    {
        public ARDecode_F()
        {
            InitializeComponent();
        }
        public List<String> FileNames;

        private void Change_B_Click(object sender, EventArgs e)
        {
            OpenFileDialog Opd = new OpenFileDialog
            {
                Multiselect = true,
                Title = "Выберите сканы чертежей",
                Filter = "TIF|*.tif|TIFF|*.tiff"
            };

            string date = DateTime.Now.ToString();
            date = date.Replace(".", "_").Replace(":", "_");

            if (Opd.ShowDialog() == DialogResult.OK)
            {
                if (Opd.FileName == String.Empty)
                {
                    return;
                }

                Directory.CreateDirectory("TempFile");

                Int32 CountFile = Opd.FileNames.Length;
                Int32 i = 0;

                foreach (String FileName in Opd.FileNames)
                {
                    Status_TB.AppendText($"Файл" +Environment.NewLine + SystemArgs.Path.GetFileName(FileName) + "обрабатывается, пожалуйста подождите..." + Environment.NewLine);

                    Status_TB.AppendText($">{i + 1}|{CountFile}<" + Environment.NewLine);

                    String CurrentInfoDataMatrix = "";
                    CurrentInfoDataMatrix=SystemArgs.ByteScout.SendAndRead(SystemArgs.ByteScout.GetPathTempFile(FileName, i),SystemArgs.Path.GetFileName(FileName));
                    FileNames.Add(FileName);
                    i++;
                }
                DeleteFilesAndDirectory();
                Status_TB.AppendText($"ОБРАБОТКА ЗАВЕРШЕНА!" + Environment.NewLine);
                Status_TB.AppendText($">{i}|{CountFile}<" + Environment.NewLine);
            }
        }

        private void ARDecode_F_Load(object sender, EventArgs e)
        {
            FileNames = new List<string>();
            Scan_DGV.AutoGenerateColumns = false;
            Scan_DGV.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            SystemArgs.ByteScout.Load += LoadToDGVAndTB;
            SystemArgs.ByteScout.Fail += StatusFailText;
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
        private bool DeleteFilesAndDirectory()
        {
            try
            {
                System.IO.DirectoryInfo di = new DirectoryInfo("TempFile");

                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }

                Directory.Delete("TempFile");
                return true;
            }
            catch
            {
                return false;
            }
        }
        private void StatusFailText(String Path)
        {
            Status_TB.AppendText($"Файл {SystemArgs.Path.GetFileName(Path)} неправильный формат DataMatrix" + Environment.NewLine);
        }
        private void LoadToDGVAndTB(List<ScanSession> DecodeSession)
        {
            Status_TB.AppendText($"Получены данные"+Environment.NewLine+ DecodeSession[DecodeSession.Count - 1].DataMatrix + Environment.NewLine);
            Scan_DGV.Rows.Add();
            Scan_DGV[0, Scan_DGV.Rows.Count - 1].Value = DecodeSession[DecodeSession.Count - 1].DataMatrix;
            if (DecodeSession[DecodeSession.Count - 1].Unique)
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

        private void CreateAct_TSM_Click(object sender, EventArgs e)
        {
            if (SystemArgs.ByteScout._DecodeSession.Count != 0)
            {
                if (SystemArgs.Excel.CreateAndExportActs(SystemArgs.ByteScout._DecodeSession, false))
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
    }
}
