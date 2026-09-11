using System;

namespace BE
{
    public class ContratoJP86
    {
        public int NumeroContrato { get; set; }
        public BE.ClienteJP86 Cliente { get; set; }
        public BE.VehiculoJP86 Vehiculo { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal TarifaDiaria { get; set; }
        public bool CargosAdicionales { get; set; }
        public decimal ImporteTotal { get; set; }
        public int KilometrajeEntrega { get; set; }
        public int? KilometrajeRetorno { get; set; }
        public EstadoUnidadDevolucionJP86? EstadoUnidadDevolucion { get; set; }
        public EstadoContratoJP86 Estado { get; set; }

        public override string ToString()
        {
            return NumeroContrato.ToString();
        }
    }
}
