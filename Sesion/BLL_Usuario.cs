using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sesion
{
    public class BLL_Usuario
    {
        Sesion.mpp_usuario mpp = new Sesion.mpp_usuario();
        private static readonly DV.DV_64PR recalculador = new DV.DV_64PR();
        public void Actdesact(Sesion.Usuario u)
        {
            mpp.Actdesact(u);
            recalculador.RecalcularTabla("USUARIO_64PR");
        }

        public bool BloqueadoInactivo(string login)
        {
            return mpp.BloqueadoInactivo(login);
        }

        public void Crear(Sesion.Usuario u)
        {
            mpp.Crear(u);
            recalculador.RecalcularTabla("USUARIO_64PR");
        }

        public void Desbloquear(Sesion.Usuario u)
        {
            mpp.Desbloquear(u);
            recalculador.RecalcularTabla("USUARIO_64PR");
        }

        public List<Sesion.Usuario> Listar()
        {
            return mpp.Listar();
        }

        public void Modificar(Sesion.Usuario u)
        {
            mpp.Modificar(u);
            recalculador.RecalcularTabla("USUARIO_64PR");
        }

        public bool VerificarClave(string login, string contra)
        {
            return mpp.VerificarClave(login, contra);
        }

        public byte[] ObtenerHashAlmacenado(string login)
        {
            return mpp.ObtenerHashAlmacenado(login);
        }

        public void SumarIntento(string login)
        {
            mpp.SumarIntento(login);
            recalculador.RecalcularTabla("USUARIO_64PR");
        }

        public bool ExisteUsuario(string login)
        {
            return mpp.ExisteUsuario(login);
        }

        public void ReiniciarIntentos(string login)
        {
            mpp.ReiniciarIntentos(login);
            recalculador.RecalcularTabla("USUARIO_64PR");
        }

        public Sesion.Usuario ObtenerUsuario(string login)
        {
            return mpp.ObtenerUsuario(login);
        }
        public void CambiarClave(string nueva, string confirmacion)
        {
            mpp.CambiarClave(nueva, confirmacion);
            recalculador.RecalcularTabla("USUARIO_64PR");
        }

        public string ObtenerIntentos(string login)
        {
            return mpp.ObtenerIntentos(login);
        }
        public string ObtenerIdioma(string login)
        {
            return mpp.ObtenerIdioma(login);
        }

        public void GuardarIdioma(string login, string idioma)
        {
            mpp.GuardarIdioma(login, idioma);
            recalculador.RecalcularTabla("USUARIO_64PR");
        }
    }
}