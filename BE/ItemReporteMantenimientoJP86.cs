using System;

namespace BE
{
    ///Una fila del Reporte de Mantenimiento Preventivo (DTO de solo lectura, no es una tabla).
    ///Los datos crudos los carga el mapper desde SP_ReporteMantenimientoPreventivoJP86_Obtener;
    ///los calculados los completa BLL_64PR.ReporteMantenimientoJP86.
    public class ItemReporteMantenimientoJP86
    {
        #region Columnas de la grilla
        public BE.VehiculoJP86 Vehiculo { get; set; }
        public BE.Categoria_64PR Categoria { get; set; }
        public int KmActual { get; set; }
        public int KmDesdeRevision { get; set; }
        public int KmRestantes { get; set; }
        public decimal Ritmo { get; set; }
        public int? DiasEstimados { get; set; }
        public DateTime? FechaEstimada { get; set; }
        public int ReportesInternos { get; set; }
        public int ReportesCliente { get; set; }
        public SemaforoMantenimientoJP86 Semaforo { get; set; }
        #endregion

        #region Recomendacion (codigo + parametros)
        public RecomendacionMantenimientoJP86 Recomendacion { get; set; }
        public int KmExcedidos { get; set; }
        public bool RevisarInspeccionInterna { get; set; }
        #endregion

        #region Datos crudos del SP
        public DateTime? FechaUltimaRevision { get; set; }
        public int? KmDevolucionPreviaRevision { get; set; }
        public DateTime? FechaPrimerContrato { get; set; }
        public int? KmEntregaPrimerContrato { get; set; }
        public int KmRecorridosVentana { get; set; }
        public DateTime? FechaPrimerContratoVentana { get; set; }
        #endregion

        #region Calculados de referencia
        public int KmUltimaRevision { get; set; }
        public DateTime? FechaReferenciaRevision { get; set; }
        #endregion
    }
}
