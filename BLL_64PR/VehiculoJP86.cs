using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_64PR
{
    public class VehiculoJP86
    {
        Mapper.mpp_vehiculo mpp = new Mapper.mpp_vehiculo();
        Mapper.mpp_contrato mppContrato = new Mapper.mpp_contrato();
        Mapper.mpp_reporte mppReporte = new Mapper.mpp_reporte();
        private static readonly DV.DV_64PR recalculador = new DV.DV_64PR();

        public List<BE.VehiculoJP86> Listar()
        {
            return mpp.Listar();
        }

        public void Crear(BE.VehiculoJP86 v)
        {
            mpp.Crear(v);
            recalculador.RecalcularTabla("VehiculoJP86");
        }

        public void Modificar(BE.VehiculoJP86 v)
        {
            mpp.Modificar(v);
            recalculador.RecalcularTabla("VehiculoJP86");
        }

        public void CambiarEstado(string patente, BE.EstadoVehiculoJP86 estado, int? nuevoKilometraje = null)
        {
            ///Regla de negocio: al pasar a EN_REVISION, el vehiculo se desactiva automaticamente
            ///(no depende de que se destilde a mano en el maestro). Unico lugar donde se aplica.
            bool? nuevoActivo = estado == BE.EstadoVehiculoJP86.EN_REVISION ? (bool?)false : null;

            mpp.CambiarEstado(patente, estado, nuevoKilometraje, nuevoActivo);
            recalculador.RecalcularTabla("VehiculoJP86");
        }

        ///Unico punto de entrada para el cambio de estado MANUAL desde el maestro (btnCambiarEstado).
        ///Los flujos de negocio (CUN-01, CUN-04) siguen llamando a CambiarEstado directamente y no pasan por aca.
        public void CambiarEstadoManual(string patente, BE.EstadoVehiculoJP86 destino)
        {
            if (destino != BE.EstadoVehiculoJP86.DISPONIBLE && destino != BE.EstadoVehiculoJP86.EN_REVISION)
                throw new InvalidOperationException("err_CambioEstado_DestinoNoPermitido");

            BE.EstadoVehiculoJP86 origen = mpp.ObtenerEstado(patente);

            if (TieneContratoAbierto(patente, origen))
                throw new InvalidOperationException("err_CambioEstado_ContratoActivo");

            if ((origen == BE.EstadoVehiculoJP86.EN_REVISION || origen == BE.EstadoVehiculoJP86.EN_REPARACION) && TieneReporteAbierto(patente))
                throw new InvalidOperationException("err_CambioEstado_ReporteAbierto");

            CambiarEstado(patente, destino);
        }

        ///Restaurar una version historica (FrmBitacoraCambiosVehiculo) es un UPDATE completo del vehiculo:
        ///se le aplican las mismas reglas que al cambio de estado manual y a la baja, para que no sea un atajo.
        public void ValidarRestauracion(string patente, BE.EstadoVehiculoJP86 estadoDestino, bool activoDestino)
        {
            BE.VehiculoJP86 actual = mpp.Listar().FirstOrDefault(v => v.Patente == patente);
            if (actual == null)
                throw new InvalidOperationException("El vehiculo no existe.");

            ///El estado se lee de la BD, no de la grilla (puede estar desactualizada)
            BE.EstadoVehiculoJP86 origen = mpp.ObtenerEstado(patente);

            if (estadoDestino != origen)
            {
                if (estadoDestino != BE.EstadoVehiculoJP86.DISPONIBLE && estadoDestino != BE.EstadoVehiculoJP86.EN_REVISION)
                    throw new InvalidOperationException("err_CambioEstado_DestinoNoPermitido");

                if (TieneContratoAbierto(patente, origen))
                    throw new InvalidOperationException("err_CambioEstado_ContratoActivo");

                if ((origen == BE.EstadoVehiculoJP86.EN_REVISION || origen == BE.EstadoVehiculoJP86.EN_REPARACION) && TieneReporteAbierto(patente))
                    throw new InvalidOperationException("err_CambioEstado_ReporteAbierto");
            }

            ///Misma regla que CambiarEstado: un vehiculo EN_REVISION nunca queda activo
            if (estadoDestino == BE.EstadoVehiculoJP86.EN_REVISION && activoDestino)
                throw new InvalidOperationException("err_RestaurarVersion_RevisionActiva");

            ///Si la version lo deja inactivo y hoy esta activo, es una baja: mismas reglas que ActDesact
            if (actual.Activo && !activoDestino)
            {
                if (TieneContratoAbierto(patente, origen))
                    throw new InvalidOperationException("err_BajaVehiculo_ContratoActivo");

                if (TieneReporteAbierto(patente))
                    throw new InvalidOperationException("err_BajaVehiculo_ReporteAbierto");
            }
        }

        ///Compartido por CambiarEstadoManual, ActDesact y ValidarRestauracion
        private bool TieneContratoAbierto(string patente, BE.EstadoVehiculoJP86 origen)
        {
            return origen == BE.EstadoVehiculoJP86.ALQUILADO && mppContrato.ExisteActivoPorVehiculo(patente);
        }

        ///Reporte PENDIENTE, CLASIFICADO o EN_REPARACION (no ACREDITADO)
        private bool TieneReporteAbierto(string patente)
        {
            return mppReporte.ExisteAbiertoPorVehiculo(patente);
        }

        ///si llega activo es una baja; si llega inactivo es una reactivacion
        public void ActDesact(BE.VehiculoJP86 v)
        {
            if (v.Activo)
            {
                ///El estado se lee de la BD, no de la grilla (puede estar desactualizada)
                BE.EstadoVehiculoJP86 origen = mpp.ObtenerEstado(v.Patente);

                if (TieneContratoAbierto(v.Patente, origen))
                    throw new InvalidOperationException("err_BajaVehiculo_ContratoActivo");

                ///A diferencia del cambio de estado manual, bloquea la baja cualquiera sea el estado actual
                if (TieneReporteAbierto(v.Patente))
                    throw new InvalidOperationException("err_BajaVehiculo_ReporteAbierto");
            }

            mpp.ActDesact(v);
            recalculador.RecalcularTabla("VehiculoJP86");
        }
    }
}
