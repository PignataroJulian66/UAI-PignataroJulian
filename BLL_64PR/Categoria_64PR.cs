using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_64PR
{
    public class Categoria_64PR
    {
        Mapper.mpp_categoria mpp = new Mapper.mpp_categoria();
        private static readonly DV.DV_64PR recalculador = new DV.DV_64PR();

        public List<BE.Categoria_64PR> Listar()
        {
            return mpp.Listar();
        }

        public void Crear(BE.Categoria_64PR c)
        {
            mpp.Crear(c);
            recalculador.RecalcularTabla("Categoria_64PR");
        }

        public void Modificar(BE.Categoria_64PR c)
        {
            mpp.Modificar(c);
            recalculador.RecalcularTabla("Categoria_64PR");
        }

        public void ActDesact(BE.Categoria_64PR c)
        {
            mpp.ActDesact(c);
            recalculador.RecalcularTabla("Categoria_64PR");
        }
    }
}
