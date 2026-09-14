using System;

namespace BE
{
    public class VehiculoBitacoraCambios
    {
        public int IdBitacora { get; set; }
        public string Patente { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int ID_Categoria { get; set; }
        public int Kilometraje { get; set; }
        public EstadoVehiculoJP86 Estado { get; set; }
        public bool Activo { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
        public bool Act { get; set; }
    }
}
