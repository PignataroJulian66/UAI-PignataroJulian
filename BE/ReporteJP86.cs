using System;

namespace BE
{
    public class ReporteJP86
    {
        public int NumeroReporte { get; set; }
        public BE.VehiculoJP86 Vehiculo { get; set; }
        public DateTime FechaHora { get; set; }
        public string DescripcionProblema { get; set; }
        public FuenteReporteJP86 Fuente { get; set; }
        public CriticidadReporteJP86? Criticidad { get; set; }
        public ModalidadReparacionJP86? ModalidadReparacion { get; set; }
        public string DescripcionCierre { get; set; }
        public DateTime? FechaAcreditacion { get; set; }
        public string ResponsableCierreDNI { get; set; }
        public EstadoReporteJP86 Estado { get; set; }

        public override string ToString()
        {
            return NumeroReporte.ToString();
        }
    }
}
