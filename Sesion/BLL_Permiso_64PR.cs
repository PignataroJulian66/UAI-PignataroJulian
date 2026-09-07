using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sesion
{
    public class BLL_Permiso_64PR
    {
        Sesion.mpp_roles mpp = new Sesion.mpp_roles();
        private static readonly DV.DV_64PR recalculador = new DV.DV_64PR();
        public List<Sesion.Rol_64PR> ObtenerTodosLosNodos()
        {
            return mpp.ObtenerTodosLosNodos();
        }
    }
}
