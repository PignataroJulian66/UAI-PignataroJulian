namespace ProyectoIS_64PR
{
    partial class ucModificarVehiculoJP86
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
            this.lblPatente = new System.Windows.Forms.Label();
            this.txtPatente = new System.Windows.Forms.TextBox();
            this.lblMarca = new System.Windows.Forms.Label();
            this.txtMarca = new System.Windows.Forms.TextBox();
            this.lblModelo = new System.Windows.Forms.Label();
            this.txtModelo = new System.Windows.Forms.TextBox();
            this.lblCategoria = new System.Windows.Forms.Label();
            this.cmbCategoria = new System.Windows.Forms.ComboBox();
            this.lblKilometraje = new System.Windows.Forms.Label();
            this.numKilometraje = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numKilometraje)).BeginInit();
            this.SuspendLayout();
            //
            // lblPatente
            //
            this.lblPatente.AutoSize = true;
            this.lblPatente.Location = new System.Drawing.Point(6, 15);
            this.lblPatente.Name = "lblPatente";
            this.lblPatente.Size = new System.Drawing.Size(58, 16);
            this.lblPatente.TabIndex = 0;
            this.lblPatente.Text = "Patente:";
            //
            // txtPatente
            //
            this.txtPatente.Location = new System.Drawing.Point(110, 12);
            this.txtPatente.Name = "txtPatente";
            this.txtPatente.Size = new System.Drawing.Size(140, 22);
            this.txtPatente.TabIndex = 1;
            //
            // lblMarca
            //
            this.lblMarca.AutoSize = true;
            this.lblMarca.Location = new System.Drawing.Point(6, 46);
            this.lblMarca.Name = "lblMarca";
            this.lblMarca.Size = new System.Drawing.Size(51, 16);
            this.lblMarca.TabIndex = 2;
            this.lblMarca.Text = "Marca:";
            //
            // txtMarca
            //
            this.txtMarca.Location = new System.Drawing.Point(110, 43);
            this.txtMarca.Name = "txtMarca";
            this.txtMarca.Size = new System.Drawing.Size(140, 22);
            this.txtMarca.TabIndex = 3;
            //
            // lblModelo
            //
            this.lblModelo.AutoSize = true;
            this.lblModelo.Location = new System.Drawing.Point(6, 77);
            this.lblModelo.Name = "lblModelo";
            this.lblModelo.Size = new System.Drawing.Size(58, 16);
            this.lblModelo.TabIndex = 4;
            this.lblModelo.Text = "Modelo:";
            //
            // txtModelo
            //
            this.txtModelo.Location = new System.Drawing.Point(110, 74);
            this.txtModelo.Name = "txtModelo";
            this.txtModelo.Size = new System.Drawing.Size(140, 22);
            this.txtModelo.TabIndex = 5;
            //
            // lblCategoria
            //
            this.lblCategoria.AutoSize = true;
            this.lblCategoria.Location = new System.Drawing.Point(6, 108);
            this.lblCategoria.Name = "lblCategoria";
            this.lblCategoria.Size = new System.Drawing.Size(70, 16);
            this.lblCategoria.TabIndex = 6;
            this.lblCategoria.Text = "Categoria:";
            //
            // cmbCategoria
            //
            this.cmbCategoria.FormattingEnabled = true;
            this.cmbCategoria.Location = new System.Drawing.Point(110, 105);
            this.cmbCategoria.Name = "cmbCategoria";
            this.cmbCategoria.Size = new System.Drawing.Size(140, 24);
            this.cmbCategoria.TabIndex = 7;
            //
            // lblKilometraje
            //
            this.lblKilometraje.AutoSize = true;
            this.lblKilometraje.Location = new System.Drawing.Point(6, 139);
            this.lblKilometraje.Name = "lblKilometraje";
            this.lblKilometraje.Size = new System.Drawing.Size(90, 16);
            this.lblKilometraje.TabIndex = 8;
            this.lblKilometraje.Text = "Kilometraje:";
            //
            // numKilometraje
            //
            this.numKilometraje.Location = new System.Drawing.Point(110, 137);
            this.numKilometraje.Name = "numKilometraje";
            this.numKilometraje.Size = new System.Drawing.Size(140, 22);
            this.numKilometraje.TabIndex = 9;
            //
            // ucModificarVehiculoJP86
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.numKilometraje);
            this.Controls.Add(this.lblKilometraje);
            this.Controls.Add(this.cmbCategoria);
            this.Controls.Add(this.lblCategoria);
            this.Controls.Add(this.txtModelo);
            this.Controls.Add(this.lblModelo);
            this.Controls.Add(this.txtMarca);
            this.Controls.Add(this.lblMarca);
            this.Controls.Add(this.txtPatente);
            this.Controls.Add(this.lblPatente);
            this.Name = "ucModificarVehiculoJP86";
            this.Size = new System.Drawing.Size(260, 170);
            ((System.ComponentModel.ISupportInitialize)(this.numKilometraje)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPatente;
        private System.Windows.Forms.TextBox txtPatente;
        private System.Windows.Forms.Label lblMarca;
        private System.Windows.Forms.TextBox txtMarca;
        private System.Windows.Forms.Label lblModelo;
        private System.Windows.Forms.TextBox txtModelo;
        private System.Windows.Forms.Label lblCategoria;
        private System.Windows.Forms.ComboBox cmbCategoria;
        private System.Windows.Forms.Label lblKilometraje;
        private System.Windows.Forms.NumericUpDown numKilometraje;
    }
}
