/*
===============================================================================
 02_Clases_RFN1.js
===============================================================================
 Proyecto: SIGAM (FleetDrive S.A.) - Trabajo de Diploma UAI

 QUE CREA:
   - Paquete "Clases" debajo del paquete raiz seleccionado en el arbol de
     proyecto.
   - Clases/enums/interfaces de BE, Mapper, BLL_64PR y DAL_64PR involucrados
     en CUN-01 a CUN-04 (RFN1), CON ATRIBUTOS TIPADOS Y FIRMAS DE METODO
     tal como estan en el codigo real (fuente de verdad: el .cs, no el
     informe, ante cualquier discrepancia).
   - Incluye las clases COMPARTIDAS con RF1/RFN2 (Categoria_64PR,
     VehiculoJP86, EstadoVehiculoJP86, ClienteJP86, DAL_64PR.Acceso,
     DV.DV_64PR-stub): se crean UNA sola vez aca; 02_Clases_RFN2.js las
     busca por nombre y las REUTILIZA, no las recrea.
   - Convencion de nombre de elemento: "Namespace.NombreClase" exactamente
     como se usa en el codigo (BE.X, BLL_64PR.X, Mapper.mpp_x, DAL_64PR.X),
     para evitar colisiones entre clases con el mismo nombre simple en
     distintos namespaces (ej. BE.ContratoJP86 vs BLL_64PR.ContratoJP86).
   - 2 diagramas de clases (asi estan organizados en Informe_RFN1.md:
     seccion 4 = modelo general: seccion 9 = patron Strategy de CUN-03):
       * "DCL - RFN1 Modelo General"        (Categoria/Vehiculo/Cliente/
         Contrato/Factura + Mapper + BLL + DAL/DV)
       * "DCL - RFN1 CUN-03 Estrategia de Pago" (Strategy Pattern del pago
         simulado: IEstrategiaPagoJP86 + 4 estrategias + Fabrica)

 NOTA IMPORTANTE - DV.DV_64PR y Documentos.*:
   Son servicios transversales PREEXISTENTES (no forman parte de RFN1/RFN2).
   Se los modela como "stub" (solo el/los metodo/s que efectivamente llaman
   las clases de este RFN) para poder dibujar la dependencia, sin pretender
   documentar su implementacion completa (fuera de alcance de esta tarea).

 PRERREQUISITOS: correr primero 01_CasosDeUso.js (crea el paquete raiz
 "Casos de Uso"; este script solo necesita que exista el paquete raiz
 "SIGAM" seleccionado en el arbol, no depende del contenido de 01).

 ORDEN DE EJECUCION: 01_CasosDeUso -> 02_Clases_RFN1 (este) ->
 02_Clases_RFN2 -> 03_Secuencia_CUN01 ... 03_Secuencia_CUN09 -> 04_DER (opcional)

 IDEMPOTENCIA: busca por nombre completo (Namespace.Clase) antes de crear
 paquetes, elementos, atributos, metodos, parametros, relaciones y
 diagramas; se puede re-ejecutar sin duplicar.
===============================================================================
*/

var contadorCreados = 0;
var contadorExistentes = 0;
var contadorErrores = 0;

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

function BuscarElementoPorNombre(paquete, nombre) {
    var i;
    for (i = 0; i < paquete.Elements.Count; i++) {
        var el = paquete.Elements.GetAt(i);
        if (el.Name == nombre) {
            return el;
        }
    }
    return null;
}

function ObtenerOCrearAtributo(elemento, nombre, tipo, visibilidad) {
    var i;
    try {
        if (elemento == null) return null;
        for (i = 0; i < elemento.Attributes.Count; i++) {
            var a = elemento.Attributes.GetAt(i);
            if (a.Name == nombre) {
                contadorExistentes++;
                return a;
            }
        }
        var nuevo = elemento.Attributes.AddNew(nombre, tipo);
        nuevo.Visibility = visibilidad;
        nuevo.Update();
        elemento.Attributes.Refresh();
        contadorCreados++;
        return nuevo;
    } catch (e) {
        contadorErrores++;
        log("ERROR", "No se pudo crear el atributo '" + nombre + "' en '" + elemento.Name + "': " + e.description);
        return null;
    }
}

function ObtenerOCrearMetodo(elemento, nombre, tipoRetorno, visibilidad) {
    var i;
    try {
        if (elemento == null) return null;
        for (i = 0; i < elemento.Methods.Count; i++) {
            var m = elemento.Methods.GetAt(i);
            if (m.Name == nombre) {
                contadorExistentes++;
                return m;
            }
        }
        var nuevo = elemento.Methods.AddNew(nombre, tipoRetorno);
        nuevo.Visibility = visibilidad;
        nuevo.Update();
        elemento.Methods.Refresh();
        contadorCreados++;
        return nuevo;
    } catch (e) {
        contadorErrores++;
        log("ERROR", "No se pudo crear el metodo '" + nombre + "' en '" + elemento.Name + "': " + e.description);
        return null;
    }
}

function AgregarParametro(metodo, nombre, tipo, posicion) {
    var i;
    try {
        if (metodo == null) return;
        for (i = 0; i < metodo.Parameters.Count; i++) {
            var p = metodo.Parameters.GetAt(i);
            if (p.Name == nombre) {
                return;
            }
        }
        var nuevo = metodo.Parameters.AddNew(nombre, tipo);
        nuevo.Position = posicion;
        nuevo.Update();
        metodo.Parameters.Refresh();
    } catch (e) {
        contadorErrores++;
        log("ERROR", "No se pudo agregar el parametro '" + nombre + "' al metodo '" + metodo.Name + "': " + e.description);
    }
}

// arrParams: array de arrays [nombre, tipo]
function CrearMetodoConParams(elemento, nombre, tipoRetorno, visibilidad, arrParams) {
    var metodo = ObtenerOCrearMetodo(elemento, nombre, tipoRetorno, visibilidad);
    var i;
    if (metodo != null && arrParams != null) {
        for (i = 0; i < arrParams.length; i++) {
            AgregarParametro(metodo, arrParams[i][0], arrParams[i][1], i);
        }
    }
    return metodo;
}

// arrAtributos: array de arrays [nombre, tipo, visibilidad]
function CrearAtributos(elemento, arrAtributos) {
    var i;
    if (elemento == null || arrAtributos == null) return;
    for (i = 0; i < arrAtributos.length; i++) {
        ObtenerOCrearAtributo(elemento, arrAtributos[i][0], arrAtributos[i][1], arrAtributos[i][2]);
    }
}

// Crea literales de enum (se modelan como Attributes sin tipo)
function CrearLiteralesEnum(elemento, arrLiterales) {
    var i;
    if (elemento == null) return;
    for (i = 0; i < arrLiterales.length; i++) {
        ObtenerOCrearAtributo(elemento, arrLiterales[i], "", "Public");
    }
}

function ObtenerOCrearRelacion(origen, destino, tipo, nombreRol) {
    var i;
    try {
        if (origen == null || destino == null) return null;
        for (i = 0; i < origen.Connectors.Count; i++) {
            var c = origen.Connectors.GetAt(i);
            if (c.Type == tipo && c.SupplierID == destino.ElementID) {
                contadorExistentes++;
                return c;
            }
        }
        var nuevo = origen.Connectors.AddNew(nombreRol ? nombreRol : "", tipo);
        nuevo.SupplierID = destino.ElementID;
        nuevo.ClientID = origen.ElementID;
        nuevo.Update();
        origen.Connectors.Refresh();
        contadorCreados++;
        log("OK", "Relacion " + tipo + " creada: " + origen.Name + " -> " + destino.Name);
        return nuevo;
    } catch (e) {
        contadorErrores++;
        log("ERROR", "No se pudo crear la relacion " + tipo + " " + origen.Name + " -> " + destino.Name + ": " + e.description);
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

function main() {
    Session.Output("===============================================================");
    Session.Output("02_Clases_RFN1.js - inicio");
    Session.Output("===============================================================");

    var raiz = Repository.GetTreeSelectedPackage();
    if (raiz == null) {
        Session.Output("ERROR: no hay ningun paquete seleccionado en el arbol de proyecto. Seleccione el paquete raiz 'SIGAM' y vuelva a ejecutar.");
        return;
    }
    log("OK", "Paquete raiz: " + raiz.Name);

    var pkgClases = ObtenerOCrearPaquete(raiz, "Clases");
    if (pkgClases == null) { return; }

    // ===============================================================
    // BE - enums y entidades (compartidas + RFN1)
    // ===============================================================
    var beEstadoVehiculo = ObtenerOCrearElemento(pkgClases, "BE.EstadoVehiculoJP86", "Enumeration");
    CrearLiteralesEnum(beEstadoVehiculo, ["DISPONIBLE", "ALQUILADO", "EN_REVISION", "EN_REPARACION"]);

    var beCategoria = ObtenerOCrearElemento(pkgClases, "BE.Categoria_64PR", "Class");
    CrearAtributos(beCategoria, [
        ["Id", "int", "Private"],
        ["Nombre", "string", "Private"],
        ["Descripcion", "string", "Private"],
        ["TarifaDiaria", "decimal", "Private"],
        ["Activo", "bool", "Private"]
    ]);
    CrearMetodoConParams(beCategoria, "ToString", "string", "Public", []);

    var beVehiculo = ObtenerOCrearElemento(pkgClases, "BE.VehiculoJP86", "Class");
    CrearAtributos(beVehiculo, [
        ["Patente", "string", "Private"],
        ["Marca", "string", "Private"],
        ["Modelo", "string", "Private"],
        ["Categoria", "BE.Categoria_64PR", "Private"],
        ["Kilometraje", "int", "Private"],
        ["Estado", "BE.EstadoVehiculoJP86", "Private"],
        ["Activo", "bool", "Private"]
    ]);
    CrearMetodoConParams(beVehiculo, "ToString", "string", "Public", []);

    var beCliente = ObtenerOCrearElemento(pkgClases, "BE.ClienteJP86", "Class");
    CrearAtributos(beCliente, [
        ["DNI", "string", "Private"],
        ["Nombre", "string", "Private"],
        ["Apellido", "string", "Private"],
        ["Telefono", "string", "Private"],
        ["Activo", "bool", "Private"]
    ]);
    CrearMetodoConParams(beCliente, "ToString", "string", "Public", []);

    var beEstadoContrato = ObtenerOCrearElemento(pkgClases, "BE.EstadoContratoJP86", "Enumeration");
    CrearLiteralesEnum(beEstadoContrato, ["ACTIVO", "FACTURADO", "CERRADO"]);

    var beEstadoUnidadDevolucion = ObtenerOCrearElemento(pkgClases, "BE.EstadoUnidadDevolucionJP86", "Enumeration");
    CrearLiteralesEnum(beEstadoUnidadDevolucion, ["SIN_NOVEDADES", "CON_OBSERVACIONES"]);

    var beMetodoPago = ObtenerOCrearElemento(pkgClases, "BE.MetodoPagoJP86", "Enumeration");
    CrearLiteralesEnum(beMetodoPago, ["EFECTIVO", "TARJETA", "TRANSFERENCIA", "MERCADOPAGO"]);

    var beContrato = ObtenerOCrearElemento(pkgClases, "BE.ContratoJP86", "Class");
    CrearAtributos(beContrato, [
        ["NumeroContrato", "int", "Private"],
        ["Cliente", "BE.ClienteJP86", "Private"],
        ["Vehiculo", "BE.VehiculoJP86", "Private"],
        ["FechaInicio", "DateTime", "Private"],
        ["FechaFin", "DateTime", "Private"],
        ["TarifaDiaria", "decimal", "Private"],
        ["CargosAdicionales", "bool", "Private"],
        ["ImporteTotal", "decimal", "Private"],
        ["KilometrajeEntrega", "int", "Private"],
        ["KilometrajeRetorno", "int?", "Private"],
        ["EstadoUnidadDevolucion", "BE.EstadoUnidadDevolucionJP86?", "Private"],
        ["Estado", "BE.EstadoContratoJP86", "Private"]
    ]);
    CrearMetodoConParams(beContrato, "ToString", "string", "Public", []);

    var beFactura = ObtenerOCrearElemento(pkgClases, "BE.FacturaJP86", "Class");
    CrearAtributos(beFactura, [
        ["NumeroFactura", "int", "Private"],
        ["Contrato", "BE.ContratoJP86", "Private"],
        ["FechaFactura", "DateTime", "Private"],
        ["MetodoPago", "BE.MetodoPagoJP86", "Private"],
        ["MontoFactura", "decimal", "Private"]
    ]);
    CrearMetodoConParams(beFactura, "ToString", "string", "Public", []);

    var beResultadoPago = ObtenerOCrearElemento(pkgClases, "BE.ResultadoPagoJP86", "Class");
    CrearAtributos(beResultadoPago, [
        ["Exito", "bool", "Public"],
        ["TiempoProcesamiento", "TimeSpan", "Public"]
    ]);

    // ===============================================================
    // Servicios transversales preexistentes (stub minimo, ver nota de cabecera)
    // ===============================================================
    var dv = ObtenerOCrearElemento(pkgClases, "DV.DV_64PR", "Class", "servicio transversal");
    if (dv != null && dv.Notes == "") {
        dv.Notes = "Servicio transversal preexistente (Digito Verificador). Se modela unicamente el metodo que invocan las clases de RFN1/RFN2; su implementacion interna esta fuera de alcance de esta documentacion.";
        dv.Update();
    }
    CrearMetodoConParams(dv, "RecalcularTabla", "void", "Public", [["nombreTabla", "string"]]);

    var documentosRecibo = ObtenerOCrearElemento(pkgClases, "Documentos.GeneradorReciboContrato", "Class", "servicio transversal");
    if (documentosRecibo != null && documentosRecibo.Notes == "") {
        documentosRecibo.Notes = "Servicio transversal preexistente (patron estatico Generar/Imprimir + PrintDocument/PrintPreviewDialog, igual que GeneradorFactura y GeneradorReporteDesperfecto). No documentado en el diagrama de secuencia original de CUN-01 en Informe_RFN1.md; agregado por discrepancia detectada contra el codigo (FrmGenerarContratoJP86.btnImprimirRecibo_Click).";
        documentosRecibo.Update();
    }
    CrearMetodoConParams(documentosRecibo, "Imprimir", "void", "Public", [["contrato", "BE.ContratoJP86"]]);

    var documentosFactura = ObtenerOCrearElemento(pkgClases, "Documentos.GeneradorFactura", "Class", "servicio transversal");
    if (documentosFactura != null && documentosFactura.Notes == "") {
        documentosFactura.Notes = "Servicio transversal preexistente (mismo patron que GeneradorReciboContrato). Usado por FrmGenerarFacturaJP86.btnImprimirFactura_Click.";
        documentosFactura.Update();
    }
    CrearMetodoConParams(documentosFactura, "Imprimir", "void", "Public", [["factura", "BE.FacturaJP86"]]);

    // ===============================================================
    // DAL_64PR - unica clase de acceso a datos (Singleton)
    // ===============================================================
    var dal = ObtenerOCrearElemento(pkgClases, "DAL_64PR.Acceso", "Class", "singleton");
    CrearAtributos(dal, [
        ["Instancia", "DAL_64PR.Acceso", "Public"]
    ]);
    CrearMetodoConParams(dal, "leerQuery", "DataTable", "Public", [["query", "string"], ["parametro", "SqlParameter[]"], ["tipoComando", "CommandType"]]);
    CrearMetodoConParams(dal, "escribirQuery", "int", "Public", [["query", "string"], ["parametro", "SqlParameter[]"], ["tipoComando", "CommandType"]]);
    CrearMetodoConParams(dal, "leerEscalar", "object", "Public", [["query", "string"], ["parametro", "SqlParameter[]"], ["tipoComando", "CommandType"]]);

    // ===============================================================
    // Mapper - acceso a datos (compartidos + RFN1)
    // ===============================================================
    var mppCategoria = ObtenerOCrearElemento(pkgClases, "Mapper.mpp_categoria", "Class");
    CrearMetodoConParams(mppCategoria, "Listar", "List<BE.Categoria_64PR>", "Public", []);
    CrearMetodoConParams(mppCategoria, "Crear", "void", "Public", [["c", "BE.Categoria_64PR"]]);
    CrearMetodoConParams(mppCategoria, "Modificar", "void", "Public", [["c", "BE.Categoria_64PR"]]);
    CrearMetodoConParams(mppCategoria, "ActDesact", "void", "Public", [["c", "BE.Categoria_64PR"]]);

    var mppVehiculo = ObtenerOCrearElemento(pkgClases, "Mapper.mpp_vehiculo", "Class");
    CrearMetodoConParams(mppVehiculo, "Listar", "List<BE.VehiculoJP86>", "Public", []);
    CrearMetodoConParams(mppVehiculo, "Crear", "void", "Public", [["v", "BE.VehiculoJP86"]]);
    CrearMetodoConParams(mppVehiculo, "Modificar", "void", "Public", [["v", "BE.VehiculoJP86"]]);
    CrearMetodoConParams(mppVehiculo, "CambiarEstado", "void", "Public", [["patente", "string"], ["estado", "BE.EstadoVehiculoJP86"], ["kilometraje", "int?"], ["activo", "bool?"]]);
    CrearMetodoConParams(mppVehiculo, "ActDesact", "void", "Public", [["v", "BE.VehiculoJP86"]]);

    var mppCliente = ObtenerOCrearElemento(pkgClases, "Mapper.mpp_cliente", "Class");
    CrearMetodoConParams(mppCliente, "Listar", "List<BE.ClienteJP86>", "Public", []);
    CrearMetodoConParams(mppCliente, "Crear", "void", "Public", [["c", "BE.ClienteJP86"]]);
    CrearMetodoConParams(mppCliente, "Modificar", "void", "Public", [["c", "BE.ClienteJP86"]]);
    CrearMetodoConParams(mppCliente, "ActDesact", "void", "Public", [["c", "BE.ClienteJP86"]]);

    var mppContrato = ObtenerOCrearElemento(pkgClases, "Mapper.mpp_contrato", "Class");
    CrearMetodoConParams(mppContrato, "BuscarUnidadesDisponibles", "List<BE.VehiculoJP86>", "Public", [["idCategoria", "int?"], ["marca", "string"], ["kilometrajeMaximo", "int?"]]);
    CrearMetodoConParams(mppContrato, "Crear", "int", "Public", [["c", "BE.ContratoJP86"]]);
    CrearMetodoConParams(mppContrato, "ListarActivos", "List<BE.ContratoJP86>", "Public", []);
    CrearMetodoConParams(mppContrato, "ListarFacturados", "List<BE.ContratoJP86>", "Public", []);
    CrearMetodoConParams(mppContrato, "CerrarPorDevolucion", "void", "Public", [["numeroContrato", "int"], ["kilometrajeRetorno", "int"], ["estadoUnidad", "BE.EstadoUnidadDevolucionJP86"]]);

    var mppFactura = ObtenerOCrearElemento(pkgClases, "Mapper.mpp_factura", "Class");
    CrearMetodoConParams(mppFactura, "Crear", "int", "Public", [["f", "BE.FacturaJP86"]]);

    // ===============================================================
    // BLL_64PR - logica de negocio (compartida + RFN1)
    // ===============================================================
    var bllCategoria = ObtenerOCrearElemento(pkgClases, "BLL_64PR.Categoria_64PR", "Class");
    CrearAtributos(bllCategoria, [
        ["mpp", "Mapper.mpp_categoria", "Private"],
        ["recalculador", "DV.DV_64PR", "Private"]
    ]);
    CrearMetodoConParams(bllCategoria, "Listar", "List<BE.Categoria_64PR>", "Public", []);
    CrearMetodoConParams(bllCategoria, "Crear", "void", "Public", [["c", "BE.Categoria_64PR"]]);
    CrearMetodoConParams(bllCategoria, "Modificar", "void", "Public", [["c", "BE.Categoria_64PR"]]);
    CrearMetodoConParams(bllCategoria, "ActDesact", "void", "Public", [["c", "BE.Categoria_64PR"]]);

    var bllVehiculo = ObtenerOCrearElemento(pkgClases, "BLL_64PR.VehiculoJP86", "Class");
    CrearAtributos(bllVehiculo, [
        ["mpp", "Mapper.mpp_vehiculo", "Private"],
        ["recalculador", "DV.DV_64PR", "Private"]
    ]);
    CrearMetodoConParams(bllVehiculo, "Listar", "List<BE.VehiculoJP86>", "Public", []);
    CrearMetodoConParams(bllVehiculo, "Crear", "void", "Public", [["v", "BE.VehiculoJP86"]]);
    CrearMetodoConParams(bllVehiculo, "Modificar", "void", "Public", [["v", "BE.VehiculoJP86"]]);
    var metCambiarEstado = CrearMetodoConParams(bllVehiculo, "CambiarEstado", "void", "Public", [["patente", "string"], ["estado", "BE.EstadoVehiculoJP86"], ["nuevoKilometraje", "int?"]]);
    if (metCambiarEstado != null && metCambiarEstado.Notes == "") {
        metCambiarEstado.Notes = "nuevoKilometraje = null por defecto. Regla de negocio unica: si estado==EN_REVISION, calcula nuevoActivo=false (Activo se destilda automaticamente); en cualquier otro estado no toca Activo. Reutilizado tanto por el boton 'Cambiar Estado' del maestro de Vehiculos como por CUN-04 (RegistrarDevolucion).";
        metCambiarEstado.Update();
    }
    CrearMetodoConParams(bllVehiculo, "ActDesact", "void", "Public", [["v", "BE.VehiculoJP86"]]);

    var bllCliente = ObtenerOCrearElemento(pkgClases, "BLL_64PR.ClienteJP86", "Class");
    CrearAtributos(bllCliente, [
        ["mpp", "Mapper.mpp_cliente", "Private"],
        ["recalculador", "DV.DV_64PR", "Private"]
    ]);
    CrearMetodoConParams(bllCliente, "Listar", "List<BE.ClienteJP86>", "Public", []);
    CrearMetodoConParams(bllCliente, "Crear", "void", "Public", [["c", "BE.ClienteJP86"]]);
    CrearMetodoConParams(bllCliente, "Modificar", "void", "Public", [["c", "BE.ClienteJP86"]]);
    CrearMetodoConParams(bllCliente, "ActDesact", "void", "Public", [["c", "BE.ClienteJP86"]]);

    var bllContrato = ObtenerOCrearElemento(pkgClases, "BLL_64PR.ContratoJP86", "Class");
    CrearAtributos(bllContrato, [
        ["PorcentajeCargosAdicionales", "decimal", "Public"],
        ["mpp", "Mapper.mpp_contrato", "Private"],
        ["mppFactura", "Mapper.mpp_factura", "Private"],
        ["recalculador", "DV.DV_64PR", "Private"]
    ]);
    CrearMetodoConParams(bllContrato, "BuscarUnidadesDisponibles", "List<BE.VehiculoJP86>", "Public", [["idCategoria", "int?"], ["marca", "string"], ["kilometrajeMaximo", "int?"]]);
    CrearMetodoConParams(bllContrato, "CalcularImporteTotal", "decimal", "Public", [["tarifaDiaria", "decimal"], ["fechaInicio", "DateTime"], ["fechaFin", "DateTime"], ["cargosAdicionales", "bool"]]);
    CrearMetodoConParams(bllContrato, "GenerarContrato", "BE.ContratoJP86", "Public", [["cliente", "BE.ClienteJP86"], ["vehiculo", "BE.VehiculoJP86"], ["fechaInicio", "DateTime"], ["fechaFin", "DateTime"], ["cargosAdicionales", "bool"]]);
    CrearMetodoConParams(bllContrato, "ListarActivos", "List<BE.ContratoJP86>", "Public", []);
    CrearMetodoConParams(bllContrato, "ListarFacturados", "List<BE.ContratoJP86>", "Public", []);
    CrearMetodoConParams(bllContrato, "GenerarFactura", "Task<BE.FacturaJP86>", "Public", [["contrato", "BE.ContratoJP86"], ["metodoPago", "BE.MetodoPagoJP86"]]);
    CrearMetodoConParams(bllContrato, "RegistrarDevolucion", "void", "Public", [["contrato", "BE.ContratoJP86"], ["kilometrajeRetorno", "int"], ["estadoUnidad", "BE.EstadoUnidadDevolucionJP86"]]);

    // ---------------------------------------------------------------
    // Strategy Pattern del pago simulado (CUN-03, seccion 9 del informe)
    // ---------------------------------------------------------------
    var iEstrategiaPago = ObtenerOCrearElemento(pkgClases, "BLL_64PR.IEstrategiaPagoJP86", "Interface");
    CrearMetodoConParams(iEstrategiaPago, "ProcesarPago", "Task<BE.ResultadoPagoJP86>", "Public", [["monto", "decimal"]]);

    var pagoEfectivo = ObtenerOCrearElemento(pkgClases, "BLL_64PR.PagoEfectivoJP86", "Class");
    CrearAtributos(pagoEfectivo, [["DelayMinMs", "int = 200", "Private"], ["DelayMaxMs", "int = 500", "Private"]]);
    CrearMetodoConParams(pagoEfectivo, "ProcesarPago", "Task<BE.ResultadoPagoJP86>", "Public", [["monto", "decimal"]]);

    var pagoTarjeta = ObtenerOCrearElemento(pkgClases, "BLL_64PR.PagoTarjetaJP86", "Class");
    CrearAtributos(pagoTarjeta, [["DelayMinMs", "int = 800", "Private"], ["DelayMaxMs", "int = 1500", "Private"]]);
    CrearMetodoConParams(pagoTarjeta, "ProcesarPago", "Task<BE.ResultadoPagoJP86>", "Public", [["monto", "decimal"]]);

    var pagoTransferencia = ObtenerOCrearElemento(pkgClases, "BLL_64PR.PagoTransferenciaJP86", "Class");
    CrearAtributos(pagoTransferencia, [["DelayMinMs", "int = 1500", "Private"], ["DelayMaxMs", "int = 3000", "Private"]]);
    CrearMetodoConParams(pagoTransferencia, "ProcesarPago", "Task<BE.ResultadoPagoJP86>", "Public", [["monto", "decimal"]]);

    var pagoMercadoPago = ObtenerOCrearElemento(pkgClases, "BLL_64PR.PagoMercadoPagoJP86", "Class");
    CrearAtributos(pagoMercadoPago, [["DelayMinMs", "int = 600", "Private"], ["DelayMaxMs", "int = 1200", "Private"]]);
    CrearMetodoConParams(pagoMercadoPago, "ProcesarPago", "Task<BE.ResultadoPagoJP86>", "Public", [["monto", "decimal"]]);

    var fabricaEstrategiaPago = ObtenerOCrearElemento(pkgClases, "BLL_64PR.FabricaEstrategiaPagoJP86", "Class", "static");
    CrearMetodoConParams(fabricaEstrategiaPago, "Obtener", "BLL_64PR.IEstrategiaPagoJP86", "Public", [["metodoPago", "BE.MetodoPagoJP86"]]);

    // ===============================================================
    // Relaciones
    // ===============================================================
    // -- BE: asociaciones de datos --
    ObtenerOCrearRelacion(beVehiculo, beCategoria, "Association", "Categoria");
    ObtenerOCrearRelacion(beVehiculo, beEstadoVehiculo, "Dependency", "");
    ObtenerOCrearRelacion(beContrato, beCliente, "Association", "Cliente");
    ObtenerOCrearRelacion(beContrato, beVehiculo, "Association", "Vehiculo");
    ObtenerOCrearRelacion(beContrato, beEstadoContrato, "Dependency", "");
    ObtenerOCrearRelacion(beContrato, beEstadoUnidadDevolucion, "Dependency", "");
    ObtenerOCrearRelacion(beFactura, beContrato, "Association", "Contrato");
    ObtenerOCrearRelacion(beFactura, beMetodoPago, "Dependency", "");

    // -- Mapper -> DAL --
    ObtenerOCrearRelacion(mppCategoria, dal, "Dependency", "");
    ObtenerOCrearRelacion(mppVehiculo, dal, "Dependency", "");
    ObtenerOCrearRelacion(mppCliente, dal, "Dependency", "");
    ObtenerOCrearRelacion(mppContrato, dal, "Dependency", "");
    ObtenerOCrearRelacion(mppFactura, dal, "Dependency", "");

    // -- Mapper -> BE (tipos que devuelve/recibe) --
    ObtenerOCrearRelacion(mppContrato, beContrato, "Dependency", "");
    ObtenerOCrearRelacion(mppContrato, beVehiculo, "Dependency", "");
    ObtenerOCrearRelacion(mppFactura, beFactura, "Dependency", "");

    // -- BLL -> Mapper (campo mpp) y BLL -> DV (campo recalculador) --
    ObtenerOCrearRelacion(bllCategoria, mppCategoria, "Association", "mpp");
    ObtenerOCrearRelacion(bllCategoria, dv, "Association", "recalculador");
    ObtenerOCrearRelacion(bllVehiculo, mppVehiculo, "Association", "mpp");
    ObtenerOCrearRelacion(bllVehiculo, dv, "Association", "recalculador");
    ObtenerOCrearRelacion(bllCliente, mppCliente, "Association", "mpp");
    ObtenerOCrearRelacion(bllCliente, dv, "Association", "recalculador");
    ObtenerOCrearRelacion(bllContrato, mppContrato, "Association", "mpp");
    ObtenerOCrearRelacion(bllContrato, mppFactura, "Association", "mppFactura");
    ObtenerOCrearRelacion(bllContrato, dv, "Association", "recalculador");

    // -- BLL_64PR.ContratoJP86 orquesta BLL_64PR.VehiculoJP86.CambiarEstado --
    ObtenerOCrearRelacion(bllContrato, bllVehiculo, "Dependency", "reutiliza CambiarEstado");

    // -- Strategy pattern --
    ObtenerOCrearRelacion(bllContrato, fabricaEstrategiaPago, "Dependency", "");
    ObtenerOCrearRelacion(fabricaEstrategiaPago, iEstrategiaPago, "Dependency", "crea");
    ObtenerOCrearRelacion(pagoEfectivo, iEstrategiaPago, "Realization", "");
    ObtenerOCrearRelacion(pagoTarjeta, iEstrategiaPago, "Realization", "");
    ObtenerOCrearRelacion(pagoTransferencia, iEstrategiaPago, "Realization", "");
    ObtenerOCrearRelacion(pagoMercadoPago, iEstrategiaPago, "Realization", "");
    ObtenerOCrearRelacion(iEstrategiaPago, beResultadoPago, "Dependency", "");

    // ===============================================================
    // Diagrama 1: DCL - RFN1 Modelo General
    // ===============================================================
    var diagGeneral = ObtenerOCrearDiagrama(pkgClases, "DCL - RFN1 Modelo General", "Class");
    if (diagGeneral != null) {
        // Fila 1: BE
        AgregarObjetoADiagrama(diagGeneral, beCategoria, 40, -40, 260, -180);
        AgregarObjetoADiagrama(diagGeneral, beVehiculo, 320, -40, 560, -220);
        AgregarObjetoADiagrama(diagGeneral, beEstadoVehiculo, 620, -40, 820, -160);
        AgregarObjetoADiagrama(diagGeneral, beCliente, 880, -40, 1120, -180);
        AgregarObjetoADiagrama(diagGeneral, beContrato, 320, -280, 620, -520);
        AgregarObjetoADiagrama(diagGeneral, beFactura, 680, -280, 940, -440);
        AgregarObjetoADiagrama(diagGeneral, beEstadoContrato, 1000, -280, 1200, -400);
        AgregarObjetoADiagrama(diagGeneral, beEstadoUnidadDevolucion, 1000, -440, 1200, -540);
        AgregarObjetoADiagrama(diagGeneral, beMetodoPago, 680, -480, 940, -600);

        // Fila 2: Mapper
        AgregarObjetoADiagrama(diagGeneral, mppCategoria, 40, -600, 260, -700);
        AgregarObjetoADiagrama(diagGeneral, mppVehiculo, 320, -600, 560, -720);
        AgregarObjetoADiagrama(diagGeneral, mppCliente, 620, -600, 840, -700);
        AgregarObjetoADiagrama(diagGeneral, mppContrato, 900, -600, 1160, -760);
        AgregarObjetoADiagrama(diagGeneral, mppFactura, 1220, -600, 1440, -680);

        // Fila 3: DAL / DV / Documentos
        AgregarObjetoADiagrama(diagGeneral, dal, 320, -800, 560, -920);
        AgregarObjetoADiagrama(diagGeneral, dv, 620, -800, 820, -900);
        AgregarObjetoADiagrama(diagGeneral, documentosRecibo, 880, -800, 1140, -900);
        AgregarObjetoADiagrama(diagGeneral, documentosFactura, 1200, -800, 1460, -900);

        // Fila 4: BLL
        AgregarObjetoADiagrama(diagGeneral, bllCategoria, 40, -1000, 280, -1120);
        AgregarObjetoADiagrama(diagGeneral, bllVehiculo, 340, -1000, 600, -1140);
        AgregarObjetoADiagrama(diagGeneral, bllCliente, 660, -1000, 900, -1120);
        AgregarObjetoADiagrama(diagGeneral, bllContrato, 960, -1000, 1300, -1200);

        try {
            Repository.ReloadDiagram(diagGeneral.DiagramID);
        } catch (e) {
            contadorErrores++;
            log("ERROR", "No se pudo recargar 'DCL - RFN1 Modelo General': " + e.description);
        }
    }

    // ===============================================================
    // Diagrama 2: DCL - RFN1 CUN-03 Estrategia de Pago
    // ===============================================================
    var diagPago = ObtenerOCrearDiagrama(pkgClases, "DCL - RFN1 CUN-03 Estrategia de Pago", "Class");
    if (diagPago != null) {
        AgregarObjetoADiagrama(diagPago, bllContrato, 40, -40, 340, -160);
        AgregarObjetoADiagrama(diagPago, fabricaEstrategiaPago, 400, -40, 660, -140);
        AgregarObjetoADiagrama(diagPago, iEstrategiaPago, 400, -220, 660, -320);
        AgregarObjetoADiagrama(diagPago, pagoEfectivo, 40, -400, 280, -520);
        AgregarObjetoADiagrama(diagPago, pagoTarjeta, 340, -400, 580, -520);
        AgregarObjetoADiagrama(diagPago, pagoTransferencia, 640, -400, 880, -520);
        AgregarObjetoADiagrama(diagPago, pagoMercadoPago, 940, -400, 1180, -520);
        AgregarObjetoADiagrama(diagPago, beResultadoPago, 720, -220, 1000, -320);

        try {
            Repository.ReloadDiagram(diagPago.DiagramID);
        } catch (e) {
            contadorErrores++;
            log("ERROR", "No se pudo recargar 'DCL - RFN1 CUN-03 Estrategia de Pago': " + e.description);
        }
    }

    try {
        Repository.RefreshModelView(pkgClases.PackageGUID);
    } catch (e) {
        // no fatal
    }

    Session.Output("===============================================================");
    Session.Output("02_Clases_RFN1.js - RESUMEN: creados=" + contadorCreados + " existentes=" + contadorExistentes + " errores=" + contadorErrores);
    Session.Output("===============================================================");
}

main();
