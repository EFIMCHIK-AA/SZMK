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
    public partial class ALLSearchOrderScan_F : Form
    {
        public ALLSearchOrderScan_F()
        {
            InitializeComponent();
        }

        private void ALLSearchOrderScan_F_FormClosing(object sender, FormClosingEventArgs e)
        {
            SystemArgs.ServerMobileAppFindedOrder.Stop();
        }
    }
}
