/*
===============================================================================
 03_Secuencia_CUN01.js
===============================================================================
 Proyecto: SIGAM (FleetDrive S.A.) - Trabajo de Diploma UAI

 QUE CREA:
   - Paquete "Secuencia" (si no existe) y dentro de el el sub-paquete
     "CUN-01 Generar Contrato de Alquiler" debajo del paquete raiz.
   - El elemento de interfaz "ProyectoIS_64PR.FrmGenerarContratoJP86"
     (estereotipo boundary), unico elemento de UI que crea este script -
     el resto de los participantes (actor, clases BLL, DAL_64PR.Acceso,
     DV.DV_64PR, Documentos.GeneradorReciboContrato) se REUTILIZAN por
     nombre desde los paquetes "Casos de Uso" / "Clases" creados por
     01_CasosDeUso.js / 02_Clases_RFN1.js.
   - El diagrama de secuencia "DS - CUN-01 Generar Contrato de Alquiler"
     con las 8 lifelines pedidas por la regla de diseño (Actor + UNA caja
     GUI + clases BLL involucradas + UNA caja DAL + DV solo si se dispara;
     sin Bitacora ni Idioma) mas Documentos (servicio transversal
     equivalente al ya usado en CUN-05, no es Bitacora/Idioma):
       Agente Comercial | FrmGenerarContratoJP86 | BLL_64PR.ContratoJP86 |
       BLL_64PR.ClienteJP86 | BLL_64PR.VehiculoJP86 | DAL_64PR.Acceso |
       DV.DV_64PR | Documentos.GeneradorReciboContrato

 FUENTE: Informe_RFN1.md seccion 5.1, verificado y ajustado contra el
 codigo real (ProyectoIS_64PR/FrmGenerarContratoJP86.cs,
 BLL_64PR/ContratoJP86.cs, Mapper/mpp_contrato.cs).

 DISCREPANCIA INFORME/CODIGO YA RESUELTA (ver Notes de CUN-01 en el
 paquete "Casos de Uso"): el informe original no documentaba el boton
 "Imprimir Recibo" (Documentos.GeneradorReciboContrato.Imprimir) que si
 esta implementado en el codigo; se agrego como paso opcional al final
 del diagrama (mensajes 37-38).

 LIMITACIONES CONOCIDAS DE ESTE SCRIPT (API de EA inestable / no
 verificable sin ejecutar EA - ver README_EA.md y avisar al desarrollador
 si el resultado visual no es el esperado):
   1. Fragmentos alt/opt nativos de UML (combined fragments) NO se
      generan via Automation Interface en este script: la creacion
      programatica de "InteractionFragment"/frames alt-opt-loop no tiene
      una API estable y documentada para JScript ES3. Los 3 flujos
      alternativos/opcionales de CUN-01 (filtro de busqueda opcional,
      extension embebida de CUN-02, impresion opcional del recibo) se
      dejan como elementos Note posicionados junto a los mensajes
      correspondientes, con el texto del flujo alternativo. RETOQUE
      MANUAL SUGERIDO: reemplazar cada Note por un frame alt/opt real
      dibujado a mano en EA (Insert Frame > alt/opt) una vez importado.
   2. El orden visual de los mensajes (numeracion de secuencia) se logra
      con DOS mecanismos redundantes: (a) el ORDEN DE CREACION de los
      DiagramLink en el diagrama (EA numera los mensajes segun el orden
      en que se agregan al diagrama), y (b) un prefijo numerico explicito
      en el nombre de cada mensaje ("01: ...", "02: ...", etc.) como
      respaldo visual si (a) no se comporta como se espera. Verificar en
      EA que la numeracion automatica coincida con el prefijo; si no,
      avisar para ajustar el mecanismo.
   3. Los mensajes de retorno (respuesta) se modelan como conectores
      "Sequence" adicionales con direccion invertida (de la clase llamada
      hacia la que llama) y el texto "retorna: ...", en lugar de usar el
      estilo nativo de flecha punteada de retorno de EA (no expuesto de
      forma confiable via Automation Interface). RETOQUE MANUAL SUGERIDO:
      marcar estos mensajes como "Response" desde las propiedades del
      mensaje en EA para que se dibujen punteados.
   4. No se generan barras de activacion (focus of control) manualmente;
      EA las genera automaticamente al abrir el diagrama en la mayoria de
      los casos - si no aparecen, agregarlas a mano.
   5. Coordenadas: se usan valores de "top"/"bottom" NEGATIVOS crecientes
      hacia abajo (convencion habitual de la Automation Interface de EA).
      Si al abrir el diagrama las lifelines aparecen invertidas o fuera
      de vista, avisar: es un ajuste de signo trivial en ColocarLifeline().

 PRERREQUISITOS: correr primero 01_CasosDeUso.js (crea el actor "Agente
 Comercial") y 02_Clases_RFN1.js (crea BLL_64PR.ContratoJP86,
 BLL_64PR.ClienteJP86, BLL_64PR.VehiculoJP86, DAL_64PR.Acceso, DV.DV_64PR,
 Documentos.GeneradorReciboContrato). Si algun elemento no se encuentra,
 este script lo informa por Session.Output con nivel ERROR y continua
 con el resto en la medida de lo posible.

 ORDEN DE EJECUCION: 01_CasosDeUso -> 02_Clases_RFN1 -> 02_Clases_RFN2 ->
 03_Secuencia_CUN01 (este) -> 03_Secuencia_CUN02 ... CUN-09 -> 04_DER (opcional)

 IDEMPOTENCIA: busca por nombre/texto antes de crear paquetes, el
 elemento GUI, mensajes (conectores tipo Sequence) y notas; se puede
 re-ejecutar sin duplicar.
===============================================================================
*/

var contadorCreados = 0;
var contadorExistentes = 0;
var contadorErrores = 0;
var numeroMensaje = 0;

function log(nivel, mensaje) {
    Session.Output(nivel + ": " + mensaje);
}

function ObtenerOCrearPaquete(paquetePadre, nombre) {
    var i;
    try {
        for (i = 0; i < paquetePadre.Packages.Count; i++) {
            var p = paquetePadre.Packages.GetAt(i);
            if (p.Name == nombre) {
                contadorExistentes++;
                return p;
            }
        }
        var nuevo = paquetePadre.Packages.AddNew(nombre, "");
        nuevo.Update();
        paquetePadre.Packages.Refresh();
        contadorCreados++;
        log("OK", "Paquete creado: " + nombre);
        return nuevo;
    } catch (e) {
        contadorErrores++;
        log("ERROR", "No se pudo crear/obtener el paquete '" + nombre + "': " + e.description);
        return null;
    }
}

function BuscarElementoPorNombreExacto(paquete, nombre) {
    var i;
    try {
        for (i = 0; i < paquete.Elements.Count; i++) {
            var el = paquete.Elements.GetAt(i);
            if (el.Name == nombre) {
                return el;
            }
        }
    } catch (e) {
        // paquete invalido, se ignora
    }
    return null;
}

// Busca un elemento por nombre en el paquete indicado y, si no lo
// encuentra, en el paquete raiz completo (recorre subpaquetes de primer
// nivel). Se usa para reutilizar clases creadas por otros scripts.
function BuscarElementoReutilizable(raiz, nombrePaqueteSugerido, nombreElemento) {
    var i;
    var pkgSugerido = null;
    for (i = 0; i < raiz.Packages.Count; i++) {
        if (raiz.Packages.GetAt(i).Name == nombrePaqueteSugerido) {
            pkgSugerido = raiz.Packages.GetAt(i);
            break;
        }
    }
    if (pkgSugerido != null) {
        var encontrado = BuscarElementoPorNombreExacto(pkgSugerido, nombreElemento);
        if (encontrado != null) return encontrado;
    }
    for (i = 0; i < raiz.Packages.Count; i++) {
        var encontrado2 = BuscarElementoPorNombreExacto(raiz.Packages.GetAt(i), nombreElemento);
        if (encontrado2 != null) return encontrado2;
    }
    return null;
}

function ObtenerOCrearElemento(paquete, nombre, tipo, estereotipo) {
    var i;
    try {
        for (i = 0; i < paquete.Elements.Count; i++) {
            var el = paquete.Elements.GetAt(i);
            if (el.Name == nombre && el.Type == tipo) {
                contadorExistentes++;
                return el;
            }
        }
        var nuevo = paquete.Elements.AddNew(nombre, tipo);
        if (estereotipo) {
            nuevo.Stereotype = estereotipo;
        }
        nuevo.Update();
        paquete.Elements.Refresh();
        contadorCreados++;
        log("OK", "Elemento creado: " + nombre + " (" + tipo + ")");
        return nuevo;
    } catch (e) {
        contadorErrores++;
        log("ERROR", "No se pudo crear/obtener el elemento '" + nombre + "': " + e.description);
        return null;
    }
}

function ObtenerOCrearNota(paquete, texto) {
    var i;
    try {
        for (i = 0; i < paquete.Elements.Count; i++) {
            var el = paquete.Elements.GetAt(i);
            if (el.Type == "Note" && el.Notes == texto) {
                contadorExistentes++;
                return el;
            }
        }
        var nuevo = paquete.Elements.AddNew("Nota", "Note");
        nuevo.Notes = texto;
        nuevo.Update();
        paquete.Elements.Refresh();
        contadorCreados++;
        return nuevo;
    } catch (e) {
        contadorErrores++;
        log("ERROR", "No se pudo crear la nota: " + e.description);
        return null;
    }
}

function ObtenerOCrearDiagrama(paquete, nombre, tipo) {
    var i;
    try {
        for (i = 0; i < paquete.Diagrams.Count; i++) {
            var d = paquete.Diagrams.GetAt(i);
            if (d.Name == nombre) {
                contadorExistentes++;
                return d;
            }
        }
        var nuevo = paquete.Diagrams.AddNew(nombre, tipo);
        nuevo.Update();
        paquete.Diagrams.Refresh();
        contadorCreados++;
        log("OK", "Diagrama creado: " + nombre);
        return nuevo;
    } catch (e) {
        contadorErrores++;
        log("ERROR", "No se pudo crear el diagrama '" + nombre + "': " + e.description);
        return null;
    }
}

function ExisteObjetoEnDiagrama(diagrama, elementID) {
    var i;
    for (i = 0; i < diagrama.DiagramObjects.Count; i++) {
        var o = diagrama.DiagramObjects.GetAt(i);
        if (o.ElementID == elementID) {
            return true;
        }
    }
    return false;
}

function AgregarObjetoADiagrama(diagrama, elemento, left, top, right, bottom) {
    try {
        if (elemento == null) return;
        if (ExisteObjetoEnDiagrama(diagrama, elemento.ElementID)) {
            return;
        }
        var geometria = "l=" + left + ";r=" + right + ";t=" + top + ";b=" + bottom + ";";
        var obj = diagrama.DiagramObjects.AddNew(geometria, "");
        obj.ElementID = elemento.ElementID;
        obj.Update();
        diagrama.DiagramObjects.Refresh();
    } catch (e) {
        contadorErrores++;
        log("ERROR", "No se pudo colocar '" + elemento.Name + "' en el diagrama '" + diagrama.Name + "': " + e.description);
    }
}

// Coloca una lifeline (header en la fila superior del diagrama de secuencia).
// Ver limitacion (5) del encabezado sobre el signo de las coordenadas.
function ColocarLifeline(diagrama, elemento, columna) {
    var anchoCol = 260;
    var left = 40 + columna * anchoCol;
    var right = left + 200;
    AgregarObjetoADiagrama(diagrama, elemento, left, -20, right, -110);
}

function ObtenerOCrearMensaje(origen, destino, etiqueta) {
    var i;
    try {
        if (origen == null || destino == null) return null;
        for (i = 0; i < origen.Connectors.Count; i++) {
            var c = origen.Connectors.GetAt(i);
            if (c.Type == "Sequence" && c.SupplierID == destino.ElementID && c.Name == etiqueta) {
                contadorExistentes++;
                return c;
            }
        }
        var nuevo = origen.Connectors.AddNew(etiqueta, "Sequence");
        nuevo.ClientID = origen.ElementID;
        nuevo.SupplierID = destino.ElementID;
        nuevo.Update();
        origen.Connectors.Refresh();
        contadorCreados++;
        return nuevo;
    } catch (e) {
        contadorErrores++;
        log("ERROR", "No se pudo crear el mensaje '" + etiqueta + "': " + e.description);
        return null;
    }
}

function ExisteLinkEnDiagrama(diagrama, connectorID) {
    var i;
    for (i = 0; i < diagrama.DiagramLinks.Count; i++) {
        var dl = diagrama.DiagramLinks.GetAt(i);
        if (dl.ConnectorID == connectorID) {
            return true;
        }
    }
    return false;
}

function AgregarMensajeADiagrama(diagrama, conector) {
    try {
        if (conector == null) return;
        if (ExisteLinkEnDiagrama(diagrama, conector.ConnectorID)) {
            return;
        }
        var link = diagrama.DiagramLinks.AddNew("", "");
        link.ConnectorID = conector.ConnectorID;
        link.Update();
        diagrama.DiagramLinks.Refresh();
    } catch (e) {
        contadorErrores++;
        log("ERROR", "No se pudo agregar el mensaje al diagrama: " + e.description);
    }
}

// Crea (o reutiliza) un mensaje numerado y lo agrega al diagrama, en el
// orden de llamada de esta funcion (ver limitacion (2) del encabezado).
function Mensaje(diagrama, origen, destino, texto) {
    numeroMensaje++;
    var numeroFormateado = numeroMensaje < 10 ? "0" + numeroMensaje : "" + numeroMensaje;
    var etiqueta = numeroFormateado + ": " + texto;
    var conector = ObtenerOCrearMensaje(origen, destino, etiqueta);
    AgregarMensajeADiagrama(diagrama, conector);
    return conector;
}

function AgregarNotaADiagrama(diagrama, paquete, texto, left, top, right, bottom, anclaElemento) {
    var nota = ObtenerOCrearNota(paquete, texto);
    if (nota == null) return;
    AgregarObjetoADiagrama(diagrama, nota, left, top, right, bottom);
    if (anclaElemento != null) {
        try {
            var yaExiste = false;
            var i;
            for (i = 0; i < nota.Connectors.Count; i++) {
                if (nota.Connectors.GetAt(i).Type == "NoteLink" && nota.Connectors.GetAt(i).SupplierID == anclaElemento.ElementID) {
                    yaExiste = true;
                }
            }
            if (!yaExiste) {
                var link = nota.Connectors.AddNew("", "NoteLink");
                link.ClientID = nota.ElementID;
                link.SupplierID = anclaElemento.ElementID;
                link.Update();
                nota.Connectors.Refresh();
            }
        } catch (e) {
            contadorErrores++;
            log("ERROR", "No se pudo anclar la nota: " + e.description);
        }
    }
}

function main() {
    Session.Output("===============================================================");
    Session.Output("03_Secuencia_CUN01.js - inicio");
    Session.Output("===============================================================");

    var raiz = Repository.GetTreeSelectedPackage();
    if (raiz == null) {
        Session.Output("ERROR: no hay ningun paquete seleccionado en el arbol de proyecto. Seleccione el paquete raiz 'SIGAM' y vuelva a ejecutar.");
        return;
    }
    log("OK", "Paquete raiz: " + raiz.Name);

    // Diagnostico: si "Casos de Uso" y "Clases" no aparecen en esta lista,
    // el paquete seleccionado en el arbol de EA NO es el mismo "SIGAM" que
    // se uso para correr 01_CasosDeUso.js/02_Clases_RFN1.js. Volver a
    // seleccionar el "SIGAM" correcto en el arbol de proyecto y reintentar.
    var nombresHijos = "";
    var j;
    for (j = 0; j < raiz.Packages.Count; j++) {
        nombresHijos += raiz.Packages.GetAt(j).Name;
        if (j < raiz.Packages.Count - 1) nombresHijos += ", ";
    }
    log("OK", "Sub-paquetes directos de '" + raiz.Name + "': " + (nombresHijos == "" ? "(ninguno)" : nombresHijos));

    var pkgSecuencia = ObtenerOCrearPaquete(raiz, "Secuencia");
    if (pkgSecuencia == null) { return; }
    var pkgCUN01 = ObtenerOCrearPaquete(pkgSecuencia, "CUN-01 Generar Contrato de Alquiler");
    if (pkgCUN01 == null) { return; }

    // ---------------------------------------------------------------
    // Participantes reutilizados (creados por 01_CasosDeUso.js /
    // 02_Clases_RFN1.js). Si no se encuentran, se informa por
    // Session.Output y se continua igual (el mensaje que los use
    // simplemente no podra crearse).
    // ---------------------------------------------------------------
    var actor = BuscarElementoReutilizable(raiz, "Casos de Uso", "Agente Comercial");
    if (actor == null) log("WARN", "No se encontro el actor 'Agente Comercial'. Corra 01_CasosDeUso.js primero.");

    var bllContrato = BuscarElementoReutilizable(raiz, "Clases", "BLL_64PR.ContratoJP86");
    var bllCliente = BuscarElementoReutilizable(raiz, "Clases", "BLL_64PR.ClienteJP86");
    var bllVehiculo = BuscarElementoReutilizable(raiz, "Clases", "BLL_64PR.VehiculoJP86");
    var dal = BuscarElementoReutilizable(raiz, "Clases", "DAL_64PR.Acceso");
    var dv = BuscarElementoReutilizable(raiz, "Clases", "DV.DV_64PR");
    var documentosRecibo = BuscarElementoReutilizable(raiz, "Clases", "Documentos.GeneradorReciboContrato");

    var participantesFaltantes = [bllContrato, bllCliente, bllVehiculo, dal, dv, documentosRecibo];
    var nombresFaltantes = ["BLL_64PR.ContratoJP86", "BLL_64PR.ClienteJP86", "BLL_64PR.VehiculoJP86", "DAL_64PR.Acceso", "DV.DV_64PR", "Documentos.GeneradorReciboContrato"];
    var i;
    for (i = 0; i < participantesFaltantes.length; i++) {
        if (participantesFaltantes[i] == null) {
            log("WARN", "No se encontro '" + nombresFaltantes[i] + "'. Corra 02_Clases_RFN1.js primero.");
        }
    }

    // GUI: unico elemento de UI, creado por este script.
    var gui = ObtenerOCrearElemento(pkgCUN01, "ProyectoIS_64PR.FrmGenerarContratoJP86", "Class", "boundary");
    if (gui != null && gui.Notes == "") {
        gui.Notes = "Formulario WinForms (Form, Idioma.IObservadorIdioma_64PR). Implementa CUN-01 (Generar Contrato de Alquiler) y embebe CUN-02 (Registrar Cliente) como extension. Verifica permisos con Sesion.Patentes_64PR.GenerarContrato antes de habilitar 'Confirmar'.";
        gui.Update();
    }

    // ---------------------------------------------------------------
    // Diagrama de secuencia
    // ---------------------------------------------------------------
    var diagrama = ObtenerOCrearDiagrama(pkgCUN01, "DS - CUN-01 Generar Contrato de Alquiler", "Sequence");
    if (diagrama == null) { return; }

    ColocarLifeline(diagrama, actor, 0);
    ColocarLifeline(diagrama, gui, 1);
    ColocarLifeline(diagrama, bllContrato, 2);
    ColocarLifeline(diagrama, bllCliente, 3);
    ColocarLifeline(diagrama, bllVehiculo, 4);
    ColocarLifeline(diagrama, dal, 5);
    ColocarLifeline(diagrama, dv, 6);
    ColocarLifeline(diagrama, documentosRecibo, 7);

    // ---------------------------------------------------------------
    // Mensajes - carga inicial del formulario (sin filtros)
    // ---------------------------------------------------------------
    Mensaje(diagrama, actor, gui, "abre 'Alquileres > Generar contrato'");
    Mensaje(diagrama, gui, bllContrato, "BuscarUnidadesDisponibles(null, null, null)");
    Mensaje(diagrama, bllContrato, dal, "mpp_contrato.BuscarUnidadesDisponibles() -> SP_ContratoJP86_BuscarUnidadesDisponibles");
    Mensaje(diagrama, dal, bllContrato, "retorna: DataTable");
    Mensaje(diagrama, bllContrato, gui, "retorna: List<VehiculoJP86> disponibles (todas DISPONIBLE)");
    Mensaje(diagrama, gui, bllCliente, "Listar()");
    Mensaje(diagrama, bllCliente, dal, "mpp_cliente.Listar() -> SP_ClienteJP86_Listar");
    Mensaje(diagrama, dal, bllCliente, "retorna: DataTable");
    Mensaje(diagrama, bllCliente, gui, "retorna: List<ClienteJP86> todos");
    Mensaje(diagrama, gui, actor, "muestra dgvUnidades (todas disponibles) y dgvClientes (todos)");

    AgregarNotaADiagrama(diagrama, pkgCUN01,
        "OPT - filtro de busqueda (no modelado como fragmento opt nativo, ver limitacion 1 del encabezado):\n" +
        "El Agente Comercial puede ingresar fechas/categoria/marca/km maximo y click 'Buscar unidades'.\n" +
        "En ese caso se repiten los mensajes 02-05 con BuscarUnidadesDisponibles(idCategoria, marca, kmMaximo).\n" +
        "Alternativo 4.1: si la busqueda filtrada no devuelve unidades, MessageBox informativo y vuelve al ingreso de criterios (no llega a GenerarContrato).",
        40, -160, 560, -280, gui);

    // ---------------------------------------------------------------
    // Seleccion de unidad y cliente
    // ---------------------------------------------------------------
    Mensaje(diagrama, actor, gui, "selecciona unidad (click fila dgvUnidades)");
    Mensaje(diagrama, actor, gui, "selecciona cliente (click fila dgvClientes)");

    AgregarNotaADiagrama(diagrama, pkgCUN01,
        "ALT - extiende CUN-02 (no modelado como fragmento alt nativo, ver limitacion 1 del encabezado):\n" +
        "Si el cliente no aparece en dgvClientes, el Agente Comercial hace click en 'Cliente nuevo',\n" +
        "completa Apellido/Nombre/DNI/Telefono en ucCrearClienteJP86 embebido y confirma 'Guardar cliente nuevo'.\n" +
        "Disparan entonces los mensajes 13-21 (Crear + RecalcularTabla + Listar de refresco).\n" +
        "Ver tambien el caso de uso propio CUN-02 (diagrama de secuencia independiente, no incluido en Fase A).",
        620, -160, 1140, -300, gui);

    Mensaje(diagrama, actor, gui, "click 'Cliente nuevo' + completa datos + 'Guardar cliente nuevo'");
    Mensaje(diagrama, gui, bllCliente, "Crear(nuevoCliente)");
    Mensaje(diagrama, bllCliente, dal, "mpp_cliente.Crear() -> SP_ClienteJP86_Crear");
    Mensaje(diagrama, dal, bllCliente, "retorna: OK");
    Mensaje(diagrama, bllCliente, dv, "RecalcularTabla(\"ClienteJP86\")");
    Mensaje(diagrama, gui, bllCliente, "Listar() [refresca dgvClientes con el nuevo cliente]");
    Mensaje(diagrama, bllCliente, dal, "mpp_cliente.Listar() -> SP_ClienteJP86_Listar");
    Mensaje(diagrama, dal, bllCliente, "retorna: DataTable");
    Mensaje(diagrama, bllCliente, gui, "retorna: List<ClienteJP86> actualizada");

    // ---------------------------------------------------------------
    // Calculo de importe y confirmacion
    // ---------------------------------------------------------------
    Mensaje(diagrama, actor, gui, "tilda 'Cargos adicionales' (opcional)");
    Mensaje(diagrama, gui, bllContrato, "CalcularImporteTotal(tarifaDiaria, fechaInicio, fechaFin, cargosAdicionales)");
    Mensaje(diagrama, bllContrato, gui, "retorna: importeTotal");
    Mensaje(diagrama, gui, actor, "muestra importe total");
    Mensaje(diagrama, actor, gui, "click 'Confirmar'");
    Mensaje(diagrama, gui, bllContrato, "GenerarContrato(cliente, vehiculo, fechaInicio, fechaFin, cargosAdicionales)");
    Mensaje(diagrama, bllContrato, dal, "mpp_contrato.Crear(contrato) -> SP_ContratoJP86_Crear");
    Mensaje(diagrama, dal, bllContrato, "retorna: NumeroContrato");
    Mensaje(diagrama, bllContrato, dv, "RecalcularTabla(\"ContratoJP86\")");
    Mensaje(diagrama, bllContrato, bllVehiculo, "CambiarEstado(patente, ALQUILADO)");
    Mensaje(diagrama, bllVehiculo, dal, "mpp_vehiculo.CambiarEstado() -> SP_VehiculoJP86_CambiarEstado");
    Mensaje(diagrama, dal, bllVehiculo, "retorna: OK");
    Mensaje(diagrama, bllVehiculo, dv, "RecalcularTabla(\"VehiculoJP86\")");
    Mensaje(diagrama, bllContrato, gui, "retorna: contrato (NumeroContrato asignado)");
    Mensaje(diagrama, gui, actor, "MessageBox 'Contrato generado. Numero: X' + habilita 'Imprimir Recibo'");

    // ---------------------------------------------------------------
    // Impresion opcional del recibo (discrepancia informe/codigo, ver
    // encabezado y Notes de CUN-01 en el paquete "Casos de Uso")
    // ---------------------------------------------------------------
    AgregarNotaADiagrama(diagrama, pkgCUN01,
        "OPT - Imprimir Recibo (no modelado como fragmento opt nativo, ver limitacion 1):\n" +
        "DISCREPANCIA informe/codigo: Informe_RFN1.md seccion 5.1 no documenta este paso; se agrego\n" +
        "porque FrmGenerarContratoJP86.btnImprimirRecibo_Click llama a Documentos.GeneradorReciboContrato.Imprimir\n" +
        "(mismo patron transversal que Documentos.GeneradorReporteDesperfecto en CUN-05).",
        1220, -160, 1740, -260, gui);

    Mensaje(diagrama, actor, gui, "click 'Imprimir Recibo' (opcional)");
    Mensaje(diagrama, gui, documentosRecibo, "Imprimir(contrato)");

    try {
        Repository.ReloadDiagram(diagrama.DiagramID);
    } catch (e) {
        contadorErrores++;
        log("ERROR", "No se pudo recargar el diagrama: " + e.description);
    }
    try {
        Repository.RefreshModelView(pkgCUN01.PackageGUID);
    } catch (e) {
        // no fatal
    }

    Session.Output("===============================================================");
    Session.Output("03_Secuencia_CUN01.js - RESUMEN: creados=" + contadorCreados + " existentes=" + contadorExistentes + " errores=" + contadorErrores + " mensajes=" + numeroMensaje);
    Session.Output("===============================================================");
}

main();
