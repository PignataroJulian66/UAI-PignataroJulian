using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_64PR
{
    public class ContratoJP86
    {
        public const decimal PorcentajeCargosAdicionales = 0.10m;

        ///Tope de las columnas ContratoJP86.ImporteTotal y FacturaJP86.MontoFactura: DECIMAL(10,2).
        public const decimal ImporteMaximo = 99999999.99m;

        ///Plazo maximo de un contrato: alquiler de corto plazo.
        public const int PlazoMaximoDias = 90;

        Mapper.mpp_contrato mpp = new Mapper.mpp_contrato();
        Mapper.mpp_factura mppFactura = new Mapper.mpp_factura();
        private static readonly DV.DV_64PR recalculador = new DV.DV_64PR();

        public List<BE.VehiculoJP86> BuscarUnidadesDisponibles(int? idCategoria, string marca, int? kilometrajeMaximo)
        {
            return mpp.BuscarUnidadesDisponibles(idCategoria, marca, kilometrajeMaximo);
        }

        public decimal CalcularImporteTotal(decimal tarifaDiaria, DateTime fechaInicio, DateTime fechaFin, bool cargosAdicionales)
        {
            int dias = Math.Max(1, (fechaFin.Date - fechaInicio.Date).Days);
            decimal importe = tarifaDiaria * dias;
            if (cargosAdicionales)
                importe += importe * PorcentajeCargosAdicionales;
            return importe;
        }

        public BE.ContratoJP86 GenerarContrato(BE.ClienteJP86 cliente, BE.VehiculoJP86 vehiculo, DateTime fechaInicio, DateTime fechaFin, bool cargosAdicionales)
        {
            ///Las excepciones de negocio llevan como Message una clave de idioma
            if (fechaFin.Date < fechaInicio.Date)
                throw new InvalidOperationException("msg_FechasValidas");
            if (fechaInicio.Date < DateTime.Today)
                throw new InvalidOperationException("err_Contrato_FechaInicioPasada");
            if ((fechaFin.Date - fechaInicio.Date).Days > PlazoMaximoDias)
                throw new InvalidOperationException("err_Contrato_PlazoMaximo");

            BE.ContratoJP86 c = new BE.ContratoJP86()
            {
                Cliente = cliente,
                Vehiculo = vehiculo,
                FechaInicio = fechaInicio,
                FechaFin = fechaFin,
                TarifaDiaria = vehiculo.Categoria.TarifaDiaria,
                CargosAdicionales = cargosAdicionales,
                KilometrajeEntrega = vehiculo.Kilometraje,
                Estado = BE.EstadoContratoJP86.ACTIVO
            };
            c.ImporteTotal = CalcularImporteTotal(c.TarifaDiaria, fechaInicio, fechaFin, cargosAdicionales);
            if (c.ImporteTotal > ImporteMaximo)
                throw new InvalidOperationException("err_Contrato_ImporteMaximo");

            c.NumeroContrato = mpp.Crear(c);
            ///El SP devuelve 0 si la unidad ya no esta DISPONIBLE/activa o el cliente no esta activo:
            ///se corta ACA, antes de recalcular DV o tocar el vehiculo.
            if (c.NumeroContrato <= 0)
                throw new InvalidOperationException("err_Contrato_NoDisponible");

            recalculador.RecalcularTabla("ContratoJP86");

            new BLL_64PR.VehiculoJP86().CambiarEstado(vehiculo.Patente, BE.EstadoVehiculoJP86.ALQUILADO);

            return c;
        }

        public List<BE.ContratoJP86> ListarActivos()
        {
            return mpp.ListarActivos();
        }

        public List<BE.ContratoJP86> ListarFacturados()
        {
            return mpp.ListarFacturados();
        }

        public async Task<BE.FacturaJP86> GenerarFactura(BE.ContratoJP86 contrato, BE.MetodoPagoJP86 metodoPago)
        {
            IEstrategiaPagoJP86 estrategiaPago = FabricaEstrategiaPagoJP86.Obtener(metodoPago);
            BE.ResultadoPagoJP86 resultadoPago = await estrategiaPago.ProcesarPago(contrato.ImporteTotal);
            ///Sin pago aprobado no se factura: el contrato sigue ACTIVO.
            if (!resultadoPago.Exito)
                throw new InvalidOperationException("err_Pago_Rechazado");

            BE.FacturaJP86 f = new BE.FacturaJP86()
            {
                Contrato = contrato,
                FechaFactura = DateTime.Now,
                MetodoPago = metodoPago,
                MontoFactura = contrato.ImporteTotal
            };
            f.NumeroFactura = mppFactura.Crear(f);
            ///El SP devuelve 0 (con ROLLBACK) si el contrato ya no estaba ACTIVO
            if (f.NumeroFactura <= 0)
                throw new InvalidOperationException("err_Factura_ContratoNoActivo");

            recalculador.RecalcularTabla("FacturaJP86");
            recalculador.RecalcularTabla("ContratoJP86");

            contrato.Estado = BE.EstadoContratoJP86.FACTURADO;

            return f;
        }

        public void RegistrarDevolucion(BE.ContratoJP86 contrato, int kilometrajeRetorno, BE.EstadoUnidadDevolucionJP86 estadoUnidad)
        {
            ///Regla de negocio (tambien la aplica la UI con el minimo del control y la BD con CK_ContratoJP86_KilometrajeRetorno)
            if (kilometrajeRetorno < contrato.KilometrajeEntrega)
                throw new InvalidOperationException("msg_KilometrajeRetornoValido");

            int filasAfectadas = mpp.CerrarPorDevolucion(contrato.NumeroContrato, kilometrajeRetorno, estadoUnidad);
            ///0 filas = el contrato ya no estaba FACTURADO (cerrado por otro usuario): se corta ACA,
            ///antes de recalcular DV o tocar el estado/kilometraje del vehiculo.
            if (filasAfectadas == 0)
                throw new InvalidOperationException("err_Devolucion_ContratoNoFacturado");

            recalculador.RecalcularTabla("ContratoJP86");

            BE.EstadoVehiculoJP86 nuevoEstadoVehiculo = estadoUnidad == BE.EstadoUnidadDevolucionJP86.SIN_NOVEDADES
                ? BE.EstadoVehiculoJP86.DISPONIBLE
                : BE.EstadoVehiculoJP86.EN_REVISION;

            new BLL_64PR.VehiculoJP86().CambiarEstado(contrato.Vehiculo.Patente, nuevoEstadoVehiculo, kilometrajeRetorno);

            contrato.Vehiculo.Kilometraje = kilometrajeRetorno;
            contrato.KilometrajeRetorno = kilometrajeRetorno;
            contrato.EstadoUnidadDevolucion = estadoUnidad;
            contrato.Estado = BE.EstadoContratoJP86.CERRADO;
        }
    }
}
