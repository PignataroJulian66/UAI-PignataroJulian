using System;
using System.Collections.Generic;

namespace BLL_64PR
{
    public class VehiculoBitacoraCambios
    {
        Mapper.mpp_vehiculoBitacoraCambios mpp = new Mapper.mpp_vehiculoBitacoraCambios();

        public List<BE.VehiculoBitacoraCambios> ObtenerBitacoraCambios(string patente, string marcaModelo, DateTime? fechaIni, DateTime? fechaFin)
        {
            if (fechaIni.HasValue && fechaFin.HasValue && fechaIni.Value.Date > fechaFin.Value.Date)
                throw new ArgumentException("La fecha de inicio no puede ser posterior a la fecha de fin.");

            return mpp.Listar(patente, marcaModelo, fechaIni, fechaFin);
        }

        public void ActivarBitacoraCambios(int idBitacora)
        {
            if (idBitacora <= 0)
                throw new ArgumentException("IdBitacora invalido.");

            mpp.Activar(idBitacora);
        }
    }
}
