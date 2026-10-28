using System.Collections.Generic;

namespace BE
{
    ///Panel de resumen del Reporte de Mantenimiento Preventivo
    public class ResumenReporteMantenimientoJP86
    {
        public Dictionary<SemaforoMantenimientoJP86, int> CantidadPorSemaforo { get; set; }
        ///null si ningun vehiculo del reporte tiene reportes de origen cliente
        public BE.Categoria_64PR CategoriaMasReportesCliente { get; set; }
        public int ReportesClienteCategoria { get; set; }
    }
}
