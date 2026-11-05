using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConfiguradorBDJP86
{
    ///Pantalla del configurador (instalacion y reinstalacion). Solo orquesta: la logica esta en GestorInstalacionJP86.
    ///No es un formulario del sistema SIGAM: no requiere sesion, patentes ni Observer de idioma (se ejecuta antes
    ///de que exista la BD y el usuario) y registra todo en su propio log de texto, no en la bitacora.
    public partial class FrmConfiguradorJP86 : Form
    {
        #region Campos
        public const int SalidaCorrecta = 0;
        public const int SalidaError = 1;
        public const int SalidaCancelada = 2;

        private readonly GestorInstalacionJP86 _gestor;
        private readonly LogInstalacionJP86 _log = LogInstalacionJP86.Instancia;
        private bool _ocupado;
        private bool _finalizado;
        #endregion

        #region Propiedades
        public int CodigoSalida { get; private set; } = SalidaCancelada;
        #endregion

        public FrmConfiguradorJP86(ParametrosEjecucionJP86 parametros)
        {
            InitializeComponent();
            _gestor = new GestorInstalacionJP86(parametros);
            try { Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath); } catch (Exception) { }
            _log.LineaRegistrada += MostrarLinea;
        }

        #region Eventos
        private void FrmConfiguradorJP86_Load(object sender, EventArgs e)
        {
            lblModo.Text = "Modo: " + DescripcionModo() + "    |    Versión del programa: " + _gestor.Parametros.VersionPrograma;
            lblRutaLog.Text = "Log: " + _log.RutaArchivo;

            foreach (string s in ConfiguracionConexionJP86.SugerenciasServidor()) cmbServidor.Items.Add(s);

            string servidor, nombreBD;
            if (_gestor.Configuracion.LeerConfiguracionSistema(out servidor, out nombreBD))
            {
                ///Reinstalacion o reconfiguracion: se proponen los valores que ya tenia el sistema
                cmbServidor.Text = servidor;
                txtBD.Text = nombreBD;
                _log.Info("Configuracion existente encontrada: servidor=" + servidor + ", base de datos=" + nombreBD);
            }
            else
            {
                cmbServidor.Text = cmbServidor.Items.Count > 0 ? cmbServidor.Items[0].ToString() : @".\SQLEXPRESS";
                txtBD.Text = ConfiguracionConexionJP86.NombreBDPorDefecto;
            }

            if (!ConfiguracionConexionJP86.HayInstanciaLocal())
                _log.Advertencia("No se detecto SQL Server instalado en esta PC. Si la base de datos esta en otro equipo, indique su nombre (ej: SERVIDOR\\SQLEXPRESS).");
        }

        private void DatosConexion_Changed(object sender, EventArgs e)
        {
            lblEstadoConexion.Text = string.Empty;
        }

        private async void btnProbar_Click(object sender, EventArgs e)
        {
            await ProbarConexionAsync(false);
        }

        private async void btnContinuar_Click(object sender, EventArgs e)
        {
            if (!await ProbarConexionAsync(true)) return;
            await ConfigurarAsync();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FrmConfiguradorJP86_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_ocupado)
            {
                MessageBox.Show(this, "Espere a que termine la operación en curso.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                e.Cancel = true;
                return;
            }
            if (!_finalizado)
            {
                if (MessageBox.Show(this, "La base de datos no quedó configurada. Podrá hacerlo más tarde desde el acceso directo \"Configurar base de datos\" de SIGAM.\n\n¿Desea salir?",
                    Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                {
                    e.Cancel = true;
                    return;
                }
                _log.Advertencia("Configuracion cancelada por el usuario.");
            }
            _log.LineaRegistrada -= MostrarLinea;
        }
        #endregion

        #region Flujo
        ///Paso 1. Devuelve true si conecto. Si falla, ofrece reintentar (corregir datos) o cancelar.
        private async Task<bool> ProbarConexionAsync(bool continuar)
        {
            string servidor = cmbServidor.Text, nombreBD = txtBD.Text;
            Ocupado(true, "Probando conexión...");
            try
            {
                await Task.Run(() => _gestor.ProbarConexion(servidor, nombreBD));
                lblEstadoConexion.ForeColor = Color.DarkGreen;
                lblEstadoConexion.Text = "Conexión correcta (SQL Server " + _gestor.VersionServidor + ").";
                return true;
            }
            catch (ArgumentException ex)
            {
                lblEstadoConexion.ForeColor = Color.DarkRed;
                lblEstadoConexion.Text = ex.Message;
                return false;
            }
            catch (Exception ex)
            {
                _log.Error("No se pudo conectar", ex);
                lblEstadoConexion.ForeColor = Color.DarkRed;
                lblEstadoConexion.Text = "No se pudo conectar con el servidor indicado.";
                DialogResult r = MessageBox.Show(this,
                    "No se pudo conectar con SQL Server en \"" + servidor + "\".\n\n" + ex.Message +
                    "\n\nVerifique que:\n- SQL Server esté instalado y su servicio iniciado.\n- El nombre del servidor/instancia sea correcto (ej: .\\SQLEXPRESS).\n- Su usuario de Windows tenga acceso a SQL Server.\n\n" +
                    "Reintentar: corregir los datos y volver a probar.\nCancelar: salir sin configurar la base de datos.",
                    "No se pudo conectar", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                if (r == DialogResult.Cancel)
                {
                    Ocupado(false, null);
                    Finalizar(SalidaCancelada);
                }
                return false;
            }
            finally
            {
                if (!_finalizado) Ocupado(false, null);
            }
        }

        ///Pasos 2 a 7
        private async Task ConfigurarAsync()
        {
            Ocupado(true, "Configurando...");
            try
            {
                await Task.Run(() => _gestor.GuardarConfiguracionSistema());

                bool existe = await Task.Run(() => _gestor.ExisteBaseDatos());
                if (!existe)
                {
                    await CrearBaseDatosAsync();
                    MostrarResumen(true, null);
                    return;
                }

                ResultadoDiagnosticoJP86 diag = await Task.Run(() => _gestor.Diagnosticar());
                if (diag.Correcto)
                {
                    _log.Info("Se conserva la base de datos existente sin cambios. Solo se actualizaron los archivos del programa.");
                    MostrarResumen(false, null);
                    return;
                }

                ///La BD existe pero tiene problemas: informar, confirmar explicitamente, backup, DROP, script y DV.
                ///El backup NO se restaura (volveria a dejar la BD fallada): queda como resguardo para recuperar datos a mano.
                string backup = await ReemplazarBaseDatosAsync(diag);
                if (backup == null && _finalizado) return;
                MostrarResumen(true, backup);
            }
            catch (Exception ex)
            {
                _log.Error("La configuracion no pudo completarse", ex);
                Ocupado(false, null);
                MessageBox.Show(this, "La configuración de la base de datos no pudo completarse.\n\n" + ex.Message +
                    "\n\nRevise el log para más detalle:\n" + _log.RutaArchivo +
                    "\n\nPuede volver a intentarlo desde el acceso directo \"Configurar base de datos\" de SIGAM.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Finalizar(SalidaError);
            }
        }

        ///Devuelve la ruta del backup hecho (o null si se siguio sin backup). Si el usuario cancela, finaliza y devuelve null.
        private async Task<string> ReemplazarBaseDatosAsync(ResultadoDiagnosticoJP86 diag)
        {
            Ocupado(false, null);
            using (FrmConfirmarEliminacionJP86 frm = new FrmConfirmarEliminacionJP86(_gestor.Servidor, _gestor.NombreBD, diag.Problemas, false))
            {
                if (frm.ShowDialog(this) != DialogResult.OK)
                {
                    _log.Advertencia("El usuario no confirmo la eliminacion: la base de datos no se modifico.");
                    MessageBox.Show(this, "No se modificó la base de datos.\n\nEl sistema quedó configurado para usarla, pero puede no funcionar " +
                        "correctamente por los problemas detectados. Puede volver a ejecutar \"Configurar base de datos\" cuando lo desee.",
                        Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Finalizar(SalidaCancelada);
                    return null;
                }
            }
            _log.Info("El usuario confirmo la eliminacion escribiendo el nombre de la base de datos.");

            string backup = null;
            Ocupado(true, "Haciendo backup de seguridad...");
            try
            {
                backup = await Task.Run(() => _gestor.HacerBackupSeguridad("antes_reinstalacion"));
            }
            catch (Exception ex)
            {
                _log.Error("No se pudo hacer el backup de seguridad", ex);
                Ocupado(false, null);
                if (MessageBox.Show(this, "No se pudo hacer el backup de seguridad de la base de datos.\n\n" + ex.Message +
                        "\n\n¿Desea eliminarla de todos modos SIN backup? Los datos actuales se perderán.",
                        "Backup fallido", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                {
                    _log.Advertencia("Se cancelo la reinstalacion por falta de backup. La base de datos no se modifico.");
                    Finalizar(SalidaCancelada);
                    return null;
                }
                _log.Advertencia("El usuario decidio continuar sin backup.");
                Ocupado(true, null);
            }

            Ocupado(true, "Eliminando la base de datos...");
            await Task.Run(() => _gestor.EliminarBaseDatos());
            await CrearBaseDatosAsync();
            return backup;
        }

        private async Task CrearBaseDatosAsync()
        {
            Ocupado(true, "Creando la base de datos...");
            barProgreso.Value = 0;
            await Task.Run(() => _gestor.CrearBaseDatos((actual, total) =>
                BeginInvoke((Action)(() => { barProgreso.Maximum = total; barProgreso.Value = Math.Min(actual, total); }))));
        }

        private void MostrarResumen(bool bdNueva, string backup)
        {
            string texto = "La configuración terminó correctamente.\n\nServidor: " + _gestor.Servidor + "\nBase de datos: " + _gestor.NombreBD + "\n\n";
            if (bdNueva)
            {
                texto += "Credenciales iniciales del administrador:\n    Usuario: " + GestorInstalacionJP86.LoginInicial +
                         "\n    Contraseña: " + GestorInstalacionJP86.ClaveInicial +
                         "\n\nPor seguridad, el sistema le pedirá cambiar la contraseña en el primer ingreso. " +
                         "Le recomendamos elegir una contraseña segura y no compartirla.";
            }
            else
            {
                texto += "Se conservó la base de datos existente con todos sus datos. Ingrese con sus usuarios habituales.";
            }
            if (backup != null)
            {
                texto += "\n\nBackup de seguridad de la base anterior (no se restauró; queda como resguardo para recuperar datos manualmente):\n" + backup;
                _log.Info("Backup de seguridad conservado como resguardo (no se restaura): " + backup);
            }
            texto += "\n\nLog de la instalación:\n" + _log.RutaArchivo;

            _log.Info("Resumen final: " + texto.Replace("\n", " | ").Replace(GestorInstalacionJP86.ClaveInicial, "********"));
            Ocupado(false, null);
            barProgreso.Value = barProgreso.Maximum;
            MessageBox.Show(this, texto, "Configuración finalizada", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Finalizar(SalidaCorrecta);
        }
        #endregion

        #region Metodos privados
        private void Finalizar(int codigo)
        {
            CodigoSalida = codigo;
            _finalizado = true;
            _log.Info("Fin del configurador. Codigo de salida: " + codigo);
            Close();
        }

        private void Ocupado(bool ocupado, string estado)
        {
            _ocupado = ocupado;
            grpConexion.Enabled = !ocupado;
            btnContinuar.Enabled = !ocupado;
            btnCancelar.Enabled = !ocupado;
            UseWaitCursor = ocupado;
            if (estado != null) _log.Info(estado);
        }

        private void MostrarLinea(string linea)
        {
            if (IsDisposed) return;
            if (InvokeRequired) { BeginInvoke((Action<string>)MostrarLinea, linea); return; }
            txtLog.AppendText(linea + Environment.NewLine);
        }

        private string DescripcionModo()
        {
            switch (_gestor.Parametros.Modo)
            {
                case ModoEjecucionJP86.Instalacion: return "Instalación";
                case ModoEjecucionJP86.Reinstalacion: return "Reinstalación / reconfiguración";
                default: return _gestor.Parametros.Modo.ToString();
            }
        }
        #endregion
    }
}
