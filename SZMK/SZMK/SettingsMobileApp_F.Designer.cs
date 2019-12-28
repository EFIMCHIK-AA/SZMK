namespace SZMK
{
    partial class SettingsMobileApp_F
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.QR_PB = new System.Windows.Forms.PictureBox();
            this.Generate_B = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.Port_TB = new System.Windows.Forms.TextBox();
            this.IP_TB = new System.Windows.Forms.TextBox();
            this.OK_B = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.QR_PB)).BeginInit();
            this.SuspendLayout();
            // 
            // QR_PB
            // 
            this.QR_PB.Location = new System.Drawing.Point(346, 50);
            this.QR_PB.Margin = new System.Windows.Forms.Padding(2);
            this.QR_PB.Name = "QR_PB";
            this.QR_PB.Size = new System.Drawing.Size(181, 184);
            this.QR_PB.TabIndex = 0;
            this.QR_PB.TabStop = false;
            // 
            // Generate_B
            // 
            this.Generate_B.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Generate_B.Location = new System.Drawing.Point(18, 98);
            this.Generate_B.Margin = new System.Windows.Forms.Padding(2);
            this.Generate_B.Name = "Generate_B";
            this.Generate_B.Size = new System.Drawing.Size(324, 25);
            this.Generate_B.TabIndex = 65;
            this.Generate_B.Text = "Сохранить и обновить QR";
            this.Generate_B.UseVisualStyleBackColor = true;
            this.Generate_B.Click += new System.EventHandler(this.Generate_B_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.Location = new System.Drawing.Point(15, 75);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 16);
            this.label7.TabIndex = 67;
            this.label7.Text = "Порт";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(15, 51);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(69, 16);
            this.label6.TabIndex = 66;
            this.label6.Text = "IP - адрес";
            // 
            // Port_TB
            // 
            this.Port_TB.Location = new System.Drawing.Point(88, 74);
            this.Port_TB.Margin = new System.Windows.Forms.Padding(2);
            this.Port_TB.Name = "Port_TB";
            this.Port_TB.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Port_TB.Size = new System.Drawing.Size(254, 20);
            this.Port_TB.TabIndex = 69;
            // 
            // IP_TB
            // 
            this.IP_TB.Location = new System.Drawing.Point(88, 50);
            this.IP_TB.Margin = new System.Windows.Forms.Padding(2);
            this.IP_TB.Name = "IP_TB";
            this.IP_TB.ReadOnly = true;
            this.IP_TB.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.IP_TB.Size = new System.Drawing.Size(254, 20);
            this.IP_TB.TabIndex = 68;
            // 
            // OK_B
            // 
            this.OK_B.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OK_B.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.OK_B.Location = new System.Drawing.Point(18, 209);
            this.OK_B.Margin = new System.Windows.Forms.Padding(2);
            this.OK_B.Name = "OK_B";
            this.OK_B.Size = new System.Drawing.Size(324, 25);
            this.OK_B.TabIndex = 72;
            this.OK_B.Text = "Завершить редактирование";
            this.OK_B.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(106, 16);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(334, 16);
            this.label3.TabIndex = 73;
            this.label3.Text = "Настрйоки подключения мобильного приложения";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.label4.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(27, 145);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(309, 38);
            this.label4.TabIndex = 74;
            this.label4.Text = "Используйте QR сканер для определения\r\nпараметров подлючения на устройстве";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SettingsMobileApp_F
            // 
            this.AcceptButton = this.OK_B;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(544, 250);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.OK_B);
            this.Controls.Add(this.QR_PB);
            this.Controls.Add(this.Port_TB);
            this.Controls.Add(this.IP_TB);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.Generate_B);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "SettingsMobileApp_F";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Настройки мобильного приложения";
            ((System.ComponentModel.ISupportInitialize)(this.QR_PB)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button Generate_B;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        public System.Windows.Forms.TextBox Port_TB;
        public System.Windows.Forms.TextBox IP_TB;
        private System.Windows.Forms.Button OK_B;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.PictureBox QR_PB;
    }
}