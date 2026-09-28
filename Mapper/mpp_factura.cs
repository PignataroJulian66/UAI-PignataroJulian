using System;
using System.Data;
using System.Data.SqlClient;

namespace Mapper
{
    public class mpp_factura
    {
        ///Devuelve el NumeroFactura, o 0 si el SP no facturo (contrato ya no ACTIVO): esa regla la evalua la BLL.
        public int Crear(BE.FacturaJP86 f)
        {
            SqlParameter[] parametros = new SqlParameter[]
            {
                new SqlParameter("@NumeroContrato", f.Contrato.NumeroContrato),
                new SqlParameter("@MetodoPago", f.MetodoPago.ToString()),
                new SqlParameter("@MontoFactura", f.MontoFactura)
            };
            object resultado = DAL_64PR.Acceso.Instancia.leerEscalar("SP_FacturaJP86_Crear", parametros, CommandType.StoredProcedure);
            ///null = el SP no devolvio nada (error de BD absorbido por Acceso.leerEscalar): NO se convierte a 0
            if (resultado == null || resultado == DBNull.Value)
                throw new InvalidOperationException("err_BD_SinRespuesta");
            return Convert.ToInt32(resultado);
        }
    }
}
