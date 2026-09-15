using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ProyectoIS_64PR
{
    public partial class FrmGenerarFacturaJP86 : Form, Idioma.IObservadorIdioma_64PR
    {
        Bitacora.Bitacora_64PR bita = new Bitacora.Bitacora_64PR();
        Bitacora.Evento_64PR ev;

        BLL_64PR.ContratoJP86 gcontratos = new BLL_64PR.ContratoJP86();
        public Dictionary<string, string> textos;

        List<BE.ContratoJP86> lst;
        BE.ContratoJP86 contratoSeleccionado;
        BE.FacturaJP86 facturaGenerada;

        public FrmGenerarFacturaJP86()
        {
            InitializeComponent();

            this.Font = UI.TemaVisual.FuenteTexto;
            this.BackColor = UI.TemaVisual.FondoPagina;
            UI.EstilosUI.AplicarEstiloGrilla(dgvContratos);
            UI.EstilosUI.AplicarBadgeColumna(dgvContratos, "Estado", UI.EstilosUI.ColorEstadoContrato);
            UI.EstilosUI.EstiloBotonSecundario(btnEfectivo);
            UI.EstilosUI.EstiloBotonSecundario(btnTarjeta);
            UI.EstilosUI.EstiloBotonSecundario(btnTransferencia);
            UI.EstilosUI.EstiloBotonSecundario(btnMercadoPago);
            UI.EstilosUI.EstiloBotonSecundario(btnImprimirFactura);

            dgvContratos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            CargaData();

            Idioma.GestorIdioma_64PR.GetInstance.Suscribir(this);

            textos = Idioma.GestorIdioma_64PR.GetInstance.ObtenerTextos();
            if (textos.Count > 0)
                ActualizarIdioma(textos);
        }

        public void ActualizarIdioma(Dictionary<string, string> textoss)
        {
            textos = textoss;
            Traductor_64PR.Traducir(this, textos);

            ///lblImporte muestra contenido dinamico: se recalcula despues de traducir
            lblImporte.Text = textos["factura_lblImporte"] + "$" + (contratoSeleccionado != null ? contratoSeleccionado.ImporteTotal.ToString("N2") : "0");
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            Idioma.GestorIdioma_64PR.GetInstance.Desuscribir(this);
            base.OnFormClosed(e);
        }

        private void FrmGenerarFacturaJP86_Load(object sender, EventArgs e)
        {
            ConfigurarPermisos();
        }

        private void ConfigurarPermisos()
        {
            Sesion.Rol_64PR rolUsuario = Sesion.SessionManager.GetInstance.Usuario.Rol;
            bool puedeFacturar = rolUsuario.TienePermiso(Sesion.Patentes_64PR.GenerarFactura);
            btnEfectivo.Visible = puedeFacturar;
            btnTarjeta.Visible = puedeFacturar;
            btnTransferencia.Visible = puedeFacturar;
            btnMercadoPago.Visible = puedeFacturar;
        }

        private void CargaData()
        {
            lst = gcontratos.ListarActivos();

            dgvContratos.DataSource = null;
            dgvContratos.DataSource = lst;
            ///Rebindear regenera las columnas (autogeneradas) y pisa cualquier traduccion previa: hay que reaplicarla
            Traductor_64PR.TraducirGrilla(this, dgvContratos, textos);

            if (dgvContratos.Columns.Contains("FechaInicio")) dgvContratos.Columns["FechaInicio"].Visible = false;
            if (dgvContratos.Columns.Contains("FechaFin")) dgvContratos.Columns["FechaFin"].Visible = false;
            if (dgvContratos.Columns.Contains("TarifaDiaria")) dgvContratos.Columns["TarifaDiaria"].Visible = false;
            if (dgvContratos.Columns.Contains("CargosAdicionales")) dgvContratos.Columns["CargosAdicionales"].Visible = false;
            if (dgvContratos.Columns.Contains("KilometrajeEntrega")) dgvContratos.Columns["KilometrajeEntrega"].Visible = false;
            if (dgvContratos.Columns.Contains("KilometrajeRetorno")) dgvContratos.Columns["KilometrajeRetorno"].Visible = false;
            if (dgvContratos.Columns.Contains("EstadoUnidadDevolucion")) dgvContratos.Columns["EstadoUnidadDevolucion"].Visible = false;
            if (dgvContratos.Columns.Contains("Estado")) dgvContratos.Columns["Estado"].Visible = false;

            contratoSeleccionado = null;
            HabilitarBotonesPago(false);
            lblImporte.Text = (textos != null ? textos["factura_lblImporte"] : "Importe: ") + "$0";

            facturaGenerada = null;
            btnImprimirFactura.Enabled = false;
        }

        private void dgvContratos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                contratoSeleccionado = dgvContratos.SelectedRows[0].DataBoundItem as BE.ContratoJP86;
                lblImporte.Text = textos["factura_lblImporte"] + "$" + contratoSeleccionado.ImporteTotal.ToString("N2");
                HabilitarBotonesPago(true);
            }
        }

        private void HabilitarBotonesPago(bool habilitar)
        {
            Sesion.Rol_64PR rolUsuario = Sesion.SessionManager.GetInstance.Usuario.Rol;
            bool puedeFacturar = habilitar && rolUsuario.TienePermiso(Sesion.Patentes_64PR.GenerarFactura);
            btnEfectivo.Enabled = puedeFacturar;
            btnTarjeta.Enabled = puedeFacturar;
            btnTransferencia.Enabled = puedeFacturar;
            btnMercadoPago.Enabled = puedeFacturar;
        }

        private void btnEfectivo_Click(object sender, EventArgs e)
        {
            Facturar(BE.MetodoPagoJP86.EFECTIVO);
        }

        private void btnTarjeta_Click(object sender, EventArgs e)
        {
            Facturar(BE.MetodoPagoJP86.TARJETA);
        }

        private void btnTransferencia_Click(object sender, EventArgs e)
        {
            Facturar(BE.MetodoPagoJP86.TRANSFERENCIA);
        }

        private void btnMercadoPago_Click(object sender, EventArgs e)
        {
            Facturar(BE.MetodoPagoJP86.MERCADOPAGO);
        }

        private void Facturar(BE.MetodoPagoJP86 metodoPago)
        {
            if (contratoSeleccionado == null)
            {
                MessageBox.Show(textos["seleccionar_contrato"]);
                return;
            }

            try
            {
                BE.FacturaJP86 factura = gcontratos.GenerarFactura(contratoSeleccionado, metodoPago);

                ev = new Bitacora.Evento_64PR(Sesion.SessionManager.GetInstance.Usuario.Login, ((int)Bitacora.Bitacora_64PR.ModuloBitacora_64PR.GestionAlquileres).ToString(), ((int)Bitacora.Bitacora_64PR.TipoEventoBitacora_64PR.FacturacionContrato).ToString(), 2);
                bita.RegistrarEvento(ev);

                MessageBox.Show(textos["factura_generada"]);

                CargaData();

                facturaGenerada = factura;
                btnImprimirFactura.Enabled = true;
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CargaData();
            }
        }

        private void btnImprimirFactura_Click(object sender, EventArgs e)
        {
            Documentos.GeneradorFactura.Imprimir(facturaGenerada);
        }

        private void FrmGenerarFacturaJP86_FormClosed(object sender, FormClosedEventArgs e)
        {
            Idioma.GestorIdioma_64PR.GetInstance.Desuscribir(this);
        }
    }
}
