using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class VehiculoJP86
    {
        public string Patente { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public BE.Categoria_64PR Categoria { get; set; }
        public int Kilometraje { get; set; }
        public EstadoVehiculoJP86 Estado { get; set; }
        public bool Activo { get; set; }

        public override string ToString()
        {
            return Patente;
        }
    }
}
