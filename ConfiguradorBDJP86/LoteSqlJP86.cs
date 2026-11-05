namespace ConfiguradorBDJP86
{
    ///Un lote del script (texto entre dos GO). ContraMaster indica si va antes de la marca de conexion.
    public class LoteSqlJP86
    {
        public int Numero { get; set; }
        public int LineaInicial { get; set; }
        public string Texto { get; set; }
        public bool ContraMaster { get; set; }

        ///Primera linea con contenido, para el log
        public string Resumen
        {
            get
            {
                foreach (string linea in Texto.Split('\n'))
                {
                    string l = linea.Trim();
                    if (l.Length > 0 && !l.StartsWith("--")) return l.Length > 90 ? l.Substring(0, 90) + "..." : l;
                }
                return "(vacio)";
            }
        }
    }
}
