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
    public partial class ARChangeOrder_F : Form
    {
        public ARChangeOrder_F(Order TempOrder)
        {
            this.TempOrder = TempOrder;
            InitializeComponent();
        }
        Order TempOrder;
        private void ARChangeOrder_F_Load(object sender, EventArgs e)
        {

        }
    }
}
