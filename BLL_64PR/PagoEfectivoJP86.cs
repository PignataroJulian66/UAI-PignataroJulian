using System;
using System.Threading.Tasks;

namespace BLL_64PR
{
    public class PagoEfectivoJP86 : IEstrategiaPagoJP86
    {
        private const int DelayMinMs = 200;
        private const int DelayMaxMs = 500;

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
