﻿using System;
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
            for(int i = 0; i < SystemArgs.UnLoadSpecific.ExecutorMails.Count(); i++)
            {
                for (int j = 0; j < SystemArgs.UnLoadSpecific.ExecutorMails[i]._Specifics.Count(); j++)
                {
                    Report_DGV.Rows.Add();
                    Report_DGV[0, Report_DGV.Rows.Count-1].Value = SystemArgs.UnLoadSpecific.ExecutorMails[i]._Specifics[j].Number;
                    Report_DGV[1, Report_DGV.Rows.Count - 1].Value = SystemArgs.UnLoadSpecific.ExecutorMails[i]._Specifics[j].List;
                    Report_DGV[2, Report_DGV.Rows.Count - 1].Value = SystemArgs.UnLoadSpecific.ExecutorMails[i].Executor;
                    Report_DGV[3, Report_DGV.Rows.Count - 1].Value = SystemArgs.UnLoadSpecific.ExecutorMails[i]._Specifics[j].NumberSpecific;
                    if (SystemArgs.UnLoadSpecific.ExecutorMails[i]._Specifics[j].Finded)
                    {
                        Report_DGV[4, Report_DGV.Rows.Count - 1].Style.BackColor = Color.Lime;
                        Report_DGV[4, Report_DGV.Rows.Count - 1].Value = "Найдено";
                    }
                    else
                    {
                        Report_DGV[4, Report_DGV.Rows.Count - 1].Style.BackColor = Color.Red;
                        Report_DGV[4, Report_DGV.Rows.Count - 1].Value = "Не найдено";
                    }
                }
            }
            bool flag = true;
            for(int i = 0; i < SystemArgs.UnLoadSpecific.ExecutorMails.Count(); i++)
            {
                for(int j = 0; j < SystemArgs.UnLoadSpecific.ExecutorMails[i]._Specifics.Count(); j++)
                {
                    if (!SystemArgs.UnLoadSpecific.ExecutorMails[i]._Specifics[j].Finded)
                    {
                        flag = false;
                    }
                }
            }
            if (SystemArgs.UnLoadSpecific.ExecutorMails.Count() == 0||flag)
            {
                Send_B.Enabled = false;
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

        private void Send_B_Click(object sender, EventArgs e)
        {
            try
            {
                SystemArgs.ServerMail.SendMail();
                MessageBox.Show("Сообщение успешно отправлено", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch(Exception E)
            {
                MessageBox.Show(E.Message, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
