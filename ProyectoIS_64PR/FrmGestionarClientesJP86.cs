using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoIS_64PR
{
    public partial class FrmGestionarClientesJP86 : Form, Idioma.IObservadorIdioma_64PR
    {
        Bitacora.Bitacora_64PR bita = new Bitacora.Bitacora_64PR();
        Bitacora.Evento_64PR ev;

        BLL_64PR.ClienteJP86 gclientes = new BLL_64PR.ClienteJP86();
        public Dictionary<string, string> textos;

        string modo = "consulta";
        UserControl uc;
        List<BE.ClienteJP86> lst;

        private static readonly Regex RegexDNI = new Regex(@"^\d{7,8}$");
        private static readonly Regex RegexTelefono = new Regex(@"^(\+?54)?0?\d{8,11}$");

        public FrmGestionarClientesJP86()
        {
            InitializeComponent();

            radioButton3.Checked = true;
            dgvClientes.ReadOnly = true;
            dgvClientes.MultiSelect = false;
            dgvClientes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvClientes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvClientes.BackgroundColor = SystemColors.Menu;
            dgvClientes.BorderStyle = BorderStyle.None;
            CargaData();

            btnGuardar.Enabled = false;

            Idioma.GestorIdioma_64PR.GetInstance.Suscribir(this);

            textos = Idioma.GestorIdioma_64PR.GetInstance.ObtenerTextos();
            if (textos.Count > 0)
                ActualizarIdioma(textos);
            lblModo.Text = textos["frmGestionClientes_lblModoConsulta"];
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
                    lblModo.Text = textos["frmGestionClientes_lblModoConsulta"];
                    break;
                case "crear":
                    lblModo.Text = textos["frmGestionClientes_lblModoCrear"];
                    break;
                case "modificar":
                    lblModo.Text = textos["frmGestionClientes_lblModoModificar"];
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
            lst = gclientes.Listar();
            AplicarFiltros();
        }

        private void AplicarFiltros()
        {
            if (lst == null) return;

            IEnumerable<BE.ClienteJP86> filtrado = lst;

            ///radioButton1 = "No activos", radioButton2 = "Activos"
            if (radioButton1.Checked)
                filtrado = filtrado.Where(c => c.Activo == false);
            else if (radioButton2.Checked)
                filtrado = filtrado.Where(c => c.Activo == true);

            if (!string.IsNullOrWhiteSpace(txtBuscar.Text))
            {
                string texto = txtBuscar.Text.Trim();
                filtrado = filtrado.Where(c =>
                    c.DNI.IndexOf(texto, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    c.Nombre.IndexOf(texto, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    c.Apellido.IndexOf(texto, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            List<BE.ClienteJP86> resultado = filtrado.ToList();

            dgvClientes.DataSource = null;
            dgvClientes.DataSource = resultado;

            if (textos != null && textos.Count > 0)
                lblCantidad.Text = textos["frmGestionClientes_lblCantidad"] + resultado.Count.ToString();
        }

        private void btnCrear_Click(object sender, EventArgs e)
        {
            modo = "crear";
            lblModo.Text = textos["frmGestionClientes_lblModoCrear"];
            uc = new ucCrearClienteJP86();
            pnlContenedor.Controls.Clear();
            uc.Dock = DockStyle.Fill;
            pnlContenedor.Controls.Add(uc);
            btnGuardar.Enabled = true;
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            modo = "modificar";
            lblModo.Text = textos["frmGestionClientes_lblModoModificar"];
            uc = new ucModificarClienteJP86();
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
                    if (uc is ucCrearClienteJP86 ucc)
                    {
                        if (!RegexDNI.IsMatch(ucc.DNI()))
                        {
                            MessageBox.Show(textos["msg_DNIValido"]);
                            return;
                        }
                        if (string.IsNullOrWhiteSpace(ucc.Nombre()))
                        {
                            MessageBox.Show(textos["msg_NombreValido"]);
                            return;
                        }
                        if (string.IsNullOrWhiteSpace(ucc.Apellido()))
                        {
                            MessageBox.Show(textos["msg_ApellidoValido"]);
                            return;
                        }
                        string telefonoNormalizado = Regex.Replace(ucc.Telefono(), @"[\s\-]", "");
                        if (!RegexTelefono.IsMatch(telefonoNormalizado))
                        {
                            MessageBox.Show(textos["msg_TelefonoValido"]);
                            return;
                        }
                        try
                        {
                            BE.ClienteJP86 c = new BE.ClienteJP86()
                            {
                                DNI = ucc.DNI(),
                                Nombre = ucc.Nombre(),
                                Apellido = ucc.Apellido(),
                                Telefono = ucc.Telefono()
                            };

                            gclientes.Crear(c);

                            ev = new Bitacora.Evento_64PR(Sesion.SessionManager.GetInstance.Usuario.Login, ((int)Bitacora.Bitacora_64PR.ModuloBitacora_64PR.GestionClientes).ToString(), ((int)Bitacora.Bitacora_64PR.TipoEventoBitacora_64PR.AltaCliente).ToString(), 4);
                            bita.RegistrarEvento(ev);

                            MessageBox.Show(textos["cliente_creado"]);

                            CargaData();
                            ucc.LimpiarCampos();
                            pnlContenedor.Controls.Clear();
                            uc = null;
                            modo = "consulta";
                            lblModo.Text = textos["frmGestionClientes_lblModoConsulta"];
                            btnGuardar.Enabled = false;
                        }
                        catch (SqlException ex)
                        {
                            if (ex.Number == 2627 || ex.Number == 2601)
                            {
                                MessageBox.Show(textos["msg_DNIDuplicado"],
                                    "DNI", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                            {
                                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    break;

                case "modificar":
                    if (uc is ucModificarClienteJP86 ucm)
                    {
                        if (string.IsNullOrWhiteSpace(ucm.Nombre()))
                        {
                            MessageBox.Show(textos["msg_NombreValido"]);
                            return;
                        }
                        if (string.IsNullOrWhiteSpace(ucm.Apellido()))
                        {
                            MessageBox.Show(textos["msg_ApellidoValido"]);
                            return;
                        }
                        string telefonoNormalizado = Regex.Replace(ucm.Telefono(), @"[\s\-]", "");
                        if (!RegexTelefono.IsMatch(telefonoNormalizado))
                        {
                            MessageBox.Show(textos["msg_TelefonoValido"]);
                            return;
                        }
                        if (dgvClientes.SelectedRows.Count == 0)
                        {
                            MessageBox.Show(textos["seleccionar_cliente"]);
                            return;
                        }
                        try
                        {
                            BE.ClienteJP86 c = dgvClientes.SelectedRows[0].DataBoundItem as BE.ClienteJP86;
                            c.Nombre = ucm.Nombre();
                            c.Apellido = ucm.Apellido();
                            c.Telefono = ucm.Telefono();

                            gclientes.Modificar(c);

                            ev = new Bitacora.Evento_64PR(Sesion.SessionManager.GetInstance.Usuario.Login, ((int)Bitacora.Bitacora_64PR.ModuloBitacora_64PR.GestionClientes).ToString(), ((int)Bitacora.Bitacora_64PR.TipoEventoBitacora_64PR.ModificacionCliente).ToString(), 4);
                            bita.RegistrarEvento(ev);

                            MessageBox.Show(textos["cliente_modificado"]);

                            CargaData();
                            ucm.LimpiarCampos();
                            pnlContenedor.Controls.Clear();
                            uc = null;
                            modo = "consulta";
                            lblModo.Text = textos["frmGestionClientes_lblModoConsulta"];
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
            if (dgvClientes.SelectedRows.Count == 0)
            {
                MessageBox.Show(textos["seleccionar_cliente"]);
                return;
            }

            BE.ClienteJP86 c = dgvClientes.SelectedRows[0].DataBoundItem as BE.ClienteJP86;

            bool vaAQuedarActivo = !c.Activo;

            gclientes.ActDesact(c);

            string tipoEvento = vaAQuedarActivo
                ? ((int)Bitacora.Bitacora_64PR.TipoEventoBitacora_64PR.AltaCliente).ToString()
                : ((int)Bitacora.Bitacora_64PR.TipoEventoBitacora_64PR.BajaCliente).ToString();
            ev = new Bitacora.Evento_64PR(Sesion.SessionManager.GetInstance.Usuario.Login, ((int)Bitacora.Bitacora_64PR.ModuloBitacora_64PR.GestionClientes).ToString(), tipoEvento, 4);
            bita.RegistrarEvento(ev);

            MessageBox.Show(textos["operacion exitosa"]);
            CargaData();
            btnEliminar.Text = textos["FrmGestionarClientesJP86.btnEliminar"];
        }

        private void dgvClientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                BE.ClienteJP86 c = dgvClientes.SelectedRows[0].DataBoundItem as BE.ClienteJP86;
                if (uc is ucModificarClienteJP86 ucm)
                {
                    ucm.EscribirControles(c);
                }

                btnEliminar.Text = c.Activo ? textos["FrmGestionarClientesJP86.btnEliminar"] : textos["activar"];
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

        private void FrmGestionarClientesJP86_Load(object sender, EventArgs e)
        {
            ConfigurarPermisos();
        }

        private void ConfigurarPermisos()
        {
            Sesion.Rol_64PR rolUsuario = Sesion.SessionManager.GetInstance.Usuario.Rol;

            btnCrear.Visible = rolUsuario.TienePermiso(Sesion.Patentes_64PR.CrearCliente);
            btnModificar.Visible = rolUsuario.TienePermiso(Sesion.Patentes_64PR.ModificarCliente);
            btnEliminar.Visible = rolUsuario.TienePermiso(Sesion.Patentes_64PR.EliminarCliente);
        }

        private void FrmGestionarClientesJP86_FormClosed(object sender, FormClosedEventArgs e)
        {
            Idioma.GestorIdioma_64PR.GetInstance.Desuscribir(this);
        }
    }
}
