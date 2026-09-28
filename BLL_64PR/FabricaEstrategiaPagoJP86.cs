using System;

namespace BLL_64PR
{
    public static class FabricaEstrategiaPagoJP86
    {
        public static IEstrategiaPagoJP86 Obtener(BE.MetodoPagoJP86 metodoPago)
        {
            switch (metodoPago)
            {
                case BE.MetodoPagoJP86.EFECTIVO:
                    return new PagoEfectivoJP86();
                case BE.MetodoPagoJP86.TARJETA:
                    return new PagoTarjetaJP86();
                case BE.MetodoPagoJP86.TRANSFERENCIA:
                    return new PagoTransferenciaJP86();
                case BE.MetodoPagoJP86.MERCADOPAGO:
                    return new PagoMercadoPagoJP86();
                default:
                    throw new ArgumentOutOfRangeException(nameof(metodoPago), metodoPago, "Método de pago no soportado");
            }
        }
    }
}
