namespace SVPresentation.Forms
{
    partial class FrmDetalleVenta
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
            btnVerPDF = new Button();
            dgvDetalle = new DataGridView();
            label1 = new Label();
            lblNumeroVenta = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvDetalle).BeginInit();
            SuspendLayout();
            // 
            // btnVerPDF
            // 
            btnVerPDF.Location = new Point(783, 37);
            btnVerPDF.Name = "btnVerPDF";
            btnVerPDF.Size = new Size(112, 34);
            btnVerPDF.TabIndex = 3;
            btnVerPDF.Text = "Ver PDF";
            btnVerPDF.UseVisualStyleBackColor = true;
            btnVerPDF.Click += btnVerPDF_Click;
            // 
            // dgvDetalle
            // 
            dgvDetalle.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDetalle.Location = new Point(40, 77);
            dgvDetalle.Name = "dgvDetalle";
            dgvDetalle.RowHeadersWidth = 62;
            dgvDetalle.Size = new Size(855, 371);
            dgvDetalle.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(40, 42);
            label1.Name = "label1";
            label1.Size = new Size(154, 25);
            label1.TabIndex = 4;
            label1.Text = "Numero de venta:";
            // 
            // lblNumeroVenta
            // 
            lblNumeroVenta.AutoSize = true;
            lblNumeroVenta.Location = new Point(200, 46);
            lblNumeroVenta.Name = "lblNumeroVenta";
            lblNumeroVenta.Size = new Size(22, 25);
            lblNumeroVenta.TabIndex = 5;
            lblNumeroVenta.Text = "0";
            // 
            // FrmDetalleVenta
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(939, 475);
            Controls.Add(lblNumeroVenta);
            Controls.Add(label1);
            Controls.Add(btnVerPDF);
            Controls.Add(dgvDetalle);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmDetalleVenta";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FrmDetalleVenta";
            Load += FrmDetalleVenta_Load;
            ((System.ComponentModel.ISupportInitialize)dgvDetalle).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnVerPDF;
        private DataGridView dgvDetalle;
        private Label label1;
        private Label lblNumeroVenta;
    }
}