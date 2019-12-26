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
    public partial class SettingsProgram_F : Form
    {
        public SettingsProgram_F()
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

                if(!Directory.Exists(RegistryPath_TB.Text.Trim()))
                {
                    RegistryPath_TB.Focus();
                    throw new Exception("Указанная дирекория реестра не существует");
                }

                if (!Directory.Exists(RegistryPath_TB.Text.Trim()))
                {
                    ArchivePath_TB.Focus();
                    throw new Exception("Указанная дирекория архива не существует");
                }

                SystemArgs.ClientProgram.ArchivePath = ArchivePath_TB.Text.Trim();
                SystemArgs.ClientProgram.RegistryPath = RegistryPath_TB.Text.Trim();

                if (!SystemArgs.ClientProgram.SetParametersConnect())
                {
                    throw new Exception("Ошибка при записи директорий");
                }
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message + ". Сохранение не выполнено", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
