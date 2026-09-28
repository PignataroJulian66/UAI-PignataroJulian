using System;
using System.Threading.Tasks;

namespace BLL_64PR
{
    public class PagoMercadoPagoJP86 : IEstrategiaPagoJP86
    {
        private const int DelayMinMs = 600;
        private const int DelayMaxMs = 1200;

        public async Task<BE.ResultadoPagoJP86> ProcesarPago(decimal monto)
        {
            TimeSpan delay = TimeSpan.FromMilliseconds(new Random().Next(DelayMinMs, DelayMaxMs + 1));
            await Task.Delay(delay);

            return new BE.ResultadoPagoJP86()
            {
                Exito = true,
                TiempoProcesamiento = delay
            };
        }
    }
}
