namespace SZMK
{
    partial class Autorization_F
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.Login_CB = new System.Windows.Forms.ComboBox();
            this.Cancel_B = new System.Windows.Forms.Button();
            this.Enter_B = new System.Windows.Forms.Button();
            this.CheckPass_CB = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.Password_TB = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Login_CB
            // 
            this.Login_CB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Login_CB.FormattingEnabled = true;
            this.Login_CB.Items.AddRange(new object[] {
            "asdasd",
            "asdsad",
            "asdad"});
            this.Login_CB.Location = new System.Drawing.Point(18, 316);
            this.Login_CB.Margin = new System.Windows.Forms.Padding(2);
            this.Login_CB.Name = "Login_CB";
            this.Login_CB.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Login_CB.Size = new System.Drawing.Size(306, 21);
            this.Login_CB.TabIndex = 1;
            // 
            // Cancel_B
            // 
            this.Cancel_B.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel_B.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Cancel_B.Location = new System.Drawing.Point(18, 449);
            this.Cancel_B.Margin = new System.Windows.Forms.Padding(2);
            this.Cancel_B.Name = "Cancel_B";
            this.Cancel_B.Size = new System.Drawing.Size(306, 25);
            this.Cancel_B.TabIndex = 5;
            this.Cancel_B.Text = "В&ыйти";
            this.Cancel_B.UseVisualStyleBackColor = true;
            this.Cancel_B.Click += new System.EventHandler(this.Cancel_B_Click);
            // 
            // Enter_B
            // 
            this.Enter_B.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Enter_B.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Enter_B.Location = new System.Drawing.Point(18, 420);
            this.Enter_B.Margin = new System.Windows.Forms.Padding(2);
            this.Enter_B.Name = "Enter_B";
            this.Enter_B.Size = new System.Drawing.Size(306, 25);
            this.Enter_B.TabIndex = 4;
            this.Enter_B.Text = "&Войти";
            this.Enter_B.UseVisualStyleBackColor = true;
            this.Enter_B.Click += new System.EventHandler(this.Enter_B_Click);
            // 
            // CheckPass_CB
            // 
            this.CheckPass_CB.AutoSize = true;
            this.CheckPass_CB.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.CheckPass_CB.Location = new System.Drawing.Point(18, 391);
            this.CheckPass_CB.Margin = new System.Windows.Forms.Padding(2);
            this.CheckPass_CB.Name = "CheckPass_CB";
            this.CheckPass_CB.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.CheckPass_CB.Size = new System.Drawing.Size(140, 20);
            this.CheckPass_CB.TabIndex = 3;
            this.CheckPass_CB.Text = "&Показать пароль";
            this.CheckPass_CB.UseVisualStyleBackColor = true;
            this.CheckPass_CB.CheckedChanged += new System.EventHandler(this.CheckPass_CB_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(15, 294);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(130, 16);
            this.label4.TabIndex = 3;
            this.label4.Text = "Имя пользователя";
            // 
            // Password_TB
            // 
            this.Password_TB.Location = new System.Drawing.Point(18, 365);
            this.Password_TB.Margin = new System.Windows.Forms.Padding(2);
            this.Password_TB.Name = "Password_TB";
            this.Password_TB.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Password_TB.Size = new System.Drawing.Size(306, 20);
            this.Password_TB.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(15, 344);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 16);
            this.label5.TabIndex = 1;
            this.label5.Text = "Пароль";
            // 
            // Autorization_F
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancel_B;
            this.ClientSize = new System.Drawing.Size(343, 490);
            this.Controls.Add(this.Login_CB);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.Password_TB);
            this.Controls.Add(this.Cancel_B);
            this.Controls.Add(this.CheckPass_CB);
            this.Controls.Add(this.Enter_B);
            this.MaximizeBox = false;
            this.Name = "Autorization_F";
            this.Text = "Авторизация ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Autorization_F_FormClosing);
            this.Load += new System.EventHandler(this.Autorization_F_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ComboBox Login_CB;
        private System.Windows.Forms.Button Cancel_B;
        private System.Windows.Forms.Button Enter_B;
        private System.Windows.Forms.CheckBox CheckPass_CB;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.TextBox Password_TB;
        private System.Windows.Forms.Label label5;
    }
}

