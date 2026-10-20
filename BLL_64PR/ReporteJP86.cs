using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_64PR
{
    public class ReporteJP86
    {
        #region Campos
        Mapper.mpp_reporte mpp = new Mapper.mpp_reporte();
        private static readonly DV.DV_64PR recalculador = new DV.DV_64PR();
        #endregion

        #region Consultas
        public List<BE.VehiculoJP86> ListarVehiculosEnRevision()
        {
            return mpp.ListarVehiculosEnRevision();
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

        public List<BE.ReporteJP86> ListarEnReparacion()
        {
            return mpp.ListarEnReparacion();
        }
        #endregion

        #region Operaciones
        public BE.ReporteJP86 RegistrarReporte(BE.VehiculoJP86 vehiculo, string descripcionProblema, BE.FuenteReporteJP86 fuente)
        {
            ///Un vehiculo tiene a lo sumo un reporte abierto (PENDIENTE o EN_REPARACION): el nuevo
            ///desperfecto se agrega al circuito ya abierto en vez de duplicarlo
            if (mpp.ExisteAbiertoPorVehiculo(vehiculo.Patente))
                throw new InvalidOperationException("err_Reporte_ReporteAbierto");

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

        ///CUN-06 "Asignar Criticidad" + CUN-07 "Determinar Modalidad Reparacion" (include obligatorio): una unica confirmacion.
        ///El SP registra criticidad y modalidad, pasa el Reporte (PENDIENTE -> EN_REPARACION) y el VehiculoJP86 (-> EN_REPARACION)
        ///en una unica transaccion, a diferencia de CUN-01/CUN-04 (que reutilizan BLL_64PR.VehiculoJP86.CambiarEstado en llamadas
        ///separadas), porque ambas actualizaciones tienen que quedar consistentes entre si
        public void AsignarCriticidadYModalidad(BE.ReporteJP86 reporte, BE.CriticidadReporteJP86 criticidad, BE.ModalidadReparacionJP86 modalidad)
        {
            int filasAfectadas = mpp.AsignarCriticidadYModalidad(reporte.NumeroReporte, criticidad, modalidad);
            ///0 filas = el reporte ya no estaba PENDIENTE (lo clasifico otro usuario): se corta ACA, antes de recalcular DV
            if (filasAfectadas == 0)
                throw new InvalidOperationException("err_Reporte_NoPendiente");

            recalculador.RecalcularTabla("ReporteJP86");
            recalculador.RecalcularTabla("VehiculoJP86");

            reporte.Criticidad = criticidad;
            reporte.ModalidadReparacion = modalidad;
            reporte.Estado = BE.EstadoReporteJP86.EN_REPARACION;
            reporte.Vehiculo.Estado = BE.EstadoVehiculoJP86.EN_REPARACION;
        }

        ///CUN-08 "Registrar Acreditacion de Reparacion" + CUN-09 "Liberar Vehiculo" (incluido siempre en el paso 5).
        ///Mismo criterio que AsignarCriticidadYModalidad: el SP actualiza Reporte (Estado=ACREDITADO) y VehiculoJP86 (Estado=DISPONIBLE)
        ///en una unica transaccion.
        public void AcreditarReparacion(BE.ReporteJP86 reporte, string descripcionCierre, string dniResponsableCierre)
        {
            int filasAfectadas = mpp.Acreditar(reporte.NumeroReporte, descripcionCierre, dniResponsableCierre);
            ///0 filas = el reporte ya no estaba EN_REPARACION (lo acredito otro usuario): se corta ACA, antes de recalcular DV
            if (filasAfectadas == 0)
                throw new InvalidOperationException("err_Reporte_NoEnReparacion");

            recalculador.RecalcularTabla("ReporteJP86");
            recalculador.RecalcularTabla("VehiculoJP86");

            reporte.DescripcionCierre = descripcionCierre;
            reporte.FechaAcreditacion = DateTime.Now;
            reporte.ResponsableCierreDNI = dniResponsableCierre;
            reporte.Estado = BE.EstadoReporteJP86.ACREDITADO;
            reporte.Vehiculo.Estado = BE.EstadoVehiculoJP86.DISPONIBLE;
        }
        #endregion
    }
}
