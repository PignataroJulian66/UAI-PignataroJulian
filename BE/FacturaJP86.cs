using System;

namespace BE
{
    public class FacturaJP86
    {
        public int NumeroFactura { get; set; }
        public BE.ContratoJP86 Contrato { get; set; }
        public DateTime FechaFactura { get; set; }
        public MetodoPagoJP86 MetodoPago { get; set; }
        public decimal MontoFactura { get; set; }

        public override string ToString()
        {
            return NumeroFactura.ToString();
        }
    }
}
