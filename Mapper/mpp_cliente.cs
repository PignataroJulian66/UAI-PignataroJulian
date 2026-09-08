using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapper
{
    public class mpp_cliente
    {
        public List<BE.ClienteJP86> Listar()
        {
            List<BE.ClienteJP86> lista = new List<BE.ClienteJP86>();

            DataTable tabla = DAL_64PR.Acceso.Instancia.leerQuery("SP_ClienteJP86_Listar", null, CommandType.StoredProcedure);

            foreach (DataRow dr in tabla.Rows)
            {
                BE.ClienteJP86 c = new BE.ClienteJP86();
                c.DNI = dr["DNI"].ToString();
                c.Nombre = dr["Nombre"].ToString();
                c.Apellido = dr["Apellido"].ToString();
                c.Telefono = dr["Telefono"].ToString();
                c.Activo = Convert.ToBoolean(dr["Activo"]);
                lista.Add(c);
            }
            return lista;
        }

        public void Crear(BE.ClienteJP86 c)
        {
            SqlParameter[] parametros = new SqlParameter[]
            {
                new SqlParameter("@DNI", c.DNI),
                new SqlParameter("@Nombre", c.Nombre),
                new SqlParameter("@Apellido", c.Apellido),
                new SqlParameter("@Telefono", c.Telefono)
            };
            DAL_64PR.Acceso.Instancia.escribirQuery("SP_ClienteJP86_Crear", parametros, CommandType.StoredProcedure);
        }

        public void Modificar(BE.ClienteJP86 c)
        {
            SqlParameter[] parametros = new SqlParameter[]
            {
                new SqlParameter("@DNI", c.DNI),
                new SqlParameter("@Nombre", c.Nombre),
                new SqlParameter("@Apellido", c.Apellido),
                new SqlParameter("@Telefono", c.Telefono)
            };
            DAL_64PR.Acceso.Instancia.escribirQuery("SP_ClienteJP86_Modificar", parametros, CommandType.StoredProcedure);
        }

        public void ActDesact(BE.ClienteJP86 c)
        {
            SqlParameter[] parametros = new SqlParameter[] { new SqlParameter("@DNI", c.DNI) };
            DAL_64PR.Acceso.Instancia.escribirQuery("SP_ClienteJP86_ActDesact", parametros, CommandType.StoredProcedure);
        }
    }
}
