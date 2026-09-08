namespace ProyectoIS_64PR
{
    partial class ucModificarCategoria
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

        #region Código generado por el Diseñador de componentes

        private void InitializeComponent()
        {
            this.lblNombre = new System.Windows.Forms.Label();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.lblDescripcion = new System.Windows.Forms.Label();
            this.txtDescripcion = new System.Windows.Forms.TextBox();
            this.lblTarifaDiaria = new System.Windows.Forms.Label();
            this.numTarifaDiaria = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numTarifaDiaria)).BeginInit();
            this.SuspendLayout();
            //
            // lblNombre
            //
            this.lblNombre.AutoSize = true;
            this.lblNombre.Location = new System.Drawing.Point(6, 15);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Size = new System.Drawing.Size(59, 16);
            this.lblNombre.TabIndex = 0;
            this.lblNombre.Text = "Nombre:";
            //
            // txtNombre
            //
            this.txtNombre.Location = new System.Drawing.Point(110, 12);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(140, 22);
            this.txtNombre.TabIndex = 1;
            //
            // lblDescripcion
            //
            this.lblDescripcion.AutoSize = true;
            this.lblDescripcion.Location = new System.Drawing.Point(6, 46);
            this.lblDescripcion.Name = "lblDescripcion";
            this.lblDescripcion.Size = new System.Drawing.Size(85, 16);
            this.lblDescripcion.TabIndex = 2;
            this.lblDescripcion.Text = "Descripción:";
            //
            // txtDescripcion
            //
            this.txtDescripcion.Location = new System.Drawing.Point(110, 43);
            this.txtDescripcion.Name = "txtDescripcion";
            this.txtDescripcion.Size = new System.Drawing.Size(140, 22);
            this.txtDescripcion.TabIndex = 3;
            //
            // lblTarifaDiaria
            //
            this.lblTarifaDiaria.AutoSize = true;
            this.lblTarifaDiaria.Location = new System.Drawing.Point(6, 77);
            this.lblTarifaDiaria.Name = "lblTarifaDiaria";
            this.lblTarifaDiaria.Size = new System.Drawing.Size(98, 16);
            this.lblTarifaDiaria.TabIndex = 4;
            this.lblTarifaDiaria.Text = "Tarifa diaria:";
            //
            // numTarifaDiaria
            //
            this.numTarifaDiaria.DecimalPlaces = 2;
            this.numTarifaDiaria.Location = new System.Drawing.Point(110, 75);
            this.numTarifaDiaria.Name = "numTarifaDiaria";
            this.numTarifaDiaria.Size = new System.Drawing.Size(140, 22);
            this.numTarifaDiaria.TabIndex = 5;
            //
            // ucModificarCategoria
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.numTarifaDiaria);
            this.Controls.Add(this.lblTarifaDiaria);
            this.Controls.Add(this.txtDescripcion);
            this.Controls.Add(this.lblDescripcion);
            this.Controls.Add(this.txtNombre);
            this.Controls.Add(this.lblNombre);
            this.Name = "ucModificarCategoria";
            this.Size = new System.Drawing.Size(260, 110);
            ((System.ComponentModel.ISupportInitialize)(this.numTarifaDiaria)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Label lblDescripcion;
        private System.Windows.Forms.TextBox txtDescripcion;
        private System.Windows.Forms.Label lblTarifaDiaria;
        private System.Windows.Forms.NumericUpDown numTarifaDiaria;
    }
}
