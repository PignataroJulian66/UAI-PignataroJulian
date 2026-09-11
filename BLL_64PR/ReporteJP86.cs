using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_64PR
{
    public class ReporteJP86
    {
        Mapper.mpp_reporte mpp = new Mapper.mpp_reporte();
        private static readonly DV.DV_64PR recalculador = new DV.DV_64PR();

        public List<BE.VehiculoJP86> ListarVehiculosEnRevision()
        {
            return mpp.ListarVehiculosEnRevision();
        }

        public BE.ReporteJP86 RegistrarReporte(BE.VehiculoJP86 vehiculo, string descripcionProblema, BE.FuenteReporteJP86 fuente)
        {
            BE.ReporteJP86 r = new BE.ReporteJP86()
            {
                Vehiculo = vehiculo,
                FechaHora = DateTime.Now,
                DescripcionProblema = descripcionProblema,
                Fuente = fuente,
                Estado = BE.EstadoReporteJP86.PENDIENTE
            };

            r.NumeroReporte = mpp.Crear(r);
            recalculador.RecalcularTabla("ReporteJP86");

            return r;
        }

        public List<BE.ReporteJP86> ListarPendientes()
        {
            return mpp.ListarPendientes();
        }

        public List<BE.ReporteJP86> ListarHistorialPorVehiculo(string patente, int? excluirNumeroReporte = null)
        {
            List<BE.ReporteJP86> lista = mpp.ListarPorVehiculo(patente);
            if (excluirNumeroReporte.HasValue)
                lista = lista.Where(r => r.NumeroReporte != excluirNumeroReporte.Value).ToList();
            return lista;
        }

        public void AsignarCriticidad(BE.ReporteJP86 reporte, BE.CriticidadReporteJP86 criticidad)
        {
            mpp.AsignarCriticidad(reporte.NumeroReporte, criticidad);
            recalculador.RecalcularTabla("ReporteJP86");

            reporte.Criticidad = criticidad;
            reporte.Estado = BE.EstadoReporteJP86.CLASIFICADO;
        }

        ///CUN-07 "Determinar Modalidad Reparacion": incluido siempre a continuacion de AsignarCriticidad (CUN-06 paso 7).
        ///El SP actualiza Reporte (Estado=EN_REPARACION) y VehiculoJP86 (Estado=EN_REPARACION) en una unica transaccion,
        ///a diferencia de CUN-01/CUN-04 (que reutilizan BLL_64PR.VehiculoJP86.CambiarEstado en llamadas separadas),
        ///porque ambas actualizaciones tienen que quedar consistentes entre si (mismo criterio que SP_FacturaJP86_Crear).
        public void DeterminarModalidad(BE.ReporteJP86 reporte, BE.ModalidadReparacionJP86 modalidad)
        {
            mpp.DeterminarModalidad(reporte.NumeroReporte, modalidad);
            recalculador.RecalcularTabla("ReporteJP86");
            recalculador.RecalcularTabla("VehiculoJP86");

            reporte.ModalidadReparacion = modalidad;
            reporte.Estado = BE.EstadoReporteJP86.EN_REPARACION;
            reporte.Vehiculo.Estado = BE.EstadoVehiculoJP86.EN_REPARACION;
        }

        public List<BE.ReporteJP86> ListarEnReparacion()
        {
            return mpp.ListarEnReparacion();
        }

        ///CUN-08 "Registrar Acreditacion de Reparacion" + CUN-09 "Liberar Vehiculo" (incluido siempre en el paso 5).
        ///Mismo criterio que DeterminarModalidad: el SP actualiza Reporte (Estado=ACREDITADO) y VehiculoJP86 (Estado=DISPONIBLE)
        ///en una unica transaccion.
        public void AcreditarReparacion(BE.ReporteJP86 reporte, string descripcionCierre, string dniResponsableCierre)
        {
            mpp.Acreditar(reporte.NumeroReporte, descripcionCierre, dniResponsableCierre);
            recalculador.RecalcularTabla("ReporteJP86");
            recalculador.RecalcularTabla("VehiculoJP86");

            reporte.DescripcionCierre = descripcionCierre;
            reporte.FechaAcreditacion = DateTime.Now;
            reporte.ResponsableCierreDNI = dniResponsableCierre;
            reporte.Estado = BE.EstadoReporteJP86.ACREDITADO;
            reporte.Vehiculo.Estado = BE.EstadoVehiculoJP86.DISPONIBLE;
        }
    }
}
