using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapper
{
    public class mpp_contrato
    {
        public List<BE.VehiculoJP86> BuscarUnidadesDisponibles(int? idCategoria, string marca, int? kilometrajeMaximo)
        {
            List<BE.VehiculoJP86> lista = new List<BE.VehiculoJP86>();

            SqlParameter[] parametros = new SqlParameter[]
            {
                new SqlParameter("@ID_Categoria", (object)idCategoria ?? DBNull.Value),
                new SqlParameter("@Marca", (object)marca ?? DBNull.Value),
                new SqlParameter("@KilometrajeMaximo", (object)kilometrajeMaximo ?? DBNull.Value)
            };

            DataTable tabla = DAL_64PR.Acceso.Instancia.leerQuery("SP_ContratoJP86_BuscarUnidadesDisponibles", parametros, CommandType.StoredProcedure);

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

        public int Crear(BE.ContratoJP86 c)
        {
            SqlParameter[] parametros = new SqlParameter[]
            {
                new SqlParameter("@DNI_Cliente", c.Cliente.DNI),
                new SqlParameter("@Patente_Vehiculo", c.Vehiculo.Patente),
                new SqlParameter("@FechaInicio", c.FechaInicio),
                new SqlParameter("@FechaFin", c.FechaFin),
                new SqlParameter("@TarifaDiaria", c.TarifaDiaria),
                new SqlParameter("@CargosAdicionales", c.CargosAdicionales),
                new SqlParameter("@ImporteTotal", c.ImporteTotal),
                new SqlParameter("@KilometrajeEntrega", c.KilometrajeEntrega)
            };
            object resultado = DAL_64PR.Acceso.Instancia.leerEscalar("SP_ContratoJP86_Crear", parametros, CommandType.StoredProcedure);
            return Convert.ToInt32(resultado);
        }

        private List<BE.ContratoJP86> MapearContratos(DataTable tabla)
        {
            List<BE.ContratoJP86> lista = new List<BE.ContratoJP86>();

            foreach (DataRow dr in tabla.Rows)
            {
                BE.ContratoJP86 c = new BE.ContratoJP86();
                c.NumeroContrato = Convert.ToInt32(dr["NumeroContrato"]);

                c.Cliente = new BE.ClienteJP86()
                {
                    DNI = dr["DNI_Cliente"].ToString(),
                    Nombre = dr["ClienteNombre"].ToString(),
                    Apellido = dr["ClienteApellido"].ToString(),
                    Telefono = dr["ClienteTelefono"].ToString(),
                    Activo = Convert.ToBoolean(dr["ClienteActivo"])
                };

                c.Vehiculo = new BE.VehiculoJP86()
                {
                    Patente = dr["Patente_Vehiculo"].ToString(),
                    Marca = dr["Marca"].ToString(),
                    Modelo = dr["Modelo"].ToString(),
                    Categoria = new BE.Categoria_64PR()
                    {
                        Id = Convert.ToInt32(dr["ID_Categoria"]),
                        Nombre = dr["CategoriaNombre"].ToString(),
                        TarifaDiaria = Convert.ToDecimal(dr["CategoriaTarifaDiaria"])
                    },
                    Kilometraje = Convert.ToInt32(dr["VehiculoKilometraje"]),
                    Estado = (BE.EstadoVehiculoJP86)Enum.Parse(typeof(BE.EstadoVehiculoJP86), dr["VehiculoEstado"].ToString()),
                    Activo = Convert.ToBoolean(dr["VehiculoActivo"])
                };

                c.FechaInicio = Convert.ToDateTime(dr["FechaInicio"]);
                c.FechaFin = Convert.ToDateTime(dr["FechaFin"]);
                c.TarifaDiaria = Convert.ToDecimal(dr["TarifaDiaria"]);
                c.CargosAdicionales = Convert.ToBoolean(dr["CargosAdicionales"]);
                c.ImporteTotal = Convert.ToDecimal(dr["ImporteTotal"]);
                c.KilometrajeEntrega = Convert.ToInt32(dr["KilometrajeEntrega"]);
                c.KilometrajeRetorno = dr["KilometrajeRetorno"] == DBNull.Value ? (int?)null : Convert.ToInt32(dr["KilometrajeRetorno"]);
                c.EstadoUnidadDevolucion = dr["EstadoUnidadDevolucion"] == DBNull.Value
                    ? (BE.EstadoUnidadDevolucionJP86?)null
                    : (BE.EstadoUnidadDevolucionJP86)Enum.Parse(typeof(BE.EstadoUnidadDevolucionJP86), dr["EstadoUnidadDevolucion"].ToString());
                c.Estado = (BE.EstadoContratoJP86)Enum.Parse(typeof(BE.EstadoContratoJP86), dr["Estado"].ToString());

                lista.Add(c);
            }
            return lista;
        }

        public List<BE.ContratoJP86> ListarActivos()
        {
            DataTable tabla = DAL_64PR.Acceso.Instancia.leerQuery("SP_ContratoJP86_ListarActivos", null, CommandType.StoredProcedure);
            return MapearContratos(tabla);
        }

        public List<BE.ContratoJP86> ListarFacturados()
        {
            DataTable tabla = DAL_64PR.Acceso.Instancia.leerQuery("SP_ContratoJP86_ListarFacturados", null, CommandType.StoredProcedure);
            return MapearContratos(tabla);
        }

        public void CerrarPorDevolucion(int numeroContrato, int kilometrajeRetorno, BE.EstadoUnidadDevolucionJP86 estadoUnidad)
        {
            SqlParameter[] parametros = new SqlParameter[]
            {
                new SqlParameter("@NumeroContrato", numeroContrato),
                new SqlParameter("@KilometrajeRetorno", kilometrajeRetorno),
                new SqlParameter("@EstadoUnidadDevolucion", estadoUnidad.ToString())
            };
            int filasAfectadas = DAL_64PR.Acceso.Instancia.escribirQuery("SP_ContratoJP86_CerrarPorDevolucion", parametros, CommandType.StoredProcedure);
            if (filasAfectadas == 0)
                throw new InvalidOperationException("El contrato no existe o no se encuentra en estado FACTURADO.");
        }
    }
}
