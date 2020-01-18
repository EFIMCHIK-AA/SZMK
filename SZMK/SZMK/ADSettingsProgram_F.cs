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
    public partial class ADSettingsProgram_F : Form
    {
        public ADSettingsProgram_F()
        {
            InitializeComponent();
        }

        private void ReviewRegistry_B_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog Ofd = new FolderBrowserDialog();

            if (Ofd.ShowDialog() == DialogResult.OK)
            {
                RegistryPath_TB.Text = Ofd.SelectedPath;
            }
        }

        private void ReviewArchive_B_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog Ofd = new FolderBrowserDialog();

            if (Ofd.ShowDialog() == DialogResult.OK)
            {
                ArchivePath_TB.Text = Ofd.SelectedPath;
            }
        }

        private void SaveArchive_B_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(RegistryPath_TB.Text))
                {
                    RegistryPath_TB.Focus();
                    throw new Exception("Необходимо указать директорию реестра");
                }

                if (String.IsNullOrEmpty(ArchivePath_TB.Text))
                {
                    ArchivePath_TB.Focus();
                    throw new Exception("Необходимо указать директорию архива");
                }

                if (String.IsNullOrEmpty(ModelsPath_TB.Text))
                {
                    ModelsPath_TB.Focus();
                    throw new Exception("Необходимо указать директорию выгрузки");
                }

                if (!Directory.Exists(RegistryPath_TB.Text.Trim()))
                {
                    RegistryPath_TB.Focus();
                    throw new Exception("Указанная дирекория реестра не существует");
                }

                if (!Directory.Exists(RegistryPath_TB.Text.Trim()))
                {
                    ArchivePath_TB.Focus();
                    throw new Exception("Указанная дирекория архива не существует");
                }

                if (!Directory.Exists(ModelsPath_TB.Text.Trim()))
                {
                    ModelsPath_TB.Focus();
                    throw new Exception("Указанная дирекория выгрузки не существует");
                }

                SystemArgs.ClientProgram.ArchivePath = ArchivePath_TB.Text.Trim();
                SystemArgs.ClientProgram.RegistryPath = RegistryPath_TB.Text.Trim();
                SystemArgs.ClientProgram.ModelsPath = ModelsPath_TB.Text.Trim();

                if(CheckMarks_CB.Checked)
                {
                    SystemArgs.ClientProgram.CheckMarks = true;
                }
                else
                {
                    SystemArgs.ClientProgram.CheckMarks = false;
                }

                SystemArgs.ClientProgram.VisualRow_N1 = Convert.ToInt32(N1_NUD.Value);
                SystemArgs.ClientProgram.VisualRow_N2 = Convert.ToInt32(N2_NUD.Value);

                if (SystemArgs.ClientProgram.SetParametersConnect())
                {
                    MessageBox.Show("Параметры успешно записаны", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    throw new Exception("Ошибка при записи параметров");
                }
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message + ". Запись не выполнена", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ReviewModels_B_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog Ofd = new FolderBrowserDialog();

            if (Ofd.ShowDialog() == DialogResult.OK)
            {
                ModelsPath_TB.Text = Ofd.SelectedPath;
            }
        }
    }
}
