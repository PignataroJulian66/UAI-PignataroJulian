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
    public partial class ucModificarVehiculoJP86 : UserControl, Idioma.IObservadorIdioma_64PR
    {
        BLL_64PR.Categoria_64PR gcategorias = new BLL_64PR.Categoria_64PR();
        Dictionary<string, string> textos;

        public ucModificarVehiculoJP86()
        {
            InitializeComponent();

            UI.EstilosUI.AplicarEstiloFormulario(this);

            cmbCategoria.DataSource = gcategorias.Listar().Where(c => c.Activo).ToList();
            cmbCategoria.DropDownStyle = ComboBoxStyle.DropDownList;

            numKilometraje.Minimum = 0;
            numKilometraje.Maximum = 9999999;

            txtPatente.ReadOnly = true;

            Idioma.GestorIdioma_64PR.GetInstance.Suscribir(this);

            textos = Idioma.GestorIdioma_64PR.GetInstance.ObtenerTextos();
            if (textos.Count > 0)
                ActualizarIdioma(textos);
        }

        /// <summary>
        /// Vuelca los valores del vehiculo seleccionado en los controles
        /// </summary>
        public void EscribirControles(BE.VehiculoJP86 v)
        {
            txtPatente.Text = v.Patente;
            txtMarca.Text = v.Marca;
            txtModelo.Text = v.Modelo;
            numKilometraje.Value = v.Kilometraje;

            for (int i = 0; i < cmbCategoria.Items.Count; i++)
            {
                if (((BE.Categoria_64PR)cmbCategoria.Items[i]).Id == v.Categoria.Id)
                {
                    cmbCategoria.SelectedIndex = i;
                    break;
                }
            }
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
            cmbCategoria.SelectedIndex = -1;
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
