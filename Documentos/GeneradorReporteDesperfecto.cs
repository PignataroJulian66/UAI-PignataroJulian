using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;

namespace Documentos
{
    public static class GeneradorReporteDesperfecto
    {
        private const string NombreSistema = "SIGAM";

        public static byte[] Generar(BE.ReporteJP86 reporte)
        {
            using (Bitmap bmp = new Bitmap(850, 1100))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.White);
                    Dibujar(g, new RectangleF(40, 40, bmp.Width - 80, bmp.Height - 80), reporte);
                }
                using (MemoryStream ms = new MemoryStream())
                {
                    bmp.Save(ms, ImageFormat.Png);
                    return ms.ToArray();
                }
            }
        }

        public static void Imprimir(BE.ReporteJP86 reporte)
        {
            PrintDocument printDoc = new PrintDocument();
            printDoc.DocumentName = "Reporte de desperfecto " + reporte.NumeroReporte;
            printDoc.PrintPage += (s, e) => Dibujar(e.Graphics, e.MarginBounds, reporte);

            PrintPreviewDialog preview = new PrintPreviewDialog();
            preview.Document = printDoc;
            preview.WindowState = FormWindowState.Maximized;
            preview.ShowDialog();
        }

        private static void Dibujar(Graphics g, RectangleF bounds, BE.ReporteJP86 reporte)
        {
            using (Font fontTitulo = new Font("Arial", 16, FontStyle.Bold))
            using (Font fontSubtitulo = new Font("Arial", 12, FontStyle.Bold))
            using (Font fontSeccion = new Font("Arial", 11, FontStyle.Bold))
            using (Font fontTexto = new Font("Arial", 10, FontStyle.Regular))
            {
                Brush brush = Brushes.Black;
                float x = bounds.Left;
                float y = bounds.Top;
                float lineH = fontTexto.GetHeight(g) + 6;

                g.DrawString(NombreSistema, fontTitulo, brush, x, y);
                y += fontTitulo.GetHeight(g) + 6;
                g.DrawString("Reporte de Desperfecto", fontSubtitulo, brush, x, y);
                y += fontSubtitulo.GetHeight(g) + 4;
                g.DrawString("Reporte N°: " + reporte.NumeroReporte, fontTexto, brush, x, y);
                y += lineH;
                g.DrawString("Fecha y hora del reporte: " + reporte.FechaHora.ToString("dd/MM/yyyy HH:mm"), fontTexto, brush, x, y);
                y += lineH + 8;

                g.DrawLine(Pens.Black, bounds.Left, y, bounds.Right, y);
                y += 14;

                g.DrawString("Datos del Vehículo", fontSeccion, brush, x, y);
                y += lineH;
                g.DrawString("Patente: " + reporte.Vehiculo.Patente, fontTexto, brush, x, y);
                y += lineH;
                g.DrawString("Marca: " + reporte.Vehiculo.Marca, fontTexto, brush, x, y);
                y += lineH;
                g.DrawString("Modelo: " + reporte.Vehiculo.Modelo, fontTexto, brush, x, y);
                y += lineH;
                g.DrawString("Categoría: " + reporte.Vehiculo.Categoria.Nombre, fontTexto, brush, x, y);
                y += lineH + 10;

                g.DrawString("Detalle del Desperfecto", fontSeccion, brush, x, y);
                y += lineH;
                g.DrawString("Fuente: " + reporte.Fuente, fontTexto, brush, x, y);
                y += lineH;
                g.DrawString("Descripción del problema:", fontTexto, brush, x, y);
                y += lineH;
                g.DrawString(reporte.DescripcionProblema, fontTexto, brush, x + 10, y, new StringFormat() { });
                y += lineH + 10;

                g.DrawString("Estado: " + reporte.Estado, fontSeccion, brush, x, y);
                y += lineH + 40;

                float firmaY = Math.Max(y, bounds.Bottom - 60);
                g.DrawLine(Pens.Black, bounds.Left, firmaY, bounds.Left + 200, firmaY);
                g.DrawString("Firma Agente Comercial", fontTexto, brush, bounds.Left, firmaY + 4);
            }
        }
    }
}
