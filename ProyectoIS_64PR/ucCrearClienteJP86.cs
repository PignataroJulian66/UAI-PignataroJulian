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
    public partial class ucCrearClienteJP86 : UserControl, Idioma.IObservadorIdioma_64PR
    {
        Dictionary<string, string> textos;

        public ucCrearClienteJP86()
        {
            InitializeComponent();

            Idioma.GestorIdioma_64PR.GetInstance.Suscribir(this);

            textos = Idioma.GestorIdioma_64PR.GetInstance.ObtenerTextos();
            if (textos.Count > 0)
                ActualizarIdioma(textos);
        }

        public string DNI()
        {
            return txtDNI.Text.Trim();
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
