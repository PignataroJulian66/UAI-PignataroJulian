namespace ProyectoIS_64PR
{
    partial class FrmGenerarFacturaJP86
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
            this.lblTitulo = new System.Windows.Forms.Label();
            this.dgvContratos = new System.Windows.Forms.DataGridView();
            this.lblImporte = new System.Windows.Forms.Label();
            this.lblMetodoPago = new System.Windows.Forms.Label();
            this.btnEfectivo = new System.Windows.Forms.Button();
            this.btnTarjeta = new System.Windows.Forms.Button();
            this.btnTransferencia = new System.Windows.Forms.Button();
            this.btnMercadoPago = new System.Windows.Forms.Button();
            this.btnImprimirFactura = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvContratos)).BeginInit();
            this.SuspendLayout();
            //
            // lblTitulo
            //
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Location = new System.Drawing.Point(12, 12);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(220, 16);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Contratos pendientes de cobro:";
            //
            // dgvContratos
            //
            this.dgvContratos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvContratos.Location = new System.Drawing.Point(12, 34);
            this.dgvContratos.MultiSelect = false;
            this.dgvContratos.Name = "dgvContratos";
            this.dgvContratos.ReadOnly = true;
            this.dgvContratos.RowHeadersWidth = 51;
            this.dgvContratos.RowTemplate.Height = 24;
            this.dgvContratos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvContratos.Size = new System.Drawing.Size(660, 300);
            this.dgvContratos.TabIndex = 1;
            this.dgvContratos.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContratos_CellClick);
            //
            // lblImporte
            //
            this.lblImporte.AutoSize = true;
            this.lblImporte.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.lblImporte.Location = new System.Drawing.Point(12, 350);
            this.lblImporte.Name = "lblImporte";
            this.lblImporte.Size = new System.Drawing.Size(150, 20);
            this.lblImporte.TabIndex = 2;
            this.lblImporte.Text = "Importe: $0";
            //
            // lblMetodoPago
            //
            this.lblMetodoPago.AutoSize = true;
            this.lblMetodoPago.Location = new System.Drawing.Point(12, 390);
            this.lblMetodoPago.Name = "lblMetodoPago";
            this.lblMetodoPago.Size = new System.Drawing.Size(160, 16);
            this.lblMetodoPago.TabIndex = 3;
            this.lblMetodoPago.Text = "Metodo de pago del cliente:";
            //
            // btnEfectivo
            //
            this.btnEfectivo.Enabled = false;
            this.btnEfectivo.Location = new System.Drawing.Point(12, 415);
            this.btnEfectivo.Name = "btnEfectivo";
            this.btnEfectivo.Size = new System.Drawing.Size(140, 30);
            this.btnEfectivo.TabIndex = 4;
            this.btnEfectivo.Text = "Efectivo";
            this.btnEfectivo.UseVisualStyleBackColor = true;
            this.btnEfectivo.Click += new System.EventHandler(this.btnEfectivo_Click);
            //
            // btnTarjeta
            //
            this.btnTarjeta.Enabled = false;
            this.btnTarjeta.Location = new System.Drawing.Point(160, 415);
            this.btnTarjeta.Name = "btnTarjeta";
            this.btnTarjeta.Size = new System.Drawing.Size(140, 30);
            this.btnTarjeta.TabIndex = 5;
            this.btnTarjeta.Text = "Tarjeta";
            this.btnTarjeta.UseVisualStyleBackColor = true;
            this.btnTarjeta.Click += new System.EventHandler(this.btnTarjeta_Click);
            //
            // btnTransferencia
            //
            this.btnTransferencia.Enabled = false;
            this.btnTransferencia.Location = new System.Drawing.Point(308, 415);
            this.btnTransferencia.Name = "btnTransferencia";
            this.btnTransferencia.Size = new System.Drawing.Size(140, 30);
            this.btnTransferencia.TabIndex = 6;
            this.btnTransferencia.Text = "Transferencia";
            this.btnTransferencia.UseVisualStyleBackColor = true;
            this.btnTransferencia.Click += new System.EventHandler(this.btnTransferencia_Click);
            //
            // btnMercadoPago
            //
            this.btnMercadoPago.Enabled = false;
            this.btnMercadoPago.Location = new System.Drawing.Point(456, 415);
            this.btnMercadoPago.Name = "btnMercadoPago";
            this.btnMercadoPago.Size = new System.Drawing.Size(140, 30);
            this.btnMercadoPago.TabIndex = 7;
            this.btnMercadoPago.Text = "Mercado Pago";
            this.btnMercadoPago.UseVisualStyleBackColor = true;
            this.btnMercadoPago.Click += new System.EventHandler(this.btnMercadoPago_Click);
            //
            // btnImprimirFactura
            //
            this.btnImprimirFactura.Enabled = false;
            this.btnImprimirFactura.Location = new System.Drawing.Point(12, 455);
            this.btnImprimirFactura.Name = "btnImprimirFactura";
            this.btnImprimirFactura.Size = new System.Drawing.Size(160, 30);
            this.btnImprimirFactura.TabIndex = 8;
            this.btnImprimirFactura.Text = "Imprimir Factura";
            this.btnImprimirFactura.UseVisualStyleBackColor = true;
            this.btnImprimirFactura.Click += new System.EventHandler(this.btnImprimirFactura_Click);
            //
            // FrmGenerarFacturaJP86
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(700, 500);
            this.Controls.Add(this.btnImprimirFactura);
            this.Controls.Add(this.btnMercadoPago);
            this.Controls.Add(this.btnTransferencia);
            this.Controls.Add(this.btnTarjeta);
            this.Controls.Add(this.btnEfectivo);
            this.Controls.Add(this.lblMetodoPago);
            this.Controls.Add(this.lblImporte);
            this.Controls.Add(this.dgvContratos);
            this.Controls.Add(this.lblTitulo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmGenerarFacturaJP86";
            this.Text = "FrmGenerarFacturaJP86";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmGenerarFacturaJP86_FormClosed);
            this.Load += new System.EventHandler(this.FrmGenerarFacturaJP86_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvContratos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.DataGridView dgvContratos;
        private System.Windows.Forms.Label lblImporte;
        private System.Windows.Forms.Label lblMetodoPago;
        private System.Windows.Forms.Button btnEfectivo;
        private System.Windows.Forms.Button btnTarjeta;
        private System.Windows.Forms.Button btnTransferencia;
        private System.Windows.Forms.Button btnMercadoPago;
        private System.Windows.Forms.Button btnImprimirFactura;
    }
}
