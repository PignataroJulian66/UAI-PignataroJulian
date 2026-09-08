using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapper
{
    public class mpp_categoria
    {
        public List<BE.Categoria_64PR> Listar()
        {
            List<BE.Categoria_64PR> lista = new List<BE.Categoria_64PR>();

            DataTable tabla = DAL_64PR.Acceso.Instancia.leerQuery("SP_Categoria_Listar", null, CommandType.StoredProcedure);

            foreach (DataRow dr in tabla.Rows)
            {
                BE.Categoria_64PR c = new BE.Categoria_64PR();
                c.Id = Convert.ToInt32(dr["ID_Categoria"]);
                c.Nombre = dr["Nombre"].ToString();
                c.Descripcion = dr["Descripcion"] == DBNull.Value ? string.Empty : dr["Descripcion"].ToString();
                c.TarifaDiaria = Convert.ToDecimal(dr["TarifaDiaria"]);
                c.Activo = Convert.ToBoolean(dr["Activo"]);
                lista.Add(c);
            }
            return lista;
        }

        public void Crear(BE.Categoria_64PR c)
        {
            SqlParameter[] parametros = new SqlParameter[]
            {
                new SqlParameter("@Nombre", c.Nombre),
                new SqlParameter("@Descripcion", (object)c.Descripcion ?? DBNull.Value),
                new SqlParameter("@TarifaDiaria", c.TarifaDiaria)
            };
            DAL_64PR.Acceso.Instancia.escribirQuery("SP_Categoria_Crear", parametros, CommandType.StoredProcedure);
        }

        public void Modificar(BE.Categoria_64PR c)
        {
            SqlParameter[] parametros = new SqlParameter[]
            {
                new SqlParameter("@Id", c.Id),
                new SqlParameter("@Nombre", c.Nombre),
                new SqlParameter("@Descripcion", (object)c.Descripcion ?? DBNull.Value),
                new SqlParameter("@TarifaDiaria", c.TarifaDiaria)
            };
            DAL_64PR.Acceso.Instancia.escribirQuery("SP_Categoria_Modificar", parametros, CommandType.StoredProcedure);
        }

        public void ActDesact(BE.Categoria_64PR c)
        {
            SqlParameter[] parametros = new SqlParameter[] { new SqlParameter("@Id", c.Id) };
            DAL_64PR.Acceso.Instancia.escribirQuery("SP_Categoria_ActDesact", parametros, CommandType.StoredProcedure);
        }
    }
}
