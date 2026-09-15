using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProyectoIS_64PR.UI;

namespace ProyectoIS_64PR
{
    public partial class FrmMenu : Form, Idioma.IObservadorIdioma_64PR
    {
        public Form formularioactual = null;
        Sesion.BLL_Usuario gusuarios = new Sesion.BLL_Usuario();
        Dictionary<string, string> textos;

        ///Ancho de los botones/paneles del nav, considerando el padding de pnlSidebarNav
        private const int AnchoNav = 220;

        ///Control actualmente resaltado en el sidebar (secc. 2.1, item activo)
        private Button _botonActivo;

        ///Grupos de navegación (acordeón) y sus cabeceras/paneles de items, para poder
        ///re-traducir el texto y controlar visibilidad por permiso sin duplicar lógica
        private FlowLayoutPanel pnlGrupoMaestros, pnlGrupoAlquileres, pnlGrupoMantenimiento, pnlGrupoConfiguracion;
        private Button headerMaestros, headerAlquileres, headerMantenimiento, headerConfiguracion;
        private FlowLayoutPanel itemsMaestros, itemsAlquileres, itemsMantenimiento, itemsConfiguracion, pnlIdiomaItems;

        private Label lblSeccionMaestros, lblSeccionOperacion, lblSeccionSistema;

        private Button btnLogin, btnCambiarContrasena, btnIdioma, btnRespaldoBD, btnRestaurarBD, btnCerrarSesionConfig;
        private Button btnGestionarUsuarios, btnGestionarPermisos, btnGestionarRoles, btnEventos, btnBitacoraCambiosVehiculo;
        private Button btnCategorias, btnVehiculos, btnClientes;
        private Button btnGenerarContrato, btnGenerarFactura, btnRegistrarDevolucion;
        private Button btnNuevoReporte, btnReportesPendientes, btnReparacionesEnCurso;

        public FrmMenu()
        {
            InitializeComponent();

            ///pnlMain (Dock=Fill) no está excluyendo el ancho de pnlSidebar (Dock=Left) del
            ///cálculo de su Bounds -- confirmado por diagnóstico en runtime (Controls.Count=2,
            ///ambos Dock correctos, Parent==this, incluso forzando PerformLayout()) y sigue
            ///devolviendo el ClientSize completo. En vez de seguir confiando en el motor de
            ///Dock para esta relación puntual, lo saco de la ecuación y posiciono pnlMain a mano.
            pnlMain.Dock = DockStyle.None;
            AjustarPanelPrincipal();
            this.Resize += (s, e) => AjustarPanelPrincipal();

            ///Mismo motivo que en el Login: el alto real de lblLogo (AutoSize) puede no
            ///coincidir con el Size estimado en el Designer; reposiciono relativo al Bottom real.
            lblSubtitulo.Location = new Point(lblLogo.Left, lblLogo.Bottom + 4);

            ConstruirNavegacion();
            AgregarSelectorIdioma();
            EstilosUI.AplicarBotonRedondeado(btnCerrarSesionFooter, TemaVisual.AzulOscuro, Color.FromArgb(40, 60, 85), Color.FromArgb(200, 210, 225), 6);

            Idioma.GestorIdioma_64PR.GetInstance.Suscribir(this);

            ///Aplico idioma actual al abrir
            var textosIniciales = Idioma.GestorIdioma_64PR.GetInstance.ObtenerTextos();
            if (textosIniciales.Count > 0)
                ActualizarIdioma(textosIniciales);
        }
        private void AjustarPanelPrincipal()
        {
            pnlMain.SetBounds(pnlSidebar.Width, 0, this.ClientSize.Width - pnlSidebar.Width, this.ClientSize.Height);
        }
        public void ActualizarIdioma(Dictionary<string, string> textoss)
        {
            textos = textoss;
            Traductor_64PR.Traducir(this, textos);
            ActualizarTextosSidebar();
            lblIdiomaPill.Text = Idioma.GestorIdioma_64PR.GetInstance.IdiomaActual.ToUpper();
        }

        #region Construcción del sidebar (reemplaza al MenuStrip)

        private void ConstruirNavegacion()
        {
            ///--- Maestros (acordeón) ---
            lblSeccionMaestros = CrearEtiquetaSeccion("Maestros");
            pnlSidebarNav.Controls.Add(lblSeccionMaestros);

            var (grupoMaestros, hMaestros, iMaestros) = CrearGrupoAcordeon("Maestros", "frmMenu_navMaestros");
            pnlGrupoMaestros = grupoMaestros;
            headerMaestros = hMaestros;
            itemsMaestros = iMaestros;

            btnCategorias = CrearBotonNav("Categorías", true);
            EnlazarNav(btnCategorias, categoriasToolStripMenuItem_Click);
            itemsMaestros.Controls.Add(btnCategorias);

            btnVehiculos = CrearBotonNav("Vehículos", true);
            EnlazarNav(btnVehiculos, vehiculosToolStripMenuItem_Click);
            itemsMaestros.Controls.Add(btnVehiculos);

            btnClientes = CrearBotonNav("Clientes", true);
            EnlazarNav(btnClientes, clientesToolStripMenuItem_Click);
            itemsMaestros.Controls.Add(btnClientes);

            pnlSidebarNav.Controls.Add(grupoMaestros);

            ///--- Operación: Alquileres + Mantenimiento (acordeones) ---
            lblSeccionOperacion = CrearEtiquetaSeccion("Operación");
            pnlSidebarNav.Controls.Add(lblSeccionOperacion);

            var (grupoAlquileres, hAlquileres, iAlquileres) = CrearGrupoAcordeon("Alquileres", "frmMenu_navAlquileres");
            pnlGrupoAlquileres = grupoAlquileres;
            headerAlquileres = hAlquileres;
            itemsAlquileres = iAlquileres;

            btnGenerarContrato = CrearBotonNav("Generar contrato", true);
            EnlazarNav(btnGenerarContrato, generarContratoToolStripMenuItem_Click);
            itemsAlquileres.Controls.Add(btnGenerarContrato);

            btnGenerarFactura = CrearBotonNav("Generar factura", true);
            EnlazarNav(btnGenerarFactura, generarFacturaToolStripMenuItem_Click);
            itemsAlquileres.Controls.Add(btnGenerarFactura);

            btnRegistrarDevolucion = CrearBotonNav("Registrar devolución", true);
            EnlazarNav(btnRegistrarDevolucion, registrarDevolucionToolStripMenuItem_Click);
            itemsAlquileres.Controls.Add(btnRegistrarDevolucion);

            pnlSidebarNav.Controls.Add(grupoAlquileres);

            var (grupoMantenimiento, hMantenimiento, iMantenimiento) = CrearGrupoAcordeon("Mantenimiento", "frmMenu_navMantenimiento");
            pnlGrupoMantenimiento = grupoMantenimiento;
            headerMantenimiento = hMantenimiento;
            itemsMantenimiento = iMantenimiento;

            btnNuevoReporte = CrearBotonNav("Nuevo reporte", true);
            EnlazarNav(btnNuevoReporte, nuevoReporteToolStripMenuItem_Click);
            itemsMantenimiento.Controls.Add(btnNuevoReporte);

            btnReportesPendientes = CrearBotonNav("Reportes pendientes", true);
            EnlazarNav(btnReportesPendientes, reportesPendientesToolStripMenuItem_Click);
            itemsMantenimiento.Controls.Add(btnReportesPendientes);

            btnReparacionesEnCurso = CrearBotonNav("Reparaciones en curso", true);
            EnlazarNav(btnReparacionesEnCurso, reparacionesEnCursoToolStripMenuItem_Click);
            itemsMantenimiento.Controls.Add(btnReparacionesEnCurso);

            pnlSidebarNav.Controls.Add(grupoMantenimiento);

            ///--- Sistema: ítems simples + Configuración (acordeón, al final) ---
            lblSeccionSistema = CrearEtiquetaSeccion("Sistema");
            pnlSidebarNav.Controls.Add(lblSeccionSistema);

            btnGestionarUsuarios = CrearBotonNav("Gestionar usuarios", false);
            EnlazarNav(btnGestionarUsuarios, gestionarUsuariosToolStripMenuItem_Click);
            pnlSidebarNav.Controls.Add(btnGestionarUsuarios);

            btnGestionarPermisos = CrearBotonNav("Gestionar permisos", false);
            EnlazarNav(btnGestionarPermisos, gestionarPermisosToolStripMenuItem_Click);
            pnlSidebarNav.Controls.Add(btnGestionarPermisos);

            btnGestionarRoles = CrearBotonNav("Gestionar roles", false);
            EnlazarNav(btnGestionarRoles, gestionarRolesToolStripMenuItem_Click);
            pnlSidebarNav.Controls.Add(btnGestionarRoles);

            btnEventos = CrearBotonNav("Eventos", false);
            EnlazarNav(btnEventos, eventosToolStripMenuItem_Click);
            pnlSidebarNav.Controls.Add(btnEventos);

            btnBitacoraCambiosVehiculo = CrearBotonNav("Bitácora de Cambios de Vehículo", false);
            EnlazarNav(btnBitacoraCambiosVehiculo, bitacoraCambiosVehiculoToolStripMenuItem_Click);
            pnlSidebarNav.Controls.Add(btnBitacoraCambiosVehiculo);

            var (grupoConfig, hConfig, iConfig) = CrearGrupoAcordeon("Configuración", "frmMenu_navConfiguracion");
            pnlGrupoConfiguracion = grupoConfig;
            headerConfiguracion = hConfig;
            itemsConfiguracion = iConfig;

            btnLogin = CrearBotonNav("Login", true);
            EnlazarNav(btnLogin, loginToolStripMenuItem1_Click);
            itemsConfiguracion.Controls.Add(btnLogin);

            btnCambiarContrasena = CrearBotonNav("Cambiar Contraseña", true);
            EnlazarNav(btnCambiarContrasena, cambiarContraseñaToolStripMenuItem1_Click);
            itemsConfiguracion.Controls.Add(btnCambiarContrasena);

            ///Idioma: acordeón anidado de segundo nivel, poblado en AgregarSelectorIdioma
            btnIdioma = CrearBotonNav("▸  Idioma", true);
            pnlIdiomaItems = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Width = AnchoNav,
                Margin = new Padding(0),
                Visible = false,
                BackColor = TemaVisual.AzulPrimario
            };
            btnIdioma.Click += (s, e) =>
            {
                pnlIdiomaItems.Visible = !pnlIdiomaItems.Visible;
                string etiqueta = (textos != null && textos.ContainsKey("frmMenu_navIdioma")) ? textos["frmMenu_navIdioma"] : "Idioma";
                btnIdioma.Text = (pnlIdiomaItems.Visible ? "▾  " : "▸  ") + etiqueta;
            };
            itemsConfiguracion.Controls.Add(btnIdioma);
            itemsConfiguracion.Controls.Add(pnlIdiomaItems);

            btnRespaldoBD = CrearBotonNav("Respaldo base de datos", true);
            btnRespaldoBD.Click += respaldoBaseDeDatosToolStripMenuItem_Click;
            itemsConfiguracion.Controls.Add(btnRespaldoBD);

            btnRestaurarBD = CrearBotonNav("Restaurar base de datos", true);
            btnRestaurarBD.Click += restaurarBaseDeDatosToolStripMenuItem_Click;
            itemsConfiguracion.Controls.Add(btnRestaurarBD);

            btnCerrarSesionConfig = CrearBotonNav("Cerrar sesión", true);
            btnCerrarSesionConfig.Click += cerrarSesionToolStripMenuItem1_Click;
            itemsConfiguracion.Controls.Add(btnCerrarSesionConfig);

            pnlSidebarNav.Controls.Add(grupoConfig);

            ///--- Footer: cerrar sesión siempre visible ---
            btnCerrarSesionFooter.Click += cerrarSesionToolStripMenuItem1_Click;
        }

        /// <summary>
        /// Etiqueta chica de sección (MAESTROS / OPERACIÓN / SISTEMA), puramente visual.
        /// </summary>
        private Label CrearEtiquetaSeccion(string texto)
        {
            return new Label
            {
                AutoSize = false,
                Width = AnchoNav,
                Height = 24,
                Margin = new Padding(0, 10, 0, 2),
                Padding = new Padding(6, 0, 0, 0),
                Font = new Font("Segoe UI", 7.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(140, 158, 185),
                TextAlign = ContentAlignment.BottomLeft,
                BackColor = TemaVisual.AzulPrimario,
                Text = texto.ToUpper()
            };
        }

        /// <summary>
        /// Crea un grupo de navegación tipo acordeón: cabecera con chevron + panel de items colapsable.
        /// </summary>
        private (FlowLayoutPanel grupo, Button header, FlowLayoutPanel items) CrearGrupoAcordeon(string textoDefault, string claveIdioma)
        {
            var grupo = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Width = AnchoNav,
                Margin = new Padding(0),
                BackColor = TemaVisual.AzulPrimario
            };

            var header = new Button
            {
                Width = AnchoNav,
                Height = 38,
                Margin = new Padding(0),
                FlatStyle = FlatStyle.Flat,
                BackColor = TemaVisual.AzulPrimario,
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(12, 0, 0, 0),
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                Cursor = Cursors.Hand,
                Text = "▸  " + textoDefault,
                UseVisualStyleBackColor = false
            };
            header.FlatAppearance.BorderSize = 0;
            header.FlatAppearance.MouseOverBackColor = TemaVisual.AzulOscuro;

            var items = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Width = AnchoNav,
                Margin = new Padding(0),
                Visible = false,
                BackColor = TemaVisual.AzulPrimario
            };

            header.Click += (s, e) =>
            {
                items.Visible = !items.Visible;
                string etiqueta = (textos != null && textos.ContainsKey(claveIdioma)) ? textos[claveIdioma] : textoDefault;
                header.Text = (items.Visible ? "▾  " : "▸  ") + etiqueta;
            };

            grupo.Controls.Add(header);
            grupo.Controls.Add(items);
            return (grupo, header, items);
        }

        /// <summary>
        /// Crea un botón con la apariencia del sidebar (nivel superior o sub-item indentado).
        /// </summary>
        private Button CrearBotonNav(string texto, bool subitem)
        {
            var btn = new Button
            {
                Width = AnchoNav,
                Height = subitem ? 30 : 36,
                Margin = new Padding(0),
                FlatStyle = FlatStyle.Flat,
                BackColor = TemaVisual.AzulPrimario,
                ForeColor = Color.White,
                Font = subitem ? new Font("Segoe UI", 8.75F) : new Font("Segoe UI", 9F),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(subitem ? 32 : 12, 0, 0, 0),
                Cursor = Cursors.Hand,
                Text = texto,
                UseVisualStyleBackColor = false
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = TemaVisual.AzulOscuro;
            btn.Paint += BotonNav_Paint;
            return btn;
        }

        /// <summary>
        /// Dibuja la barra vertical de acento en el botón activo (secc. 2.1).
        /// </summary>
        private void BotonNav_Paint(object sender, PaintEventArgs e)
        {
            var btn = (Button)sender;
            if (btn == _botonActivo)
            {
                using (var brush = new SolidBrush(TemaVisual.Info))
                    e.Graphics.FillRectangle(brush, 0, 0, 3, btn.Height);
            }
        }

        private void MarcarActivo(Button btn)
        {
            var anterior = _botonActivo;
            _botonActivo = btn;
            anterior?.Invalidate();
            btn.Invalidate();
        }

        private void LimpiarActivo()
        {
            var anterior = _botonActivo;
            _botonActivo = null;
            anterior?.Invalidate();
        }

        /// <summary>
        /// Suscribe un botón de navegación al handler existente (sin tocar su cuerpo), agrega
        /// el resaltado visual del ítem activo y actualiza el breadcrumb de la topbar.
        /// </summary>
        private void EnlazarNav(Button btn, EventHandler handlerOriginal)
        {
            btn.Click += handlerOriginal;
            btn.Click += (s, e) =>
            {
                lblCrumb.Text = btn.Text.Trim();
                if (formularioactual == null) LimpiarActivo();
                else MarcarActivo(btn);
            };
        }

        private void ActualizarTextoGrupo(Button header, FlowLayoutPanel items, string clave, string valorPorDefecto)
        {
            string texto = (textos != null && textos.ContainsKey(clave)) ? textos[clave] : valorPorDefecto;
            header.Text = (items.Visible ? "▾  " : "▸  ") + texto;
        }

        private void ActualizarTextosSidebar()
        {
            if (textos == null || textos.Count == 0) return;

            ActualizarTextoGrupo(headerMaestros, itemsMaestros, "frmMenu_navMaestros", "Maestros");
            ActualizarTextoGrupo(headerAlquileres, itemsAlquileres, "frmMenu_navAlquileres", "Alquileres");
            ActualizarTextoGrupo(headerMantenimiento, itemsMantenimiento, "frmMenu_navMantenimiento", "Mantenimiento");
            ActualizarTextoGrupo(headerConfiguracion, itemsConfiguracion, "frmMenu_navConfiguracion", "Configuración");
            ActualizarTextoGrupo(btnIdioma, pnlIdiomaItems, "frmMenu_navIdioma", "Idioma");

            if (textos.ContainsKey("frmMenu_navMaestros")) lblSeccionMaestros.Text = textos["frmMenu_navMaestros"].ToUpper();
            if (textos.ContainsKey("frmMenu_navOperacion")) lblSeccionOperacion.Text = textos["frmMenu_navOperacion"].ToUpper();
            if (textos.ContainsKey("frmMenu_navSistema")) lblSeccionSistema.Text = textos["frmMenu_navSistema"].ToUpper();

            if (textos.ContainsKey("frmMenu_navLogin")) btnLogin.Text = textos["frmMenu_navLogin"];
            if (textos.ContainsKey("frmMenu_navCambiarContrasena")) btnCambiarContrasena.Text = textos["frmMenu_navCambiarContrasena"];
            if (textos.ContainsKey("frmMenu_navRespaldoBD")) btnRespaldoBD.Text = textos["frmMenu_navRespaldoBD"];
            if (textos.ContainsKey("frmMenu_navRestaurarBD")) btnRestaurarBD.Text = textos["frmMenu_navRestaurarBD"];
            if (textos.ContainsKey("frmMenu_navCerrarSesion"))
            {
                btnCerrarSesionConfig.Text = textos["frmMenu_navCerrarSesion"];
            }

            if (textos.ContainsKey("frmMenu_navGestionarUsuarios")) btnGestionarUsuarios.Text = textos["frmMenu_navGestionarUsuarios"];
            if (textos.ContainsKey("frmMenu_navGestionarPermisos")) btnGestionarPermisos.Text = textos["frmMenu_navGestionarPermisos"];
            if (textos.ContainsKey("frmMenu_navGestionarRoles")) btnGestionarRoles.Text = textos["frmMenu_navGestionarRoles"];
            if (textos.ContainsKey("frmMenu_navEventos")) btnEventos.Text = textos["frmMenu_navEventos"];
            if (textos.ContainsKey("frmMenu_navBitacoraCambiosVehiculo")) btnBitacoraCambiosVehiculo.Text = textos["frmMenu_navBitacoraCambiosVehiculo"];

            if (textos.ContainsKey("frmMenu_navCategorias")) btnCategorias.Text = textos["frmMenu_navCategorias"];
            if (textos.ContainsKey("frmMenu_navVehiculos")) btnVehiculos.Text = textos["frmMenu_navVehiculos"];
            if (textos.ContainsKey("frmMenu_navClientes")) btnClientes.Text = textos["frmMenu_navClientes"];

            if (textos.ContainsKey("frmMenu_navGenerarContrato")) btnGenerarContrato.Text = textos["frmMenu_navGenerarContrato"];
            if (textos.ContainsKey("frmMenu_navGenerarFactura")) btnGenerarFactura.Text = textos["frmMenu_navGenerarFactura"];
            if (textos.ContainsKey("frmMenu_navRegistrarDevolucion")) btnRegistrarDevolucion.Text = textos["frmMenu_navRegistrarDevolucion"];

            if (textos.ContainsKey("frmMenu_navNuevoReporte")) btnNuevoReporte.Text = textos["frmMenu_navNuevoReporte"];
            if (textos.ContainsKey("frmMenu_navReportesPendientes")) btnReportesPendientes.Text = textos["frmMenu_navReportesPendientes"];
            if (textos.ContainsKey("frmMenu_navReparacionesEnCurso")) btnReparacionesEnCurso.Text = textos["frmMenu_navReparacionesEnCurso"];
        }

        #endregion

        #region Bordes sutiles (puramente cosmético, no afecta layout)

        private void pnlTopbar_Paint(object sender, PaintEventArgs e)
        {
            using (var pen = new Pen(TemaVisual.Borde))
                e.Graphics.DrawLine(pen, 0, pnlTopbar.Height - 1, pnlTopbar.Width, pnlTopbar.Height - 1);
        }

        private void pnlSidebarHeader_Paint(object sender, PaintEventArgs e)
        {
            using (var pen = new Pen(Color.FromArgb(20, 255, 255, 255)))
                e.Graphics.DrawLine(pen, 0, pnlSidebarHeader.Height - 1, pnlSidebarHeader.Width, pnlSidebarHeader.Height - 1);
        }

        private void pnlSidebarFooter_Paint(object sender, PaintEventArgs e)
        {
            using (var pen = new Pen(Color.FromArgb(20, 255, 255, 255)))
                e.Graphics.DrawLine(pen, 0, 0, pnlSidebarFooter.Width, 0);
        }

        #endregion

        private void AgregarSelectorIdioma()
        {
            foreach (string codigo in Idioma.GestorIdioma_64PR.GetInstance.IdiomasDisponibles())
            {
                string codigoLocal = codigo; ///Captura para el delegado de abajo
                string etiqueta = codigo.ToUpper(); /// Tipo "ES" / "EN"

                var subBoton = CrearBotonNav(etiqueta, true);
                subBoton.Padding = new Padding(52, 0, 0, 0);
                subBoton.Click += (s, e) =>
                {
                    ///No guardamos en BD aca, ya que se guarda al hacer logout
                    Idioma.GestorIdioma_64PR.GetInstance.SetIdioma(codigoLocal);
                };
                pnlIdiomaItems.Controls.Add(subBoton);
            }
        }
        public void AbrirFormularioHijo(Form f)
        {
            if (formularioactual != null)
            {
                if (formularioactual.GetType() == f.GetType())
                {
                    formularioactual.Close();
                    pnlContenidoMenu.Controls.Clear();
                    formularioactual = null;
                    return;
                }

                formularioactual.Close();
                pnlContenidoMenu.Controls.Clear();
                formularioactual = null;
            }

            f.TopLevel = false;
            f.FormBorderStyle = FormBorderStyle.None;
            f.Dock = DockStyle.Fill;
            pnlContenidoMenu.Controls.Add(f);
            f.Show();
            formularioactual = f;
        }

        private void gestionarUsuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormularioHijo(new FrmGestionarUsuarios_64PR());
        }

        private void eventosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormularioHijo(new FrmBitacora_64PR());
        }

        private void bitacoraCambiosVehiculoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormularioHijo(new FrmBitacoraCambiosVehiculo());
        }

        private void gestionarPermisosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormularioHijo(new FrmGestionFamilias_64PR());
        }

        private void loginToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AbrirFormularioHijo(new FrmLogin_64PR());
        }

        private void cambiarContraseñaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AbrirFormularioHijo(new FrmCambiarClave_64PR());
        }

        private void cerrarSesionToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (formularioactual != null)
            {
                formularioactual.Close();
            }
            var textos = Idioma.GestorIdioma_64PR.GetInstance.ObtenerTextos();
            string msg = textos.ContainsKey("msg_cerrarSesion") ? textos["msg_cerrarSesion"] : "¿Está seguro de que desea cerrar la sesión?";
            string titulo = textos.ContainsKey("msg_cerrarSesion_titulo") ? textos["msg_cerrarSesion_titulo"] : "Cerrar Sesión";

            var resultado = MessageBox.Show(msg, titulo, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                ///Guardamos el idioma en BD antes de cerrar sesión
                string loginActual = Sesion.SessionManager.GetInstance.Usuario.Login;
                string idiomaActual = Idioma.GestorIdioma_64PR.GetInstance.IdiomaActual;
                gusuarios.GuardarIdioma(loginActual, idiomaActual);

                ///Registrasmo ele vento en bitacora

                Bitacora.Bitacora_64PR bita3 = new Bitacora.Bitacora_64PR();
                Bitacora.Evento_64PR ev3 = new Bitacora.Evento_64PR(loginActual, ((int)Bitacora.Bitacora_64PR.ModuloBitacora_64PR.Login).ToString(), ((int)Bitacora.Bitacora_64PR.TipoEventoBitacora_64PR.Logout).ToString(), 5);
                bita3.RegistrarEvento(ev3);

                FrmContenedor_64PR.Instancia.MostrarHijo(new FrmLogin_64PR());
                ///aca lo idea seria usar el metodo .Close(), pero ese metodo me llama al metodo de abajo
                ///que contiene el application.exit y me detiene la ejecucion del programa
                Sesion.SessionManager.GetInstance.Logout();
            }
        }

        private void gestionarRolesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormularioHijo(new FrmGestionarRoles_64PR());
        }

        private void categoriasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormularioHijo(new FrmGestionarCategorias_64PR());
        }

        private void vehiculosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormularioHijo(new FrmGestionarVehiculosJP86());
        }

        private void clientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormularioHijo(new FrmGestionarClientesJP86());
        }

        private void generarContratoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormularioHijo(new FrmGenerarContratoJP86());
        }

        private void generarFacturaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormularioHijo(new FrmGenerarFacturaJP86());
        }

        private void registrarDevolucionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormularioHijo(new FrmRegistrarDevolucionJP86());
        }

        private void nuevoReporteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormularioHijo(new FrmNuevoReporteJP86());
        }

        private void reportesPendientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormularioHijo(new FrmReportesPendientesJP86());
        }

        private void reparacionesEnCursoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormularioHijo(new FrmReparacionesEnCursoJP86());
        }
        private void FrmMenu_FormClosed(object sender, FormClosedEventArgs e)
        {
            Idioma.GestorIdioma_64PR.GetInstance.Desuscribir(this); ///observer del cambio de idioma
        }
        private void FrmMenu_Load(object sender, EventArgs e)
        {
            ConfigurarPermisos();
        }
        private void ConfigurarPermisos()
        {
            Sesion.Rol_64PR rolUsuario = Sesion.SessionManager.GetInstance.Usuario.Rol;

            bool puedeGestionarUsuarios = rolUsuario.TienePermiso(Sesion.Patentes_64PR.CrearUsuario) ||
                                          rolUsuario.TienePermiso(Sesion.Patentes_64PR.ModificarUsuario) ||
                                          rolUsuario.TienePermiso(Sesion.Patentes_64PR.ActivarDesactivarUsuarios) ||
                                          rolUsuario.TienePermiso(Sesion.Patentes_64PR.DesbloquearUsuario);
            btnGestionarUsuarios.Visible = puedeGestionarUsuarios;

            bool puedeGestionarFamilias = rolUsuario.TienePermiso(Sesion.Patentes_64PR.CrearFamilias) ||
                                          rolUsuario.TienePermiso(Sesion.Patentes_64PR.EliminarFamilias) ||
                                          rolUsuario.TienePermiso(Sesion.Patentes_64PR.ModificarFamilias);
            btnGestionarPermisos.Visible = puedeGestionarFamilias;

            bool puedeGestionarRoles = rolUsuario.TienePermiso(Sesion.Patentes_64PR.CrearRoles) ||
                                       rolUsuario.TienePermiso(Sesion.Patentes_64PR.EliminarRoles) ||
                                       rolUsuario.TienePermiso(Sesion.Patentes_64PR.ModificarRoles);
            btnGestionarRoles.Visible = puedeGestionarRoles;

            btnEventos.Visible = rolUsuario.TienePermiso(Sesion.Patentes_64PR.Bitacora);

            btnBitacoraCambiosVehiculo.Visible = rolUsuario.TienePermiso(Sesion.Patentes_64PR.VerBitacoraCambiosVehiculo);

            bool puedeSistema = btnGestionarUsuarios.Visible || btnGestionarPermisos.Visible || btnGestionarRoles.Visible ||
                                 btnEventos.Visible || btnBitacoraCambiosVehiculo.Visible;
            lblSeccionSistema.Visible = puedeSistema;

            btnCambiarContrasena.Visible = rolUsuario.TienePermiso(Sesion.Patentes_64PR.CambiarContra);

            btnIdioma.Visible = rolUsuario.TienePermiso(Sesion.Patentes_64PR.CambiarIdioma);
            if (!btnIdioma.Visible) pnlIdiomaItems.Visible = false;

            btnRespaldoBD.Visible = rolUsuario.TienePermiso(Sesion.Patentes_64PR.Respaldos);

            btnRestaurarBD.Visible = rolUsuario.TienePermiso(Sesion.Patentes_64PR.Restauraciones);

            bool puedeGestionarCategorias = rolUsuario.TienePermiso(Sesion.Patentes_64PR.CrearCategoria) ||
                                             rolUsuario.TienePermiso(Sesion.Patentes_64PR.ModificarCategoria) ||
                                             rolUsuario.TienePermiso(Sesion.Patentes_64PR.EliminarCategoria);
            btnCategorias.Visible = puedeGestionarCategorias;

            bool puedeGestionarVehiculos = rolUsuario.TienePermiso(Sesion.Patentes_64PR.CrearVehiculo) ||
                                            rolUsuario.TienePermiso(Sesion.Patentes_64PR.ModificarVehiculo) ||
                                            rolUsuario.TienePermiso(Sesion.Patentes_64PR.EliminarVehiculo) ||
                                            rolUsuario.TienePermiso(Sesion.Patentes_64PR.CambiarEstadoVehiculo);
            btnVehiculos.Visible = puedeGestionarVehiculos;

            bool puedeGestionarClientes = rolUsuario.TienePermiso(Sesion.Patentes_64PR.CrearCliente) ||
                                           rolUsuario.TienePermiso(Sesion.Patentes_64PR.ModificarCliente) ||
                                           rolUsuario.TienePermiso(Sesion.Patentes_64PR.EliminarCliente);
            btnClientes.Visible = puedeGestionarClientes;

            pnlGrupoMaestros.Visible = puedeGestionarCategorias || puedeGestionarVehiculos || puedeGestionarClientes;
            lblSeccionMaestros.Visible = pnlGrupoMaestros.Visible;

            bool puedeGenerarContrato = rolUsuario.TienePermiso(Sesion.Patentes_64PR.GenerarContrato);
            btnGenerarContrato.Visible = puedeGenerarContrato;

            bool puedeGenerarFactura = rolUsuario.TienePermiso(Sesion.Patentes_64PR.GenerarFactura);
            btnGenerarFactura.Visible = puedeGenerarFactura;

            bool puedeRegistrarDevolucion = rolUsuario.TienePermiso(Sesion.Patentes_64PR.RegistrarDevolucion);
            btnRegistrarDevolucion.Visible = puedeRegistrarDevolucion;

            pnlGrupoAlquileres.Visible = puedeGenerarContrato || puedeGenerarFactura || puedeRegistrarDevolucion;

            bool puedeRegistrarReporte = rolUsuario.TienePermiso(Sesion.Patentes_64PR.RegistrarReporte);
            btnNuevoReporte.Visible = puedeRegistrarReporte;

            bool puedeAsignarCriticidad = rolUsuario.TienePermiso(Sesion.Patentes_64PR.AsignarCriticidad);
            bool puedeDeterminarModalidad = rolUsuario.TienePermiso(Sesion.Patentes_64PR.DeterminarModalidad);
            btnReportesPendientes.Visible = puedeAsignarCriticidad || puedeDeterminarModalidad;

            bool puedeRegistrarAcreditacion = rolUsuario.TienePermiso(Sesion.Patentes_64PR.RegistrarAcreditacion);
            btnReparacionesEnCurso.Visible = puedeRegistrarAcreditacion;

            pnlGrupoMantenimiento.Visible = puedeRegistrarReporte || puedeAsignarCriticidad || puedeDeterminarModalidad || puedeRegistrarAcreditacion;

            lblSeccionOperacion.Visible = pnlGrupoAlquileres.Visible || pnlGrupoMantenimiento.Visible;

            ///Footer: usuario logueado, avatar con iniciales y rol
            string login = Sesion.SessionManager.GetInstance.Usuario?.Login ?? "";
            lblUsuarioActual.Text = login;
            lblRolActual.Text = Sesion.SessionManager.GetInstance.Usuario?.Rol?.Nombre ?? "";
            lblAvatar.Text = ObtenerIniciales(login);
        }

        private string ObtenerIniciales(string login)
        {
            if (string.IsNullOrWhiteSpace(login)) return "?";
            var partes = login.Split(new[] { '.', '_', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (partes.Length >= 2)
                return (partes[0][0].ToString() + partes[1][0].ToString()).ToUpper();
            return login.Length >= 2 ? login.Substring(0, 2).ToUpper() : login.ToUpper();
        }

        private void respaldoBaseDeDatosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Respaldos.Backup gBackup = new Respaldos.Backup();

                string carpetaProyecto = @"C:\Backups_64PR";
                gBackup.CrearCarpetaSiNoExiste(carpetaProyecto); /// SQL la crea si no existe, no rompe si ya existe

                string nombreArchivo = $"BD_64PR_{DateTime.Now:yyyyMMdd_HHmmss}.bak";
                string rutaCompleta = System.IO.Path.Combine(carpetaProyecto, nombreArchivo);

                gBackup.GenerarBackup(rutaCompleta);

                Bitacora.Bitacora_64PR bita = new Bitacora.Bitacora_64PR();
                Bitacora.Evento_64PR ev = new Bitacora.Evento_64PR(Sesion.SessionManager.GetInstance.Usuario.Login, ((int)Bitacora.Bitacora_64PR.ModuloBitacora_64PR.Login).ToString(), ((int)Bitacora.Bitacora_64PR.TipoEventoBitacora_64PR.Backup).ToString(), 3);
                bita.RegistrarEvento(ev);

                MessageBox.Show(textos["backup_Exitoso"] + $":\n{rutaCompleta}",
                                "Backup", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error: " + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void restaurarBaseDeDatosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Archivos de Backup (*.bak)|*.bak";
                ofd.Title = textos["seleccionar_archivo"];

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string rutaBackup = ofd.FileName;

                    var confirmacion = MessageBox.Show(textos["msg_confirmacion"],
                        textos["Confirmar restauración"], MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (confirmacion != DialogResult.Yes) return;

                    try
                    {
                        Respaldos.Backup gBackup = new Respaldos.Backup();

                        /// leer los nombres lógicos del backup
                        DataTable fileList = gBackup.ObtenerFileList(rutaBackup);
                        string logicalData = null, logicalLog = null;

                        foreach (DataRow fila in fileList.Rows)
                        {
                            string tipo = fila["Type"].ToString();
                            if (tipo == "D") logicalData = fila["LogicalName"].ToString();
                            else if (tipo == "L") logicalLog = fila["LogicalName"].ToString();
                        }

                        /// calcular dónde van a ir los archivos físicos
                        string rutaDefaultData = gBackup.ObtenerRutaDefaultData();
                        string rutaDestinoMdf = System.IO.Path.Combine(rutaDefaultData, "BD_64PR.mdf");
                        string rutaDestinoLdf = System.IO.Path.Combine(rutaDefaultData, "BD_64PR_log.ldf");

                        gBackup.RestaurarBackup(rutaBackup, logicalData, logicalLog, rutaDestinoMdf, rutaDestinoLdf);

                        Bitacora.Bitacora_64PR bita = new Bitacora.Bitacora_64PR();
                        Bitacora.Evento_64PR ev = new Bitacora.Evento_64PR(Sesion.SessionManager.GetInstance.Usuario.Login, ((int)Bitacora.Bitacora_64PR.ModuloBitacora_64PR.Login).ToString(), ((int)Bitacora.Bitacora_64PR.TipoEventoBitacora_64PR.Restore).ToString(), 1);
                        bita.RegistrarEvento(ev);

                        MessageBox.Show(textos["msg_restauracion"],
                                        textos["Restauración exitosa"], MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Application.Restart();
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Error: " + ex.Message,
                                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        this.Cursor = Cursors.Default;
                    }
                }
            }
        }
    }
}
