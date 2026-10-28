using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_64PR
{
    ///Reporte Inteligente de Mantenimiento Preventivo. Solo lectura: no escribe datos ni recalcula DV.
    ///La prediccion se basa en km (contratos devueltos); los reportes de desperfecto son solo contexto.
    public class ReporteMantenimientoJP86
    {
        #region Constantes de negocio
        ///Regla de negocio: todo vehiculo requiere service cada 10.000 km
        private const int UMBRAL_SERVICE_KM = 10000;
        ///AMARILLO por km: quedan 20% del umbral o menos
        private const int UMBRAL_AMARILLO_KM = UMBRAL_SERVICE_KM * 20 / 100;
        private const int DIAS_ROJO = 7;
        private const int DIAS_AMARILLO = 30;
        ///Ventana de calculo del ritmo de uso
        private const int VENTANA_RITMO_DIAS = 90;
        #endregion

        #region Campos
        Mapper.mpp_ReporteMantenimientoJP86 mpp = new Mapper.mpp_ReporteMantenimientoJP86();
        #endregion

        #region Consultas
        public List<BE.ItemReporteMantenimientoJP86> Generar()
        {
            ValidarPermiso();

            DateTime hoy = DateTime.Today;
            List<BE.ItemReporteMantenimientoJP86> items = mpp.Obtener(hoy, VENTANA_RITMO_DIAS);

            ///Los dados de baja no entran al reporte; los EN_REVISION/EN_REPARACION si, aunque esten inactivos
            ///(pasar a EN_REVISION desactiva el vehiculo por regla de BLL_64PR.VehiculoJP86.CambiarEstado)
            items = items.Where(i => i.Vehiculo.Activo || EstaEnTaller(i.Vehiculo.Estado)).ToList();

            foreach (BE.ItemReporteMantenimientoJP86 item in items)
            {
                item.KmUltimaRevision = ObtenerKmUltimaRevision(item);
                item.FechaReferenciaRevision = item.FechaUltimaRevision ?? item.FechaPrimerContrato;
                item.KmDesdeRevision = item.KmActual - item.KmUltimaRevision;
                item.KmRestantes = UMBRAL_SERVICE_KM - item.KmDesdeRevision;
                item.KmExcedidos = Math.Max(0, -item.KmRestantes);
                item.Ritmo = CalcularRitmo(item.KmRecorridosVentana, item.FechaPrimerContratoVentana, hoy);
                ProyectarFecha(item, hoy);
                item.Semaforo = ClasificarSemaforo(item);
                GenerarRecomendacion(item);
            }

            return items
                .OrderBy(i => i.Semaforo)
                .ThenBy(i => i.DiasEstimados ?? int.MaxValue)
                .ThenBy(i => i.KmRestantes)
                .ToList();
        }

        ///Filtros de la pantalla. null = sin filtro. El horizonte deja solo los que alcanzan el service
        ///dentro de esa cantidad de dias (los vencidos tienen 0 dias); SIN_DATOS y EN_TALLER no tienen proyeccion.
        public List<BE.ItemReporteMantenimientoJP86> Filtrar(List<BE.ItemReporteMantenimientoJP86> items, int? idCategoria, BE.SemaforoMantenimientoJP86? semaforo, int? horizonteDias)
        {
            IEnumerable<BE.ItemReporteMantenimientoJP86> resultado = items;

            if (idCategoria.HasValue)
                resultado = resultado.Where(i => i.Categoria.Id == idCategoria.Value);

            if (semaforo.HasValue)
                resultado = resultado.Where(i => i.Semaforo == semaforo.Value);

            if (horizonteDias.HasValue)
                resultado = resultado.Where(i => i.DiasEstimados.HasValue && i.DiasEstimados.Value <= horizonteDias.Value);

            return resultado.ToList();
        }

        public BE.ResumenReporteMantenimientoJP86 ObtenerResumen(List<BE.ItemReporteMantenimientoJP86> items)
        {
            BE.ResumenReporteMantenimientoJP86 resumen = new BE.ResumenReporteMantenimientoJP86();

            resumen.CantidadPorSemaforo = new Dictionary<BE.SemaforoMantenimientoJP86, int>();
            foreach (BE.SemaforoMantenimientoJP86 s in Enum.GetValues(typeof(BE.SemaforoMantenimientoJP86)))
                resumen.CantidadPorSemaforo[s] = items.Count(i => i.Semaforo == s);

            var masReportada = items
                .GroupBy(i => i.Categoria.Id)
                .Select(g => new { Categoria = g.First().Categoria, ReportesCliente = g.Sum(i => i.ReportesCliente) })
                .Where(x => x.ReportesCliente > 0)
                .OrderByDescending(x => x.ReportesCliente)
                .FirstOrDefault();

            if (masReportada != null)
            {
                resumen.CategoriaMasReportesCliente = masReportada.Categoria;
                resumen.ReportesClienteCategoria = masReportada.ReportesCliente;
            }

            return resumen;
        }
        #endregion

        #region Reglas de calculo
        ///El reporte de desperfecto no guarda km: se usa el km de devolucion del ultimo contrato devuelto
        ///antes de la revision (el vehiculo no se mueve en el taller). Sin revision: km de entrega del primer contrato.
        private int ObtenerKmUltimaRevision(BE.ItemReporteMantenimientoJP86 item)
        {
            if (item.FechaUltimaRevision.HasValue && item.KmDevolucionPreviaRevision.HasValue)
                return item.KmDevolucionPreviaRevision.Value;

            ///Sin revision, o revision sin contrato devuelto antes: el vehiculo no se movio hasta su primer contrato
            return item.KmEntregaPrimerContrato ?? 0;
        }

        ///km por dia calendario, desde el primer contrato de la ventana hasta hoy (minimo 1 dia)
        public decimal CalcularRitmo(int kmRecorridosVentana, DateTime? fechaPrimerContratoVentana, DateTime hoy)
        {
            if (!fechaPrimerContratoVentana.HasValue || kmRecorridosVentana <= 0)
                return 0;

            int dias = Math.Max(1, (hoy.Date - fechaPrimerContratoVentana.Value.Date).Days);
            return (decimal)kmRecorridosVentana / dias;
        }

        ///Dias estimados = km restantes / ritmo (redondeo hacia abajo). Vencido = 0 dias (ya alcanzo el service).
        public void ProyectarFecha(BE.ItemReporteMantenimientoJP86 item, DateTime hoy)
        {
            item.DiasEstimados = null;
            item.FechaEstimada = null;

            if (EstaEnTaller(item.Vehiculo.Estado))
                return;

            if (item.KmRestantes <= 0)
            {
                item.DiasEstimados = 0;
                item.FechaEstimada = hoy.Date;
                return;
            }

            if (item.Ritmo <= 0)
                return;

            item.DiasEstimados = (int)Math.Floor(item.KmRestantes / item.Ritmo);
            item.FechaEstimada = hoy.Date.AddDays(item.DiasEstimados.Value);
        }

        ///Un vencido se informa como ROJO aunque no tenga uso en la ventana: no necesita proyeccion
        ///para saber que ya supero el service, y como SIN_DATOS quedaria oculto en gris.
        public BE.SemaforoMantenimientoJP86 ClasificarSemaforo(BE.ItemReporteMantenimientoJP86 item)
        {
            if (EstaEnTaller(item.Vehiculo.Estado))
                return BE.SemaforoMantenimientoJP86.EN_TALLER;

            if (item.KmRestantes <= 0)
                return BE.SemaforoMantenimientoJP86.ROJO;

            if (item.Ritmo <= 0 || !item.DiasEstimados.HasValue)
                return BE.SemaforoMantenimientoJP86.SIN_DATOS;

            if (item.DiasEstimados.Value <= DIAS_ROJO)
                return BE.SemaforoMantenimientoJP86.ROJO;

            if (item.DiasEstimados.Value <= DIAS_AMARILLO || item.KmRestantes <= UMBRAL_AMARILLO_KM)
                return BE.SemaforoMantenimientoJP86.AMARILLO;

            return BE.SemaforoMantenimientoJP86.VERDE;
        }

        ///Devuelve solo el codigo y sus parametros (KmExcedidos, DiasEstimados, FechaEstimada): el texto lo arma la UI
        public void GenerarRecomendacion(BE.ItemReporteMantenimientoJP86 item)
        {
            switch (item.Semaforo)
            {
                case BE.SemaforoMantenimientoJP86.EN_TALLER:
                    item.Recomendacion = BE.RecomendacionMantenimientoJP86.EN_TALLER;
                    break;
                case BE.SemaforoMantenimientoJP86.SIN_DATOS:
                    item.Recomendacion = BE.RecomendacionMantenimientoJP86.SIN_DATOS;
                    break;
                case BE.SemaforoMantenimientoJP86.ROJO:
                    item.Recomendacion = item.KmRestantes <= 0
                        ? BE.RecomendacionMantenimientoJP86.VENCIDO
                        : BE.RecomendacionMantenimientoJP86.PROGRAMAR_REVISION;
                    break;
                case BE.SemaforoMantenimientoJP86.AMARILLO:
                    item.Recomendacion = BE.RecomendacionMantenimientoJP86.PLANIFICAR_REVISION;
                    break;
                default:
                    item.Recomendacion = BE.RecomendacionMantenimientoJP86.SIN_ACCION;
                    break;
            }

            item.RevisarInspeccionInterna = item.ReportesCliente > item.ReportesInternos;
        }
        #endregion

        #region Validaciones
        private void ValidarPermiso()
        {
            Sesion.Usuario usuario = Sesion.SessionManager.GetInstance.Usuario;
            if (usuario == null || usuario.Rol == null || !usuario.Rol.TienePermiso(Sesion.Patentes_64PR.ReporteMantenimientoPreventivo))
                throw new InvalidOperationException("err_ReporteMantenimiento_SinPermiso");
        }
        #endregion

        #region Metodos privados
        private bool EstaEnTaller(BE.EstadoVehiculoJP86 estado)
        {
            return estado == BE.EstadoVehiculoJP86.EN_REVISION || estado == BE.EstadoVehiculoJP86.EN_REPARACION;
        }
        #endregion
    }
}
