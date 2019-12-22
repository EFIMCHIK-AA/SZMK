namespace SZMK
{
    partial class Archive_F
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.добавлениеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сканированиеЧертежаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.распознованиеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.ChangeUser_B = new System.Windows.Forms.Button();
            this.Exit_B = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.Decode_B = new System.Windows.Forms.Button();
            this.Scan_B = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Order_DGV = new System.Windows.Forms.DataGridView();
            this.DateCreate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Executor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.List = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Mark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Lenght = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Weight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuStrip1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Order_DGV)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.добавлениеToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(919, 24);
            this.menuStrip1.TabIndex = 11;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // добавлениеToolStripMenuItem
            // 
            this.добавлениеToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.сканированиеЧертежаToolStripMenuItem,
            this.распознованиеToolStripMenuItem});
            this.добавлениеToolStripMenuItem.Name = "добавлениеToolStripMenuItem";
            this.добавлениеToolStripMenuItem.Size = new System.Drawing.Size(86, 20);
            this.добавлениеToolStripMenuItem.Text = "Добавление";
            // 
            // сканированиеЧертежаToolStripMenuItem
            // 
            this.сканированиеЧертежаToolStripMenuItem.Name = "сканированиеЧертежаToolStripMenuItem";
            this.сканированиеЧертежаToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.сканированиеЧертежаToolStripMenuItem.Text = "Сканирование чертежа";
            // 
            // распознованиеToolStripMenuItem
            // 
            this.распознованиеToolStripMenuItem.Name = "распознованиеToolStripMenuItem";
            this.распознованиеToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.распознованиеToolStripMenuItem.Text = "Распознование";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.ChangeUser_B);
            this.groupBox3.Controls.Add(this.Exit_B);
            this.groupBox3.Location = new System.Drawing.Point(744, 551);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(164, 72);
            this.groupBox3.TabIndex = 10;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Система";
            // 
            // ChangeUser_B
            // 
            this.ChangeUser_B.Location = new System.Drawing.Point(6, 19);
            this.ChangeUser_B.Name = "ChangeUser_B";
            this.ChangeUser_B.Size = new System.Drawing.Size(146, 23);
            this.ChangeUser_B.TabIndex = 1;
            this.ChangeUser_B.Text = "Смена пользователя";
            this.ChangeUser_B.UseVisualStyleBackColor = true;
            // 
            // Exit_B
            // 
            this.Exit_B.Location = new System.Drawing.Point(6, 43);
            this.Exit_B.Name = "Exit_B";
            this.Exit_B.Size = new System.Drawing.Size(146, 23);
            this.Exit_B.TabIndex = 0;
            this.Exit_B.Text = "Выход";
            this.Exit_B.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.Decode_B);
            this.groupBox2.Controls.Add(this.Scan_B);
            this.groupBox2.Location = new System.Drawing.Point(744, 27);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(164, 72);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Добавление";
            // 
            // Decode_B
            // 
            this.Decode_B.Location = new System.Drawing.Point(6, 43);
            this.Decode_B.Name = "Decode_B";
            this.Decode_B.Size = new System.Drawing.Size(152, 23);
            this.Decode_B.TabIndex = 8;
            this.Decode_B.Text = "Распознование";
            this.Decode_B.UseVisualStyleBackColor = true;
            // 
            // Scan_B
            // 
            this.Scan_B.Location = new System.Drawing.Point(6, 17);
            this.Scan_B.Name = "Scan_B";
            this.Scan_B.Size = new System.Drawing.Size(152, 23);
            this.Scan_B.TabIndex = 0;
            this.Scan_B.Text = "Сканирование чертежа";
            this.Scan_B.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.Order_DGV);
            this.groupBox1.Location = new System.Drawing.Point(12, 27);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(720, 596);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(311, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Список позиций";
            // 
            // Order_DGV
            // 
            this.Order_DGV.AllowUserToAddRows = false;
            this.Order_DGV.AllowUserToDeleteRows = false;
            this.Order_DGV.AllowUserToResizeColumns = false;
            this.Order_DGV.AllowUserToResizeRows = false;
            this.Order_DGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Order_DGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DateCreate,
            this.Executor,
            this.Number,
            this.List,
            this.Mark,
            this.Lenght,
            this.Weight});
            this.Order_DGV.Location = new System.Drawing.Point(6, 33);
            this.Order_DGV.Name = "Order_DGV";
            this.Order_DGV.RowHeadersVisible = false;
            this.Order_DGV.Size = new System.Drawing.Size(708, 557);
            this.Order_DGV.TabIndex = 0;
            // 
            // DateCreate
            // 
            this.DateCreate.HeaderText = "Дата добавления";
            this.DateCreate.Name = "DateCreate";
            // 
            // Executor
            // 
            this.Executor.HeaderText = "Исполнитель";
            this.Executor.Name = "Executor";
            // 
            // Number
            // 
            this.Number.HeaderText = "Номер заказа";
            this.Number.Name = "Number";
            // 
            // List
            // 
            this.List.HeaderText = "Лист";
            this.List.Name = "List";
            // 
            // Mark
            // 
            this.Mark.HeaderText = "Марка";
            this.Mark.Name = "Mark";
            // 
            // Lenght
            // 
            this.Lenght.HeaderText = "Длина";
            this.Lenght.Name = "Lenght";
            // 
            // Weight
            // 
            this.Weight.HeaderText = "Вес";
            this.Weight.Name = "Weight";
            // 
            // Archive
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(919, 634);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Archive";
            this.Text = "Архивариус";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Order_DGV)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem добавлениеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сканированиеЧертежаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem распознованиеToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button ChangeUser_B;
        private System.Windows.Forms.Button Exit_B;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button Decode_B;
        private System.Windows.Forms.Button Scan_B;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView Order_DGV;
        private System.Windows.Forms.DataGridViewTextBoxColumn DateCreate;
        private System.Windows.Forms.DataGridViewTextBoxColumn Executor;
        private System.Windows.Forms.DataGridViewTextBoxColumn Number;
        private System.Windows.Forms.DataGridViewTextBoxColumn List;
        private System.Windows.Forms.DataGridViewTextBoxColumn Mark;
        private System.Windows.Forms.DataGridViewTextBoxColumn Lenght;
        private System.Windows.Forms.DataGridViewTextBoxColumn Weight;
    }
}