namespace ProyectoIS_64PR
{
    partial class FrmReporteMantenimientoJP86
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
            this.pnlResumen = new System.Windows.Forms.Panel();
            this.lblResumenTitulo = new System.Windows.Forms.Label();
            this.lblResumenSemaforos = new System.Windows.Forms.Label();
            this.lblResumenCategoria = new System.Windows.Forms.Label();
            this.lblCategoria = new System.Windows.Forms.Label();
            this.cmbCategoria = new System.Windows.Forms.ComboBox();
            this.lblSemaforo = new System.Windows.Forms.Label();
            this.cmbSemaforo = new System.Windows.Forms.ComboBox();
            this.lblHorizonte = new System.Windows.Forms.Label();
            this.cmbHorizonte = new System.Windows.Forms.ComboBox();
            this.btnGenerar = new System.Windows.Forms.Button();
            this.dgvReporte = new System.Windows.Forms.DataGridView();
            this.btnImprimir = new System.Windows.Forms.Button();
            this.btnEnviarRevision = new System.Windows.Forms.Button();
            this.pnlResumen.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReporte)).BeginInit();
            this.SuspendLayout();
            //
            // pnlResumen
            //
            this.pnlResumen.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlResumen.Controls.Add(this.lblResumenTitulo);
            this.pnlResumen.Controls.Add(this.lblResumenSemaforos);
            this.pnlResumen.Controls.Add(this.lblResumenCategoria);
            this.pnlResumen.Location = new System.Drawing.Point(12, 12);
            this.pnlResumen.Name = "pnlResumen";
            this.pnlResumen.Size = new System.Drawing.Size(860, 84);
            this.pnlResumen.TabIndex = 0;
            //
            // lblResumenTitulo
            //
            this.lblResumenTitulo.AutoSize = true;
            this.lblResumenTitulo.Location = new System.Drawing.Point(12, 8);
            this.lblResumenTitulo.Name = "lblResumenTitulo";
            this.lblResumenTitulo.Size = new System.Drawing.Size(70, 16);
            this.lblResumenTitulo.TabIndex = 0;
            this.lblResumenTitulo.Text = "Resumen";
            //
            // lblResumenSemaforos
            //
            this.lblResumenSemaforos.AutoSize = true;
            this.lblResumenSemaforos.Location = new System.Drawing.Point(12, 36);
            this.lblResumenSemaforos.Name = "lblResumenSemaforos";
            this.lblResumenSemaforos.Size = new System.Drawing.Size(10, 16);
            this.lblResumenSemaforos.TabIndex = 1;
            //
            // lblResumenCategoria
            //
            this.lblResumenCategoria.AutoSize = true;
            this.lblResumenCategoria.Location = new System.Drawing.Point(12, 58);
            this.lblResumenCategoria.Name = "lblResumenCategoria";
            this.lblResumenCategoria.Size = new System.Drawing.Size(10, 16);
            this.lblResumenCategoria.TabIndex = 2;
            //
            // lblCategoria
            //
            this.lblCategoria.AutoSize = true;
            this.lblCategoria.Location = new System.Drawing.Point(12, 115);
            this.lblCategoria.Name = "lblCategoria";
            this.lblCategoria.Size = new System.Drawing.Size(69, 16);
            this.lblCategoria.TabIndex = 1;
            this.lblCategoria.Text = "Categoria:";
            //
            // cmbCategoria
            //
            this.cmbCategoria.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategoria.Location = new System.Drawing.Point(90, 112);
            this.cmbCategoria.Name = "cmbCategoria";
            this.cmbCategoria.Size = new System.Drawing.Size(160, 24);
            this.cmbCategoria.TabIndex = 2;
            this.cmbCategoria.SelectedIndexChanged += new System.EventHandler(this.cmbFiltro_SelectedIndexChanged);
            //
            // lblSemaforo
            //
            this.lblSemaforo.AutoSize = true;
            this.lblSemaforo.Location = new System.Drawing.Point(266, 115);
            this.lblSemaforo.Name = "lblSemaforo";
            this.lblSemaforo.Size = new System.Drawing.Size(70, 16);
            this.lblSemaforo.TabIndex = 3;
            this.lblSemaforo.Text = "Semaforo:";
            //
            // cmbSemaforo
            //
            this.cmbSemaforo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSemaforo.Location = new System.Drawing.Point(342, 112);
            this.cmbSemaforo.Name = "cmbSemaforo";
            this.cmbSemaforo.Size = new System.Drawing.Size(140, 24);
            this.cmbSemaforo.TabIndex = 4;
            this.cmbSemaforo.SelectedIndexChanged += new System.EventHandler(this.cmbFiltro_SelectedIndexChanged);
            //
            // lblHorizonte
            //
            this.lblHorizonte.AutoSize = true;
            this.lblHorizonte.Location = new System.Drawing.Point(498, 115);
            this.lblHorizonte.Name = "lblHorizonte";
            this.lblHorizonte.Size = new System.Drawing.Size(70, 16);
            this.lblHorizonte.TabIndex = 5;
            this.lblHorizonte.Text = "Horizonte:";
            //
            // cmbHorizonte
            //
            this.cmbHorizonte.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbHorizonte.Location = new System.Drawing.Point(574, 112);
            this.cmbHorizonte.Name = "cmbHorizonte";
            this.cmbHorizonte.Size = new System.Drawing.Size(120, 24);
            this.cmbHorizonte.TabIndex = 6;
            this.cmbHorizonte.SelectedIndexChanged += new System.EventHandler(this.cmbFiltro_SelectedIndexChanged);
            //
            // btnGenerar
            //
            this.btnGenerar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGenerar.Location = new System.Drawing.Point(712, 109);
            this.btnGenerar.Name = "btnGenerar";
            this.btnGenerar.Size = new System.Drawing.Size(160, 30);
            this.btnGenerar.TabIndex = 7;
            this.btnGenerar.Text = "Generar reporte";
            this.btnGenerar.UseVisualStyleBackColor = true;
            this.btnGenerar.Click += new System.EventHandler(this.btnGenerar_Click);
            //
            // dgvReporte
            //
            this.dgvReporte.AllowUserToAddRows = false;
            this.dgvReporte.AllowUserToDeleteRows = false;
            this.dgvReporte.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvReporte.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReporte.Location = new System.Drawing.Point(12, 152);
            this.dgvReporte.MultiSelect = false;
            this.dgvReporte.Name = "dgvReporte";
            this.dgvReporte.ReadOnly = true;
            this.dgvReporte.RowHeadersWidth = 51;
            this.dgvReporte.RowTemplate.Height = 24;
            this.dgvReporte.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvReporte.Size = new System.Drawing.Size(860, 290);
            this.dgvReporte.TabIndex = 8;
            this.dgvReporte.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvReporte_CellClick);
            this.dgvReporte.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvReporte_CellFormatting);
            //
            // btnImprimir
            //
            this.btnImprimir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnImprimir.Location = new System.Drawing.Point(12, 455);
            this.btnImprimir.Name = "btnImprimir";
            this.btnImprimir.Size = new System.Drawing.Size(220, 30);
            this.btnImprimir.TabIndex = 9;
            this.btnImprimir.Text = "Imprimir informe";
            this.btnImprimir.UseVisualStyleBackColor = true;
            this.btnImprimir.Click += new System.EventHandler(this.btnImprimir_Click);
            //
            // btnEnviarRevision
            //
            this.btnEnviarRevision.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEnviarRevision.Location = new System.Drawing.Point(244, 455);
            this.btnEnviarRevision.Name = "btnEnviarRevision";
            this.btnEnviarRevision.Size = new System.Drawing.Size(220, 30);
            this.btnEnviarRevision.TabIndex = 10;
            this.btnEnviarRevision.Text = "Enviar a revision";
            this.btnEnviarRevision.UseVisualStyleBackColor = true;
            this.btnEnviarRevision.Click += new System.EventHandler(this.btnEnviarRevision_Click);
            //
            // FrmReporteMantenimientoJP86
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(884, 500);
            this.Controls.Add(this.btnEnviarRevision);
            this.Controls.Add(this.btnImprimir);
            this.Controls.Add(this.dgvReporte);
            this.Controls.Add(this.btnGenerar);
            this.Controls.Add(this.cmbHorizonte);
            this.Controls.Add(this.lblHorizonte);
            this.Controls.Add(this.cmbSemaforo);
            this.Controls.Add(this.lblSemaforo);
            this.Controls.Add(this.cmbCategoria);
            this.Controls.Add(this.lblCategoria);
            this.Controls.Add(this.pnlResumen);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmReporteMantenimientoJP86";
            this.Text = "FrmReporteMantenimientoJP86";
            this.Load += new System.EventHandler(this.FrmReporteMantenimientoJP86_Load);
            this.pnlResumen.ResumeLayout(false);
            this.pnlResumen.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReporte)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlResumen;
        private System.Windows.Forms.Label lblResumenTitulo;
        private System.Windows.Forms.Label lblResumenSemaforos;
        private System.Windows.Forms.Label lblResumenCategoria;
        private System.Windows.Forms.Label lblCategoria;
        private System.Windows.Forms.ComboBox cmbCategoria;
        private System.Windows.Forms.Label lblSemaforo;
        private System.Windows.Forms.ComboBox cmbSemaforo;
        private System.Windows.Forms.Label lblHorizonte;
        private System.Windows.Forms.ComboBox cmbHorizonte;
        private System.Windows.Forms.Button btnGenerar;
        private System.Windows.Forms.DataGridView dgvReporte;
        private System.Windows.Forms.Button btnImprimir;
        private System.Windows.Forms.Button btnEnviarRevision;
    }
}
