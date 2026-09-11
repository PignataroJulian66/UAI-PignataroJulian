using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;

namespace Documentos
{
    public static class GeneradorFactura
    {
        private const string NombreSistema = "SIGAM";

        public static byte[] Generar(BE.FacturaJP86 factura)
        {
            using (Bitmap bmp = new Bitmap(850, 1100))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.White);
                    Dibujar(g, new RectangleF(40, 40, bmp.Width - 80, bmp.Height - 80), factura);
                }
                using (MemoryStream ms = new MemoryStream())
                {
                    bmp.Save(ms, ImageFormat.Png);
                    return ms.ToArray();
                }
            }
        }

        public static void Imprimir(BE.FacturaJP86 factura)
        {
            PrintDocument printDoc = new PrintDocument();
            printDoc.DocumentName = "Factura " + factura.NumeroFactura;
            printDoc.PrintPage += (s, e) => Dibujar(e.Graphics, e.MarginBounds, factura);

            PrintPreviewDialog preview = new PrintPreviewDialog();
            preview.Document = printDoc;
            preview.WindowState = FormWindowState.Maximized;
            preview.ShowDialog();
        }

        private static void Dibujar(Graphics g, RectangleF bounds, BE.FacturaJP86 factura)
        {
            BE.ContratoJP86 contrato = factura.Contrato;
            int dias = Math.Max(1, (contrato.FechaFin.Date - contrato.FechaInicio.Date).Days);
            decimal subtotal = contrato.TarifaDiaria * dias;

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

                g.DrawString(NombreSistema, fontTitulo, brush, x, y);
                y += fontTitulo.GetHeight(g) + 6;
                g.DrawString("Factura de Alquiler", fontSubtitulo, brush, x, y);
                y += fontSubtitulo.GetHeight(g) + 4;
                g.DrawString("Factura N°: " + factura.NumeroFactura, fontTexto, brush, x, y);
                y += lineH;
                g.DrawString("Fecha de emisión: " + factura.FechaFactura.ToString("dd/MM/yyyy HH:mm"), fontTexto, brush, x, y);
                y += lineH;
                g.DrawString("Contrato asociado N°: " + contrato.NumeroContrato, fontTexto, brush, x, y);
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

                g.DrawString("Detalle", fontSeccion, brush, x, y);
                y += lineH;
                g.DrawString("Tarifa diaria x días: $" + contrato.TarifaDiaria.ToString("N2") + " x " + dias + " = $" + subtotal.ToString("N2"), fontTexto, brush, x, y);
                y += lineH;
                g.DrawString("Cargos adicionales: " + (contrato.CargosAdicionales ? "Sí" : "No"), fontTexto, brush, x, y);
                y += lineH;
                g.DrawString("Importe total: $" + factura.MontoFactura.ToString("N2"), fontSeccion, brush, x, y);
                y += lineH;
                g.DrawString("Método de pago: " + factura.MetodoPago, fontTexto, brush, x, y);
                y += lineH + 30;

                float pieY = Math.Max(y, bounds.Bottom - 40);
                g.DrawString("Comprobante interno, no válido como factura fiscal. Sin integración con AFIP/facturación electrónica.", fontPie, brush, bounds.Left, pieY);
            }
        }
    }
}
