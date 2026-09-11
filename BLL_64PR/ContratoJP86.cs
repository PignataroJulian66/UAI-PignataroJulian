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

            c.NumeroContrato = mpp.Crear(c);
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

        public BE.FacturaJP86 GenerarFactura(BE.ContratoJP86 contrato, BE.MetodoPagoJP86 metodoPago)
        {
            BE.FacturaJP86 f = new BE.FacturaJP86()
            {
                Contrato = contrato,
                FechaFactura = DateTime.Now,
                MetodoPago = metodoPago,
                MontoFactura = contrato.ImporteTotal
            };
            f.NumeroFactura = mppFactura.Crear(f);
            recalculador.RecalcularTabla("FacturaJP86");
            recalculador.RecalcularTabla("ContratoJP86");

            contrato.Estado = BE.EstadoContratoJP86.FACTURADO;

            return f;
        }

        public void RegistrarDevolucion(BE.ContratoJP86 contrato, int kilometrajeRetorno, BE.EstadoUnidadDevolucionJP86 estadoUnidad)
        {
            mpp.CerrarPorDevolucion(contrato.NumeroContrato, kilometrajeRetorno, estadoUnidad);
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
