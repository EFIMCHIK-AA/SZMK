using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SZMK
{
    public partial class ADSettingsByteScout_F : Form
    {
        public ADSettingsByteScout_F()
        {
            InitializeComponent();
        }

        private void ReviewProgram_B_Click(object sender, EventArgs e)
        {

            OpenFileDialog OPF = new OpenFileDialog();

            if (OPF.ShowDialog() == DialogResult.OK)
            {
                PrpgramPath_TB.Text = OPF.FileName;
            }
        }

        private void DirectoryProgPath_B_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog Ofd = new FolderBrowserDialog();

            if (Ofd.ShowDialog() == DialogResult.OK)
            {
                DirectoryProgPath_TB.Text = Ofd.SelectedPath;
            }
        }

        private void SaveDirectoryProgPath_B_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(PrpgramPath_TB.Text))
                {
                    PrpgramPath_TB.Focus();
                    throw new Exception("Необходимо указать исполняемый файл программы распознавания");
                }

                if (String.IsNullOrEmpty(DirectoryProgPath_TB.Text))
                {
                    DirectoryProgPath_TB.Focus();
                    throw new Exception("Указать путь до директории распознавания не существует");
                }

                if (!File.Exists(PrpgramPath_TB.Text.Trim()))
                {
                    PrpgramPath_TB.Focus();
                    throw new Exception("Указанный исполняемый файл программы распознавания не существует");
                }

                if (!Directory.Exists(DirectoryProgPath_TB.Text.Trim()))
                {
                    DirectoryProgPath_TB.Focus();
                    throw new Exception("Указанная дирекория распознавания не существует");
                }

                if (String.IsNullOrEmpty(IP_TB.Text))
                {
                    IP_TB.Focus();
                    throw new Exception("Необходимо ввести адрес программы распознавания");
                }

                if (String.IsNullOrEmpty(Port_TB.Text))
                {
                    Port_TB.Focus();
                    throw new Exception("Необходимо ввести порт программы распознавания");
                }

                Int32 Port = Convert.ToInt32(Port_TB.Text);

                SystemArgs.ByteScout.Server = IP_TB.Text.Trim();
                SystemArgs.ByteScout.Port = Port_TB.Text.Trim();
                SystemArgs.ByteScout.ProgramPath = PrpgramPath_TB.Text.Trim();
                SystemArgs.ByteScout.DirectoryProgramPath = DirectoryProgPath_TB.Text.Trim();
                if (SystemArgs.ByteScout.CheckConnect())
                {
                    if (SystemArgs.ByteScout.SetParametersConnect())
                    {
                        MessageBox.Show("Параметры успешно записаны", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        throw new Exception("Ошибка при записи директорий");
                    }
                }
                else
                {
                    throw new Exception("Ошибка при подключении к программе распознования");
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

        private void ADSettingsByteScout_F_Load(object sender, EventArgs e)
        {

        }
    }
}
