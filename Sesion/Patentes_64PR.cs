using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sesion
{
    public enum Patentes_64PR
    {
        ///esta clase contiene unicamente los "identificadores" para las patentes (se utilizo el nombre patente y no permiso, para diferenciarlas aunque sean sinonimos)
        ///se hace con el objetivo de no usar strings en crudo, y si en la base de datos se modifica una patente, esa
        ///patente se cambie una sola vez aqui. Cabe resaltar que el valor de la constante debe coincidir EXACTAMENTE con la BD

        CrearUsuario = 1,
        DesbloquearUsuario = 2,
        ModificarUsuario = 3,
        ActivarDesactivarUsuarios = 4,
        Bitacora = 5,
        CrearRoles = 6,
        ModificarRoles = 7,
        EliminarRoles = 8,
        CrearFamilias = 9,
        ModificarFamilias = 10,
        EliminarFamilias = 11,
        RepararIntegridadDV = 12,
        CambiarContra = 13,
        CambiarIdioma = 14,
        Respaldos = 16,
        Restauraciones = 17,

        ///Maestro de Categorias (RF1)
        CrearCategoria =18,
         ModificarCategoria = 19,
         EliminarCategoria = 20,

        ///Maestro de Vehiculos (RF1)
         CrearVehiculo = 21,
         ModificarVehiculo = 22,
         EliminarVehiculo = 23,
         CambiarEstadoVehiculo = 24,
         VerBitacoraCambiosVehiculo = 35,

        ///Maestro de Clientes (RF1)
         CrearCliente = 25,
         ModificarCliente = 26,
         EliminarCliente = 27,
         SerializarClientes = 36,
         DeserializarClientes = 37,

        ///Gestion de Alquileres (RFN1)
         GenerarContrato = 28,
         GenerarFactura = 29,
         RegistrarDevolucion = 30,

        ///Gestion de Mantenimiento (RFN2)
         RegistrarReporte = 31,
         AsignarCriticidad = 32,
         DeterminarModalidad = 33,
         RegistrarAcreditacion = 34
    }
}
