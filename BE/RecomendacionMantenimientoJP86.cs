namespace BE
{
    ///Codigo de recomendacion del Reporte de Mantenimiento Preventivo: la BLL devuelve el codigo
    ///y la UI arma el texto desde los JSON de idioma (clave "recomendacion_" + codigo)
    public enum RecomendacionMantenimientoJP86
    {
        VENCIDO,
        PROGRAMAR_REVISION,
        PLANIFICAR_REVISION,
        SIN_ACCION,
        SIN_DATOS,
        EN_TALLER
    }
}
