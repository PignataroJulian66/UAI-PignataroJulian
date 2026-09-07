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
    public partial class FrmLogin_64PR : Form, Idioma.IObservadorIdioma_64PR
    {
        Sesion.BLL_Usuario gusuarios = new Sesion.BLL_Usuario();
        Dictionary<string, string> textos;
        public FrmLogin_64PR()
        {
            InitializeComponent();
            this.AcceptButton = btnIniciarSesion;
            txtContra.UseSystemPasswordChar = true;
            lblMensaje.Enabled = false;
            Idioma.GestorIdioma_64PR.GetInstance.Suscribir(this); ///Evento del observer

            CargarComboIdiomas();

            ///Aplico el idioma que ya está cargado
            textos = Idioma.GestorIdioma_64PR.GetInstance.ObtenerTextos();
            if (textos.Count > 0)
                ActualizarIdioma(textos);
            lblMensaje.Hide();
        }
        private void CargarComboIdiomas()
        {
            cmbIdioma.Items.Clear();

            foreach (string codigo in Idioma.GestorIdioma_64PR.GetInstance.IdiomasDisponibles())
                cmbIdioma.Items.Add(codigo.ToUpper()); /// "ES", "EN"

            ///Seleccionar el idioma actual
            string actual = Idioma.GestorIdioma_64PR.GetInstance.IdiomaActual.ToUpper();
            int index = cmbIdioma.Items.IndexOf(actual);
            if (index >= 0)
                cmbIdioma.SelectedIndex = index;

            cmbIdioma.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void cmbIdioma_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (cmbIdioma.SelectedItem == null) return;

            string seleccionado = cmbIdioma.SelectedItem.ToString().ToLower(); ///"es" o "en"
            Idioma.GestorIdioma_64PR.GetInstance.SetIdioma(seleccionado);
            ///El Observer se encarga de actualizar el formulario automáticamente
        }
        public void ActualizarIdioma(Dictionary<string, string> textoss)
        {
            ///Esto me actualiza los textos visibles
            textos = textoss;
            Traductor_64PR.Traducir(this, textos);
            if (lblMensaje.Enabled)
            {
                string[] aux = lblMensaje.Text.Split(':');
                aux[0]= textos.ContainsKey("FrmLogin_64PR.lblMensaje") ? textos["FrmLogin_64PR.lblMensaje"] : "Intentos";
                lblMensaje.Text = aux[0]+":" + aux[1];
            }
        }
        private void btnIniciarSesion_Click(object sender, EventArgs e)
        {
            var textos = Idioma.GestorIdioma_64PR.GetInstance.ObtenerTextos();

            if (string.IsNullOrEmpty(txtContra.Text.Trim()) || string.IsNullOrEmpty(txtLogin.Text.Trim()))
            {
                string msg = textos.ContainsKey("msg_camposVacios") ? textos["msg_camposVacios"] : "Completá todos los campos.";
                MessageBox.Show(msg, "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (Sesion.SessionManager.GetInstance.Usuario != null)
            {
                string msg = textos.ContainsKey("msg_sesionYaIniciada") ? textos["msg_sesionYaIniciada"] : "Ya hay una sesión activa.";
                MessageBox.Show(msg);
                return;
            }

            if (!gusuarios.ExisteUsuario(txtLogin.Text.Trim()))
            {
                string msg = textos.ContainsKey("msg_usuarioNoEncontrado") ? textos["msg_usuarioNoEncontrado"] : "Usuario no encontrado.";
                MessageBox.Show(msg);
                return;
            }

            if (gusuarios.BloqueadoInactivo(txtLogin.Text.Trim()))
            {
                string msg = textos.ContainsKey("msg_bloqueadoInactivo") ? textos["msg_bloqueadoInactivo"] : "El usuario se encuentra bloqueado o inactivo.";
                MessageBox.Show(msg);
                return;
            }

            if (gusuarios.VerificarClave(txtLogin.Text.Trim(), txtContra.Text.Trim()))
            {
                ///Cargo idioma del usuario desde la BD
                string idiomaGuardado = gusuarios.ObtenerIdioma(txtLogin.Text.Trim());
                Idioma.GestorIdioma_64PR.GetInstance.SetIdioma(idiomaGuardado);

                ///Obtengo los datos del usuario por el login y lo pongo en sesion
                Sesion.Usuario u = gusuarios.ObtenerUsuario(txtLogin.Text.Trim());
                Sesion.SessionManager.GetInstance.Login(u);

                ///Registro el evennto en bitacora
                Bitacora.Bitacora_64PR bita2 = new Bitacora.Bitacora_64PR();
                Bitacora.Evento_64PR ev2 = new Bitacora.Evento_64PR(Sesion.SessionManager.GetInstance.Usuario.Login, ((int)Bitacora.Bitacora_64PR.ModuloBitacora_64PR.Login).ToString(), ((int)Bitacora.Bitacora_64PR.TipoEventoBitacora_64PR.LoginExitoso).ToString(), 5);
                bita2.RegistrarEvento(ev2);

                if (Sesion.SessionManager.GetInstance.Usuario.PrimeraVez)
                {
                    string msgTemp = textos.ContainsKey("msg_contrasenaTemporal") ? textos["msg_contrasenaTemporal"] : "Su contraseña es temporal. Debe cambiarla antes de continuar.";
                    string tituTemp = textos.ContainsKey("msg_contrasenaTemporal_titulo") ? textos["msg_contrasenaTemporal_titulo"] : "Cambio de Contraseña Requerido";

                    MessageBox.Show(msgTemp, tituTemp, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    FrmContenedor_64PR.Instancia.MostrarHijo(new FrmCambiarClave_64PR());
                }
                else
                {
                    DV.DV_64PR bllDV = new DV.DV_64PR();
                    Dictionary<string, List<DV.FilaInconsistente_64PR>> tablasInconsistentes = bllDV.VerificarIntegridadCompleta();

                    if (tablasInconsistentes.Count > 0)
                    {
                        bool esAdmin = Sesion.SessionManager.GetInstance.Usuario.Rol.TienePermiso("Reparar Integridad DV");

                        if (esAdmin)
                        {
                            /// abrir el GUI de reparación, pasándole qué tablas fallaron
                            FrmContenedor_64PR.Instancia.MostrarHijo(new FrmReparacionDV_64PR(tablasInconsistentes));
                            return; /// no sigue al menú normal hasta que se resuelva
                        }
                        else
                        {
                            MessageBox.Show(
                                textos["msg_inconsistencia2"],
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            Bitacora.Bitacora_64PR bita = new Bitacora.Bitacora_64PR();
                            Bitacora.Evento_64PR ev = new Bitacora.Evento_64PR(Sesion.SessionManager.GetInstance.Usuario.Login, ((int)Bitacora.Bitacora_64PR.ModuloBitacora_64PR.Login).ToString(), ((int)Bitacora.Bitacora_64PR.TipoEventoBitacora_64PR.Logout).ToString(), 5);
                            bita.RegistrarEvento(ev);

                            Sesion.SessionManager.GetInstance.Logout();
                            return; /// bloquea el acceso, no abre FrmMenu
                        }
                    }
                    else
                    {
                        ///poner en 0 el contador de intentos en la base de datos por si erro a la contraseña
                        ///SE HACE TAN ABAJO XQ ME RECALCULA LA TABLA DE USUARIOS, LO CUAL PODRIA CUBRIR UNA INCONSISTENCIA EN LA TABLA
                        gusuarios.ReiniciarIntentos(txtLogin.Text.Trim());
                        FrmContenedor_64PR.Instancia.MostrarHijo(new FrmMenu());
                    }
                }
            }
            else
            {
                ///Si la contraseña no es correcta entra aca y sumamos un intento, registrandolo en bitacora
                gusuarios.SumarIntento(txtLogin.Text.Trim());
                Bitacora.Bitacora_64PR bita2 = new Bitacora.Bitacora_64PR();
                Bitacora.Evento_64PR ev2 = new Bitacora.Evento_64PR(txtLogin.Text, ((int)Bitacora.Bitacora_64PR.ModuloBitacora_64PR.Login).ToString(), ((int)Bitacora.Bitacora_64PR.TipoEventoBitacora_64PR.LoginFallido).ToString(), 4);
                bita2.RegistrarEvento(ev2);

                ///Obtenemos los intentos del usuario en base de datos y lo volcamos en el label
                string temp = gusuarios.ObtenerIntentos(txtLogin.Text.Trim());
                lblMensaje.Enabled = true;
                lblMensaje.Show();
                lblMensaje.Text = textos["FrmLogin_64PR.lblMensaje"] + ": " + temp + "/3";

                if (Convert.ToInt16(temp) == 3)
                {
                    ///Si los intentos llegan a 3 el bloqueo se hace desde la BD, aca lo que hago en registrar en la bitaora nomas
                    ev2 = new Bitacora.Evento_64PR(txtLogin.Text, ((int)Bitacora.Bitacora_64PR.ModuloBitacora_64PR.Login).ToString(), ((int)Bitacora.Bitacora_64PR.TipoEventoBitacora_64PR.UsuarioBloqueado).ToString(), 5);
                    bita2.RegistrarEvento(ev2);
                }

                string msgIncorrecta = textos.ContainsKey("msg_contrasenaIncorrecta") ? textos["msg_contrasenaIncorrecta"] : "Contraseña incorrecta.";
                MessageBox.Show(msgIncorrecta);
            }
        }

        private void FrmLogin_64PR_FormClosed(object sender, FormClosedEventArgs e)
        {
            Idioma.GestorIdioma_64PR.GetInstance.Desuscribir(this); ///observer del cambio de idioma
        }
    }
}
