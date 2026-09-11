namespace ProyectoIS_64PR
{
    partial class FrmNuevoReporteJP86
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

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.dgvVehiculos = new System.Windows.Forms.DataGridView();
            this.lblDescripcion = new System.Windows.Forms.Label();
            this.txtDescripcion = new System.Windows.Forms.TextBox();
            this.lblFuente = new System.Windows.Forms.Label();
            this.cmbFuente = new System.Windows.Forms.ComboBox();
            this.btnConfirmar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnImprimirReporte = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVehiculos)).BeginInit();
            this.SuspendLayout();
            //
            // dgvVehiculos
            //
            this.dgvVehiculos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVehiculos.Location = new System.Drawing.Point(12, 12);
            this.dgvVehiculos.MultiSelect = false;
            this.dgvVehiculos.Name = "dgvVehiculos";
            this.dgvVehiculos.ReadOnly = true;
            this.dgvVehiculos.RowHeadersWidth = 51;
            this.dgvVehiculos.RowTemplate.Height = 24;
            this.dgvVehiculos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvVehiculos.Size = new System.Drawing.Size(860, 180);
            this.dgvVehiculos.TabIndex = 0;
            this.dgvVehiculos.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvVehiculos_CellClick);
            //
            // lblDescripcion
            //
            this.lblDescripcion.AutoSize = true;
            this.lblDescripcion.Location = new System.Drawing.Point(12, 204);
            this.lblDescripcion.Name = "lblDescripcion";
            this.lblDescripcion.Size = new System.Drawing.Size(180, 16);
            this.lblDescripcion.TabIndex = 1;
            this.lblDescripcion.Text = "Descripcion del problema:";
            //
            // txtDescripcion
            //
            this.txtDescripcion.Location = new System.Drawing.Point(12, 224);
            this.txtDescripcion.Multiline = true;
            this.txtDescripcion.Name = "txtDescripcion";
            this.txtDescripcion.Size = new System.Drawing.Size(860, 80);
            this.txtDescripcion.TabIndex = 2;
            //
            // lblFuente
            //
            this.lblFuente.AutoSize = true;
            this.lblFuente.Location = new System.Drawing.Point(12, 316);
            this.lblFuente.Name = "lblFuente";
            this.lblFuente.Size = new System.Drawing.Size(55, 16);
            this.lblFuente.TabIndex = 3;
            this.lblFuente.Text = "Fuente:";
            //
            // cmbFuente
            //
            this.cmbFuente.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFuente.FormattingEnabled = true;
            this.cmbFuente.Location = new System.Drawing.Point(90, 313);
            this.cmbFuente.Name = "cmbFuente";
            this.cmbFuente.Size = new System.Drawing.Size(200, 24);
            this.cmbFuente.TabIndex = 4;
            //
            // btnConfirmar
            //
            this.btnConfirmar.Location = new System.Drawing.Point(12, 350);
            this.btnConfirmar.Name = "btnConfirmar";
            this.btnConfirmar.Size = new System.Drawing.Size(120, 28);
            this.btnConfirmar.TabIndex = 5;
            this.btnConfirmar.Text = "Confirmar";
            this.btnConfirmar.UseVisualStyleBackColor = true;
            this.btnConfirmar.Click += new System.EventHandler(this.btnConfirmar_Click);
            //
            // btnCancelar
            //
            this.btnCancelar.Location = new System.Drawing.Point(150, 350);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(120, 28);
            this.btnCancelar.TabIndex = 6;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            //
            // btnImprimirReporte
            //
            this.btnImprimirReporte.Enabled = false;
            this.btnImprimirReporte.Location = new System.Drawing.Point(288, 350);
            this.btnImprimirReporte.Name = "btnImprimirReporte";
            this.btnImprimirReporte.Size = new System.Drawing.Size(160, 28);
            this.btnImprimirReporte.TabIndex = 7;
            this.btnImprimirReporte.Text = "Imprimir Reporte";
            this.btnImprimirReporte.UseVisualStyleBackColor = true;
            this.btnImprimirReporte.Click += new System.EventHandler(this.btnImprimirReporte_Click);
            //
            // FrmNuevoReporteJP86
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 400);
            this.Controls.Add(this.btnImprimirReporte);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnConfirmar);
            this.Controls.Add(this.cmbFuente);
            this.Controls.Add(this.lblFuente);
            this.Controls.Add(this.txtDescripcion);
            this.Controls.Add(this.lblDescripcion);
            this.Controls.Add(this.dgvVehiculos);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmNuevoReporteJP86";
            this.Text = "FrmNuevoReporteJP86";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmNuevoReporteJP86_FormClosed);
            this.Load += new System.EventHandler(this.FrmNuevoReporteJP86_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvVehiculos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvVehiculos;
        private System.Windows.Forms.Label lblDescripcion;
        private System.Windows.Forms.TextBox txtDescripcion;
        private System.Windows.Forms.Label lblFuente;
        private System.Windows.Forms.ComboBox cmbFuente;
        private System.Windows.Forms.Button btnConfirmar;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnImprimirReporte;
    }
}
