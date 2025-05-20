namespace SVPresentation.Forms
{
    partial class FrmLogin
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
            Label label1;
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            txbUsuario = new TextBox();
            txbContrasena = new TextBox();
            linkOlvideContrasena = new LinkLabel();
            btnEntrar = new Button();
            btnSalir = new Button();
            label1 = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.BackColor = Color.FromArgb(44, 110, 203);
            label1.Dock = DockStyle.Left;
            label1.Font = new Font("Segoe UI", 26F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.ForeColor = SystemColors.ControlLightLight;
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(624, 620);
            label1.TabIndex = 0;
            label1.Text = "Demo Sistemas de ventas";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 20F);
            label2.Location = new Point(672, 27);
            label2.Name = "label2";
            label2.Size = new Size(251, 54);
            label2.TabIndex = 1;
            label2.Text = "Iniciar sesión";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 12F);
            label3.Location = new Point(681, 120);
            label3.Name = "label3";
            label3.Size = new Size(94, 32);
            label3.TabIndex = 2;
            label3.Text = "Usuario";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 12F);
            label4.Location = new Point(681, 261);
            label4.Name = "label4";
            label4.Size = new Size(134, 32);
            label4.TabIndex = 3;
            label4.Text = "Contraseña";
            // 
            // txbUsuario
            // 
            txbUsuario.Location = new Point(681, 191);
            txbUsuario.Name = "txbUsuario";
            txbUsuario.Size = new Size(499, 31);
            txbUsuario.TabIndex = 4;
            // 
            // txbContrasena
            // 
            txbContrasena.Location = new Point(681, 325);
            txbContrasena.Name = "txbContrasena";
            txbContrasena.PasswordChar = '*';
            txbContrasena.Size = new Size(499, 31);
            txbContrasena.TabIndex = 5;
            // 
            // linkOlvideContrasena
            // 
            linkOlvideContrasena.AutoSize = true;
            linkOlvideContrasena.Font = new Font("Segoe UI", 11F);
            linkOlvideContrasena.Location = new Point(963, 529);
            linkOlvideContrasena.Name = "linkOlvideContrasena";
            linkOlvideContrasena.Size = new Size(217, 30);
            linkOlvideContrasena.TabIndex = 6;
            linkOlvideContrasena.TabStop = true;
            linkOlvideContrasena.Text = "Olvidé mi contraseña";
            linkOlvideContrasena.LinkClicked += linkOlvideContrasena_LinkClicked;
            // 
            // btnEntrar
            // 
            btnEntrar.BackColor = SystemColors.MenuBar;
            btnEntrar.Cursor = Cursors.Hand;
            btnEntrar.FlatStyle = FlatStyle.Flat;
            btnEntrar.ForeColor = SystemColors.Highlight;
            btnEntrar.Location = new Point(1068, 418);
            btnEntrar.Name = "btnEntrar";
            btnEntrar.Size = new Size(112, 34);
            btnEntrar.TabIndex = 8;
            btnEntrar.Text = "Entrar";
            btnEntrar.UseVisualStyleBackColor = false;
            btnEntrar.Click += btnEntrar_Click;
            // 
            // btnSalir
            // 
            btnSalir.BackColor = SystemColors.Menu;
            btnSalir.Cursor = Cursors.Hand;
            btnSalir.FlatStyle = FlatStyle.Flat;
            btnSalir.Location = new Point(681, 418);
            btnSalir.Name = "btnSalir";
            btnSalir.Size = new Size(112, 34);
            btnSalir.TabIndex = 7;
            btnSalir.Text = "Salir";
            btnSalir.UseVisualStyleBackColor = false;
            btnSalir.Click += btnSalir_Click;
            // 
            // FrmLogin
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1248, 620);
            Controls.Add(btnEntrar);
            Controls.Add(btnSalir);
            Controls.Add(linkOlvideContrasena);
            Controls.Add(txbContrasena);
            Controls.Add(txbUsuario);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmLogin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FrmLogin";
            Load += FrmLogin_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private TextBox txbUsuario;
        private TextBox txbContrasena;
        private LinkLabel linkOlvideContrasena;
        private Button btnEntrar;
        private Button btnSalir;
    }
}