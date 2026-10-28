using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace ProyectoIS_64PR
{
    ///Reporte Inteligente de Mantenimiento Preventivo: consulta de solo lectura. Los calculos (semaforo,
    ///recomendacion, filtros, resumen) los hace BLL_64PR.ReporteMantenimientoJP86; aca solo se arman los
    ///textos desde el idioma. "Enviar a revision" reutiliza BLL_64PR.VehiculoJP86.CambiarEstadoManual.
    public partial class FrmReporteMantenimientoJP86 : Form, Idioma.IObservadorIdioma_64PR
    {
        #region Campos
        Bitacora.Bitacora_64PR bita = new Bitacora.Bitacora_64PR();
        Bitacora.Evento_64PR ev;

        BLL_64PR.ReporteMantenimientoJP86 greporte = new BLL_64PR.ReporteMantenimientoJP86();
        BLL_64PR.VehiculoJP86 gvehiculos = new BLL_64PR.VehiculoJP86();
        public Dictionary<string, string> textos;

        List<BE.ItemReporteMantenimientoJP86> itemsReporte = new List<BE.ItemReporteMantenimientoJP86>();
        List<BE.ItemReporteMantenimientoJP86> itemsFiltrados = new List<BE.ItemReporteMantenimientoJP86>();
        BE.ItemReporteMantenimientoJP86 itemSeleccionado;

        ///Mientras se rearman los combos (carga / cambio de idioma) no se refiltra en cada SelectedIndexChanged
        bool cargandoFiltros;

        ///Columnas de la grilla (el resto de las propiedades del DTO se ocultan); Recomendacion es no enlazada
        private static readonly string[] ColumnasVisibles =
        {
            "Vehiculo", "Categoria", "Semaforo", "KmActual", "KmDesdeRevision", "KmRestantes", "Ritmo",
            "DiasEstimados", "FechaEstimada", "ReportesInternos", "ReportesCliente"
        };
        private const string ColumnaRecomendacion = "Recomendacion";
        private static readonly int[] Horizontes = { 7, 15, 30 };
        #endregion

        #region Constructor
        public FrmReporteMantenimientoJP86()
        {
            InitializeComponent();

            this.Font = UI.TemaVisual.FuenteTexto;
            this.BackColor = UI.TemaVisual.FondoPagina;
            UI.EstilosUI.AplicarTarjeta(pnlResumen);
            lblResumenTitulo.Font = UI.TemaVisual.FuenteSubtitulo;
            lblResumenTitulo.ForeColor = UI.TemaVisual.AzulMedio;
            lblResumenSemaforos.Font = UI.TemaVisual.FuenteTextoNegrita;
            lblResumenSemaforos.ForeColor = UI.TemaVisual.TextoPrincipal;
            lblResumenCategoria.ForeColor = UI.TemaVisual.TextoSecundario;
            UI.EstilosUI.AplicarEstiloGrilla(dgvReporte);
            UI.EstilosUI.AplicarFondoFila(dgvReporte, item => item is BE.ItemReporteMantenimientoJP86 i
                ? UI.EstilosUI.ColorFondoSemaforo(i.Semaforo.ToString())
                : (System.Drawing.Color?)null);
            UI.EstilosUI.EstiloBotonPrimario(btnGenerar);
            UI.EstilosUI.EstiloBotonSecundario(btnImprimir);
            UI.EstilosUI.EstiloBotonPrimario(btnEnviarRevision);

            Idioma.GestorIdioma_64PR.GetInstance.Suscribir(this);

            textos = Idioma.GestorIdioma_64PR.GetInstance.ObtenerTextos();
            if (textos.Count > 0)
                ActualizarIdioma(textos);
        }
        #endregion

        #region Idioma (Observer)
        public void ActualizarIdioma(Dictionary<string, string> textoss)
        {
            textos = textoss;
            Traductor_64PR.Traducir(this, textos);

            ///Traducir() solo recorre los controles de primer nivel: el panel de resumen y los combos se arman aca
            lblResumenTitulo.Text = Texto("FrmReporteMantenimientoJP86.lblResumenTitulo");
            CargarFiltros();
            MostrarResumen();
            ///Semaforo y Recomendacion se formatean con el idioma en CellFormatting: repintar
            dgvReporte.Invalidate();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            Idioma.GestorIdioma_64PR.GetInstance.Desuscribir(this);
            base.OnFormClosed(e);
        }
        #endregion

        #region Permisos
        private bool ConfigurarPermisos()
        {
            Sesion.Rol_64PR rolUsuario = Sesion.SessionManager.GetInstance.Usuario.Rol;
            bool puedeVerReporte = rolUsuario.TienePermiso(Sesion.Patentes_64PR.ReporteMantenimientoPreventivo);
            btnGenerar.Enabled = puedeVerReporte;
            btnImprimir.Enabled = puedeVerReporte;

            ///"Enviar a revision" exige ademas la patente del cambio de estado manual del maestro de vehiculos
            btnEnviarRevision.Visible = puedeVerReporte && rolUsuario.TienePermiso(Sesion.Patentes_64PR.CambiarEstadoVehiculo);
            btnEnviarRevision.Enabled = false;
            return puedeVerReporte;
        }
        #endregion

        #region Carga de datos
        private void GenerarReporte(bool registrarEvento = true)
        {
            try
            {
                itemsReporte = greporte.Generar();

                ///El refresco posterior a "Enviar a revision" no es una nueva generacion pedida por el usuario
                if (registrarEvento)
                {
                    ev = new Bitacora.Evento_64PR(Sesion.SessionManager.GetInstance.Usuario.Login, ((int)Bitacora.Bitacora_64PR.ModuloBitacora_64PR.GestionMantenimiento).ToString(), ((int)Bitacora.Bitacora_64PR.TipoEventoBitacora_64PR.GenerarReporteMantenimiento).ToString(), 4);
                    bita.RegistrarEvento(ev);
                }
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(Traductor_64PR.TraducirMensaje(textos, ex.Message), textos["titulo_Validacion"], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                itemsReporte = new List<BE.ItemReporteMantenimientoJP86>();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                itemsReporte = new List<BE.ItemReporteMantenimientoJP86>();
            }

            CargarFiltros();
            AplicarFiltros();
            MostrarResumen();
        }

        private void AplicarFiltros()
        {
            itemsFiltrados = greporte.Filtrar(itemsReporte, ValorSeleccionado<int?>(cmbCategoria), ValorSeleccionado<BE.SemaforoMantenimientoJP86?>(cmbSemaforo), ValorSeleccionado<int?>(cmbHorizonte));

            dgvReporte.DataSource = null;
            dgvReporte.DataSource = itemsFiltrados;
            ConfigurarColumnas();
            ///Rebindear regenera las columnas (autogeneradas) y pisa cualquier traduccion previa: hay que reaplicarla
            Traductor_64PR.TraducirGrilla(this, dgvReporte, textos);

            LimpiarSeleccion();
        }

        private void ConfigurarColumnas()
        {
            if (!dgvReporte.Columns.Contains(ColumnaRecomendacion))
                dgvReporte.Columns.Add(new DataGridViewTextBoxColumn() { Name = ColumnaRecomendacion, FillWeight = 260 });

            ///Se quitan (no se ocultan) las demas propiedades del DTO, asi TraducirGrilla no las reporta como claves faltantes
            List<DataGridViewColumn> sobrantes = dgvReporte.Columns.Cast<DataGridViewColumn>()
                .Where(c => c.Name != ColumnaRecomendacion && !ColumnasVisibles.Contains(c.Name))
                .ToList();
            foreach (DataGridViewColumn columna in sobrantes)
                dgvReporte.Columns.Remove(columna);

            for (int i = 0; i < ColumnasVisibles.Length; i++)
                if (dgvReporte.Columns.Contains(ColumnasVisibles[i]))
                    dgvReporte.Columns[ColumnasVisibles[i]].DisplayIndex = i;
            dgvReporte.Columns[ColumnaRecomendacion].DisplayIndex = dgvReporte.Columns.Count - 1;

            foreach (string km in new[] { "KmActual", "KmDesdeRevision", "KmRestantes" })
                if (dgvReporte.Columns.Contains(km))
                    dgvReporte.Columns[km].DefaultCellStyle.Format = "N0";
            if (dgvReporte.Columns.Contains("Ritmo"))
                dgvReporte.Columns["Ritmo"].DefaultCellStyle.Format = "N1";
            if (dgvReporte.Columns.Contains("FechaEstimada"))
                dgvReporte.Columns["FechaEstimada"].DefaultCellStyle.Format = "dd/MM/yyyy";
        }

        private void MostrarResumen()
        {
            if (textos == null || textos.Count == 0) return;

            BE.ResumenReporteMantenimientoJP86 resumen = greporte.ObtenerResumen(itemsReporte);

            lblResumenSemaforos.Text = string.Join("   ·   ", resumen.CantidadPorSemaforo
                .Select(par => TextoSemaforo(par.Key) + ": " + par.Value));

            string categoria = resumen.CategoriaMasReportesCliente == null
                ? Texto("resumen_SinReportesCliente")
                : resumen.CategoriaMasReportesCliente.Nombre + " (" + resumen.ReportesClienteCategoria + ")";
            lblResumenCategoria.Text = Texto("resumen_CategoriaMasReportesCliente") + " " + categoria;
        }

        ///Combos con opcion "Todos" (valor null). Conserva la seleccion al cambiar de idioma.
        private void CargarFiltros()
        {
            cargandoFiltros = true;

            int? categoriaActual = ValorSeleccionado<int?>(cmbCategoria);
            BE.SemaforoMantenimientoJP86? semaforoActual = ValorSeleccionado<BE.SemaforoMantenimientoJP86?>(cmbSemaforo);
            int? horizonteActual = ValorSeleccionado<int?>(cmbHorizonte);

            List<OpcionFiltro> categorias = new List<OpcionFiltro>() { new OpcionFiltro(null, Texto("filtro_Todos")) };
            categorias.AddRange(itemsReporte
                .GroupBy(i => i.Categoria.Id)
                .Select(g => new OpcionFiltro((int?)g.Key, g.First().Categoria.Nombre))
                .OrderBy(o => o.Texto));
            LlenarCombo(cmbCategoria, categorias, categoriaActual);

            List<OpcionFiltro> semaforos = new List<OpcionFiltro>() { new OpcionFiltro(null, Texto("filtro_Todos")) };
            foreach (BE.SemaforoMantenimientoJP86 s in Enum.GetValues(typeof(BE.SemaforoMantenimientoJP86)))
                semaforos.Add(new OpcionFiltro((BE.SemaforoMantenimientoJP86?)s, TextoSemaforo(s)));
            LlenarCombo(cmbSemaforo, semaforos, semaforoActual);

            List<OpcionFiltro> horizontes = new List<OpcionFiltro>() { new OpcionFiltro(null, Texto("filtro_Todos")) };
            foreach (int dias in Horizontes)
                horizontes.Add(new OpcionFiltro((int?)dias, Texto("filtro_HorizonteDias").Replace("{dias}", dias.ToString())));
            LlenarCombo(cmbHorizonte, horizontes, horizonteActual);

            cargandoFiltros = false;
        }
        #endregion

        #region Eventos
        private void FrmReporteMantenimientoJP86_Load(object sender, EventArgs e)
        {
            if (ConfigurarPermisos())
                GenerarReporte();
        }

        private void btnGenerar_Click(object sender, EventArgs e)
        {
            GenerarReporte();
        }

        private void cmbFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cargandoFiltros) return;
            AplicarFiltros();
        }

        private void dgvReporte_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                itemSeleccionado = dgvReporte.SelectedRows[0].DataBoundItem as BE.ItemReporteMantenimientoJP86;
                ///EN_TALLER ya esta en revision/reparacion: no se puede volver a enviar
                btnEnviarRevision.Enabled = itemSeleccionado != null && itemSeleccionado.Semaforo != BE.SemaforoMantenimientoJP86.EN_TALLER;
            }
        }

        private void dgvReporte_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            BE.ItemReporteMantenimientoJP86 item = dgvReporte.Rows[e.RowIndex].DataBoundItem as BE.ItemReporteMantenimientoJP86;
            if (item == null) return;

            string columna = dgvReporte.Columns[e.ColumnIndex].Name;
            if (columna == ColumnaRecomendacion)
            {
                e.Value = TextoRecomendacion(item);
                e.FormattingApplied = true;
            }
            else if (columna == "Semaforo")
            {
                e.Value = TextoSemaforo(item.Semaforo);
                e.FormattingApplied = true;
            }
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            ///El informe respeta los filtros: resumen y detalle se calculan sobre lo que muestra la grilla
            BE.ResumenReporteMantenimientoJP86 resumen = greporte.ObtenerResumen(itemsFiltrados);
            string filtros = lblCategoria.Text + " " + cmbCategoria.Text + "   " + lblSemaforo.Text + " " + cmbSemaforo.Text + "   " + lblHorizonte.Text + " " + cmbHorizonte.Text;

            Documentos.GeneradorReporteMantenimiento.Imprimir(resumen, itemsFiltrados, TextoRecomendacion, textos, filtros);

            ev = new Bitacora.Evento_64PR(Sesion.SessionManager.GetInstance.Usuario.Login, ((int)Bitacora.Bitacora_64PR.ModuloBitacora_64PR.GestionMantenimiento).ToString(), ((int)Bitacora.Bitacora_64PR.TipoEventoBitacora_64PR.ImprimirReporteMantenimiento).ToString(), 4);
            bita.RegistrarEvento(ev);
        }

        private void btnEnviarRevision_Click(object sender, EventArgs e)
        {
            if (itemSeleccionado == null)
            {
                MessageBox.Show(textos["seleccionar_vehiculo"]);
                return;
            }

            try
            {
                gvehiculos.CambiarEstadoManual(itemSeleccionado.Vehiculo.Patente, BE.EstadoVehiculoJP86.EN_REVISION);
            }
            catch (InvalidOperationException ex)
            {
                ///Mismas validaciones y mensajes que el cambio de estado manual del maestro de vehiculos
                MessageBox.Show(Traductor_64PR.TraducirMensaje(textos, ex.Message), textos["titulo_Validacion"], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ev = new Bitacora.Evento_64PR(Sesion.SessionManager.GetInstance.Usuario.Login, ((int)Bitacora.Bitacora_64PR.ModuloBitacora_64PR.GestionMantenimiento).ToString(), ((int)Bitacora.Bitacora_64PR.TipoEventoBitacora_64PR.CambioEstadoVehiculo).ToString(), 3);
            bita.RegistrarEvento(ev);

            MessageBox.Show(textos["estado_cambiado"]);
            GenerarReporte(false);
        }
        #endregion

        #region Metodos privados
        ///Arma el texto desde el JSON con los parametros que devuelve la BLL
        private string TextoRecomendacion(BE.ItemReporteMantenimientoJP86 item)
        {
            string texto = Texto("recomendacion_" + item.Recomendacion)
                .Replace("{km}", item.KmExcedidos.ToString("N0"))
                .Replace("{dias}", item.DiasEstimados.HasValue ? item.DiasEstimados.Value.ToString() : "-")
                .Replace("{fecha}", item.FechaEstimada.HasValue ? item.FechaEstimada.Value.ToString("dd/MM/yyyy") : "-");

            if (item.RevisarInspeccionInterna)
                texto += " " + Texto("recomendacion_RevisarInspeccionInterna");

            return texto;
        }

        private string TextoSemaforo(BE.SemaforoMantenimientoJP86 semaforo)
        {
            return Texto("semaforo_" + semaforo);
        }

        private string Texto(string clave)
        {
            return textos != null && textos.ContainsKey(clave) ? textos[clave] : clave;
        }

        private T ValorSeleccionado<T>(ComboBox cmb)
        {
            OpcionFiltro opcion = cmb.SelectedItem as OpcionFiltro;
            return opcion == null || opcion.Valor == null ? default(T) : (T)opcion.Valor;
        }

        private void LlenarCombo(ComboBox cmb, List<OpcionFiltro> opciones, object valorActual)
        {
            cmb.DataSource = opciones;
            OpcionFiltro seleccion = opciones.FirstOrDefault(o => Equals(o.Valor, valorActual)) ?? opciones[0];
            cmb.SelectedItem = seleccion;
        }

        private void LimpiarSeleccion()
        {
            itemSeleccionado = null;
            dgvReporte.ClearSelection();
            btnEnviarRevision.Enabled = false;
        }

        ///Opcion de un combo de filtro: Valor null = "Todos"
        private class OpcionFiltro
        {
            public object Valor { get; }
            public string Texto { get; }

            public OpcionFiltro(object valor, string texto)
            {
                Valor = valor;
                Texto = texto;
            }

            public override string ToString()
            {
                return Texto;
            }
        }
        #endregion
    }
}
