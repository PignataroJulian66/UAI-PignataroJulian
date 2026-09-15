using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ProyectoIS_64PR.UI
{
    /// <summary>
    /// Helpers de estilo visual compartidos (grillas, botones, badges de estado).
    /// Puramente cosmeticos: no alteran datos, eventos de negocio ni el orden de carga existente.
    /// </summary>
    public static class EstilosUI
    {
        public static void AplicarEstiloGrilla(DataGridView dgv)
        {
            dgv.EnableHeadersVisualStyles = false;
            dgv.BackgroundColor = TemaVisual.FondoTarjeta;
            dgv.BorderStyle = BorderStyle.None;
            dgv.GridColor = TemaVisual.Borde;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = TemaVisual.AzulMedio;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgv.ColumnHeadersHeight = 36;
            dgv.DefaultCellStyle.BackColor = TemaVisual.FondoTarjeta;
            dgv.DefaultCellStyle.ForeColor = TemaVisual.TextoPrincipal;
            dgv.DefaultCellStyle.SelectionBackColor = TemaVisual.AzulAcento;
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F);
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 247, 250);
            dgv.RowTemplate.Height = 30;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.RowHeadersVisible = false;
            dgv.BackgroundColor = TemaVisual.FondoTarjeta;
            ///Reparte el ancho disponible entre las columnas en vez de dejarlas con su ancho
            ///de diseño (evita el scroll horizontal / columnas cortadas al agrandar la grilla).
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        /// <summary>
        /// Arma el GraphicsPath de un rectangulo con esquinas redondeadas.
        /// </summary>
        private static GraphicsPath CrearPathRedondeado(Rectangle rect, int radio)
        {
            int d = Math.Max(2, Math.Min(radio * 2, Math.Min(rect.Width, rect.Height)));
            var path = new GraphicsPath();
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        /// <summary>
        /// Redondea un Button dibujandolo el mismo (Paint + AntiAlias), no con Control.Region
        /// (Region recorta pixel a pixel, sin antialiasing, y con radios chicos se ve
        /// "mordido" en vez de curvo). Borra el pintado cuadrado que ya hizo el control base
        /// usando el color de fondo del padre, dibuja el path relleno y redibuja el texto.
        /// Maneja el hover manualmente porque el pintado propio reemplaza el de FlatAppearance.
        /// </summary>
        public static void AplicarBotonRedondeado(Button btn, Color colorNormal, Color colorHover, Color colorTexto, int radio = 6, Color? colorBorde = null)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = colorNormal;
            btn.ForeColor = colorTexto;
            btn.UseVisualStyleBackColor = false;
            btn.Cursor = Cursors.Hand;

            bool hover = false;
            btn.MouseEnter += (s, e) => { hover = true; btn.Invalidate(); };
            btn.MouseLeave += (s, e) => { hover = false; btn.Invalidate(); };

            btn.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                var rectCompleto = new Rectangle(0, 0, btn.Width, btn.Height);
                ///Borra el fondo/texto cuadrado que ya pinto el control base, con el color
                ///de lo que hay detras (el padre), para que las esquinas queden "libres".
                using (var brushFondo = new SolidBrush(btn.Parent?.BackColor ?? TemaVisual.FondoPagina))
                    g.FillRectangle(brushFondo, rectCompleto);

                var rect = new Rectangle(0, 0, btn.Width - 1, btn.Height - 1);
                using (var path = CrearPathRedondeado(rect, radio))
                {
                    using (var brush = new SolidBrush(hover ? colorHover : colorNormal))
                        g.FillPath(brush, path);

                    if (colorBorde.HasValue)
                        using (var pen = new Pen(colorBorde.Value))
                            g.DrawPath(pen, path);
                }

                TextRenderer.DrawText(g, btn.Text, btn.Font, rectCompleto, colorTexto,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            };
        }

        /// <summary>
        /// Pinta una columna de estado como badge segun el valor de texto de la celda.
        /// No modifica el valor subyacente de la celda, solo su presentacion.
        /// </summary>
        public static void AplicarBadgeColumna(DataGridView dgv, string nombreColumna, Func<string, (Color fondo, Color texto)> colorPorValor)
        {
            dgv.CellFormatting += (s, e) =>
            {
                if (e.ColumnIndex < 0 || e.ColumnIndex >= dgv.Columns.Count) return;
                if (dgv.Columns[e.ColumnIndex].Name != nombreColumna) return;
                if (e.Value == null) return;

                var (fondo, texto) = colorPorValor(e.Value.ToString());
                e.CellStyle.BackColor = fondo;
                e.CellStyle.ForeColor = texto;
                e.CellStyle.Font = TemaVisual.FuenteTextoNegrita;
                e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            };
        }

        /// <summary>
        /// Pinta la fila entera cuando el valor de la columna indicada cumple la condicion.
        /// Usado para resaltar eventos/reportes criticos (secc. 8.1).
        /// </summary>
        public static void AplicarFilaCritica(DataGridView dgv, string columnaCriterio, Func<object, bool> esCritica)
        {
            dgv.CellFormatting += (s, e) =>
            {
                if (dgv.Columns[columnaCriterio] == null) return;
                if (e.RowIndex < 0 || e.RowIndex >= dgv.Rows.Count) return;

                var valor = dgv.Rows[e.RowIndex].Cells[columnaCriterio].Value;
                if (valor != null && esCritica(valor))
                {
                    e.CellStyle.BackColor = TemaVisual.Peligro;
                    e.CellStyle.ForeColor = Color.White;
                    e.CellStyle.Font = TemaVisual.FuenteTextoNegrita;
                }
            };
        }

        /// <summary>
        /// Tarjeta blanca con esquinas redondeadas, dibujada por Paint (no Region) para que
        /// el borde quede antialiaseado. Los hijos del panel se pintan despues de este Paint,
        /// asi que no hay riesgo de taparlos con el borrado del fondo.
        /// </summary>
        public static void AplicarTarjeta(Panel panel)
        {
            panel.BackColor = TemaVisual.FondoTarjeta;
            panel.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                using (var brushFondo = new SolidBrush(panel.Parent?.BackColor ?? TemaVisual.FondoPagina))
                    g.FillRectangle(brushFondo, new Rectangle(0, 0, panel.Width, panel.Height));

                var rect = new Rectangle(0, 0, panel.Width - 1, panel.Height - 1);
                using (var path = CrearPathRedondeado(rect, 8))
                {
                    using (var brush = new SolidBrush(TemaVisual.FondoTarjeta))
                        g.FillPath(brush, path);
                    using (var pen = new Pen(TemaVisual.Borde))
                        g.DrawPath(pen, path);
                }
            };
        }

        /// <summary>
        /// Mapeos de color por valor real de cada enum de estado del sistema
        /// (BE.EstadoVehiculoJP86 / EstadoContratoJP86 / EstadoReporteJP86 / EstadoUnidadDevolucionJP86).
        /// </summary>
        public static (Color fondo, Color texto) ColorEstadoVehiculo(string estado)
        {
            switch (estado)
            {
                case "DISPONIBLE": return (TemaVisual.Exito, Color.White);
                case "ALQUILADO": return (TemaVisual.Info, Color.White);
                case "EN_REVISION": return (TemaVisual.Advertencia, Color.White);
                case "EN_REPARACION": return (TemaVisual.Peligro, Color.White);
                default: return (TemaVisual.Borde, TemaVisual.TextoPrincipal);
            }
        }

        public static (Color fondo, Color texto) ColorEstadoContrato(string estado)
        {
            switch (estado)
            {
                case "ACTIVO": return (TemaVisual.Exito, Color.White);
                case "FACTURADO": return (TemaVisual.Info, Color.White);
                case "CERRADO": return (TemaVisual.TextoSecundario, Color.White);
                default: return (TemaVisual.Borde, TemaVisual.TextoPrincipal);
            }
        }

        public static (Color fondo, Color texto) ColorEstadoReporte(string estado)
        {
            switch (estado)
            {
                case "PENDIENTE": return (TemaVisual.Advertencia, Color.White);
                case "CLASIFICADO": return (TemaVisual.Info, Color.White);
                case "EN_REPARACION": return (TemaVisual.Peligro, Color.White);
                case "ACREDITADO": return (TemaVisual.Exito, Color.White);
                default: return (TemaVisual.Borde, TemaVisual.TextoPrincipal);
            }
        }

        public static (Color fondo, Color texto) ColorEstadoUnidadDevolucion(string estado)
        {
            switch (estado)
            {
                case "SIN_NOVEDADES": return (TemaVisual.Exito, Color.White);
                case "CON_OBSERVACIONES": return (TemaVisual.Advertencia, Color.White);
                default: return (TemaVisual.Borde, TemaVisual.TextoPrincipal);
            }
        }

        public static (Color fondo, Color texto) ColorActivo(bool activo)
        {
            return activo ? (TemaVisual.Exito, Color.White) : (TemaVisual.TextoSecundario, Color.White);
        }

        /// <summary>
        /// Estilo de "tarjeta" para los uc* de alta/edición (secc. 5): fondo blanco, labels en
        /// TextoSecundario, tipografía consistente. No reordena ni renombra controles.
        /// </summary>
        public static void AplicarEstiloFormulario(Control contenedor)
        {
            contenedor.BackColor = TemaVisual.FondoTarjeta;
            contenedor.Font = TemaVisual.FuenteTexto;
            foreach (Control c in contenedor.Controls)
            {
                if (c is Label lbl)
                    lbl.ForeColor = TemaVisual.TextoSecundario;
            }
        }

        public static void EstiloBotonPrimario(Button btn)
        {
            btn.Font = TemaVisual.FuenteTextoNegrita;
            AplicarBotonRedondeado(btn, TemaVisual.AzulMedio, TemaVisual.AzulAcento, Color.White, 6);
        }

        public static void EstiloBotonPeligro(Button btn)
        {
            btn.Font = TemaVisual.FuenteTextoNegrita;
            AplicarBotonRedondeado(btn, TemaVisual.Peligro, ControlPaint.Dark(TemaVisual.Peligro, 0.1f), Color.White, 6);
        }

        public static void EstiloBotonSecundario(Button btn)
        {
            btn.Font = TemaVisual.FuenteTextoNegrita;
            AplicarBotonRedondeado(btn, TemaVisual.FondoTarjeta, Color.FromArgb(235, 240, 247), TemaVisual.AzulMedio, 6, TemaVisual.AzulMedio);
        }
    }
}
