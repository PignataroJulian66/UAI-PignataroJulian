using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace ProyectoIS_64PR
{
    ///CUN-08 "Registrar Acreditacion de Reparacion" + CUN-09 "Liberar Vehiculo" (incluido siempre en el paso 5 de CUN-08).
    public partial class FrmReparacionesEnCursoJP86 : Form, Idioma.IObservadorIdioma_64PR
    {
        Bitacora.Bitacora_64PR bita = new Bitacora.Bitacora_64PR();
        Bitacora.Evento_64PR ev;

        BLL_64PR.ReporteJP86 greportes = new BLL_64PR.ReporteJP86();
        public Dictionary<string, string> textos;

        BE.ReporteJP86 reporteSeleccionado;

        public FrmReparacionesEnCursoJP86()
        {
            InitializeComponent();

            Idioma.GestorIdioma_64PR.GetInstance.Suscribir(this);

            textos = Idioma.GestorIdioma_64PR.GetInstance.ObtenerTextos();
            if (textos.Count > 0)
                ActualizarIdioma(textos);

            CargarReportesEnReparacion();
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

        private void FrmReparacionesEnCursoJP86_Load(object sender, EventArgs e)
        {
            ConfigurarPermisos();
        }

        private void ConfigurarPermisos()
        {
            Sesion.Rol_64PR rolUsuario = Sesion.SessionManager.GetInstance.Usuario.Rol;
            bool puedeAcreditar = rolUsuario.TienePermiso(Sesion.Patentes_64PR.RegistrarAcreditacion);
            txtDescripcionCierre.Enabled = puedeAcreditar;
            btnAcreditar.Enabled = puedeAcreditar && reporteSeleccionado != null;
        }

        private void CargarReportesEnReparacion()
        {
            List<BE.ReporteJP86> enReparacion = greportes.ListarEnReparacion();

            dgvReportes.DataSource = null;
            dgvReportes.DataSource = enReparacion;

            LimpiarSeleccion();

            if (enReparacion.Count == 0)
                MessageBox.Show(textos["msg_sinReparacionesEnCurso"]);
        }

        private void dgvReportes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                reporteSeleccionado = dgvReportes.SelectedRows[0].DataBoundItem as BE.ReporteJP86;
                txtDescripcionCierre.Text = string.Empty;

                Sesion.Rol_64PR rolUsuario = Sesion.SessionManager.GetInstance.Usuario.Rol;
                btnAcreditar.Enabled = rolUsuario.TienePermiso(Sesion.Patentes_64PR.RegistrarAcreditacion);
            }
        }

        private void LimpiarSeleccion()
        {
            reporteSeleccionado = null;
            txtDescripcionCierre.Text = string.Empty;
            btnAcreditar.Enabled = false;
        }

        private void btnAcreditar_Click(object sender, EventArgs e)
        {
            if (reporteSeleccionado == null)
            {
                MessageBox.Show(textos["msg_seleccionarReporte"]);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtDescripcionCierre.Text))
            {
                MessageBox.Show(textos["msg_descripcionCierreValida"]);
                return;
            }

            try
            {
                string dniResponsable = Sesion.SessionManager.GetInstance.Usuario.DNI;

                greportes.AcreditarReparacion(reporteSeleccionado, txtDescripcionCierre.Text.Trim(), dniResponsable);

                ev = new Bitacora.Evento_64PR(Sesion.SessionManager.GetInstance.Usuario.Login, ((int)Bitacora.Bitacora_64PR.ModuloBitacora_64PR.GestionMantenimiento).ToString(), ((int)Bitacora.Bitacora_64PR.TipoEventoBitacora_64PR.AcreditacionReparacion).ToString(), 4);
                bita.RegistrarEvento(ev);

                ///CUN-09 "Liberar Vehiculo" -- incluido siempre, se registra como evento propio
                ev = new Bitacora.Evento_64PR(Sesion.SessionManager.GetInstance.Usuario.Login, ((int)Bitacora.Bitacora_64PR.ModuloBitacora_64PR.GestionMantenimiento).ToString(), ((int)Bitacora.Bitacora_64PR.TipoEventoBitacora_64PR.LiberacionVehiculo).ToString(), 4);
                bita.RegistrarEvento(ev);

                MessageBox.Show(textos["acreditacion_registrada"]);

                CargarReportesEnReparacion();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CargarReportesEnReparacion();
            }
        }

        private void FrmReparacionesEnCursoJP86_FormClosed(object sender, FormClosedEventArgs e)
        {
            Idioma.GestorIdioma_64PR.GetInstance.Desuscribir(this);
        }
    }
}
