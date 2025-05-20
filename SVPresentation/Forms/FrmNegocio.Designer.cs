namespace SVPresentation.Forms
{
    partial class FrmNegocio
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
            label1 = new Label();
            txbRazonSocial = new TextBox();
            label2 = new Label();
            label3 = new Label();
            txbRFC = new TextBox();
            label4 = new Label();
            txbDireccion = new TextBox();
            label5 = new Label();
            txbCelular = new TextBox();
            label6 = new Label();
            txbCorreo = new TextBox();
            label7 = new Label();
            txbSimboloMoneda = new TextBox();
            label8 = new Label();
            txbRutaImagen = new TextBox();
            label9 = new Label();
            btnBuscar = new Button();
            pbLogo = new PictureBox();
            btnGuardarConfiguracion = new Button();
            ((System.ComponentModel.ISupportInitialize)pbLogo).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.BackColor = Color.White;
            label1.Location = new Point(12, 92);
            label1.Name = "label1";
            label1.Size = new Size(1216, 519);
            label1.TabIndex = 0;
            label1.Click += label1_Click;
            // 
            // txbRazonSocial
            // 
            txbRazonSocial.Location = new Point(20, 159);
            txbRazonSocial.Name = "txbRazonSocial";
            txbRazonSocial.Size = new Size(532, 31);
            txbRazonSocial.TabIndex = 3;
            txbRazonSocial.TextChanged += txbNombreNuevo_TextChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.White;
            label2.Location = new Point(20, 131);
            label2.Name = "label2";
            label2.Size = new Size(114, 25);
            label2.TabIndex = 2;
            label2.Text = "Razón social:";
            label2.Click += label2_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(20, 24);
            label3.Name = "label3";
            label3.Size = new Size(123, 25);
            label3.TabIndex = 4;
            label3.Text = "Configuración";
            // 
            // txbRFC
            // 
            txbRFC.Location = new Point(20, 233);
            txbRFC.Name = "txbRFC";
            txbRFC.Size = new Size(532, 31);
            txbRFC.TabIndex = 6;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.White;
            label4.Location = new Point(20, 205);
            label4.Name = "label4";
            label4.Size = new Size(47, 25);
            label4.TabIndex = 5;
            label4.Text = "RFC:";
            // 
            // txbDireccion
            // 
            txbDireccion.Location = new Point(20, 315);
            txbDireccion.Name = "txbDireccion";
            txbDireccion.Size = new Size(532, 31);
            txbDireccion.TabIndex = 8;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = Color.White;
            label5.Location = new Point(20, 287);
            label5.Name = "label5";
            label5.Size = new Size(89, 25);
            label5.TabIndex = 7;
            label5.Text = "Direccion:";
            // 
            // txbCelular
            // 
            txbCelular.Location = new Point(20, 396);
            txbCelular.Name = "txbCelular";
            txbCelular.Size = new Size(532, 31);
            txbCelular.TabIndex = 10;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.BackColor = Color.White;
            label6.Location = new Point(20, 368);
            label6.Name = "label6";
            label6.Size = new Size(69, 25);
            label6.TabIndex = 9;
            label6.Text = "Celular:";
            // 
            // txbCorreo
            // 
            txbCorreo.Location = new Point(20, 480);
            txbCorreo.Name = "txbCorreo";
            txbCorreo.Size = new Size(532, 31);
            txbCorreo.TabIndex = 12;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.BackColor = Color.White;
            label7.Location = new Point(20, 452);
            label7.Name = "label7";
            label7.Size = new Size(70, 25);
            label7.TabIndex = 11;
            label7.Text = "Correo:";
            // 
            // txbSimboloMoneda
            // 
            txbSimboloMoneda.Location = new Point(584, 159);
            txbSimboloMoneda.Name = "txbSimboloMoneda";
            txbSimboloMoneda.Size = new Size(532, 31);
            txbSimboloMoneda.TabIndex = 14;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.BackColor = Color.White;
            label8.Location = new Point(584, 131);
            label8.Name = "label8";
            label8.Size = new Size(149, 25);
            label8.TabIndex = 13;
            label8.Text = "SimboloMoneda:";
            // 
            // txbRutaImagen
            // 
            txbRutaImagen.Location = new Point(584, 233);
            txbRutaImagen.Name = "txbRutaImagen";
            txbRutaImagen.ReadOnly = true;
            txbRutaImagen.Size = new Size(451, 31);
            txbRutaImagen.TabIndex = 16;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.BackColor = Color.White;
            label9.Location = new Point(585, 205);
            label9.Name = "label9";
            label9.Size = new Size(57, 25);
            label9.TabIndex = 15;
            label9.Text = "Logo:";
            label9.Click += label9_Click;
            // 
            // btnBuscar
            // 
            btnBuscar.BackColor = SystemColors.Menu;
            btnBuscar.Cursor = Cursors.Hand;
            btnBuscar.FlatStyle = FlatStyle.Flat;
            btnBuscar.Location = new Point(1060, 231);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(112, 34);
            btnBuscar.TabIndex = 17;
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = false;
            btnBuscar.Click += btnBuscar_Click;
            // 
            // pbLogo
            // 
            pbLogo.Location = new Point(584, 287);
            pbLogo.Name = "pbLogo";
            pbLogo.Size = new Size(451, 242);
            pbLogo.TabIndex = 19;
            pbLogo.TabStop = false;
            // 
            // btnGuardarConfiguracion
            // 
            btnGuardarConfiguracion.BackColor = SystemColors.Menu;
            btnGuardarConfiguracion.Cursor = Cursors.Hand;
            btnGuardarConfiguracion.FlatStyle = FlatStyle.Flat;
            btnGuardarConfiguracion.ForeColor = Color.CornflowerBlue;
            btnGuardarConfiguracion.Location = new Point(1060, 541);
            btnGuardarConfiguracion.Name = "btnGuardarConfiguracion";
            btnGuardarConfiguracion.Size = new Size(112, 34);
            btnGuardarConfiguracion.TabIndex = 20;
            btnGuardarConfiguracion.Text = "Guardar";
            btnGuardarConfiguracion.UseVisualStyleBackColor = false;
            btnGuardarConfiguracion.Click += btnGuardarConfiguracion_Click;
            // 
            // FrmNegocio
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1248, 620);
            Controls.Add(btnGuardarConfiguracion);
            Controls.Add(pbLogo);
            Controls.Add(btnBuscar);
            Controls.Add(txbRutaImagen);
            Controls.Add(label9);
            Controls.Add(txbSimboloMoneda);
            Controls.Add(label8);
            Controls.Add(txbCorreo);
            Controls.Add(label7);
            Controls.Add(txbCelular);
            Controls.Add(label6);
            Controls.Add(txbDireccion);
            Controls.Add(label5);
            Controls.Add(txbRFC);
            Controls.Add(label4);
            Controls.Add(txbRazonSocial);
            Controls.Add(label2);
            Controls.Add(label3);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmNegocio";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FrmNegocio";
            Load += FrmNegocio_Load;
            ((System.ComponentModel.ISupportInitialize)pbLogo).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox txbRazonSocial;
        private Label label2;
        private Label label3;
        private TextBox txbRFC;
        private Label label4;
        private TextBox txbDireccion;
        private Label label5;
        private TextBox txbCelular;
        private Label label6;
        private TextBox txbCorreo;
        private Label label7;
        private TextBox txbSimboloMoneda;
        private Label label8;
        private TextBox txbRutaImagen;
        private Label label9;
        private Button btnBuscar;
        private PictureBox pbLogo;
        private Button btnGuardarConfiguracion;
    }
}