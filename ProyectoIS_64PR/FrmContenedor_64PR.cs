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
    public partial class FrmContenedor_64PR : Form
    {
        public static FrmContenedor_64PR Instancia { get; private set; }

        public FrmContenedor_64PR()
        {
            InitializeComponent();
            Instancia = this;
            this.WindowState = FormWindowState.Normal;
            MostrarHijo(new FrmLogin_64PR());
            this.Text = "SIGAM";
        }
        public void MostrarHijo(Form hijo)
        {
            /// Cierra el hijo anterior
            foreach (Control c in pnlContenido.Controls)
            {
                if (c is Form f) f.Close();
            }
            pnlContenido.Controls.Clear();

            /// Embebe el nuevo form como control dentro del panel
            hijo.TopLevel = false;
            hijo.FormBorderStyle = FormBorderStyle.None;
            hijo.Dock = DockStyle.Fill;
            pnlContenido.Controls.Add(hijo);
            hijo.Show();
        }

        private void FrmContenedor_64PR_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Sesion.SessionManager.GetInstance.Usuario != null)
            {
                string loginActual = Sesion.SessionManager.GetInstance.Usuario.Login;
                string idiomaActual = Idioma.GestorIdioma_64PR.GetInstance.IdiomaActual;
                new Sesion.BLL_Usuario().GuardarIdioma(loginActual, idiomaActual);

                Bitacora.Bitacora_64PR bita2 = new Bitacora.Bitacora_64PR();
                Bitacora.Evento_64PR ev2 = new Bitacora.Evento_64PR(Sesion.SessionManager.GetInstance.Usuario.Login, ((int)Bitacora.Bitacora_64PR.ModuloBitacora_64PR.Login).ToString(), ((int)Bitacora.Bitacora_64PR.TipoEventoBitacora_64PR.Logout).ToString(), 5);
                bita2.RegistrarEvento(ev2);

                Sesion.SessionManager.GetInstance.Logout();
            }
        }
    }
}
