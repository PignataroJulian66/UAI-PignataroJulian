using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoIS_64PR
{
    public partial class FrmGestionarRoles_64PR : Form, Idioma.IObservadorIdioma_64PR
    {
        Sesion.BLL_Rol_64PR bllRol = new Sesion.BLL_Rol_64PR();
        Sesion.BLL_Permiso_64PR bllpermisos = new Sesion.BLL_Permiso_64PR();
        List<Sesion.Rol_64PR> nodos2 = new List<Sesion.Rol_64PR>();
        Dictionary<string, string> textos;

        private enum Modo { Crear, Modificar, Eliminar }
        private Modo modoActual = Modo.Crear;
        private int idRolEnEdicion = -1;
        public FrmGestionarRoles_64PR()
        {
            InitializeComponent();

            this.Font = UI.TemaVisual.FuenteTexto;
            this.BackColor = UI.TemaVisual.FondoPagina;
            UI.EstilosUI.EstiloBotonPrimario(btnAgregar);
            UI.EstilosUI.EstiloBotonPrimario(btnAplicar);
            UI.EstilosUI.EstiloBotonSecundario(btnQuitar);
            UI.EstilosUI.EstiloBotonPeligro(btnEliminar);

            CargaNodos();
            CargaRoles();

            btnEliminar.Hide();
            btnAgregar.Hide();
            btnQuitar.Hide();
            btnAplicar.Hide();
            label1.Visible = false;
            txtNombre.Visible = false;
            treeView2.Visible = false;

            Idioma.GestorIdioma_64PR.GetInstance.Suscribir(this); ///observer del cambio de idioma
             ///Aplico el idioma que ya está cargado
            textos = Idioma.GestorIdioma_64PR.GetInstance.ObtenerTextos();
            if (textos.Count > 0)
                ActualizarIdioma(textos);
        }
        private void CargaNodos()
        {
            treeView1.Nodes.Clear();
            List<Sesion.Rol_64PR> nodos = bllpermisos.ObtenerTodosLosNodos();

            foreach (var nodo in nodos)
                treeView1.Nodes.Add(CrearNodoVisual(nodo));

            treeView1.ExpandAll();
        }

        private void CargaRoles()
        {
            listBox1.Items.Clear();
            List<Sesion.Rol_64PR> roles = bllRol.ListarRoles();

            foreach (var rol in roles)
                listBox1.Items.Add(rol);
        }
        private TreeNode CrearNodoVisual(Sesion.Rol_64PR rol)
        {
            TreeNode tn = new TreeNode(rol.Nombre);
            tn.Tag = rol;

            foreach (var hijo in rol.Hijos)
                tn.Nodes.Add(CrearNodoVisual(hijo));

            return tn;
        }

        private void rbCrear_CheckedChanged(object sender, EventArgs e)
        {
            if (!rbCrear.Checked) return;
            modoActual = Modo.Crear;
            txtNombre.Text = string.Empty;
            txtNombre.Enabled = true;
            treeView2.Nodes.Clear();
            nodos2.Clear();
            btnEliminar.Hide();
            btnAgregar.Show();
            btnQuitar.Show();
            btnAplicar.Show();
            idRolEnEdicion = -1;
            label1.Visible = true;
            txtNombre.Visible = true;
            treeView2.Visible = true;
        }

        private void rbModificar_CheckedChanged(object sender, EventArgs e)
        {
            if (!rbModificar.Checked) return;
            modoActual = Modo.Modificar;
            txtNombre.Clear();
            txtNombre.Enabled = true;
            treeView2.Nodes.Clear();
            nodos2.Clear();
            btnEliminar.Hide();
            btnAgregar.Show();
            btnQuitar.Show();
            btnAplicar.Show();
            idRolEnEdicion = -1;
            label1.Visible = true;
            txtNombre.Visible = true;
            treeView2.Visible = true;
        }

        private void rbEliminar_CheckedChanged(object sender, EventArgs e)
        {
            if (!rbEliminar.Checked) return;
            modoActual = Modo.Eliminar;
            txtNombre.Clear();
            txtNombre.Enabled = false;
            treeView2.Nodes.Clear();
            nodos2.Clear();
            btnEliminar.Show();
            btnAgregar.Hide();
            btnQuitar.Hide();
            btnAplicar.Hide();
            label1.Visible = false;
            txtNombre.Visible = false;
            treeView2.Visible = false;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null) return;
            if (modoActual == Modo.Crear) return;
            if(modoActual == Modo.Eliminar) return;

            Sesion.Rol_64PR rolSeleccionado = (Sesion.Rol_64PR)listBox1.SelectedItem;

            Sesion.Rol_64PR rolCompleto = bllRol.ObtenerRolCompleto(rolSeleccionado.Id);

            if (modoActual == Modo.Modificar)
            {
                if (rolCompleto.Id == 2) ///El id 2 es del administrador, y esto sera siempre asi ya que este no se puede eliminar
                {
                    MessageBox.Show(textos["admin_no_modificar"]);
                    treeView2.Nodes.Clear();
                    nodos2.Clear();
                    txtNombre.Text = string.Empty;
                    return;
                }
                idRolEnEdicion = rolCompleto.Id;
                txtNombre.Text = rolCompleto.Nombre;
                treeView2.Nodes.Clear();
                nodos2.Clear();

                foreach (var hijo in rolCompleto.Hijos)
                {
                    nodos2.Add(hijo);
                    treeView2.Nodes.Add(CrearNodoVisual(hijo));
                }
                treeView2.ExpandAll();
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null) return;

            Sesion.Rol_64PR nodoSeleccionado = (Sesion.Rol_64PR)treeView1.SelectedNode.Tag;

            foreach (TreeNode tn in treeView2.Nodes)
            {
                Sesion.Rol_64PR nodoExistente = (Sesion.Rol_64PR)tn.Tag;

                if (nodoExistente.GetType() == nodoSeleccionado.GetType() &&
                    nodoExistente.Id == nodoSeleccionado.Id)
                {
                    MessageBox.Show(textos["ya_agregado"]);
                    return;
                }

                if (EsHijo(nodoExistente, nodoSeleccionado))
                {
                    MessageBox.Show($"'{nodoSeleccionado.Nombre}'" + textos["ya_incluido"] + $"'{nodoExistente.Nombre}'.");
                    return;
                }

                if (EsHijo(nodoSeleccionado, nodoExistente))
                {
                    MessageBox.Show($"'{nodoExistente.Nombre}'" + textos["ya_incluido"] + $"'{nodoSeleccionado.Nombre}'.");
                    return;
                }
            }

            nodos2.Add(nodoSeleccionado);
            treeView2.Nodes.Add(CrearNodoVisual(nodoSeleccionado));
            treeView2.ExpandAll();
        }
        private bool EsHijo(Sesion.Rol_64PR padre, Sesion.Rol_64PR buscado)
        {
            foreach (var hijo in padre.Hijos)
            {
                if (hijo.GetType() == buscado.GetType() && hijo.Id == buscado.Id)
                    return true;

                if (EsHijo(hijo, buscado))
                    return true;
            }
            return false;
        }

        private void btnQuitar_Click(object sender, EventArgs e)
        {
            if (treeView2.SelectedNode == null) return;

            if (treeView2.SelectedNode.Parent != null)
            {
                MessageBox.Show(textos["solo_quitar_raiz"]);
                return;
            }

            Sesion.Rol_64PR nodoSeleccionado = (Sesion.Rol_64PR)treeView2.SelectedNode.Tag;
            nodos2.Remove(nodoSeleccionado);
            treeView2.Nodes.Remove(treeView2.SelectedNode);
        }

        private void btnAplicar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show(textos["nombre_para_rol"]);
                return;
            }
            if (treeView2.Nodes.Count == 0)
            {
                MessageBox.Show(textos["almenos_un_elemento"]);
                return;
            }

            List<Sesion.Rol_64PR> hijos = new List<Sesion.Rol_64PR>();
            foreach (TreeNode tn in treeView2.Nodes)
                hijos.Add((Sesion.Rol_64PR)tn.Tag);

            try
            {
                if (modoActual == Modo.Crear)
                {
                    bllRol.CrearRol(txtNombre.Text.Trim(), hijos);

                    Bitacora.Bitacora_64PR bita = new Bitacora.Bitacora_64PR();
                    Bitacora.Evento_64PR ev = new Bitacora.Evento_64PR(Sesion.SessionManager.GetInstance.Usuario.Login, ((int)Bitacora.Bitacora_64PR.ModuloBitacora_64PR.GestionRoles).ToString(), ((int)Bitacora.Bitacora_64PR.TipoEventoBitacora_64PR.CreacionRol).ToString(), 2);
                    bita.RegistrarEvento(ev);

                    MessageBox.Show(textos["rol_creado"]);
                }
                else if (modoActual == Modo.Modificar)
                {
                    if (idRolEnEdicion == -1)
                    {
                        MessageBox.Show(textos["seleccionar_rol"]);
                        return;
                    }
                    bllRol.ModificarRol(idRolEnEdicion, txtNombre.Text.Trim(), hijos);
                    Bitacora.Bitacora_64PR bita = new Bitacora.Bitacora_64PR();
                    Bitacora.Evento_64PR ev = new Bitacora.Evento_64PR(Sesion.SessionManager.GetInstance.Usuario.Login, ((int)Bitacora.Bitacora_64PR.ModuloBitacora_64PR.GestionRoles).ToString(), ((int)Bitacora.Bitacora_64PR.TipoEventoBitacora_64PR.ModificacionRol).ToString(), 2);
                    bita.RegistrarEvento(ev);
                    MessageBox.Show(textos["rol_modificado"]);
                }

                CargaRoles();
                treeView2.Nodes.Clear();
                nodos2.Clear();
                txtNombre.Clear();
                idRolEnEdicion = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        public void ActualizarIdioma(Dictionary<string, string> textoss)
        {
            textos = textoss;
            Traductor_64PR.Traducir(this, textos);
            btnAgregar.Text += "-->";
            btnQuitar.Text +="<--";
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null) return;
            if (modoActual == Modo.Crear) return;

            Sesion.Rol_64PR rolSeleccionado = (Sesion.Rol_64PR)listBox1.SelectedItem;

            /// Rol básico y admin no se pueden tocar
            if (modoActual == Modo.Eliminar && (rolSeleccionado.Id == 1 || rolSeleccionado.Id == 2))
            {
                MessageBox.Show(textos["basico_y_admin"]);
                listBox1.ClearSelected();
                return;
            }

            Sesion.Rol_64PR rolCompleto = bllRol.ObtenerRolCompleto(rolSeleccionado.Id);

            int cantUsuarios = bllRol.ContarUsuariosConRol(rolCompleto.Id);

            string mensaje = textos["pregunta_eliminacion"] + rolCompleto.Nombre + "?";

            if (MessageBox.Show(mensaje, textos["confirmar_eliminacion"], MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                listBox1.ClearSelected();
                return;
            }

            try
            {
                bllRol.EliminarRol(rolCompleto.Id, cantUsuarios > 0);
                Bitacora.Bitacora_64PR bita = new Bitacora.Bitacora_64PR();
                Bitacora.Evento_64PR ev = new Bitacora.Evento_64PR(Sesion.SessionManager.GetInstance.Usuario.Login, ((int)Bitacora.Bitacora_64PR.ModuloBitacora_64PR.GestionRoles).ToString(), ((int)Bitacora.Bitacora_64PR.TipoEventoBitacora_64PR.EliminacionRol).ToString(), 1);
                bita.RegistrarEvento(ev);
                MessageBox.Show(textos["rol_eliminado"]);
                CargaRoles();
                treeView2.Nodes.Clear();
                nodos2.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void FrmGestionarRoles_64PR_FormClosed(object sender, FormClosedEventArgs e)
        {
            Idioma.GestorIdioma_64PR.GetInstance.Desuscribir(this); ///observer del cambio de idioma
        }

        private void FrmGestionarRoles_64PR_Load(object sender, EventArgs e)
        {
            ConfigurarPermisos();
        }
        private void ConfigurarPermisos()
        {
            Sesion.Rol_64PR rolUsuario = Sesion.SessionManager.GetInstance.Usuario.Rol;

            rbCrear.Visible = rolUsuario.TienePermiso(Sesion.Patentes_64PR.CrearRoles);
            rbModificar.Visible = rolUsuario.TienePermiso(Sesion.Patentes_64PR.ModificarRoles);
            rbEliminar.Visible = rolUsuario.TienePermiso(Sesion.Patentes_64PR.EliminarRoles);
        }
    }
}
