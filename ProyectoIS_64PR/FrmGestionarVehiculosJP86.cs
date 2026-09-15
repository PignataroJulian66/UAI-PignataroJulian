using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoIS_64PR
{
    public partial class FrmGestionarVehiculosJP86 : Form, Idioma.IObservadorIdioma_64PR
    {
        Bitacora.Bitacora_64PR bita = new Bitacora.Bitacora_64PR();
        Bitacora.Evento_64PR ev;

        BLL_64PR.VehiculoJP86 gvehiculos = new BLL_64PR.VehiculoJP86();
        public Dictionary<string, string> textos;

        string modo = "consulta";
        UserControl uc;
        List<BE.VehiculoJP86> lst;

        private static readonly Regex RegexPatente = new Regex(@"^([A-Z]{3}\d{3}|[A-Z]{2}\d{3}[A-Z]{2})$");

        public FrmGestionarVehiculosJP86()
        {
            InitializeComponent();

            this.Font = UI.TemaVisual.FuenteTexto;
            this.BackColor = UI.TemaVisual.FondoPagina;
            UI.EstilosUI.AplicarTarjeta(pnlContenedor);
            UI.EstilosUI.EstiloBotonPrimario(btnCrear);
            UI.EstilosUI.EstiloBotonPrimario(btnGuardar);
            UI.EstilosUI.EstiloBotonSecundario(btnModificar);
            UI.EstilosUI.EstiloBotonSecundario(btnCambiarEstado);
            UI.EstilosUI.EstiloBotonPeligro(btnEliminar);

            radioButton3.Checked = true;
            dgvVehiculos.ReadOnly = true;
            dgvVehiculos.MultiSelect = false;
            dgvVehiculos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvVehiculos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvVehiculos.BackgroundColor = SystemColors.Menu;
            dgvVehiculos.BorderStyle = BorderStyle.None;
            UI.EstilosUI.AplicarEstiloGrilla(dgvVehiculos);
            UI.EstilosUI.AplicarBadgeColumna(dgvVehiculos, "Estado", UI.EstilosUI.ColorEstadoVehiculo);

            cmbEstado.DataSource = Enum.GetValues(typeof(BE.EstadoVehiculoJP86));

            CargaData();

            btnGuardar.Enabled = false;
            btnCambiarEstado.Enabled = false;

            Idioma.GestorIdioma_64PR.GetInstance.Suscribir(this);

            textos = Idioma.GestorIdioma_64PR.GetInstance.ObtenerTextos();
            if (textos.Count > 0)
                ActualizarIdioma(textos);
            lblModo.Text = textos["frmGestionVehiculos_lblModoConsulta"];
        }

        public void ActualizarIdioma(Dictionary<string, string> textoss)
        {
            textos = textoss;
            string[] aux = lblCantidad.Text.Split(':');

            Traductor_64PR.Traducir(this, textos);
            if (aux.Length > 1)
                lblCantidad.Text += aux[1];

            switch (modo)
            {
                case "consulta":
                    lblModo.Text = textos["frmGestionVehiculos_lblModoConsulta"];
                    break;
                case "crear":
                    lblModo.Text = textos["frmGestionVehiculos_lblModoCrear"];
                    break;
                case "modificar":
                    lblModo.Text = textos["frmGestionVehiculos_lblModoModificar"];
                    break;
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            Idioma.GestorIdioma_64PR.GetInstance.Desuscribir(this);
            base.OnFormClosed(e);
        }

        private void CargaData()
        {
            lst = gvehiculos.Listar();
            AplicarFiltros();
        }

        private void AplicarFiltros()
        {
            if (lst == null) return;

            IEnumerable<BE.VehiculoJP86> filtrado = lst;

            ///radioButton1 = "No activos", radioButton2 = "Activos" (misma convencion que Categorias)
            if (radioButton1.Checked)
                filtrado = filtrado.Where(v => v.Activo == false);
            else if (radioButton2.Checked)
                filtrado = filtrado.Where(v => v.Activo == true);

            if (!string.IsNullOrWhiteSpace(txtBuscar.Text))
            {
                string texto = txtBuscar.Text.Trim();
                filtrado = filtrado.Where(v =>
                    v.Patente.IndexOf(texto, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    v.Marca.IndexOf(texto, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    v.Modelo.IndexOf(texto, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            List<BE.VehiculoJP86> resultado = filtrado.ToList();

            dgvVehiculos.DataSource = null;
            dgvVehiculos.DataSource = resultado;
            ///Rebindear regenera las columnas (autogeneradas) y pisa cualquier traduccion previa: hay que reaplicarla
            Traductor_64PR.TraducirGrilla(this, dgvVehiculos, textos);

            if (textos != null && textos.Count > 0)
                lblCantidad.Text = textos["frmGestionVehiculos_lblCantidad"] + resultado.Count.ToString();
        }

        private void btnCrear_Click(object sender, EventArgs e)
        {
            modo = "crear";
            lblModo.Text = textos["frmGestionVehiculos_lblModoCrear"];
            uc = new ucCrearVehiculoJP86();
            pnlContenedor.Controls.Clear();
            uc.Dock = DockStyle.Fill;
            pnlContenedor.Controls.Add(uc);
            btnGuardar.Enabled = true;
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            modo = "modificar";
            lblModo.Text = textos["frmGestionVehiculos_lblModoModificar"];
            uc = new ucModificarVehiculoJP86();
            pnlContenedor.Controls.Clear();
            uc.Dock = DockStyle.Fill;
            pnlContenedor.Controls.Add(uc);
            btnGuardar.Enabled = true;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            switch (modo)
            {
                case "consulta":
                    MessageBox.Show(textos["msg_NoCambios"]);
                    break;

                case "crear":
                    if (uc is ucCrearVehiculoJP86 ucc)
                    {
                        if (!RegexPatente.IsMatch(ucc.Patente()))
                        {
                            MessageBox.Show(textos["msg_PatenteValida"]);
                            return;
                        }
                        if (string.IsNullOrWhiteSpace(ucc.Marca()))
                        {
                            MessageBox.Show(textos["msg_MarcaValida"]);
                            return;
                        }
                        if (string.IsNullOrWhiteSpace(ucc.Modelo()))
                        {
                            MessageBox.Show(textos["msg_ModeloValida"]);
                            return;
                        }
                        if (ucc.Categoria() == null)
                        {
                            MessageBox.Show(textos["msg_CategoriaValida"]);
                            return;
                        }
                        try
                        {
                            BE.VehiculoJP86 v = new BE.VehiculoJP86()
                            {
                                Patente = ucc.Patente(),
                                Marca = ucc.Marca(),
                                Modelo = ucc.Modelo(),
                                Categoria = ucc.Categoria(),
                                Kilometraje = ucc.Kilometraje()
                            };

                            gvehiculos.Crear(v);

                            ev = new Bitacora.Evento_64PR(Sesion.SessionManager.GetInstance.Usuario.Login, ((int)Bitacora.Bitacora_64PR.ModuloBitacora_64PR.GestionVehiculos).ToString(), ((int)Bitacora.Bitacora_64PR.TipoEventoBitacora_64PR.AltaVehiculo).ToString(), 3);
                            bita.RegistrarEvento(ev);

                            MessageBox.Show(textos["vehiculo_creado"]);

                            CargaData();
                            ucc.LimpiarCampos();
                            pnlContenedor.Controls.Clear();
                            uc = null;
                            modo = "consulta";
                            lblModo.Text = textos["frmGestionVehiculos_lblModoConsulta"];
                            btnGuardar.Enabled = false;
                        }
                        catch (SqlException ex)
                        {
                            if (ex.Number == 2627 || ex.Number == 2601)
                            {
                                MessageBox.Show(textos["msg_PatenteDuplicada"],
                                    "Patente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                            {
                                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    break;

                case "modificar":
                    if (uc is ucModificarVehiculoJP86 ucm)
                    {
                        if (string.IsNullOrWhiteSpace(ucm.Marca()))
                        {
                            MessageBox.Show(textos["msg_MarcaValida"]);
                            return;
                        }
                        if (string.IsNullOrWhiteSpace(ucm.Modelo()))
                        {
                            MessageBox.Show(textos["msg_ModeloValida"]);
                            return;
                        }
                        if (ucm.Categoria() == null)
                        {
                            MessageBox.Show(textos["msg_CategoriaValida"]);
                            return;
                        }
                        if (dgvVehiculos.SelectedRows.Count == 0)
                        {
                            MessageBox.Show(textos["seleccionar_vehiculo"]);
                            return;
                        }
                        try
                        {
                            BE.VehiculoJP86 v = dgvVehiculos.SelectedRows[0].DataBoundItem as BE.VehiculoJP86;
                            v.Marca = ucm.Marca();
                            v.Modelo = ucm.Modelo();
                            v.Categoria = ucm.Categoria();
                            v.Kilometraje = ucm.Kilometraje();

                            gvehiculos.Modificar(v);

                            ev = new Bitacora.Evento_64PR(Sesion.SessionManager.GetInstance.Usuario.Login, ((int)Bitacora.Bitacora_64PR.ModuloBitacora_64PR.GestionVehiculos).ToString(), ((int)Bitacora.Bitacora_64PR.TipoEventoBitacora_64PR.ModificacionVehiculo).ToString(), 4);
                            bita.RegistrarEvento(ev);

                            MessageBox.Show(textos["vehiculo_modificado"]);

                            CargaData();
                            ucm.LimpiarCampos();
                            pnlContenedor.Controls.Clear();
                            uc = null;
                            modo = "consulta";
                            lblModo.Text = textos["frmGestionVehiculos_lblModoConsulta"];
                            btnGuardar.Enabled = false;
                        }
                        catch (SqlException ex)
                        {
                            MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    break;
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvVehiculos.SelectedRows.Count == 0)
            {
                MessageBox.Show(textos["seleccionar_vehiculo"]);
                return;
            }

            BE.VehiculoJP86 v = dgvVehiculos.SelectedRows[0].DataBoundItem as BE.VehiculoJP86;

            bool vaAQuedarActivo = !v.Activo;

            gvehiculos.ActDesact(v);

            string tipoEvento = vaAQuedarActivo
                ? ((int)Bitacora.Bitacora_64PR.TipoEventoBitacora_64PR.AltaVehiculo).ToString()
                : ((int)Bitacora.Bitacora_64PR.TipoEventoBitacora_64PR.BajaVehiculo).ToString();
            ev = new Bitacora.Evento_64PR(Sesion.SessionManager.GetInstance.Usuario.Login, ((int)Bitacora.Bitacora_64PR.ModuloBitacora_64PR.GestionVehiculos).ToString(), tipoEvento, vaAQuedarActivo ? 3 : 2);
            bita.RegistrarEvento(ev);

            MessageBox.Show(textos["operacion exitosa"]);
            CargaData();
            btnEliminar.Text = textos["FrmGestionarVehiculosJP86.btnEliminar"];
        }

        private void btnCambiarEstado_Click(object sender, EventArgs e)
        {
            if (dgvVehiculos.SelectedRows.Count == 0)
            {
                MessageBox.Show(textos["seleccionar_vehiculo"]);
                return;
            }

            BE.VehiculoJP86 v = dgvVehiculos.SelectedRows[0].DataBoundItem as BE.VehiculoJP86;
            BE.EstadoVehiculoJP86 nuevoEstado = (BE.EstadoVehiculoJP86)cmbEstado.SelectedItem;

            if (nuevoEstado == v.Estado)
            {
                MessageBox.Show(textos["msg_NoCambios"]);
                return;
            }

            gvehiculos.CambiarEstado(v.Patente, nuevoEstado);

            ev = new Bitacora.Evento_64PR(Sesion.SessionManager.GetInstance.Usuario.Login, ((int)Bitacora.Bitacora_64PR.ModuloBitacora_64PR.GestionVehiculos).ToString(), ((int)Bitacora.Bitacora_64PR.TipoEventoBitacora_64PR.CambioEstadoVehiculo).ToString(), 3);
            bita.RegistrarEvento(ev);

            MessageBox.Show(textos["estado_cambiado"]);
            CargaData();
        }

        private void dgvVehiculos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                BE.VehiculoJP86 v = dgvVehiculos.SelectedRows[0].DataBoundItem as BE.VehiculoJP86;
                if (uc is ucModificarVehiculoJP86 ucm)
                {
                    ucm.EscribirControles(v);
                }

                btnEliminar.Text = v.Activo ? textos["FrmGestionarVehiculosJP86.btnEliminar"] : textos["activar"];

                cmbEstado.SelectedItem = v.Estado;
                btnCambiarEstado.Enabled = true;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked) AplicarFiltros();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked) AplicarFiltros();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked) AplicarFiltros();
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            AplicarFiltros();
        }

        private void FrmGestionarVehiculosJP86_Load(object sender, EventArgs e)
        {
            ConfigurarPermisos();
        }

        private void ConfigurarPermisos()
        {
            Sesion.Rol_64PR rolUsuario = Sesion.SessionManager.GetInstance.Usuario.Rol;

            btnCrear.Visible = rolUsuario.TienePermiso(Sesion.Patentes_64PR.CrearVehiculo);
            btnModificar.Visible = rolUsuario.TienePermiso(Sesion.Patentes_64PR.ModificarVehiculo);
            btnEliminar.Visible = rolUsuario.TienePermiso(Sesion.Patentes_64PR.EliminarVehiculo);

            bool puedeCambiarEstado = rolUsuario.TienePermiso(Sesion.Patentes_64PR.CambiarEstadoVehiculo);
            cmbEstado.Visible = puedeCambiarEstado;
            btnCambiarEstado.Visible = puedeCambiarEstado;
        }

        private void FrmGestionarVehiculosJP86_FormClosed(object sender, FormClosedEventArgs e)
        {
            Idioma.GestorIdioma_64PR.GetInstance.Desuscribir(this);
        }
    }
}
