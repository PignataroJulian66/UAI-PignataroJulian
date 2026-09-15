namespace ProyectoIS_64PR
{
    partial class FrmReportesPendientesJP86
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
            this.dgvReportes = new System.Windows.Forms.DataGridView();
            this.lblHistorialTitulo = new System.Windows.Forms.Label();
            this.dgvHistorial = new System.Windows.Forms.DataGridView();
            this.lblCriticidad = new System.Windows.Forms.Label();
            this.cmbCriticidad = new System.Windows.Forms.ComboBox();
            this.btnAsignarCriticidad = new System.Windows.Forms.Button();
            this.lblModalidad = new System.Windows.Forms.Label();
            this.rbInterna = new System.Windows.Forms.RadioButton();
            this.rbExterna = new System.Windows.Forms.RadioButton();
            this.btnConfirmarModalidad = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReportes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistorial)).BeginInit();
            this.SuspendLayout();
            //
            // dgvReportes
            //
            this.dgvReportes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right))));
            this.dgvReportes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReportes.Location = new System.Drawing.Point(12, 12);
            this.dgvReportes.MultiSelect = false;
            this.dgvReportes.Name = "dgvReportes";
            this.dgvReportes.ReadOnly = true;
            this.dgvReportes.RowHeadersWidth = 51;
            this.dgvReportes.RowTemplate.Height = 24;
            this.dgvReportes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvReportes.Size = new System.Drawing.Size(860, 140);
            this.dgvReportes.TabIndex = 0;
            this.dgvReportes.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvReportes_CellClick);
            //
            // lblHistorialTitulo
            //
            this.lblHistorialTitulo.AutoSize = true;
            this.lblHistorialTitulo.Location = new System.Drawing.Point(12, 158);
            this.lblHistorialTitulo.Name = "lblHistorialTitulo";
            this.lblHistorialTitulo.Size = new System.Drawing.Size(280, 16);
            this.lblHistorialTitulo.TabIndex = 1;
            this.lblHistorialTitulo.Text = "Historial de mantenimiento de la unidad:";
            //
            // dgvHistorial
            //
            this.dgvHistorial.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right))));
            this.dgvHistorial.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHistorial.Location = new System.Drawing.Point(12, 178);
            this.dgvHistorial.MultiSelect = false;
            this.dgvHistorial.Name = "dgvHistorial";
            this.dgvHistorial.ReadOnly = true;
            this.dgvHistorial.RowHeadersWidth = 51;
            this.dgvHistorial.RowTemplate.Height = 24;
            this.dgvHistorial.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvHistorial.Size = new System.Drawing.Size(860, 110);
            this.dgvHistorial.TabIndex = 2;
            //
            // lblCriticidad
            //
            this.lblCriticidad.AutoSize = true;
            this.lblCriticidad.Location = new System.Drawing.Point(12, 300);
            this.lblCriticidad.Name = "lblCriticidad";
            this.lblCriticidad.Size = new System.Drawing.Size(75, 16);
            this.lblCriticidad.TabIndex = 3;
            this.lblCriticidad.Text = "Criticidad:";
            this.lblCriticidad.Visible = false;
            //
            // cmbCriticidad
            //
            this.cmbCriticidad.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCriticidad.FormattingEnabled = true;
            this.cmbCriticidad.Location = new System.Drawing.Point(95, 297);
            this.cmbCriticidad.Name = "cmbCriticidad";
            this.cmbCriticidad.Size = new System.Drawing.Size(150, 24);
            this.cmbCriticidad.TabIndex = 4;
            this.cmbCriticidad.Visible = false;
            //
            // btnAsignarCriticidad
            //
            this.btnAsignarCriticidad.Location = new System.Drawing.Point(260, 296);
            this.btnAsignarCriticidad.Name = "btnAsignarCriticidad";
            this.btnAsignarCriticidad.Size = new System.Drawing.Size(180, 28);
            this.btnAsignarCriticidad.TabIndex = 5;
            this.btnAsignarCriticidad.Text = "Asignar criticidad";
            this.btnAsignarCriticidad.UseVisualStyleBackColor = true;
            this.btnAsignarCriticidad.Visible = false;
            this.btnAsignarCriticidad.Click += new System.EventHandler(this.btnAsignarCriticidad_Click);
            //
            // lblModalidad
            //
            this.lblModalidad.AutoSize = true;
            this.lblModalidad.Location = new System.Drawing.Point(12, 300);
            this.lblModalidad.Name = "lblModalidad";
            this.lblModalidad.Size = new System.Drawing.Size(170, 16);
            this.lblModalidad.TabIndex = 6;
            this.lblModalidad.Text = "Modalidad de reparacion:";
            this.lblModalidad.Visible = false;
            //
            // rbInterna
            //
            this.rbInterna.AutoSize = true;
            this.rbInterna.Location = new System.Drawing.Point(190, 299);
            this.rbInterna.Name = "rbInterna";
            this.rbInterna.Size = new System.Drawing.Size(90, 20);
            this.rbInterna.TabIndex = 7;
            this.rbInterna.TabStop = true;
            this.rbInterna.Text = "Interna";
            this.rbInterna.UseVisualStyleBackColor = true;
            this.rbInterna.Visible = false;
            //
            // rbExterna
            //
            this.rbExterna.AutoSize = true;
            this.rbExterna.Location = new System.Drawing.Point(290, 299);
            this.rbExterna.Name = "rbExterna";
            this.rbExterna.Size = new System.Drawing.Size(90, 20);
            this.rbExterna.TabIndex = 8;
            this.rbExterna.Text = "Externa";
            this.rbExterna.UseVisualStyleBackColor = true;
            this.rbExterna.Visible = false;
            //
            // btnConfirmarModalidad
            //
            this.btnConfirmarModalidad.Location = new System.Drawing.Point(400, 296);
            this.btnConfirmarModalidad.Name = "btnConfirmarModalidad";
            this.btnConfirmarModalidad.Size = new System.Drawing.Size(180, 28);
            this.btnConfirmarModalidad.TabIndex = 9;
            this.btnConfirmarModalidad.Text = "Confirmar modalidad";
            this.btnConfirmarModalidad.UseVisualStyleBackColor = true;
            this.btnConfirmarModalidad.Visible = false;
            this.btnConfirmarModalidad.Click += new System.EventHandler(this.btnConfirmarModalidad_Click);
            //
            // FrmReportesPendientesJP86
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(900, 340);
            this.Controls.Add(this.btnConfirmarModalidad);
            this.Controls.Add(this.rbExterna);
            this.Controls.Add(this.rbInterna);
            this.Controls.Add(this.lblModalidad);
            this.Controls.Add(this.btnAsignarCriticidad);
            this.Controls.Add(this.cmbCriticidad);
            this.Controls.Add(this.lblCriticidad);
            this.Controls.Add(this.dgvHistorial);
            this.Controls.Add(this.lblHistorialTitulo);
            this.Controls.Add(this.dgvReportes);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmReportesPendientesJP86";
            this.Text = "FrmReportesPendientesJP86";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmReportesPendientesJP86_FormClosed);
            this.Load += new System.EventHandler(this.FrmReportesPendientesJP86_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvReportes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistorial)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvReportes;
        private System.Windows.Forms.Label lblHistorialTitulo;
        private System.Windows.Forms.DataGridView dgvHistorial;
        private System.Windows.Forms.Label lblCriticidad;
        private System.Windows.Forms.ComboBox cmbCriticidad;
        private System.Windows.Forms.Button btnAsignarCriticidad;
        private System.Windows.Forms.Label lblModalidad;
        private System.Windows.Forms.RadioButton rbInterna;
        private System.Windows.Forms.RadioButton rbExterna;
        private System.Windows.Forms.Button btnConfirmarModalidad;
    }
}
