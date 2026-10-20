using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sesion
{
    public class Permiso_64PR : Rol_64PR
    {
        public override bool TienePermiso(Patentes_64PR id)
        {
            return this.Id == ((int)id);
        }
    }
}
