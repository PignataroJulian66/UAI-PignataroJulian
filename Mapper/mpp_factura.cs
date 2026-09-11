using System;
using System.Data;
using System.Data.SqlClient;

namespace Mapper
{
    public class mpp_factura
    {
        public int Crear(BE.FacturaJP86 f)
        {
            SqlParameter[] parametros = new SqlParameter[]
            {
                new SqlParameter("@NumeroContrato", f.Contrato.NumeroContrato),
                new SqlParameter("@MetodoPago", f.MetodoPago.ToString()),
                new SqlParameter("@MontoFactura", f.MontoFactura)
            };
            object resultado = DAL_64PR.Acceso.Instancia.leerEscalar("SP_FacturaJP86_Crear", parametros, CommandType.StoredProcedure);
            int nuevoNumero = Convert.ToInt32(resultado);
            if (nuevoNumero == 0)
                throw new InvalidOperationException("El contrato no existe o no se encuentra en estado ACTIVO.");
            return nuevoNumero;
        }
    }
}
