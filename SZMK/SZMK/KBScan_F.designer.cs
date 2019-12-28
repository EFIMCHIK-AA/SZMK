namespace SZMK
{
    partial class KBScan_F
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
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.CreateAct_TSM = new System.Windows.Forms.ToolStripMenuItem();
            this.проверкиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CheckedUnloading_TSM = new System.Windows.Forms.ToolStripMenuItem();
            this.Scan_DGV = new System.Windows.Forms.DataGridView();
            this.DataMatrixOrder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Unique = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.ServerStatus_TB = new System.Windows.Forms.TextBox();
            this.Status_TB = new System.Windows.Forms.TextBox();
            this.Add_B = new System.Windows.Forms.Button();
            this.Cancel_B = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Scan_DGV)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.LightSeaGreen;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.проверкиToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(776, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CreateAct_TSM});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(47, 20);
            this.toolStripMenuItem1.Text = "Акты";
            // 
            // CreateAct_TSM
            // 
            this.CreateAct_TSM.Name = "CreateAct_TSM";
            this.CreateAct_TSM.Size = new System.Drawing.Size(180, 22);
            this.CreateAct_TSM.Text = "Сформировать акт";
            this.CreateAct_TSM.Click += new System.EventHandler(this.CreateAct_TSM_Click);
            // 
            // проверкиToolStripMenuItem
            // 
            this.проверкиToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CheckedUnloading_TSM});
            this.проверкиToolStripMenuItem.Name = "проверкиToolStripMenuItem";
            this.проверкиToolStripMenuItem.Size = new System.Drawing.Size(74, 20);
            this.проверкиToolStripMenuItem.Text = "Проверки";
            // 
            // CheckedUnloading_TSM
            // 
            this.CheckedUnloading_TSM.Name = "CheckedUnloading_TSM";
            this.CheckedUnloading_TSM.Size = new System.Drawing.Size(187, 22);
            this.CheckedUnloading_TSM.Text = "Проверить выгрузку";
            this.CheckedUnloading_TSM.Click += new System.EventHandler(this.CheckedUnloading_TSM_Click);
            // 
            // Scan_DGV
            // 
            this.Scan_DGV.AllowUserToAddRows = false;
            this.Scan_DGV.AllowUserToDeleteRows = false;
            this.Scan_DGV.AllowUserToResizeColumns = false;
            this.Scan_DGV.AllowUserToResizeRows = false;
            this.Scan_DGV.BackgroundColor = System.Drawing.Color.White;
            this.Scan_DGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Scan_DGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DataMatrixOrder,
            this.Unique});
            this.Scan_DGV.Location = new System.Drawing.Point(0, 52);
            this.Scan_DGV.Name = "Scan_DGV";
            this.Scan_DGV.ReadOnly = true;
            this.Scan_DGV.RowHeadersVisible = false;
            this.Scan_DGV.Size = new System.Drawing.Size(450, 339);
            this.Scan_DGV.TabIndex = 2;
            // 
            // DataMatrixOrder
            // 
            this.DataMatrixOrder.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.DataMatrixOrder.DataPropertyName = "DataMatrix";
            this.DataMatrixOrder.FillWeight = 70F;
            this.DataMatrixOrder.HeaderText = "DataMatrix";
            this.DataMatrixOrder.Name = "DataMatrixOrder";
            this.DataMatrixOrder.ReadOnly = true;
            // 
            // Unique
            // 
            this.Unique.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Unique.DataPropertyName = "Unique";
            this.Unique.FalseValue = "Не уникален";
            this.Unique.FillWeight = 30F;
            this.Unique.HeaderText = "Уникальность";
            this.Unique.Name = "Unique";
            this.Unique.ReadOnly = true;
            this.Unique.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Unique.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Unique.TrueValue = "Уникален";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(572, 413);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Статус сервера";
            // 
            // ServerStatus_TB
            // 
            this.ServerStatus_TB.BackColor = System.Drawing.Color.White;
            this.ServerStatus_TB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ServerStatus_TB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ServerStatus_TB.Location = new System.Drawing.Point(664, 411);
            this.ServerStatus_TB.Name = "ServerStatus_TB";
            this.ServerStatus_TB.ReadOnly = true;
            this.ServerStatus_TB.Size = new System.Drawing.Size(100, 20);
            this.ServerStatus_TB.TabIndex = 4;
            this.ServerStatus_TB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Status_TB
            // 
            this.Status_TB.BackColor = System.Drawing.Color.White;
            this.Status_TB.Location = new System.Drawing.Point(465, 52);
            this.Status_TB.Multiline = true;
            this.Status_TB.Name = "Status_TB";
            this.Status_TB.ReadOnly = true;
            this.Status_TB.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.Status_TB.Size = new System.Drawing.Size(311, 339);
            this.Status_TB.TabIndex = 5;
            // 
            // Add_B
            // 
            this.Add_B.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Add_B.Location = new System.Drawing.Point(12, 404);
            this.Add_B.Name = "Add_B";
            this.Add_B.Size = new System.Drawing.Size(100, 30);
            this.Add_B.TabIndex = 6;
            this.Add_B.Text = "Добавить";
            this.Add_B.UseVisualStyleBackColor = true;
            this.Add_B.Click += new System.EventHandler(this.Add_B_Click);
            // 
            // Cancel_B
            // 
            this.Cancel_B.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel_B.Location = new System.Drawing.Point(118, 404);
            this.Cancel_B.Name = "Cancel_B";
            this.Cancel_B.Size = new System.Drawing.Size(100, 30);
            this.Cancel_B.TabIndex = 7;
            this.Cancel_B.Text = "Отменить";
            this.Cancel_B.UseVisualStyleBackColor = true;
            this.Cancel_B.Click += new System.EventHandler(this.Cancel_B_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.BackColor = System.Drawing.Color.MediumTurquoise;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(0, 26);
            this.label2.Margin = new System.Windows.Forms.Padding(5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(450, 23);
            this.label2.TabIndex = 8;
            this.label2.Text = "Позиции";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.BackColor = System.Drawing.Color.MediumTurquoise;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(465, 26);
            this.label3.Margin = new System.Windows.Forms.Padding(5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(311, 23);
            this.label3.TabIndex = 9;
            this.label3.Text = "Статус операции";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // KBScan_F
            // 
            this.AcceptButton = this.Add_B;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.Cancel_B;
            this.ClientSize = new System.Drawing.Size(776, 446);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Cancel_B);
            this.Controls.Add(this.Add_B);
            this.Controls.Add(this.Status_TB);
            this.Controls.Add(this.ServerStatus_TB);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Scan_DGV);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "KBScan_F";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Сканирование чертежей";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.KBScan_F_FormClosing);
            this.Load += new System.EventHandler(this.Scan_F_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Scan_DGV)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem CreateAct_TSM;
        private System.Windows.Forms.ToolStripMenuItem проверкиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CheckedUnloading_TSM;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Add_B;
        private System.Windows.Forms.Button Cancel_B;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView Scan_DGV;
        public System.Windows.Forms.TextBox ServerStatus_TB;
        public System.Windows.Forms.TextBox Status_TB;
        private System.Windows.Forms.DataGridViewTextBoxColumn DataMatrixOrder;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Unique;
    }
}