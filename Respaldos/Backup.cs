using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Respaldos
{
    public class Backup
    {
        Respaldos.mpp_backup mpp = new Respaldos.mpp_backup();

        public void CrearCarpetaSiNoExiste(string ruta)
        {
            mpp.CrearCarpetaSiNoExiste(ruta);
        }

        public void GenerarBackup(string rutaCompleta)
        {
            mpp.GenerarBackup(rutaCompleta);
        }
        public DataTable ObtenerFileList(string rutaBackup)
        {
            return mpp.ObtenerFileList(rutaBackup);
        }

        public string ObtenerRutaDefaultData()
        {
            return mpp.ObtenerRutaDefaultData();
        }

        public void RestaurarBackup(string rutaBackup, string logicalData, string logicalLog,
                                     string rutaDestinoMdf, string rutaDestinoLdf)
        {
            mpp.RestaurarBackup(rutaBackup, logicalData, logicalLog, rutaDestinoMdf, rutaDestinoLdf);
        }

        ///[INSTALADOR v2] Nombre de la BD configurada (para nombrar el .bak y los .mdf/.ldf al restaurar)
        public string ObtenerNombreBaseDatos()
        {
            return mpp.ObtenerNombreBaseDatos();
        }

        ///[INSTALADOR v2] Carpeta de backups por defecto de la instancia (la usa el configurador de BD)
        public string ObtenerRutaDefaultBackup()
        {
            return mpp.ObtenerRutaDefaultBackup();
        }
    }
}
