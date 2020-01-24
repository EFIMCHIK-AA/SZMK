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
    public partial class ARNotDecode_F : Form
    {
        public ARNotDecode_F()
        {
            InitializeComponent();
        }

        private void ARNotDecode_F_Load(object sender, EventArgs e)
        {
            Report_DGV.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void Report_DGV_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            e.CellStyle.SelectionBackColor = Color.FromArgb(112, 238, 226);
            e.CellStyle.SelectionForeColor = Color.Black;
        }
    }
}
