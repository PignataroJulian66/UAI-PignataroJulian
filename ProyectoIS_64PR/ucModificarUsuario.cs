using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoIS_64PR
{
    public partial class ucModificarUsuario : UserControl, Idioma.IObservadorIdioma_64PR
    {
        Sesion.BLL_Rol_64PR groles = new Sesion.BLL_Rol_64PR();
        Dictionary<string, string> textos;
        public ucModificarUsuario()
        {
            InitializeComponent();
            cmbRol.DataSource = groles.ListarRoles();
            cmbRol.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRol.SelectedIndex = 0;

            Idioma.GestorIdioma_64PR.GetInstance.Suscribir(this);

            textos = Idioma.GestorIdioma_64PR.GetInstance.ObtenerTextos();
            if (textos.Count > 0)
                ActualizarIdioma(textos);
        }

        /// <summary>
        /// Practicamente esta clase contiene todos metodos para poder
        /// volcar los valores en los controles y para poder
        /// acceder a los valores de los controles del diseñador, ya que,
        /// no se lo puede acceder de otra forma
        /// </summary>
        public void EscribirControles(Sesion.Usuario u)
        {
            cmbRol.Text = u.Rol.Nombre;
            txtEmail.Text = u.Email;
        }

        public int Rol()
        {
            Sesion.Rol_64PR rol = cmbRol.SelectedItem as Sesion.Rol_64PR;
            return rol.Id;
        }

        public string Email()
        {
            return txtEmail.Text.Trim();
        }

        public void LimpiarCampos()
        {
            cmbRol.SelectedIndex = -1;
            txtEmail.Text = string.Empty;
        }

        public void ActualizarIdioma(Dictionary<string, string> textoss)
        {
            textos = textoss;
            label4.Text = textos["ucModificarUsuario_lblRol"];
            label5.Text = textos["ucModificarUsuario_lblEmail"];
        }

    }
}
