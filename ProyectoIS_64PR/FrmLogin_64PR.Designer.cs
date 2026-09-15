namespace ProyectoIS_64PR
{
    partial class FrmLogin_64PR
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
            this.txtLogin = new System.Windows.Forms.TextBox();
            this.btnIniciarSesion = new System.Windows.Forms.Button();
            this.txtContra = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblMensaje = new System.Windows.Forms.Label();
            this.cmbIdioma = new System.Windows.Forms.ComboBox();
            this.pnlMarca = new System.Windows.Forms.Panel();
            this.lblLogoLogin = new System.Windows.Forms.Label();
            this.lblSubtituloLogin = new System.Windows.Forms.Label();
            this.pnlMarca.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlMarca
            //
            this.pnlMarca.BackColor = ProyectoIS_64PR.UI.TemaVisual.AzulPrimario;
            this.pnlMarca.Controls.Add(this.lblLogoLogin);
            this.pnlMarca.Controls.Add(this.lblSubtituloLogin);
            this.pnlMarca.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlMarca.Location = new System.Drawing.Point(0, 0);
            this.pnlMarca.Name = "pnlMarca";
            this.pnlMarca.Size = new System.Drawing.Size(350, 450);
            this.pnlMarca.TabIndex = 10;
            //
            // lblLogoLogin
            //
            this.lblLogoLogin.AutoSize = true;
            this.lblLogoLogin.Font = new System.Drawing.Font("Segoe UI", 28F, System.Drawing.FontStyle.Bold);
            this.lblLogoLogin.ForeColor = System.Drawing.Color.White;
            this.lblLogoLogin.Location = new System.Drawing.Point(75, 175);
            this.lblLogoLogin.Name = "lblLogoLogin";
            this.lblLogoLogin.Size = new System.Drawing.Size(200, 53);
            this.lblLogoLogin.TabIndex = 0;
            this.lblLogoLogin.Text = "SIGAM";
            //
            // lblSubtituloLogin
            //
            this.lblSubtituloLogin.AutoSize = true;
            this.lblSubtituloLogin.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSubtituloLogin.ForeColor = System.Drawing.Color.FromArgb(200, 210, 225);
            this.lblSubtituloLogin.Location = new System.Drawing.Point(78, 230);
            this.lblSubtituloLogin.MaximumSize = new System.Drawing.Size(220, 0);
            this.lblSubtituloLogin.Name = "lblSubtituloLogin";
            this.lblSubtituloLogin.Size = new System.Drawing.Size(180, 15);
            this.lblSubtituloLogin.TabIndex = 1;
            this.lblSubtituloLogin.Text = "Sistema de Gestión de Alquileres";
            //
            // txtLogin
            //
            this.txtLogin.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtLogin.Location = new System.Drawing.Point(400, 148);
            this.txtLogin.Name = "txtLogin";
            this.txtLogin.Size = new System.Drawing.Size(280, 25);
            this.txtLogin.TabIndex = 0;
            //
            // btnIniciarSesion
            //
            this.btnIniciarSesion.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnIniciarSesion.Location = new System.Drawing.Point(400, 262);
            this.btnIniciarSesion.Name = "btnIniciarSesion";
            this.btnIniciarSesion.Size = new System.Drawing.Size(280, 38);
            this.btnIniciarSesion.TabIndex = 2;
            this.btnIniciarSesion.Text = "Iniciar sesion";
            this.btnIniciarSesion.UseVisualStyleBackColor = true;
            this.btnIniciarSesion.Click += new System.EventHandler(this.btnIniciarSesion_Click);
            //
            // txtContra
            //
            this.txtContra.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtContra.Location = new System.Drawing.Point(400, 207);
            this.txtContra.Name = "txtContra";
            this.txtContra.Size = new System.Drawing.Size(280, 25);
            this.txtContra.TabIndex = 1;
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label1.ForeColor = ProyectoIS_64PR.UI.TemaVisual.TextoSecundario;
            this.label1.Location = new System.Drawing.Point(400, 128);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "Nombre de usuario";
            //
            // label2
            //
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label2.ForeColor = ProyectoIS_64PR.UI.TemaVisual.TextoSecundario;
            this.label2.Location = new System.Drawing.Point(400, 187);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 16);
            this.label2.TabIndex = 4;
            this.label2.Text = "Contraseña";
            //
            // lblMensaje
            //
            this.lblMensaje.AutoSize = true;
            this.lblMensaje.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblMensaje.ForeColor = ProyectoIS_64PR.UI.TemaVisual.Peligro;
            this.lblMensaje.Location = new System.Drawing.Point(400, 310);
            this.lblMensaje.Name = "lblMensaje";
            this.lblMensaje.Size = new System.Drawing.Size(0, 16);
            this.lblMensaje.TabIndex = 5;
            //
            // cmbIdioma
            //
            this.cmbIdioma.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbIdioma.FormattingEnabled = true;
            this.cmbIdioma.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbIdioma.Location = new System.Drawing.Point(400, 345);
            this.cmbIdioma.Name = "cmbIdioma";
            this.cmbIdioma.Size = new System.Drawing.Size(100, 25);
            this.cmbIdioma.TabIndex = 6;
            this.cmbIdioma.SelectedIndexChanged += new System.EventHandler(this.cmbIdioma_SelectedIndexChanged_1);
            //
            // FrmLogin_64PR
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = ProyectoIS_64PR.UI.TemaVisual.FondoTarjeta;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pnlMarca);
            this.Controls.Add(this.cmbIdioma);
            this.Controls.Add(this.lblMensaje);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtContra);
            this.Controls.Add(this.btnIniciarSesion);
            this.Controls.Add(this.txtLogin);
            this.Name = "FrmLogin_64PR";
            this.Text = "FrmLogin_64PR";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmLogin_64PR_FormClosed);
            this.pnlMarca.ResumeLayout(false);
            this.pnlMarca.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtLogin;
        private System.Windows.Forms.Button btnIniciarSesion;
        private System.Windows.Forms.TextBox txtContra;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblMensaje;
        private System.Windows.Forms.ComboBox cmbIdioma;
        private System.Windows.Forms.Panel pnlMarca;
        private System.Windows.Forms.Label lblLogoLogin;
        private System.Windows.Forms.Label lblSubtituloLogin;
    }
}