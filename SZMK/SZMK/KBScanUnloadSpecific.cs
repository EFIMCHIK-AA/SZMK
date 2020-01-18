using System;
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
    public partial class KBScanUnloadSpecific : Form
    {
        public KBScanUnloadSpecific()
        {
            InitializeComponent();
        }

        private void KBScanUnloadSpecific_Load(object sender, EventArgs e)
        {
            Report_DGV.AutoGenerateColumns = false;
            Report_DGV.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Report_DGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            Report_DGV.DataSource = SystemArgs.UnLoadSpecific.Specifics;
            for (int i = 0; i < Report_DGV.Rows.Count; i++)
            {
                if (Report_DGV[4, i].Value.ToString()=="Найдено")
                {
                    Report_DGV[4, i].Style.BackColor = Color.Lime;
                }
                else
                {
                    Report_DGV[4, i].Style.BackColor = Color.Red;
                }
            }
        }

        private void Report_DGV_SelectionChanged(object sender, EventArgs e)
        {

        }

        private void Report_DGV_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            e.CellStyle.SelectionBackColor = Color.FromArgb(112, 238, 226);
            e.CellStyle.SelectionForeColor = Color.Black;
        }
    }
}
