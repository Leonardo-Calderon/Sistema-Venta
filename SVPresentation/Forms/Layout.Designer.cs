namespace SVPresentation.Forms
{
    partial class Layout
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
            panel1 = new Panel();
            pictureBox1 = new PictureBox();
            label1 = new Label();
            linkCerrarSesion = new LinkLabel();
            lblRol = new Label();
            lblUsuario = new Label();
            panel2 = new Panel();
            msMenu = new MenuStrip();
            mnVentas = new ToolStripMenuItem();
            smNuevo = new ToolStripMenuItem();
            smHistorial = new ToolStripMenuItem();
            mnInventario = new ToolStripMenuItem();
            smProdcutos = new ToolStripMenuItem();
            smCategorias = new ToolStripMenuItem();
            mnReporte = new ToolStripMenuItem();
            smVentas = new ToolStripMenuItem();
            mnUsuarios = new ToolStripMenuItem();
            mnConfigruacion = new ToolStripMenuItem();
            panelMain = new Panel();
            lblMain = new Label();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panel2.SuspendLayout();
            msMenu.SuspendLayout();
            panelMain.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(44, 110, 203);
            panel1.Controls.Add(pictureBox1);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(linkCerrarSesion);
            panel1.Controls.Add(lblRol);
            panel1.Controls.Add(lblUsuario);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1446, 100);
            panel1.TabIndex = 0;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.smiley_silhouette_942453006;
            pictureBox1.Location = new Point(33, 9);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(97, 75);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 4;
            pictureBox1.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(178, 9);
            label1.Name = "label1";
            label1.Size = new Size(416, 65);
            label1.TabIndex = 3;
            label1.Text = "Sistema De Ventas";
            // 
            // linkCerrarSesion
            // 
            linkCerrarSesion.AutoSize = true;
            linkCerrarSesion.ForeColor = Color.White;
            linkCerrarSesion.LinkColor = Color.White;
            linkCerrarSesion.Location = new Point(1152, 72);
            linkCerrarSesion.Name = "linkCerrarSesion";
            linkCerrarSesion.Size = new Size(116, 25);
            linkCerrarSesion.TabIndex = 2;
            linkCerrarSesion.TabStop = true;
            linkCerrarSesion.Text = "Cerrar Sesión";
            linkCerrarSesion.LinkClicked += linkCerrarSesion_LinkClicked;
            // 
            // lblRol
            // 
            lblRol.ForeColor = Color.White;
            lblRol.Location = new Point(948, 27);
            lblRol.Name = "lblRol";
            lblRol.Size = new Size(320, 27);
            lblRol.TabIndex = 1;
            lblRol.Text = "Rol";
            lblRol.TextAlign = ContentAlignment.TopRight;
            // 
            // lblUsuario
            // 
            lblUsuario.ForeColor = Color.White;
            lblUsuario.Location = new Point(948, 0);
            lblUsuario.Name = "lblUsuario";
            lblUsuario.Size = new Size(320, 27);
            lblUsuario.TabIndex = 0;
            lblUsuario.Text = "Usuario";
            lblUsuario.TextAlign = ContentAlignment.TopRight;
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(44, 110, 203);
            panel2.Controls.Add(msMenu);
            panel2.Dock = DockStyle.Left;
            panel2.Location = new Point(0, 100);
            panel2.Name = "panel2";
            panel2.Size = new Size(178, 594);
            panel2.TabIndex = 1;
            // 
            // msMenu
            // 
            msMenu.BackColor = Color.FromArgb(44, 110, 203);
            msMenu.ImageScalingSize = new Size(24, 24);
            msMenu.Items.AddRange(new ToolStripItem[] { mnVentas, mnInventario, mnReporte, mnUsuarios, mnConfigruacion });
            msMenu.LayoutStyle = ToolStripLayoutStyle.VerticalStackWithOverflow;
            msMenu.Location = new Point(0, 0);
            msMenu.Name = "msMenu";
            msMenu.Size = new Size(178, 356);
            msMenu.TabIndex = 0;
            msMenu.Text = "menuStrip1";
            // 
            // mnVentas
            // 
            mnVentas.AutoSize = false;
            mnVentas.DropDownItems.AddRange(new ToolStripItem[] { smNuevo, smHistorial });
            mnVentas.Name = "mnVentas";
            mnVentas.Size = new Size(171, 70);
            mnVentas.Tag = "ventas";
            mnVentas.Text = "Ventas >";
            // 
            // smNuevo
            // 
            smNuevo.AutoSize = false;
            smNuevo.Name = "smNuevo";
            smNuevo.Size = new Size(270, 40);
            smNuevo.Tag = "nuevo";
            smNuevo.Text = "Nuevo";
            smNuevo.Click += smNuevo_Click;
            // 
            // smHistorial
            // 
            smHistorial.AutoSize = false;
            smHistorial.Name = "smHistorial";
            smHistorial.Size = new Size(270, 40);
            smHistorial.Tag = "historial";
            smHistorial.Text = "Historial";
            smHistorial.Click += smHistorial_Click;
            // 
            // mnInventario
            // 
            mnInventario.AutoSize = false;
            mnInventario.DropDownItems.AddRange(new ToolStripItem[] { smProdcutos, smCategorias });
            mnInventario.Name = "mnInventario";
            mnInventario.Size = new Size(171, 70);
            mnInventario.Tag = "inventario";
            mnInventario.Text = "Inventario >";
            // 
            // smProdcutos
            // 
            smProdcutos.AutoSize = false;
            smProdcutos.Name = "smProdcutos";
            smProdcutos.Size = new Size(171, 40);
            smProdcutos.Tag = "productos";
            smProdcutos.Text = "Productos";
            smProdcutos.Click += smProdcutos_Click;
            // 
            // smCategorias
            // 
            smCategorias.AutoSize = false;
            smCategorias.Name = "smCategorias";
            smCategorias.Size = new Size(171, 40);
            smCategorias.Tag = "categorias";
            smCategorias.Text = "Categorias";
            smCategorias.Click += smCategorias_Click;
            // 
            // mnReporte
            // 
            mnReporte.AutoSize = false;
            mnReporte.DropDownItems.AddRange(new ToolStripItem[] { smVentas });
            mnReporte.Name = "mnReporte";
            mnReporte.Size = new Size(171, 70);
            mnReporte.Tag = "reportes";
            mnReporte.Text = "Reporte >";
            // 
            // smVentas
            // 
            smVentas.AutoSize = false;
            smVentas.Name = "smVentas";
            smVentas.Size = new Size(270, 40);
            smVentas.Tag = "ventas";
            smVentas.Text = "Reporte de Ventas";
            smVentas.Click += smVentas_Click;
            // 
            // mnUsuarios
            // 
            mnUsuarios.AutoSize = false;
            mnUsuarios.Name = "mnUsuarios";
            mnUsuarios.Size = new Size(171, 70);
            mnUsuarios.Tag = "usuarios";
            mnUsuarios.Text = "Usuarios";
            mnUsuarios.Click += mnUsuarios_Click;
            // 
            // mnConfigruacion
            // 
            mnConfigruacion.AutoSize = false;
            mnConfigruacion.Name = "mnConfigruacion";
            mnConfigruacion.Size = new Size(171, 70);
            mnConfigruacion.Tag = "configuracion";
            mnConfigruacion.Text = "Configuración";
            mnConfigruacion.Click += mnConfigruacion_Click;
            // 
            // panelMain
            // 
            panelMain.Controls.Add(lblMain);
            panelMain.Dock = DockStyle.Fill;
            panelMain.Location = new Point(178, 100);
            panelMain.Name = "panelMain";
            panelMain.Size = new Size(1268, 594);
            panelMain.TabIndex = 2;
            // 
            // lblMain
            // 
            lblMain.Dock = DockStyle.Fill;
            lblMain.Font = new Font("Segoe UI", 24F);
            lblMain.Location = new Point(0, 0);
            lblMain.Name = "lblMain";
            lblMain.Size = new Size(1268, 594);
            lblMain.TabIndex = 5;
            lblMain.Text = "Bienvenido";
            lblMain.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Layout
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1446, 694);
            Controls.Add(panelMain);
            Controls.Add(panel2);
            Controls.Add(panel1);
            MainMenuStrip = msMenu;
            MaximizeBox = false;
            MaximumSize = new Size(1468, 750);
            MinimizeBox = false;
            MinimumSize = new Size(1468, 750);
            Name = "Layout";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Layout";
            Load += Layout_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            msMenu.ResumeLayout(false);
            msMenu.PerformLayout();
            panelMain.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel panel2;
        private Panel panelMain;
        private MenuStrip msMenu;
        private ToolStripMenuItem mnVentas;
        private ToolStripMenuItem mnInventario;
        private ToolStripMenuItem mnReporte;
        private ToolStripMenuItem mnUsuarios;
        private ToolStripMenuItem mnConfigruacion;
        private ToolStripMenuItem smNuevo;
        private ToolStripMenuItem smHistorial;
        private ToolStripMenuItem smProdcutos;
        private ToolStripMenuItem smCategorias;
        private ToolStripMenuItem smVentas;
        private Label lblRol;
        private Label lblUsuario;
        private LinkLabel linkCerrarSesion;
        private Label label1;
        private PictureBox pictureBox1;
        private Label lblMain;
    }
}