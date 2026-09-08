using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_64PR
{
    public class ClienteJP86
    {
        Mapper.mpp_cliente mpp = new Mapper.mpp_cliente();
        private static readonly DV.DV_64PR recalculador = new DV.DV_64PR();

        public List<BE.ClienteJP86> Listar()
        {
            return mpp.Listar();
        }

        public void Crear(BE.ClienteJP86 c)
        {
            mpp.Crear(c);
            recalculador.RecalcularTabla("ClienteJP86");
        }

        public void Modificar(BE.ClienteJP86 c)
        {
            mpp.Modificar(c);
            recalculador.RecalcularTabla("ClienteJP86");
        }

        public void ActDesact(BE.ClienteJP86 c)
        {
            mpp.ActDesact(c);
            recalculador.RecalcularTabla("ClienteJP86");
        }
    }
}
