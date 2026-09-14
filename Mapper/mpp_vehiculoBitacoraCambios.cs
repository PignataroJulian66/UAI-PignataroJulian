using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Mapper
{
    public class mpp_vehiculoBitacoraCambios
    {
        public List<BE.VehiculoBitacoraCambios> Listar(string patente, string marcaModelo, DateTime? fechaIni, DateTime? fechaFin)
        {
            List<BE.VehiculoBitacoraCambios> lista = new List<BE.VehiculoBitacoraCambios>();

            SqlParameter[] parametros = new SqlParameter[]
            {
                new SqlParameter("@Patente", (object)patente ?? DBNull.Value),
                new SqlParameter("@MarcaModelo", (object)marcaModelo ?? DBNull.Value),
                new SqlParameter("@FechaIni", (object)fechaIni ?? DBNull.Value),
                new SqlParameter("@FechaFin", (object)fechaFin ?? DBNull.Value)
            };

            DataTable tabla = DAL_64PR.Acceso.Instancia.leerQuery("SP_VehiculoBitacoraCambios_Listar", parametros, CommandType.StoredProcedure);

            foreach (DataRow dr in tabla.Rows)
            {
                BE.VehiculoBitacoraCambios b = new BE.VehiculoBitacoraCambios();
                b.IdBitacora = Convert.ToInt32(dr["IdBitacora"]);
                b.Patente = dr["Patente"].ToString();
                b.Marca = dr["Marca"].ToString();
                b.Modelo = dr["Modelo"].ToString();
                b.ID_Categoria = Convert.ToInt32(dr["ID_Categoria"]);
                b.Kilometraje = Convert.ToInt32(dr["Kilometraje"]);
                b.Estado = (BE.EstadoVehiculoJP86)Enum.Parse(typeof(BE.EstadoVehiculoJP86), dr["Estado"].ToString());
                b.Activo = Convert.ToBoolean(dr["Activo"]);
                b.Fecha = Convert.ToDateTime(dr["Fecha"]);
                b.Hora = (TimeSpan)dr["Hora"];
                b.Act = Convert.ToBoolean(dr["Act"]);
                lista.Add(b);
            }
            return lista;
        }

        public void Activar(int idBitacora)
        {
            SqlParameter[] parametros = new SqlParameter[] { new SqlParameter("@IdBitacora", idBitacora) };
            DAL_64PR.Acceso.Instancia.escribirQuery("SP_VehiculoBitacoraCambios_Activar", parametros, CommandType.StoredProcedure);
        }
    }
}
