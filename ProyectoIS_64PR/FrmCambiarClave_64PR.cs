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
    public partial class FrmCambiarClave_64PR : Form, Idioma.IObservadorIdioma_64PR
    {
        Dictionary<string, string> textos;
        public FrmCambiarClave_64PR()
        {
            InitializeComponent();

            txtContra.UseSystemPasswordChar = true;
            txtConfirmar.UseSystemPasswordChar = true;
            txtNueva.UseSystemPasswordChar = true;

            Idioma.GestorIdioma_64PR.GetInstance.Suscribir(this); ///observer del cambio de idioma

            ///Aplico el idioma que ya está cargado
            textos = Idioma.GestorIdioma_64PR.GetInstance.ObtenerTextos();
            if (textos.Count > 0)
                ActualizarIdioma(textos);
        }
        public void ActualizarIdioma(Dictionary<string, string> textoss)
        {
            textos = textoss;
            Traductor_64PR.Traducir(this, textos);
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            ///Validaciones de UI ANTES de llamar al BLL: asi el mensaje de error sale traducido
            ///segun el idioma actual (BLL_Usuario.CambiarClave tira Exception con texto fijo en espanol,
            ///que se muestra tal cual via ex.Message si esta validacion no la ataja antes).
            if (string.IsNullOrWhiteSpace(txtContra.Text) || string.IsNullOrWhiteSpace(txtNueva.Text) || string.IsNullOrWhiteSpace(txtConfirmar.Text))
            {
                MessageBox.Show(textos["msg_camposVacios"]);
                return;
            }
            if (txtNueva.Text.Trim() != txtConfirmar.Text.Trim())
            {
                MessageBox.Show(textos["msg_claveNoCoincide"]);
                return;
            }
            if (txtNueva.Text.Trim().Length < 8)
            {
                MessageBox.Show(textos["msg_claveCorta"]);
                return;
            }

            try
            {
                Sesion.BLL_Usuario gusuario = new Sesion.BLL_Usuario();

                ///Obtengo el hash en BD y comparo con la contraseña ingresada
                byte[] hashalmacenado = gusuario.ObtenerHashAlmacenado(Sesion.SessionManager.GetInstance.Usuario.Login);
                if (Encriptacion.Encriptación.Instancia.VerifyPassword(txtContra.Text.Trim(), hashalmacenado))
                {
                    if(txtContra.Text.Trim() == txtNueva.Text.Trim() && txtNueva.Text.Trim() == txtConfirmar.Text.Trim())
                    {
                        MessageBox.Show(textos["msg_claveActualComoNueva"]);
                    }
                    else
                    {

                        ///Linea que me cambia la contraseña
                        gusuario.CambiarClave(txtNueva.Text.Trim(), txtConfirmar.Text.Trim());

                        ///registro el evento en bitacora
                        Bitacora.Bitacora_64PR bita2 = new Bitacora.Bitacora_64PR();
                        Bitacora.Evento_64PR ev2 = new Bitacora.Evento_64PR(Sesion.SessionManager.GetInstance.Usuario.Login, ((int)Bitacora.Bitacora_64PR.ModuloBitacora_64PR.Login).ToString(), ((int)Bitacora.Bitacora_64PR.TipoEventoBitacora_64PR.CambioClave).ToString(), 2);
                        bita2.RegistrarEvento(ev2);

                        MessageBox.Show(textos["msg_cambio_exitoso"], "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;

                        FrmContenedor_64PR.Instancia.MostrarHijo(new FrmLogin_64PR());

                        ev2 = new Bitacora.Evento_64PR(Sesion.SessionManager.GetInstance.Usuario.Login, ((int)Bitacora.Bitacora_64PR.ModuloBitacora_64PR.Login).ToString(), ((int)Bitacora.Bitacora_64PR.TipoEventoBitacora_64PR.Logout).ToString(), 5);
                        bita2.RegistrarEvento(ev2);

                        Sesion.SessionManager.GetInstance.Logout();
                    }
                }
                else
                {
                    MessageBox.Show(textos["msg_contrasenaIncorrecta"]);
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                txtNueva.Clear();
                txtConfirmar.Clear();
            }
        }

        private void FrmCambiarClave_64PR_FormClosed(object sender, FormClosedEventArgs e)
        {
            Idioma.GestorIdioma_64PR.GetInstance.Desuscribir(this);
        }
    }
}
