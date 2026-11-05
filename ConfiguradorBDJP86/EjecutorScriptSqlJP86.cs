using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace ConfiguradorBDJP86
{
    ///Ejecuta el script de instalacion desde C#. SqlCommand no entiende "GO", asi que el texto se separa en lotes.
    ///Los lotes anteriores a la marca de conexion van contra master (DAL_64PR.Acceso.EjecutarComandoMaster);
    ///el resto, contra la BD recien creada (DAL_64PR.Acceso.EjecutarComandoSinTransaccion). No se abren conexiones propias.
    public class EjecutorScriptSqlJP86
    {
        #region Campos
        public const string TokenNombreBD = "{{NOMBRE_BD}}";
        public const string MarcaConexion = "-- @@SIGAM:CONECTAR_BD@@";
        public const string CarpetaScript = "BaseDatos";
        public const string ArchivoScript = "SIGAM_Instalacion.sql";
        private const int TimeoutLoteSegundos = 600;

        private static readonly Regex PatronGo = new Regex(@"^\s*GO\s*(--.*)?$", RegexOptions.IgnoreCase);
        ///Objetos que define el script (tablas, SP, triggers): es la lista de "objetos criticos" del diagnostico
        private static readonly Regex PatronObjeto = new Regex(
            @"^\s*CREATE\s+(?:OR\s+ALTER\s+)?(TABLE|PROCEDURE|PROC|TRIGGER)\s+(?:\[?dbo\]?\s*\.\s*)?\[?(\w+)\]?",
            RegexOptions.IgnoreCase | RegexOptions.Multiline);

        private readonly LogInstalacionJP86 _log = LogInstalacionJP86.Instancia;
        #endregion

        #region Propiedades
        public string RutaScript
        {
            get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, CarpetaScript, ArchivoScript); }
        }
        #endregion

        #region Consultas
        public string LeerScript()
        {
            if (!File.Exists(RutaScript))
                throw new FileNotFoundException("No se encontró el script de instalación de la base de datos.", RutaScript);
            return File.ReadAllText(RutaScript, Encoding.UTF8);
        }

        public static List<LoteSqlJP86> SepararLotes(string texto)
        {
            List<LoteSqlJP86> lotes = new List<LoteSqlJP86>();
            StringBuilder actual = new StringBuilder();
            bool contraMaster = true;
            int numeroLinea = 0, inicioLote = 1;

            foreach (string lineaCruda in texto.Replace("\r\n", "\n").Split('\n'))
            {
                numeroLinea++;
                string linea = lineaCruda.TrimEnd('\r');

                if (linea.Trim() == MarcaConexion)
                {
                    Cerrar(lotes, actual, contraMaster, inicioLote);
                    contraMaster = false;
                    inicioLote = numeroLinea + 1;
                    continue;
                }
                if (PatronGo.IsMatch(linea))
                {
                    Cerrar(lotes, actual, contraMaster, inicioLote);
                    inicioLote = numeroLinea + 1;
                    continue;
                }
                if (actual.Length == 0 && linea.Trim().Length == 0) { inicioLote = numeroLinea + 1; continue; }
                actual.AppendLine(linea);
            }
            Cerrar(lotes, actual, contraMaster, inicioLote);
            return lotes;
        }

        ///nombre de objeto -> tipo de sys.objects ('U' tabla, 'P' procedimiento, 'TR' trigger)
        public static Dictionary<string, string> ObtenerObjetosDefinidos(string texto)
        {
            Dictionary<string, string> objetos = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (Match m in PatronObjeto.Matches(texto))
            {
                string tipo = m.Groups[1].Value.ToUpperInvariant();
                objetos[m.Groups[2].Value] = tipo == "TABLE" ? "U" : tipo == "TRIGGER" ? "TR" : "P";
            }
            return objetos;
        }
        #endregion

        #region Operaciones
        ///progreso(loteActual, totalLotes)
        public void Ejecutar(string nombreBD, Action<int, int> progreso)
        {
            string texto = LeerScript().Replace(TokenNombreBD, nombreBD);
            List<LoteSqlJP86> lotes = SepararLotes(texto);
            _log.Info("Script: " + RutaScript + " (" + lotes.Count + " lotes)");

            bool primeroContraBD = true;
            foreach (LoteSqlJP86 lote in lotes)
            {
                if (!lote.ContraMaster && primeroContraBD)
                {
                    ///Recien creada la BD: se descartan conexiones previas del pool antes de conectarse a ella
                    SqlConnection.ClearAllPools();
                    primeroContraBD = false;
                }
                try
                {
                    if (lote.ContraMaster)
                        DAL_64PR.Acceso.Instancia.EjecutarComandoMaster(lote.Texto, null, TimeoutLoteSegundos);
                    else
                        DAL_64PR.Acceso.Instancia.EjecutarComandoSinTransaccion(lote.Texto, null, TimeoutLoteSegundos);
                }
                catch (Exception ex)
                {
                    _log.Error("Fallo el lote " + lote.Numero + "/" + lotes.Count + " (línea " + lote.LineaInicial + "): " + lote.Resumen, ex);
                    throw new InvalidOperationException("Falló la ejecución del script en el lote " + lote.Numero + " (línea " + lote.LineaInicial
                        + " de " + ArchivoScript + "): " + ex.Message, ex);
                }
                if (progreso != null) progreso(lote.Numero, lotes.Count);
            }
            _log.Info("Script ejecutado completo: " + lotes.Count + " lotes sin errores.");
        }
        #endregion

        #region Metodos privados
        private static void Cerrar(List<LoteSqlJP86> lotes, StringBuilder actual, bool contraMaster, int inicio)
        {
            string texto = actual.ToString();
            actual.Clear();
            if (texto.Trim().Length == 0) return;
            lotes.Add(new LoteSqlJP86 { Numero = lotes.Count + 1, LineaInicial = inicio, Texto = texto, ContraMaster = contraMaster });
        }
        #endregion
    }
}
