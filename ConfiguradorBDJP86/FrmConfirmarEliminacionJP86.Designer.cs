namespace ConfiguradorBDJP86
{
    partial class FrmConfirmarEliminacionJP86
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
            this.lblMotivo = new System.Windows.Forms.Label();
            this.txtMotivo = new System.Windows.Forms.TextBox();
            this.lblServidor = new System.Windows.Forms.Label();
            this.lblBaseDatos = new System.Windows.Forms.Label();
            this.lblAdvertencia = new System.Windows.Forms.Label();
            this.chkBackup = new System.Windows.Forms.CheckBox();
            this.lblConfirmar = new System.Windows.Forms.Label();
            this.txtConfirmacion = new System.Windows.Forms.TextBox();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // lblTitulo
            //
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.DarkRed;
            this.lblTitulo.Location = new System.Drawing.Point(16, 14);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(310, 21);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Se va a ELIMINAR la base de datos";
            //
            // lblMotivo
            //
            this.lblMotivo.AutoSize = true;
            this.lblMotivo.Location = new System.Drawing.Point(17, 48);
            this.lblMotivo.Name = "lblMotivo";
            this.lblMotivo.Size = new System.Drawing.Size(50, 15);
            this.lblMotivo.TabIndex = 1;
            this.lblMotivo.Text = "Motivo:";
            //
            // txtMotivo
            //
            this.txtMotivo.BackColor = System.Drawing.SystemColors.Window;
            this.txtMotivo.Location = new System.Drawing.Point(20, 66);
            this.txtMotivo.Multiline = true;
            this.txtMotivo.Name = "txtMotivo";
            this.txtMotivo.ReadOnly = true;
            this.txtMotivo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMotivo.Size = new System.Drawing.Size(500, 84);
            this.txtMotivo.TabIndex = 2;
            this.txtMotivo.TabStop = false;
            //
            // lblServidor
            //
            this.lblServidor.AutoSize = true;
            this.lblServidor.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblServidor.Location = new System.Drawing.Point(17, 162);
            this.lblServidor.Name = "lblServidor";
            this.lblServidor.Size = new System.Drawing.Size(70, 19);
            this.lblServidor.TabIndex = 3;
            this.lblServidor.Text = "Servidor:";
            //
            // lblBaseDatos
            //
            this.lblBaseDatos.AutoSize = true;
            this.lblBaseDatos.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblBaseDatos.Location = new System.Drawing.Point(17, 186);
            this.lblBaseDatos.Name = "lblBaseDatos";
            this.lblBaseDatos.Size = new System.Drawing.Size(115, 19);
            this.lblBaseDatos.TabIndex = 4;
            this.lblBaseDatos.Text = "Base de datos:";
            //
            // lblAdvertencia
            //
            this.lblAdvertencia.Location = new System.Drawing.Point(17, 214);
            this.lblAdvertencia.Name = "lblAdvertencia";
            this.lblAdvertencia.Size = new System.Drawing.Size(503, 48);
            this.lblAdvertencia.TabIndex = 5;
            this.lblAdvertencia.Text = "Se borrarán todas las tablas y todos los datos de esta base de datos.";
            //
            // chkBackup
            //
            this.chkBackup.AutoSize = true;
            this.chkBackup.Checked = true;
            this.chkBackup.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBackup.Location = new System.Drawing.Point(20, 266);
            this.chkBackup.Name = "chkBackup";
            this.chkBackup.Size = new System.Drawing.Size(298, 19);
            this.chkBackup.TabIndex = 6;
            this.chkBackup.Text = "Hacer un backup de seguridad antes de eliminarla";
            this.chkBackup.UseVisualStyleBackColor = true;
            //
            // lblConfirmar
            //
            this.lblConfirmar.AutoSize = true;
            this.lblConfirmar.Location = new System.Drawing.Point(17, 296);
            this.lblConfirmar.Name = "lblConfirmar";
            this.lblConfirmar.Size = new System.Drawing.Size(330, 15);
            this.lblConfirmar.TabIndex = 7;
            this.lblConfirmar.Text = "Para confirmar, escriba el nombre de la base de datos:";
            //
            // txtConfirmacion
            //
            this.txtConfirmacion.Location = new System.Drawing.Point(20, 316);
            this.txtConfirmacion.Name = "txtConfirmacion";
            this.txtConfirmacion.Size = new System.Drawing.Size(300, 23);
            this.txtConfirmacion.TabIndex = 0;
            this.txtConfirmacion.TextChanged += new System.EventHandler(this.txtConfirmacion_TextChanged);
            //
            // btnEliminar
            //
            this.btnEliminar.Enabled = false;
            this.btnEliminar.Location = new System.Drawing.Point(300, 356);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(130, 30);
            this.btnEliminar.TabIndex = 1;
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.UseVisualStyleBackColor = true;
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);
            //
            // btnCancelar
            //
            this.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelar.Location = new System.Drawing.Point(436, 356);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(84, 30);
            this.btnCancelar.TabIndex = 2;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            //
            // FrmConfirmarEliminacionJP86
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancelar;
            this.ClientSize = new System.Drawing.Size(540, 402);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnEliminar);
            this.Controls.Add(this.txtConfirmacion);
            this.Controls.Add(this.lblConfirmar);
            this.Controls.Add(this.chkBackup);
            this.Controls.Add(this.lblAdvertencia);
            this.Controls.Add(this.lblBaseDatos);
            this.Controls.Add(this.lblServidor);
            this.Controls.Add(this.txtMotivo);
            this.Controls.Add(this.lblMotivo);
            this.Controls.Add(this.lblTitulo);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmConfirmarEliminacionJP86";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SIGAM - Confirmar eliminación de la base de datos";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblMotivo;
        private System.Windows.Forms.TextBox txtMotivo;
        private System.Windows.Forms.Label lblServidor;
        private System.Windows.Forms.Label lblBaseDatos;
        private System.Windows.Forms.Label lblAdvertencia;
        private System.Windows.Forms.CheckBox chkBackup;
        private System.Windows.Forms.Label lblConfirmar;
        private System.Windows.Forms.TextBox txtConfirmacion;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.Button btnCancelar;
    }
}
