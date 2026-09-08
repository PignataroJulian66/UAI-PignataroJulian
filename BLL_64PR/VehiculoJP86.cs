using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_64PR
{
    public class VehiculoJP86
    {
        Mapper.mpp_vehiculo mpp = new Mapper.mpp_vehiculo();
        private static readonly DV.DV_64PR recalculador = new DV.DV_64PR();

        public List<BE.VehiculoJP86> Listar()
        {
            return mpp.Listar();
        }

        public void Crear(BE.VehiculoJP86 v)
        {
            mpp.Crear(v);
            recalculador.RecalcularTabla("VehiculoJP86");
        }

        public void Modificar(BE.VehiculoJP86 v)
        {
            mpp.Modificar(v);
            recalculador.RecalcularTabla("VehiculoJP86");
        }

        public void CambiarEstado(string patente, BE.EstadoVehiculoJP86 estado)
        {
            mpp.CambiarEstado(patente, estado);
            recalculador.RecalcularTabla("VehiculoJP86");
        }

        public void ActDesact(BE.VehiculoJP86 v)
        {
            mpp.ActDesact(v);
            recalculador.RecalcularTabla("VehiculoJP86");
        }
    }
}
