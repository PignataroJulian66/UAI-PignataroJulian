using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sesion
{
    public static class Patentes_64PR
    {
        ///esta clase contiene unicamente los "identificadores" para las patentes (se utilizo el nombre patente y no permiso, para diferenciarlas aunque sean sinonimos)
        ///se hace con el objetivo de no usar strings en crudo, y si en la base de datos se modifica una patente, esa
        ///patente se cambie una sola vez aqui. Cabe resaltar que el valor de la constante debe coincidir EXACTAMENTE con la BD

        public const string CrearUsuario = "Crear usuario";
        public const string DesbloquearUsuario = "Desbloquear usuario";
        public const string ModificarUsuario = "Modificar usuario";
        public const string ActivarDesactivarUsuarios = "Activar y desactivar usuarios";
        public const string Bitacora = "Bitacora";
        public const string CrearRoles = "Crear roles";
        public const string ModificarRoles = "Modificar roles";
        public const string EliminarRoles = "Eliminar roles";
        public const string CrearFamilias = "Crear familias";
        public const string ModificarFamilias = "Modificar familias";
        public const string EliminarFamilias = "Eliminar familias";
        public const string CambiarContra = "Cambiar clave";
        public const string CambiarIdioma = "Cambiar idioma";
        public const string Respaldos = "Hacer respaldos";
        public const string Restauraciones = "Hacer restauraciones";

        ///Maestro de Categorias (RF1)
        public const string CrearCategoria = "Crear categoria";
        public const string ModificarCategoria = "Modificar categoria";
        public const string EliminarCategoria = "Eliminar categoria";

        ///Maestro de Vehiculos (RF1)
        public const string CrearVehiculo = "Crear vehiculo";
        public const string ModificarVehiculo = "Modificar vehiculo";
        public const string EliminarVehiculo = "Eliminar vehiculo";
        public const string CambiarEstadoVehiculo = "Cambiar estado vehiculo";

        ///Maestro de Clientes (RF1)
        public const string CrearCliente = "Crear cliente";
        public const string ModificarCliente = "Modificar cliente";
        public const string EliminarCliente = "Eliminar cliente";

        ///Gestion de Alquileres (RFN1)
        public const string GenerarContrato = "Generar contrato";
        public const string GenerarFactura = "Generar factura";
        public const string RegistrarDevolucion = "Registrar devolucion";

        ///Gestion de Mantenimiento (RFN2)
        public const string RegistrarReporte = "Registrar reporte";
        public const string AsignarCriticidad = "Asignar criticidad";
        public const string DeterminarModalidad = "Determinar modalidad";
        public const string RegistrarAcreditacion = "Registrar acreditacion";
    }
}
