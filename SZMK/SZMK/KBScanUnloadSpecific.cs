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
            Report_DGV.DataSource = SystemArgs.UnLoadSpecific.Specifics;
            for (int i = 0; i < Report_DGV.Rows.Count; i++)
            {
                if (Convert.ToBoolean(Report_DGV[5, i].Value))
                {
                    Report_DGV[1, i].Style.BackColor = Color.Lime;
                }
                else
                {
                    Report_DGV[5, i].Style.BackColor = Color.Red;
                }
            }
        }
    }
}
