/*
===============================================================================
 01_CasosDeUso.js
===============================================================================
 Proyecto: SIGAM (FleetDrive S.A.) - Trabajo de Diploma UAI

 QUE CREA:
   - Paquete "Casos de Uso" debajo del paquete raiz seleccionado en el arbol
     de proyecto (se espera que sea el paquete "SIGAM").
   - 2 actores: "Agente Comercial" y "Responsable de Flota".
   - 9 casos de uso: CUN-01 a CUN-09 (RFN1: alquileres: CUN-01 a CUN-04;
     RFN2: mantenimiento: CUN-05 a CUN-09), con el campo Notes completo
     (actor, objetivo, precondiciones, flujo principal, alternativos).
   - Relaciones:
       * Asociaciones Actor -> Caso de Uso (solo para los casos disparados
         directamente por un actor; los incluidos/extendidos que SIEMPRE
         se disparan como parte de otro caso de uso no llevan asociacion
         propia: CUN-02, CUN-07, CUN-09).
       * <<extend>>  CUN-02 -> CUN-01   (el caso que extiende apunta al base)
       * <<include>> CUN-03 -> CUN-04   (inclusion diferida en el tiempo,
                                          ya justificada y cerrada con el
                                          desarrollador - ver Notes de CUN-03)
       * <<include>> CUN-06 -> CUN-07   (el caso base apunta al incluido)
       * <<include>> CUN-08 -> CUN-09   (el caso base apunta al incluido)
   - 2 diagramas de casos de uso: "DCU - RFN1 Alquileres" y
     "DCU - RFN2 Mantenimiento".

 PRERREQUISITOS:
   - Tener seleccionado en el arbol de proyecto de EA el paquete raiz
     "SIGAM" (o el que se use como raiz del modelo) antes de correr el
     script.
   - Ninguno de los demas scripts es prerrequisito de este (es el primero).

 ORDEN DE EJECUCION: 01 (este) -> 02_Clases_RFN1 -> 02_Clases_RFN2 ->
                      03_Secuencia_CUN01 ... 03_Secuencia_CUN09 -> 04_DER (opcional)

 IDEMPOTENCIA: el script busca por nombre antes de crear paquetes, actores,
 casos de uso, relaciones y diagramas; se puede re-ejecutar sin duplicar.
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

function ObtenerOCrearElemento(paquete, nombre, tipo) {
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

function EstablecerNotas(elemento, texto) {
    try {
        if (elemento == null) return;
        if (elemento.Notes != texto) {
            elemento.Notes = texto;
            elemento.Update();
        }
    } catch (e) {
        contadorErrores++;
        log("ERROR", "No se pudieron establecer las notas de '" + elemento.Name + "': " + e.description);
    }
}

function ObtenerOCrearAsociacion(actor, casoDeUso) {
    var i;
    try {
        for (i = 0; i < actor.Connectors.Count; i++) {
            var c = actor.Connectors.GetAt(i);
            if (c.Type == "Association" && c.SupplierID == casoDeUso.ElementID) {
                contadorExistentes++;
                return c;
            }
        }
        var nuevo = actor.Connectors.AddNew("", "Association");
        nuevo.SupplierID = casoDeUso.ElementID;
        nuevo.ClientID = actor.ElementID;
        nuevo.Update();
        actor.Connectors.Refresh();
        contadorCreados++;
        log("OK", "Asociacion creada: " + actor.Name + " -> " + casoDeUso.Name);
        return nuevo;
    } catch (e) {
        contadorErrores++;
        log("ERROR", "No se pudo crear la asociacion " + actor.Name + " -> " + casoDeUso.Name + ": " + e.description);
        return null;
    }
}

function ObtenerOCrearRelacionUC(origen, destino, tipo) {
    var i;
    try {
        for (i = 0; i < origen.Connectors.Count; i++) {
            var c = origen.Connectors.GetAt(i);
            if (c.Type == tipo && c.SupplierID == destino.ElementID) {
                contadorExistentes++;
                return c;
            }
        }
        var nuevo = origen.Connectors.AddNew("", tipo);
        nuevo.SupplierID = destino.ElementID;
        nuevo.ClientID = origen.ElementID;
        nuevo.Update();
        origen.Connectors.Refresh();
        contadorCreados++;
        log("OK", "Relacion <<" + tipo + ">> creada: " + origen.Name + " -> " + destino.Name);
        return nuevo;
    } catch (e) {
        contadorErrores++;
        log("ERROR", "No se pudo crear la relacion <<" + tipo + ">> " + origen.Name + " -> " + destino.Name + ": " + e.description);
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
    Session.Output("01_CasosDeUso.js - inicio");
    Session.Output("===============================================================");

    var raiz = Repository.GetTreeSelectedPackage();
    if (raiz == null) {
        Session.Output("ERROR: no hay ningun paquete seleccionado en el arbol de proyecto. Seleccione el paquete raiz 'SIGAM' y vuelva a ejecutar.");
        return;
    }
    log("OK", "Paquete raiz: " + raiz.Name);

    var pkgCasosDeUso = ObtenerOCrearPaquete(raiz, "Casos de Uso");
    if (pkgCasosDeUso == null) { return; }

    // -----------------------------------------------------------------
    // Actores
    // -----------------------------------------------------------------
    var actorAgente = ObtenerOCrearElemento(pkgCasosDeUso, "Agente Comercial", "Actor");
    var actorFlota = ObtenerOCrearElemento(pkgCasosDeUso, "Responsable de Flota", "Actor");
    EstablecerNotas(actorFlota, "Rol conceptual definido por la ECU. Al momento de este script el rol 'Responsable de Flota' todavia no fue dado de alta en Sesion/Rol_64PR: las patentes de RFN2 (Asignar criticidad, Determinar modalidad, Registrar acreditacion) estan asignadas unicamente al rol Administrador. Toda la logica de permisos valida por patente (Sesion.Patentes_64PR + Rol_64PR.TienePermiso), nunca por nombre de rol, asi que no requiere cambios de codigo cuando se cree el rol.");

    // -----------------------------------------------------------------
    // Casos de uso RFN1 (CUN-01 a CUN-04)
    // -----------------------------------------------------------------
    var cun01 = ObtenerOCrearElemento(pkgCasosDeUso, "CUN-01 Generar Contrato de Alquiler", "UseCase");
    // Notas = ECU de Diagramas_RFN1_CUN01-04.md (unica version; mantener sincronizadas)
    EstablecerNotas(cun01,
        "ID y Nombre: CUN-01 \"Generar Contrato de Alquiler\"\n" +
        "Actor principal: Agente Comercial\n" +
        "Precondiciones: El Agente Comercial inicio sesion y posee el permiso \"Generar contrato\".\n" +
        "Punto de extension: CUN-02 \"Registrar Cliente\", en el paso 5 (solo si el cliente no esta registrado).\n" +
        "Postcondiciones: Queda registrado un contrato en estado Activo, con numero asignado, tarifa diaria vigente, importe total y kilometraje de entrega. El vehiculo seleccionado queda en estado \"Alquilado\".\n" +
        "Escenario principal:\n" +
        "1. El Agente Comercial selecciona \"Alquileres\" > \"Generar contrato\".\n" +
        "2. El sistema muestra el formulario con las categorias para filtrar, todas las unidades disponibles (Patente, Marca, Modelo, Categoria, Tarifa diaria, Kilometraje) y los clientes activos.\n" +
        "3. Opcionalmente, el Agente Comercial filtra las unidades por categoria, marca y/o kilometraje maximo.\n" +
        "4. El sistema muestra las unidades disponibles que cumplen los criterios.\n" +
        "5. El Agente Comercial selecciona una unidad y un cliente.\n" +
        "6. El Agente Comercial ingresa la fecha de inicio y la fecha de fin, e indica si se aplican cargos adicionales.\n" +
        "7. El sistema calcula el importe total (tarifa diaria x dias + 10% de cargos adicionales opcionales) y lo muestra. Lo recalcula cada vez que cambian la unidad, las fechas o los cargos.\n" +
        "8. El Agente Comercial confirma el contrato.\n" +
        "9. El sistema valida las fechas, el plazo y el importe, verifica que la unidad siga disponible y el cliente activo, genera el numero de contrato, registra el kilometraje de entrega (el kilometraje actual de la unidad), guarda el contrato, actualiza el estado del vehiculo a \"Alquilado\", informa el numero de contrato y deja el formulario listo para un nuevo contrato.\n" +
        "10. Opcionalmente, el Agente Comercial imprime el recibo del contrato.\n" +
        "Escenario alternativo:\n" +
        "4.1 No hay unidades disponibles para los criterios ingresados. El sistema muestra un mensaje y vuelve al paso 3.\n" +
        "5.1 El cliente no esta registrado. Se extiende con CUN-02 \"Registrar Cliente\". Al finalizar, el cliente queda seleccionado y el flujo continua en el paso 6.\n" +
        "8.1 El Agente Comercial cancela antes de confirmar. El sistema descarta los datos ingresados y deja el formulario limpio.\n" +
        "9.1 No hay una unidad seleccionada. El sistema muestra un mensaje y vuelve al paso 5.\n" +
        "9.2 No hay un cliente seleccionado. El sistema muestra un mensaje y vuelve al paso 5.\n" +
        "9.3 La fecha de fin es anterior a la de inicio. El sistema muestra un mensaje y vuelve al paso 6.\n" +
        "9.4 La fecha de inicio es anterior a hoy. El sistema muestra un mensaje y vuelve al paso 6.\n" +
        "9.5 El plazo del contrato supera los 90 dias. El sistema muestra un mensaje y vuelve al paso 6.\n" +
        "9.6 El importe total supera el maximo permitido. El sistema muestra un mensaje y vuelve al paso 6.\n" +
        "9.7 La unidad ya no esta disponible (la alquilo otro usuario) o el cliente dejo de estar activo. El sistema no genera el contrato, muestra un mensaje, actualiza las listas de unidades y clientes, y vuelve al paso 5.\n" +
        "Nota: La disponibilidad de una unidad depende de su estado (\"Disponible\"), no de las fechas del contrato. Las fechas intervienen en el calculo del importe y en las validaciones de 9.3 a 9.5. El plazo maximo de 90 dias corresponde a alquiler de corto plazo.");

    var cun02 = ObtenerOCrearElemento(pkgCasosDeUso, "CUN-02 Registrar Cliente", "UseCase");
    // Notas = ECU de Diagramas_RFN1_CUN01-04.md (unica version; mantener sincronizadas)
    EstablecerNotas(cun02,
        "ID y Nombre: CUN-02 \"Registrar Cliente\"\n" +
        "Actor principal: Agente Comercial\n" +
        "Precondiciones: El Agente Comercial esta ejecutando CUN-01 y el cliente no figura en la lista de clientes registrados.\n" +
        "Punto de extension: No aplica (es un caso de uso que extiende a CUN-01).\n" +
        "Postcondiciones: Queda registrado un nuevo cliente, que queda seleccionado para continuar con CUN-01.\n" +
        "Escenario principal:\n" +
        "1. El Agente Comercial selecciona \"Cliente nuevo\".\n" +
        "2. El sistema muestra los campos para completar los datos del cliente.\n" +
        "3. El Agente Comercial ingresa DNI, nombre, apellido y telefono.\n" +
        "4. El Agente Comercial presiona \"Guardar cliente nuevo\".\n" +
        "5. El sistema valida los datos y verifica que el DNI no este registrado.\n" +
        "6. El sistema registra el cliente, actualiza la lista de clientes y lo deja seleccionado.\n" +
        "Escenario alternativo:\n" +
        "5.1 Algun dato no es valido (DNI de 7 u 8 digitos, nombre y apellido obligatorios, telefono con formato valido). El sistema muestra un mensaje que indica el dato incorrecto y vuelve al paso 3.\n" +
        "5.2 El DNI ya esta registrado. El sistema muestra un mensaje y vuelve al paso 3.");

    var cun03 = ObtenerOCrearElemento(pkgCasosDeUso, "CUN-03 Generar Factura", "UseCase");
    // Notas = ECU de Diagramas_RFN1_CUN01-04.md (unica version; mantener sincronizadas)
    EstablecerNotas(cun03,
        "ID y Nombre: CUN-03 \"Generar Factura\"\n" +
        "Actor principal: Agente Comercial\n" +
        "Precondiciones: El Agente Comercial posee el permiso \"Generar factura\" y existe al menos un contrato en estado Activo (CUN-01).\n" +
        "Punto de extension: No aplica (CUN-04 se incluye siempre, no es condicional).\n" +
        "Postcondiciones: Queda registrada la factura con numero, fecha de emision, metodo de pago y monto. El contrato pasa a estado Facturado y queda habilitado para CUN-04 \"Registrar Devolucion de Vehiculo\".\n" +
        "Escenario principal:\n" +
        "1. El Agente Comercial selecciona \"Alquileres\" > \"Generar factura\".\n" +
        "2. El sistema muestra los contratos sin cobrar (numero, cliente, vehiculo, importe total) y los metodos de pago (Efectivo, Tarjeta, Transferencia, Mercado Pago), que permanecen deshabilitados hasta que se selecciona un contrato.\n" +
        "3. El Agente Comercial selecciona un contrato.\n" +
        "4. El sistema muestra el importe total y habilita los metodos de pago.\n" +
        "5. El Agente Comercial selecciona el metodo de pago indicado por el cliente.\n" +
        "6. El sistema procesa el pago, genera la factura, pasa el contrato a estado Facturado, informa el resultado y actualiza la lista de contratos. Mientras procesa el pago, no permite seleccionar otro contrato ni salir de la pantalla.\n" +
        "7. Opcionalmente, el Agente Comercial imprime la factura.\n" +
        "Escenario alternativo:\n" +
        "6.1 El pago no es aprobado. El sistema no genera la factura, el contrato sigue en estado Activo, muestra un mensaje, actualiza la lista y vuelve al paso 3.\n" +
        "6.2 El contrato ya no se encuentra en estado Activo (por ejemplo, porque lo facturo otro usuario). El sistema muestra un mensaje, actualiza la lista y vuelve al paso 3.\n" +
        "Nota de alcance: El procesamiento del pago se simula por metodo de pago, con un tiempo de procesamiento propio de cada uno. En esta version la simulacion siempre aprueba: el alternativo 6.1 esta implementado y se activa en cuanto un metodo de pago informe un rechazo. La integracion con entidades de pago externas queda fuera del alcance.");

    var cun04 = ObtenerOCrearElemento(pkgCasosDeUso, "CUN-04 Registrar Devolucion de Vehiculo", "UseCase");
    // Notas = ECU de Diagramas_RFN1_CUN01-04.md (unica version; mantener sincronizadas)
    EstablecerNotas(cun04,
        "ID y Nombre: CUN-04 \"Registrar Devolucion de Vehiculo\"\n" +
        "Actor principal: Agente Comercial\n" +
        "Precondiciones: El Agente Comercial posee el permiso \"Registrar devolucion\" y existe al menos un contrato en estado Facturado (CUN-03), con el vehiculo en estado \"Alquilado\".\n" +
        "Punto de extension: No aplica (es un caso de uso incluido en CUN-03).\n" +
        "Postcondiciones: El contrato queda Cerrado, con el kilometraje de retorno y el estado de la unidad registrados. El kilometraje del vehiculo se actualiza con el de retorno. El vehiculo pasa a \"Disponible\" o, si se registraron observaciones, a \"En revision\" e inactivo. En ese caso queda inactivo hasta que se lo reactiva manualmente desde el maestro de Vehiculos (control de calidad antes de volver a la flota), aun despues de liberarse en el circuito de mantenimiento.\n" +
        "Escenario principal:\n" +
        "1. El Agente Comercial selecciona \"Alquileres\" > \"Registrar devolucion\".\n" +
        "2. El sistema muestra los contratos facturados (numero de contrato, cliente, vehiculo, fecha de inicio, fecha de fin, kilometraje de entrega).\n" +
        "3. Opcionalmente, el Agente Comercial busca por numero de contrato o patente.\n" +
        "4. El Agente Comercial selecciona el contrato.\n" +
        "5. El sistema muestra los datos del contrato, incluido el kilometraje de entrega.\n" +
        "6. Opcionalmente, el Agente Comercial imprime el recibo del contrato.\n" +
        "7. El Agente Comercial ingresa el kilometraje de retorno y el estado de la unidad (sin novedades / con observaciones).\n" +
        "8. El Agente Comercial confirma la devolucion.\n" +
        "9. El sistema valida el kilometraje, cierra el contrato, actualiza el kilometraje y el estado del vehiculo (\"Disponible\" o \"En revision\" segun el paso 7), informa el resultado y actualiza la lista de contratos.\n" +
        "Escenario alternativo:\n" +
        "3.1 La busqueda no encuentra coincidencias. La lista queda vacia y el flujo vuelve al paso 3.\n" +
        "9.1 El kilometraje de retorno es menor al de entrega. El sistema muestra un mensaje y vuelve al paso 7.\n" +
        "9.2 El contrato ya no se encuentra en estado Facturado (por ejemplo, porque lo cerro otro usuario). El sistema no modifica el vehiculo, muestra un mensaje, actualiza la lista y vuelve al paso 4.");

    // -----------------------------------------------------------------
    // Casos de uso RFN2 (CUN-05 a CUN-09)
    // -----------------------------------------------------------------
    var cun05 = ObtenerOCrearElemento(pkgCasosDeUso, "CUN-05 Registrar Reporte de Desperfecto", "UseCase");
    EstablecerNotas(cun05,
        "Actor: Agente Comercial.\n" +
        "Objetivo: registrar un reporte de desperfecto sobre un vehiculo que se encuentra en revision.\n" +
        "Precondiciones: usuario autenticado con la patente 'Registrar reporte'. Debe existir al menos un vehiculo en estado EN_REVISION.\n" +
        "Flujo principal:\n" +
        "1. El sistema muestra los vehiculos EN_REVISION.\n" +
        "2. El Agente Comercial selecciona un vehiculo.\n" +
        "3. Ingresa la descripcion del problema (texto libre) y la fuente (Cliente / Inspeccion interna).\n" +
        "4. Confirma.\n" +
        "5. El sistema genera el reporte (Estado=PENDIENTE) y habilita el boton 'Imprimir Reporte' (Documentos.GeneradorReporteDesperfecto).\n" +
        "Flujos alternativos:\n" +
        "Sin vehiculos EN_REVISION: mensaje informativo, el resto del formulario queda deshabilitado.");

    var cun06 = ObtenerOCrearElemento(pkgCasosDeUso, "CUN-06 Asignar Criticidad", "UseCase");
    EstablecerNotas(cun06,
        "Actor: Responsable de Flota.\n" +
        "Objetivo: clasificar la criticidad de un reporte pendiente, consultando el historial de mantenimiento del vehiculo, y continuar siempre con CUN-07 (Determinar Modalidad de Reparacion).\n" +
        "Precondiciones: usuario autenticado con la patente 'Asignar criticidad'. Debe existir al menos un reporte en estado PENDIENTE.\n" +
        "Flujo principal:\n" +
        "1. El sistema muestra los reportes PENDIENTE.\n" +
        "2. El Responsable de Flota selecciona un reporte.\n" +
        "3. El sistema muestra el historial de reportes previos de esa unidad (excluyendo el reporte actual).\n" +
        "4. El Responsable de Flota asigna la criticidad (Baja/Media/Alta/Critica) y confirma.\n" +
        "5. El sistema clasifica el reporte (Estado=CLASIFICADO).\n" +
        "6. Sin volver a un menu, la pantalla pasa directamente a CUN-07 (inclusion obligatoria, paso 7 de la ECU).\n" +
        "Flujos alternativos:\n" +
        "Sin reportes pendientes: mensaje informativo.\n" +
        "Relacion <<include>> con CUN-07: inclusion obligatoria e inmediata (misma pantalla FrmReportesPendientesJP86, sin retorno a un menu intermedio).");

    var cun07 = ObtenerOCrearElemento(pkgCasosDeUso, "CUN-07 Determinar Modalidad de Reparacion", "UseCase");
    EstablecerNotas(cun07,
        "Actor: Responsable de Flota.\n" +
        "Objetivo: definir si la reparacion del vehiculo sera Interna o Externa, dejando el reporte y el vehiculo en estado EN_REPARACION de forma atomica.\n" +
        "Precondiciones: reporte recien clasificado (Estado=CLASIFICADO). Siempre se dispara incluido desde CUN-06, por eso no tiene asociacion propia con el actor en el DCU.\n" +
        "Flujo principal:\n" +
        "1. El Responsable de Flota elige la modalidad (Interna/Externa, RadioButton atado al enum).\n" +
        "2. Confirma.\n" +
        "3. El sistema actualiza el reporte (Estado=EN_REPARACION) y el vehiculo (Estado=EN_REPARACION) en una unica transaccion de base de datos (SP_ReporteJP86_DeterminarModalidad), a diferencia de CUN-01/CUN-04 que reutilizan BLL_64PR.VehiculoJP86.CambiarEstado en llamadas separadas.\n" +
        "4. El control vuelve a CUN-06 (la grilla de pendientes se refresca; el reporte ya no aparece).");

    var cun08 = ObtenerOCrearElemento(pkgCasosDeUso, "CUN-08 Registrar Acreditacion de Reparacion", "UseCase");
    EstablecerNotas(cun08,
        "Actor: Responsable de Flota.\n" +
        "Objetivo: cerrar un reporte en reparacion documentando el trabajo realizado, e incluir siempre a CUN-09 (Liberar Vehiculo).\n" +
        "Precondiciones: usuario autenticado con la patente 'Registrar acreditacion'. Debe existir al menos un reporte en estado EN_REPARACION.\n" +
        "Flujo principal:\n" +
        "1. El sistema muestra los reportes EN_REPARACION.\n" +
        "2. El Responsable de Flota selecciona un reporte.\n" +
        "3. Ingresa la descripcion de cierre (texto libre, no vacia).\n" +
        "4. Confirma 'Acreditar reparacion'.\n" +
        "5. El sistema acredita el reporte (Estado=ACREDITADO) e incluye siempre a CUN-09, liberando el vehiculo (Estado=DISPONIBLE) en la misma transaccion (SP_ReporteJP86_Acreditar).\n" +
        "Flujos alternativos:\n" +
        "Trabajos no finalizados: el Responsable de Flota no confirma; vuelve al paso 2, la grilla sigue mostrando el reporte sin cambios.\n" +
        "Relacion <<include>> con CUN-09: inclusion obligatoria dentro de la misma transaccion SQL.");

    var cun09 = ObtenerOCrearElemento(pkgCasosDeUso, "CUN-09 Liberar Vehiculo", "UseCase");
    EstablecerNotas(cun09,
        "Actor: Responsable de Flota.\n" +
        "Objetivo: dejar el vehiculo disponible nuevamente una vez acreditada la reparacion.\n" +
        "Precondiciones: reporte recien acreditado. Siempre se dispara incluido desde CUN-08, por eso no tiene asociacion propia con el actor en el DCU.\n" +
        "Flujo principal:\n" +
        "1. En la misma transaccion SQL que la acreditacion (SP_ReporteJP86_Acreditar), el sistema cambia el estado del vehiculo a DISPONIBLE.\n" +
        "2. El control vuelve a CUN-08 (la grilla de reparaciones en curso se refresca; el reporte ya no aparece).\n" +
        "Postcondiciones: el vehiculo queda en estado \"Disponible\" pero sigue inactivo (se desactivo al pasar a \"En revision\" en CUN-04). No se reactiva automaticamente: la reactivacion es manual desde el maestro de Vehiculos, como control de calidad antes de volver a la flota. Hasta entonces no aparece como unidad disponible para alquilar (CUN-01).");

    // -----------------------------------------------------------------
    // Relaciones actor -> caso de uso
    // -----------------------------------------------------------------
    ObtenerOCrearAsociacion(actorAgente, cun01);
    ObtenerOCrearAsociacion(actorAgente, cun03);
    ObtenerOCrearAsociacion(actorAgente, cun04);
    ObtenerOCrearAsociacion(actorAgente, cun05);
    ObtenerOCrearAsociacion(actorFlota, cun06);
    ObtenerOCrearAsociacion(actorFlota, cun08);

    // -----------------------------------------------------------------
    // Relaciones include / extend
    // En <<extend>>: la flecha va del caso que extiende hacia el caso base.
    // En <<include>>: la flecha va del caso base hacia el caso incluido.
    // -----------------------------------------------------------------
    ObtenerOCrearRelacionUC(cun02, cun01, "Extend");
    ObtenerOCrearRelacionUC(cun03, cun04, "Include");
    ObtenerOCrearRelacionUC(cun06, cun07, "Include");
    ObtenerOCrearRelacionUC(cun08, cun09, "Include");

    // -----------------------------------------------------------------
    // Diagrama DCU - RFN1 Alquileres
    // -----------------------------------------------------------------
    var diagRFN1 = ObtenerOCrearDiagrama(pkgCasosDeUso, "DCU - RFN1 Alquileres", "Use Case");
    if (diagRFN1 != null) {
        AgregarObjetoADiagrama(diagRFN1, actorAgente, 40, -40, 160, -140);
        AgregarObjetoADiagrama(diagRFN1, cun01, 320, -40, 620, -120);
        AgregarObjetoADiagrama(diagRFN1, cun02, 700, -40, 1000, -120);
        AgregarObjetoADiagrama(diagRFN1, cun03, 320, -220, 620, -300);
        AgregarObjetoADiagrama(diagRFN1, cun04, 320, -400, 620, -480);
        try {
            Repository.ReloadDiagram(diagRFN1.DiagramID);
        } catch (e) {
            contadorErrores++;
            log("ERROR", "No se pudo recargar el diagrama 'DCU - RFN1 Alquileres': " + e.description);
        }
    }

    // -----------------------------------------------------------------
    // Diagrama DCU - RFN2 Mantenimiento
    // -----------------------------------------------------------------
    var diagRFN2 = ObtenerOCrearDiagrama(pkgCasosDeUso, "DCU - RFN2 Mantenimiento", "Use Case");
    if (diagRFN2 != null) {
        AgregarObjetoADiagrama(diagRFN2, actorAgente, 40, -40, 160, -140);
        AgregarObjetoADiagrama(diagRFN2, actorFlota, 40, -300, 160, -400);
        AgregarObjetoADiagrama(diagRFN2, cun05, 320, -40, 620, -120);
        AgregarObjetoADiagrama(diagRFN2, cun06, 320, -220, 620, -300);
        AgregarObjetoADiagrama(diagRFN2, cun07, 700, -220, 1000, -300);
        AgregarObjetoADiagrama(diagRFN2, cun08, 320, -400, 620, -480);
        AgregarObjetoADiagrama(diagRFN2, cun09, 700, -400, 1000, -480);
        try {
            Repository.ReloadDiagram(diagRFN2.DiagramID);
        } catch (e) {
            contadorErrores++;
            log("ERROR", "No se pudo recargar el diagrama 'DCU - RFN2 Mantenimiento': " + e.description);
        }
    }

    try {
        Repository.RefreshModelView(pkgCasosDeUso.PackageGUID);
    } catch (e) {
        // no fatal, solo informativo
    }

    Session.Output("===============================================================");
    Session.Output("01_CasosDeUso.js - RESUMEN: creados=" + contadorCreados + " existentes=" + contadorExistentes + " errores=" + contadorErrores);
    Session.Output("===============================================================");
}

main();
