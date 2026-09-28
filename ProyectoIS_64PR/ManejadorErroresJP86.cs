using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ProyectoIS_64PR
{
    ///Punto unico para los errores NO previstos: lo usan los catch (Exception) de respaldo de los forms
    ///y los handlers globales de Program.cs (Application.ThreadException / AppDomain.UnhandledException).
    public static class ManejadorErroresJP86
    {
        ///Textos de respaldo por si el error ocurre antes de que se haya cargado un idioma (ej. antes del login).
        private const string RespaldoInesperado = "Ocurrio un error inesperado. La operacion puede no haberse completado.";
        private const string RespaldoFatal = "Ocurrio un error inesperado y la aplicacion debe cerrarse.";

        ///Escala de criticidad de la bitacora: 1 = maxima, 5 = minima
        private const int CriticidadErrorNoControlado = 1;

        public static void MostrarErrorInesperado(Exception ex)
        {
            RegistrarEnBitacora();
            Mostrar("err_Inesperado", RespaldoInesperado, ex);
        }

        public static void MostrarErrorFatal(Exception ex)
        {
            RegistrarEnBitacora();
            Mostrar("err_Fatal", RespaldoFatal, ex);
        }

        ///Solo se registra con un usuario logueado (Evento_64PR.Login tiene FK a USUARIO_64PR).
        ///Nunca lanza: si la BD es justamente la que fallo, el registro se pierde pero el mensaje se muestra igual.
        private static void RegistrarEnBitacora()
        {
            try
            {
                Sesion.Usuario usuario = Sesion.SessionManager.GetInstance.Usuario;
                if (usuario == null)
                    return;

                Bitacora.Evento_64PR ev = new Bitacora.Evento_64PR(usuario.Login, ((int)Bitacora.Bitacora_64PR.ModuloBitacora_64PR.Sistema).ToString(), ((int)Bitacora.Bitacora_64PR.TipoEventoBitacora_64PR.ErrorNoControlado).ToString(), CriticidadErrorNoControlado);
                new Bitacora.Bitacora_64PR().RegistrarEvento(ev);
            }
            catch
            {
                ///Intencional: el manejador de errores no puede generar un nuevo error no controlado
            }
        }

        private static void Mostrar(string clave, string respaldo, Exception ex)
        {
            Dictionary<string, string> textos = Idioma.GestorIdioma_64PR.GetInstance.ObtenerTextos();
            string mensaje = textos != null && textos.ContainsKey(clave) ? textos[clave] : respaldo;
            string titulo = textos != null && textos.ContainsKey("titulo_Error") ? textos["titulo_Error"] : "Error";

            ///El Message puede ser una clave de idioma (ej. err_BD_SinRespuesta lanzada por un mapper)
            if (ex != null)
                mensaje += Environment.NewLine + Environment.NewLine + Traductor_64PR.TraducirMensaje(textos, ex.Message);

            MessageBox.Show(mensaje, titulo, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
