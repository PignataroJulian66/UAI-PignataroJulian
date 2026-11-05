namespace Mapper
{
    ///[INSTALADOR v2] Acceso a la verificacion de configuracion de DAL_64PR.Acceso (patron BLL -> Mapper -> DAL)
    public class mpp_configuracionSistemaJP86
    {
        ///Lanza InvalidOperationException si el .config todavia tiene el marcador #SQLINSTANCE#
        public void VerificarConfiguracion()
        {
            DAL_64PR.Acceso.Instancia.VerificarConfiguracion();
        }
    }
}
