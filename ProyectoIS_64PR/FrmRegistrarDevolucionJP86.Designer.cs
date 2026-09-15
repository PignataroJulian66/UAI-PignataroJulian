namespace ProyectoIS_64PR
{
    partial class FrmRegistrarDevolucionJP86
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
            this.lblBuscar = new System.Windows.Forms.Label();
            this.txtBuscar = new System.Windows.Forms.TextBox();
            this.dgvContratos = new System.Windows.Forms.DataGridView();
            this.lblKilometrajeRetorno = new System.Windows.Forms.Label();
            this.numKilometrajeRetorno = new System.Windows.Forms.NumericUpDown();
            this.lblEstadoUnidad = new System.Windows.Forms.Label();
            this.cmbEstadoUnidad = new System.Windows.Forms.ComboBox();
            this.btnRegistrarDevolucion = new System.Windows.Forms.Button();
            this.btnImprimirRecibo = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvContratos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numKilometrajeRetorno)).BeginInit();
            this.SuspendLayout();
            //
            // lblBuscar
            //
            this.lblBuscar.AutoSize = true;
            this.lblBuscar.Location = new System.Drawing.Point(12, 16);
            this.lblBuscar.Name = "lblBuscar";
            this.lblBuscar.Size = new System.Drawing.Size(55, 16);
            this.lblBuscar.TabIndex = 0;
            this.lblBuscar.Text = "Buscar:";
            //
            // txtBuscar
            //
            this.txtBuscar.Location = new System.Drawing.Point(73, 13);
            this.txtBuscar.Name = "txtBuscar";
            this.txtBuscar.Size = new System.Drawing.Size(250, 22);
            this.txtBuscar.TabIndex = 1;
            this.txtBuscar.TextChanged += new System.EventHandler(this.txtBuscar_TextChanged);
            //
            // dgvContratos
            //
            this.dgvContratos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvContratos.Location = new System.Drawing.Point(12, 42);
            this.dgvContratos.MultiSelect = false;
            this.dgvContratos.Name = "dgvContratos";
            this.dgvContratos.ReadOnly = true;
            this.dgvContratos.RowHeadersWidth = 51;
            this.dgvContratos.RowTemplate.Height = 24;
            this.dgvContratos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvContratos.Size = new System.Drawing.Size(660, 260);
            this.dgvContratos.TabIndex = 2;
            this.dgvContratos.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContratos_CellClick);
            //
            // lblKilometrajeRetorno
            //
            this.lblKilometrajeRetorno.AutoSize = true;
            this.lblKilometrajeRetorno.Location = new System.Drawing.Point(12, 320);
            this.lblKilometrajeRetorno.Name = "lblKilometrajeRetorno";
            this.lblKilometrajeRetorno.Size = new System.Drawing.Size(160, 16);
            this.lblKilometrajeRetorno.TabIndex = 3;
            this.lblKilometrajeRetorno.Text = "Kilometraje de retorno:";
            //
            // numKilometrajeRetorno
            //
            this.numKilometrajeRetorno.Location = new System.Drawing.Point(180, 317);
            this.numKilometrajeRetorno.Maximum = new decimal(new int[] { 9999999, 0, 0, 0 });
            this.numKilometrajeRetorno.Name = "numKilometrajeRetorno";
            this.numKilometrajeRetorno.Size = new System.Drawing.Size(140, 22);
            this.numKilometrajeRetorno.TabIndex = 4;
            //
            // lblEstadoUnidad
            //
            this.lblEstadoUnidad.AutoSize = true;
            this.lblEstadoUnidad.Location = new System.Drawing.Point(12, 355);
            this.lblEstadoUnidad.Name = "lblEstadoUnidad";
            this.lblEstadoUnidad.Size = new System.Drawing.Size(160, 16);
            this.lblEstadoUnidad.TabIndex = 5;
            this.lblEstadoUnidad.Text = "Estado de la unidad:";
            //
            // cmbEstadoUnidad
            //
            this.cmbEstadoUnidad.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEstadoUnidad.FormattingEnabled = true;
            this.cmbEstadoUnidad.Location = new System.Drawing.Point(180, 352);
            this.cmbEstadoUnidad.Name = "cmbEstadoUnidad";
            this.cmbEstadoUnidad.Size = new System.Drawing.Size(180, 24);
            this.cmbEstadoUnidad.TabIndex = 6;
            //
            // btnRegistrarDevolucion
            //
            this.btnRegistrarDevolucion.Location = new System.Drawing.Point(12, 390);
            this.btnRegistrarDevolucion.Name = "btnRegistrarDevolucion";
            this.btnRegistrarDevolucion.Size = new System.Drawing.Size(180, 30);
            this.btnRegistrarDevolucion.TabIndex = 7;
            this.btnRegistrarDevolucion.Text = "Registrar devolucion";
            this.btnRegistrarDevolucion.UseVisualStyleBackColor = true;
            this.btnRegistrarDevolucion.Click += new System.EventHandler(this.btnRegistrarDevolucion_Click);
            //
            // btnImprimirRecibo
            //
            this.btnImprimirRecibo.Enabled = false;
            this.btnImprimirRecibo.Location = new System.Drawing.Point(200, 390);
            this.btnImprimirRecibo.Name = "btnImprimirRecibo";
            this.btnImprimirRecibo.Size = new System.Drawing.Size(180, 30);
            this.btnImprimirRecibo.TabIndex = 8;
            this.btnImprimirRecibo.Text = "Imprimir Recibo";
            this.btnImprimirRecibo.UseVisualStyleBackColor = true;
            this.btnImprimirRecibo.Click += new System.EventHandler(this.btnImprimirRecibo_Click);
            //
            // FrmRegistrarDevolucionJP86
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(700, 440);
            this.Controls.Add(this.btnImprimirRecibo);
            this.Controls.Add(this.btnRegistrarDevolucion);
            this.Controls.Add(this.cmbEstadoUnidad);
            this.Controls.Add(this.lblEstadoUnidad);
            this.Controls.Add(this.numKilometrajeRetorno);
            this.Controls.Add(this.lblKilometrajeRetorno);
            this.Controls.Add(this.dgvContratos);
            this.Controls.Add(this.txtBuscar);
            this.Controls.Add(this.lblBuscar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmRegistrarDevolucionJP86";
            this.Text = "FrmRegistrarDevolucionJP86";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmRegistrarDevolucionJP86_FormClosed);
            this.Load += new System.EventHandler(this.FrmRegistrarDevolucionJP86_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvContratos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numKilometrajeRetorno)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblBuscar;
        private System.Windows.Forms.TextBox txtBuscar;
        private System.Windows.Forms.DataGridView dgvContratos;
        private System.Windows.Forms.Label lblKilometrajeRetorno;
        private System.Windows.Forms.NumericUpDown numKilometrajeRetorno;
        private System.Windows.Forms.Label lblEstadoUnidad;
        private System.Windows.Forms.ComboBox cmbEstadoUnidad;
        private System.Windows.Forms.Button btnRegistrarDevolucion;
        private System.Windows.Forms.Button btnImprimirRecibo;
    }
}
