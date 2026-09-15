namespace ProyectoIS_64PR
{
    partial class FrmMenu
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
            this.pnlMain = new System.Windows.Forms.Panel();
            this.pnlContenidoMenu = new System.Windows.Forms.Panel();
            this.pnlTopbar = new System.Windows.Forms.Panel();
            this.lblCrumb = new System.Windows.Forms.Label();
            this.lblIdiomaPill = new System.Windows.Forms.Label();
            this.pnlSidebar = new System.Windows.Forms.Panel();
            this.pnlSidebarNav = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlSidebarHeader = new System.Windows.Forms.Panel();
            this.pnlIconoMarca = new System.Windows.Forms.Label();
            this.lblLogo = new System.Windows.Forms.Label();
            this.lblSubtitulo = new System.Windows.Forms.Label();
            this.pnlSidebarFooter = new System.Windows.Forms.Panel();
            this.lblAvatar = new System.Windows.Forms.Label();
            this.lblUsuarioActual = new System.Windows.Forms.Label();
            this.lblRolActual = new System.Windows.Forms.Label();
            this.btnCerrarSesionFooter = new System.Windows.Forms.Button();
            this.pnlMain.SuspendLayout();
            this.pnlTopbar.SuspendLayout();
            this.pnlSidebar.SuspendLayout();
            this.pnlSidebarHeader.SuspendLayout();
            this.pnlSidebarFooter.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlContenidoMenu
            //
            this.pnlContenidoMenu.AutoScroll = true;
            this.pnlContenidoMenu.BackColor = ProyectoIS_64PR.UI.TemaVisual.FondoPagina;
            this.pnlContenidoMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContenidoMenu.Location = new System.Drawing.Point(0, 58);
            this.pnlContenidoMenu.Margin = new System.Windows.Forms.Padding(2);
            this.pnlContenidoMenu.Name = "pnlContenidoMenu";
            this.pnlContenidoMenu.Size = new System.Drawing.Size(766, 399);
            this.pnlContenidoMenu.TabIndex = 0;
            //
            // lblCrumb
            //
            this.lblCrumb.AutoSize = true;
            this.lblCrumb.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblCrumb.ForeColor = ProyectoIS_64PR.UI.TemaVisual.TextoSecundario;
            this.lblCrumb.Location = new System.Drawing.Point(26, 20);
            this.lblCrumb.Name = "lblCrumb";
            this.lblCrumb.Size = new System.Drawing.Size(45, 17);
            this.lblCrumb.TabIndex = 0;
            this.lblCrumb.Text = "Panel";
            //
            // lblIdiomaPill
            //
            this.lblIdiomaPill.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblIdiomaPill.AutoSize = true;
            this.lblIdiomaPill.BackColor = ProyectoIS_64PR.UI.TemaVisual.FondoPagina;
            this.lblIdiomaPill.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblIdiomaPill.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblIdiomaPill.ForeColor = ProyectoIS_64PR.UI.TemaVisual.TextoSecundario;
            this.lblIdiomaPill.Location = new System.Drawing.Point(700, 18);
            this.lblIdiomaPill.Name = "lblIdiomaPill";
            this.lblIdiomaPill.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.lblIdiomaPill.Size = new System.Drawing.Size(40, 21);
            this.lblIdiomaPill.TabIndex = 1;
            this.lblIdiomaPill.Text = "ES";
            //
            // pnlTopbar
            //
            this.pnlTopbar.BackColor = ProyectoIS_64PR.UI.TemaVisual.FondoTarjeta;
            this.pnlTopbar.Controls.Add(this.lblCrumb);
            this.pnlTopbar.Controls.Add(this.lblIdiomaPill);
            this.pnlTopbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTopbar.Location = new System.Drawing.Point(0, 0);
            this.pnlTopbar.Name = "pnlTopbar";
            this.pnlTopbar.Size = new System.Drawing.Size(766, 58);
            this.pnlTopbar.TabIndex = 1;
            this.pnlTopbar.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlTopbar_Paint);
            //
            // pnlMain
            //
            this.pnlMain.Controls.Add(this.pnlContenidoMenu);
            this.pnlMain.Controls.Add(this.pnlTopbar);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(264, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(766, 457);
            this.pnlMain.TabIndex = 0;
            //
            // pnlIconoMarca
            //
            this.pnlIconoMarca.BackColor = ProyectoIS_64PR.UI.TemaVisual.AzulPrimario;
            this.pnlIconoMarca.Font = new System.Drawing.Font("Segoe UI", 15F);
            this.pnlIconoMarca.ForeColor = System.Drawing.Color.White;
            this.pnlIconoMarca.Location = new System.Drawing.Point(22, 20);
            this.pnlIconoMarca.Name = "pnlIconoMarca";
            this.pnlIconoMarca.Size = new System.Drawing.Size(34, 34);
            this.pnlIconoMarca.TabIndex = 0;
            this.pnlIconoMarca.Text = "🚗";
            this.pnlIconoMarca.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // lblLogo
            //
            this.lblLogo.AutoSize = true;
            this.lblLogo.Font = new System.Drawing.Font("Segoe UI", 12.5F, System.Drawing.FontStyle.Bold);
            this.lblLogo.ForeColor = System.Drawing.Color.White;
            this.lblLogo.Location = new System.Drawing.Point(66, 19);
            this.lblLogo.Name = "lblLogo";
            this.lblLogo.Size = new System.Drawing.Size(70, 21);
            this.lblLogo.TabIndex = 1;
            this.lblLogo.Text = "SIGAM";
            //
            // lblSubtitulo
            //
            this.lblSubtitulo.AutoSize = true;
            this.lblSubtitulo.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblSubtitulo.ForeColor = System.Drawing.Color.FromArgb(180, 195, 215);
            this.lblSubtitulo.Location = new System.Drawing.Point(67, 41);
            this.lblSubtitulo.MaximumSize = new System.Drawing.Size(180, 0);
            this.lblSubtitulo.Name = "lblSubtitulo";
            this.lblSubtitulo.Size = new System.Drawing.Size(112, 13);
            this.lblSubtitulo.TabIndex = 2;
            this.lblSubtitulo.Text = "Gestión de Alquileres";
            //
            // pnlSidebarHeader
            //
            this.pnlSidebarHeader.BackColor = ProyectoIS_64PR.UI.TemaVisual.AzulPrimario;
            this.pnlSidebarHeader.Controls.Add(this.pnlIconoMarca);
            this.pnlSidebarHeader.Controls.Add(this.lblLogo);
            this.pnlSidebarHeader.Controls.Add(this.lblSubtitulo);
            this.pnlSidebarHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSidebarHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlSidebarHeader.Name = "pnlSidebarHeader";
            this.pnlSidebarHeader.Size = new System.Drawing.Size(264, 74);
            this.pnlSidebarHeader.TabIndex = 0;
            this.pnlSidebarHeader.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlSidebarHeader_Paint);
            //
            // pnlSidebarNav
            //
            this.pnlSidebarNav.AutoScroll = true;
            this.pnlSidebarNav.BackColor = ProyectoIS_64PR.UI.TemaVisual.AzulPrimario;
            ///NO uso Dock=Fill aca a proposito: con Header(Top)+Footer(Bottom)+Nav(Fill) los
            ///3 juntos, el orden de Controls.Add para resolver el Fill es ambiguo y ya causo
            ///overlaps. Uso limites explicitos + Anchor en las 4 puntas, sin depender de Dock.
            this.pnlSidebarNav.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlSidebarNav.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.pnlSidebarNav.Location = new System.Drawing.Point(0, 74);
            this.pnlSidebarNav.Name = "pnlSidebarNav";
            this.pnlSidebarNav.Padding = new System.Windows.Forms.Padding(10, 10, 10, 10);
            this.pnlSidebarNav.Size = new System.Drawing.Size(264, 315);
            this.pnlSidebarNav.TabIndex = 1;
            this.pnlSidebarNav.WrapContents = false;
            //
            // lblAvatar
            //
            this.lblAvatar.BackColor = ProyectoIS_64PR.UI.TemaVisual.Info;
            this.lblAvatar.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblAvatar.ForeColor = System.Drawing.Color.White;
            this.lblAvatar.Location = new System.Drawing.Point(16, 15);
            this.lblAvatar.Name = "lblAvatar";
            this.lblAvatar.Size = new System.Drawing.Size(34, 34);
            this.lblAvatar.TabIndex = 0;
            this.lblAvatar.Text = "?";
            this.lblAvatar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // lblUsuarioActual
            //
            this.lblUsuarioActual.AutoSize = true;
            this.lblUsuarioActual.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblUsuarioActual.ForeColor = System.Drawing.Color.White;
            this.lblUsuarioActual.Location = new System.Drawing.Point(58, 13);
            this.lblUsuarioActual.Name = "lblUsuarioActual";
            this.lblUsuarioActual.Size = new System.Drawing.Size(0, 15);
            this.lblUsuarioActual.TabIndex = 1;
            //
            // lblRolActual
            //
            this.lblRolActual.AutoSize = true;
            this.lblRolActual.Font = new System.Drawing.Font("Segoe UI", 7.75F);
            this.lblRolActual.ForeColor = System.Drawing.Color.FromArgb(180, 195, 215);
            this.lblRolActual.Location = new System.Drawing.Point(58, 30);
            this.lblRolActual.Name = "lblRolActual";
            this.lblRolActual.Size = new System.Drawing.Size(0, 13);
            this.lblRolActual.TabIndex = 2;
            //
            // btnCerrarSesionFooter
            //
            this.btnCerrarSesionFooter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCerrarSesionFooter.BackColor = ProyectoIS_64PR.UI.TemaVisual.AzulOscuro;
            this.btnCerrarSesionFooter.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCerrarSesionFooter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCerrarSesionFooter.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnCerrarSesionFooter.ForeColor = System.Drawing.Color.FromArgb(200, 210, 225);
            this.btnCerrarSesionFooter.Location = new System.Drawing.Point(218, 17);
            this.btnCerrarSesionFooter.Name = "btnCerrarSesionFooter";
            this.btnCerrarSesionFooter.Size = new System.Drawing.Size(30, 30);
            this.btnCerrarSesionFooter.TabIndex = 3;
            this.btnCerrarSesionFooter.Text = "⏻";
            this.btnCerrarSesionFooter.UseVisualStyleBackColor = false;
            //
            // pnlSidebarFooter
            //
            this.pnlSidebarFooter.BackColor = ProyectoIS_64PR.UI.TemaVisual.AzulOscuro;
            this.pnlSidebarFooter.Controls.Add(this.lblAvatar);
            this.pnlSidebarFooter.Controls.Add(this.lblUsuarioActual);
            this.pnlSidebarFooter.Controls.Add(this.lblRolActual);
            this.pnlSidebarFooter.Controls.Add(this.btnCerrarSesionFooter);
            this.pnlSidebarFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlSidebarFooter.Location = new System.Drawing.Point(0, 389);
            this.pnlSidebarFooter.Name = "pnlSidebarFooter";
            this.pnlSidebarFooter.Size = new System.Drawing.Size(264, 68);
            this.pnlSidebarFooter.TabIndex = 2;
            this.pnlSidebarFooter.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlSidebarFooter_Paint);
            //
            // pnlSidebar
            //
            this.pnlSidebar.BackColor = ProyectoIS_64PR.UI.TemaVisual.AzulPrimario;
            this.pnlSidebar.Location = new System.Drawing.Point(0, 0);
            this.pnlSidebar.Name = "pnlSidebar";
            ///Ancho fijo (264px, tomado del mockup) seteado ANTES de Dock; Nav (Fill) se
            ///agrega primero, Header/Footer despues para que reclamen su franja arriba/abajo.
            this.pnlSidebar.Size = new System.Drawing.Size(264, 457);
            this.pnlSidebar.Controls.Add(this.pnlSidebarNav);
            this.pnlSidebar.Controls.Add(this.pnlSidebarHeader);
            this.pnlSidebar.Controls.Add(this.pnlSidebarFooter);
            this.pnlSidebarHeader.BringToFront();
            this.pnlSidebarFooter.BringToFront();
            this.pnlSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSidebar.TabIndex = 1;
            //
            // FrmMenu
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = ProyectoIS_64PR.UI.TemaVisual.FondoPagina;
            this.ClientSize = new System.Drawing.Size(1030, 457);
            ///pnlMain.Dock queda declarado Fill acá pero se pisa a None en el constructor de
            ///FrmMenu (AjustarPanelPrincipal se encarga de su Bounds a mano) -- el Dock=Fill
            ///automático no excluía el ancho de pnlSidebar. Ver comentario en FrmMenu.cs.
            this.Controls.Add(this.pnlSidebar);
            this.Controls.Add(this.pnlMain);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FrmMenu";
            this.Text = "SIGAM";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmMenu_FormClosed);
            this.Load += new System.EventHandler(this.FrmMenu_Load);
            this.pnlMain.ResumeLayout(false);
            this.pnlTopbar.ResumeLayout(false);
            this.pnlTopbar.PerformLayout();
            this.pnlSidebar.ResumeLayout(false);
            this.pnlSidebarHeader.ResumeLayout(false);
            this.pnlSidebarHeader.PerformLayout();
            this.pnlSidebarFooter.ResumeLayout(false);
            this.pnlSidebarFooter.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Panel pnlContenidoMenu;
        private System.Windows.Forms.Panel pnlTopbar;
        private System.Windows.Forms.Label lblCrumb;
        private System.Windows.Forms.Label lblIdiomaPill;
        private System.Windows.Forms.Panel pnlSidebar;
        private System.Windows.Forms.Panel pnlSidebarHeader;
        private System.Windows.Forms.Label pnlIconoMarca;
        private System.Windows.Forms.Label lblLogo;
        private System.Windows.Forms.Label lblSubtitulo;
        private System.Windows.Forms.FlowLayoutPanel pnlSidebarNav;
        private System.Windows.Forms.Panel pnlSidebarFooter;
        private System.Windows.Forms.Label lblAvatar;
        private System.Windows.Forms.Label lblUsuarioActual;
        private System.Windows.Forms.Label lblRolActual;
        private System.Windows.Forms.Button btnCerrarSesionFooter;
    }
}
