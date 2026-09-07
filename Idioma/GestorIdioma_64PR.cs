using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
namespace Idioma
{
    public class GestorIdioma_64PR
    {
        private static GestorIdioma_64PR _instancia;

        public static GestorIdioma_64PR GetInstance
        {
            get
            {
                if (_instancia == null)
                    _instancia = new GestorIdioma_64PR();
                return _instancia;
            }
        }

        private GestorIdioma_64PR() { }

        private string _idiomaActual = "es";   ///Idioma por defecto
        private Dictionary<string, string> _textos = new Dictionary<string, string>();
        private readonly List<IObservadorIdioma_64PR> _observadores = new List<IObservadorIdioma_64PR>();

        public string IdiomaActual => _idiomaActual;

        ///Carpeta donde están los JSON. Usamos AppDomain para que funcione tanto en Debug como publicado.
        private string RutaIdiomas => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "idiomas");

        public void Suscribir(IObservadorIdioma_64PR observador)
        {
            if (!_observadores.Contains(observador))
                _observadores.Add(observador);
        }

        public void Desuscribir(IObservadorIdioma_64PR observador)
        {
            _observadores.Remove(observador);
        }

        /// <summary>
        /// Cambiar idioma:
        /// Carga el JSON del idioma indicado y notifica a todos los observadores.
        /// </summary>
        /// <param name="codigoIdioma">Ej: "es" o "en"</param>
        public void SetIdioma(string codigoIdioma)
        {
            string ruta = Path.Combine(RutaIdiomas, codigoIdioma + ".json");

            if (!File.Exists(ruta))
            {
                throw new FileNotFoundException(
                    $"No se encontró el archivo de idioma: {ruta}");
            }

            string json = File.ReadAllText(ruta, System.Text.Encoding.UTF8);

            ///El JavaScriptSerializer está disponible en .NET sin necesidad de NuGet
            var serializer = new JavaScriptSerializer();
            _textos = serializer.Deserialize<Dictionary<string, string>>(json);

            _idiomaActual = codigoIdioma;

            NotificarObservadores();
        }
        private void NotificarObservadores()
        {
            ///Iteramos sobre una copia para evitar problemas si algún
            ///observador se desuscribe durante la notificación.
            foreach (var obs in new List<IObservadorIdioma_64PR>(_observadores))
                obs.ActualizarIdioma(_textos);
        }

        /// <summary>
        /// Devuelve los textos actuales (útil para que un form recién
        /// abierto se inicialice sin esperar una notificación).
        /// </summary>
        public Dictionary<string, string> ObtenerTextos() => _textos;
        public List<string> IdiomasDisponibles()
        {
            /// Lista los idiomas disponibles leyendo los JSON en la carpeta.
            var lista = new List<string>();
            if (!Directory.Exists(RutaIdiomas)) return lista;

            foreach (string archivo in Directory.GetFiles(RutaIdiomas, "*.json"))
                lista.Add(Path.GetFileNameWithoutExtension(archivo));

            return lista;
        }
        public void RegistrarClavesFaltantes(List<string> claves)
        {
            try
            {
                var serializer = new JavaScriptSerializer();

                foreach (var codigoIdioma in IdiomasDisponibles()) // ej: "es", "en"
                {
                    string rutaArchivo = Path.Combine(RutaIdiomas, codigoIdioma + ".json");

                    string json = File.ReadAllText(rutaArchivo, System.Text.Encoding.UTF8);
                    var diccionario = serializer.Deserialize<Dictionary<string, string>>(json);

                    bool huboCambios = false;
                    foreach (var clave in claves)
                    {
                        if (!diccionario.ContainsKey(clave))
                        {
                            diccionario[clave] = "FALTA TRADUCCION";
                            huboCambios = true;
                        }
                    }

                    if (huboCambios)
                    {
                        string jsonActualizado = serializer.Serialize(diccionario);

                        ///formato visual (saltos de línea y espacios)
                        jsonActualizado = FormatearJson(jsonActualizado);

                        File.WriteAllText(rutaArchivo, jsonActualizado, System.Text.Encoding.UTF8);

                        ///Si el idioma actual es el que se modificó, refrescamos _textos y notificamos
                        if (codigoIdioma == _idiomaActual)
                        {
                            _textos = diccionario;
                            NotificarObservadores();
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw new Exception("Error en RegistrarClavesFaltantes: " + ex.Message);
            }
        }
        private string FormatearJson(string json)
        {
            var stringBuilder = new StringBuilder();
            bool entreComillas = false;
            int nivelIndentacion = 0;

            foreach (char c in json)
            {
                switch (c)
                {
                    case '"':
                        stringBuilder.Append(c);
                        entreComillas = !entreComillas;
                        break;
                    case '{':
                    case '[':
                        stringBuilder.Append(c);
                        if (!entreComillas)
                        {
                            stringBuilder.AppendLine();
                            nivelIndentacion++;
                            stringBuilder.Append(new string(' ', nivelIndentacion * 4)); // 4 espacios de sangría
                        }
                        break;
                    case '}':
                    case ']':
                        if (!entreComillas)
                        {
                            stringBuilder.AppendLine();
                            nivelIndentacion--;
                            stringBuilder.Append(new string(' ', nivelIndentacion * 4));
                        }
                        stringBuilder.Append(c);
                        break;
                    case ',':
                        stringBuilder.Append(c);
                        if (!entreComillas)
                        {
                            stringBuilder.AppendLine();
                            stringBuilder.Append(new string(' ', nivelIndentacion * 4));
                        }
                        break;
                    case ':':
                        stringBuilder.Append(c);
                        if (!entreComillas)
                            stringBuilder.Append(" ");
                        break;
                    default:
                        stringBuilder.Append(c);
                        break;
                }
            }

            return stringBuilder.ToString();
        }
    }
}
