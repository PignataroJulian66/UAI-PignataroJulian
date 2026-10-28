using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace Documentos
{
    ///Mismo formato y encabezado que GeneradorFactura / GeneradorReciboContrato (PrintDocument + PrintPreviewDialog).
    ///A diferencia de ellos, los textos llegan del JSON de idioma (las recomendaciones ya se arman ahi) y el
    ///informe puede ocupar varias paginas: el encabezado se repite en cada una.
    public static class GeneradorReporteMantenimiento
    {
        private const string NombreSistema = "SIGAM";

        public static void Imprimir(BE.ResumenReporteMantenimientoJP86 resumen, List<BE.ItemReporteMantenimientoJP86> items,
            Func<BE.ItemReporteMantenimientoJP86, string> textoRecomendacion, Dictionary<string, string> textos, string filtrosAplicados)
        {
            DateTime fechaEmision = DateTime.Now;
            int siguienteItem = 0;
            int pagina = 0;

            PrintDocument printDoc = new PrintDocument();
            printDoc.DocumentName = Texto(textos, "doc_ReporteMantenimiento_Titulo", "Reporte de Mantenimiento Preventivo");
            ///La vista previa y la impresion real recorren el documento cada una desde el principio
            printDoc.BeginPrint += (s, e) => { siguienteItem = 0; pagina = 0; };
            printDoc.PrintPage += (s, e) =>
            {
                pagina++;
                siguienteItem = DibujarPagina(e, resumen, items, textoRecomendacion, textos, filtrosAplicados, fechaEmision, pagina, siguienteItem);
                e.HasMorePages = siguienteItem < items.Count;
            };

            PrintPreviewDialog preview = new PrintPreviewDialog();
            preview.Document = printDoc;
            preview.WindowState = FormWindowState.Maximized;
            preview.ShowDialog();
        }

        ///Devuelve el indice del primer item que no entro en la pagina
        private static int DibujarPagina(PrintPageEventArgs e, BE.ResumenReporteMantenimientoJP86 resumen, List<BE.ItemReporteMantenimientoJP86> items,
            Func<BE.ItemReporteMantenimientoJP86, string> textoRecomendacion, Dictionary<string, string> textos, string filtrosAplicados,
            DateTime fechaEmision, int pagina, int desde)
        {
            Graphics g = e.Graphics;
            RectangleF bounds = e.MarginBounds;

            using (Font fontTitulo = new Font("Arial", 16, FontStyle.Bold))
            using (Font fontSubtitulo = new Font("Arial", 12, FontStyle.Bold))
            using (Font fontSeccion = new Font("Arial", 11, FontStyle.Bold))
            using (Font fontTexto = new Font("Arial", 10, FontStyle.Regular))
            using (Font fontPie = new Font("Arial", 8, FontStyle.Italic))
            {
                Brush brush = Brushes.Black;
                float x = bounds.Left;
                float y = bounds.Top;
                float lineH = fontTexto.GetHeight(g) + 6;

                ///--- Encabezado (igual que factura y recibo) ---
                g.DrawString(NombreSistema, fontTitulo, brush, x, y);
                y += fontTitulo.GetHeight(g) + 6;
                g.DrawString(Texto(textos, "doc_ReporteMantenimiento_Titulo", "Reporte de Mantenimiento Preventivo"), fontSubtitulo, brush, x, y);
                y += fontSubtitulo.GetHeight(g) + 4;
                g.DrawString(Texto(textos, "doc_ReporteMantenimiento_FechaEmision", "Fecha de emisión:") + " " + fechaEmision.ToString("dd/MM/yyyy HH:mm"), fontTexto, brush, x, y);
                y += lineH;
                g.DrawString(Texto(textos, "doc_ReporteMantenimiento_Filtros", "Filtros aplicados:") + " " + filtrosAplicados, fontTexto, brush, x, y);
                y += lineH + 8;

                g.DrawLine(Pens.Black, bounds.Left, y, bounds.Right, y);
                y += 14;

                ///--- Resumen (solo en la primera pagina) ---
                if (pagina == 1)
                {
                    g.DrawString(Texto(textos, "doc_ReporteMantenimiento_Resumen", "Resumen"), fontSeccion, brush, x, y);
                    y += lineH;
                    foreach (KeyValuePair<BE.SemaforoMantenimientoJP86, int> par in resumen.CantidadPorSemaforo)
                    {
                        g.DrawString(Texto(textos, "semaforo_" + par.Key, par.Key.ToString()) + ": " + par.Value, fontTexto, brush, x + 10, y);
                        y += lineH;
                    }
                    string categoria = resumen.CategoriaMasReportesCliente == null
                        ? Texto(textos, "resumen_SinReportesCliente", "-")
                        : resumen.CategoriaMasReportesCliente.Nombre + " (" + resumen.ReportesClienteCategoria + ")";
                    g.DrawString(Texto(textos, "resumen_CategoriaMasReportesCliente", "Categoría con más reportes de clientes:") + " " + categoria, fontTexto, brush, x + 10, y);
                    y += lineH + 10;

                    g.DrawString(Texto(textos, "doc_ReporteMantenimiento_Detalle", "Detalle por vehículo"), fontSeccion, brush, x, y);
                    y += lineH;
                }

                ///--- Detalle por vehiculo ---
                float pieY = bounds.Bottom - fontPie.GetHeight(g);
                int i = desde;
                while (i < items.Count)
                {
                    BE.ItemReporteMantenimientoJP86 item = items[i];
                    string titulo = item.Vehiculo.Patente + " - " + item.Vehiculo.Marca + " " + item.Vehiculo.Modelo + " (" + item.Categoria.Nombre + ")";
                    string datos = Texto(textos, "doc_ReporteMantenimiento_Semaforo", "Semáforo:") + " " + Texto(textos, "semaforo_" + item.Semaforo, item.Semaforo.ToString())
                        + "   " + Texto(textos, "doc_ReporteMantenimiento_KmActual", "Km actual:") + " " + item.KmActual.ToString("N0")
                        + "   " + Texto(textos, "doc_ReporteMantenimiento_KmRestantes", "Km restantes:") + " " + item.KmRestantes.ToString("N0")
                        + "   " + Texto(textos, "doc_ReporteMantenimiento_FechaEstimada", "Fecha estimada:") + " " + (item.FechaEstimada.HasValue ? item.FechaEstimada.Value.ToString("dd/MM/yyyy") : "-");
                    string recomendacion = textoRecomendacion(item);

                    SizeF tamRecomendacion = g.MeasureString(recomendacion, fontTexto, (int)(bounds.Width - 10));
                    float alto = lineH * 2 + tamRecomendacion.Height + 10;

                    ///Si no entra y la pagina ya tiene algun vehiculo, sigue en la proxima
                    if (y + alto > pieY - 6 && i > desde)
                        break;

                    g.DrawString(titulo, fontSeccion, brush, x, y);
                    y += lineH;
                    g.DrawString(datos, fontTexto, brush, x + 10, y);
                    y += lineH;
                    g.DrawString(recomendacion, fontTexto, brush, new RectangleF(x + 10, y, bounds.Width - 10, tamRecomendacion.Height));
                    y += tamRecomendacion.Height + 10;
                    i++;
                }

                if (items.Count == 0)
                    g.DrawString(Texto(textos, "msg_ReporteMantenimiento_SinResultados", "No hay vehículos para los filtros seleccionados."), fontTexto, brush, x, y);

                g.DrawString(Texto(textos, "doc_ReporteMantenimiento_Pagina", "Página") + " " + pagina, fontPie, brush, x, pieY);

                return i;
            }
        }

        private static string Texto(Dictionary<string, string> textos, string clave, string respaldo)
        {
            return textos != null && textos.ContainsKey(clave) ? textos[clave] : respaldo;
        }
    }
}
