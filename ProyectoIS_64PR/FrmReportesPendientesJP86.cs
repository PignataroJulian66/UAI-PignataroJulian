using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace ProyectoIS_64PR
{
    ///CUN-06 "Asignar Criticidad" + CUN-07 "Determinar Modalidad Reparacion" (incluido siempre en el paso 7 de CUN-06).
    public partial class FrmReportesPendientesJP86 : Form, Idioma.IObservadorIdioma_64PR
    {
        Bitacora.Bitacora_64PR bita = new Bitacora.Bitacora_64PR();
        Bitacora.Evento_64PR ev;

        BLL_64PR.ReporteJP86 greportes = new BLL_64PR.ReporteJP86();
        public Dictionary<string, string> textos;

        BE.ReporteJP86 reporteSeleccionado;

        public FrmReportesPendientesJP86()
        {
            InitializeComponent();

            this.Font = UI.TemaVisual.FuenteTexto;
            this.BackColor = UI.TemaVisual.FondoPagina;
            UI.EstilosUI.AplicarEstiloGrilla(dgvReportes);
            UI.EstilosUI.AplicarEstiloGrilla(dgvHistorial);
            UI.EstilosUI.AplicarBadgeColumna(dgvReportes, "Estado", UI.EstilosUI.ColorEstadoReporte);
            UI.EstilosUI.AplicarBadgeColumna(dgvHistorial, "Estado", UI.EstilosUI.ColorEstadoReporte);
            ///Fila entera en rojo cuando Criticidad == CRITICA (secc. 8.1.b)
            UI.EstilosUI.AplicarFilaCritica(dgvReportes, "Criticidad", valor => valor.ToString().ToUpper() == "CRITICA");
            UI.EstilosUI.AplicarFilaCritica(dgvHistorial, "Criticidad", valor => valor.ToString().ToUpper() == "CRITICA");
            UI.EstilosUI.EstiloBotonPrimario(btnAsignarCriticidad);
            UI.EstilosUI.EstiloBotonPrimario(btnConfirmarModalidad);

            cmbCriticidad.DataSource = Enum.GetValues(typeof(BE.CriticidadReporteJP86));

            Idioma.GestorIdioma_64PR.GetInstance.Suscribir(this);

            textos = Idioma.GestorIdioma_64PR.GetInstance.ObtenerTextos();
            if (textos.Count > 0)
                ActualizarIdioma(textos);

            CargarReportesPendientes();
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

        private void FrmReportesPendientesJP86_Load(object sender, EventArgs e)
        {
            ConfigurarPermisos();
        }

        private void ConfigurarPermisos()
        {
            Sesion.Rol_64PR rolUsuario = Sesion.SessionManager.GetInstance.Usuario.Rol;
            bool puedeAsignarCriticidad = rolUsuario.TienePermiso(Sesion.Patentes_64PR.AsignarCriticidad);
            bool puedeDeterminarModalidad = rolUsuario.TienePermiso(Sesion.Patentes_64PR.DeterminarModalidad);

            cmbCriticidad.Enabled = puedeAsignarCriticidad;
            btnAsignarCriticidad.Enabled = puedeAsignarCriticidad && reporteSeleccionado != null;

            rbInterna.Enabled = puedeDeterminarModalidad;
            rbExterna.Enabled = puedeDeterminarModalidad;
            btnConfirmarModalidad.Enabled = puedeDeterminarModalidad;
        }

        private void CargarReportesPendientes()
        {
            List<BE.ReporteJP86> pendientes = greportes.ListarPendientes();

            dgvReportes.DataSource = null;
            dgvReportes.DataSource = pendientes;
            ///Rebindear regenera las columnas (autogeneradas) y pisa cualquier traduccion previa: hay que reaplicarla
            Traductor_64PR.TraducirGrilla(this, dgvReportes, textos);

            LimpiarSeleccion();

            if (pendientes.Count == 0)
                MessageBox.Show(textos["msg_sinReportesPendientes"]);
        }

        private void dgvReportes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                reporteSeleccionado = dgvReportes.SelectedRows[0].DataBoundItem as BE.ReporteJP86;

                dgvHistorial.DataSource = null;
                dgvHistorial.DataSource = greportes.ListarHistorialPorVehiculo(reporteSeleccionado.Vehiculo.Patente, reporteSeleccionado.NumeroReporte);
                ///Rebindear regenera las columnas (autogeneradas) y pisa cualquier traduccion previa: hay que reaplicarla
                Traductor_64PR.TraducirGrilla(this, dgvHistorial, textos);

                MostrarPasoCriticidad();

                Sesion.Rol_64PR rolUsuario = Sesion.SessionManager.GetInstance.Usuario.Rol;
                btnAsignarCriticidad.Enabled = rolUsuario.TienePermiso(Sesion.Patentes_64PR.AsignarCriticidad);
            }
        }

        private void MostrarPasoCriticidad()
        {
            lblCriticidad.Visible = true;
            cmbCriticidad.Visible = true;
            btnAsignarCriticidad.Visible = true;
            cmbCriticidad.SelectedIndex = 0;

            lblModalidad.Visible = false;
            rbInterna.Visible = false;
            rbExterna.Visible = false;
            btnConfirmarModalidad.Visible = false;
        }

        private void MostrarPasoModalidad()
        {
            lblCriticidad.Visible = false;
            cmbCriticidad.Visible = false;
            btnAsignarCriticidad.Visible = false;

            lblModalidad.Visible = true;
            rbInterna.Visible = true;
            rbExterna.Visible = true;
            btnConfirmarModalidad.Visible = true;
            rbInterna.Checked = true;
        }

        private void LimpiarSeleccion()
        {
            reporteSeleccionado = null;
            dgvHistorial.DataSource = null;

            lblCriticidad.Visible = false;
            cmbCriticidad.Visible = false;
            btnAsignarCriticidad.Visible = false;
            lblModalidad.Visible = false;
            rbInterna.Visible = false;
            rbExterna.Visible = false;
            btnConfirmarModalidad.Visible = false;
        }

        private void btnAsignarCriticidad_Click(object sender, EventArgs e)
        {
            if (reporteSeleccionado == null)
            {
                MessageBox.Show(textos["msg_seleccionarReporte"]);
                return;
            }

            BE.CriticidadReporteJP86 criticidad = (BE.CriticidadReporteJP86)cmbCriticidad.SelectedItem;

            try
            {
                greportes.AsignarCriticidad(reporteSeleccionado, criticidad);

                ev = new Bitacora.Evento_64PR(Sesion.SessionManager.GetInstance.Usuario.Login, ((int)Bitacora.Bitacora_64PR.ModuloBitacora_64PR.GestionMantenimiento).ToString(), ((int)Bitacora.Bitacora_64PR.TipoEventoBitacora_64PR.AsignacionCriticidad).ToString(), 2);
                bita.RegistrarEvento(ev);

                ///CUN-07 incluido siempre: pasa directo al paso de determinar modalidad, sin volver al menu
                MostrarPasoModalidad();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CargarReportesPendientes();
            }
        }

        private void btnConfirmarModalidad_Click(object sender, EventArgs e)
        {
            if (reporteSeleccionado == null)
            {
                MessageBox.Show(textos["msg_seleccionarReporte"]);
                return;
            }

            BE.ModalidadReparacionJP86 modalidad = rbInterna.Checked ? BE.ModalidadReparacionJP86.INTERNA : BE.ModalidadReparacionJP86.EXTERNA;

            try
            {
                greportes.DeterminarModalidad(reporteSeleccionado, modalidad);

                ev = new Bitacora.Evento_64PR(Sesion.SessionManager.GetInstance.Usuario.Login, ((int)Bitacora.Bitacora_64PR.ModuloBitacora_64PR.GestionMantenimiento).ToString(), ((int)Bitacora.Bitacora_64PR.TipoEventoBitacora_64PR.DeterminacionModalidad).ToString(), 3);
                bita.RegistrarEvento(ev);

                MessageBox.Show(textos["modalidad_determinada"]);

                CargarReportesPendientes();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CargarReportesPendientes();
            }
        }

        private void FrmReportesPendientesJP86_FormClosed(object sender, FormClosedEventArgs e)
        {
            Idioma.GestorIdioma_64PR.GetInstance.Desuscribir(this);
        }
    }
}
