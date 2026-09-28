using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace BLL_64PR
{
    ///Serializacion XML generica (XmlSerializer) de una lista de objetos a un archivo del disco.
    ///No conoce ninguna entidad: la usan las BLL de cada maestro (ej. BLL_64PR.ClienteJP86).
    ///Las excepciones llevan como Message una clave de idioma (la UI la traduce), igual que el resto de las BLL.
    public static class SerializadorXmlJP86
    {
        public static void Serializar<T>(List<T> lista, string ruta)
        {
            if (lista == null || lista.Count == 0)
                throw new InvalidOperationException("err_Xml_SinDatos");

            string rutaCompleta = ValidarRuta(ruta);

            string directorio = Path.GetDirectoryName(rutaCompleta);
            if (string.IsNullOrEmpty(directorio) || !Directory.Exists(directorio))
                throw new InvalidOperationException("err_Xml_DirectorioInexistente");

            XmlSerializer serializador = new XmlSerializer(typeof(List<T>));
            XmlWriterSettings configuracion = new XmlWriterSettings { Indent = true, Encoding = new UTF8Encoding(false) };

            try
            {
                ///FileMode.Create: si el archivo existe se sobrescribe (el SaveFileDialog ya pidio confirmacion al operador)
                using (FileStream archivo = new FileStream(rutaCompleta, FileMode.Create, FileAccess.Write))
                using (XmlWriter escritor = XmlWriter.Create(archivo, configuracion))
                {
                    serializador.Serialize(escritor, lista);
                }
            }
            catch (UnauthorizedAccessException)
            {
                throw new InvalidOperationException("err_Xml_SinPermisosEscritura");
            }
            catch (SecurityException)
            {
                throw new InvalidOperationException("err_Xml_SinPermisosEscritura");
            }
            catch (IOException)
            {
                ///Archivo abierto por otro programa, disco lleno, unidad desconectada, etc.
                throw new InvalidOperationException("err_Xml_ErrorEscritura");
            }
        }

        public static List<T> Deserializar<T>(string ruta)
        {
            string rutaCompleta = ValidarRuta(ruta);

            if (!File.Exists(rutaCompleta))
                throw new InvalidOperationException("err_Xml_ArchivoInexistente");

            XmlSerializer serializador = new XmlSerializer(typeof(List<T>));
            ///DtdProcessing.Prohibit (default de XmlReader.Create): un XML con DTD/entidades externas se rechaza
            XmlReaderSettings configuracion = new XmlReaderSettings { DtdProcessing = DtdProcessing.Prohibit, XmlResolver = null };

            try
            {
                using (FileStream archivo = new FileStream(rutaCompleta, FileMode.Open, FileAccess.Read))
                {
                    if (archivo.Length == 0)
                        throw new InvalidOperationException("err_Xml_ArchivoVacio");

                    using (XmlReader lector = XmlReader.Create(archivo, configuracion))
                    {
                        if (!serializador.CanDeserialize(lector))
                            throw new InvalidOperationException("err_Xml_EstructuraInvalida");

                        List<T> resultado = serializador.Deserialize(lector) as List<T>;
                        if (resultado == null)
                            throw new InvalidOperationException("err_Xml_EstructuraInvalida");
                        return resultado;
                    }
                }
            }
            catch (XmlException)
            {
                ///CanDeserialize lee el elemento raiz: si el archivo no es XML bien formado falla ahi
                throw new InvalidOperationException("err_Xml_Corrupto");
            }
            catch (InvalidOperationException ex) when (!ex.Message.StartsWith("err_Xml_"))
            {
                ///XmlSerializer envuelve los errores en InvalidOperationException: XmlException adentro = XML mal formado;
                ///cualquier otra cosa (tipo de dato invalido, ej. "abc" en un bool) = estructura que no corresponde a la clase
                if (ex.InnerException is XmlException)
                    throw new InvalidOperationException("err_Xml_Corrupto");
                throw new InvalidOperationException("err_Xml_EstructuraInvalida");
            }
            catch (UnauthorizedAccessException)
            {
                throw new InvalidOperationException("err_Xml_SinPermisosLectura");
            }
            catch (SecurityException)
            {
                throw new InvalidOperationException("err_Xml_SinPermisosLectura");
            }
            catch (IOException)
            {
                throw new InvalidOperationException("err_Xml_ErrorLectura");
            }
        }

        ///Devuelve la ruta absoluta o lanza la clave de idioma que corresponda
        private static string ValidarRuta(string ruta)
        {
            if (string.IsNullOrWhiteSpace(ruta))
                throw new InvalidOperationException("err_Xml_RutaVacia");

            string rutaCompleta;
            try
            {
                rutaCompleta = Path.GetFullPath(ruta.Trim());
            }
            catch (Exception ex) when (ex is ArgumentException || ex is NotSupportedException || ex is PathTooLongException || ex is SecurityException)
            {
                throw new InvalidOperationException("err_Xml_RutaInvalida");
            }

            if (!string.Equals(Path.GetExtension(rutaCompleta), ".xml", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("err_Xml_RutaInvalida");

            return rutaCompleta;
        }
    }
}
