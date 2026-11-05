using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ConfiguradorBDJP86
{
    ///Confirmacion explicita antes de un DROP: muestra servidor, BD y motivo, y exige escribir el nombre exacto de la BD.
    public partial class FrmConfirmarEliminacionJP86 : Form
    {
        #region Campos
        private readonly string _nombreBD;
        #endregion

        #region Propiedades
        ///Solo tiene efecto si el backup es opcional (desinstalacion). En reinstalacion el backup siempre se hace.
        public bool HacerBackup
        {
            get { return chkBackup.Checked; }
        }
        #endregion

        public FrmConfirmarEliminacionJP86(string servidor, string nombreBD, IEnumerable<string> motivos, bool backupOpcional)
        {
            InitializeComponent();
            _nombreBD = nombreBD;

            lblServidor.Text = "Servidor:  " + servidor;
            lblBaseDatos.Text = "Base de datos:  " + nombreBD;
            txtMotivo.Text = "- " + string.Join(Environment.NewLine + "- ", motivos);

            chkBackup.Visible = backupOpcional;
            lblAdvertencia.Text = "Se borrarán todas las tablas y todos los datos de esta base de datos. "
                + (backupOpcional
                    ? "Si marca la opción de abajo, antes se guarda una copia de seguridad (.bak) en la carpeta de backups de SQL Server."
                    : "Antes de eliminarla se guarda una copia de seguridad (.bak) en la carpeta de backups de SQL Server. La copia no se restaura: queda como resguardo para recuperar datos manualmente.");
        }

        #region Eventos
        private void txtConfirmacion_TextChanged(object sender, EventArgs e)
        {
            ///Coincidencia exacta (no se ignoran mayusculas): obliga a leer y escribir el nombre
            btnEliminar.Enabled = txtConfirmacion.Text.Trim() == _nombreBD;
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (txtConfirmacion.Text.Trim() != _nombreBD) return;
            DialogResult = DialogResult.OK;
            Close();
        }
        #endregion
    }
}
