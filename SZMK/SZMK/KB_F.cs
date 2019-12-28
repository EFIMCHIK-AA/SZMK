using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace SZMK
{
    public partial class KB_F : Form
    {
        public KB_F()
        {
            InitializeComponent();
        }
        private void KB_F_Load(object sender, EventArgs e)
        {
            try
            {

                Load_F Dialog = new Load_F();

                Dialog.Show();

                SystemArgs.MobileApplication = new MobileApplication(); //Конфигурация мобильного приложения
                SystemArgs.ServerMobileApp = new ServerMobileApp();//Сервер мобильного приложения

                Thread.Sleep(2000);

                Dialog.Close();
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddOrder_TSB_Click(object sender, EventArgs e)
        {
            AddOrder();
        }

        private void ChangeOrder_TSB_Click(object sender, EventArgs e)
        {
            ChangeOrder();
        }
        private void DeleteOrder_TSB_Click(object sender, EventArgs e)
        {
            DeleteOrder();
        }

        private void AddOrder_TSM_Click(object sender, EventArgs e)
        {
            AddOrder();
        }
        private void ChangeOrder_TSM_Click(object sender, EventArgs e)
        {
            ChangeOrder();
        }

        private void DeleteOrder_TSM_Click(object sender, EventArgs e)
        {
            DeleteOrder();
        }

        private void ReportDate_TSM_Click(object sender, EventArgs e)
        {

        }
        private void Exit_TSM_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Search_TSB_Click(object sender, EventArgs e)
        {
                                        
        }

        private void Reset_TSB_Click(object sender, EventArgs e)
        {

        }

        private void AdvancedSearch_TSB_Click(object sender, EventArgs e)
        {

        }
        private void AddOrder()
        {
            try
            {
                KBScan_F Dialog = new KBScan_F();
                if (SystemArgs.ServerMobileApp.Start())
                {
                    Dialog.ServerStatus_TB.Text = "Запущен";
                    Dialog.ServerStatus_TB.BackColor = Color.MediumTurquoise;
                    Dialog.Status_TB.AppendText($"Ожидание QR" + Environment.NewLine);
                    if (Dialog.ShowDialog() == DialogResult.OK)
                    {
                        //Вызывается метод с запросом в БД
                    }
                }
                else
                {
                    Dialog.ServerStatus_TB.Text = "Остановлен";
                    Dialog.ServerStatus_TB.BackColor = Color.Tomato;
                }
            }
            catch
            {
                MessageBox.Show("Порт открытия сервера занят", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void ChangeOrder()
        {

        }
        private void DeleteOrder()
        {

        }

        private void Order_DGV_SelectionChanged(object sender, EventArgs e)
        {

        }
    }
}
