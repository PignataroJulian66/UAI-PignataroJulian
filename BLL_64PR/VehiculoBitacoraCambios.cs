using System;
using System.Collections.Generic;

namespace BLL_64PR
{
    public class VehiculoBitacoraCambios
    {
        Mapper.mpp_vehiculoBitacoraCambios mpp = new Mapper.mpp_vehiculoBitacoraCambios();
        private static readonly DV.DV_64PR recalculador = new DV.DV_64PR();

        public List<BE.VehiculoBitacoraCambios> ObtenerBitacoraCambios(string patente, string marcaModelo, DateTime? fechaIni, DateTime? fechaFin)
        {
            if (fechaIni.HasValue && fechaFin.HasValue && fechaIni.Value.Date > fechaFin.Value.Date)
                throw new ArgumentException("La fecha de inicio no puede ser posterior a la fecha de fin.");

            return mpp.Listar(patente, marcaModelo, fechaIni, fechaFin);
        }

        ///Restaura el vehiculo a los valores de una version historica. El SP hace un UPDATE sobre VehiculoJP86,
        ///asi que se valida igual que un cambio de estado/baja y se recalcula el DV como en el resto de los ABM.
        public void ActivarBitacoraCambios(BE.VehiculoBitacoraCambios version)
        {
            if (version == null || version.IdBitacora <= 0)
                throw new ArgumentException("IdBitacora invalido.");

            new BLL_64PR.VehiculoJP86().ValidarRestauracion(version.Patente, version.Estado, version.Activo);

            mpp.Activar(version.IdBitacora);
            recalculador.RecalcularTabla("VehiculoJP86");
        }
    }
}
