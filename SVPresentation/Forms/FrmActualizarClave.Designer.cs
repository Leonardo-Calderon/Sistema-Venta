namespace SVPresentation.Forms
{
    partial class FrmActualizarClave
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
            label2 = new Label();
            txbClave = new TextBox();
            txbClavex2 = new TextBox();
            lblValidacion = new Label();
            btnGuardar = new Button();
            btnVolver = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 14F);
            label1.Location = new Point(73, 48);
            label1.Name = "label1";
            label1.Size = new Size(162, 38);
            label1.TabIndex = 0;
            label1.Text = "Contraseña:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 14F);
            label2.Location = new Point(73, 185);
            label2.Name = "label2";
            label2.Size = new Size(279, 38);
            label2.TabIndex = 1;
            label2.Text = "Confimar contraseña:";
            // 
            // txbClave
            // 
            txbClave.Location = new Point(73, 120);
            txbClave.Name = "txbClave";
            txbClave.Size = new Size(429, 31);
            txbClave.TabIndex = 2;
            // 
            // txbClavex2
            // 
            txbClavex2.Location = new Point(73, 257);
            txbClavex2.Name = "txbClavex2";
            txbClavex2.Size = new Size(429, 31);
            txbClavex2.TabIndex = 3;
            // 
            // lblValidacion
            // 
            lblValidacion.AutoSize = true;
            lblValidacion.Font = new Font("Segoe UI", 11F);
            lblValidacion.ForeColor = Color.Firebrick;
            lblValidacion.Location = new Point(73, 322);
            lblValidacion.Name = "lblValidacion";
            lblValidacion.Size = new Size(292, 30);
            lblValidacion.TabIndex = 4;
            lblValidacion.Text = "Las contraseñas no coinciden";
            // 
            // btnGuardar
            // 
            btnGuardar.BackColor = SystemColors.MenuBar;
            btnGuardar.Cursor = Cursors.Hand;
            btnGuardar.FlatStyle = FlatStyle.Flat;
            btnGuardar.ForeColor = SystemColors.Highlight;
            btnGuardar.Location = new Point(618, 382);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(112, 34);
            btnGuardar.TabIndex = 9;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = false;
            btnGuardar.Click += btnGuardar_Click;
            // 
            // btnVolver
            // 
            btnVolver.BackColor = SystemColors.MenuBar;
            btnVolver.Cursor = Cursors.Hand;
            btnVolver.FlatStyle = FlatStyle.Flat;
            btnVolver.ForeColor = SystemColors.ControlText;
            btnVolver.Location = new Point(73, 382);
            btnVolver.Name = "btnVolver";
            btnVolver.Size = new Size(112, 34);
            btnVolver.TabIndex = 10;
            btnVolver.Text = "Volver";
            btnVolver.UseVisualStyleBackColor = false;
            btnVolver.Click += btnVolver_Click;
            // 
            // FrmActualizarClave
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnVolver);
            Controls.Add(btnGuardar);
            Controls.Add(lblValidacion);
            Controls.Add(txbClavex2);
            Controls.Add(txbClave);
            Controls.Add(label2);
            Controls.Add(label1);
            MaximizeBox = false;
            MaximumSize = new Size(822, 506);
            MinimizeBox = false;
            MinimumSize = new Size(822, 506);
            Name = "FrmActualizarClave";
            Text = "Actualizar Contraseña";
            Load += FrmActualizarClave_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private TextBox txbClave;
        private TextBox txbClavex2;
        private Label lblValidacion;
        private Button btnGuardar;
        private Button btnVolver;
    }
}