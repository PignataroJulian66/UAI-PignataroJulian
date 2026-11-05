namespace ConfiguradorBDJP86
{
    ///Lo indica Inno Setup con /modo=. Sin parametro (acceso directo "Configurar base de datos") se usa Reinstalacion,
    ///que diagnostica antes de tocar nada.
    public enum ModoEjecucionJP86
    {
        Instalacion,
        Reinstalacion,
        Desinstalacion
    }
}
