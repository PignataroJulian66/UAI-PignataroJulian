using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DV
{
    public class DV_64PR
    {
        public static readonly Dictionary<string, string[]> Tablas = new Dictionary<string, string[]>
        {
            { "Roles_64PR",         new string[] { "ID_Rol" } },
            { "Familia_64PR",       new string[] { "ID_Familia" } },
            { "Familia_N_64PR",     new string[] { "ID_FamiliaPadre", "ID_FamiliaHija" } },
            { "Patente_64PR",       new string[] { "ID_Patente" } },
            { "PatenteFamilia_64PR",new string[] { "ID_Familia", "ID_Patente" } },
            { "RolFamilia_64PR",    new string[] { "ID_Rol", "ID_Familia" } },
            { "RolPatente_64PR",    new string[] { "ID_Rol", "ID_Patente" } },
            { "USUARIO_64PR",       new string[] { "DNI" } },
            { "Categoria_64PR",     new string[] { "ID_Categoria" } },
            { "VehiculoJP86",       new string[] { "Patente" } },
            { "ClienteJP86",        new string[] { "DNI" } }
        };

        DV.mpp_DV mpp = new DV.mpp_DV();

        /// Compara el DV recalculado contra el DV guardado para UNA tabla
        public bool VerificarTabla(string nombreTabla, string[] columnasPK)
        {
            long calculado = mpp.CalcularDVTablaActual(nombreTabla, columnasPK);
            long guardado = mpp.ObtenerDVGuardado(nombreTabla);
            return calculado == guardado;
        }

        /// Recorre TODO el catálogo y devuelve la lista de tablas inconsistentes
        public Dictionary<string, List<DV.FilaInconsistente_64PR>> VerificarIntegridadCompleta()
        {
            Dictionary<string, List<DV.FilaInconsistente_64PR>> resultado = new Dictionary<string, List<DV.FilaInconsistente_64PR>>();

            foreach (var entrada in Tablas)
            {
                string nombreTabla = entrada.Key;
                string[] columnasPK = entrada.Value;

                if (!VerificarTabla(nombreTabla, columnasPK))
                {
                    List<DV.FilaInconsistente_64PR> filasAfectadas = mpp.ObtenerFilasInconsistentes(nombreTabla, columnasPK);
                    resultado[nombreTabla] = filasAfectadas;
                }
            }

            return resultado;
        }
        public void RecalcularIntegridadCompleta()
        {
            foreach (var entrada in Tablas)
            {
                string nombreTabla = entrada.Key;
                string[] columnasPK = entrada.Value;
                mpp.RecalcularTabla(nombreTabla, columnasPK);
            }
        }
        public void RecalcularTabla(string nombreTabla)
        {
            if (!Tablas.ContainsKey(nombreTabla))
                throw new ArgumentException($"Tabla '{nombreTabla}' no registrada en el sistema de DV.");

            string[] columnasPK = Tablas[nombreTabla];
            mpp.RecalcularTabla(nombreTabla, columnasPK);
        }
    }
}
