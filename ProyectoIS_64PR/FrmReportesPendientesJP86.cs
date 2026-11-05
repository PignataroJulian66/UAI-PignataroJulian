using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace ProyectoIS_64PR
{
    ///CUN-06 "Asignar Criticidad" + CUN-07 "Determinar Modalidad Reparacion" (include obligatorio: mismo actor, mismo form, mismo momento).
    ///Una unica confirmacion registra criticidad + modalidad y pasa el reporte a EN_REPARACION (valor interno); si no se confirma, no se guarda nada.
    public partial class FrmReportesPendientesJP86 : Form, Idioma.IObservadorIdioma_64PR
    {
        #region Campos
        Bitacora.Bitacora_64PR bita = new Bitacora.Bitacora_64PR();
        Bitacora.Evento_64PR ev;

        BLL_64PR.ReporteJP86 greportes = new BLL_64PR.ReporteJP86();
        public Dictionary<string, string> textos;

        BE.ReporteJP86 reporteSeleccionado;
        #endregion

        #region Constructor
        public FrmReportesPendientesJP86()
        {
            InitializeComponent();
            ///Dos grillas apiladas: el alto libre se reparte entre reportes pendientes e historial de la unidad
            UI.EstilosUI.RepartirAltoEntreGrillas(this, dgvReportes, dgvHistorial, lblHistorialTitulo);

            this.Font = UI.TemaVisual.FuenteTexto;
            this.BackColor = UI.TemaVisual.FondoPagina;
            UI.EstilosUI.AplicarEstiloGrilla(dgvReportes);
            UI.EstilosUI.AplicarEstiloGrilla(dgvHistorial);
            UI.EstilosUI.AplicarBadgeColumna(dgvReportes, "Estado", UI.EstilosUI.ColorEstadoReporte);
            UI.EstilosUI.AplicarBadgeColumna(dgvHistorial, "Estado", UI.EstilosUI.ColorEstadoReporte);
            ///Fila entera en rojo cuando Criticidad == CRITICA (secc. 8.1.b)
            UI.EstilosUI.AplicarFilaCritica(dgvReportes, "Criticidad", valor => valor.ToString().ToUpper() == "CRITICA");
            UI.EstilosUI.AplicarFilaCritica(dgvHistorial, "Criticidad", valor => valor.ToString().ToUpper() == "CRITICA");
            UI.EstilosUI.EstiloBotonPrimario(btnConfirmar);
            ///Estado/Fuente/Criticidad/Modalidad se muestran traducidos; los valores de las celdas siguen siendo los enums
            Traductor_64PR.TraducirColumnasReporte(dgvReportes, () => textos);
            Traductor_64PR.TraducirColumnasReporte(dgvHistorial, () => textos);

            CargarCriticidades();

            Idioma.GestorIdioma_64PR.GetInstance.Suscribir(this);

            textos = Idioma.GestorIdioma_64PR.GetInstance.ObtenerTextos();
            if (textos.Count > 0)
                ActualizarIdioma(textos);

            CargarReportesPendientes();
        }
        #endregion

        #region Idioma (Observer)
        public void ActualizarIdioma(Dictionary<string, string> textoss)
        {
            textos = textoss;
            Traductor_64PR.Traducir(this, textos);
            CargarCriticidades();
            dgvReportes.Invalidate();
            dgvHistorial.Invalidate();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            Idioma.GestorIdioma_64PR.GetInstance.Desuscribir(this);
            base.OnFormClosed(e);
        }
        #endregion

        #region Permisos
        ///CUN-07 es include obligatorio de CUN-06: hacen falta AMBAS patentes para clasificar un reporte
        private bool PuedeClasificar()
        {
            Sesion.Rol_64PR rolUsuario = Sesion.SessionManager.GetInstance.Usuario.Rol;
            return rolUsuario.TienePermiso(Sesion.Patentes_64PR.AsignarCriticidad) &&
                   rolUsuario.TienePermiso(Sesion.Patentes_64PR.DeterminarModalidad);
        }

        private void ConfigurarPermisos()
        {
            bool puedeClasificar = PuedeClasificar();

            cmbCriticidad.Enabled = puedeClasificar;
            rbInterna.Enabled = puedeClasificar;
            rbExterna.Enabled = puedeClasificar;
            btnConfirmar.Enabled = puedeClasificar && reporteSeleccionado != null;
        }
        #endregion

        #region Carga de datos
        ///El combo muestra el texto traducido ("criticidadReporte_<valor>") y guarda el enum como SelectedValue
        private void CargarCriticidades()
        {
            Traductor_64PR.CargarComboValores<BE.CriticidadReporteJP86>(cmbCriticidad, textos, Traductor_64PR.PrefijoCriticidadReporte);
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
        #endregion

        #region Eventos
        private void FrmReportesPendientesJP86_Load(object sender, EventArgs e)
        {
            ConfigurarPermisos();
        }

        private void dgvReportes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                ///Cambiar de fila sin confirmar descarta lo elegido: no se guardo nada todavia
                reporteSeleccionado = dgvReportes.SelectedRows[0].DataBoundItem as BE.ReporteJP86;

                dgvHistorial.DataSource = null;
                dgvHistorial.DataSource = greportes.ListarHistorialPorVehiculo(reporteSeleccionado.Vehiculo.Patente, reporteSeleccionado.NumeroReporte);
                ///Rebindear regenera las columnas (autogeneradas) y pisa cualquier traduccion previa: hay que reaplicarla
                Traductor_64PR.TraducirGrilla(this, dgvHistorial, textos);

                MostrarClasificacion();

                btnConfirmar.Enabled = PuedeClasificar();
            }
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            if (reporteSeleccionado == null)
            {
                MessageBox.Show(textos["msg_seleccionarReporte"]);
                return;
            }

            BE.CriticidadReporteJP86 criticidad = (BE.CriticidadReporteJP86)cmbCriticidad.SelectedValue;
            BE.ModalidadReparacionJP86 modalidad = rbInterna.Checked ? BE.ModalidadReparacionJP86.INTERNA : BE.ModalidadReparacionJP86.EXTERNA;

            try
            {
                greportes.AsignarCriticidadYModalidad(reporteSeleccionado, criticidad, modalidad);

                ///Solo si la operacion fue exitosa: un evento por cada caso de uso (CUN-06 y CUN-07)
                ev = new Bitacora.Evento_64PR(Sesion.SessionManager.GetInstance.Usuario.Login, ((int)Bitacora.Bitacora_64PR.ModuloBitacora_64PR.GestionMantenimiento).ToString(), ((int)Bitacora.Bitacora_64PR.TipoEventoBitacora_64PR.AsignacionCriticidad).ToString(), 2);
                bita.RegistrarEvento(ev);

                ev = new Bitacora.Evento_64PR(Sesion.SessionManager.GetInstance.Usuario.Login, ((int)Bitacora.Bitacora_64PR.ModuloBitacora_64PR.GestionMantenimiento).ToString(), ((int)Bitacora.Bitacora_64PR.TipoEventoBitacora_64PR.DeterminacionModalidad).ToString(), 3);
                bita.RegistrarEvento(ev);

                MessageBox.Show(textos["reporte_clasificado"]);

                CargarReportesPendientes();
            }
            catch (InvalidOperationException ex)
            {
                ///Regla de negocio (clave de idioma lanzada por la BLL/Mapper): no se registra evento
                MessageBox.Show(Traductor_64PR.TraducirMensaje(textos, ex.Message), textos["titulo_Validacion"], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CargarReportesPendientes();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Metodos privados
        ///Criticidad (CUN-06) y modalidad (CUN-07) se muestran juntas y se confirman una sola vez
        private void MostrarClasificacion()
        {
            lblCriticidad.Visible = true;
            cmbCriticidad.Visible = true;
            cmbCriticidad.SelectedIndex = 0;

            lblModalidad.Visible = true;
            rbInterna.Visible = true;
            rbExterna.Visible = true;
            rbInterna.Checked = true;

            btnConfirmar.Visible = true;
        }

        private void LimpiarSeleccion()
        {
            reporteSeleccionado = null;
            dgvHistorial.DataSource = null;

            lblCriticidad.Visible = false;
            cmbCriticidad.Visible = false;
            lblModalidad.Visible = false;
            rbInterna.Visible = false;
            rbExterna.Visible = false;
            btnConfirmar.Visible = false;
        }
        #endregion
    }
}
