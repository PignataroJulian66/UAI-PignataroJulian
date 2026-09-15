namespace ProyectoIS_64PR
{
    partial class FrmGenerarContratoJP86
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
            this.lblFechaInicio = new System.Windows.Forms.Label();
            this.dtpFechaInicio = new System.Windows.Forms.DateTimePicker();
            this.lblFechaFin = new System.Windows.Forms.Label();
            this.dtpFechaFin = new System.Windows.Forms.DateTimePicker();
            this.lblCategoria = new System.Windows.Forms.Label();
            this.cmbCategoria = new System.Windows.Forms.ComboBox();
            this.lblMarca = new System.Windows.Forms.Label();
            this.txtMarca = new System.Windows.Forms.TextBox();
            this.lblKilometraje = new System.Windows.Forms.Label();
            this.numKilometrajeMax = new System.Windows.Forms.NumericUpDown();
            this.btnBuscarUnidades = new System.Windows.Forms.Button();
            this.dgvUnidades = new System.Windows.Forms.DataGridView();
            this.lblClientesTitulo = new System.Windows.Forms.Label();
            this.dgvClientes = new System.Windows.Forms.DataGridView();
            this.btnClienteNuevo = new System.Windows.Forms.Button();
            this.lblClienteInfo = new System.Windows.Forms.Label();
            this.pnlNuevoCliente = new System.Windows.Forms.Panel();
            this.btnGuardarCliente = new System.Windows.Forms.Button();
            this.chkCargosAdicionales = new System.Windows.Forms.CheckBox();
            this.lblImporteTotal = new System.Windows.Forms.Label();
            this.btnConfirmar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnImprimirRecibo = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numKilometrajeMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUnidades)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvClientes)).BeginInit();
            this.SuspendLayout();
            //
            // lblFechaInicio
            //
            this.lblFechaInicio.AutoSize = true;
            this.lblFechaInicio.Location = new System.Drawing.Point(12, 15);
            this.lblFechaInicio.Name = "lblFechaInicio";
            this.lblFechaInicio.Size = new System.Drawing.Size(90, 16);
            this.lblFechaInicio.TabIndex = 0;
            this.lblFechaInicio.Text = "Fecha inicio:";
            //
            // dtpFechaInicio
            //
            this.dtpFechaInicio.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFechaInicio.Location = new System.Drawing.Point(108, 12);
            this.dtpFechaInicio.Name = "dtpFechaInicio";
            this.dtpFechaInicio.Size = new System.Drawing.Size(110, 22);
            this.dtpFechaInicio.TabIndex = 1;
            //
            // lblFechaFin
            //
            this.lblFechaFin.AutoSize = true;
            this.lblFechaFin.Location = new System.Drawing.Point(230, 15);
            this.lblFechaFin.Name = "lblFechaFin";
            this.lblFechaFin.Size = new System.Drawing.Size(75, 16);
            this.lblFechaFin.TabIndex = 2;
            this.lblFechaFin.Text = "Fecha fin:";
            //
            // dtpFechaFin
            //
            this.dtpFechaFin.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFechaFin.Location = new System.Drawing.Point(313, 12);
            this.dtpFechaFin.Name = "dtpFechaFin";
            this.dtpFechaFin.Size = new System.Drawing.Size(110, 22);
            this.dtpFechaFin.TabIndex = 3;
            //
            // lblCategoria
            //
            this.lblCategoria.AutoSize = true;
            this.lblCategoria.Location = new System.Drawing.Point(12, 46);
            this.lblCategoria.Name = "lblCategoria";
            this.lblCategoria.Size = new System.Drawing.Size(70, 16);
            this.lblCategoria.TabIndex = 4;
            this.lblCategoria.Text = "Categoria:";
            //
            // cmbCategoria
            //
            this.cmbCategoria.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategoria.FormattingEnabled = true;
            this.cmbCategoria.Location = new System.Drawing.Point(108, 43);
            this.cmbCategoria.Name = "cmbCategoria";
            this.cmbCategoria.Size = new System.Drawing.Size(140, 24);
            this.cmbCategoria.TabIndex = 5;
            //
            // lblMarca
            //
            this.lblMarca.AutoSize = true;
            this.lblMarca.Location = new System.Drawing.Point(260, 46);
            this.lblMarca.Name = "lblMarca";
            this.lblMarca.Size = new System.Drawing.Size(51, 16);
            this.lblMarca.TabIndex = 6;
            this.lblMarca.Text = "Marca:";
            //
            // txtMarca
            //
            this.txtMarca.Location = new System.Drawing.Point(313, 43);
            this.txtMarca.Name = "txtMarca";
            this.txtMarca.Size = new System.Drawing.Size(110, 22);
            this.txtMarca.TabIndex = 7;
            //
            // lblKilometraje
            //
            this.lblKilometraje.AutoSize = true;
            this.lblKilometraje.Location = new System.Drawing.Point(12, 77);
            this.lblKilometraje.Name = "lblKilometraje";
            this.lblKilometraje.Size = new System.Drawing.Size(150, 16);
            this.lblKilometraje.TabIndex = 8;
            this.lblKilometraje.Text = "Km maximo (0 = sin tope):";
            //
            // numKilometrajeMax
            //
            this.numKilometrajeMax.Location = new System.Drawing.Point(168, 75);
            this.numKilometrajeMax.Maximum = new decimal(new int[] { 9999999, 0, 0, 0 });
            this.numKilometrajeMax.Name = "numKilometrajeMax";
            this.numKilometrajeMax.Size = new System.Drawing.Size(110, 22);
            this.numKilometrajeMax.TabIndex = 9;
            //
            // btnBuscarUnidades
            //
            this.btnBuscarUnidades.Location = new System.Drawing.Point(313, 74);
            this.btnBuscarUnidades.Name = "btnBuscarUnidades";
            this.btnBuscarUnidades.Size = new System.Drawing.Size(110, 25);
            this.btnBuscarUnidades.TabIndex = 10;
            this.btnBuscarUnidades.Text = "Buscar unidades";
            this.btnBuscarUnidades.UseVisualStyleBackColor = true;
            this.btnBuscarUnidades.Click += new System.EventHandler(this.btnBuscarUnidades_Click);
            //
            // dgvUnidades
            //
            this.dgvUnidades.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right))));
            this.dgvUnidades.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUnidades.Location = new System.Drawing.Point(12, 106);
            this.dgvUnidades.MultiSelect = false;
            this.dgvUnidades.Name = "dgvUnidades";
            this.dgvUnidades.ReadOnly = true;
            this.dgvUnidades.RowHeadersWidth = 51;
            this.dgvUnidades.RowTemplate.Height = 24;
            this.dgvUnidades.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUnidades.Size = new System.Drawing.Size(860, 140);
            this.dgvUnidades.TabIndex = 11;
            this.dgvUnidades.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvUnidades_CellClick);
            //
            // lblClientesTitulo
            //
            this.lblClientesTitulo.AutoSize = true;
            this.lblClientesTitulo.Location = new System.Drawing.Point(12, 256);
            this.lblClientesTitulo.Name = "lblClientesTitulo";
            this.lblClientesTitulo.Size = new System.Drawing.Size(70, 16);
            this.lblClientesTitulo.TabIndex = 12;
            this.lblClientesTitulo.Text = "Clientes:";
            //
            // dgvClientes
            //
            this.dgvClientes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvClientes.Location = new System.Drawing.Point(12, 276);
            this.dgvClientes.MultiSelect = false;
            this.dgvClientes.Name = "dgvClientes";
            this.dgvClientes.ReadOnly = true;
            this.dgvClientes.RowHeadersWidth = 51;
            this.dgvClientes.RowTemplate.Height = 24;
            this.dgvClientes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvClientes.Size = new System.Drawing.Size(530, 160);
            this.dgvClientes.TabIndex = 13;
            this.dgvClientes.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvClientes_CellClick);
            //
            // btnClienteNuevo
            //
            this.btnClienteNuevo.Location = new System.Drawing.Point(560, 276);
            this.btnClienteNuevo.Name = "btnClienteNuevo";
            this.btnClienteNuevo.Size = new System.Drawing.Size(150, 25);
            this.btnClienteNuevo.TabIndex = 14;
            this.btnClienteNuevo.Text = "Cliente nuevo";
            this.btnClienteNuevo.UseVisualStyleBackColor = true;
            this.btnClienteNuevo.Click += new System.EventHandler(this.btnClienteNuevo_Click);
            //
            // lblClienteInfo
            //
            this.lblClienteInfo.AutoSize = true;
            this.lblClienteInfo.Location = new System.Drawing.Point(560, 310);
            this.lblClienteInfo.Name = "lblClienteInfo";
            this.lblClienteInfo.Size = new System.Drawing.Size(0, 16);
            this.lblClienteInfo.TabIndex = 15;
            //
            // pnlNuevoCliente
            //
            this.pnlNuevoCliente.Location = new System.Drawing.Point(560, 335);
            this.pnlNuevoCliente.Name = "pnlNuevoCliente";
            this.pnlNuevoCliente.Size = new System.Drawing.Size(300, 170);
            this.pnlNuevoCliente.TabIndex = 16;
            this.pnlNuevoCliente.Visible = false;
            //
            // btnGuardarCliente
            //
            this.btnGuardarCliente.Location = new System.Drawing.Point(560, 511);
            this.btnGuardarCliente.Name = "btnGuardarCliente";
            this.btnGuardarCliente.Size = new System.Drawing.Size(180, 25);
            this.btnGuardarCliente.TabIndex = 17;
            this.btnGuardarCliente.Text = "Guardar cliente nuevo";
            this.btnGuardarCliente.UseVisualStyleBackColor = true;
            this.btnGuardarCliente.Visible = false;
            this.btnGuardarCliente.Click += new System.EventHandler(this.btnGuardarCliente_Click);
            //
            // chkCargosAdicionales
            //
            this.chkCargosAdicionales.AutoSize = true;
            this.chkCargosAdicionales.Location = new System.Drawing.Point(12, 448);
            this.chkCargosAdicionales.Name = "chkCargosAdicionales";
            this.chkCargosAdicionales.Size = new System.Drawing.Size(240, 20);
            this.chkCargosAdicionales.TabIndex = 18;
            this.chkCargosAdicionales.Text = "Aplicar cargos adicionales (10%)";
            this.chkCargosAdicionales.UseVisualStyleBackColor = true;
            this.chkCargosAdicionales.CheckedChanged += new System.EventHandler(this.chkCargosAdicionales_CheckedChanged);
            //
            // lblImporteTotal
            //
            this.lblImporteTotal.AutoSize = true;
            this.lblImporteTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.lblImporteTotal.Location = new System.Drawing.Point(12, 476);
            this.lblImporteTotal.Name = "lblImporteTotal";
            this.lblImporteTotal.Size = new System.Drawing.Size(140, 20);
            this.lblImporteTotal.TabIndex = 19;
            this.lblImporteTotal.Text = "Importe total: $0";
            //
            // btnConfirmar
            //
            this.btnConfirmar.Location = new System.Drawing.Point(12, 511);
            this.btnConfirmar.Name = "btnConfirmar";
            this.btnConfirmar.Size = new System.Drawing.Size(120, 28);
            this.btnConfirmar.TabIndex = 20;
            this.btnConfirmar.Text = "Confirmar";
            this.btnConfirmar.UseVisualStyleBackColor = true;
            this.btnConfirmar.Click += new System.EventHandler(this.btnConfirmar_Click);
            //
            // btnCancelar
            //
            this.btnCancelar.Location = new System.Drawing.Point(150, 511);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(120, 28);
            this.btnCancelar.TabIndex = 21;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            //
            // btnImprimirRecibo
            //
            this.btnImprimirRecibo.Enabled = false;
            this.btnImprimirRecibo.Location = new System.Drawing.Point(300, 511);
            this.btnImprimirRecibo.Name = "btnImprimirRecibo";
            this.btnImprimirRecibo.Size = new System.Drawing.Size(160, 28);
            this.btnImprimirRecibo.TabIndex = 22;
            this.btnImprimirRecibo.Text = "Imprimir Recibo";
            this.btnImprimirRecibo.UseVisualStyleBackColor = true;
            this.btnImprimirRecibo.Click += new System.EventHandler(this.btnImprimirRecibo_Click);
            //
            // FrmGenerarContratoJP86
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(900, 560);
            this.Controls.Add(this.btnImprimirRecibo);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnConfirmar);
            this.Controls.Add(this.lblImporteTotal);
            this.Controls.Add(this.chkCargosAdicionales);
            this.Controls.Add(this.btnGuardarCliente);
            this.Controls.Add(this.pnlNuevoCliente);
            this.Controls.Add(this.lblClienteInfo);
            this.Controls.Add(this.btnClienteNuevo);
            this.Controls.Add(this.dgvClientes);
            this.Controls.Add(this.lblClientesTitulo);
            this.Controls.Add(this.dgvUnidades);
            this.Controls.Add(this.btnBuscarUnidades);
            this.Controls.Add(this.numKilometrajeMax);
            this.Controls.Add(this.lblKilometraje);
            this.Controls.Add(this.txtMarca);
            this.Controls.Add(this.lblMarca);
            this.Controls.Add(this.cmbCategoria);
            this.Controls.Add(this.lblCategoria);
            this.Controls.Add(this.dtpFechaFin);
            this.Controls.Add(this.lblFechaFin);
            this.Controls.Add(this.dtpFechaInicio);
            this.Controls.Add(this.lblFechaInicio);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmGenerarContratoJP86";
            this.Text = "FrmGenerarContratoJP86";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmGenerarContratoJP86_FormClosed);
            this.Load += new System.EventHandler(this.FrmGenerarContratoJP86_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numKilometrajeMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUnidades)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvClientes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblFechaInicio;
        private System.Windows.Forms.DateTimePicker dtpFechaInicio;
        private System.Windows.Forms.Label lblFechaFin;
        private System.Windows.Forms.DateTimePicker dtpFechaFin;
        private System.Windows.Forms.Label lblCategoria;
        private System.Windows.Forms.ComboBox cmbCategoria;
        private System.Windows.Forms.Label lblMarca;
        private System.Windows.Forms.TextBox txtMarca;
        private System.Windows.Forms.Label lblKilometraje;
        private System.Windows.Forms.NumericUpDown numKilometrajeMax;
        private System.Windows.Forms.Button btnBuscarUnidades;
        private System.Windows.Forms.DataGridView dgvUnidades;
        private System.Windows.Forms.Label lblClientesTitulo;
        private System.Windows.Forms.DataGridView dgvClientes;
        private System.Windows.Forms.Button btnClienteNuevo;
        private System.Windows.Forms.Label lblClienteInfo;
        private System.Windows.Forms.Panel pnlNuevoCliente;
        private System.Windows.Forms.Button btnGuardarCliente;
        private System.Windows.Forms.CheckBox chkCargosAdicionales;
        private System.Windows.Forms.Label lblImporteTotal;
        private System.Windows.Forms.Button btnConfirmar;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnImprimirRecibo;
    }
}
