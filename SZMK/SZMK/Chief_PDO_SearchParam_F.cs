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
    public partial class Chief_PDO_SearchParam_F : Form
    {
        public Chief_PDO_SearchParam_F()
        {
            InitializeComponent();
        }

        private void Chief_PDO_SearchParam_F_Load(object sender, EventArgs e)
        {
            First_DP.Enabled = false;
            Second_DP.Enabled = false;
        }

        private void DateEnable_CB_CheckedChanged(object sender, EventArgs e)
        {
            if (DateEnable_CB.Checked)
            {
                DateEnable_CB.Text = "Выключить";
                First_DP.Enabled = true;
                Second_DP.Enabled = true;
            }
            else
            {
                DateEnable_CB.Text = "Включить";
                First_DP.Enabled = false;
                Second_DP.Enabled = false;
            }
        }

        private void Status_CB_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<User> Users = new List<User>();
            Users.Add(new User(0, "Нет имени", "Нет отчества", "Нет фамилии", DateTime.Now, SystemArgs.Positions[0].ID, SystemArgs.Mails, "Не задано", "Нет хеша", true));
            User_CB.DataSource = null;
            User_CB.DataSource = Users;
            if (Status_CB.SelectedIndex != 0)
            {
                foreach (Status status in SystemArgs.Statuses)
                    Users.AddRange(SystemArgs.Users.Where(p => (Status)Status_CB.SelectedItem == status && status.IDPosition == p.GetPosition().ID));
                User_CB.DataSource = null;
                User_CB.DataSource = Users;
            }
            else
            {
                Users.AddRange(SystemArgs.Users);
                User_CB.DataSource = null;
                User_CB.DataSource = Users;
            }
        }

        private void OK_B_Click(object sender, EventArgs e)
        {

        }
    }
}
