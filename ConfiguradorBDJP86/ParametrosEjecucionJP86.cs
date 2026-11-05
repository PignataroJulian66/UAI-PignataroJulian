using System;
using System.Reflection;

namespace ConfiguradorBDJP86
{
    ///Parametros de linea de comandos que envia Inno Setup: /modo=instalacion|reinstalacion|desinstalacion /version=2.0
    public class ParametrosEjecucionJP86
    {
        #region Propiedades
        public ModoEjecucionJP86 Modo { get; private set; }
        public string VersionPrograma { get; private set; }
        #endregion

        #region Operaciones
        public static ParametrosEjecucionJP86 Parsear(string[] args)
        {
            ParametrosEjecucionJP86 p = new ParametrosEjecucionJP86
            {
                Modo = ModoEjecucionJP86.Reinstalacion,
                VersionPrograma = VersionDelEnsamblado()
            };

            foreach (string arg in args ?? new string[0])
            {
                string a = arg.Trim().TrimStart('/', '-');
                int igual = a.IndexOf('=');
                if (igual <= 0) continue;

                string clave = a.Substring(0, igual).Trim().ToLowerInvariant();
                string valor = a.Substring(igual + 1).Trim().Trim('"');

                if (clave == "modo")
                {
                    ModoEjecucionJP86 modo;
                    if (Enum.TryParse(valor, true, out modo)) p.Modo = modo;
                }
                else if (clave == "version" && valor.Length > 0)
                {
                    p.VersionPrograma = valor;
                }
            }
            return p;
        }
        #endregion

        #region Metodos privados
        private static string VersionDelEnsamblado()
        {
            AssemblyInformationalVersionAttribute atributo = (AssemblyInformationalVersionAttribute)Attribute.GetCustomAttribute(
                Assembly.GetExecutingAssembly(), typeof(AssemblyInformationalVersionAttribute));
            return atributo != null ? atributo.InformationalVersion : "2.0";
        }
        #endregion
    }
}
