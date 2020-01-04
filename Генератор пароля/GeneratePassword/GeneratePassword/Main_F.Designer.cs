namespace GeneratePassword
{
    partial class Main_F
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main_F));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.Status_TB = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.Password_TB = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.Encryption_TB = new System.Windows.Forms.TextBox();
            this.GeneratePass_B = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.Status_TB, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.button1, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.Password_TB, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.Encryption_TB, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.GeneratePass_B, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(593, 213);
            this.tableLayoutPanel1.TabIndex = 43;
            // 
            // Status_TB
            // 
            this.Status_TB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.tableLayoutPanel1.SetColumnSpan(this.Status_TB, 2);
            this.Status_TB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Status_TB.Location = new System.Drawing.Point(5, 188);
            this.Status_TB.Margin = new System.Windows.Forms.Padding(5, 2, 5, 2);
            this.Status_TB.Name = "Status_TB";
            this.Status_TB.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Status_TB.Size = new System.Drawing.Size(583, 20);
            this.Status_TB.TabIndex = 59;
            this.Status_TB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.tableLayoutPanel1.SetColumnSpan(this.button1, 2);
            this.button1.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(223)))), ((int)(((byte)(253)))));
            this.button1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(217)))), ((int)(((byte)(254)))));
            this.button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(237)))), ((int)(((byte)(253)))));
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.button1.Location = new System.Drawing.Point(5, 148);
            this.button1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(583, 35);
            this.button1.TabIndex = 58;
            this.button1.Text = "Сгенерировать исходную последовательность (8 символов)";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Password_TB
            // 
            this.Password_TB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.Password_TB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Password_TB.Location = new System.Drawing.Point(258, 57);
            this.Password_TB.Margin = new System.Windows.Forms.Padding(2, 2, 5, 2);
            this.Password_TB.Name = "Password_TB";
            this.Password_TB.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Password_TB.Size = new System.Drawing.Size(330, 20);
            this.Password_TB.TabIndex = 24;
            this.Password_TB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(228)))), ((int)(((byte)(213)))));
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.label2, 2);
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(5, 10);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 10, 5, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(583, 35);
            this.label2.TabIndex = 20;
            this.label2.Text = "Генератор паролей";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(5, 79);
            this.label4.Margin = new System.Windows.Forms.Padding(5, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(249, 16);
            this.label4.TabIndex = 25;
            this.label4.Text = "Зашифрованная последовательость";
            // 
            // Encryption_TB
            // 
            this.Encryption_TB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.Encryption_TB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Encryption_TB.Location = new System.Drawing.Point(258, 81);
            this.Encryption_TB.Margin = new System.Windows.Forms.Padding(2, 2, 5, 2);
            this.Encryption_TB.Name = "Encryption_TB";
            this.Encryption_TB.ReadOnly = true;
            this.Encryption_TB.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Encryption_TB.Size = new System.Drawing.Size(330, 20);
            this.Encryption_TB.TabIndex = 26;
            this.Encryption_TB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // GeneratePass_B
            // 
            this.GeneratePass_B.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.tableLayoutPanel1.SetColumnSpan(this.GeneratePass_B, 2);
            this.GeneratePass_B.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.GeneratePass_B.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(223)))), ((int)(((byte)(253)))));
            this.GeneratePass_B.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(217)))), ((int)(((byte)(254)))));
            this.GeneratePass_B.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(237)))), ((int)(((byte)(253)))));
            this.GeneratePass_B.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.GeneratePass_B.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.GeneratePass_B.Location = new System.Drawing.Point(5, 106);
            this.GeneratePass_B.Margin = new System.Windows.Forms.Padding(5, 3, 5, 7);
            this.GeneratePass_B.Name = "GeneratePass_B";
            this.GeneratePass_B.Size = new System.Drawing.Size(583, 35);
            this.GeneratePass_B.TabIndex = 56;
            this.GeneratePass_B.Text = "Зашифровать";
            this.GeneratePass_B.UseVisualStyleBackColor = false;
            this.GeneratePass_B.Click += new System.EventHandler(this.RefreshStatus_B_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(5, 55);
            this.label3.Margin = new System.Windows.Forms.Padding(5, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(211, 16);
            this.label3.TabIndex = 23;
            this.label3.Text = "Исходная последовательность";
            // 
            // Main_F
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(593, 213);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Main_F";
            this.Text = "Генератор СЗМК";
            this.Load += new System.EventHandler(this.Main_F_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.TextBox Password_TB;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.TextBox Encryption_TB;
        public System.Windows.Forms.Button GeneratePass_B;
        public System.Windows.Forms.Button button1;
        public System.Windows.Forms.TextBox Status_TB;
    }
}

