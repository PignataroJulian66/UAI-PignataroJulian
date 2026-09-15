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
    public partial class ucModificarCategoria : UserControl, Idioma.IObservadorIdioma_64PR
    {
        Dictionary<string, string> textos;
        public ucModificarCategoria()
        {
            InitializeComponent();

            UI.EstilosUI.AplicarEstiloFormulario(this);

            numTarifaDiaria.Minimum = 0.01m;
            numTarifaDiaria.Maximum = 999999;
            numTarifaDiaria.DecimalPlaces = 2;

            Idioma.GestorIdioma_64PR.GetInstance.Suscribir(this);

            textos = Idioma.GestorIdioma_64PR.GetInstance.ObtenerTextos();
            if (textos.Count > 0)
                ActualizarIdioma(textos);
        }

        /// <summary>
        /// Vuelca los valores de la categoria seleccionada en los controles
        /// </summary>
        public void EscribirControles(BE.Categoria_64PR c)
        {
            txtNombre.Text = c.Nombre;
            txtDescripcion.Text = c.Descripcion;
            numTarifaDiaria.Value = c.TarifaDiaria;
        }

        public string Nombre()
        {
            return txtNombre.Text.Trim();
        }

        public string Descripcion()
        {
            return txtDescripcion.Text.Trim();
        }

        public decimal TarifaDiaria()
        {
            return numTarifaDiaria.Value;
        }

        public void LimpiarCampos()
        {
            txtNombre.Text = string.Empty;
            txtDescripcion.Text = string.Empty;
            numTarifaDiaria.Value = numTarifaDiaria.Minimum;
        }

        public void ActualizarIdioma(Dictionary<string, string> textoss)
        {
            textos = textoss;
            lblNombre.Text = textos["ucCategoria_lblNombre"];
            lblDescripcion.Text = textos["ucCategoria_lblDescripcion"];
            lblTarifaDiaria.Text = textos["ucCategoria_lblTarifaDiaria"];
        }
    }
}
