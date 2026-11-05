using System;
using System.IO;
using System.Text;

namespace ConfiguradorBDJP86
{
    ///Log de instalacion en archivo de texto: <directorio de instalacion>\Logs\Configurador_aaaaMMdd_HHmmss.log
    ///(si no se puede escribir ahi, en %TEMP%). Ademas avisa cada linea a la pantalla.
    public class LogInstalacionJP86
    {
        #region Campos
        private static LogInstalacionJP86 _instancia;
        private readonly object _bloqueo = new object();
        #endregion

        #region Propiedades
        public string RutaArchivo { get; private set; }
        public event Action<string> LineaRegistrada;

        public static LogInstalacionJP86 Instancia
        {
            get
            {
                if (_instancia == null) _instancia = new LogInstalacionJP86();
                return _instancia;
            }
        }
        #endregion

        private LogInstalacionJP86()
        {
            string nombre = "Configurador_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".log";
            try
            {
                string carpeta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
                Directory.CreateDirectory(carpeta);
                RutaArchivo = Path.Combine(carpeta, nombre);
                File.AppendAllText(RutaArchivo, string.Empty, Encoding.UTF8);
            }
            catch (Exception)
            {
                RutaArchivo = Path.Combine(Path.GetTempPath(), "SIGAM_" + nombre);
            }
        }

        #region Operaciones
        public void Info(string mensaje) { Registrar("INFO ", mensaje); }
        public void Advertencia(string mensaje) { Registrar("AVISO", mensaje); }
        public void Error(string mensaje, Exception ex = null)
        {
            Registrar("ERROR", ex == null ? mensaje : mensaje + " | " + ex.GetType().Name + ": " + ex.Message);
        }
        #endregion

        #region Metodos privados
        private void Registrar(string nivel, string mensaje)
        {
            string linea = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " [" + nivel + "] " + mensaje;
            lock (_bloqueo)
            {
                try { File.AppendAllText(RutaArchivo, linea + Environment.NewLine, Encoding.UTF8); }
                catch (Exception) { /* el log nunca debe cortar la instalacion */ }
            }
            Action<string> aviso = LineaRegistrada;
            if (aviso != null) aviso(linea);
        }
        #endregion
    }
}
