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
    public partial class ucCrearVehiculoJP86 : UserControl, Idioma.IObservadorIdioma_64PR
    {
        BLL_64PR.Categoria_64PR gcategorias = new BLL_64PR.Categoria_64PR();
        Dictionary<string, string> textos;

        public ucCrearVehiculoJP86()
        {
            InitializeComponent();

            UI.EstilosUI.AplicarEstiloFormulario(this);

            cmbCategoria.DataSource = gcategorias.Listar().Where(c => c.Activo).ToList();
            cmbCategoria.DropDownStyle = ComboBoxStyle.DropDownList;
            if (cmbCategoria.Items.Count > 0)
                cmbCategoria.SelectedIndex = 0;

            numKilometraje.Minimum = 0;
            numKilometraje.Maximum = 9999999;
            numKilometraje.Value = 0;

            Idioma.GestorIdioma_64PR.GetInstance.Suscribir(this);

            textos = Idioma.GestorIdioma_64PR.GetInstance.ObtenerTextos();
            if (textos.Count > 0)
                ActualizarIdioma(textos);
        }

        public string Patente()
        {
            return txtPatente.Text.Trim().ToUpper();
        }

        public string Marca()
        {
            return txtMarca.Text.Trim();
        }

        public string Modelo()
        {
            return txtModelo.Text.Trim();
        }

        public BE.Categoria_64PR Categoria()
        {
            return cmbCategoria.SelectedItem as BE.Categoria_64PR;
        }

        public int Kilometraje()
        {
            return (int)numKilometraje.Value;
        }

        public void LimpiarCampos()
        {
            txtPatente.Text = string.Empty;
            txtMarca.Text = string.Empty;
            txtModelo.Text = string.Empty;
            numKilometraje.Value = numKilometraje.Minimum;
            if (cmbCategoria.Items.Count > 0)
                cmbCategoria.SelectedIndex = 0;
        }

        public void ActualizarIdioma(Dictionary<string, string> textoss)
        {
            textos = textoss;
            lblPatente.Text = textos["ucVehiculo_lblPatente"];
            lblMarca.Text = textos["ucVehiculo_lblMarca"];
            lblModelo.Text = textos["ucVehiculo_lblModelo"];
            lblCategoria.Text = textos["ucVehiculo_lblCategoria"];
            lblKilometraje.Text = textos["ucVehiculo_lblKilometraje"];
        }
    }
}
