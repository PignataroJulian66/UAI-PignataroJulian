using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoIS_64PR
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ///Red de seguridad: ninguna excepcion no capturada llega al dialogo generico de .NET.
            ///ThreadException cubre el hilo de UI (incluidos los async void de los forms);
            ///UnhandledException cubre otros hilos, donde el proceso termina igual.
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += (s, e) => ManejadorErroresJP86.MostrarErrorInesperado(e.Exception);
            AppDomain.CurrentDomain.UnhandledException += (s, e) => ManejadorErroresJP86.MostrarErrorFatal(e.ExceptionObject as Exception);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ///[INSTALADOR v2] Sistema instalado sin BD configurada (config con #SQLINSTANCE#): se avisa y no se abre.
            ///El texto no pasa por el JSON de idioma: el idioma es por usuario y se lee de la BD, que aca no esta disponible.
            try
            {
                BLL_64PR.ConfiguracionSistemaJP86.VerificarConfiguracion();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "SIGAM", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Application.Run(new FrmContenedor_64PR());
            /*Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            FrmLogin_64PR formInicio = new FrmLogin_64PR();
            formInicio.Show();

            Application.Run();
            */
            ///<summary>
            ///se cambio el Application.Run(New FrmLogin_64PR) a Application.Run() simplemente
            ///ya que necesitabamos cerrar todos los formularios y volver a abrir el de login sin que el programa
            ///se detuviera al momento de cambiar la contraseña, ya que se deben cerrar todos los formularios abiertos,
            ///cerrar la sesion y abrir un nuevo formulario de login
            /// </summary>
        }
    }
}
