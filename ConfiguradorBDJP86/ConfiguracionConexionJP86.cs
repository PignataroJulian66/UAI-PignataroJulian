using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace ConfiguradorBDJP86
{
    ///Lee y escribe las cadenas de conexion.
    /// - En el .config del SISTEMA (ProyectoIS_64PR.exe.config): para que SIGAM use la BD elegida.
    /// - En el .config del CONFIGURADOR: DAL_64PR.Acceso lee ConfigurationManager del proceso que lo usa,
    ///   asi la DV y el backup existentes (que van por Acceso) trabajan sobre la BD elegida sin cambiar su codigo.
    public class ConfiguracionConexionJP86
    {
        #region Campos
        public const string NombreBDPorDefecto = "SIGAM";
        public const string ArchivoConfigSistema = "ProyectoIS_64PR.exe.config";
        private const string MarcadorSinConfigurar = "#SQLINSTANCE#";
        private const string Proveedor = "System.Data.SqlClient";
        private const string ClaveInstancias = @"SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL";

        ///Solo letras, numeros y guion bajo: el nombre se usa dentro de CREATE/DROP DATABASE
        private static readonly Regex PatronNombreBD = new Regex(@"^[A-Za-z_][A-Za-z0-9_]{0,99}$");
        private static readonly string[] BasesDelSistema = { "master", "model", "msdb", "tempdb" };
        #endregion

        #region Propiedades
        public string RutaConfigSistema
        {
            get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ArchivoConfigSistema); }
        }
        #endregion

        #region Consultas
        public static bool NombreBDValido(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre) || !PatronNombreBD.IsMatch(nombre.Trim())) return false;
            foreach (string sistema in BasesDelSistema)
                if (string.Equals(nombre.Trim(), sistema, StringComparison.OrdinalIgnoreCase)) return false;
            return true;
        }

        public static string ArmarCadena(string servidor, string catalogo)
        {
            SqlConnectionStringBuilder b = new SqlConnectionStringBuilder
            {
                DataSource = servidor.Trim(),
                InitialCatalog = catalogo.Trim(),
                IntegratedSecurity = true,
                TrustServerCertificate = true
            };
            return b.ConnectionString;
        }

        ///Servidor y BD que ya tenia configurados el sistema (instalacion anterior). False si no hay datos validos.
        public bool LeerConfiguracionSistema(out string servidor, out string nombreBD)
        {
            servidor = null;
            nombreBD = null;
            if (!File.Exists(RutaConfigSistema)) return false;
            try
            {
                Configuration cfg = AbrirConfigSistema();
                ConnectionStringSettings cs = cfg.ConnectionStrings.ConnectionStrings["ConexionBD"];
                if (cs == null || string.IsNullOrWhiteSpace(cs.ConnectionString) || cs.ConnectionString.Contains(MarcadorSinConfigurar))
                    return false;

                SqlConnectionStringBuilder b = new SqlConnectionStringBuilder(cs.ConnectionString);
                if (string.IsNullOrWhiteSpace(b.DataSource) || string.IsNullOrWhiteSpace(b.InitialCatalog)) return false;
                servidor = b.DataSource;
                nombreBD = b.InitialCatalog;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        ///Instancias de SQL Server instaladas en esta PC (registro de 64 y 32 bits) + sugerencias habituales.
        ///Primero las instancias con nombre (ej: .\SQLEXPRESS, la mas comun en una PC de usuario) y despues la predeterminada.
        public static List<string> SugerenciasServidor()
        {
            List<string> lista = new List<string>();
            bool hayPredeterminada = false;
            foreach (string instancia in InstanciasLocales())
            {
                if (instancia.Equals("MSSQLSERVER", StringComparison.OrdinalIgnoreCase)) hayPredeterminada = true;
                else Agregar(lista, @".\" + instancia);
            }
            if (hayPredeterminada) Agregar(lista, "localhost");
            Agregar(lista, @".\SQLEXPRESS");
            Agregar(lista, "localhost");
            Agregar(lista, "(local)");
            return lista;
        }

        public static bool HayInstanciaLocal()
        {
            return InstanciasLocales().Count > 0;
        }
        #endregion

        #region Escritura
        ///Apunta DAL_64PR.Acceso (dentro de este proceso) al servidor y BD indicados
        public void AplicarAlConfigurador(string servidor, string nombreBD)
        {
            Configuration cfg = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            EscribirCadenas(cfg, servidor, nombreBD);
            ConfigurationManager.RefreshSection("connectionStrings");

            ///Control de seguridad: si Acceso no tomo la BD indicada (ej: config no escribible) no se sigue,
            ///porque Acceso usaria su cadena de respaldo y podria operar sobre otra base de datos.
            string efectiva = DAL_64PR.Acceso.Instancia.ObtenerNombreBaseDatos();
            if (!string.Equals(efectiva, nombreBD.Trim(), StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("No se pudo aplicar la configuración de conexión al configurador (se obtuvo '" + efectiva + "').");
        }

        ///Escribe ConexionBD y ConexionMaster en el .config del sistema conservando solo servidor y BD.
        ///Si el archivo no existe o no se puede interpretar, se genera uno nuevo (el anterior se guarda como .anterior).
        public void GuardarEnSistema(string servidor, string nombreBD)
        {
            string ruta = RutaConfigSistema;
            Configuration cfg = null;
            if (File.Exists(ruta))
            {
                try { cfg = AbrirConfigSistema(); }
                catch (ConfigurationErrorsException)
                {
                    File.Copy(ruta, ruta + ".anterior", true);
                    cfg = null;
                }
            }
            if (cfg == null)
            {
                File.WriteAllText(ruta, PlantillaConfig(), Encoding.UTF8);
                cfg = AbrirConfigSistema();
            }
            EscribirCadenas(cfg, servidor, nombreBD);
        }
        #endregion

        #region Metodos privados
        private static List<string> InstanciasLocales()
        {
            List<string> instancias = new List<string>();
            foreach (RegistryView vista in new[] { RegistryView.Registry64, RegistryView.Registry32 })
            {
                try
                {
                    using (RegistryKey raiz = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, vista))
                    using (RegistryKey clave = raiz.OpenSubKey(ClaveInstancias))
                    {
                        if (clave == null) continue;
                        foreach (string instancia in clave.GetValueNames()) Agregar(instancias, instancia);
                    }
                }
                catch (Exception) { /* sin permisos de lectura o sin la clave: se usan las sugerencias fijas */ }
            }
            return instancias;
        }

        private Configuration AbrirConfigSistema()
        {
            ExeConfigurationFileMap mapa = new ExeConfigurationFileMap { ExeConfigFilename = RutaConfigSistema };
            return ConfigurationManager.OpenMappedExeConfiguration(mapa, ConfigurationUserLevel.None);
        }

        private static void EscribirCadenas(Configuration cfg, string servidor, string nombreBD)
        {
            ConnectionStringSettingsCollection cs = cfg.ConnectionStrings.ConnectionStrings;
            Reemplazar(cs, "ConexionBD", ArmarCadena(servidor, nombreBD));
            Reemplazar(cs, "ConexionMaster", ArmarCadena(servidor, "master"));
            cfg.Save(ConfigurationSaveMode.Modified);
        }

        private static void Reemplazar(ConnectionStringSettingsCollection cs, string nombre, string cadena)
        {
            if (cs[nombre] != null) cs.Remove(nombre);
            cs.Add(new ConnectionStringSettings(nombre, cadena, Proveedor));
        }

        private static void Agregar(List<string> lista, string valor)
        {
            foreach (string s in lista)
                if (string.Equals(s, valor, StringComparison.OrdinalIgnoreCase)) return;
            lista.Add(valor);
        }

        private static string PlantillaConfig()
        {
            return "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n<configuration>\r\n\t<startup>\r\n" +
                   "\t\t<supportedRuntime version=\"v4.0\" sku=\".NETFramework,Version=v4.7.2\" />\r\n\t</startup>\r\n" +
                   "\t<connectionStrings />\r\n</configuration>\r\n";
        }
        #endregion
    }
}
