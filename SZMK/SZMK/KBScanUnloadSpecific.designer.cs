namespace SZMK
{
    partial class KBScanUnloadSpecific
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
            this.Report_DGV = new System.Windows.Forms.DataGridView();
            this.Number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.List = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Executor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NumberSpecific = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Finded = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.OK_B = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.Report_DGV)).BeginInit();
            this.SuspendLayout();
            // 
            // Report_DGV
            // 
            this.Report_DGV.AllowUserToAddRows = false;
            this.Report_DGV.AllowUserToDeleteRows = false;
            this.Report_DGV.AllowUserToResizeColumns = false;
            this.Report_DGV.AllowUserToResizeRows = false;
            this.Report_DGV.BackgroundColor = System.Drawing.Color.White;
            this.Report_DGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Report_DGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Number,
            this.List,
            this.Executor,
            this.NumberSpecific,
            this.Finded});
            this.Report_DGV.Location = new System.Drawing.Point(9, 52);
            this.Report_DGV.Name = "Report_DGV";
            this.Report_DGV.ReadOnly = true;
            this.Report_DGV.RowHeadersVisible = false;
            this.Report_DGV.Size = new System.Drawing.Size(773, 386);
            this.Report_DGV.TabIndex = 0;
            // 
            // Number
            // 
            this.Number.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Number.DataPropertyName = "Number";
            this.Number.FillWeight = 20F;
            this.Number.HeaderText = "Номер заказа";
            this.Number.Name = "Number";
            this.Number.ReadOnly = true;
            this.Number.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // List
            // 
            this.List.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.List.DataPropertyName = "List";
            this.List.FillWeight = 15F;
            this.List.HeaderText = "Номер листа";
            this.List.Name = "List";
            this.List.ReadOnly = true;
            this.List.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Executor
            // 
            this.Executor.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Executor.DataPropertyName = "Executor";
            this.Executor.FillWeight = 40F;
            this.Executor.HeaderText = "Исполнитель";
            this.Executor.Name = "Executor";
            this.Executor.ReadOnly = true;
            this.Executor.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // NumberSpecific
            // 
            this.NumberSpecific.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.NumberSpecific.DataPropertyName = "NumberSpecific";
            this.NumberSpecific.FillWeight = 15F;
            this.NumberSpecific.HeaderText = "Номер детали";
            this.NumberSpecific.Name = "NumberSpecific";
            this.NumberSpecific.ReadOnly = true;
            this.NumberSpecific.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Finded
            // 
            this.Finded.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Finded.DataPropertyName = "Finded";
            this.Finded.FalseValue = "Найден";
            this.Finded.FillWeight = 10F;
            this.Finded.HeaderText = "Найден";
            this.Finded.Name = "Finded";
            this.Finded.ReadOnly = true;
            this.Finded.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Finded.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Finded.TrueValue = "Не найден";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.MediumTurquoise;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(773, 40);
            this.label1.TabIndex = 1;
            this.label1.Text = "Отчет о проверке";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // OK_B
            // 
            this.OK_B.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.OK_B.Location = new System.Drawing.Point(329, 444);
            this.OK_B.Name = "OK_B";
            this.OK_B.Size = new System.Drawing.Size(150, 28);
            this.OK_B.TabIndex = 2;
            this.OK_B.Text = "Закрыть";
            this.OK_B.UseVisualStyleBackColor = true;
            // 
            // KBScanUnloadSpecific
            // 
            this.AcceptButton = this.OK_B;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.OK_B;
            this.ClientSize = new System.Drawing.Size(790, 477);
            this.Controls.Add(this.OK_B);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Report_DGV);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "KBScanUnloadSpecific";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Отчет о проверке";
            this.Load += new System.EventHandler(this.KBScanUnloadSpecific_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Report_DGV)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView Report_DGV;
        private System.Windows.Forms.DataGridViewTextBoxColumn Number;
        private System.Windows.Forms.DataGridViewTextBoxColumn List;
        private System.Windows.Forms.DataGridViewTextBoxColumn Executor;
        private System.Windows.Forms.DataGridViewTextBoxColumn NumberSpecific;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Finded;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button OK_B;
    }
}