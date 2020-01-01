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
    public partial class ChangeOrderKB_F : Form
    {
        public ChangeOrderKB_F(Order TempOrder)
        {
            this.TempOrder = TempOrder;
            InitializeComponent();
        }
        private Order TempOrder;
        private void ChangeOrderKB_F_Load(object sender, EventArgs e)
        {

        }
    }
}
