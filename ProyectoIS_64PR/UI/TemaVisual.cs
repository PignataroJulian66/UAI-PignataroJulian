using System.Drawing;

namespace ProyectoIS_64PR.UI
{
    /// <summary>
    /// Paleta de colores unica de SIGAM. Ningun form/control debe hardcodear
    /// hex sueltos: usar siempre estas constantes.
    /// </summary>
    public static class TemaVisual
    {
        public static readonly Color AzulPrimario = ColorTranslator.FromHtml("#0E3357");
        public static readonly Color AzulOscuro = ColorTranslator.FromHtml("#0A2740");
        public static readonly Color AzulMedio = ColorTranslator.FromHtml("#1F3864");
        public static readonly Color AzulAcento = ColorTranslator.FromHtml("#2E5FA3");
        public static readonly Color FondoPagina = ColorTranslator.FromHtml("#F4F6FA");
        public static readonly Color FondoTarjeta = ColorTranslator.FromHtml("#FFFFFF");
        public static readonly Color TextoPrincipal = ColorTranslator.FromHtml("#1B1F27");
        public static readonly Color TextoSecundario = ColorTranslator.FromHtml("#6B7280");
        public static readonly Color Borde = ColorTranslator.FromHtml("#E2E5EB");
        public static readonly Color Exito = ColorTranslator.FromHtml("#22A06B");
        public static readonly Color Advertencia = ColorTranslator.FromHtml("#E0A93A");
        public static readonly Color Peligro = ColorTranslator.FromHtml("#D64545");
        public static readonly Color Info = ColorTranslator.FromHtml("#2E9BB0");

        public static readonly Font FuenteTitulo = new Font("Segoe UI", 13F, FontStyle.Bold);
        public static readonly Font FuenteSubtitulo = new Font("Segoe UI", 11F, FontStyle.Bold);
        public static readonly Font FuenteTexto = new Font("Segoe UI", 9.5F, FontStyle.Regular);
        public static readonly Font FuenteTextoNegrita = new Font("Segoe UI", 9.5F, FontStyle.Bold);
    }
}
