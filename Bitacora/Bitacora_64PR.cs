using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitacora
{
    public class Bitacora_64PR
    {
        public enum TipoEventoBitacora_64PR
        {
            LoginExitoso = 1,
            CambioClave = 2,
            LoginFallido = 3,
            UsuarioBloqueado = 4,
            Logout = 5,
            AltaUsuario = 6,
            ModificacionUsuario = 7,
            CreacionRol = 9,
            EliminacionRol = 10,
            ModificacionRol = 11,
            CreacionFamilia = 12,
            EliminacionFamilia = 13,
            ModificacionFamilia = 14,
            Backup = 15,
            Restore=16,
            AltaCategoria = 17,
            ModificacionCategoria = 18,
            BajaCategoria = 19,
            AltaVehiculo = 20,
            ModificacionVehiculo = 21,
            BajaVehiculo = 22,
            CambioEstadoVehiculo = 23,
            AltaCliente = 24,
            ModificacionCliente = 25,
            BajaCliente = 26
        }
        public enum ModuloBitacora_64PR
        {
            Login = 1,
            GestionUsuarios = 2,
            GestionRoles = 3,
            GestionFamilias = 4,
            GestionCategorias = 5,
            GestionVehiculos = 6,
            GestionClientes = 7
        }

        Bitacora.mpp_bitacora mpp = new Bitacora.mpp_bitacora();
        public List<Bitacora.Evento_64PR> ListarEventos()
        {
            return mpp.ListarEventos();
        }

        public List<string> ListarLogins()
        {
            return mpp.ListarLogins();
        }

        public List<string> ListarModulos()
        {
            return mpp.ListarModulos();
        }

        public List<string> ListarTipos()
        {
            return mpp.ListarTipos();
        }

        public void RegistrarEvento(Bitacora.Evento_64PR e)
        {
            mpp.RegistrarEvento(e);
        }
    }
}
