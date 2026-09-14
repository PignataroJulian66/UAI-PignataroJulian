namespace ProyectoIS_64PR
{
    partial class FrmBitacoraCambiosVehiculo
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
            this.lblTitulo = new System.Windows.Forms.Label();
            this.dgvBitacora = new System.Windows.Forms.DataGridView();
            this.lblPatente = new System.Windows.Forms.Label();
            this.txtPatente = new System.Windows.Forms.TextBox();
            this.cbPatente = new System.Windows.Forms.CheckBox();
            this.lblMarcaModelo = new System.Windows.Forms.Label();
            this.txtMarcaModelo = new System.Windows.Forms.TextBox();
            this.cbMarcaModelo = new System.Windows.Forms.CheckBox();
            this.lblFechaInicio = new System.Windows.Forms.Label();
            this.dtpInicio = new System.Windows.Forms.DateTimePicker();
            this.cbInicio = new System.Windows.Forms.CheckBox();
            this.lblFechaFin = new System.Windows.Forms.Label();
            this.dtpFin = new System.Windows.Forms.DateTimePicker();
            this.cbFin = new System.Windows.Forms.CheckBox();
            this.btnLimpiar = new System.Windows.Forms.Button();
            this.btnAplicar = new System.Windows.Forms.Button();
            this.btnActivar = new System.Windows.Forms.Button();
            this.btnSalir = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBitacora)).BeginInit();
            this.SuspendLayout();
            //
            // lblTitulo
            //
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Location = new System.Drawing.Point(19, 11);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(220, 16);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Bitacora de cambios de vehiculo";
            //
            // dgvBitacora
            //
            this.dgvBitacora.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBitacora.Location = new System.Drawing.Point(22, 30);
            this.dgvBitacora.Name = "dgvBitacora";
            this.dgvBitacora.RowHeadersWidth = 51;
            this.dgvBitacora.RowTemplate.Height = 24;
            this.dgvBitacora.Size = new System.Drawing.Size(759, 260);
            this.dgvBitacora.TabIndex = 1;
            //
            // lblPatente
            //
            this.lblPatente.AutoSize = true;
            this.lblPatente.Location = new System.Drawing.Point(22, 301);
            this.lblPatente.Name = "lblPatente";
            this.lblPatente.Size = new System.Drawing.Size(53, 16);
            this.lblPatente.TabIndex = 2;
            this.lblPatente.Text = "Patente";
            //
            // txtPatente
            //
            this.txtPatente.Location = new System.Drawing.Point(90, 298);
            this.txtPatente.Name = "txtPatente";
            this.txtPatente.Size = new System.Drawing.Size(120, 22);
            this.txtPatente.TabIndex = 3;
            this.txtPatente.Enabled = false;
            //
            // cbPatente
            //
            this.cbPatente.AutoSize = true;
            this.cbPatente.Location = new System.Drawing.Point(216, 301);
            this.cbPatente.Name = "cbPatente";
            this.cbPatente.Size = new System.Drawing.Size(18, 17);
            this.cbPatente.TabIndex = 4;
            this.cbPatente.UseVisualStyleBackColor = true;
            this.cbPatente.CheckedChanged += new System.EventHandler(this.cbPatente_CheckedChanged);
            //
            // lblMarcaModelo
            //
            this.lblMarcaModelo.AutoSize = true;
            this.lblMarcaModelo.Location = new System.Drawing.Point(250, 301);
            this.lblMarcaModelo.Name = "lblMarcaModelo";
            this.lblMarcaModelo.Size = new System.Drawing.Size(90, 16);
            this.lblMarcaModelo.TabIndex = 5;
            this.lblMarcaModelo.Text = "Marca/Modelo";
            //
            // txtMarcaModelo
            //
            this.txtMarcaModelo.Location = new System.Drawing.Point(350, 298);
            this.txtMarcaModelo.Name = "txtMarcaModelo";
            this.txtMarcaModelo.Size = new System.Drawing.Size(140, 22);
            this.txtMarcaModelo.TabIndex = 6;
            this.txtMarcaModelo.Enabled = false;
            //
            // cbMarcaModelo
            //
            this.cbMarcaModelo.AutoSize = true;
            this.cbMarcaModelo.Location = new System.Drawing.Point(496, 301);
            this.cbMarcaModelo.Name = "cbMarcaModelo";
            this.cbMarcaModelo.Size = new System.Drawing.Size(18, 17);
            this.cbMarcaModelo.TabIndex = 7;
            this.cbMarcaModelo.UseVisualStyleBackColor = true;
            this.cbMarcaModelo.CheckedChanged += new System.EventHandler(this.cbMarcaModelo_CheckedChanged);
            //
            // lblFechaInicio
            //
            this.lblFechaInicio.AutoSize = true;
            this.lblFechaInicio.Location = new System.Drawing.Point(22, 341);
            this.lblFechaInicio.Name = "lblFechaInicio";
            this.lblFechaInicio.Size = new System.Drawing.Size(79, 16);
            this.lblFechaInicio.TabIndex = 8;
            this.lblFechaInicio.Text = "Fecha inicio";
            //
            // dtpInicio
            //
            this.dtpInicio.Enabled = false;
            this.dtpInicio.Location = new System.Drawing.Point(110, 338);
            this.dtpInicio.Name = "dtpInicio";
            this.dtpInicio.Size = new System.Drawing.Size(140, 22);
            this.dtpInicio.TabIndex = 9;
            //
            // cbInicio
            //
            this.cbInicio.AutoSize = true;
            this.cbInicio.Location = new System.Drawing.Point(256, 341);
            this.cbInicio.Name = "cbInicio";
            this.cbInicio.Size = new System.Drawing.Size(18, 17);
            this.cbInicio.TabIndex = 10;
            this.cbInicio.UseVisualStyleBackColor = true;
            this.cbInicio.CheckedChanged += new System.EventHandler(this.cbInicio_CheckedChanged);
            //
            // lblFechaFin
            //
            this.lblFechaFin.AutoSize = true;
            this.lblFechaFin.Location = new System.Drawing.Point(290, 341);
            this.lblFechaFin.Name = "lblFechaFin";
            this.lblFechaFin.Size = new System.Drawing.Size(61, 16);
            this.lblFechaFin.TabIndex = 11;
            this.lblFechaFin.Text = "Fecha fin";
            //
            // dtpFin
            //
            this.dtpFin.Enabled = false;
            this.dtpFin.Location = new System.Drawing.Point(357, 338);
            this.dtpFin.Name = "dtpFin";
            this.dtpFin.Size = new System.Drawing.Size(140, 22);
            this.dtpFin.TabIndex = 12;
            //
            // cbFin
            //
            this.cbFin.AutoSize = true;
            this.cbFin.Location = new System.Drawing.Point(503, 341);
            this.cbFin.Name = "cbFin";
            this.cbFin.Size = new System.Drawing.Size(18, 17);
            this.cbFin.TabIndex = 13;
            this.cbFin.UseVisualStyleBackColor = true;
            this.cbFin.CheckedChanged += new System.EventHandler(this.cbFin_CheckedChanged);
            //
            // btnLimpiar
            //
            this.btnLimpiar.Location = new System.Drawing.Point(706, 296);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(75, 26);
            this.btnLimpiar.TabIndex = 14;
            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.UseVisualStyleBackColor = true;
            this.btnLimpiar.Click += new System.EventHandler(this.btnLimpiar_Click);
            //
            // btnAplicar
            //
            this.btnAplicar.Location = new System.Drawing.Point(706, 328);
            this.btnAplicar.Name = "btnAplicar";
            this.btnAplicar.Size = new System.Drawing.Size(75, 26);
            this.btnAplicar.TabIndex = 15;
            this.btnAplicar.Text = "Aplicar";
            this.btnAplicar.UseVisualStyleBackColor = true;
            this.btnAplicar.Click += new System.EventHandler(this.btnAplicar_Click);
            //
            // btnActivar
            //
            this.btnActivar.Location = new System.Drawing.Point(706, 360);
            this.btnActivar.Name = "btnActivar";
            this.btnActivar.Size = new System.Drawing.Size(75, 26);
            this.btnActivar.TabIndex = 16;
            this.btnActivar.Text = "Activar";
            this.btnActivar.UseVisualStyleBackColor = true;
            this.btnActivar.Click += new System.EventHandler(this.btnActivar_Click);
            //
            // btnSalir
            //
            this.btnSalir.Location = new System.Drawing.Point(706, 392);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(75, 26);
            this.btnSalir.TabIndex = 17;
            this.btnSalir.Text = "Salir";
            this.btnSalir.UseVisualStyleBackColor = true;
            this.btnSalir.Click += new System.EventHandler(this.btnSalir_Click);
            //
            // FrmBitacoraCambiosVehiculo
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnSalir);
            this.Controls.Add(this.btnActivar);
            this.Controls.Add(this.btnAplicar);
            this.Controls.Add(this.btnLimpiar);
            this.Controls.Add(this.cbFin);
            this.Controls.Add(this.dtpFin);
            this.Controls.Add(this.lblFechaFin);
            this.Controls.Add(this.cbInicio);
            this.Controls.Add(this.dtpInicio);
            this.Controls.Add(this.lblFechaInicio);
            this.Controls.Add(this.cbMarcaModelo);
            this.Controls.Add(this.txtMarcaModelo);
            this.Controls.Add(this.lblMarcaModelo);
            this.Controls.Add(this.cbPatente);
            this.Controls.Add(this.txtPatente);
            this.Controls.Add(this.lblPatente);
            this.Controls.Add(this.dgvBitacora);
            this.Controls.Add(this.lblTitulo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmBitacoraCambiosVehiculo";
            this.Text = "FrmBitacoraCambiosVehiculo";
            this.Load += new System.EventHandler(this.FrmBitacoraCambiosVehiculo_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmBitacoraCambiosVehiculo_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBitacora)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.DataGridView dgvBitacora;
        private System.Windows.Forms.Label lblPatente;
        private System.Windows.Forms.TextBox txtPatente;
        private System.Windows.Forms.CheckBox cbPatente;
        private System.Windows.Forms.Label lblMarcaModelo;
        private System.Windows.Forms.TextBox txtMarcaModelo;
        private System.Windows.Forms.CheckBox cbMarcaModelo;
        private System.Windows.Forms.Label lblFechaInicio;
        private System.Windows.Forms.DateTimePicker dtpInicio;
        private System.Windows.Forms.CheckBox cbInicio;
        private System.Windows.Forms.Label lblFechaFin;
        private System.Windows.Forms.DateTimePicker dtpFin;
        private System.Windows.Forms.CheckBox cbFin;
        private System.Windows.Forms.Button btnLimpiar;
        private System.Windows.Forms.Button btnAplicar;
        private System.Windows.Forms.Button btnActivar;
        private System.Windows.Forms.Button btnSalir;
    }
}
