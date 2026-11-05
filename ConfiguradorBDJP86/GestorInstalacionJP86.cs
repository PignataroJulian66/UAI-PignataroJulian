using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace ConfiguradorBDJP86
{
    ///Logica de instalacion / reinstalacion / desinstalacion de la BD. No muestra nada en pantalla:
    ///las confirmaciones las pide el formulario, que solo orquesta.
    ///Reutiliza DAL_64PR.Acceso (conexion), Respaldos.Backup (backup) y DV.DV_64PR (digito verificador).
    ///El backup de seguridad previo a un DROP no se restaura: queda como resguardo para recuperar datos manualmente.
    public class GestorInstalacionJP86
    {
        #region Campos
        ///Credenciales del unico usuario que crea el script (PrimeraVez = 1: el sistema obliga a cambiarla)
        public const string LoginInicial = "admin";
        public const string ClaveInicial = "admin123";
        private const string CarpetaBackupRespaldo = @"C:\Backups_64PR";

        private readonly LogInstalacionJP86 _log = LogInstalacionJP86.Instancia;
        private readonly DiagnosticoBDJP86 _diagnostico = new DiagnosticoBDJP86();
        private readonly EjecutorScriptSqlJP86 _ejecutor = new EjecutorScriptSqlJP86();
        #endregion

        #region Propiedades
        public ParametrosEjecucionJP86 Parametros { get; private set; }
        public ConfiguracionConexionJP86 Configuracion { get; private set; }
        public string Servidor { get; private set; }
        public string NombreBD { get; private set; }
        public string VersionServidor { get; private set; }
        public bool Conectado { get; private set; }
        #endregion

        public GestorInstalacionJP86(ParametrosEjecucionJP86 parametros)
        {
            Parametros = parametros;
            Configuracion = new ConfiguracionConexionJP86();
        }

        #region Operaciones
        ///Paso 1: valida los datos, apunta Acceso al servidor/BD y prueba la conexion contra master.
        ///Lanza la excepcion de SQL si no se puede conectar (el formulario ofrece reintentar o cancelar).
        public void ProbarConexion(string servidor, string nombreBD)
        {
            Conectado = false;
            if (string.IsNullOrWhiteSpace(servidor))
                throw new ArgumentException("Debe indicar el servidor de SQL Server.");
            if (!ConfiguracionConexionJP86.NombreBDValido(nombreBD))
                throw new ArgumentException("El nombre de la base de datos solo puede tener letras, números y guion bajo, debe empezar con una letra y no puede ser una base del sistema.");

            Servidor = servidor.Trim();
            NombreBD = nombreBD.Trim();
            _log.Info("Probando conexion: servidor=" + Servidor + ", base de datos=" + NombreBD);

            Configuracion.AplicarAlConfigurador(Servidor, NombreBD);
            SqlConnection.ClearAllPools();
            object version = DAL_64PR.Acceso.Instancia.leerEscalarMaster(
                "SELECT CAST(SERVERPROPERTY('ProductVersion') AS nvarchar(64)) + N' - ' + CAST(SERVERPROPERTY('Edition') AS nvarchar(128))", null, 15);
            VersionServidor = version == null ? "?" : version.ToString();
            Conectado = true;
            _log.Info("Conexion correcta. SQL Server " + VersionServidor);
        }

        ///Paso 2: escribe la cadena de conexion en el .config del sistema instalado
        public void GuardarConfiguracionSistema()
        {
            ExigirConexion();
            Configuracion.GuardarEnSistema(Servidor, NombreBD);
            _log.Info("Cadena de conexion escrita en " + Configuracion.RutaConfigSistema);
        }

        public bool ExisteBaseDatos()
        {
            ExigirConexion();
            bool existe = _diagnostico.ExisteBaseDatos(NombreBD);
            _log.Info("La base de datos " + NombreBD + (existe ? " existe." : " no existe."));
            return existe;
        }

        public ResultadoDiagnosticoJP86 Diagnosticar()
        {
            ExigirConexion();
            Dictionary<string, string> criticos = EjecutorScriptSqlJP86.ObtenerObjetosDefinidos(_ejecutor.LeerScript());
            _log.Info("Diagnosticando " + NombreBD + " contra " + criticos.Count + " objetos criticos del script.");
            return _diagnostico.Diagnosticar(NombreBD, Parametros.VersionPrograma, criticos);
        }

        ///Backup de la BD existente en la carpeta de backups por defecto de la instancia
        ///(la cuenta de servicio de SQL Server tiene permisos de escritura ahi). Devuelve la ruta del .bak.
        public string HacerBackupSeguridad(string motivo)
        {
            ExigirConexion();
            Respaldos.Backup backup = new Respaldos.Backup();

            string carpeta = backup.ObtenerRutaDefaultBackup();
            if (string.IsNullOrWhiteSpace(carpeta)) carpeta = backup.ObtenerRutaDefaultData();
            if (string.IsNullOrWhiteSpace(carpeta))
            {
                carpeta = CarpetaBackupRespaldo;
                backup.CrearCarpetaSiNoExiste(carpeta);
            }

            string ruta = Path.Combine(carpeta, NombreBD + "_" + motivo + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".bak");
            _log.Info("Generando backup de seguridad: " + ruta);
            backup.GenerarBackup(ruta);
            _log.Info("Backup de seguridad generado.");
            return ruta;
        }

        ///DROP de la BD con SINGLE_USER WITH ROLLBACK IMMEDIATE (cierra las conexiones abiertas)
        public void EliminarBaseDatos()
        {
            ExigirConexion();
            _log.Advertencia("Eliminando la base de datos " + NombreBD + " en " + Servidor);
            SqlConnection.ClearAllPools();
            const string sql = @"
DECLARE @sql NVARCHAR(400) = N'ALTER DATABASE ' + QUOTENAME(@n) + N' SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE ' + QUOTENAME(@n) + N';';
IF DB_ID(@n) IS NOT NULL EXEC (@sql);";
            DAL_64PR.Acceso.Instancia.EjecutarComandoMaster(sql, new[] { new SqlParameter("@n", NombreBD) }, 600);
            SqlConnection.ClearAllPools();

            if (_diagnostico.ExisteBaseDatos(NombreBD))
                throw new InvalidOperationException("La base de datos " + NombreBD + " no pudo eliminarse.");
            _log.Info("Base de datos eliminada.");
        }

        ///Ejecuta el script (estructura + datos minimos) y recalcula el DV. progreso(lote, total)
        public void CrearBaseDatos(Action<int, int> progreso)
        {
            ExigirConexion();
            _log.Info("Creando la base de datos " + NombreBD + " desde el script de instalacion.");
            _ejecutor.Ejecutar(NombreBD, progreso);
            int inconsistentes = RecalcularDV();
            if (inconsistentes > 0)
                throw new InvalidOperationException("El dígito verificador quedó con " + inconsistentes + " tablas inconsistentes.");
        }

        ///Recalcula DVH/DVV de todas las tablas registradas en DV.DV_64PR y devuelve cuantas quedaron inconsistentes
        public int RecalcularDV()
        {
            ExigirConexion();
            DV.DV_64PR dv = new DV.DV_64PR();
            dv.RecalcularIntegridadCompleta();
            int inconsistentes = dv.VerificarIntegridadCompleta().Count;
            _log.Info("Digito verificador recalculado (" + DV.DV_64PR.Tablas.Count + " tablas). Tablas inconsistentes: " + inconsistentes);
            return inconsistentes;
        }
        #endregion

        #region Metodos privados
        private void ExigirConexion()
        {
            if (!Conectado) throw new InvalidOperationException("Primero debe probarse la conexión con SQL Server.");
        }
        #endregion
    }
}
