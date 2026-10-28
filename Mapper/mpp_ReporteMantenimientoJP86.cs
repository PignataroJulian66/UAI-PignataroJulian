using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapper
{
    ///Reporte de Mantenimiento Preventivo: solo lectura, sin DV propio. Mapea los datos CRUDOS del SP;
    ///los calculos (km de revision, ritmo, proyeccion, semaforo, recomendacion) son de la BLL.
    public class mpp_ReporteMantenimientoJP86
    {
        #region Consultas
        public List<BE.ItemReporteMantenimientoJP86> Obtener(DateTime fechaReferencia, int diasVentana)
        {
            List<BE.ItemReporteMantenimientoJP86> lista = new List<BE.ItemReporteMantenimientoJP86>();

            SqlParameter[] parametros = new SqlParameter[]
            {
                new SqlParameter("@FechaReferencia", SqlDbType.Date) { Value = fechaReferencia.Date },
                new SqlParameter("@DiasVentana", diasVentana)
            };
            DataTable tabla = DAL_64PR.Acceso.Instancia.leerQuery("SP_ReporteMantenimientoPreventivoJP86_Obtener", parametros, CommandType.StoredProcedure);

            foreach (DataRow dr in tabla.Rows)
            {
                BE.ItemReporteMantenimientoJP86 item = new BE.ItemReporteMantenimientoJP86();

                BE.Categoria_64PR categoria = new BE.Categoria_64PR()
                {
                    Id = Convert.ToInt32(dr["ID_Categoria"]),
                    Nombre = dr["CategoriaNombre"].ToString()
                };
                item.Vehiculo = new BE.VehiculoJP86()
                {
                    Patente = dr["Patente"].ToString(),
                    Marca = dr["Marca"].ToString(),
                    Modelo = dr["Modelo"].ToString(),
                    Categoria = categoria,
                    Kilometraje = Convert.ToInt32(dr["Kilometraje"]),
                    Estado = (BE.EstadoVehiculoJP86)Enum.Parse(typeof(BE.EstadoVehiculoJP86), dr["Estado"].ToString()),
                    Activo = Convert.ToBoolean(dr["Activo"])
                };
                item.Categoria = categoria;
                item.KmActual = item.Vehiculo.Kilometraje;

                item.FechaUltimaRevision = dr["FechaUltimaRevision"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["FechaUltimaRevision"]);
                item.KmDevolucionPreviaRevision = dr["KmDevolucionPreviaRevision"] == DBNull.Value ? (int?)null : Convert.ToInt32(dr["KmDevolucionPreviaRevision"]);
                item.FechaPrimerContrato = dr["FechaPrimerContrato"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["FechaPrimerContrato"]);
                item.KmEntregaPrimerContrato = dr["KmEntregaPrimerContrato"] == DBNull.Value ? (int?)null : Convert.ToInt32(dr["KmEntregaPrimerContrato"]);
                item.KmRecorridosVentana = Convert.ToInt32(dr["KmRecorridosVentana"]);
                item.FechaPrimerContratoVentana = dr["FechaPrimerContratoVentana"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["FechaPrimerContratoVentana"]);
                item.ReportesInternos = Convert.ToInt32(dr["ReportesInternos"]);
                item.ReportesCliente = Convert.ToInt32(dr["ReportesCliente"]);

                lista.Add(item);
            }
            return lista;
        }
        #endregion
    }
}
