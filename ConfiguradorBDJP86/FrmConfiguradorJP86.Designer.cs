namespace ConfiguradorBDJP86
{
    partial class FrmConfiguradorJP86
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        private void InitializeComponent()
        {
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblModo = new System.Windows.Forms.Label();
            this.grpConexion = new System.Windows.Forms.GroupBox();
            this.lblEstadoConexion = new System.Windows.Forms.Label();
            this.btnProbar = new System.Windows.Forms.Button();
            this.lblAyudaBD = new System.Windows.Forms.Label();
            this.txtBD = new System.Windows.Forms.TextBox();
            this.lblBD = new System.Windows.Forms.Label();
            this.lblAyudaServidor = new System.Windows.Forms.Label();
            this.cmbServidor = new System.Windows.Forms.ComboBox();
            this.lblServidor = new System.Windows.Forms.Label();
            this.lblProgreso = new System.Windows.Forms.Label();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.barProgreso = new System.Windows.Forms.ProgressBar();
            this.lblRutaLog = new System.Windows.Forms.Label();
            this.btnContinuar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.grpConexion.SuspendLayout();
            this.SuspendLayout();
            //
            // lblTitulo
            //
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(56)))), ((int)(((byte)(100)))));
            this.lblTitulo.Location = new System.Drawing.Point(14, 12);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(330, 25);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "SIGAM - Configuración de la base de datos";
            //
            // lblModo
            //
            this.lblModo.AutoSize = true;
            this.lblModo.Location = new System.Drawing.Point(16, 44);
            this.lblModo.Name = "lblModo";
            this.lblModo.Size = new System.Drawing.Size(42, 15);
            this.lblModo.TabIndex = 1;
            this.lblModo.Text = "Modo:";
            //
            // grpConexion
            //
            this.grpConexion.Controls.Add(this.lblEstadoConexion);
            this.grpConexion.Controls.Add(this.btnProbar);
            this.grpConexion.Controls.Add(this.lblAyudaBD);
            this.grpConexion.Controls.Add(this.txtBD);
            this.grpConexion.Controls.Add(this.lblBD);
            this.grpConexion.Controls.Add(this.lblAyudaServidor);
            this.grpConexion.Controls.Add(this.cmbServidor);
            this.grpConexion.Controls.Add(this.lblServidor);
            this.grpConexion.Location = new System.Drawing.Point(16, 70);
            this.grpConexion.Name = "grpConexion";
            this.grpConexion.Size = new System.Drawing.Size(606, 152);
            this.grpConexion.TabIndex = 2;
            this.grpConexion.TabStop = false;
            this.grpConexion.Text = "Conexión con SQL Server";
            //
            // lblEstadoConexion
            //
            this.lblEstadoConexion.Location = new System.Drawing.Point(14, 124);
            this.lblEstadoConexion.Name = "lblEstadoConexion";
            this.lblEstadoConexion.Size = new System.Drawing.Size(578, 18);
            this.lblEstadoConexion.TabIndex = 7;
            //
            // btnProbar
            //
            this.btnProbar.Location = new System.Drawing.Point(460, 24);
            this.btnProbar.Name = "btnProbar";
            this.btnProbar.Size = new System.Drawing.Size(132, 28);
            this.btnProbar.TabIndex = 2;
            this.btnProbar.Text = "Probar conexión";
            this.btnProbar.UseVisualStyleBackColor = true;
            this.btnProbar.Click += new System.EventHandler(this.btnProbar_Click);
            //
            // lblAyudaBD
            //
            this.lblAyudaBD.AutoSize = true;
            this.lblAyudaBD.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblAyudaBD.Location = new System.Drawing.Point(180, 102);
            this.lblAyudaBD.Name = "lblAyudaBD";
            this.lblAyudaBD.Size = new System.Drawing.Size(285, 15);
            this.lblAyudaBD.TabIndex = 6;
            this.lblAyudaBD.Text = "Letras, números y guion bajo. Debe empezar con letra.";
            //
            // txtBD
            //
            this.txtBD.Location = new System.Drawing.Point(180, 77);
            this.txtBD.MaxLength = 100;
            this.txtBD.Name = "txtBD";
            this.txtBD.Size = new System.Drawing.Size(262, 23);
            this.txtBD.TabIndex = 1;
            this.txtBD.TextChanged += new System.EventHandler(this.DatosConexion_Changed);
            //
            // lblBD
            //
            this.lblBD.AutoSize = true;
            this.lblBD.Location = new System.Drawing.Point(14, 80);
            this.lblBD.Name = "lblBD";
            this.lblBD.Size = new System.Drawing.Size(157, 15);
            this.lblBD.TabIndex = 5;
            this.lblBD.Text = "Nombre de la base de datos:";
            //
            // lblAyudaServidor
            //
            this.lblAyudaServidor.AutoSize = true;
            this.lblAyudaServidor.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblAyudaServidor.Location = new System.Drawing.Point(180, 51);
            this.lblAyudaServidor.Name = "lblAyudaServidor";
            this.lblAyudaServidor.Size = new System.Drawing.Size(232, 15);
            this.lblAyudaServidor.TabIndex = 4;
            this.lblAyudaServidor.Text = "Ejemplos: .\\SQLEXPRESS, localhost, (local)";
            //
            // cmbServidor
            //
            this.cmbServidor.FormattingEnabled = true;
            this.cmbServidor.Location = new System.Drawing.Point(180, 26);
            this.cmbServidor.Name = "cmbServidor";
            this.cmbServidor.Size = new System.Drawing.Size(262, 23);
            this.cmbServidor.TabIndex = 0;
            this.cmbServidor.TextChanged += new System.EventHandler(this.DatosConexion_Changed);
            //
            // lblServidor
            //
            this.lblServidor.AutoSize = true;
            this.lblServidor.Location = new System.Drawing.Point(14, 29);
            this.lblServidor.Name = "lblServidor";
            this.lblServidor.Size = new System.Drawing.Size(130, 15);
            this.lblServidor.TabIndex = 3;
            this.lblServidor.Text = "Servidor de SQL Server:";
            //
            // lblProgreso
            //
            this.lblProgreso.AutoSize = true;
            this.lblProgreso.Location = new System.Drawing.Point(16, 232);
            this.lblProgreso.Name = "lblProgreso";
            this.lblProgreso.Size = new System.Drawing.Size(165, 15);
            this.lblProgreso.TabIndex = 3;
            this.lblProgreso.Text = "Progreso de la configuración:";
            //
            // txtLog
            //
            this.txtLog.BackColor = System.Drawing.SystemColors.Window;
            this.txtLog.Font = new System.Drawing.Font("Consolas", 8.25F);
            this.txtLog.Location = new System.Drawing.Point(16, 252);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLog.Size = new System.Drawing.Size(606, 236);
            this.txtLog.TabIndex = 4;
            this.txtLog.TabStop = false;
            this.txtLog.WordWrap = false;
            //
            // barProgreso
            //
            this.barProgreso.Location = new System.Drawing.Point(16, 496);
            this.barProgreso.Name = "barProgreso";
            this.barProgreso.Size = new System.Drawing.Size(606, 16);
            this.barProgreso.TabIndex = 5;
            //
            // lblRutaLog
            //
            this.lblRutaLog.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblRutaLog.Location = new System.Drawing.Point(16, 518);
            this.lblRutaLog.Name = "lblRutaLog";
            this.lblRutaLog.Size = new System.Drawing.Size(606, 18);
            this.lblRutaLog.TabIndex = 6;
            this.lblRutaLog.Text = "Log:";
            //
            // btnContinuar
            //
            this.btnContinuar.Location = new System.Drawing.Point(406, 546);
            this.btnContinuar.Name = "btnContinuar";
            this.btnContinuar.Size = new System.Drawing.Size(120, 32);
            this.btnContinuar.TabIndex = 0;
            this.btnContinuar.Text = "Continuar";
            this.btnContinuar.UseVisualStyleBackColor = true;
            this.btnContinuar.Click += new System.EventHandler(this.btnContinuar_Click);
            //
            // btnCancelar
            //
            this.btnCancelar.Location = new System.Drawing.Point(532, 546);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(90, 32);
            this.btnCancelar.TabIndex = 1;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            //
            // FrmConfiguradorJP86
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(638, 592);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnContinuar);
            this.Controls.Add(this.lblRutaLog);
            this.Controls.Add(this.barProgreso);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.lblProgreso);
            this.Controls.Add(this.grpConexion);
            this.Controls.Add(this.lblModo);
            this.Controls.Add(this.lblTitulo);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FrmConfiguradorJP86";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SIGAM - Configurar base de datos";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmConfiguradorJP86_FormClosing);
            this.Load += new System.EventHandler(this.FrmConfiguradorJP86_Load);
            this.grpConexion.ResumeLayout(false);
            this.grpConexion.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblModo;
        private System.Windows.Forms.GroupBox grpConexion;
        private System.Windows.Forms.Label lblServidor;
        private System.Windows.Forms.ComboBox cmbServidor;
        private System.Windows.Forms.Label lblAyudaServidor;
        private System.Windows.Forms.Label lblBD;
        private System.Windows.Forms.TextBox txtBD;
        private System.Windows.Forms.Label lblAyudaBD;
        private System.Windows.Forms.Button btnProbar;
        private System.Windows.Forms.Label lblEstadoConexion;
        private System.Windows.Forms.Label lblProgreso;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.ProgressBar barProgreso;
        private System.Windows.Forms.Label lblRutaLog;
        private System.Windows.Forms.Button btnContinuar;
        private System.Windows.Forms.Button btnCancelar;
    }
}
