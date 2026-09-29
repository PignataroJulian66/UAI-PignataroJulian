using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BLL_64PR
{
    public class ClienteJP86
    {
        Mapper.mpp_cliente mpp = new Mapper.mpp_cliente();
        Mapper.mpp_contrato mppContrato = new Mapper.mpp_contrato();
        private static readonly DV.DV_64PR recalculador = new DV.DV_64PR();

        ///Unico lugar con las reglas de formato del cliente (antes estaban copiadas en FrmGenerarContratoJP86 y FrmGestionarClientesJP86)
        private static readonly Regex RegexDNI = new Regex(@"^\d{7,8}$");
        private static readonly Regex RegexTelefono = new Regex(@"^(\+?54)?0?\d{8,11}$");

        public List<BE.ClienteJP86> Listar()
        {
            return mpp.Listar();
        }

        ///Solo los clientes activos pueden alquilar (CUN-01)
        public List<BE.ClienteJP86> ListarActivos()
        {
            return mpp.Listar().Where(c => c.Activo).ToList();
        }

        public void Crear(BE.ClienteJP86 c)
        {
            ///Las excepciones llevan como Message una clave de idioma
            if (c.DNI == null || !RegexDNI.IsMatch(c.DNI))
                throw new InvalidOperationException("msg_DNIValido");
            ValidarDatosPersonales(c);
            ///La PK de ClienteJP86.DNI sigue siendo la garantia final 
            if (mpp.ExisteDNI(c.DNI))
                throw new InvalidOperationException("msg_DNIDuplicado");

            mpp.Crear(c);
            recalculador.RecalcularTabla("ClienteJP86");
        }

        public void Modificar(BE.ClienteJP86 c)
        {
            ValidarDatosPersonales(c);

            mpp.Modificar(c);
            recalculador.RecalcularTabla("ClienteJP86");
        }

        private void ValidarDatosPersonales(BE.ClienteJP86 c)
        {
            if (string.IsNullOrWhiteSpace(c.Nombre))
                throw new InvalidOperationException("msg_NombreValido");
            if (string.IsNullOrWhiteSpace(c.Apellido))
                throw new InvalidOperationException("msg_ApellidoValido");

            ///Se valida el telefono sin espacios ni guiones, pero se guarda tal como lo cargo el usuario
            string telefonoNormalizado = Regex.Replace(c.Telefono ?? string.Empty, @"[\s\-]", "");
            if (!RegexTelefono.IsMatch(telefonoNormalizado))
                throw new InvalidOperationException("msg_TelefonoValido");
        }

        //si llega activo es una baja (se valida); si llega inactivo es una reactivacion
        public void ActDesact(BE.ClienteJP86 c)
        {
            ///Baja logica: los contratos CERRADO no bloquean, solo los que siguen abiertos
            if (c.Activo && mppContrato.ExisteAbiertoPorCliente(c.DNI))
                throw new InvalidOperationException("err_BajaCliente_ContratoAbierto");

            mpp.ActDesact(c);
            recalculador.RecalcularTabla("ClienteJP86");
        }

        ///Serializa a XML los clientes recibidos (los visibles en la grilla) y relee el archivo para verificarlo.
        ///Devuelve la cantidad de clientes verificada. No toca la BD (ni el DV).
        public int Serializar(List<BE.ClienteJP86> clientes, string ruta)
        {
            SerializadorXmlJP86.Serializar(clientes, ruta);

            List<BE.ClienteJP86> releidos = SerializadorXmlJP86.Deserializar<BE.ClienteJP86>(ruta);
            if (releidos.Count != clientes.Count)
                throw new InvalidOperationException("err_Xml_VerificacionFallida");

            return releidos.Count;
        }

        ///Des-serializa un XML de clientes solo para visualizarlo: NO persiste nada en la BD.
        public List<BE.ClienteJP86> Deserializar(string ruta)
        {
            List<BE.ClienteJP86> clientes = SerializadorXmlJP86.Deserializar<BE.ClienteJP86>(ruta);

            if (clientes.Count == 0)
                throw new InvalidOperationException("err_Xml_SinClientes");

            
            ///devuelve clientes sin datos. Sin DNI/Nombre/Apellido no es un archivo de clientes valido.
            if (clientes.Any(c => c == null || string.IsNullOrWhiteSpace(c.DNI) || string.IsNullOrWhiteSpace(c.Nombre) || string.IsNullOrWhiteSpace(c.Apellido)))
                throw new InvalidOperationException("err_Xml_EstructuraInvalida");

            return clientes;
        }
    }
}
