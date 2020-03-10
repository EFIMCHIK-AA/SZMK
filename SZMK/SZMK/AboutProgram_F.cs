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
            for(int i = 0; i < SystemArgs.AboutProgram.GetUpdates().Count(); i++)
            {
                DiscriptionsUpdate_RTB.AppendText($"Версия: "+SystemArgs.AboutProgram[i].Version+Environment.NewLine);
                DiscriptionsUpdate_RTB.AppendText($"Дата выпуска: " + SystemArgs.AboutProgram[i].Date.ToShortDateString() + Environment.NewLine);
                if (SystemArgs.AboutProgram[i].GetAdded().Count()!=0)
                {
                    DiscriptionsUpdate_RTB.AppendText($"Добавлено: " + Environment.NewLine);
                }
                for (int j = 0; j < SystemArgs.AboutProgram[i].GetAdded().Count(); j++)
                {
                    DiscriptionsUpdate_RTB.AppendText($"- " + SystemArgs.AboutProgram[i].GetAdded()[j] + Environment.NewLine);
                }
                if (SystemArgs.AboutProgram[i].GetDeleted().Count() != 0)
                {
                    DiscriptionsUpdate_RTB.AppendText($"Убрано: " + Environment.NewLine);
                }
                for (int j = 0; j < SystemArgs.AboutProgram[i].GetDeleted().Count(); j++)
                {
                    DiscriptionsUpdate_RTB.AppendText($"- " + SystemArgs.AboutProgram[i].GetDeleted()[j] + Environment.NewLine);
                }
            }
            Developers_RTB.SelectAll();
            Developers_RTB.SelectionAlignment = HorizontalAlignment.Center;
        }
    }
}
