using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ConfiguradorBDJP86
{
    ///Diagnostico de reinstalacion: existe la BD, esta en linea, su version coincide con la del programa
    ///y tiene todas las tablas, SP y triggers que define el script de instalacion.
    public class DiagnosticoBDJP86
    {
        #region Campos
        private readonly LogInstalacionJP86 _log = LogInstalacionJP86.Instancia;
        private static readonly Dictionary<string, string> NombreTipo = new Dictionary<string, string>
        {
            { "U", "tabla" }, { "P", "procedimiento" }, { "TR", "trigger" }
        };
        #endregion

        #region Consultas
        public bool ExisteBaseDatos(string nombreBD)
        {
            object id = DAL_64PR.Acceso.Instancia.leerEscalarMaster("SELECT DB_ID(@n)", new[] { new SqlParameter("@n", nombreBD) });
            return id != null && id != DBNull.Value;
        }

        public ResultadoDiagnosticoJP86 Diagnosticar(string nombreBD, string versionEsperada, Dictionary<string, string> objetosCriticos)
        {
            ResultadoDiagnosticoJP86 r = new ResultadoDiagnosticoJP86 { VersionEsperada = versionEsperada };

            r.BaseExiste = ExisteBaseDatos(nombreBD);
            if (!r.BaseExiste)
            {
                r.Problemas.Add("La base de datos no existe.");
                return Registrar(r);
            }

            object estado = DAL_64PR.Acceso.Instancia.leerEscalarMaster(
                "SELECT state_desc FROM sys.databases WHERE name = @n", new[] { new SqlParameter("@n", nombreBD) });
            r.Estado = estado == null ? "?" : estado.ToString();
            if (r.Estado != "ONLINE")
            {
                r.Problemas.Add("La base de datos no está en línea (estado: " + r.Estado + ").");
                return Registrar(r);
            }

            DataTable existentes;
            try
            {
                ///leerQuery propaga los errores de conexion (a diferencia de leerEscalar), asi no se confunden con "faltantes"
                existentes = DAL_64PR.Acceso.Instancia.leerQuery(
                    "SELECT name, RTRIM(type) AS tipo FROM sys.objects WHERE type IN ('U','P','TR') AND is_ms_shipped = 0", null);
            }
            catch (Exception ex)
            {
                r.Problemas.Add("No se pudo leer la estructura de la base de datos: " + ex.Message);
                return Registrar(r);
            }

            HashSet<string> presentes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (DataRow fila in existentes.Rows)
                presentes.Add(fila["tipo"] + "|" + fila["name"]);

            ///Version
            if (presentes.Contains("U|VersionBDJP86"))
            {
                DataTable v = DAL_64PR.Acceso.Instancia.leerQuery("SELECT TOP 1 Version FROM dbo.VersionBDJP86 ORDER BY ID_Version DESC", null);
                r.VersionEncontrada = v.Rows.Count > 0 ? v.Rows[0][0].ToString().Trim() : null;
                if (r.VersionEncontrada == null)
                    r.Problemas.Add("La tabla de versión está vacía.");
                else if (!string.Equals(r.VersionEncontrada, versionEsperada, StringComparison.OrdinalIgnoreCase))
                    r.Problemas.Add("La versión de la base de datos (" + r.VersionEncontrada + ") no coincide con la del programa (" + versionEsperada + ").");
            }
            else
            {
                r.Problemas.Add("La base de datos no tiene tabla de versión (corresponde a una instalación anterior a la " + versionEsperada + ").");
            }

            ///Objetos criticos
            foreach (KeyValuePair<string, string> o in objetosCriticos)
            {
                if (!presentes.Contains(o.Value + "|" + o.Key))
                    r.ObjetosFaltantes.Add(NombreTipo[o.Value] + " " + o.Key);
            }
            if (r.ObjetosFaltantes.Count > 0)
            {
                string lista = string.Join(", ", r.ObjetosFaltantes.Count > 8 ? r.ObjetosFaltantes.GetRange(0, 8) : r.ObjetosFaltantes);
                if (r.ObjetosFaltantes.Count > 8) lista += " y " + (r.ObjetosFaltantes.Count - 8) + " más";
                r.Problemas.Add("Faltan " + r.ObjetosFaltantes.Count + " objetos: " + lista + ".");
            }
            return Registrar(r);
        }
        #endregion

        #region Metodos privados
        private ResultadoDiagnosticoJP86 Registrar(ResultadoDiagnosticoJP86 r)
        {
            _log.Info("Diagnostico: existe=" + r.BaseExiste + ", estado=" + (r.Estado ?? "-") + ", version=" + (r.VersionEncontrada ?? "-")
                      + " (esperada " + r.VersionEsperada + "), objetos faltantes=" + r.ObjetosFaltantes.Count);
            foreach (string p in r.Problemas) _log.Advertencia("Diagnostico: " + p);
            if (r.Correcto) _log.Info("Diagnostico: la base de datos esta correcta.");
            return r;
        }
        #endregion
    }
}
