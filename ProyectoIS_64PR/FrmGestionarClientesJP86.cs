using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoIS_64PR
{
    public partial class FrmGestionarClientesJP86 : Form, Idioma.IObservadorIdioma_64PR
    {
        Bitacora.Bitacora_64PR bita = new Bitacora.Bitacora_64PR();
        Bitacora.Evento_64PR ev;

        BLL_64PR.ClienteJP86 gclientes = new BLL_64PR.ClienteJP86();
        public Dictionary<string, string> textos;

        string modo = "consulta";
        UserControl uc;
        List<BE.ClienteJP86> lst;

        ///Ultimo mensaje del panel de mensajes: se guarda la clave (y sus argumentos) para re-traducirlo si cambia el idioma
        string claveMensaje;
        object[] argsMensaje;
        Color colorMensaje;
        Color colorModoNormal;

        public FrmGestionarClientesJP86()
        {
            InitializeComponent();

            this.Font = UI.TemaVisual.FuenteTexto;
            this.BackColor = UI.TemaVisual.FondoPagina;
            UI.EstilosUI.AplicarTarjeta(pnlContenedor);
            UI.EstilosUI.EstiloBotonPrimario(btnCrear);
            UI.EstilosUI.EstiloBotonPrimario(btnGuardar);
            UI.EstilosUI.EstiloBotonSecundario(btnModificar);
            UI.EstilosUI.EstiloBotonPeligro(btnEliminar);
            UI.EstilosUI.EstiloBotonSecundario(btnActualizar);
            UI.EstilosUI.EstiloBotonSecundario(btnLimpiar);
            UI.EstilosUI.EstiloBotonPrimario(btnSerializar);
            UI.EstilosUI.EstiloBotonPrimario(btnDeserializar);
            UI.EstilosUI.EstiloBotonSecundario(btnCarpetaSerializar);
            UI.EstilosUI.EstiloBotonSecundario(btnCarpetaDeserializar);
            ///Icono de carpeta: glifo de la fuente de iconos de Windows 10/11 (el pintado redondeado usa btn.Font)
            btnCarpetaSerializar.Font = new Font("Segoe MDL2 Assets", 10F);
            btnCarpetaSerializar.Text = "\uED25";
            btnCarpetaDeserializar.Font = new Font("Segoe MDL2 Assets", 10F);
            btnCarpetaDeserializar.Text = "\uED25";
            pnlSeparador.BackColor = UI.TemaVisual.Borde;
            pnlSerializacion.BackColor = UI.TemaVisual.FondoPagina;
            txtRutaSerializar.BackColor = UI.TemaVisual.FondoTarjeta;
            txtRutaDeserializar.BackColor = UI.TemaVisual.FondoTarjeta;
            lblMensajes.BackColor = UI.TemaVisual.FondoTarjeta;
            lblMensajes.Font = UI.TemaVisual.FuenteTexto;
            colorMensaje = UI.TemaVisual.TextoPrincipal;
            colorModoNormal = lblModo.ForeColor;
            pnlContenedor.Controls.Add(lblMensajes);

            radioButton3.Checked = true;
            dgvClientes.ReadOnly = true;
            dgvClientes.MultiSelect = false;
            dgvClientes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvClientes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvClientes.BackgroundColor = SystemColors.Menu;
            dgvClientes.BorderStyle = BorderStyle.None;
            UI.EstilosUI.AplicarEstiloGrilla(dgvClientes);
            CargaData();

            btnGuardar.Enabled = false;

            Idioma.GestorIdioma_64PR.GetInstance.Suscribir(this);

            textos = Idioma.GestorIdioma_64PR.GetInstance.ObtenerTextos();
            if (textos.Count > 0)
                ActualizarIdioma(textos);
            lblModo.Text = textos["frmGestionClientes_lblModoConsulta"];
        }

        public void ActualizarIdioma(Dictionary<string, string> textoss)
        {
            textos = textoss;
            string[] aux = lblCantidad.Text.Split(':');

            Traductor_64PR.Traducir(this, textos);
            if (aux.Length > 1)
                lblCantidad.Text += aux[1];

            switch (modo)
            {
                case "consulta":
                    lblModo.Text = textos["frmGestionClientes_lblModoConsulta"];
                    break;
                case "crear":
                    lblModo.Text = textos["frmGestionClientes_lblModoCrear"];
                    break;
                case "modificar":
                    lblModo.Text = textos["frmGestionClientes_lblModoModificar"];
                    break;
                case "xml":
                    lblModo.Text = Texto("frmGestionClientes_lblModoXml");
                    break;
            }

            ///La franja de serializacion esta dentro de pnlSerializacion (Traducir solo recorre los controles directos del form,
            ///y le pisaria el glifo a los botones de carpeta): se traduce aca
            btnSerializar.Text = Texto("FrmGestionarClientesJP86.btnSerializar");
            btnDeserializar.Text = Texto("FrmGestionarClientesJP86.btnDeserializar");

            ttAyuda.SetToolTip(btnActualizar, Texto("tt_Clientes_Actualizar"));
            ttAyuda.SetToolTip(btnLimpiar, Texto("tt_Clientes_Limpiar"));
            ttAyuda.SetToolTip(btnSerializar, Texto("tt_Clientes_Serializar"));
            ttAyuda.SetToolTip(btnDeserializar, Texto("tt_Clientes_Deserializar"));
            ttAyuda.SetToolTip(btnCarpetaSerializar, Texto("tt_Clientes_CarpetaSerializar"));
            ttAyuda.SetToolTip(btnCarpetaDeserializar, Texto("tt_Clientes_CarpetaDeserializar"));

            MostrarMensajeActual();
        }

        private string Texto(string clave)
        {
            return Traductor_64PR.TraducirMensaje(textos, clave);
        }

        #region Panel de mensajes

        ///Los argumentos que sean claves de idioma (ej. el Message de una excepcion de la BLL) tambien se traducen
        private void MostrarMensaje(string clave, Color color, params object[] args)
        {
            claveMensaje = clave;
            argsMensaje = args;
            colorMensaje = color;
            MostrarMensajeActual();
        }

        private void MostrarMensajeActual()
        {
            if (string.IsNullOrEmpty(claveMensaje))
            {
                lblMensajes.Text = string.Empty;
                return;
            }

            object[] traducidos = (argsMensaje ?? new object[0])
                .Select(a => a is string s ? (object)Texto(s) : a)
                .ToArray();
            lblMensajes.ForeColor = colorMensaje;
            lblMensajes.Text = string.Format(Texto(claveMensaje), traducidos);
        }

        private void LimpiarMensaje()
        {
            claveMensaje = null;
            argsMensaje = null;
            lblMensajes.Text = string.Empty;
        }

        ///pnlContenedor tambien aloja los uc de Crear/Modificar: al volver a modo consulta se vuelve a mostrar el panel de mensajes
        private void MostrarPanelMensajes()
        {
            LimpiarMensaje();
            if (!pnlContenedor.Controls.Contains(lblMensajes))
                pnlContenedor.Controls.Add(lblMensajes);
        }

        #endregion

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            Idioma.GestorIdioma_64PR.GetInstance.Desuscribir(this);
            base.OnFormClosed(e);
        }

        private void CargaData()
        {
            lst = gclientes.Listar();
            AplicarFiltros();
        }

        private void AplicarFiltros()
        {
            if (lst == null) return;

            IEnumerable<BE.ClienteJP86> filtrado = lst;

            ///radioButton1 = "No activos", radioButton2 = "Activos"
            if (radioButton1.Checked)
                filtrado = filtrado.Where(c => c.Activo == false);
            else if (radioButton2.Checked)
                filtrado = filtrado.Where(c => c.Activo == true);

            if (!string.IsNullOrWhiteSpace(txtBuscar.Text))
            {
                string texto = txtBuscar.Text.Trim();
                filtrado = filtrado.Where(c =>
                    c.DNI.IndexOf(texto, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    c.Nombre.IndexOf(texto, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    c.Apellido.IndexOf(texto, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            List<BE.ClienteJP86> resultado = filtrado.ToList();

            dgvClientes.DataSource = null;
            dgvClientes.DataSource = resultado;
            ///Rebindear regenera las columnas (autogeneradas) y pisa cualquier traduccion previa: hay que reaplicarla
            Traductor_64PR.TraducirGrilla(this, dgvClientes, textos);

            if (textos != null && textos.Count > 0)
                lblCantidad.Text = textos["frmGestionClientes_lblCantidad"] + resultado.Count.ToString();
        }

        private void btnCrear_Click(object sender, EventArgs e)
        {
            modo = "crear";
            lblModo.Text = textos["frmGestionClientes_lblModoCrear"];
            uc = new ucCrearClienteJP86();
            pnlContenedor.Controls.Clear();
            uc.Dock = DockStyle.Fill;
            pnlContenedor.Controls.Add(uc);
            btnGuardar.Enabled = true;
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            modo = "modificar";
            lblModo.Text = textos["frmGestionClientes_lblModoModificar"];
            uc = new ucModificarClienteJP86();
            pnlContenedor.Controls.Clear();
            uc.Dock = DockStyle.Fill;
            pnlContenedor.Controls.Add(uc);
            btnGuardar.Enabled = true;

            ///Si ya hay una fila seleccionada, se cargan sus datos: el control nunca queda vacio o con datos de otro cliente
            if (dgvClientes.SelectedRows.Count > 0 && dgvClientes.SelectedRows[0].DataBoundItem is BE.ClienteJP86 seleccionado)
                ((ucModificarClienteJP86)uc).EscribirControles(seleccionado);
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            switch (modo)
            {
                case "consulta":
                    MessageBox.Show(textos["msg_NoCambios"]);
                    break;

                case "crear":
                    if (uc is ucCrearClienteJP86 ucc)
                    {
                        ///Validaciones de formato: BLL_64PR.ClienteJP86.Crear (mismo codigo que CUN-02)
                        try
                        {
                            BE.ClienteJP86 c = new BE.ClienteJP86()
                            {
                                DNI = ucc.DNI(),
                                Nombre = ucc.Nombre(),
                                Apellido = ucc.Apellido(),
                                Telefono = ucc.Telefono()
                            };

                            gclientes.Crear(c);

                            ev = new Bitacora.Evento_64PR(Sesion.SessionManager.GetInstance.Usuario.Login, ((int)Bitacora.Bitacora_64PR.ModuloBitacora_64PR.GestionClientes).ToString(), ((int)Bitacora.Bitacora_64PR.TipoEventoBitacora_64PR.AltaCliente).ToString(), 4);
                            bita.RegistrarEvento(ev);

                            MessageBox.Show(textos["cliente_creado"]);

                            CargaData();
                            ucc.LimpiarCampos();
                            pnlContenedor.Controls.Clear();
                            uc = null;
                            MostrarPanelMensajes();
                            modo = "consulta";
                            lblModo.Text = textos["frmGestionClientes_lblModoConsulta"];
                            btnGuardar.Enabled = false;
                        }
                        catch (InvalidOperationException ex)
                        {
                            MessageBox.Show(Traductor_64PR.TraducirMensaje(textos, ex.Message), textos["titulo_Validacion"], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        catch (SqlException ex)
                        {
                            if (ex.Number == 2627 || ex.Number == 2601)
                            {
                                MessageBox.Show(textos["msg_DNIDuplicado"],
                                    textos["titulo_DNIDuplicado"], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                            {
                                MessageBox.Show(textos["msg_ErrorBaseDatos"] + ex.Message, textos["titulo_Error"], MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    break;

                case "modificar":
                    if (uc is ucModificarClienteJP86 ucm)
                    {
                        ///Validaciones de formato: BLL_64PR.ClienteJP86.Modificar
                        ///Se modifica el cliente cuyos datos estan en el control, no la fila seleccionada en ese momento
                        BE.ClienteJP86 seleccionado = ucm.ClienteCargado;
                        if (seleccionado == null)
                        {
                            MessageBox.Show(textos["seleccionar_cliente"]);
                            return;
                        }
                        try
                        {
                            ///Copia: si la BLL rechaza los datos, la fila de la grilla no queda modificada en memoria
                            BE.ClienteJP86 c = new BE.ClienteJP86()
                            {
                                DNI = seleccionado.DNI,
                                Nombre = ucm.Nombre(),
                                Apellido = ucm.Apellido(),
                                Telefono = ucm.Telefono(),
                                Activo = seleccionado.Activo
                            };

                            gclientes.Modificar(c);

                            ev = new Bitacora.Evento_64PR(Sesion.SessionManager.GetInstance.Usuario.Login, ((int)Bitacora.Bitacora_64PR.ModuloBitacora_64PR.GestionClientes).ToString(), ((int)Bitacora.Bitacora_64PR.TipoEventoBitacora_64PR.ModificacionCliente).ToString(), 4);
                            bita.RegistrarEvento(ev);

                            MessageBox.Show(textos["cliente_modificado"]);

                            CargaData();
                            ucm.LimpiarCampos();
                            pnlContenedor.Controls.Clear();
                            uc = null;
                            MostrarPanelMensajes();
                            modo = "consulta";
                            lblModo.Text = textos["frmGestionClientes_lblModoConsulta"];
                            btnGuardar.Enabled = false;
                        }
                        catch (InvalidOperationException ex)
                        {
                            MessageBox.Show(Traductor_64PR.TraducirMensaje(textos, ex.Message), textos["titulo_Validacion"], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        catch (SqlException ex)
                        {
                            MessageBox.Show(textos["msg_ErrorBaseDatos"] + ex.Message, textos["titulo_Error"], MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    break;
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvClientes.SelectedRows.Count == 0)
            {
                MessageBox.Show(textos["seleccionar_cliente"]);
                return;
            }

            BE.ClienteJP86 c = dgvClientes.SelectedRows[0].DataBoundItem as BE.ClienteJP86;

            bool vaAQuedarActivo = !c.Activo;

            try
            {
                gclientes.ActDesact(c);
            }
            catch (InvalidOperationException ex)
            {
                ///Regla de negocio de la baja (clave de idioma lanzada por la BLL): no se registra evento
                MessageBox.Show(Traductor_64PR.TraducirMensaje(textos, ex.Message), textos["titulo_Validacion"], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string tipoEvento = vaAQuedarActivo
                ? ((int)Bitacora.Bitacora_64PR.TipoEventoBitacora_64PR.AltaCliente).ToString()
                : ((int)Bitacora.Bitacora_64PR.TipoEventoBitacora_64PR.BajaCliente).ToString();
            ev = new Bitacora.Evento_64PR(Sesion.SessionManager.GetInstance.Usuario.Login, ((int)Bitacora.Bitacora_64PR.ModuloBitacora_64PR.GestionClientes).ToString(), tipoEvento, vaAQuedarActivo ? 4 : 3);
            bita.RegistrarEvento(ev);

            MessageBox.Show(textos["operacion exitosa"]);
            CargaData();
            btnEliminar.Text = textos["FrmGestionarClientesJP86.btnEliminar"];
        }

        private void dgvClientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                BE.ClienteJP86 c = dgvClientes.SelectedRows[0].DataBoundItem as BE.ClienteJP86;
                if (uc is ucModificarClienteJP86 ucm)
                {
                    ucm.EscribirControles(c);
                }

                btnEliminar.Text = c.Activo ? textos["FrmGestionarClientesJP86.btnEliminar"] : textos["activar"];
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked) AplicarFiltros();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked) AplicarFiltros();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked) AplicarFiltros();
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            AplicarFiltros();
        }

        private void FrmGestionarClientesJP86_Load(object sender, EventArgs e)
        {
            ConfigurarPermisos();
        }

        private void ConfigurarPermisos()
        {
            Sesion.Rol_64PR rolUsuario = Sesion.SessionManager.GetInstance.Usuario.Rol;

            btnCrear.Visible = rolUsuario.TienePermiso(Sesion.Patentes_64PR.CrearCliente);
            btnModificar.Visible = rolUsuario.TienePermiso(Sesion.Patentes_64PR.ModificarCliente);
            btnEliminar.Visible = rolUsuario.TienePermiso(Sesion.Patentes_64PR.EliminarCliente);

            bool puedeSerializar = rolUsuario.TienePermiso(Sesion.Patentes_64PR.SerializarClientes);
            btnSerializar.Visible = puedeSerializar;
            txtRutaSerializar.Visible = puedeSerializar;
            btnCarpetaSerializar.Visible = puedeSerializar;

            bool puedeDeserializar = rolUsuario.TienePermiso(Sesion.Patentes_64PR.DeserializarClientes);
            btnDeserializar.Visible = puedeDeserializar;
            txtRutaDeserializar.Visible = puedeDeserializar;
            btnCarpetaDeserializar.Visible = puedeDeserializar;

            ///Limpiar solo tiene sentido si hay algo que limpiar de la franja de serializacion
            btnLimpiar.Visible = puedeSerializar || puedeDeserializar;
            pnlSeparador.Visible = btnLimpiar.Visible;
        }

        #region Serializacion XML

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            ///Si se estaba viendo un XML se sale de ese modo (y se rehabilitan los botones segun patentes)
            if (modo == "xml")
            {
                SalirModoXml();
                return;
            }

            CargaData();
            if (uc == null)
                MostrarMensaje("msg_Clientes_GrillaActualizada", UI.TemaVisual.TextoPrincipal);
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtRutaSerializar.Clear();
            txtRutaDeserializar.Clear();
            LimpiarMensaje();

            if (modo == "xml")
            {
                SalirModoXml();
                LimpiarMensaje();
            }
        }

        private void btnCarpetaSerializar_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog dialogo = new SaveFileDialog())
            {
                dialogo.Title = Texto("dlg_Xml_TituloSerializar");
                dialogo.Filter = Texto("dlg_Xml_Filtro");
                dialogo.DefaultExt = "xml";
                dialogo.AddExtension = true;
                ///Si el archivo ya existe el dialogo pide confirmacion para sobrescribirlo
                dialogo.OverwritePrompt = true;
                dialogo.FileName = "Clientes.xml";

                if (dialogo.ShowDialog(this) == DialogResult.OK)
                    txtRutaSerializar.Text = dialogo.FileName;
            }
        }

        private void btnCarpetaDeserializar_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialogo = new OpenFileDialog())
            {
                dialogo.Title = Texto("dlg_Xml_TituloDeserializar");
                dialogo.Filter = Texto("dlg_Xml_Filtro");
                dialogo.CheckFileExists = true;
                dialogo.Multiselect = false;

                if (dialogo.ShowDialog(this) == DialogResult.OK)
                    txtRutaDeserializar.Text = dialogo.FileName;
            }
        }

        private void btnSerializar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRutaSerializar.Text))
            {
                MostrarMensajeEnConsulta("msg_Xml_SeleccioneUbicacion", UI.TemaVisual.Advertencia);
                return;
            }

            ///Se serializa exactamente lo visible: la lista enlazada a la grilla ya tiene aplicados Buscar y Modo consulta
            List<BE.ClienteJP86> visibles = dgvClientes.DataSource as List<BE.ClienteJP86>;
            if (visibles == null || visibles.Count == 0)
            {
                MostrarMensajeEnConsulta("msg_Xml_GrillaVacia", UI.TemaVisual.Advertencia);
                return;
            }

            try
            {
                int cantidad = gclientes.Serializar(new List<BE.ClienteJP86>(visibles), txtRutaSerializar.Text);

                RegistrarEventoXml(Bitacora.Bitacora_64PR.TipoEventoBitacora_64PR.SerializacionClientes, 4);
                MostrarMensajeEnConsulta("msg_Xml_SerializacionOk", UI.TemaVisual.Exito, cantidad, txtRutaSerializar.Text);
            }
            catch (InvalidOperationException ex)
            {
                RegistrarEventoXml(Bitacora.Bitacora_64PR.TipoEventoBitacora_64PR.ErrorSerializacionClientes, 2);
                MostrarMensajeEnConsulta("msg_Xml_ErrorSerializar", UI.TemaVisual.Peligro, ex.Message);
            }
            catch (Exception ex)
            {
                MostrarMensajeEnConsulta("msg_Xml_ErrorSerializar", UI.TemaVisual.Peligro, "err_Inesperado");
                ManejadorErroresJP86.MostrarErrorInesperado(ex);
            }
        }

        private void btnDeserializar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRutaDeserializar.Text))
            {
                MostrarMensajeEnConsulta("msg_Xml_SeleccioneArchivo", UI.TemaVisual.Advertencia);
                return;
            }

            try
            {
                List<BE.ClienteJP86> clientes = gclientes.Deserializar(txtRutaDeserializar.Text);

                EntrarModoXml(clientes);

                RegistrarEventoXml(Bitacora.Bitacora_64PR.TipoEventoBitacora_64PR.DeserializacionClientes, 4);
                MostrarMensaje("msg_Xml_DeserializacionOk", UI.TemaVisual.Info, clientes.Count, txtRutaDeserializar.Text);
            }
            catch (InvalidOperationException ex)
            {
                ///Si falla, la grilla queda como estaba (BD o el XML anterior)
                RegistrarEventoXml(Bitacora.Bitacora_64PR.TipoEventoBitacora_64PR.ErrorDeserializacionClientes, 2);
                MostrarMensajeEnConsulta("msg_Xml_ErrorDeserializar", UI.TemaVisual.Peligro, ex.Message);
            }
            catch (Exception ex)
            {
                MostrarMensajeEnConsulta("msg_Xml_ErrorDeserializar", UI.TemaVisual.Peligro, "err_Inesperado");
                ManejadorErroresJP86.MostrarErrorInesperado(ex);
            }
        }

        ///Los mensajes de la franja se muestran en pnlContenedor: si habia un alta/modificacion abierta (sin guardar) se cierra
        private void MostrarMensajeEnConsulta(string clave, Color color, params object[] args)
        {
            if (uc != null)
                VolverAModoConsulta();
            MostrarMensaje(clave, color, args);
        }

        private void VolverAModoConsulta()
        {
            pnlContenedor.Controls.Clear();
            uc = null;
            MostrarPanelMensajes();
            btnGuardar.Enabled = false;
            if (modo != "xml")
            {
                modo = "consulta";
                lblModo.Text = textos["frmGestionClientes_lblModoConsulta"];
            }
        }

        ///Modo visualizacion XML: la grilla muestra el contenido del archivo y se bloquea todo lo que escribe en la BD.
        ///Buscar y Modo consulta siguen funcionando porque filtran sobre lst.
        private void EntrarModoXml(List<BE.ClienteJP86> clientes)
        {
            if (uc != null)
                VolverAModoConsulta();

            modo = "xml";
            lst = clientes;
            AplicarFiltros();

            ///Se ocultan ademas de deshabilitarse: el estilo redondeado no pinta distinto un boton deshabilitado
            foreach (Button b in new[] { btnCrear, btnModificar, btnEliminar, btnGuardar })
            {
                b.Enabled = false;
                b.Visible = false;
            }

            lblModo.Text = Texto("frmGestionClientes_lblModoXml");
            lblModo.ForeColor = UI.TemaVisual.Peligro;
        }

        private void SalirModoXml()
        {
            modo = "consulta";
            lblModo.Text = textos["frmGestionClientes_lblModoConsulta"];
            lblModo.ForeColor = colorModoNormal;

            btnCrear.Enabled = true;
            btnModificar.Enabled = true;
            btnEliminar.Enabled = true;
            btnEliminar.Text = textos["FrmGestionarClientesJP86.btnEliminar"];
            btnGuardar.Enabled = false;
            btnGuardar.Visible = true;
            ConfigurarPermisos();

            CargaData();
            MostrarMensaje("msg_Xml_VueltaABD", UI.TemaVisual.TextoPrincipal);
        }

        private void RegistrarEventoXml(Bitacora.Bitacora_64PR.TipoEventoBitacora_64PR tipo, int criticidad)
        {
            ev = new Bitacora.Evento_64PR(Sesion.SessionManager.GetInstance.Usuario.Login, ((int)Bitacora.Bitacora_64PR.ModuloBitacora_64PR.GestionClientes).ToString(), ((int)tipo).ToString(), criticidad);
            bita.RegistrarEvento(ev);
        }

        #endregion

        private void FrmGestionarClientesJP86_FormClosed(object sender, FormClosedEventArgs e)
        {
            Idioma.GestorIdioma_64PR.GetInstance.Desuscribir(this);
        }
    }
}
