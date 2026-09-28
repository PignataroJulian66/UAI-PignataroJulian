using System.Threading.Tasks;

namespace BLL_64PR
{
    public interface IEstrategiaPagoJP86
    {
        Task<BE.ResultadoPagoJP86> ProcesarPago(decimal monto);
    }
}
