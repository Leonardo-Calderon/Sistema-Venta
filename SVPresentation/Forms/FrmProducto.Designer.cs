namespace SVPresentation.Forms
{
    partial class FrmProducto
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
            btnGuardarNuevo = new Button();
            btnVolverNuevo = new Button();
            cbxCategoriaNuevo = new ComboBox();
            label3 = new Label();
            tabEditar = new TabPage();
            cbxHabilitadoEditar = new ComboBox();
            label14 = new Label();
            btnVolverEditar = new Button();
            label4 = new Label();
            txbCantidadEditar = new NumericUpDown();
            txbPrecioVenEditar = new TextBox();
            label5 = new Label();
            txbPrecioCompEditar = new TextBox();
            label6 = new Label();
            txbDescripcionEditar = new TextBox();
            label11 = new Label();
            cbxCategoriaEditar = new ComboBox();
            label12 = new Label();
            txbCodigoEditar = new TextBox();
            label13 = new Label();
            txbCodigoNuevo = new TextBox();
            tabNuevo = new TabPage();
            label10 = new Label();
            txbCantidadNuevo = new NumericUpDown();
            txbPrecioVenNuevo = new TextBox();
            label9 = new Label();
            txbPrecioCompNuevo = new TextBox();
            label8 = new Label();
            txbDescripcionNuevo = new TextBox();
            label7 = new Label();
            label2 = new Label();
            dgvProductoLista = new DataGridView();
            btnBuscar = new Button();
            btnNuevoLista = new Button();
            txbBuscar = new TextBox();
            tabLista = new TabPage();
            tabControlMain = new TabControl();
            tabEditar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)txbCantidadEditar).BeginInit();
            tabNuevo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)txbCantidadNuevo).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvProductoLista).BeginInit();
            tabLista.SuspendLayout();
            tabControlMain.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 11);
            label1.Name = "label1";
            label1.Size = new Size(178, 25);
            label1.TabIndex = 3;
            label1.Text = "Inventario | Producto";
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
            // cbxCategoriaNuevo
            // 
            cbxCategoriaNuevo.Cursor = Cursors.Hand;
            cbxCategoriaNuevo.DropDownStyle = ComboBoxStyle.DropDownList;
            cbxCategoriaNuevo.FormattingEnabled = true;
            cbxCategoriaNuevo.Location = new Point(24, 44);
            cbxCategoriaNuevo.Name = "cbxCategoriaNuevo";
            cbxCategoriaNuevo.Size = new Size(532, 33);
            cbxCategoriaNuevo.TabIndex = 3;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(27, 19);
            label3.Name = "label3";
            label3.Size = new Size(92, 25);
            label3.TabIndex = 2;
            label3.Text = "Cateogría:";
            // 
            // tabEditar
            // 
            tabEditar.Controls.Add(cbxHabilitadoEditar);
            tabEditar.Controls.Add(label14);
            tabEditar.Controls.Add(btnVolverEditar);
            tabEditar.Controls.Add(label4);
            tabEditar.Controls.Add(txbCantidadEditar);
            tabEditar.Controls.Add(txbPrecioVenEditar);
            tabEditar.Controls.Add(label5);
            tabEditar.Controls.Add(txbPrecioCompEditar);
            tabEditar.Controls.Add(label6);
            tabEditar.Controls.Add(txbDescripcionEditar);
            tabEditar.Controls.Add(label11);
            tabEditar.Controls.Add(cbxCategoriaEditar);
            tabEditar.Controls.Add(label12);
            tabEditar.Controls.Add(txbCodigoEditar);
            tabEditar.Controls.Add(label13);
            tabEditar.Controls.Add(btnGuardarEditar);
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
            cbxHabilitadoEditar.Location = new Point(651, 126);
            cbxHabilitadoEditar.Name = "cbxHabilitadoEditar";
            cbxHabilitadoEditar.Size = new Size(532, 33);
            cbxHabilitadoEditar.TabIndex = 30;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(654, 101);
            label14.Name = "label14";
            label14.Size = new Size(98, 25);
            label14.TabIndex = 29;
            label14.Text = "Habilitado:";
            label14.Click += label14_Click;
            // 
            // btnVolverEditar
            // 
            btnVolverEditar.BackColor = SystemColors.Menu;
            btnVolverEditar.Cursor = Cursors.Hand;
            btnVolverEditar.FlatStyle = FlatStyle.Flat;
            btnVolverEditar.Location = new Point(21, 455);
            btnVolverEditar.Name = "btnVolverEditar";
            btnVolverEditar.Size = new Size(112, 34);
            btnVolverEditar.TabIndex = 28;
            btnVolverEditar.Text = "Volver";
            btnVolverEditar.UseVisualStyleBackColor = false;
            btnVolverEditar.Click += btnVolverEditar_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(651, 17);
            label4.Name = "label4";
            label4.Size = new Size(87, 25);
            label4.TabIndex = 27;
            label4.Text = "Cantidad:";
            // 
            // txbCantidadEditar
            // 
            txbCantidadEditar.Cursor = Cursors.IBeam;
            txbCantidadEditar.Location = new Point(651, 45);
            txbCantidadEditar.Name = "txbCantidadEditar";
            txbCantidadEditar.Size = new Size(532, 31);
            txbCantidadEditar.TabIndex = 26;
            // 
            // txbPrecioVenEditar
            // 
            txbPrecioVenEditar.Cursor = Cursors.IBeam;
            txbPrecioVenEditar.Location = new Point(27, 374);
            txbPrecioVenEditar.Name = "txbPrecioVenEditar";
            txbPrecioVenEditar.Size = new Size(532, 31);
            txbPrecioVenEditar.TabIndex = 25;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(30, 349);
            label5.Name = "label5";
            label5.Size = new Size(137, 25);
            label5.TabIndex = 24;
            label5.Text = "Precio de venta:";
            // 
            // txbPrecioCompEditar
            // 
            txbPrecioCompEditar.Cursor = Cursors.IBeam;
            txbPrecioCompEditar.Location = new Point(24, 296);
            txbPrecioCompEditar.Name = "txbPrecioCompEditar";
            txbPrecioCompEditar.Size = new Size(532, 31);
            txbPrecioCompEditar.TabIndex = 23;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(27, 271);
            label6.Name = "label6";
            label6.Size = new Size(155, 25);
            label6.TabIndex = 22;
            label6.Text = "Precio de compra:";
            // 
            // txbDescripcionEditar
            // 
            txbDescripcionEditar.Cursor = Cursors.IBeam;
            txbDescripcionEditar.Location = new Point(24, 209);
            txbDescripcionEditar.Name = "txbDescripcionEditar";
            txbDescripcionEditar.Size = new Size(532, 31);
            txbDescripcionEditar.TabIndex = 21;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(27, 184);
            label11.Name = "label11";
            label11.Size = new Size(108, 25);
            label11.TabIndex = 20;
            label11.Text = "Descripción:";
            // 
            // cbxCategoriaEditar
            // 
            cbxCategoriaEditar.Cursor = Cursors.Hand;
            cbxCategoriaEditar.DropDownStyle = ComboBoxStyle.DropDownList;
            cbxCategoriaEditar.FormattingEnabled = true;
            cbxCategoriaEditar.Location = new Point(24, 44);
            cbxCategoriaEditar.Name = "cbxCategoriaEditar";
            cbxCategoriaEditar.Size = new Size(532, 33);
            cbxCategoriaEditar.TabIndex = 19;
            cbxCategoriaEditar.SelectedIndexChanged += cbxCategoriaEditar_SelectedIndexChanged;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(27, 19);
            label12.Name = "label12";
            label12.Size = new Size(92, 25);
            label12.TabIndex = 18;
            label12.Text = "Cateogría:";
            // 
            // txbCodigoEditar
            // 
            txbCodigoEditar.Cursor = Cursors.IBeam;
            txbCodigoEditar.Location = new Point(24, 126);
            txbCodigoEditar.Name = "txbCodigoEditar";
            txbCodigoEditar.Size = new Size(532, 31);
            txbCodigoEditar.TabIndex = 17;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(27, 101);
            label13.Name = "label13";
            label13.Size = new Size(75, 25);
            label13.TabIndex = 16;
            label13.Text = "Código:";
            // 
            // txbCodigoNuevo
            // 
            txbCodigoNuevo.Cursor = Cursors.IBeam;
            txbCodigoNuevo.Location = new Point(24, 126);
            txbCodigoNuevo.Name = "txbCodigoNuevo";
            txbCodigoNuevo.Size = new Size(532, 31);
            txbCodigoNuevo.TabIndex = 1;
            // 
            // tabNuevo
            // 
            tabNuevo.Controls.Add(label10);
            tabNuevo.Controls.Add(txbCantidadNuevo);
            tabNuevo.Controls.Add(txbPrecioVenNuevo);
            tabNuevo.Controls.Add(label9);
            tabNuevo.Controls.Add(txbPrecioCompNuevo);
            tabNuevo.Controls.Add(label8);
            tabNuevo.Controls.Add(txbDescripcionNuevo);
            tabNuevo.Controls.Add(label7);
            tabNuevo.Controls.Add(btnGuardarNuevo);
            tabNuevo.Controls.Add(btnVolverNuevo);
            tabNuevo.Controls.Add(cbxCategoriaNuevo);
            tabNuevo.Controls.Add(label3);
            tabNuevo.Controls.Add(txbCodigoNuevo);
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
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(651, 17);
            label10.Name = "label10";
            label10.Size = new Size(87, 25);
            label10.TabIndex = 15;
            label10.Text = "Cantidad:";
            label10.Click += label10_Click;
            // 
            // txbCantidadNuevo
            // 
            txbCantidadNuevo.Cursor = Cursors.IBeam;
            txbCantidadNuevo.Location = new Point(651, 45);
            txbCantidadNuevo.Name = "txbCantidadNuevo";
            txbCantidadNuevo.Size = new Size(532, 31);
            txbCantidadNuevo.TabIndex = 14;
            // 
            // txbPrecioVenNuevo
            // 
            txbPrecioVenNuevo.Cursor = Cursors.IBeam;
            txbPrecioVenNuevo.Location = new Point(27, 374);
            txbPrecioVenNuevo.Name = "txbPrecioVenNuevo";
            txbPrecioVenNuevo.Size = new Size(532, 31);
            txbPrecioVenNuevo.TabIndex = 13;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(30, 349);
            label9.Name = "label9";
            label9.Size = new Size(137, 25);
            label9.TabIndex = 12;
            label9.Text = "Precio de venta:";
            label9.Click += label9_Click;
            // 
            // txbPrecioCompNuevo
            // 
            txbPrecioCompNuevo.Cursor = Cursors.IBeam;
            txbPrecioCompNuevo.Location = new Point(24, 296);
            txbPrecioCompNuevo.Name = "txbPrecioCompNuevo";
            txbPrecioCompNuevo.Size = new Size(532, 31);
            txbPrecioCompNuevo.TabIndex = 11;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(27, 271);
            label8.Name = "label8";
            label8.Size = new Size(155, 25);
            label8.TabIndex = 10;
            label8.Text = "Precio de compra:";
            // 
            // txbDescripcionNuevo
            // 
            txbDescripcionNuevo.Cursor = Cursors.IBeam;
            txbDescripcionNuevo.Location = new Point(24, 209);
            txbDescripcionNuevo.Name = "txbDescripcionNuevo";
            txbDescripcionNuevo.Size = new Size(532, 31);
            txbDescripcionNuevo.TabIndex = 9;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(27, 184);
            label7.Name = "label7";
            label7.Size = new Size(108, 25);
            label7.TabIndex = 8;
            label7.Text = "Descripción:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(27, 101);
            label2.Name = "label2";
            label2.Size = new Size(75, 25);
            label2.TabIndex = 0;
            label2.Text = "Código:";
            // 
            // dgvProductoLista
            // 
            dgvProductoLista.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvProductoLista.Location = new Point(6, 65);
            dgvProductoLista.Name = "dgvProductoLista";
            dgvProductoLista.RowHeadersWidth = 62;
            dgvProductoLista.Size = new Size(1204, 438);
            dgvProductoLista.TabIndex = 7;
            dgvProductoLista.CellContentClick += dgvProductoLista_CellContentClick;
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
            // tabLista
            // 
            tabLista.Controls.Add(dgvProductoLista);
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
            // FrmProducto
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1248, 620);
            Controls.Add(label1);
            Controls.Add(tabControlMain);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmProducto";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FrmProducto";
            Load += FrmProducto_Load;
            tabEditar.ResumeLayout(false);
            tabEditar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)txbCantidadEditar).EndInit();
            tabNuevo.ResumeLayout(false);
            tabNuevo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)txbCantidadNuevo).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvProductoLista).EndInit();
            tabLista.ResumeLayout(false);
            tabLista.PerformLayout();
            tabControlMain.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Button btnGuardarEditar;
        private Button btnGuardarNuevo;
        private Button btnVolverNuevo;
        private ComboBox cbxCategoriaNuevo;
        private Label label3;
        private TabPage tabEditar;
        private TextBox txbCodigoNuevo;
        private TabPage tabNuevo;
        private Label label2;
        private DataGridView dgvProductoLista;
        private Button btnBuscar;
        private Button btnNuevoLista;
        private TextBox txbBuscar;
        private TabPage tabLista;
        private TabControl tabControlMain;
        private TextBox txbPrecioCompNuevo;
        private Label label8;
        private TextBox txbDescripcionNuevo;
        private Label label7;
        private TextBox txbPrecioVenNuevo;
        private Label label9;
        private Label label10;
        private NumericUpDown txbCantidadNuevo;
        private Label label4;
        private NumericUpDown txbCantidadEditar;
        private TextBox txbPrecioVenEditar;
        private Label label5;
        private TextBox txbPrecioCompEditar;
        private Label label6;
        private TextBox txbDescripcionEditar;
        private Label label11;
        private ComboBox cbxCategoriaEditar;
        private Label label12;
        private TextBox txbCodigoEditar;
        private Label label13;
        private Button btnVolverEditar;
        private ComboBox cbxHabilitadoEditar;
        private Label label14;
    }
}