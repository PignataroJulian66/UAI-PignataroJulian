using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DV
{
    public enum TipoAnomalia_64PR
    {
        Insercion,   /// Fila nueva que nunca tuvo DVH guardado
        Modificacion, /// Fila existente cuyo contenido cambió
        Eliminacion  /// Fila que estaba guardada y ya no existe
    }
}
