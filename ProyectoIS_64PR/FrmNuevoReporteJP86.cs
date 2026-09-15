using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace ProyectoIS_64PR
{
    public partial class FrmNuevoReporteJP86 : Form, Idioma.IObservadorIdioma_64PR
    {
        Bitacora.Bitacora_64PR bita = new Bitacora.Bitacora_64PR();
        Bitacora.Evento_64PR ev;

        BLL_64PR.ReporteJP86 greportes = new BLL_64PR.ReporteJP86();
        public Dictionary<string, string> textos;

        BE.VehiculoJP86 vehiculoSeleccionado;
        BE.ReporteJP86 reporteGenerado;

        public FrmNuevoReporteJP86()
        {
            InitializeComponent();

            this.Font = UI.TemaVisual.FuenteTexto;
            this.BackColor = UI.TemaVisual.FondoPagina;
            UI.EstilosUI.AplicarEstiloGrilla(dgvVehiculos);
            UI.EstilosUI.AplicarBadgeColumna(dgvVehiculos, "Estado", UI.EstilosUI.ColorEstadoVehiculo);
            UI.EstilosUI.EstiloBotonPrimario(btnConfirmar);
            UI.EstilosUI.EstiloBotonSecundario(btnImprimirReporte);
            UI.EstilosUI.EstiloBotonPeligro(btnCancelar);

            cmbFuente.DataSource = Enum.GetValues(typeof(BE.FuenteReporteJP86));

            Idioma.GestorIdioma_64PR.GetInstance.Suscribir(this);

            textos = Idioma.GestorIdioma_64PR.GetInstance.ObtenerTextos();
            if (textos.Count > 0)
                ActualizarIdioma(textos);

            CargarVehiculos();
        }

        public void ActualizarIdioma(Dictionary<string, string> textoss)
        {
            textos = textoss;
            Traductor_64PR.Traducir(this, textos);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            Idioma.GestorIdioma_64PR.GetInstance.Desuscribir(this);
            base.OnFormClosed(e);
        }

        private void FrmNuevoReporteJP86_Load(object sender, EventArgs e)
        {
            ConfigurarPermisos();
        }

        private void ConfigurarPermisos()
        {
            Sesion.Rol_64PR rolUsuario = Sesion.SessionManager.GetInstance.Usuario.Rol;
            bool puedeRegistrar = rolUsuario.TienePermiso(Sesion.Patentes_64PR.RegistrarReporte);
            txtDescripcion.Enabled = puedeRegistrar;
            cmbFuente.Enabled = puedeRegistrar;
            btnConfirmar.Enabled = puedeRegistrar && vehiculoSeleccionado != null;
        }

        private void CargarVehiculos()
        {
            List<BE.VehiculoJP86> disponibles = greportes.ListarVehiculosEnRevision();

            dgvVehiculos.DataSource = null;
            dgvVehiculos.DataSource = disponibles;
            ///Rebindear regenera las columnas (autogeneradas) y pisa cualquier traduccion previa: hay que reaplicarla
            Traductor_64PR.TraducirGrilla(this, dgvVehiculos, textos);

            vehiculoSeleccionado = null;
            btnConfirmar.Enabled = false;

            bool haySinRevision = disponibles.Count == 0;
            txtDescripcion.Enabled = !haySinRevision;
            cmbFuente.Enabled = !haySinRevision;
            dgvVehiculos.Enabled = !haySinRevision;

            if (haySinRevision)
                MessageBox.Show(textos["msg_sinVehiculosEnRevision"]);
        }

        private void dgvVehiculos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                vehiculoSeleccionado = dgvVehiculos.SelectedRows[0].DataBoundItem as BE.VehiculoJP86;

                Sesion.Rol_64PR rolUsuario = Sesion.SessionManager.GetInstance.Usuario.Rol;
                btnConfirmar.Enabled = rolUsuario.TienePermiso(Sesion.Patentes_64PR.RegistrarReporte);
            }
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            if (vehiculoSeleccionado == null)
            {
                MessageBox.Show(textos["msg_seleccionarVehiculo"]);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
            {
                MessageBox.Show(textos["msg_descripcionValida"]);
                return;
            }

            BE.FuenteReporteJP86 fuente = (BE.FuenteReporteJP86)cmbFuente.SelectedItem;

            try
            {
                BE.ReporteJP86 reporte = greportes.RegistrarReporte(vehiculoSeleccionado, txtDescripcion.Text.Trim(), fuente);

                ev = new Bitacora.Evento_64PR(Sesion.SessionManager.GetInstance.Usuario.Login, ((int)Bitacora.Bitacora_64PR.ModuloBitacora_64PR.GestionMantenimiento).ToString(), ((int)Bitacora.Bitacora_64PR.TipoEventoBitacora_64PR.AltaReporte).ToString(), 3);
                bita.RegistrarEvento(ev);

                MessageBox.Show(textos["reporte_generado"] + reporte.NumeroReporte.ToString());

                reporteGenerado = reporte;
                btnImprimirReporte.Enabled = true;

                LimpiarFormulario();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnImprimirReporte_Click(object sender, EventArgs e)
        {
            Documentos.GeneradorReporteDesperfecto.Imprimir(reporteGenerado);
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        private void LimpiarFormulario()
        {
            vehiculoSeleccionado = null;
            txtDescripcion.Text = string.Empty;
            cmbFuente.SelectedIndex = 0;
            dgvVehiculos.ClearSelection();

            CargarVehiculos();
        }

        private void FrmNuevoReporteJP86_FormClosed(object sender, FormClosedEventArgs e)
        {
            Idioma.GestorIdioma_64PR.GetInstance.Desuscribir(this);
        }
    }
}
