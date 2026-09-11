using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;

namespace Documentos
{
    public static class GeneradorReciboContrato
    {
        private const string NombreSistema = "SIGAM";

        public static byte[] Generar(BE.ContratoJP86 contrato)
        {
            using (Bitmap bmp = new Bitmap(850, 1100))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.White);
                    Dibujar(g, new RectangleF(40, 40, bmp.Width - 80, bmp.Height - 80), contrato);
                }
                using (MemoryStream ms = new MemoryStream())
                {
                    bmp.Save(ms, ImageFormat.Png);
                    return ms.ToArray();
                }
            }
        }

        public static void Imprimir(BE.ContratoJP86 contrato)
        {
            PrintDocument printDoc = new PrintDocument();
            printDoc.DocumentName = "Recibo de contrato " + contrato.NumeroContrato;
            printDoc.PrintPage += (s, e) => Dibujar(e.Graphics, e.MarginBounds, contrato);

            PrintPreviewDialog preview = new PrintPreviewDialog();
            preview.Document = printDoc;
            preview.WindowState = FormWindowState.Maximized;
            preview.ShowDialog();
        }

        private static void Dibujar(Graphics g, RectangleF bounds, BE.ContratoJP86 contrato)
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
                g.DrawString("Recibo de Contrato de Alquiler", fontSubtitulo, brush, x, y);
                y += fontSubtitulo.GetHeight(g) + 4;
                g.DrawString("Contrato N°: " + contrato.NumeroContrato, fontTexto, brush, x, y);
                y += lineH;
                g.DrawString("Fecha de emisión: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"), fontTexto, brush, x, y);
                y += lineH + 8;

                g.DrawLine(Pens.Black, bounds.Left, y, bounds.Right, y);
                y += 14;

                g.DrawString("Datos del Cliente", fontSeccion, brush, x, y);
                y += lineH;
                g.DrawString("Nombre: " + contrato.Cliente.Nombre, fontTexto, brush, x, y);
                y += lineH;
                g.DrawString("Apellido: " + contrato.Cliente.Apellido, fontTexto, brush, x, y);
                y += lineH;
                g.DrawString("DNI/CUIT: " + contrato.Cliente.DNI, fontTexto, brush, x, y);
                y += lineH;
                g.DrawString("Teléfono: " + contrato.Cliente.Telefono, fontTexto, brush, x, y);
                y += lineH + 10;

                g.DrawString("Datos del Vehículo", fontSeccion, brush, x, y);
                y += lineH;
                g.DrawString("Patente: " + contrato.Vehiculo.Patente, fontTexto, brush, x, y);
                y += lineH;
                g.DrawString("Marca: " + contrato.Vehiculo.Marca, fontTexto, brush, x, y);
                y += lineH;
                g.DrawString("Modelo: " + contrato.Vehiculo.Modelo, fontTexto, brush, x, y);
                y += lineH;
                g.DrawString("Categoría: " + contrato.Vehiculo.Categoria.Nombre, fontTexto, brush, x, y);
                y += lineH;
                g.DrawString("Kilometraje de entrega: " + contrato.KilometrajeEntrega, fontTexto, brush, x, y);
                y += lineH + 10;

                g.DrawString("Datos del Alquiler", fontSeccion, brush, x, y);
                y += lineH;
                g.DrawString("Fecha inicio: " + contrato.FechaInicio.ToString("dd/MM/yyyy"), fontTexto, brush, x, y);
                y += lineH;
                g.DrawString("Fecha fin: " + contrato.FechaFin.ToString("dd/MM/yyyy"), fontTexto, brush, x, y);
                y += lineH;
                g.DrawString("Tarifa diaria: $" + contrato.TarifaDiaria.ToString("N2"), fontTexto, brush, x, y);
                y += lineH;
                g.DrawString("Cargos adicionales: " + (contrato.CargosAdicionales ? "Sí" : "No"), fontTexto, brush, x, y);
                y += lineH;
                g.DrawString("Importe total estimado: $" + contrato.ImporteTotal.ToString("N2"), fontSeccion, brush, x, y);
                y += lineH + 40;

                float firmaY = Math.Max(y, bounds.Bottom - 60);
                g.DrawLine(Pens.Black, bounds.Left, firmaY, bounds.Left + 200, firmaY);
                g.DrawString("Firma Agente Comercial", fontTexto, brush, bounds.Left, firmaY + 4);

                g.DrawLine(Pens.Black, bounds.Right - 200, firmaY, bounds.Right, firmaY);
                g.DrawString("Firma Cliente", fontTexto, brush, bounds.Right - 200, firmaY + 4);
            }
        }
    }
}
