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
    public partial class KB_ChangeOrder_F : Form
    {
        public KB_ChangeOrder_F(Order TempOrder)
        {
            this.TempOrder = TempOrder;
            InitializeComponent();
        }
        private Order TempOrder;

        private void KBChangeOrder_F_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
            {
                try
                {
                    if (String.IsNullOrEmpty(Executor_TB.Text))
                    {
                        Executor_TB.Focus();
                        throw new Exception("Необходимо указать исполнителя чертежа");
                    }

                    if (String.IsNullOrEmpty(Number_TB.Text))
                    {
                        Number_TB.Focus();
                        throw new Exception("Необходимо указать номер заказа");
                    }

                    if (String.IsNullOrEmpty(List_TB.Text))
                    {
                        List_TB.Focus();
                        throw new Exception("Необходимо указать лист");
                    }

                    if (Convert.ToInt32(List_TB.Text)<=0)
                    {
                        List_TB.Focus();
                        throw new Exception("Лист должен быть больше 0");
                    }

                    if (String.IsNullOrEmpty(Mark_TB.Text))
                    {
                        Mark_TB.Focus();
                        throw new Exception("Необходимо указать марку");
                    }

                    if (String.IsNullOrEmpty(Lenght_TB.Text))
                    {
                        Lenght_TB.Focus();
                        throw new Exception("Необходимо указать длину");
                    }

                    if (Convert.ToDouble(Lenght_TB.Text) <= 0)
                    {
                        List_TB.Focus();
                        throw new Exception("Длина должна быть больше 0");
                    }

                    if (String.IsNullOrEmpty(Weight_TB.Text))
                    {
                        Weight_TB.Focus();
                        throw new Exception("Необходимо указать вес");
                    }

                    if (Convert.ToDouble(Lenght_TB.Text) <= 0)
                    {
                        List_TB.Focus();
                        throw new Exception("Вес должен быть больше 0");
                    }

                    List<Order> TempList = SystemArgs.Orders;

                    if (TempOrder != null)
                    {
                        TempList.Remove(TempOrder);
                    }

                    String NewDataMatrix = Number_TB.Text + "_" + List_TB.Text + "_" + Mark_TB.Text + "_" + Executor_TB.Text + "_" + Lenght_TB.Text + "_" + Weight_TB.Text;

                    for (Int32 i = 0; i < TempList.Count(); i++)
                    {
                        if (TempList[i].DataMatrix == NewDataMatrix.Trim())
                        {
                            throw new Exception("Чертеж с указанными данными уже существует");
                        }
                    }
                }
                catch (FormatException)
                {
                    MessageBox.Show("Указан неверный формат числовых полей, лист целое число, вес и длина вещественные числа", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    e.Cancel = true;
                }
                catch (Exception E)
                {
                    MessageBox.Show(E.Message, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    e.Cancel = true;
                }
            }
        }

        private void KB_ChangeOrder_F_Load(object sender, EventArgs e)
        {

        }
    }
}
