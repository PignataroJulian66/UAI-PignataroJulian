# EA_Scripts — Documentación automática de SIGAM en Enterprise Architect

Scripts JavaScript (motor JScript de EA, nivel ES3) para cargar en Enterprise
Architect Ultimate Edition, vía la ventana **Scripting**, la documentación UML
de RFN1 (CUN-01 a CUN-04) y RFN2 (CUN-05 a CUN-09) de SIGAM.

Esta carpeta vive fuera de cualquier proyecto de la solución (no está
referenciada en ningún `.csproj`) y es de **solo lectura respecto del
código**: los scripts no modifican nada del proyecto .NET, la base de datos
ni los JSON de idioma. Solo escriben en el repositorio (`.eap`/`.eapx`/…) de
Enterprise Architect.

## Prerrequisito común a todos los scripts

Antes de correr cualquiera de estos scripts, **seleccioná en el árbol de
proyecto de EA el paquete raíz "SIGAM"** (o el que uses como raíz del
modelo). Todos los scripts arrancan con `Repository.GetTreeSelectedPackage()`
y, si no hay nada seleccionado, informan el error por `Session.Output` y
terminan sin crear nada.

Debajo de ese paquete raíz, los scripts crean (si no existen) 3 sub-paquetes:

- **Casos de Uso** — actores, casos de uso, relaciones include/extend, 2
  diagramas de casos de uso (RFN1 y RFN2).
- **Clases** — todas las clases/enums/interfaces de `BE`, `Mapper`,
  `BLL_64PR` y `DAL_64PR` (incluidas las compartidas entre RFN1 y RFN2,
  como `VehiculoJP86`), con sus diagramas de clases.
- **Secuencia** — un sub-paquete por CUN (`CUN-01 ...`, `CUN-02 ...`, etc.)
  con el elemento de UI (el `Frm*`) y el diagrama de secuencia de ese caso
  de uso.

## Cómo cargar y ejecutar un script en EA

1. Abrí el proyecto SIGAM en Enterprise Architect Ultimate Edition.
2. En el árbol de proyecto, seleccioná el paquete raíz "SIGAM" (el mismo
   paquete para los 3 scripts de la Fase A — no cambiar la selección entre
   scripts).
3. Menú **Ver > Scripting** (o `Ctrl+Shift+F11` según versión) para abrir la
   ventana Scripting.
4. Creá un nuevo script de tipo **JavaScript** (clic derecho sobre el grupo
   de scripts > Add Script), pegá el contenido completo de uno de los
   archivos `.js` de esta carpeta, y ejecutalo (F5 o el botón "Run").
5. Revisá la pestaña **System Output** de EA: cada script imprime líneas
   `OK` / `WARN` / `ERROR` y termina con un resumen de contadores
   (`creados` / `existentes` / `errores`).
6. Repetí para el siguiente script, **respetando el orden de ejecución**
   de la sección siguiente. Todos los scripts son idempotentes: se pueden
   volver a correr sin duplicar paquetes, elementos, atributos, métodos,
   relaciones ni diagramas.

Cada archivo `.js` es autocontenido (no usa `!INC`): se puede pegar y
correr de a uno, sin dependencias de scripts previos cargados en la misma
sesión de Scripting (sí dependen, en tiempo de ejecución, de que los
elementos de scripts anteriores ya existan en el modelo — ver la sección
"Prerrequisitos" de la cabecera de cada script).

## Orden de ejecución

| Orden | Script | Qué crea | Depende de |
|---|---|---|---|
| 1 | `01_CasosDeUso.js` | Actores, 9 casos de uso, relaciones include/extend, 2 DCU (RFN1 y RFN2) | — |
| 2 | `02_Clases_RFN1.js` | Clases BE/Mapper/BLL/DAL de CUN-01 a CUN-04 (incluye las compartidas Categoría/Vehículo/Cliente/Acceso/DV), 2 DCL | — |
| 3 | `02_Clases_RFN2.js` *(Fase B)* | Clases BE/Mapper/BLL de CUN-05 a CUN-09, reutiliza Vehículo/Acceso/DV de `02_Clases_RFN1.js` | `02_Clases_RFN1.js` |
| 4 | `03_Secuencia_CUN01.js` | Elemento GUI `FrmGenerarContratoJP86` + DS de CUN-01 | `01_CasosDeUso.js`, `02_Clases_RFN1.js` |
| 5 | `03_Secuencia_CUN02.js` … `03_Secuencia_CUN09.js` *(Fase B)* | Elemento GUI + DS por cada CUN restante | `01_CasosDeUso.js`, `02_Clases_RFN1.js`/`RFN2.js` |
| 6 | `04_DER.js` *(opcional, Fase B, a confirmar)* | Tablas como elementos de datos, si el alcance lo permite | — |

**Fase A (esta entrega):** solo 1, 2 y 4 de la tabla, más este README.
**Fase B (con confirmación del desarrollador):** el resto.

## Matriz de trazabilidad CUN → diagramas → script

| CUN | Nombre | DCU | DCL | DS | Script(s) |
|---|---|---|---|---|---|
| CUN-01 | Generar Contrato de Alquiler | DCU - RFN1 Alquileres | DCL - RFN1 Modelo General | DS - CUN-01 Generar Contrato de Alquiler | `01_CasosDeUso.js`, `02_Clases_RFN1.js`, `03_Secuencia_CUN01.js` |
| CUN-02 | Registrar Cliente (extend de CUN-01) | DCU - RFN1 Alquileres | DCL - RFN1 Modelo General | DS - CUN-02 Registrar Cliente *(Fase B)* | `01_CasosDeUso.js`, `02_Clases_RFN1.js`, `03_Secuencia_CUN02.js` |
| CUN-03 | Generar Factura (include diferido de CUN-04) | DCU - RFN1 Alquileres | DCL - RFN1 Modelo General + DCL - RFN1 CUN-03 Estrategia de Pago | DS - CUN-03 Generar Factura *(Fase B)* | `01_CasosDeUso.js`, `02_Clases_RFN1.js`, `03_Secuencia_CUN03.js` |
| CUN-04 | Registrar Devolución de Vehículo | DCU - RFN1 Alquileres | DCL - RFN1 Modelo General | DS - CUN-04 Registrar Devolucion de Vehiculo *(Fase B)* | `01_CasosDeUso.js`, `02_Clases_RFN1.js`, `03_Secuencia_CUN04.js` |
| CUN-05 | Registrar Reporte de Desperfecto | DCU - RFN2 Mantenimiento | DCL - RFN2 *(Fase B)* | DS - CUN-05 *(Fase B)* | `01_CasosDeUso.js`, `02_Clases_RFN2.js`, `03_Secuencia_CUN05.js` |
| CUN-06 | Asignar Criticidad (include de CUN-07) | DCU - RFN2 Mantenimiento | DCL - RFN2 *(Fase B)* | DS - CUN-06 *(Fase B)* | `01_CasosDeUso.js`, `02_Clases_RFN2.js`, `03_Secuencia_CUN06.js` |
| CUN-07 | Determinar Modalidad de Reparación (incluido en CUN-06) | DCU - RFN2 Mantenimiento | DCL - RFN2 *(Fase B)* | DS - CUN-07 *(Fase B, diagrama propio como en el informe)* | `01_CasosDeUso.js`, `02_Clases_RFN2.js`, `03_Secuencia_CUN07.js` |
| CUN-08 | Registrar Acreditación de Reparación (include de CUN-09) | DCU - RFN2 Mantenimiento | DCL - RFN2 *(Fase B)* | DS - CUN-08 *(Fase B)* | `01_CasosDeUso.js`, `02_Clases_RFN2.js`, `03_Secuencia_CUN08.js` |
| CUN-09 | Liberar Vehículo (incluido en CUN-08) | DCU - RFN2 Mantenimiento | DCL - RFN2 *(Fase B)* | DS - CUN-09 *(Fase B, diagrama propio como en el informe)* | `01_CasosDeUso.js`, `02_Clases_RFN2.js`, `03_Secuencia_CUN09.js` |

## Limitaciones conocidas (ver también la cabecera de cada script)

1. **Fragmentos `alt`/`opt` nativos de UML no se generan por Automation
   Interface** en `03_Secuencia_CUN01.js`: no hay una API estable y
   documentada para crear "combined fragments" desde JScript ES3. Los
   flujos alternativos/opcionales se dejan como elementos **Note**
   ancladas cerca de los mensajes correspondientes, con el texto del flujo.
   Retoque manual sugerido: reemplazar cada Note por un frame real
   (Insert Frame > alt/opt) en EA.
2. **Numeración de mensajes de secuencia**: se apoya en el orden de
   creación de los `DiagramLink` (que es el mecanismo que usa EA para
   numerar mensajes) reforzado con un prefijo numérico explícito en el
   nombre de cada mensaje (`"01: ..."`, `"02: ..."`, etc.) como respaldo
   visual. Verificar en EA que ambos coincidan.
3. **Mensajes de retorno**: se modelan como conectores `Sequence`
   adicionales de dirección invertida con el texto `"retorna: ..."`, en
   vez del estilo nativo de flecha punteada de retorno de EA (no expuesto
   de forma confiable vía script). Retoque manual sugerido: marcar estos
   mensajes como "Response" desde las propiedades del mensaje en EA.
4. **Barras de activación (focus of control)**: no se crean a mano; EA
   normalmente las genera solas al abrir el diagrama. Si no aparecen,
   agregarlas manualmente.
5. **Coordenadas de layout**: se usa la convención `top`/`bottom`
   negativos crecientes hacia abajo, habitual en la Automation Interface
   de EA. Si algo aparece invertido o fuera de vista al abrir el diagrama,
   es un ajuste de signo puntual — avisar para corregirlo.

## Discrepancias encontradas entre los informes y el código (Fase A)

- **CUN-01 — botón "Imprimir Recibo"**: `Informe_RFN1.md` sección 5.1 no
  documenta este paso, pero `FrmGenerarContratoJP86.btnImprimirRecibo_Click`
  sí llama a `Documentos.GeneradorReciboContrato.Imprimir(contratoGenerado)`
  (mismo patrón transversal ya documentado para CUN-05 con
  `Documentos.GeneradorReporteDesperfecto`). Se agregó como paso opcional al
  final del diagrama de secuencia de CUN-01 y se dejó registrado en el
  campo Notes de CUN-01 en el paquete "Casos de Uso".
- El resto del código revisado para CUN-01 a CUN-04 (`BE.ContratoJP86`,
  `BE.FacturaJP86`, `BE.ResultadoPagoJP86`, los 4 enums, `BLL_64PR.ContratoJP86`,
  `BLL_64PR.VehiculoJP86.CambiarEstado`, `Mapper.mpp_contrato`,
  `Mapper.mpp_factura`, el Strategy Pattern completo de pago, y los 3
  formularios `FrmGenerarContratoJP86`/`FrmGenerarFacturaJP86`/
  `FrmRegistrarDevolucionJP86`) coincide exactamente con lo documentado en
  `Informe_RFN1.md` — no se encontraron más discrepancias en el alcance de
  esta Fase A.

## Supuestos tomados

- No se encontraron documentos de ECU (especificación de caso de uso)
  independientes de `Informe_RFN1.md`/`Informe_RFN2.md` en el repositorio;
  el campo Notes de cada caso de uso en `01_CasosDeUso.js` se redactó a
  partir de los diagramas de secuencia y las tablas de validaciones de
  esos informes (que sí reflejan fielmente el código, según lo verificado).
  Si existen ECU formales fuera del repo, avisar para ajustar el texto de
  Notes contra esa fuente.
- Las relaciones actor → caso de uso se limitaron a los casos disparados
  **directamente** por un actor (CUN-01, 03, 04, 05, 06, 08). CUN-02,
  CUN-07 y CUN-09 no llevan asociación propia con el actor por ser
  siempre disparados como extensión/inclusión de otro caso de uso — es
  una convención UML estándar, no una omisión.
- El actor "Responsable de Flota" se modela igual que "Agente Comercial"
  pese a que el rol homónimo todavía no existe en `Sesion.Rol_64PR` (las
  patentes de RFN2 están asignadas solo a `Administrador`); queda
  documentado en el campo Notes del actor.
