namespace ProyectoIS_64PR
{
    partial class FrmReparacionesEnCursoJP86
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
            this.lblDescripcionCierre = new System.Windows.Forms.Label();
            this.txtDescripcionCierre = new System.Windows.Forms.TextBox();
            this.btnAcreditar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReportes)).BeginInit();
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
            this.dgvReportes.Size = new System.Drawing.Size(860, 220);
            this.dgvReportes.TabIndex = 0;
            this.dgvReportes.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvReportes_CellClick);
            //
            // lblDescripcionCierre
            //
            this.lblDescripcionCierre.AutoSize = true;
            this.lblDescripcionCierre.Location = new System.Drawing.Point(12, 244);
            this.lblDescripcionCierre.Name = "lblDescripcionCierre";
            this.lblDescripcionCierre.Size = new System.Drawing.Size(220, 16);
            this.lblDescripcionCierre.TabIndex = 1;
            this.lblDescripcionCierre.Text = "Descripcion de cierre de reparacion:";
            //
            // txtDescripcionCierre
            //
            this.txtDescripcionCierre.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right))));
            this.txtDescripcionCierre.Location = new System.Drawing.Point(12, 264);
            this.txtDescripcionCierre.Multiline = true;
            this.txtDescripcionCierre.Name = "txtDescripcionCierre";
            this.txtDescripcionCierre.Size = new System.Drawing.Size(860, 80);
            this.txtDescripcionCierre.TabIndex = 2;
            //
            // btnAcreditar
            //
            this.btnAcreditar.Location = new System.Drawing.Point(12, 355);
            this.btnAcreditar.Name = "btnAcreditar";
            this.btnAcreditar.Size = new System.Drawing.Size(220, 30);
            this.btnAcreditar.TabIndex = 3;
            this.btnAcreditar.Text = "Acreditar reparacion";
            this.btnAcreditar.UseVisualStyleBackColor = true;
            this.btnAcreditar.Click += new System.EventHandler(this.btnAcreditar_Click);
            //
            // FrmReparacionesEnCursoJP86
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(900, 400);
            this.Controls.Add(this.btnAcreditar);
            this.Controls.Add(this.txtDescripcionCierre);
            this.Controls.Add(this.lblDescripcionCierre);
            this.Controls.Add(this.dgvReportes);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmReparacionesEnCursoJP86";
            this.Text = "FrmReparacionesEnCursoJP86";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmReparacionesEnCursoJP86_FormClosed);
            this.Load += new System.EventHandler(this.FrmReparacionesEnCursoJP86_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvReportes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvReportes;
        private System.Windows.Forms.Label lblDescripcionCierre;
        private System.Windows.Forms.TextBox txtDescripcionCierre;
        private System.Windows.Forms.Button btnAcreditar;
    }
}
