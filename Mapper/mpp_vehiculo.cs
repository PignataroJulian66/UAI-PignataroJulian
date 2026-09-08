using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapper
{
    public class mpp_vehiculo
    {
        public List<BE.VehiculoJP86> Listar()
        {
            List<BE.VehiculoJP86> lista = new List<BE.VehiculoJP86>();

            DataTable tabla = DAL_64PR.Acceso.Instancia.leerQuery("SP_VehiculoJP86_Listar", null, CommandType.StoredProcedure);

            foreach (DataRow dr in tabla.Rows)
            {
                BE.VehiculoJP86 v = new BE.VehiculoJP86();
                v.Patente = dr["Patente"].ToString();
                v.Marca = dr["Marca"].ToString();
                v.Modelo = dr["Modelo"].ToString();
                v.Categoria = new BE.Categoria_64PR()
                {
                    Id = Convert.ToInt32(dr["ID_Categoria"]),
                    Nombre = dr["CategoriaNombre"].ToString(),
                    TarifaDiaria = Convert.ToDecimal(dr["TarifaDiaria"])
                };
                v.Kilometraje = Convert.ToInt32(dr["Kilometraje"]);
                v.Estado = (BE.EstadoVehiculoJP86)Enum.Parse(typeof(BE.EstadoVehiculoJP86), dr["Estado"].ToString());
                v.Activo = Convert.ToBoolean(dr["Activo"]);
                lista.Add(v);
            }
            return lista;
        }

        public void Crear(BE.VehiculoJP86 v)
        {
            SqlParameter[] parametros = new SqlParameter[]
            {
                new SqlParameter("@Patente", v.Patente),
                new SqlParameter("@Marca", v.Marca),
                new SqlParameter("@Modelo", v.Modelo),
                new SqlParameter("@ID_Categoria", v.Categoria.Id),
                new SqlParameter("@Kilometraje", v.Kilometraje)
            };
            DAL_64PR.Acceso.Instancia.escribirQuery("SP_VehiculoJP86_Crear", parametros, CommandType.StoredProcedure);
        }

        public void Modificar(BE.VehiculoJP86 v)
        {
            SqlParameter[] parametros = new SqlParameter[]
            {
                new SqlParameter("@Patente", v.Patente),
                new SqlParameter("@Marca", v.Marca),
                new SqlParameter("@Modelo", v.Modelo),
                new SqlParameter("@ID_Categoria", v.Categoria.Id),
                new SqlParameter("@Kilometraje", v.Kilometraje)
            };
            DAL_64PR.Acceso.Instancia.escribirQuery("SP_VehiculoJP86_Modificar", parametros, CommandType.StoredProcedure);
        }

        public void CambiarEstado(string patente, BE.EstadoVehiculoJP86 estado)
        {
            SqlParameter[] parametros = new SqlParameter[]
            {
                new SqlParameter("@Patente", patente),
                new SqlParameter("@Estado", estado.ToString())
            };
            DAL_64PR.Acceso.Instancia.escribirQuery("SP_VehiculoJP86_CambiarEstado", parametros, CommandType.StoredProcedure);
        }

        public void ActDesact(BE.VehiculoJP86 v)
        {
            SqlParameter[] parametros = new SqlParameter[] { new SqlParameter("@Patente", v.Patente) };
            DAL_64PR.Acceso.Instancia.escribirQuery("SP_VehiculoJP86_ActDesact", parametros, CommandType.StoredProcedure);
        }
    }
}
