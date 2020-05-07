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
    public partial class AD_SettingsProgram_F : Form
    {
        public AD_SettingsProgram_F()
        {
            InitializeComponent();
        }

        private void SaveArchive_B_Click(object sender, EventArgs e)
        {
            try
            {
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

        private void OK_B_Click(object sender, EventArgs e)
        {

        }

        private void AD_SettingsProgram_F_Load(object sender, EventArgs e)
        {

        }
    }
}
