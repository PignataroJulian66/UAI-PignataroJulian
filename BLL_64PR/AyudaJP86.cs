using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;

namespace BLL_64PR
{
    ///Ayuda en linea: abre el manual PDF con el visor predeterminado de Windows y lo registra en la bitacora.
    ///Disponible para todo usuario logueado (no requiere patente).
    ///Las excepciones llevan como Message una clave de idioma, igual que el resto de las BLL.
    public static class AyudaJP86
    {
        #region Campos
        ///El PDF viaja junto al .exe en <directorioBase>\Ayuda (Content + Copiar si es posterior en ProyectoIS_64PR)
        private const string CarpetaAyuda = "Ayuda";
        private const string NombreArchivo = "SIGAM_AyudaEnLinea";

        ///Escala de criticidad de la bitacora: 1 = maxima, 5 = minima
        private const int CriticidadConsulta = 5;
        private const int CriticidadError = 3;

        ///Win32 ERROR_NO_ASSOCIATION: no hay ningun programa asociado a la extension .pdf
        private const int ErrorSinAsociacion = 1155;
        #endregion

        #region Operaciones
        public static void AbrirAyuda(string directorioBase, string idioma)
        {
            string ruta = ObtenerRutaAyuda(directorioBase, idioma);

            if (!File.Exists(ruta))
            {
                RegistrarEvento(Bitacora.Bitacora_64PR.TipoEventoBitacora_64PR.ErrorAperturaAyuda, CriticidadError);
                throw new InvalidOperationException("err_Ayuda_ArchivoInexistente");
            }

            try
            {
                ///UseShellExecute: Windows resuelve el visor asociado a .pdf (Process.Start puede devolver null si reutiliza una instancia)
                using (Process.Start(new ProcessStartInfo { FileName = ruta, UseShellExecute = true })) { }
            }
            catch (Win32Exception ex)
            {
                RegistrarEvento(Bitacora.Bitacora_64PR.TipoEventoBitacora_64PR.ErrorAperturaAyuda, CriticidadError);
                throw new InvalidOperationException(ex.NativeErrorCode == ErrorSinAsociacion ? "err_Ayuda_SinVisor" : "err_Ayuda_ErrorApertura");
            }

            RegistrarEvento(Bitacora.Bitacora_64PR.TipoEventoBitacora_64PR.ConsultaAyuda, CriticidadConsulta);
        }

        ///Version por idioma: SIGAM_AyudaEnLinea_<idioma>.pdf. Si no existe, la base SIGAM_AyudaEnLinea.pdf (español)
        public static string ObtenerRutaAyuda(string directorioBase, string idioma)
        {
            string carpeta = Path.Combine(directorioBase, CarpetaAyuda);

            if (!string.IsNullOrWhiteSpace(idioma))
            {
                string rutaIdioma = Path.Combine(carpeta, NombreArchivo + "_" + idioma.Trim().ToLowerInvariant() + ".pdf");
                if (File.Exists(rutaIdioma))
                    return rutaIdioma;
            }

            return Path.Combine(carpeta, NombreArchivo + ".pdf");
        }
        #endregion

        #region Metodos privados
        ///Solo se registra con un usuario logueado (Evento_64PR.Login tiene FK a USUARIO_64PR)
        private static void RegistrarEvento(Bitacora.Bitacora_64PR.TipoEventoBitacora_64PR tipo, int criticidad)
        {
            Sesion.Usuario usuario = Sesion.SessionManager.GetInstance.Usuario;
            if (usuario == null)
                return;

            Bitacora.Evento_64PR ev = new Bitacora.Evento_64PR(usuario.Login, ((int)Bitacora.Bitacora_64PR.ModuloBitacora_64PR.Sistema).ToString(), ((int)tipo).ToString(), criticidad);
            new Bitacora.Bitacora_64PR().RegistrarEvento(ev);
        }
        #endregion
    }
}
