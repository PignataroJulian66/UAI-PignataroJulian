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
    public partial class ucModificarClienteJP86 : UserControl, Idioma.IObservadorIdioma_64PR
    {
        Dictionary<string, string> textos;

        public ucModificarClienteJP86()
        {
            InitializeComponent();

            UI.EstilosUI.AplicarEstiloFormulario(this);

            txtDNI.ReadOnly = true;

            Idioma.GestorIdioma_64PR.GetInstance.Suscribir(this);

            textos = Idioma.GestorIdioma_64PR.GetInstance.ObtenerTextos();
            if (textos.Count > 0)
                ActualizarIdioma(textos);
        }

        /// <summary>
        /// Vuelca los valores del cliente seleccionado en los controles
        /// </summary>
        public void EscribirControles(BE.ClienteJP86 c)
        {
            txtDNI.Text = c.DNI;
            txtNombre.Text = c.Nombre;
            txtApellido.Text = c.Apellido;
            txtTelefono.Text = c.Telefono;
        }

        public string Nombre()
        {
            return txtNombre.Text.Trim();
        }

        public string Apellido()
        {
            return txtApellido.Text.Trim();
        }

        public string Telefono()
        {
            return txtTelefono.Text.Trim();
        }

        public void LimpiarCampos()
        {
            txtDNI.Text = string.Empty;
            txtNombre.Text = string.Empty;
            txtApellido.Text = string.Empty;
            txtTelefono.Text = string.Empty;
        }

        public void ActualizarIdioma(Dictionary<string, string> textoss)
        {
            textos = textoss;
            lblDNI.Text = textos["ucCliente_lblDNI"];
            lblNombre.Text = textos["ucCliente_lblNombre"];
            lblApellido.Text = textos["ucCliente_lblApellido"];
            lblTelefono.Text = textos["ucCliente_lblTelefono"];
        }
    }
}
