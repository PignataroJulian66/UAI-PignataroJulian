using System.Collections.Generic;

namespace ConfiguradorBDJP86
{
    ///Resultado del diagnostico de una base de datos existente (reinstalacion)
    public class ResultadoDiagnosticoJP86
    {
        public bool BaseExiste { get; set; }
        public string Estado { get; set; }
        public string VersionEsperada { get; set; }
        public string VersionEncontrada { get; set; }
        public List<string> ObjetosFaltantes { get; private set; } = new List<string>();
        public List<string> Problemas { get; private set; } = new List<string>();

        ///La BD existe y no se encontro ningun problema: se conserva intacta
        public bool Correcto
        {
            get { return BaseExiste && Problemas.Count == 0; }
        }
    }
}
