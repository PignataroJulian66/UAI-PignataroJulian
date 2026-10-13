using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapper
{
    public class mpp_reporte
    {
        public List<BE.VehiculoJP86> ListarVehiculosEnRevision()
        {
            List<BE.VehiculoJP86> lista = new List<BE.VehiculoJP86>();

            DataTable tabla = DAL_64PR.Acceso.Instancia.leerQuery("SP_ReporteJP86_ListarVehiculosEnRevision", null, CommandType.StoredProcedure);

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

        public bool ExisteAbiertoPorVehiculo(string patente)
        {
            SqlParameter[] parametros = new SqlParameter[] { new SqlParameter("@Patente_Vehiculo", patente) };
            object resultado = DAL_64PR.Acceso.Instancia.leerEscalar("SP_ReporteJP86_ExisteAbiertoPorVehiculo", parametros, CommandType.StoredProcedure);
            ///null = error de BD absorbido por Acceso.leerEscalar: NO se interpreta como "no existe" (dejaria pasar el cambio de estado)
            if (resultado == null || resultado == DBNull.Value)
                throw new InvalidOperationException("err_BD_SinRespuesta");
            return Convert.ToInt32(resultado) == 1;
        }

        public int Crear(BE.ReporteJP86 r)
        {
            SqlParameter[] parametros = new SqlParameter[]
            {
                new SqlParameter("@Patente_Vehiculo", r.Vehiculo.Patente),
                new SqlParameter("@DescripcionProblema", r.DescripcionProblema),
                new SqlParameter("@Fuente", r.Fuente.ToString())
            };
            object resultado = DAL_64PR.Acceso.Instancia.leerEscalar("SP_ReporteJP86_Crear", parametros, CommandType.StoredProcedure);
            ///null = error de BD absorbido por Acceso.leerEscalar: NO se convierte a 0
            if (resultado == null || resultado == DBNull.Value)
                throw new InvalidOperationException("err_BD_SinRespuesta");
            return Convert.ToInt32(resultado);
        }

        private List<BE.ReporteJP86> MapearReportes(DataTable tabla)
        {
            List<BE.ReporteJP86> lista = new List<BE.ReporteJP86>();

            foreach (DataRow dr in tabla.Rows)
            {
                BE.ReporteJP86 r = new BE.ReporteJP86();
                r.NumeroReporte = Convert.ToInt32(dr["NumeroReporte"]);

                r.Vehiculo = new BE.VehiculoJP86()
                {
                    Patente = dr["Patente_Vehiculo"].ToString(),
                    Marca = dr["Marca"].ToString(),
                    Modelo = dr["Modelo"].ToString(),
                    Categoria = new BE.Categoria_64PR()
                    {
                        Id = Convert.ToInt32(dr["ID_Categoria"]),
                        Nombre = dr["CategoriaNombre"].ToString(),
                        TarifaDiaria = Convert.ToDecimal(dr["TarifaDiaria"])
                    },
                    Kilometraje = Convert.ToInt32(dr["VehiculoKilometraje"]),
                    Estado = (BE.EstadoVehiculoJP86)Enum.Parse(typeof(BE.EstadoVehiculoJP86), dr["VehiculoEstado"].ToString()),
                    Activo = Convert.ToBoolean(dr["VehiculoActivo"])
                };

                r.FechaHora = Convert.ToDateTime(dr["FechaHora"]);
                r.DescripcionProblema = dr["DescripcionProblema"].ToString();
                r.Fuente = (BE.FuenteReporteJP86)Enum.Parse(typeof(BE.FuenteReporteJP86), dr["Fuente"].ToString());
                r.Criticidad = dr["Criticidad"] == DBNull.Value
                    ? (BE.CriticidadReporteJP86?)null
                    : (BE.CriticidadReporteJP86)Enum.Parse(typeof(BE.CriticidadReporteJP86), dr["Criticidad"].ToString());
                r.ModalidadReparacion = dr["ModalidadReparacion"] == DBNull.Value
                    ? (BE.ModalidadReparacionJP86?)null
                    : (BE.ModalidadReparacionJP86)Enum.Parse(typeof(BE.ModalidadReparacionJP86), dr["ModalidadReparacion"].ToString());
                r.DescripcionCierre = dr["DescripcionCierre"] == DBNull.Value ? null : dr["DescripcionCierre"].ToString();
                r.FechaAcreditacion = dr["FechaAcreditacion"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["FechaAcreditacion"]);
                r.ResponsableCierreDNI = dr["DNI_ResponsableCierre"] == DBNull.Value ? null : dr["DNI_ResponsableCierre"].ToString();
                r.Estado = (BE.EstadoReporteJP86)Enum.Parse(typeof(BE.EstadoReporteJP86), dr["Estado"].ToString());

                lista.Add(r);
            }
            return lista;
        }

        public List<BE.ReporteJP86> ListarPendientes()
        {
            DataTable tabla = DAL_64PR.Acceso.Instancia.leerQuery("SP_ReporteJP86_ListarPendientes", null, CommandType.StoredProcedure);
            return MapearReportes(tabla);
        }

        public List<BE.ReporteJP86> ListarPorVehiculo(string patente)
        {
            SqlParameter[] parametros = new SqlParameter[]
            {
                new SqlParameter("@Patente_Vehiculo", patente)
            };
            DataTable tabla = DAL_64PR.Acceso.Instancia.leerQuery("SP_ReporteJP86_ListarPorVehiculo", parametros, CommandType.StoredProcedure);
            return MapearReportes(tabla);
        }

        public List<BE.ReporteJP86> ListarEnReparacion()
        {
            DataTable tabla = DAL_64PR.Acceso.Instancia.leerQuery("SP_ReporteJP86_ListarEnReparacion", null, CommandType.StoredProcedure);
            return MapearReportes(tabla);
        }

        ///CUN-06 + CUN-07 en una unica operacion: criticidad + modalidad, reporte PENDIENTE -> EN_REPARACION y vehiculo -> EN_REPARACION.
        ///Devuelve las filas afectadas (0 si el reporte ya no estaba PENDIENTE): esa regla la evalua la BLL.
        ///El SP devuelve @@ROWCOUNT como resultado (SELECT) porque con SET NOCOUNT ON, ExecuteNonQuery siempre da -1.
        public int AsignarCriticidadYModalidad(int numeroReporte, BE.CriticidadReporteJP86 criticidad, BE.ModalidadReparacionJP86 modalidad)
        {
            SqlParameter[] parametros = new SqlParameter[]
            {
                new SqlParameter("@NumeroReporte", numeroReporte),
                new SqlParameter("@Criticidad", criticidad.ToString()),
                new SqlParameter("@ModalidadReparacion", modalidad.ToString())
            };
            object resultado = DAL_64PR.Acceso.Instancia.leerEscalar("SP_ReporteJP86_AsignarCriticidadYModalidad", parametros, CommandType.StoredProcedure);
            ///null = error de BD absorbido por Acceso.leerEscalar: NO se convierte a 0
            if (resultado == null || resultado == DBNull.Value)
                throw new InvalidOperationException("err_BD_SinRespuesta");
            return Convert.ToInt32(resultado);
        }

        ///CUN-08 + CUN-09: reporte EN_REPARACION -> ACREDITADO y vehiculo -> DISPONIBLE.
        ///Devuelve las filas afectadas (0 si el reporte ya no estaba EN_REPARACION): esa regla la evalua la BLL.
        public int Acreditar(int numeroReporte, string descripcionCierre, string dniResponsableCierre)
        {
            SqlParameter[] parametros = new SqlParameter[]
            {
                new SqlParameter("@NumeroReporte", numeroReporte),
                new SqlParameter("@DescripcionCierre", descripcionCierre),
                new SqlParameter("@DNI_ResponsableCierre", dniResponsableCierre)
            };
            object resultado = DAL_64PR.Acceso.Instancia.leerEscalar("SP_ReporteJP86_Acreditar", parametros, CommandType.StoredProcedure);
            ///null = error de BD absorbido por Acceso.leerEscalar: NO se convierte a 0
            if (resultado == null || resultado == DBNull.Value)
                throw new InvalidOperationException("err_BD_SinRespuesta");
            return Convert.ToInt32(resultado);
        }
    }
}
