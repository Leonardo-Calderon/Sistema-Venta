namespace SVPresentation.Forms
{
    partial class FrmVenta
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
            btnGuardarConfiguracion = new Button();
            btnBuscar = new Button();
            txbCambio = new TextBox();
            label7 = new Label();
            txbPagoCon = new TextBox();
            label6 = new Label();
            txbNombreCliente = new TextBox();
            label4 = new Label();
            txbCodigoProducto = new TextBox();
            label2 = new Label();
            label1 = new Label();
            label3 = new Label();
            label5 = new Label();
            lblTotal = new Label();
            dgvDetalleVenta = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgvDetalleVenta).BeginInit();
            SuspendLayout();
            // 
            // btnGuardarConfiguracion
            // 
            btnGuardarConfiguracion.BackColor = SystemColors.Menu;
            btnGuardarConfiguracion.Cursor = Cursors.Hand;
            btnGuardarConfiguracion.FlatStyle = FlatStyle.Flat;
            btnGuardarConfiguracion.ForeColor = Color.CornflowerBlue;
            btnGuardarConfiguracion.Location = new Point(1111, 534);
            btnGuardarConfiguracion.Name = "btnGuardarConfiguracion";
            btnGuardarConfiguracion.Size = new Size(112, 34);
            btnGuardarConfiguracion.TabIndex = 39;
            btnGuardarConfiguracion.Text = "Guardar";
            btnGuardarConfiguracion.UseVisualStyleBackColor = false;
            btnGuardarConfiguracion.Click += btnGuardarConfiguracion_Click;
            // 
            // btnBuscar
            // 
            btnBuscar.BackColor = SystemColors.Menu;
            btnBuscar.Cursor = Cursors.Hand;
            btnBuscar.FlatStyle = FlatStyle.Flat;
            btnBuscar.Location = new Point(736, 150);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(112, 34);
            btnBuscar.TabIndex = 37;
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = false;
            btnBuscar.Click += btnBuscar_Click;
            // 
            // txbCambio
            // 
            txbCambio.Location = new Point(340, 537);
            txbCambio.Name = "txbCambio";
            txbCambio.Size = new Size(219, 31);
            txbCambio.TabIndex = 32;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.BackColor = Color.White;
            label7.Location = new Point(340, 509);
            label7.Name = "label7";
            label7.Size = new Size(78, 25);
            label7.TabIndex = 31;
            label7.Text = "Cambio:";
            // 
            // txbPagoCon
            // 
            txbPagoCon.Location = new Point(24, 537);
            txbPagoCon.Name = "txbPagoCon";
            txbPagoCon.Size = new Size(219, 31);
            txbPagoCon.TabIndex = 30;
            txbPagoCon.KeyDown += txbPagoCon_KeyDown;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.BackColor = Color.White;
            label6.Location = new Point(24, 509);
            label6.Name = "label6";
            label6.Size = new Size(93, 25);
            label6.TabIndex = 29;
            label6.Text = "Pago Con:";
            // 
            // txbNombreCliente
            // 
            txbNombreCliente.Location = new Point(24, 226);
            txbNombreCliente.Name = "txbNombreCliente";
            txbNombreCliente.Size = new Size(824, 31);
            txbNombreCliente.TabIndex = 26;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.White;
            label4.Location = new Point(24, 198);
            label4.Name = "label4";
            label4.Size = new Size(136, 25);
            label4.TabIndex = 25;
            label4.Text = "Nombre Cliente";
            // 
            // txbCodigoProducto
            // 
            txbCodigoProducto.Location = new Point(24, 152);
            txbCodigoProducto.Name = "txbCodigoProducto";
            txbCodigoProducto.Size = new Size(673, 31);
            txbCodigoProducto.TabIndex = 23;
            txbCodigoProducto.KeyDown += txbCodigoProducto_KeyDown;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.White;
            label2.Location = new Point(24, 124);
            label2.Name = "label2";
            label2.Size = new Size(153, 25);
            label2.TabIndex = 22;
            label2.Text = "Código Producto:";
            // 
            // label1
            // 
            label1.BackColor = Color.White;
            label1.Location = new Point(16, 85);
            label1.Name = "label1";
            label1.Size = new Size(1220, 519);
            label1.TabIndex = 21;
            // 
            // label3
            // 
            label3.BackColor = Color.FromArgb(44, 110, 203);
            label3.Location = new Point(869, 124);
            label3.Name = "label3";
            label3.Size = new Size(354, 133);
            label3.TabIndex = 40;
            // 
            // label5
            // 
            label5.BackColor = Color.FromArgb(44, 110, 203);
            label5.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.ForeColor = Color.White;
            label5.Location = new Point(869, 136);
            label5.Name = "label5";
            label5.Size = new Size(354, 47);
            label5.TabIndex = 41;
            label5.Text = "Total:";
            label5.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTotal
            // 
            lblTotal.BackColor = Color.FromArgb(44, 110, 203);
            lblTotal.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTotal.ForeColor = Color.White;
            lblTotal.Location = new Point(869, 198);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(354, 47);
            lblTotal.TabIndex = 42;
            lblTotal.Text = "0";
            lblTotal.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // dgvDetalleVenta
            // 
            dgvDetalleVenta.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDetalleVenta.Location = new Point(22, 272);
            dgvDetalleVenta.Name = "dgvDetalleVenta";
            dgvDetalleVenta.RowHeadersWidth = 62;
            dgvDetalleVenta.Size = new Size(1204, 234);
            dgvDetalleVenta.TabIndex = 43;
            dgvDetalleVenta.CellContentClick += dgvDetalleVenta_CellContentClick;
            // 
            // FrmVenta
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1248, 620);
            Controls.Add(dgvDetalleVenta);
            Controls.Add(lblTotal);
            Controls.Add(label5);
            Controls.Add(label3);
            Controls.Add(btnGuardarConfiguracion);
            Controls.Add(btnBuscar);
            Controls.Add(txbCambio);
            Controls.Add(label7);
            Controls.Add(txbPagoCon);
            Controls.Add(label6);
            Controls.Add(txbNombreCliente);
            Controls.Add(label4);
            Controls.Add(txbCodigoProducto);
            Controls.Add(label2);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmVenta";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FrmVenta";
            Load += FrmVenta_Load;
            ((System.ComponentModel.ISupportInitialize)dgvDetalleVenta).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnGuardarConfiguracion;
        private Button btnBuscar;
        private TextBox txbCambio;
        private Label label7;
        private TextBox txbPagoCon;
        private Label label6;
        private TextBox txbNombreCliente;
        private Label label4;
        private TextBox txbCodigoProducto;
        private Label label2;
        private Label label1;
        private Label label3;
        private Label label5;
        private Label lblTotal;
        private DataGridView dgvDetalleVenta;
    }
}