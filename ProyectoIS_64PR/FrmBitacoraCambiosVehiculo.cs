using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ProyectoIS_64PR
{
    public partial class FrmBitacoraCambiosVehiculo : Form, Idioma.IObservadorIdioma_64PR
    {
        BLL_64PR.VehiculoBitacoraCambios bll = new BLL_64PR.VehiculoBitacoraCambios();
        List<BE.VehiculoBitacoraCambios> lst;
        Dictionary<string, string> textos;

        public FrmBitacoraCambiosVehiculo()
        {
            InitializeComponent();

            dgvBitacora.ReadOnly = true;
            dgvBitacora.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBitacora.MultiSelect = false;
            dgvBitacora.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvBitacora.BackgroundColor = SystemColors.Menu;
            dgvBitacora.BorderStyle = BorderStyle.None;

            CargaData();
            LimpiarFiltros();

            Idioma.GestorIdioma_64PR.GetInstance.Suscribir(this);

            textos = Idioma.GestorIdioma_64PR.GetInstance.ObtenerTextos();
            if (textos.Count > 0)
                ActualizarIdioma(textos);
        }

        private void CargaData()
        {
            lst = bll.ObtenerBitacoraCambios(null, null, null, null);
            dgvBitacora.DataSource = lst;
        }

        private void LimpiarFiltros()
        {
            cbPatente.Checked = false;
            txtPatente.Text = string.Empty;
            txtPatente.Enabled = false;

            cbMarcaModelo.Checked = false;
            txtMarcaModelo.Text = string.Empty;
            txtMarcaModelo.Enabled = false;

            cbInicio.Checked = false;
            dtpInicio.Enabled = false;

            cbFin.Checked = false;
            dtpFin.Enabled = false;

            dgvBitacora.DataSource = null;
            dgvBitacora.DataSource = lst;
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFiltros();
        }

        private void btnAplicar_Click(object sender, EventArgs e)
        {
            AplicarFiltros();
        }

        private void AplicarFiltros()
        {
            if (cbInicio.Checked && cbFin.Checked && dtpInicio.Value.Date > dtpFin.Value.Date)
            {
                MessageBox.Show(textos["fechas_filtros"]);
                return;
            }

            IEnumerable<BE.VehiculoBitacoraCambios> resultado = lst;

            if (cbPatente.Checked && !string.IsNullOrWhiteSpace(txtPatente.Text))
                resultado = resultado.Where(b => b.Patente.IndexOf(txtPatente.Text.Trim(), StringComparison.OrdinalIgnoreCase) >= 0);

            if (cbMarcaModelo.Checked && !string.IsNullOrWhiteSpace(txtMarcaModelo.Text))
            {
                string texto = txtMarcaModelo.Text.Trim();
                resultado = resultado.Where(b =>
                    b.Marca.IndexOf(texto, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    b.Modelo.IndexOf(texto, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            if (cbInicio.Checked)
                resultado = resultado.Where(b => b.Fecha.Date >= dtpInicio.Value.Date);

            if (cbFin.Checked)
                resultado = resultado.Where(b => b.Fecha.Date <= dtpFin.Value.Date);

            dgvBitacora.DataSource = resultado.ToList();
        }

        private void cbPatente_CheckedChanged(object sender, EventArgs e)
        {
            txtPatente.Enabled = cbPatente.Checked;
            if (!cbPatente.Checked) txtPatente.Text = string.Empty;
        }

        private void cbMarcaModelo_CheckedChanged(object sender, EventArgs e)
        {
            txtMarcaModelo.Enabled = cbMarcaModelo.Checked;
            if (!cbMarcaModelo.Checked) txtMarcaModelo.Text = string.Empty;
        }

        private void cbInicio_CheckedChanged(object sender, EventArgs e)
        {
            dtpInicio.Enabled = cbInicio.Checked;
        }

        private void cbFin_CheckedChanged(object sender, EventArgs e)
        {
            dtpFin.Enabled = cbFin.Checked;
        }

        private void btnActivar_Click(object sender, EventArgs e)
        {
            if (dgvBitacora.SelectedRows.Count == 0)
            {
                MessageBox.Show(textos["seleccionar_registro_bitacora"]);
                return;
            }

            BE.VehiculoBitacoraCambios seleccionado = dgvBitacora.SelectedRows[0].DataBoundItem as BE.VehiculoBitacoraCambios;

            if (seleccionado.Act)
            {
                MessageBox.Show(textos["bitacoraCambios_yaVigente"]);
                return;
            }

            DialogResult confirmacion = MessageBox.Show(
                textos["confirmar_activacion_bitacoraCambios"],
                textos["confirmar_activacion_titulo"],
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmacion != DialogResult.Yes) return;

            try
            {
                bll.ActivarBitacoraCambios(seleccionado.IdBitacora);
                MessageBox.Show(textos["bitacoraCambios_activado"]);
                CargaData();
                LimpiarFiltros();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmBitacoraCambiosVehiculo_Load(object sender, EventArgs e)
        {
            ConfigurarPermisos();
        }

        private void ConfigurarPermisos()
        {
            Sesion.Rol_64PR rolUsuario = Sesion.SessionManager.GetInstance.Usuario.Rol;
            btnActivar.Visible = rolUsuario.TienePermiso(Sesion.Patentes_64PR.VerBitacoraCambiosVehiculo);
        }

        public void ActualizarIdioma(Dictionary<string, string> textoss)
        {
            textos = textoss;
            Traductor_64PR.Traducir(this, textos);
        }

        private void FrmBitacoraCambiosVehiculo_FormClosed(object sender, FormClosedEventArgs e)
        {
            Idioma.GestorIdioma_64PR.GetInstance.Desuscribir(this);
        }
    }
}
