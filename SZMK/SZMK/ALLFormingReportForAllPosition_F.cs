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
    public partial class ALLFormingReportForAllPosition_F : Form
    {
        public ALLFormingReportForAllPosition_F()
        {
            InitializeComponent();
        }

        private void FormingReportForAllPosition_F_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = Properties.Resources._239;
            pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
        }
    }
}