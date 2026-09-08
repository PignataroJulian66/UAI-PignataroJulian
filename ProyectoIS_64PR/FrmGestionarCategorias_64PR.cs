using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoIS_64PR
{
    public partial class FrmGestionarCategorias_64PR : Form, Idioma.IObservadorIdioma_64PR
    {
        Bitacora.Bitacora_64PR bita = new Bitacora.Bitacora_64PR();
        Bitacora.Evento_64PR ev;

        BLL_64PR.Categoria_64PR gcategorias = new BLL_64PR.Categoria_64PR();
        public Dictionary<string, string> textos;

        string modo = "consulta";
        UserControl uc;
        List<BE.Categoria_64PR> lst;

        public FrmGestionarCategorias_64PR()
        {
            InitializeComponent();

            radioButton3.Checked = true;
            dgvCategorias.ReadOnly = true;
            dgvCategorias.MultiSelect = false;
            dgvCategorias.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCategorias.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvCategorias.BackgroundColor = SystemColors.Menu;
            dgvCategorias.BorderStyle = BorderStyle.None;
            CargaData();

            btnGuardar.Enabled = false;

            Idioma.GestorIdioma_64PR.GetInstance.Suscribir(this); ///Observer del cambio de idioma

            textos = Idioma.GestorIdioma_64PR.GetInstance.ObtenerTextos();
            if (textos.Count > 0)
                ActualizarIdioma(textos);
            lblModo.Text = textos["frmGestionCategorias_lblModoConsulta"];
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
                    lblModo.Text = textos["frmGestionCategorias_lblModoConsulta"];
                    break;
                case "crear":
                    lblModo.Text = textos["frmGestionCategorias_lblModoCrear"];
                    break;
                case "modificar":
                    lblModo.Text = textos["frmGestionCategorias_lblModoModificar"];
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
            lst = gcategorias.Listar();
            AplicarFiltros();
        }

        private void AplicarFiltros()
        {
            if (lst == null) return; ///Necesario para no ejecutar esto durante la construccion del frm (radioButtonX.Checked = true dispara el evento antes de CargaData)

            IEnumerable<BE.Categoria_64PR> filtrado = lst;

            ///radioButton1 = "No Activas", radioButton2 = "Activas"
            if (radioButton1.Checked)
                filtrado = filtrado.Where(c => c.Activo == false);
            else if (radioButton2.Checked)
                filtrado = filtrado.Where(c => c.Activo == true);

            if (!string.IsNullOrWhiteSpace(txtBuscar.Text))
                filtrado = filtrado.Where(c => c.Nombre.IndexOf(txtBuscar.Text.Trim(), StringComparison.OrdinalIgnoreCase) >= 0);

            List<BE.Categoria_64PR> resultado = filtrado.ToList();

            dgvCategorias.DataSource = null;
            dgvCategorias.DataSource = resultado;

            if (textos != null && textos.Count > 0)
                lblCantidad.Text = textos["frmGestionCategorias_lblCantidad"] + resultado.Count.ToString();
        }

        private void btnCrear_Click(object sender, EventArgs e)
        {
            modo = "crear";
            lblModo.Text = textos["frmGestionCategorias_lblModoCrear"];
            uc = new ucCrearCategoria();
            pnlContenedor.Controls.Clear();
            uc.Dock = DockStyle.Fill;
            pnlContenedor.Controls.Add(uc);
            btnGuardar.Enabled = true;
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            modo = "modificar";
            lblModo.Text = textos["frmGestionCategorias_lblModoModificar"];
            uc = new ucModificarCategoria();
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
                    if (uc is ucCrearCategoria ucc)
                    {
                        if (string.IsNullOrWhiteSpace(ucc.Nombre()))
                        {
                            MessageBox.Show(textos["msg_NombreCategoriaValido"]);
                            return;
                        }
                        try
                        {
                            BE.Categoria_64PR c = new BE.Categoria_64PR()
                            {
                                Nombre = ucc.Nombre(),
                                Descripcion = ucc.Descripcion(),
                                TarifaDiaria = ucc.TarifaDiaria()
                            };

                            gcategorias.Crear(c);

                            ev = new Bitacora.Evento_64PR(Sesion.SessionManager.GetInstance.Usuario.Login, ((int)Bitacora.Bitacora_64PR.ModuloBitacora_64PR.GestionCategorias).ToString(), ((int)Bitacora.Bitacora_64PR.TipoEventoBitacora_64PR.AltaCategoria).ToString(), 4);
                            bita.RegistrarEvento(ev);

                            MessageBox.Show(textos["categoria_creada"]);

                            CargaData();
                            ucc.LimpiarCampos();
                            pnlContenedor.Controls.Clear();
                            uc = null;
                            modo = "consulta";
                            lblModo.Text = textos["frmGestionCategorias_lblModoConsulta"];
                            btnGuardar.Enabled = false;
                        }
                        catch (SqlException ex)
                        {
                            if (ex.Number == 2627 || ex.Number == 2601)
                            {
                                MessageBox.Show(textos["msg_NombreCategoriaDuplicado"],
                                    "Nombre", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                            {
                                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    break;

                case "modificar":
                    if (uc is ucModificarCategoria ucm)
                    {
                        if (string.IsNullOrWhiteSpace(ucm.Nombre()))
                        {
                            MessageBox.Show(textos["msg_NombreCategoriaValido"]);
                            return;
                        }
                        if (dgvCategorias.SelectedRows.Count == 0)
                        {
                            MessageBox.Show(textos["seleccionar_categoria"]);
                            return;
                        }
                        try
                        {
                            BE.Categoria_64PR c = dgvCategorias.SelectedRows[0].DataBoundItem as BE.Categoria_64PR;
                            c.Nombre = ucm.Nombre();
                            c.Descripcion = ucm.Descripcion();
                            c.TarifaDiaria = ucm.TarifaDiaria();

                            gcategorias.Modificar(c);

                            ev = new Bitacora.Evento_64PR(Sesion.SessionManager.GetInstance.Usuario.Login, ((int)Bitacora.Bitacora_64PR.ModuloBitacora_64PR.GestionCategorias).ToString(), ((int)Bitacora.Bitacora_64PR.TipoEventoBitacora_64PR.ModificacionCategoria).ToString(), 4);
                            bita.RegistrarEvento(ev);

                            MessageBox.Show(textos["categoria_modificada"]);

                            CargaData();
                            ucm.LimpiarCampos();
                            pnlContenedor.Controls.Clear();
                            uc = null;
                            modo = "consulta";
                            lblModo.Text = textos["frmGestionCategorias_lblModoConsulta"];
                            btnGuardar.Enabled = false;
                        }
                        catch (SqlException ex)
                        {
                            if (ex.Number == 2627 || ex.Number == 2601)
                            {
                                MessageBox.Show(textos["msg_NombreCategoriaDuplicado"],
                                    "Nombre", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                            {
                                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    break;
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvCategorias.SelectedRows.Count == 0)
            {
                MessageBox.Show(textos["seleccionar_categoria"]);
                return;
            }

            BE.Categoria_64PR c = dgvCategorias.SelectedRows[0].DataBoundItem as BE.Categoria_64PR;

            bool vaAQuedarActivo = !c.Activo; ///se calcula antes de togglear, para saber que evento registrar

            gcategorias.ActDesact(c);

            string tipoEvento = vaAQuedarActivo
                ? ((int)Bitacora.Bitacora_64PR.TipoEventoBitacora_64PR.AltaCategoria).ToString()
                : ((int)Bitacora.Bitacora_64PR.TipoEventoBitacora_64PR.BajaCategoria).ToString();
            ev = new Bitacora.Evento_64PR(Sesion.SessionManager.GetInstance.Usuario.Login, ((int)Bitacora.Bitacora_64PR.ModuloBitacora_64PR.GestionCategorias).ToString(), tipoEvento, 4);
            bita.RegistrarEvento(ev);

            MessageBox.Show(textos["operacion exitosa"]);
            CargaData();
            btnEliminar.Text = textos["FrmGestionarCategorias_64PR.btnEliminar"]; ///se perdio la seleccion al recargar, vuelve al texto por defecto
        }

        private void dgvCategorias_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                BE.Categoria_64PR c = dgvCategorias.SelectedRows[0].DataBoundItem as BE.Categoria_64PR;
                if (uc is ucModificarCategoria ucm)
                {
                    ucm.EscribirControles(c);
                }

                ///El boton "Eliminar" cambia a "Activar" cuando la categoria seleccionada ya esta inactiva
                btnEliminar.Text = c.Activo ? textos["FrmGestionarCategorias_64PR.btnEliminar"] : textos["activar"];
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

        private void FrmGestionarCategorias_64PR_Load(object sender, EventArgs e)
        {
            ConfigurarPermisos();
        }

        private void ConfigurarPermisos()
        {
            Sesion.Rol_64PR rolUsuario = Sesion.SessionManager.GetInstance.Usuario.Rol;

            btnCrear.Visible = rolUsuario.TienePermiso(Sesion.Patentes_64PR.CrearCategoria);
            btnModificar.Visible = rolUsuario.TienePermiso(Sesion.Patentes_64PR.ModificarCategoria);
            btnEliminar.Visible = rolUsuario.TienePermiso(Sesion.Patentes_64PR.EliminarCategoria);
        }

        private void FrmGestionarCategorias_64PR_FormClosed(object sender, FormClosedEventArgs e)
        {
            Idioma.GestorIdioma_64PR.GetInstance.Desuscribir(this);
        }
    }
}
