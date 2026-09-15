using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace ProyectoIS_64PR
{
    public partial class FrmRegistrarDevolucionJP86 : Form, Idioma.IObservadorIdioma_64PR
    {
        Bitacora.Bitacora_64PR bita = new Bitacora.Bitacora_64PR();
        Bitacora.Evento_64PR ev;

        BLL_64PR.ContratoJP86 gcontratos = new BLL_64PR.ContratoJP86();
        public Dictionary<string, string> textos;

        List<BE.ContratoJP86> lst;
        BE.ContratoJP86 contratoSeleccionado;

        public FrmRegistrarDevolucionJP86()
        {
            InitializeComponent();

            this.Font = UI.TemaVisual.FuenteTexto;
            this.BackColor = UI.TemaVisual.FondoPagina;
            UI.EstilosUI.AplicarEstiloGrilla(dgvContratos);
            UI.EstilosUI.AplicarBadgeColumna(dgvContratos, "Estado", UI.EstilosUI.ColorEstadoContrato);
            UI.EstilosUI.EstiloBotonPrimario(btnRegistrarDevolucion);
            UI.EstilosUI.EstiloBotonSecundario(btnImprimirRecibo);

            dgvContratos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            cmbEstadoUnidad.DataSource = Enum.GetValues(typeof(BE.EstadoUnidadDevolucionJP86));

            CargaData();

            btnRegistrarDevolucion.Enabled = false;

            Idioma.GestorIdioma_64PR.GetInstance.Suscribir(this);

            textos = Idioma.GestorIdioma_64PR.GetInstance.ObtenerTextos();
            if (textos.Count > 0)
                ActualizarIdioma(textos);
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

        private void FrmRegistrarDevolucionJP86_Load(object sender, EventArgs e)
        {
            ConfigurarPermisos();
        }

        private void ConfigurarPermisos()
        {
            Sesion.Rol_64PR rolUsuario = Sesion.SessionManager.GetInstance.Usuario.Rol;
            bool puedeRegistrar = rolUsuario.TienePermiso(Sesion.Patentes_64PR.RegistrarDevolucion);
            numKilometrajeRetorno.Enabled = puedeRegistrar;
            cmbEstadoUnidad.Enabled = puedeRegistrar;
        }

        private void CargaData()
        {
            lst = gcontratos.ListarFacturados();
            AplicarFiltros();
        }

        private void AplicarFiltros()
        {
            if (lst == null) return;

            IEnumerable<BE.ContratoJP86> filtrado = lst;

            if (!string.IsNullOrWhiteSpace(txtBuscar.Text))
            {
                string texto = txtBuscar.Text.Trim();
                filtrado = filtrado.Where(c =>
                    c.NumeroContrato.ToString().IndexOf(texto, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    c.Vehiculo.Patente.IndexOf(texto, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            List<BE.ContratoJP86> resultado = filtrado.ToList();

            dgvContratos.DataSource = null;
            dgvContratos.DataSource = resultado;
            ///Rebindear regenera las columnas (autogeneradas) y pisa cualquier traduccion previa: hay que reaplicarla
            Traductor_64PR.TraducirGrilla(this, dgvContratos, textos);

            if (dgvContratos.Columns.Contains("TarifaDiaria")) dgvContratos.Columns["TarifaDiaria"].Visible = false;
            if (dgvContratos.Columns.Contains("CargosAdicionales")) dgvContratos.Columns["CargosAdicionales"].Visible = false;
            if (dgvContratos.Columns.Contains("ImporteTotal")) dgvContratos.Columns["ImporteTotal"].Visible = false;
            ///KilometrajeEntrega queda visible: referencia para comparar contra el km de retorno a ingresar
            if (dgvContratos.Columns.Contains("KilometrajeRetorno")) dgvContratos.Columns["KilometrajeRetorno"].Visible = false;
            if (dgvContratos.Columns.Contains("EstadoUnidadDevolucion")) dgvContratos.Columns["EstadoUnidadDevolucion"].Visible = false;
            if (dgvContratos.Columns.Contains("Estado")) dgvContratos.Columns["Estado"].Visible = false;

            LimpiarSeleccion();
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            AplicarFiltros();
        }

        private void dgvContratos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                contratoSeleccionado = dgvContratos.SelectedRows[0].DataBoundItem as BE.ContratoJP86;
                numKilometrajeRetorno.Minimum = contratoSeleccionado.KilometrajeEntrega;
                numKilometrajeRetorno.Value = contratoSeleccionado.KilometrajeEntrega;
                cmbEstadoUnidad.SelectedIndex = 0;

                Sesion.Rol_64PR rolUsuario = Sesion.SessionManager.GetInstance.Usuario.Rol;
                btnRegistrarDevolucion.Enabled = rolUsuario.TienePermiso(Sesion.Patentes_64PR.RegistrarDevolucion);
                btnImprimirRecibo.Enabled = true;
            }
        }

        private void LimpiarSeleccion()
        {
            contratoSeleccionado = null;
            btnRegistrarDevolucion.Enabled = false;
            btnImprimirRecibo.Enabled = false;
            numKilometrajeRetorno.Value = numKilometrajeRetorno.Minimum;
        }

        private void btnImprimirRecibo_Click(object sender, EventArgs e)
        {
            if (contratoSeleccionado == null)
            {
                MessageBox.Show(textos["seleccionar_contrato"]);
                return;
            }

            Documentos.GeneradorReciboContrato.Imprimir(contratoSeleccionado);
        }

        private void btnRegistrarDevolucion_Click(object sender, EventArgs e)
        {
            if (contratoSeleccionado == null)
            {
                MessageBox.Show(textos["seleccionar_contrato"]);
                return;
            }

            int kilometrajeRetorno = (int)numKilometrajeRetorno.Value;
            if (kilometrajeRetorno < contratoSeleccionado.KilometrajeEntrega)
            {
                MessageBox.Show(textos["msg_KilometrajeRetornoValido"]);
                return;
            }

            BE.EstadoUnidadDevolucionJP86 estadoUnidad = (BE.EstadoUnidadDevolucionJP86)cmbEstadoUnidad.SelectedItem;

            try
            {
                gcontratos.RegistrarDevolucion(contratoSeleccionado, kilometrajeRetorno, estadoUnidad);

                ev = new Bitacora.Evento_64PR(Sesion.SessionManager.GetInstance.Usuario.Login, ((int)Bitacora.Bitacora_64PR.ModuloBitacora_64PR.GestionAlquileres).ToString(), ((int)Bitacora.Bitacora_64PR.TipoEventoBitacora_64PR.DevolucionVehiculo).ToString(), 3);
                bita.RegistrarEvento(ev);

                MessageBox.Show(textos["devolucion_registrada"]);

                CargaData();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CargaData();
            }
        }

        private void FrmRegistrarDevolucionJP86_FormClosed(object sender, FormClosedEventArgs e)
        {
            Idioma.GestorIdioma_64PR.GetInstance.Desuscribir(this);
        }
    }
}
