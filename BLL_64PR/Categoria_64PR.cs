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
        Mapper.mpp_vehiculo mppVehiculo = new Mapper.mpp_vehiculo();
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

        ///Toggle: si llega activa es una baja (se valida); si llega inactiva es una reactivacion (sin reglas)
        public void ActDesact(BE.Categoria_64PR c)
        {
            if (c.Activo && mppVehiculo.ExisteActivoPorCategoria(c.Id))
                throw new InvalidOperationException("err_BajaCategoria_VehiculosAsociados");

            mpp.ActDesact(c);
            recalculador.RecalcularTabla("Categoria_64PR");
        }
    }
}
