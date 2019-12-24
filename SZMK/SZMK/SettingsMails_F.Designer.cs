namespace SZMK
{
    partial class SettingsMails_F
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.Mails_DGV = new System.Windows.Forms.DataGridView();
            this.label4 = new System.Windows.Forms.Label();
            this.Add_B = new System.Windows.Forms.Button();
            this.Change_B = new System.Windows.Forms.Button();
            this.Delete_B = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.OK_B = new System.Windows.Forms.Button();
            this.Search_TB = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Search_B = new System.Windows.Forms.Button();
            this.ResetSearch_B = new System.Windows.Forms.Button();
            this.MoreInfo_B = new System.Windows.Forms.Button();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.Mails_DGV)).BeginInit();
            this.SuspendLayout();
            // 
            // Mails_DGV
            // 
            this.Mails_DGV.AllowUserToAddRows = false;
            this.Mails_DGV.AllowUserToDeleteRows = false;
            this.Mails_DGV.AllowUserToResizeColumns = false;
            this.Mails_DGV.AllowUserToResizeRows = false;
            this.Mails_DGV.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Mails_DGV.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.Mails_DGV.BackgroundColor = System.Drawing.Color.White;
            this.Mails_DGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Mails_DGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column2,
            this.Column1});
            this.Mails_DGV.Location = new System.Drawing.Point(16, 42);
            this.Mails_DGV.Margin = new System.Windows.Forms.Padding(5);
            this.Mails_DGV.MultiSelect = false;
            this.Mails_DGV.Name = "Mails_DGV";
            this.Mails_DGV.ReadOnly = true;
            this.Mails_DGV.RowHeadersVisible = false;
            this.Mails_DGV.RowHeadersWidth = 51;
            this.Mails_DGV.Size = new System.Drawing.Size(325, 341);
            this.Mails_DGV.TabIndex = 19;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(93, 19);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(178, 16);
            this.label4.TabIndex = 83;
            this.label4.Text = "Список почтовых адресов";
            // 
            // Add_B
            // 
            this.Add_B.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Add_B.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Add_B.Location = new System.Drawing.Point(348, 42);
            this.Add_B.Margin = new System.Windows.Forms.Padding(2);
            this.Add_B.Name = "Add_B";
            this.Add_B.Size = new System.Drawing.Size(212, 25);
            this.Add_B.TabIndex = 84;
            this.Add_B.Text = "Добавить";
            this.Add_B.UseVisualStyleBackColor = true;
            this.Add_B.Click += new System.EventHandler(this.Add_B_Click);
            // 
            // Change_B
            // 
            this.Change_B.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Change_B.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Change_B.Location = new System.Drawing.Point(348, 71);
            this.Change_B.Margin = new System.Windows.Forms.Padding(2);
            this.Change_B.Name = "Change_B";
            this.Change_B.Size = new System.Drawing.Size(212, 25);
            this.Change_B.TabIndex = 85;
            this.Change_B.Text = "Изменить";
            this.Change_B.UseVisualStyleBackColor = true;
            this.Change_B.Click += new System.EventHandler(this.Change_B_Click);
            // 
            // Delete_B
            // 
            this.Delete_B.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Delete_B.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Delete_B.Location = new System.Drawing.Point(348, 100);
            this.Delete_B.Margin = new System.Windows.Forms.Padding(2);
            this.Delete_B.Name = "Delete_B";
            this.Delete_B.Size = new System.Drawing.Size(212, 25);
            this.Delete_B.TabIndex = 86;
            this.Delete_B.Text = "Удалить";
            this.Delete_B.UseVisualStyleBackColor = true;
            this.Delete_B.Click += new System.EventHandler(this.Delete_B_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(411, 24);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 16);
            this.label1.TabIndex = 87;
            this.label1.Text = "Операции";
            // 
            // OK_B
            // 
            this.OK_B.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OK_B.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.OK_B.Location = new System.Drawing.Point(348, 358);
            this.OK_B.Margin = new System.Windows.Forms.Padding(2);
            this.OK_B.Name = "OK_B";
            this.OK_B.Size = new System.Drawing.Size(212, 25);
            this.OK_B.TabIndex = 88;
            this.OK_B.Text = "Завершить редактирование";
            this.OK_B.UseVisualStyleBackColor = true;
            // 
            // Search_TB
            // 
            this.Search_TB.Location = new System.Drawing.Point(348, 206);
            this.Search_TB.Margin = new System.Windows.Forms.Padding(2);
            this.Search_TB.Name = "Search_TB";
            this.Search_TB.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Search_TB.Size = new System.Drawing.Size(212, 20);
            this.Search_TB.TabIndex = 89;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(428, 185);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 16);
            this.label2.TabIndex = 90;
            this.label2.Text = "Поиск";
            // 
            // Search_B
            // 
            this.Search_B.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Search_B.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Search_B.Location = new System.Drawing.Point(348, 230);
            this.Search_B.Margin = new System.Windows.Forms.Padding(2);
            this.Search_B.Name = "Search_B";
            this.Search_B.Size = new System.Drawing.Size(212, 25);
            this.Search_B.TabIndex = 91;
            this.Search_B.Text = "Поиск";
            this.Search_B.UseVisualStyleBackColor = true;
            this.Search_B.Click += new System.EventHandler(this.Search_B_Click);
            // 
            // ResetSearch_B
            // 
            this.ResetSearch_B.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ResetSearch_B.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ResetSearch_B.Location = new System.Drawing.Point(348, 259);
            this.ResetSearch_B.Margin = new System.Windows.Forms.Padding(2);
            this.ResetSearch_B.Name = "ResetSearch_B";
            this.ResetSearch_B.Size = new System.Drawing.Size(212, 25);
            this.ResetSearch_B.TabIndex = 92;
            this.ResetSearch_B.Text = "Сбросить";
            this.ResetSearch_B.UseVisualStyleBackColor = true;
            this.ResetSearch_B.Click += new System.EventHandler(this.ResetSearch_B_Click);
            // 
            // MoreInfo_B
            // 
            this.MoreInfo_B.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.MoreInfo_B.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MoreInfo_B.Location = new System.Drawing.Point(348, 129);
            this.MoreInfo_B.Margin = new System.Windows.Forms.Padding(2);
            this.MoreInfo_B.Name = "MoreInfo_B";
            this.MoreInfo_B.Size = new System.Drawing.Size(212, 25);
            this.MoreInfo_B.TabIndex = 94;
            this.MoreInfo_B.Text = "Подробнее";
            this.MoreInfo_B.UseVisualStyleBackColor = true;
            this.MoreInfo_B.Click += new System.EventHandler(this.MoreInfo_B_Click);
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "DateCreateView";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column2.DefaultCellStyle = dataGridViewCellStyle7;
            this.Column2.HeaderText = "Дата создания";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column1.DataPropertyName = "MailAddress";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column1.DefaultCellStyle = dataGridViewCellStyle8;
            this.Column1.HeaderText = "Адрес электронной почты";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // SettingsMails_F
            // 
            this.AcceptButton = this.OK_B;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(576, 399);
            this.Controls.Add(this.MoreInfo_B);
            this.Controls.Add(this.ResetSearch_B);
            this.Controls.Add(this.Search_B);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Search_TB);
            this.Controls.Add(this.OK_B);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Delete_B);
            this.Controls.Add(this.Change_B);
            this.Controls.Add(this.Add_B);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.Mails_DGV);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "SettingsMails_F";
            this.Text = "Настройка почтовых адресов";
            ((System.ComponentModel.ISupportInitialize)(this.Mails_DGV)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.DataGridView Mails_DGV;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button Add_B;
        private System.Windows.Forms.Button Change_B;
        private System.Windows.Forms.Button Delete_B;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button OK_B;
        public System.Windows.Forms.TextBox Search_TB;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button Search_B;
        private System.Windows.Forms.Button ResetSearch_B;
        private System.Windows.Forms.Button MoreInfo_B;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
    }
}