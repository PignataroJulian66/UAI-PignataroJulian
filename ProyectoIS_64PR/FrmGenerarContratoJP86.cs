using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ProyectoIS_64PR
{
    public partial class FrmGenerarContratoJP86 : Form, Idioma.IObservadorIdioma_64PR
    {
        Bitacora.Bitacora_64PR bita = new Bitacora.Bitacora_64PR();
        Bitacora.Evento_64PR ev;

        BLL_64PR.ContratoJP86 gcontratos = new BLL_64PR.ContratoJP86();
        BLL_64PR.Categoria_64PR gcategorias = new BLL_64PR.Categoria_64PR();
        BLL_64PR.ClienteJP86 gclientes = new BLL_64PR.ClienteJP86();
        public Dictionary<string, string> textos;

        private static readonly Regex RegexDNI = new Regex(@"^\d{7,8}$");
        private static readonly Regex RegexTelefono = new Regex(@"^(\+?54)?0?\d{8,11}$");

        BE.VehiculoJP86 unidadSeleccionada;
        BE.ClienteJP86 clienteSeleccionado;
        BE.ContratoJP86 contratoGenerado;
        ucCrearClienteJP86 ucNuevoCliente;

        public FrmGenerarContratoJP86()
        {
            InitializeComponent();

            this.Font = UI.TemaVisual.FuenteTexto;
            this.BackColor = UI.TemaVisual.FondoPagina;
            UI.EstilosUI.AplicarEstiloGrilla(dgvUnidades);
            UI.EstilosUI.AplicarEstiloGrilla(dgvClientes);
            UI.EstilosUI.AplicarBadgeColumna(dgvUnidades, "Estado", UI.EstilosUI.ColorEstadoVehiculo);
            UI.EstilosUI.EstiloBotonPrimario(btnConfirmar);
            UI.EstilosUI.EstiloBotonPrimario(btnGuardarCliente);
            UI.EstilosUI.EstiloBotonSecundario(btnBuscarUnidades);
            UI.EstilosUI.EstiloBotonSecundario(btnClienteNuevo);
            UI.EstilosUI.EstiloBotonSecundario(btnImprimirRecibo);
            UI.EstilosUI.EstiloBotonPeligro(btnCancelar);

            ucNuevoCliente = new ucCrearClienteJP86();
            ucNuevoCliente.Dock = DockStyle.Fill;
            pnlNuevoCliente.Controls.Add(ucNuevoCliente);

            cmbCategoria.Items.Add("(Todas)");
            foreach (var cat in gcategorias.Listar().Where(c => c.Activo))
                cmbCategoria.Items.Add(cat);
            cmbCategoria.SelectedIndex = 0;

            dtpFechaInicio.Value = DateTime.Now;
            dtpFechaFin.Value = DateTime.Now.AddDays(1);
            dtpFechaInicio.ValueChanged += (s, e) => ActualizarImporte();
            dtpFechaFin.ValueChanged += (s, e) => ActualizarImporte();

            ///Los DataGridView se bindean ANTES de traducir: Traductor_64PR recorre dgv.Columns,
            ///y esas columnas recien existen (autogeneradas) despues de asignar el DataSource.
            ///Sin filtros aplicados todavia: la grilla arranca mostrando TODAS las unidades disponibles
            CargarUnidadesDisponibles(false);
            CargarClientes();

            Idioma.GestorIdioma_64PR.GetInstance.Suscribir(this);

            textos = Idioma.GestorIdioma_64PR.GetInstance.ObtenerTextos();
            if (textos.Count > 0)
                ActualizarIdioma(textos);
        }

        public void ActualizarIdioma(Dictionary<string, string> textoss)
        {
            textos = textoss;
            Traductor_64PR.Traducir(this, textos);

            ///chkCargosAdicionales no es traducido automaticamente por Traductor_64PR (no soporta CheckBox)
            chkCargosAdicionales.Text = textos["FrmGenerarContratoJP86.chkCargosAdicionales"];

            ///lblClienteInfo y lblImporteTotal muestran contenido dinamico: se recalculan despues de traducir
            if (clienteSeleccionado != null)
                lblClienteInfo.Text = clienteSeleccionado.Apellido + ", " + clienteSeleccionado.Nombre;
            ActualizarImporte();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            Idioma.GestorIdioma_64PR.GetInstance.Desuscribir(this);
            base.OnFormClosed(e);
        }

        private void FrmGenerarContratoJP86_Load(object sender, EventArgs e)
        {
            ConfigurarPermisos();
        }

        private void ConfigurarPermisos()
        {
            Sesion.Rol_64PR rolUsuario = Sesion.SessionManager.GetInstance.Usuario.Rol;
            btnConfirmar.Enabled = rolUsuario.TienePermiso(Sesion.Patentes_64PR.GenerarContrato);
        }

        private void btnBuscarUnidades_Click(object sender, EventArgs e)
        {
            CargarUnidadesDisponibles(true);
        }

        private void CargarUnidadesDisponibles(bool mostrarMensajeSiVacio)
        {
            int? idCategoria = cmbCategoria.SelectedIndex <= 0 ? (int?)null : ((BE.Categoria_64PR)cmbCategoria.SelectedItem).Id;
            string marca = string.IsNullOrWhiteSpace(txtMarca.Text) ? null : txtMarca.Text.Trim();
            int? kmMaximo = numKilometrajeMax.Value <= 0 ? (int?)null : (int)numKilometrajeMax.Value;

            List<BE.VehiculoJP86> disponibles = gcontratos.BuscarUnidadesDisponibles(idCategoria, marca, kmMaximo);

            unidadSeleccionada = null;
            ActualizarImporte();

            dgvUnidades.DataSource = null;
            dgvUnidades.DataSource = disponibles;
            ///Rebindear regenera las columnas (autogeneradas) y pisa cualquier traduccion previa: hay que reaplicarla
            Traductor_64PR.TraducirGrilla(this, dgvUnidades, textos);

            if (mostrarMensajeSiVacio && disponibles.Count == 0)
                MessageBox.Show(textos["msg_sinUnidadesDisponibles"]);
        }

        private void dgvUnidades_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                unidadSeleccionada = dgvUnidades.SelectedRows[0].DataBoundItem as BE.VehiculoJP86;
                ActualizarImporte();
            }
        }

        private void CargarClientes()
        {
            dgvClientes.DataSource = null;
            dgvClientes.DataSource = gclientes.Listar();
            ///Rebindear regenera las columnas (autogeneradas) y pisa cualquier traduccion previa: hay que reaplicarla
            Traductor_64PR.TraducirGrilla(this, dgvClientes, textos);
        }

        private void dgvClientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                clienteSeleccionado = dgvClientes.SelectedRows[0].DataBoundItem as BE.ClienteJP86;
                lblClienteInfo.Text = clienteSeleccionado.Apellido + ", " + clienteSeleccionado.Nombre;
                pnlNuevoCliente.Visible = false;
                btnGuardarCliente.Visible = false;
            }
        }

        private void btnClienteNuevo_Click(object sender, EventArgs e)
        {
            clienteSeleccionado = null;
            lblClienteInfo.Text = string.Empty;
            dgvClientes.ClearSelection();
            pnlNuevoCliente.Visible = true;
            btnGuardarCliente.Visible = true;
        }

        private void btnGuardarCliente_Click(object sender, EventArgs e)
        {
            if (!RegexDNI.IsMatch(ucNuevoCliente.DNI()))
            {
                MessageBox.Show(textos["msg_DNIValido"]);
                return;
            }
            if (string.IsNullOrWhiteSpace(ucNuevoCliente.Nombre()))
            {
                MessageBox.Show(textos["msg_NombreValido"]);
                return;
            }
            if (string.IsNullOrWhiteSpace(ucNuevoCliente.Apellido()))
            {
                MessageBox.Show(textos["msg_ApellidoValido"]);
                return;
            }
            string telefonoNormalizado = Regex.Replace(ucNuevoCliente.Telefono(), @"[\s\-]", "");
            if (!RegexTelefono.IsMatch(telefonoNormalizado))
            {
                MessageBox.Show(textos["msg_TelefonoValido"]);
                return;
            }

            try
            {
                BE.ClienteJP86 nuevoCliente = new BE.ClienteJP86()
                {
                    DNI = ucNuevoCliente.DNI(),
                    Nombre = ucNuevoCliente.Nombre(),
                    Apellido = ucNuevoCliente.Apellido(),
                    Telefono = ucNuevoCliente.Telefono()
                };

                gclientes.Crear(nuevoCliente);

                ev = new Bitacora.Evento_64PR(Sesion.SessionManager.GetInstance.Usuario.Login, ((int)Bitacora.Bitacora_64PR.ModuloBitacora_64PR.GestionClientes).ToString(), ((int)Bitacora.Bitacora_64PR.TipoEventoBitacora_64PR.AltaCliente).ToString(), 4);
                bita.RegistrarEvento(ev);

                clienteSeleccionado = nuevoCliente;
                lblClienteInfo.Text = nuevoCliente.Apellido + ", " + nuevoCliente.Nombre;
                pnlNuevoCliente.Visible = false;
                btnGuardarCliente.Visible = false;
                ucNuevoCliente.LimpiarCampos();

                CargarClientes();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    MessageBox.Show(textos["msg_DNIDuplicado"], "DNI", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void chkCargosAdicionales_CheckedChanged(object sender, EventArgs e)
        {
            ActualizarImporte();
        }

        private void ActualizarImporte()
        {
            if (unidadSeleccionada == null || textos == null)
            {
                lblImporteTotal.Text = (textos != null ? textos["contrato_lblImporteTotal"] : "Importe total: ") + "$0";
                return;
            }

            decimal importe = gcontratos.CalcularImporteTotal(unidadSeleccionada.Categoria.TarifaDiaria, dtpFechaInicio.Value, dtpFechaFin.Value, chkCargosAdicionales.Checked);
            lblImporteTotal.Text = textos["contrato_lblImporteTotal"] + "$" + importe.ToString("N2");
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            if (dtpFechaFin.Value.Date < dtpFechaInicio.Value.Date)
            {
                MessageBox.Show(textos["msg_FechasValidas"]);
                return;
            }
            if (unidadSeleccionada == null)
            {
                MessageBox.Show(textos["msg_seleccionarUnidad"]);
                return;
            }
            if (clienteSeleccionado == null)
            {
                MessageBox.Show(textos["msg_seleccionarCliente"]);
                return;
            }

            try
            {
                BE.ContratoJP86 contrato = gcontratos.GenerarContrato(clienteSeleccionado, unidadSeleccionada, dtpFechaInicio.Value.Date, dtpFechaFin.Value.Date, chkCargosAdicionales.Checked);

                ev = new Bitacora.Evento_64PR(Sesion.SessionManager.GetInstance.Usuario.Login, ((int)Bitacora.Bitacora_64PR.ModuloBitacora_64PR.GestionAlquileres).ToString(), ((int)Bitacora.Bitacora_64PR.TipoEventoBitacora_64PR.AltaContrato).ToString(), 3);
                bita.RegistrarEvento(ev);

                MessageBox.Show(textos["contrato_generado"] + contrato.NumeroContrato.ToString());

                LimpiarFormulario();

                contratoGenerado = contrato;
                btnImprimirRecibo.Enabled = true;
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        private void btnImprimirRecibo_Click(object sender, EventArgs e)
        {
            Documentos.GeneradorReciboContrato.Imprimir(contratoGenerado);
        }

        private void LimpiarFormulario()
        {
            unidadSeleccionada = null;
            clienteSeleccionado = null;
            contratoGenerado = null;
            btnImprimirRecibo.Enabled = false;

            lblClienteInfo.Text = string.Empty;
            pnlNuevoCliente.Visible = false;
            btnGuardarCliente.Visible = false;
            ucNuevoCliente.LimpiarCampos();
            dgvClientes.ClearSelection();

            chkCargosAdicionales.Checked = false;
            cmbCategoria.SelectedIndex = 0;
            txtMarca.Text = string.Empty;
            numKilometrajeMax.Value = 0;
            dtpFechaInicio.Value = DateTime.Now;
            dtpFechaFin.Value = DateTime.Now.AddDays(1);

            CargarUnidadesDisponibles(false);
            CargarClientes();

            ActualizarImporte();
        }

        private void FrmGenerarContratoJP86_FormClosed(object sender, FormClosedEventArgs e)
        {
            Idioma.GestorIdioma_64PR.GetInstance.Desuscribir(this);
        }
    }
}
