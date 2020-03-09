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
    public partial class AboutProgram_F : Form
    {
        public AboutProgram_F()
        {
            InitializeComponent();
        }

        private void AboutProgram_F_Load(object sender, EventArgs e)
        {
            Version_TB.Text = SystemArgs.AboutProgram.Version;
            DateUpdate_TB.Text = SystemArgs.AboutProgram.DateUpdate.ToShortDateString();
            for(int i = 0; i < SystemArgs.AboutProgram.GetDiscriptionsUpdate().Count(); i++)
            {
                DiscriptionsUpdate_RTB.AppendText($"- "+SystemArgs.AboutProgram[i]+Environment.NewLine);
            }
            Developers_RTB.SelectAll();
            Developers_RTB.SelectionAlignment = HorizontalAlignment.Center;
        }
    }
}
