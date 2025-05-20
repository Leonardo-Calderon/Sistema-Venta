namespace SVPresentation.Forms
{
    partial class FrmCategoria
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
            tabControlMain = new TabControl();
            tabLista = new TabPage();
            dgvCategoriasLista = new DataGridView();
            btnBuscar = new Button();
            btnNuevoLista = new Button();
            txbBuscar = new TextBox();
            tabNuevo = new TabPage();
            btnGuardarNuevo = new Button();
            btnVolverNuevo = new Button();
            cbxMedidaNuevo = new ComboBox();
            label3 = new Label();
            txbNombreNuevo = new TextBox();
            label2 = new Label();
            tabEditar = new TabPage();
            cbxHabilitadoEditar = new ComboBox();
            label6 = new Label();
            btnGuardarEditar = new Button();
            btnVolverEditar = new Button();
            cbxMedidaEditar = new ComboBox();
            label4 = new Label();
            txbNombreEditar = new TextBox();
            label5 = new Label();
            label1 = new Label();
            tabControlMain.SuspendLayout();
            tabLista.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCategoriasLista).BeginInit();
            tabNuevo.SuspendLayout();
            tabEditar.SuspendLayout();
            SuspendLayout();
            // 
            // tabControlMain
            // 
            tabControlMain.Controls.Add(tabLista);
            tabControlMain.Controls.Add(tabNuevo);
            tabControlMain.Controls.Add(tabEditar);
            tabControlMain.ItemSize = new Size(80, 30);
            tabControlMain.Location = new Point(12, 51);
            tabControlMain.Name = "tabControlMain";
            tabControlMain.SelectedIndex = 0;
            tabControlMain.Size = new Size(1224, 557);
            tabControlMain.SizeMode = TabSizeMode.Fixed;
            tabControlMain.TabIndex = 0;
            // 
            // tabLista
            // 
            tabLista.Controls.Add(dgvCategoriasLista);
            tabLista.Controls.Add(btnBuscar);
            tabLista.Controls.Add(btnNuevoLista);
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
            // dgvCategoriasLista
            // 
            dgvCategoriasLista.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvCategoriasLista.Location = new Point(6, 65);
            dgvCategoriasLista.Name = "dgvCategoriasLista";
            dgvCategoriasLista.RowHeadersWidth = 62;
            dgvCategoriasLista.Size = new Size(1204, 438);
            dgvCategoriasLista.TabIndex = 7;
            dgvCategoriasLista.CellContentClick += dgvCategorias_CellContentClick;
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
            // btnNuevoLista
            // 
            btnNuevoLista.BackColor = SystemColors.Menu;
            btnNuevoLista.Cursor = Cursors.Hand;
            btnNuevoLista.FlatStyle = FlatStyle.Flat;
            btnNuevoLista.Location = new Point(18, 16);
            btnNuevoLista.Name = "btnNuevoLista";
            btnNuevoLista.Size = new Size(112, 34);
            btnNuevoLista.TabIndex = 5;
            btnNuevoLista.Text = "Nuevo";
            btnNuevoLista.UseVisualStyleBackColor = false;
            btnNuevoLista.Click += btnNuevoLista_Click;
            // 
            // txbBuscar
            // 
            txbBuscar.Cursor = Cursors.IBeam;
            txbBuscar.Location = new Point(795, 18);
            txbBuscar.Name = "txbBuscar";
            txbBuscar.Size = new Size(249, 31);
            txbBuscar.TabIndex = 4;
            // 
            // tabNuevo
            // 
            tabNuevo.Controls.Add(btnGuardarNuevo);
            tabNuevo.Controls.Add(btnVolverNuevo);
            tabNuevo.Controls.Add(cbxMedidaNuevo);
            tabNuevo.Controls.Add(label3);
            tabNuevo.Controls.Add(txbNombreNuevo);
            tabNuevo.Controls.Add(label2);
            tabNuevo.Location = new Point(4, 34);
            tabNuevo.Name = "tabNuevo";
            tabNuevo.Padding = new Padding(3);
            tabNuevo.Size = new Size(1216, 519);
            tabNuevo.TabIndex = 1;
            tabNuevo.Text = "Nuevo";
            tabNuevo.UseVisualStyleBackColor = true;
            tabNuevo.Click += tabNuevo_Click;
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
            // cbxMedidaNuevo
            // 
            cbxMedidaNuevo.Cursor = Cursors.Hand;
            cbxMedidaNuevo.DropDownStyle = ComboBoxStyle.DropDownList;
            cbxMedidaNuevo.FormattingEnabled = true;
            cbxMedidaNuevo.Location = new Point(21, 148);
            cbxMedidaNuevo.Name = "cbxMedidaNuevo";
            cbxMedidaNuevo.Size = new Size(532, 33);
            cbxMedidaNuevo.TabIndex = 3;
            cbxMedidaNuevo.SelectedIndexChanged += cbxMedidaNuevo_SelectedIndexChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(21, 120);
            label3.Name = "label3";
            label3.Size = new Size(76, 25);
            label3.TabIndex = 2;
            label3.Text = "Medida:";
            label3.Click += label3_Click;
            // 
            // txbNombreNuevo
            // 
            txbNombreNuevo.Location = new Point(21, 57);
            txbNombreNuevo.Name = "txbNombreNuevo";
            txbNombreNuevo.Size = new Size(532, 31);
            txbNombreNuevo.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(21, 29);
            label2.Name = "label2";
            label2.Size = new Size(82, 25);
            label2.TabIndex = 0;
            label2.Text = "Nombre:";
            // 
            // tabEditar
            // 
            tabEditar.Controls.Add(cbxHabilitadoEditar);
            tabEditar.Controls.Add(label6);
            tabEditar.Controls.Add(btnGuardarEditar);
            tabEditar.Controls.Add(btnVolverEditar);
            tabEditar.Controls.Add(cbxMedidaEditar);
            tabEditar.Controls.Add(label4);
            tabEditar.Controls.Add(txbNombreEditar);
            tabEditar.Controls.Add(label5);
            tabEditar.Location = new Point(4, 34);
            tabEditar.Name = "tabEditar";
            tabEditar.Padding = new Padding(3);
            tabEditar.Size = new Size(1216, 519);
            tabEditar.TabIndex = 2;
            tabEditar.Text = "Editar";
            tabEditar.UseVisualStyleBackColor = true;
            // 
            // cbxHabilitadoEditar
            // 
            cbxHabilitadoEditar.Cursor = Cursors.Hand;
            cbxHabilitadoEditar.DropDownStyle = ComboBoxStyle.DropDownList;
            cbxHabilitadoEditar.FormattingEnabled = true;
            cbxHabilitadoEditar.Location = new Point(21, 239);
            cbxHabilitadoEditar.Name = "cbxHabilitadoEditar";
            cbxHabilitadoEditar.Size = new Size(532, 33);
            cbxHabilitadoEditar.TabIndex = 15;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(21, 211);
            label6.Name = "label6";
            label6.Size = new Size(98, 25);
            label6.TabIndex = 14;
            label6.Text = "Habilitado:";
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
            // cbxMedidaEditar
            // 
            cbxMedidaEditar.Cursor = Cursors.Hand;
            cbxMedidaEditar.DropDownStyle = ComboBoxStyle.DropDownList;
            cbxMedidaEditar.FormattingEnabled = true;
            cbxMedidaEditar.Location = new Point(21, 148);
            cbxMedidaEditar.Name = "cbxMedidaEditar";
            cbxMedidaEditar.Size = new Size(532, 33);
            cbxMedidaEditar.TabIndex = 11;
            cbxMedidaEditar.SelectedIndexChanged += comboBox2_SelectedIndexChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(21, 120);
            label4.Name = "label4";
            label4.Size = new Size(76, 25);
            label4.TabIndex = 10;
            label4.Text = "Medida:";
            label4.Click += label4_Click;
            // 
            // txbNombreEditar
            // 
            txbNombreEditar.Location = new Point(21, 57);
            txbNombreEditar.Name = "txbNombreEditar";
            txbNombreEditar.Size = new Size(532, 31);
            txbNombreEditar.TabIndex = 9;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(21, 29);
            label5.Name = "label5";
            label5.Size = new Size(82, 25);
            label5.TabIndex = 8;
            label5.Text = "Nombre:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(178, 25);
            label1.TabIndex = 1;
            label1.Text = "Inventario | categoría";
            // 
            // FrmCategoria
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1248, 620);
            Controls.Add(label1);
            Controls.Add(tabControlMain);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmCategoria";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FrmCategoria";
            Load += FrmCategoria_Load;
            tabControlMain.ResumeLayout(false);
            tabLista.ResumeLayout(false);
            tabLista.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCategoriasLista).EndInit();
            tabNuevo.ResumeLayout(false);
            tabNuevo.PerformLayout();
            tabEditar.ResumeLayout(false);
            tabEditar.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TabControl tabControlMain;
        private TabPage tabLista;
        private TabPage tabNuevo;
        private Label label1;
        private TabPage tabEditar;
        private DataGridView dgvCategoriasLista;
        private Button btnBuscar;
        private Button btnNuevoLista;
        private TextBox txbBuscar;
        private Label label2;
        private TextBox txbNombreNuevo;
        private Label label3;
        private ComboBox cbxMedidaNuevo;
        private Button btnVolverNuevo;
        private Button btnGuardarNuevo;
        private Button btnGuardarEditar;
        private Button btnVolverEditar;
        private ComboBox cbxMedidaEditar;
        private Label label4;
        private TextBox txbNombreEditar;
        private Label label5;
        private ComboBox cbxHabilitadoEditar;
        private Label label6;
    }
}