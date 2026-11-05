namespace BLL_64PR
{
    ///[INSTALADOR v2] Verificacion al iniciar el sistema: si el sistema instalado todavia no tiene la base de datos
    ///configurada (config con #SQLINSTANCE#) lanza InvalidOperationException con el mensaje para el usuario.
    ///No registra en bitacora: sin BD configurada no hay donde registrar.
    public static class ConfiguracionSistemaJP86
    {
        public static void VerificarConfiguracion()
        {
            new Mapper.mpp_configuracionSistemaJP86().VerificarConfiguracion();
        }
    }
}
