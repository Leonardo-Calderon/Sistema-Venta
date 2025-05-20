namespace SVPresentation.Forms
{
    partial class FrmUsuario
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
            btnGuardarEditar = new Button();
            btnVolverEditar = new Button();
            btnGuardarNuevo = new Button();
            btnVolverNuevo = new Button();
            cbxRolNuevo = new ComboBox();
            label3 = new Label();
            tabEditar = new TabPage();
            cbxHabilitado = new ComboBox();
            label10 = new Label();
            txbNomUsuEditar = new TextBox();
            label4 = new Label();
            tbxCorreoEditar = new TextBox();
            label5 = new Label();
            cbxRolEditar = new ComboBox();
            label6 = new Label();
            txbNomComEditar = new TextBox();
            label9 = new Label();
            txbNomComNuevo = new TextBox();
            tabNuevo = new TabPage();
            txbNomUsuNuevo = new TextBox();
            label8 = new Label();
            txbCorreoNuevo = new TextBox();
            label7 = new Label();
            label2 = new Label();
            dgvUsuarios = new DataGridView();
            btnBuscar = new Button();
            btnNuevo = new Button();
            txbBuscar = new TextBox();
            tabLista = new TabPage();
            tabControlMain = new TabControl();
            tabEditar.SuspendLayout();
            tabNuevo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).BeginInit();
            tabLista.SuspendLayout();
            tabControlMain.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 11);
            label1.Name = "label1";
            label1.Size = new Size(72, 25);
            label1.TabIndex = 3;
            label1.Text = "Usuario";
            // 
            // btnGuardarEditar
            // 
            btnGuardarEditar.BackColor = SystemColors.Menu;
            btnGuardarEditar.Cursor = Cursors.Hand;
            btnGuardarEditar.FlatStyle = FlatStyle.Flat;
            btnGuardarEditar.ForeColor = Color.CornflowerBlue;
            btnGuardarEditar.Location = new Point(1083, 455);
            btnGuardarEditar.Name = "btnGuardarEditar";
            btnGuardarEditar.Size = new Size(112, 34);
            btnGuardarEditar.TabIndex = 13;
            btnGuardarEditar.Text = "Guardar";
            btnGuardarEditar.UseVisualStyleBackColor = false;
            btnGuardarEditar.Click += btnGuardarEditar_Click;
            // 
            // btnVolverEditar
            // 
            btnVolverEditar.BackColor = SystemColors.Menu;
            btnVolverEditar.Cursor = Cursors.Hand;
            btnVolverEditar.FlatStyle = FlatStyle.Flat;
            btnVolverEditar.Location = new Point(21, 455);
            btnVolverEditar.Name = "btnVolverEditar";
            btnVolverEditar.Size = new Size(112, 34);
            btnVolverEditar.TabIndex = 12;
            btnVolverEditar.Text = "Volver";
            btnVolverEditar.UseVisualStyleBackColor = false;
            btnVolverEditar.Click += btnVolverEditar_Click;
            // 
            // btnGuardarNuevo
            // 
            btnGuardarNuevo.BackColor = SystemColors.Menu;
            btnGuardarNuevo.Cursor = Cursors.Hand;
            btnGuardarNuevo.FlatStyle = FlatStyle.Flat;
            btnGuardarNuevo.ForeColor = Color.CornflowerBlue;
            btnGuardarNuevo.Location = new Point(1083, 455);
            btnGuardarNuevo.Name = "btnGuardarNuevo";
            btnGuardarNuevo.Size = new Size(112, 34);
            btnGuardarNuevo.TabIndex = 7;
            btnGuardarNuevo.Text = "Guardar";
            btnGuardarNuevo.UseVisualStyleBackColor = false;
            btnGuardarNuevo.Click += btnGuardarNuevo_Click;
            // 
            // btnVolverNuevo
            // 
            btnVolverNuevo.BackColor = SystemColors.Menu;
            btnVolverNuevo.Cursor = Cursors.Hand;
            btnVolverNuevo.FlatStyle = FlatStyle.Flat;
            btnVolverNuevo.Location = new Point(21, 455);
            btnVolverNuevo.Name = "btnVolverNuevo";
            btnVolverNuevo.Size = new Size(112, 34);
            btnVolverNuevo.TabIndex = 6;
            btnVolverNuevo.Text = "Volver";
            btnVolverNuevo.UseVisualStyleBackColor = false;
            btnVolverNuevo.Click += btnVolverNuevo_Click;
            // 
            // cbxRolNuevo
            // 
            cbxRolNuevo.Cursor = Cursors.Hand;
            cbxRolNuevo.DropDownStyle = ComboBoxStyle.DropDownList;
            cbxRolNuevo.FormattingEnabled = true;
            cbxRolNuevo.Location = new Point(21, 67);
            cbxRolNuevo.Name = "cbxRolNuevo";
            cbxRolNuevo.Size = new Size(532, 33);
            cbxRolNuevo.TabIndex = 3;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(24, 30);
            label3.Name = "label3";
            label3.Size = new Size(41, 25);
            label3.TabIndex = 2;
            label3.Text = "Rol:";
            // 
            // tabEditar
            // 
            tabEditar.Controls.Add(cbxHabilitado);
            tabEditar.Controls.Add(label10);
            tabEditar.Controls.Add(txbNomUsuEditar);
            tabEditar.Controls.Add(label4);
            tabEditar.Controls.Add(tbxCorreoEditar);
            tabEditar.Controls.Add(label5);
            tabEditar.Controls.Add(cbxRolEditar);
            tabEditar.Controls.Add(label6);
            tabEditar.Controls.Add(txbNomComEditar);
            tabEditar.Controls.Add(label9);
            tabEditar.Controls.Add(btnGuardarEditar);
            tabEditar.Controls.Add(btnVolverEditar);
            tabEditar.Location = new Point(4, 34);
            tabEditar.Name = "tabEditar";
            tabEditar.Padding = new Padding(3);
            tabEditar.Size = new Size(1216, 519);
            tabEditar.TabIndex = 2;
            tabEditar.Text = "Editar";
            tabEditar.UseVisualStyleBackColor = true;
            // 
            // cbxHabilitado
            // 
            cbxHabilitado.Cursor = Cursors.Hand;
            cbxHabilitado.DropDownStyle = ComboBoxStyle.DropDownList;
            cbxHabilitado.FormattingEnabled = true;
            cbxHabilitado.Location = new Point(24, 371);
            cbxHabilitado.Name = "cbxHabilitado";
            cbxHabilitado.Size = new Size(532, 33);
            cbxHabilitado.TabIndex = 23;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(27, 334);
            label10.Name = "label10";
            label10.Size = new Size(98, 25);
            label10.TabIndex = 22;
            label10.Text = "Habilitado:";
            // 
            // txbNomUsuEditar
            // 
            txbNomUsuEditar.Location = new Point(21, 292);
            txbNomUsuEditar.Name = "txbNomUsuEditar";
            txbNomUsuEditar.Size = new Size(532, 31);
            txbNomUsuEditar.TabIndex = 21;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(24, 255);
            label4.Name = "label4";
            label4.Size = new Size(172, 25);
            label4.TabIndex = 20;
            label4.Text = "Nombre de Usuario:";
            // 
            // tbxCorreoEditar
            // 
            tbxCorreoEditar.Location = new Point(21, 212);
            tbxCorreoEditar.Name = "tbxCorreoEditar";
            tbxCorreoEditar.Size = new Size(532, 31);
            tbxCorreoEditar.TabIndex = 19;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(24, 175);
            label5.Name = "label5";
            label5.Size = new Size(70, 25);
            label5.TabIndex = 18;
            label5.Text = "Correo:";
            // 
            // cbxRolEditar
            // 
            cbxRolEditar.Cursor = Cursors.Hand;
            cbxRolEditar.DropDownStyle = ComboBoxStyle.DropDownList;
            cbxRolEditar.FormattingEnabled = true;
            cbxRolEditar.Location = new Point(21, 50);
            cbxRolEditar.Name = "cbxRolEditar";
            cbxRolEditar.Size = new Size(532, 33);
            cbxRolEditar.TabIndex = 17;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(24, 13);
            label6.Name = "label6";
            label6.Size = new Size(41, 25);
            label6.TabIndex = 16;
            label6.Text = "Rol:";
            // 
            // txbNomComEditar
            // 
            txbNomComEditar.Location = new Point(21, 132);
            txbNomComEditar.Name = "txbNomComEditar";
            txbNomComEditar.Size = new Size(532, 31);
            txbNomComEditar.TabIndex = 15;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(24, 95);
            label9.Name = "label9";
            label9.Size = new Size(166, 25);
            label9.TabIndex = 14;
            label9.Text = "Nombre Completo:";
            // 
            // txbNomComNuevo
            // 
            txbNomComNuevo.Location = new Point(21, 149);
            txbNomComNuevo.Name = "txbNomComNuevo";
            txbNomComNuevo.Size = new Size(532, 31);
            txbNomComNuevo.TabIndex = 1;
            // 
            // tabNuevo
            // 
            tabNuevo.Controls.Add(txbNomUsuNuevo);
            tabNuevo.Controls.Add(label8);
            tabNuevo.Controls.Add(txbCorreoNuevo);
            tabNuevo.Controls.Add(label7);
            tabNuevo.Controls.Add(btnGuardarNuevo);
            tabNuevo.Controls.Add(btnVolverNuevo);
            tabNuevo.Controls.Add(cbxRolNuevo);
            tabNuevo.Controls.Add(label3);
            tabNuevo.Controls.Add(txbNomComNuevo);
            tabNuevo.Controls.Add(label2);
            tabNuevo.Location = new Point(4, 34);
            tabNuevo.Name = "tabNuevo";
            tabNuevo.Padding = new Padding(3);
            tabNuevo.Size = new Size(1216, 519);
            tabNuevo.TabIndex = 1;
            tabNuevo.Text = "Nuevo";
            tabNuevo.UseVisualStyleBackColor = true;
            // 
            // txbNomUsuNuevo
            // 
            txbNomUsuNuevo.Location = new Point(21, 309);
            txbNomUsuNuevo.Name = "txbNomUsuNuevo";
            txbNomUsuNuevo.Size = new Size(532, 31);
            txbNomUsuNuevo.TabIndex = 11;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(24, 272);
            label8.Name = "label8";
            label8.Size = new Size(172, 25);
            label8.TabIndex = 10;
            label8.Text = "Nombre de Usuario:";
            // 
            // txbCorreoNuevo
            // 
            txbCorreoNuevo.Location = new Point(21, 229);
            txbCorreoNuevo.Name = "txbCorreoNuevo";
            txbCorreoNuevo.Size = new Size(532, 31);
            txbCorreoNuevo.TabIndex = 9;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(24, 192);
            label7.Name = "label7";
            label7.Size = new Size(70, 25);
            label7.TabIndex = 8;
            label7.Text = "Correo:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(24, 112);
            label2.Name = "label2";
            label2.Size = new Size(166, 25);
            label2.TabIndex = 0;
            label2.Text = "Nombre Completo:";
            // 
            // dgvUsuarios
            // 
            dgvUsuarios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvUsuarios.Location = new Point(6, 65);
            dgvUsuarios.Name = "dgvUsuarios";
            dgvUsuarios.RowHeadersWidth = 62;
            dgvUsuarios.Size = new Size(1204, 438);
            dgvUsuarios.TabIndex = 7;
            dgvUsuarios.CellContentClick += dgvUsuarios_CellContentClick;
            // 
            // btnBuscar
            // 
            btnBuscar.BackColor = SystemColors.Menu;
            btnBuscar.Cursor = Cursors.Hand;
            btnBuscar.FlatStyle = FlatStyle.Flat;
            btnBuscar.Location = new Point(1067, 16);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(112, 34);
            btnBuscar.TabIndex = 6;
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = false;
            btnBuscar.Click += btnBuscar_Click;
            // 
            // btnNuevo
            // 
            btnNuevo.BackColor = SystemColors.Menu;
            btnNuevo.Cursor = Cursors.Hand;
            btnNuevo.FlatStyle = FlatStyle.Flat;
            btnNuevo.Location = new Point(18, 16);
            btnNuevo.Name = "btnNuevo";
            btnNuevo.Size = new Size(112, 34);
            btnNuevo.TabIndex = 5;
            btnNuevo.Text = "Nuevo";
            btnNuevo.UseVisualStyleBackColor = false;
            btnNuevo.Click += btnNuevo_Click;
            // 
            // txbBuscar
            // 
            txbBuscar.Cursor = Cursors.IBeam;
            txbBuscar.Location = new Point(795, 18);
            txbBuscar.Name = "txbBuscar";
            txbBuscar.Size = new Size(249, 31);
            txbBuscar.TabIndex = 4;
            // 
            // tabLista
            // 
            tabLista.Controls.Add(dgvUsuarios);
            tabLista.Controls.Add(btnBuscar);
            tabLista.Controls.Add(btnNuevo);
            tabLista.Controls.Add(txbBuscar);
            tabLista.ImeMode = ImeMode.Off;
            tabLista.Location = new Point(4, 34);
            tabLista.Name = "tabLista";
            tabLista.Padding = new Padding(3);
            tabLista.Size = new Size(1216, 519);
            tabLista.TabIndex = 0;
            tabLista.Text = "Lista";
            tabLista.UseVisualStyleBackColor = true;
            // 
            // tabControlMain
            // 
            tabControlMain.Controls.Add(tabLista);
            tabControlMain.Controls.Add(tabNuevo);
            tabControlMain.Controls.Add(tabEditar);
            tabControlMain.ItemSize = new Size(80, 30);
            tabControlMain.Location = new Point(12, 53);
            tabControlMain.Name = "tabControlMain";
            tabControlMain.SelectedIndex = 0;
            tabControlMain.Size = new Size(1224, 557);
            tabControlMain.SizeMode = TabSizeMode.Fixed;
            tabControlMain.TabIndex = 2;
            // 
            // FrmUsuario
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(1248, 620);
            Controls.Add(label1);
            Controls.Add(tabControlMain);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmUsuario";
            StartPosition = FormStartPosition.CenterParent;
            Text = "FrmUsuario";
            Load += FrmUsuario_Load;
            tabEditar.ResumeLayout(false);
            tabEditar.PerformLayout();
            tabNuevo.ResumeLayout(false);
            tabNuevo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).EndInit();
            tabLista.ResumeLayout(false);
            tabLista.PerformLayout();
            tabControlMain.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Button btnGuardarEditar;
        private Button btnVolverEditar;
        private Button btnGuardarNuevo;
        private Button btnVolverNuevo;
        private ComboBox cbxRolNuevo;
        private Label label3;
        private TabPage tabEditar;
        private TextBox txbNomComNuevo;
        private TabPage tabNuevo;
        private Label label2;
        private DataGridView dgvUsuarios;
        private Button btnBuscar;
        private Button btnNuevo;
        private TextBox txbBuscar;
        private TabPage tabLista;
        private TabControl tabControlMain;
        private TextBox txbNomUsuNuevo;
        private Label label8;
        private TextBox txbCorreoNuevo;
        private Label label7;
        private ComboBox cbxHabilitado;
        private Label label10;
        private TextBox txbNomUsuEditar;
        private Label label4;
        private TextBox tbxCorreoEditar;
        private Label label5;
        private ComboBox cbxRolEditar;
        private Label label6;
        private TextBox txbNomComEditar;
        private Label label9;
    }
}