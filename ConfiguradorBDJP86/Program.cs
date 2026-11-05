using System;
using System.Threading;
using System.Windows.Forms;

namespace ConfiguradorBDJP86
{
    ///Configurador de la base de datos de SIGAM. Lo ejecuta Inno Setup:
    ///   ConfiguradorBDJP86.exe /modo=instalacion|reinstalacion /version=2.0   (al instalar)
    ///   ConfiguradorBDJP86.exe /modo=desinstalacion                          (al desinstalar, solo si se pidio borrar la BD)
    ///Sin parametros (acceso directo "Configurar base de datos") se comporta como reinstalacion: diagnostica antes de tocar nada.
    ///Codigo de salida: 0 correcto, 1 error, 2 cancelado.
    internal static class Program
    {
        [STAThread]
        private static int Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            LogInstalacionJP86 log = LogInstalacionJP86.Instancia;
            Application.ThreadException += (s, e) => log.Error("Error no controlado", e.Exception);
            AppDomain.CurrentDomain.UnhandledException += (s, e) => log.Error("Error no controlado", e.ExceptionObject as Exception);

            ParametrosEjecucionJP86 parametros = ParametrosEjecucionJP86.Parsear(args);
            log.Info("Inicio del configurador. Modo=" + parametros.Modo + ", version del programa=" + parametros.VersionPrograma
                     + ", usuario de Windows=" + Environment.UserDomainName + "\\" + Environment.UserName
                     + ", argumentos=" + string.Join(" ", args));

            if (parametros.Modo == ModoEjecucionJP86.Desinstalacion)
                return EjecutarDesinstalacion(parametros);

            FrmConfiguradorJP86 frm = new FrmConfiguradorJP86(parametros);
            Application.Run(frm);
            return frm.CodigoSalida;
        }

        ///Inno ya pregunto "¿eliminar tambien la base de datos?" (por defecto NO) y el usuario eligio SI.
        ///Aca se aplica la misma confirmacion explicita que en la reinstalacion: servidor + BD + escribir el nombre.
        private static int EjecutarDesinstalacion(ParametrosEjecucionJP86 parametros)
        {
            LogInstalacionJP86 log = LogInstalacionJP86.Instancia;
            const string titulo = "SIGAM - Desinstalación";
            GestorInstalacionJP86 gestor = new GestorInstalacionJP86(parametros);

            string servidor, nombreBD;
            if (!gestor.Configuracion.LeerConfiguracionSistema(out servidor, out nombreBD))
            {
                log.Advertencia("No hay configuracion de base de datos: no se elimina ninguna base.");
                MessageBox.Show("No se encontró la configuración de la base de datos de SIGAM. No se eliminó ninguna base de datos.",
                    titulo, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return FrmConfiguradorJP86.SalidaCorrecta;
            }

            try
            {
                gestor.ProbarConexion(servidor, nombreBD);
                if (!gestor.ExisteBaseDatos())
                {
                    MessageBox.Show("La base de datos \"" + nombreBD + "\" no existe en \"" + servidor + "\". No hay nada que eliminar.",
                        titulo, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return FrmConfiguradorJP86.SalidaCorrecta;
                }

                bool hacerBackup;
                using (FrmConfirmarEliminacionJP86 frm = new FrmConfirmarEliminacionJP86(servidor, nombreBD,
                    new[] { "Se pidió eliminar la base de datos junto con la desinstalación de SIGAM." }, true))
                {
                    if (frm.ShowDialog() != DialogResult.OK)
                    {
                        log.Info("El usuario no confirmo: se conserva la base de datos.");
                        MessageBox.Show("Se conservó la base de datos \"" + nombreBD + "\".", titulo, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return FrmConfiguradorJP86.SalidaCancelada;
                    }
                    hacerBackup = frm.HacerBackup;
                }

                string backup = null;
                if (hacerBackup)
                {
                    try { backup = gestor.HacerBackupSeguridad("antes_desinstalacion"); }
                    catch (Exception ex)
                    {
                        log.Error("No se pudo hacer el backup de seguridad", ex);
                        if (MessageBox.Show("No se pudo hacer el backup de seguridad.\n\n" + ex.Message + "\n\n¿Eliminar la base de datos de todos modos SIN backup?",
                            titulo, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                            return FrmConfiguradorJP86.SalidaCancelada;
                    }
                }

                Cursor.Current = Cursors.WaitCursor;
                gestor.EliminarBaseDatos();
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Se eliminó la base de datos \"" + nombreBD + "\" de \"" + servidor + "\"." +
                    (backup != null ? "\n\nBackup de seguridad:\n" + backup : string.Empty),
                    titulo, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return FrmConfiguradorJP86.SalidaCorrecta;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                log.Error("No se pudo eliminar la base de datos", ex);
                MessageBox.Show("No se pudo eliminar la base de datos \"" + nombreBD + "\".\n\n" + ex.Message +
                    "\n\nLa desinstalación de los archivos continúa. Si lo necesita, puede eliminarla manualmente desde SQL Server Management Studio.",
                    titulo, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return FrmConfiguradorJP86.SalidaError;
            }
        }
    }
}
