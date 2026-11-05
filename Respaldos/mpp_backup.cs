using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Respaldos
{
    public class mpp_backup
    {
        public void CrearCarpetaSiNoExiste(string ruta)
        {
            string query = "EXEC master.dbo.xp_create_subdir @ruta";
            SqlParameter[] parametros = new SqlParameter[] { new SqlParameter("@ruta", ruta) };
            DAL_64PR.Acceso.Instancia.escribirQuery(query, parametros);
        }

        public void GenerarBackup(string rutaCompleta)
        {
            // [INSTALADOR v2] código original:
            // string query = @"BACKUP DATABASE BD_64PR TO DISK = @ruta WITH INIT, STATS = 10";
            // SqlParameter[] parametros = new SqlParameter[]
            // {
            // new SqlParameter("@ruta", rutaCompleta)
            // };
            ///BACKUP DATABASE acepta una variable como nombre: se respalda la BD configurada, no una fija
            string query = @"BACKUP DATABASE @nombreBD TO DISK = @ruta WITH INIT, STATS = 10";
            SqlParameter[] parametros = new SqlParameter[]
            {
            new SqlParameter("@nombreBD", ObtenerNombreBaseDatos()),
            new SqlParameter("@ruta", rutaCompleta)
            };
            DAL_64PR.Acceso.Instancia.EjecutarComandoSinTransaccion(query, parametros, 600);
        }
        public DataTable ObtenerFileList(string rutaBackup)
        {
            string query = "RESTORE FILELISTONLY FROM DISK = @ruta";
            SqlParameter[] parametros = new SqlParameter[] { new SqlParameter("@ruta", rutaBackup) };
            return DAL_64PR.Acceso.Instancia.leerQuery(query, parametros);
        }

        public string ObtenerRutaDefaultData()
        {
            string query = "SELECT SERVERPROPERTY('InstanceDefaultDataPath') AS Ruta";
            object resultado = DAL_64PR.Acceso.Instancia.leerEscalar(query, null);
            return resultado?.ToString();
        }

        public void RestaurarBackup(string rutaBackup, string logicalData, string logicalLog,
                             string rutaDestinoMdf, string rutaDestinoLdf)
        {
            // [INSTALADOR v2] código original:
            //     string query = $@"
            // ALTER DATABASE BD_64PR SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
            // RESTORE DATABASE BD_64PR FROM DISK = @ruta
            // WITH MOVE '{logicalData}' TO '{rutaDestinoMdf}',
            //      MOVE '{logicalLog}' TO '{rutaDestinoLdf}',
            //      REPLACE, STATS = 10;
            // ALTER DATABASE BD_64PR SET MULTI_USER;";
            //
            //     SqlParameter[] parametros = new SqlParameter[] { new SqlParameter("@ruta", rutaBackup) };

            ///ALTER DATABASE no acepta variables: se arma con QUOTENAME (SQL dinamico). RESTORE si acepta @nombreBD.
            ///El IF DB_ID evita fallar si la BD destino todavia no existe (el RESTORE la crea).
            string query = $@"
        DECLARE @sql NVARCHAR(400);
        IF DB_ID(@nombreBD) IS NOT NULL
        BEGIN
            SET @sql = N'ALTER DATABASE ' + QUOTENAME(@nombreBD) + N' SET SINGLE_USER WITH ROLLBACK IMMEDIATE';
            EXEC (@sql);
        END
        RESTORE DATABASE @nombreBD FROM DISK = @ruta
        WITH MOVE '{logicalData}' TO '{rutaDestinoMdf}',
             MOVE '{logicalLog}' TO '{rutaDestinoLdf}',
             REPLACE, STATS = 10;
        SET @sql = N'ALTER DATABASE ' + QUOTENAME(@nombreBD) + N' SET MULTI_USER';
        EXEC (@sql);";

            SqlParameter[] parametros = new SqlParameter[]
            {
                new SqlParameter("@nombreBD", ObtenerNombreBaseDatos()),
                new SqlParameter("@ruta", rutaBackup)
            };
            DAL_64PR.Acceso.Instancia.EjecutarComandoMaster(query, parametros, 600);
        }

        ///[INSTALADOR v2] Nombre de la BD configurada en ConexionBD (reemplaza el "BD_64PR" fijo)
        public string ObtenerNombreBaseDatos()
        {
            return DAL_64PR.Acceso.Instancia.ObtenerNombreBaseDatos();
        }

        ///[INSTALADOR v2] Carpeta de backups por defecto de la instancia: la cuenta de servicio de SQL Server tiene permisos ahi
        public string ObtenerRutaDefaultBackup()
        {
            string query = "SELECT SERVERPROPERTY('InstanceDefaultBackupPath') AS Ruta";
            object resultado = DAL_64PR.Acceso.Instancia.leerEscalarMaster(query, null);
            return (resultado == null || resultado == DBNull.Value) ? null : resultado.ToString();
        }
    }
}
