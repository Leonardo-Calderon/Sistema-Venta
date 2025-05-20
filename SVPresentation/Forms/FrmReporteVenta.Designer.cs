namespace SVPresentation.Forms
{
    partial class FrmReporteVenta
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
            dgvReporte = new DataGridView();
            dtpFechaFin = new DateTimePicker();
            dtpFechaInicio = new DateTimePicker();
            btnBuscar = new Button();
            label4 = new Label();
            label2 = new Label();
            label3 = new Label();
            label1 = new Label();
            btnExcel = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvReporte).BeginInit();
            SuspendLayout();
            // 
            // dgvReporte
            // 
            dgvReporte.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvReporte.Location = new Point(35, 198);
            dgvReporte.Name = "dgvReporte";
            dgvReporte.RowHeadersWidth = 62;
            dgvReporte.Size = new Size(1182, 376);
            dgvReporte.TabIndex = 50;
            // 
            // dtpFechaFin
            // 
            dtpFechaFin.Format = DateTimePickerFormat.Short;
            dtpFechaFin.Location = new Point(341, 136);
            dtpFechaFin.Name = "dtpFechaFin";
            dtpFechaFin.Size = new Size(300, 31);
            dtpFechaFin.TabIndex = 49;
            // 
            // dtpFechaInicio
            // 
            dtpFechaInicio.Format = DateTimePickerFormat.Short;
            dtpFechaInicio.Location = new Point(16, 136);
            dtpFechaInicio.Name = "dtpFechaInicio";
            dtpFechaInicio.Size = new Size(300, 31);
            dtpFechaInicio.TabIndex = 48;
            // 
            // btnBuscar
            // 
            btnBuscar.BackColor = SystemColors.Menu;
            btnBuscar.Cursor = Cursors.Hand;
            btnBuscar.FlatStyle = FlatStyle.Flat;
            btnBuscar.Location = new Point(660, 136);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(112, 34);
            btnBuscar.TabIndex = 47;
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = false;
            btnBuscar.Click += btnBuscar_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.White;
            label4.Location = new Point(341, 107);
            label4.Name = "label4";
            label4.Size = new Size(89, 25);
            label4.TabIndex = 44;
            label4.Text = "Fecha Fin:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.White;
            label2.Location = new Point(16, 107);
            label2.Name = "label2";
            label2.Size = new Size(108, 25);
            label2.TabIndex = 42;
            label2.Text = "Fecha Inicio:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(20, 33);
            label3.Name = "label3";
            label3.Size = new Size(147, 25);
            label3.TabIndex = 43;
            label3.Text = "Reporte de venta";
            // 
            // label1
            // 
            label1.BackColor = Color.White;
            label1.Location = new Point(16, 68);
            label1.Name = "label1";
            label1.Size = new Size(1216, 519);
            label1.TabIndex = 41;
            // 
            // btnExcel
            // 
            btnExcel.BackColor = SystemColors.Menu;
            btnExcel.Cursor = Cursors.Hand;
            btnExcel.FlatStyle = FlatStyle.Flat;
            btnExcel.Location = new Point(1105, 136);
            btnExcel.Name = "btnExcel";
            btnExcel.Size = new Size(112, 34);
            btnExcel.TabIndex = 51;
            btnExcel.Text = "Excel";
            btnExcel.UseVisualStyleBackColor = false;
            btnExcel.Click += btnExcel_Click;
            // 
            // FrmReporteVenta
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1248, 620);
            Controls.Add(btnExcel);
            Controls.Add(dgvReporte);
            Controls.Add(dtpFechaFin);
            Controls.Add(dtpFechaInicio);
            Controls.Add(btnBuscar);
            Controls.Add(label4);
            Controls.Add(label2);
            Controls.Add(label3);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmReporteVenta";
            Text = "FrmReporteVenta";
            Load += FrmReporteVenta_Load;
            ((System.ComponentModel.ISupportInitialize)dgvReporte).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvReporte;
        private DateTimePicker dtpFechaFin;
        private DateTimePicker dtpFechaInicio;
        private Button btnBuscar;
        private Label label4;
        private Label label2;
        private Label label3;
        private Label label1;
        private Button btnExcel;
    }
}