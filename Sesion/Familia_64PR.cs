using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sesion
{
    public class Familia_64PR : Rol_64PR
    {
        public override bool TienePermiso(Patentes_64PR id)
        {
            foreach (var hijo in Hijos)
                if (hijo.TienePermiso(id)) return true;
            return false;
        }
    }
}
