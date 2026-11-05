/*
   SIGAM - Script de instalacion de la base de datos (version 2.0)
   Generado: 2026-11-05 13:55

   - {{NOMBRE_BD}} se reemplaza por el nombre de la base de datos antes de ejecutar.
   - Los lotes previos a la marca '-- @@SIGAM:CONECTAR_BD@@' se ejecutan contra master;
     el resto, contra la base de datos creada.
   - El digito verificador (DV_64PR / DVH_64PR) se calcula al finalizar la instalacion.
*/
USE [master]
GO
IF DB_ID(N'{{NOMBRE_BD}}') IS NOT NULL
    RAISERROR(N'La base de datos {{NOMBRE_BD}} ya existe. Debe eliminarse antes de ejecutar este script.', 16, 1);
GO
CREATE DATABASE [{{NOMBRE_BD}}] COLLATE Modern_Spanish_CI_AS;
GO
-- @@SIGAM:CONECTAR_BD@@
USE [{{NOMBRE_BD}}]
GO

-- ======================================================================
-- TABLAS (21)
-- ======================================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categoria_64PR](
	[ID_Categoria] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](100) NOT NULL,
	[Descripcion] [varchar](255) NULL,
	[TarifaDiaria] [decimal](10, 2) NOT NULL,
	[Activo] [bit] NOT NULL
)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ClienteJP86](
	[DNI] [nvarchar](20) NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[Apellido] [nvarchar](100) NOT NULL,
	[Telefono] [nvarchar](20) NOT NULL,
	[Activo] [bit] NOT NULL
)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContratoJP86](
	[NumeroContrato] [int] IDENTITY(1,1) NOT NULL,
	[DNI_Cliente] [nvarchar](20) NOT NULL,
	[Patente_Vehiculo] [nvarchar](10) NOT NULL,
	[FechaInicio] [date] NOT NULL,
	[FechaFin] [date] NOT NULL,
	[TarifaDiaria] [decimal](10, 2) NOT NULL,
	[CargosAdicionales] [bit] NOT NULL,
	[ImporteTotal] [decimal](10, 2) NOT NULL,
	[KilometrajeEntrega] [int] NOT NULL,
	[KilometrajeRetorno] [int] NULL,
	[EstadoUnidadDevolucion] [varchar](20) NULL,
	[Estado] [varchar](20) NOT NULL
)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DV_64PR](
	[Tabla] [nvarchar](100) NOT NULL,
	[DV] [bigint] NOT NULL
)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DVH_64PR](
	[Tabla] [nvarchar](100) NOT NULL,
	[IdFila] [nvarchar](100) NOT NULL,
	[DVH] [bigint] NOT NULL
)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Evento_64PR](
	[Id_evento] [int] IDENTITY(1,1) NOT NULL,
	[Login] [nvarchar](50) NOT NULL,
	[Fecha] [date] NOT NULL,
	[Hora] [time](7) NOT NULL,
	[Id_modulo] [int] NOT NULL,
	[Id_Tipo] [int] NOT NULL,
	[criticidad] [tinyint] NOT NULL
)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FacturaJP86](
	[NumeroFactura] [int] IDENTITY(1,1) NOT NULL,
	[NumeroContrato] [int] NOT NULL,
	[FechaFactura] [datetime] NOT NULL,
	[MetodoPago] [varchar](20) NOT NULL,
	[MontoFactura] [decimal](10, 2) NOT NULL
)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Familia_64PR](
	[ID_Familia] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](100) NOT NULL
)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Familia_N_64PR](
	[ID_FamiliaPadre] [int] NOT NULL,
	[ID_FamiliaHija] [int] NOT NULL
)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Modulo_64PR](
	[Id_modulo] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](50) NOT NULL
)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Patente_64PR](
	[ID_Patente] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](100) NOT NULL
)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PatenteFamilia_64PR](
	[ID_Familia] [int] NOT NULL,
	[ID_Patente] [int] NOT NULL
)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReporteJP86](
	[NumeroReporte] [int] IDENTITY(1,1) NOT NULL,
	[Patente_Vehiculo] [nvarchar](10) NOT NULL,
	[FechaHora] [datetime] NOT NULL,
	[DescripcionProblema] [nvarchar](500) NOT NULL,
	[Fuente] [varchar](20) NOT NULL,
	[Criticidad] [varchar](10) NULL,
	[ModalidadReparacion] [varchar](10) NULL,
	[DescripcionCierre] [nvarchar](500) NULL,
	[FechaAcreditacion] [datetime] NULL,
	[DNI_ResponsableCierre] [nvarchar](20) NULL,
	[Estado] [varchar](20) NOT NULL
)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles_64PR](
	[ID_Rol] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](100) NOT NULL
)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RolFamilia_64PR](
	[ID_Rol] [int] NOT NULL,
	[ID_Familia] [int] NOT NULL
)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RolPatente_64PR](
	[ID_Rol] [int] NOT NULL,
	[ID_Patente] [int] NOT NULL
)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TipoEvento_64PR](
	[Id_tipo] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](100) NOT NULL
)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[USUARIO_64PR](
	[DNI] [nvarchar](20) NOT NULL,
	[Apellido] [nvarchar](100) NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[Login] [nvarchar](50) NOT NULL,
	[Contraseña] [varbinary](48) NOT NULL,
	[Rol] [int] NULL,
	[Email] [nvarchar](256) NULL,
	[Bloqueo] [bit] NOT NULL,
	[Activo] [bit] NOT NULL,
	[Intentos] [int] NOT NULL,
	[PrimeraVez] [bit] NOT NULL,
	[Idioma] [varchar](5) NOT NULL
)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vehiculo_BitacoraCambios](
	[IdBitacora] [int] IDENTITY(1,1) NOT NULL,
	[Patente] [nvarchar](10) NOT NULL,
	[Marca] [nvarchar](50) NOT NULL,
	[Modelo] [nvarchar](50) NOT NULL,
	[ID_Categoria] [int] NOT NULL,
	[Kilometraje] [int] NOT NULL,
	[Estado] [varchar](20) NOT NULL,
	[Activo] [bit] NOT NULL,
	[Fecha] [date] NOT NULL,
	[Hora] [time](0) NOT NULL,
	[Act] [bit] NOT NULL
)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VehiculoJP86](
	[Patente] [nvarchar](10) NOT NULL,
	[Marca] [nvarchar](50) NOT NULL,
	[Modelo] [nvarchar](50) NOT NULL,
	[ID_Categoria] [int] NOT NULL,
	[Kilometraje] [int] NOT NULL,
	[Estado] [varchar](20) NOT NULL,
	[Activo] [bit] NOT NULL
)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VersionBDJP86](
	[ID_Version] [int] IDENTITY(1,1) NOT NULL,
	[Version] [varchar](20) NOT NULL,
	[FechaAplicacion] [datetime] NOT NULL,
	[Descripcion] [nvarchar](200) NULL
)
GO

-- ======================================================================
-- CLAVES PRIMARIAS (21)
-- ======================================================================
ALTER TABLE [dbo].[Categoria_64PR] ADD CONSTRAINT [PK_Categoria_64PR] PRIMARY KEY CLUSTERED ([ID_Categoria]);
ALTER TABLE [dbo].[ClienteJP86] ADD CONSTRAINT [PK_ClienteJP86] PRIMARY KEY CLUSTERED ([DNI]);
ALTER TABLE [dbo].[ContratoJP86] ADD CONSTRAINT [PK_ContratoJP86] PRIMARY KEY CLUSTERED ([NumeroContrato]);
ALTER TABLE [dbo].[DV_64PR] ADD CONSTRAINT [PK_DV_64PR] PRIMARY KEY CLUSTERED ([Tabla]);
ALTER TABLE [dbo].[DVH_64PR] ADD CONSTRAINT [PK_DVH_64PR] PRIMARY KEY CLUSTERED ([Tabla],[IdFila]);
ALTER TABLE [dbo].[Evento_64PR] ADD CONSTRAINT [PK_Evento_64PR] PRIMARY KEY CLUSTERED ([Id_evento]);
ALTER TABLE [dbo].[FacturaJP86] ADD CONSTRAINT [PK_FacturaJP86] PRIMARY KEY CLUSTERED ([NumeroFactura]);
ALTER TABLE [dbo].[Familia_64PR] ADD CONSTRAINT [PK_Familia_64PR] PRIMARY KEY CLUSTERED ([ID_Familia]);
ALTER TABLE [dbo].[Familia_N_64PR] ADD CONSTRAINT [PK_Familia_N_64PR] PRIMARY KEY CLUSTERED ([ID_FamiliaPadre],[ID_FamiliaHija]);
ALTER TABLE [dbo].[Modulo_64PR] ADD CONSTRAINT [PK_Modulo_64PR] PRIMARY KEY CLUSTERED ([Id_modulo]);
ALTER TABLE [dbo].[Patente_64PR] ADD CONSTRAINT [PK_Patente_64PR] PRIMARY KEY CLUSTERED ([ID_Patente]);
ALTER TABLE [dbo].[PatenteFamilia_64PR] ADD CONSTRAINT [PK_PatenteFamilia_64PR] PRIMARY KEY CLUSTERED ([ID_Familia],[ID_Patente]);
ALTER TABLE [dbo].[ReporteJP86] ADD CONSTRAINT [PK_ReporteJP86] PRIMARY KEY CLUSTERED ([NumeroReporte]);
ALTER TABLE [dbo].[Roles_64PR] ADD CONSTRAINT [PK_Roles_64PR] PRIMARY KEY CLUSTERED ([ID_Rol]);
ALTER TABLE [dbo].[RolFamilia_64PR] ADD CONSTRAINT [PK_RolFamilia_64PR] PRIMARY KEY CLUSTERED ([ID_Rol],[ID_Familia]);
ALTER TABLE [dbo].[RolPatente_64PR] ADD CONSTRAINT [PK_RolPatente_64PR] PRIMARY KEY CLUSTERED ([ID_Rol],[ID_Patente]);
ALTER TABLE [dbo].[TipoEvento_64PR] ADD CONSTRAINT [PK_TipoEvento_64PR] PRIMARY KEY CLUSTERED ([Id_tipo]);
ALTER TABLE [dbo].[USUARIO_64PR] ADD CONSTRAINT [PK_USUARIO_64PR] PRIMARY KEY CLUSTERED ([DNI]);
ALTER TABLE [dbo].[Vehiculo_BitacoraCambios] ADD CONSTRAINT [PK_Vehiculo_BitacoraCambios] PRIMARY KEY CLUSTERED ([IdBitacora]);
ALTER TABLE [dbo].[VehiculoJP86] ADD CONSTRAINT [PK_VehiculoJP86] PRIMARY KEY CLUSTERED ([Patente]);
ALTER TABLE [dbo].[VersionBDJP86] ADD CONSTRAINT [PK_VersionBDJP86] PRIMARY KEY CLUSTERED ([ID_Version]);
GO

-- ======================================================================
-- VALORES POR DEFECTO (19)
-- ======================================================================
ALTER TABLE [dbo].[Categoria_64PR] ADD CONSTRAINT [DF_Categoria_Activo] DEFAULT ((1)) FOR [Activo];
ALTER TABLE [dbo].[ClienteJP86] ADD CONSTRAINT [DF_ClienteJP86_Activo] DEFAULT ((1)) FOR [Activo];
ALTER TABLE [dbo].[ContratoJP86] ADD CONSTRAINT [DF_ContratoJP86_CargosAdicionales] DEFAULT ((0)) FOR [CargosAdicionales];
ALTER TABLE [dbo].[ContratoJP86] ADD CONSTRAINT [DF_ContratoJP86_Estado] DEFAULT ('ACTIVO') FOR [Estado];
ALTER TABLE [dbo].[Evento_64PR] ADD CONSTRAINT [DF_Bitacora_Fecha] DEFAULT (CONVERT([date],getdate())) FOR [Fecha];
ALTER TABLE [dbo].[Evento_64PR] ADD CONSTRAINT [DF_Bitacora_Hora] DEFAULT (CONVERT([time],getdate())) FOR [Hora];
ALTER TABLE [dbo].[FacturaJP86] ADD CONSTRAINT [DF_FacturaJP86_FechaFactura] DEFAULT (getdate()) FOR [FechaFactura];
ALTER TABLE [dbo].[ReporteJP86] ADD CONSTRAINT [DF_ReporteJP86_Estado] DEFAULT ('PENDIENTE') FOR [Estado];
ALTER TABLE [dbo].[USUARIO_64PR] ADD CONSTRAINT [DF_USUARIO_64PR_Activo] DEFAULT ((1)) FOR [Activo];
ALTER TABLE [dbo].[USUARIO_64PR] ADD CONSTRAINT [DF_USUARIO_64PR_Bloqueo] DEFAULT ((0)) FOR [Bloqueo];
ALTER TABLE [dbo].[USUARIO_64PR] ADD CONSTRAINT [DF_USUARIO_64PR_Idioma] DEFAULT ('es') FOR [Idioma];
ALTER TABLE [dbo].[USUARIO_64PR] ADD CONSTRAINT [DF_USUARIO_64PR_Intentos] DEFAULT ((0)) FOR [Intentos];
ALTER TABLE [dbo].[USUARIO_64PR] ADD CONSTRAINT [DF_USUARIO_64PR_PrimeraVez] DEFAULT ((1)) FOR [PrimeraVez];
ALTER TABLE [dbo].[Vehiculo_BitacoraCambios] ADD CONSTRAINT [DF_VehiculoBitacoraCambios_Act] DEFAULT ((1)) FOR [Act];
ALTER TABLE [dbo].[Vehiculo_BitacoraCambios] ADD CONSTRAINT [DF_VehiculoBitacoraCambios_Fecha] DEFAULT (CONVERT([date],getdate())) FOR [Fecha];
ALTER TABLE [dbo].[Vehiculo_BitacoraCambios] ADD CONSTRAINT [DF_VehiculoBitacoraCambios_Hora] DEFAULT (CONVERT([time](0),getdate())) FOR [Hora];
ALTER TABLE [dbo].[VehiculoJP86] ADD CONSTRAINT [DF_VehiculoJP86_Activo] DEFAULT ((1)) FOR [Activo];
ALTER TABLE [dbo].[VehiculoJP86] ADD CONSTRAINT [DF_VehiculoJP86_Estado] DEFAULT ('DISPONIBLE') FOR [Estado];
ALTER TABLE [dbo].[VersionBDJP86] ADD CONSTRAINT [DF_VersionBDJP86_Fecha] DEFAULT (getdate()) FOR [FechaAplicacion];
GO

-- ======================================================================
-- DATOS: Modulo_64PR (10 filas)
-- ======================================================================
SET IDENTITY_INSERT [dbo].[Modulo_64PR] ON;
INSERT INTO [dbo].[Modulo_64PR] ([Id_modulo], [Nombre]) VALUES (1, N'Login');
INSERT INTO [dbo].[Modulo_64PR] ([Id_modulo], [Nombre]) VALUES (2, N'Gestion de usuarios');
INSERT INTO [dbo].[Modulo_64PR] ([Id_modulo], [Nombre]) VALUES (3, N'Gestion de roles');
INSERT INTO [dbo].[Modulo_64PR] ([Id_modulo], [Nombre]) VALUES (4, N'Gestion de familias');
INSERT INTO [dbo].[Modulo_64PR] ([Id_modulo], [Nombre]) VALUES (5, N'Gestion de categorias');
INSERT INTO [dbo].[Modulo_64PR] ([Id_modulo], [Nombre]) VALUES (6, N'Gestion de vehiculos');
INSERT INTO [dbo].[Modulo_64PR] ([Id_modulo], [Nombre]) VALUES (7, N'Gestion de clientes');
INSERT INTO [dbo].[Modulo_64PR] ([Id_modulo], [Nombre]) VALUES (8, N'Gestion de alquileres');
INSERT INTO [dbo].[Modulo_64PR] ([Id_modulo], [Nombre]) VALUES (9, N'Gestion de mantenimiento');
INSERT INTO [dbo].[Modulo_64PR] ([Id_modulo], [Nombre]) VALUES (10, N'Sistema');
SET IDENTITY_INSERT [dbo].[Modulo_64PR] OFF;
GO

-- ======================================================================
-- DATOS: TipoEvento_64PR (43 filas)
-- ======================================================================
SET IDENTITY_INSERT [dbo].[TipoEvento_64PR] ON;
INSERT INTO [dbo].[TipoEvento_64PR] ([Id_tipo], [Nombre]) VALUES (1, N'Login exitoso');
INSERT INTO [dbo].[TipoEvento_64PR] ([Id_tipo], [Nombre]) VALUES (2, N'Cambio de clave');
INSERT INTO [dbo].[TipoEvento_64PR] ([Id_tipo], [Nombre]) VALUES (3, N'Login fallido');
INSERT INTO [dbo].[TipoEvento_64PR] ([Id_tipo], [Nombre]) VALUES (4, N'Usuario bloqueado');
INSERT INTO [dbo].[TipoEvento_64PR] ([Id_tipo], [Nombre]) VALUES (5, N'Logout');
INSERT INTO [dbo].[TipoEvento_64PR] ([Id_tipo], [Nombre]) VALUES (6, N'Creacion de usuario');
INSERT INTO [dbo].[TipoEvento_64PR] ([Id_tipo], [Nombre]) VALUES (7, N'Modificacion de usuario');
INSERT INTO [dbo].[TipoEvento_64PR] ([Id_tipo], [Nombre]) VALUES (9, N'Creacion de rol');
INSERT INTO [dbo].[TipoEvento_64PR] ([Id_tipo], [Nombre]) VALUES (10, N'Eliminacion de rol');
INSERT INTO [dbo].[TipoEvento_64PR] ([Id_tipo], [Nombre]) VALUES (11, N'Modificacion de rol');
INSERT INTO [dbo].[TipoEvento_64PR] ([Id_tipo], [Nombre]) VALUES (12, N'Creacion de familia');
INSERT INTO [dbo].[TipoEvento_64PR] ([Id_tipo], [Nombre]) VALUES (13, N'Eliminacion de familia');
INSERT INTO [dbo].[TipoEvento_64PR] ([Id_tipo], [Nombre]) VALUES (14, N'Modificacion de familia');
INSERT INTO [dbo].[TipoEvento_64PR] ([Id_tipo], [Nombre]) VALUES (15, N'Backup');
INSERT INTO [dbo].[TipoEvento_64PR] ([Id_tipo], [Nombre]) VALUES (16, N'Restore');
INSERT INTO [dbo].[TipoEvento_64PR] ([Id_tipo], [Nombre]) VALUES (17, N'Creacion de categoria');
INSERT INTO [dbo].[TipoEvento_64PR] ([Id_tipo], [Nombre]) VALUES (18, N'Modificacion de categoria');
INSERT INTO [dbo].[TipoEvento_64PR] ([Id_tipo], [Nombre]) VALUES (19, N'Baja de categoria');
INSERT INTO [dbo].[TipoEvento_64PR] ([Id_tipo], [Nombre]) VALUES (20, N'Creacion de vehiculo');
INSERT INTO [dbo].[TipoEvento_64PR] ([Id_tipo], [Nombre]) VALUES (21, N'Modificacion de vehiculo');
INSERT INTO [dbo].[TipoEvento_64PR] ([Id_tipo], [Nombre]) VALUES (22, N'Baja de vehiculo');
INSERT INTO [dbo].[TipoEvento_64PR] ([Id_tipo], [Nombre]) VALUES (23, N'Cambio de estado de vehiculo');
INSERT INTO [dbo].[TipoEvento_64PR] ([Id_tipo], [Nombre]) VALUES (24, N'Creacion de cliente');
INSERT INTO [dbo].[TipoEvento_64PR] ([Id_tipo], [Nombre]) VALUES (25, N'Modificacion de cliente');
INSERT INTO [dbo].[TipoEvento_64PR] ([Id_tipo], [Nombre]) VALUES (26, N'Baja de cliente');
INSERT INTO [dbo].[TipoEvento_64PR] ([Id_tipo], [Nombre]) VALUES (27, N'Creacion de contrato');
INSERT INTO [dbo].[TipoEvento_64PR] ([Id_tipo], [Nombre]) VALUES (28, N'Facturacion de contrato');
INSERT INTO [dbo].[TipoEvento_64PR] ([Id_tipo], [Nombre]) VALUES (29, N'Devolucion de vehiculo');
INSERT INTO [dbo].[TipoEvento_64PR] ([Id_tipo], [Nombre]) VALUES (30, N'Alta de reporte de desperfecto');
INSERT INTO [dbo].[TipoEvento_64PR] ([Id_tipo], [Nombre]) VALUES (31, N'Asignacion de criticidad');
INSERT INTO [dbo].[TipoEvento_64PR] ([Id_tipo], [Nombre]) VALUES (32, N'Determinacion de modalidad de reparacion');
INSERT INTO [dbo].[TipoEvento_64PR] ([Id_tipo], [Nombre]) VALUES (33, N'Acreditacion de reparacion');
INSERT INTO [dbo].[TipoEvento_64PR] ([Id_tipo], [Nombre]) VALUES (34, N'Liberacion de vehiculo');
INSERT INTO [dbo].[TipoEvento_64PR] ([Id_tipo], [Nombre]) VALUES (35, N'Error no controlado');
INSERT INTO [dbo].[TipoEvento_64PR] ([Id_tipo], [Nombre]) VALUES (36, N'Serializacion de clientes');
INSERT INTO [dbo].[TipoEvento_64PR] ([Id_tipo], [Nombre]) VALUES (37, N'Deserializacion de clientes');
INSERT INTO [dbo].[TipoEvento_64PR] ([Id_tipo], [Nombre]) VALUES (38, N'Error en serializacion de clientes');
INSERT INTO [dbo].[TipoEvento_64PR] ([Id_tipo], [Nombre]) VALUES (39, N'Error en deserializacion de clientes');
INSERT INTO [dbo].[TipoEvento_64PR] ([Id_tipo], [Nombre]) VALUES (40, N'Restauracion de version de vehiculo');
INSERT INTO [dbo].[TipoEvento_64PR] ([Id_tipo], [Nombre]) VALUES (41, N'Generar reporte de mantenimiento preventivo');
INSERT INTO [dbo].[TipoEvento_64PR] ([Id_tipo], [Nombre]) VALUES (42, N'Imprimir reporte de mantenimiento preventivo');
INSERT INTO [dbo].[TipoEvento_64PR] ([Id_tipo], [Nombre]) VALUES (43, N'Consulta de ayuda');
INSERT INTO [dbo].[TipoEvento_64PR] ([Id_tipo], [Nombre]) VALUES (44, N'Error al abrir la ayuda');
SET IDENTITY_INSERT [dbo].[TipoEvento_64PR] OFF;
GO

-- ======================================================================
-- DATOS: Patente_64PR (37 filas)
-- ======================================================================
SET IDENTITY_INSERT [dbo].[Patente_64PR] ON;
INSERT INTO [dbo].[Patente_64PR] ([ID_Patente], [Nombre]) VALUES (1, N'Crear usuario');
INSERT INTO [dbo].[Patente_64PR] ([ID_Patente], [Nombre]) VALUES (2, N'Desbloquear usuario');
INSERT INTO [dbo].[Patente_64PR] ([ID_Patente], [Nombre]) VALUES (3, N'Modificar usuario');
INSERT INTO [dbo].[Patente_64PR] ([ID_Patente], [Nombre]) VALUES (4, N'Activar y desactivar usuarios');
INSERT INTO [dbo].[Patente_64PR] ([ID_Patente], [Nombre]) VALUES (5, N'Bitacora');
INSERT INTO [dbo].[Patente_64PR] ([ID_Patente], [Nombre]) VALUES (6, N'Crear roles');
INSERT INTO [dbo].[Patente_64PR] ([ID_Patente], [Nombre]) VALUES (7, N'Modificar roles');
INSERT INTO [dbo].[Patente_64PR] ([ID_Patente], [Nombre]) VALUES (8, N'Eliminar roles');
INSERT INTO [dbo].[Patente_64PR] ([ID_Patente], [Nombre]) VALUES (9, N'Crear familias');
INSERT INTO [dbo].[Patente_64PR] ([ID_Patente], [Nombre]) VALUES (10, N'Modificar familias');
INSERT INTO [dbo].[Patente_64PR] ([ID_Patente], [Nombre]) VALUES (11, N'Eliminar familias');
INSERT INTO [dbo].[Patente_64PR] ([ID_Patente], [Nombre]) VALUES (12, N'Reparar Integridad DV');
INSERT INTO [dbo].[Patente_64PR] ([ID_Patente], [Nombre]) VALUES (13, N'Cambiar clave');
INSERT INTO [dbo].[Patente_64PR] ([ID_Patente], [Nombre]) VALUES (14, N'Cambiar idioma');
INSERT INTO [dbo].[Patente_64PR] ([ID_Patente], [Nombre]) VALUES (16, N'Hacer respaldos');
INSERT INTO [dbo].[Patente_64PR] ([ID_Patente], [Nombre]) VALUES (17, N'Hacer restauraciones');
INSERT INTO [dbo].[Patente_64PR] ([ID_Patente], [Nombre]) VALUES (18, N'Crear categoria');
INSERT INTO [dbo].[Patente_64PR] ([ID_Patente], [Nombre]) VALUES (19, N'Modificar categoria');
INSERT INTO [dbo].[Patente_64PR] ([ID_Patente], [Nombre]) VALUES (20, N'Eliminar categoria');
INSERT INTO [dbo].[Patente_64PR] ([ID_Patente], [Nombre]) VALUES (21, N'Crear vehiculo');
INSERT INTO [dbo].[Patente_64PR] ([ID_Patente], [Nombre]) VALUES (22, N'Modificar vehiculo');
INSERT INTO [dbo].[Patente_64PR] ([ID_Patente], [Nombre]) VALUES (23, N'Eliminar vehiculo');
INSERT INTO [dbo].[Patente_64PR] ([ID_Patente], [Nombre]) VALUES (24, N'Cambiar estado vehiculo');
INSERT INTO [dbo].[Patente_64PR] ([ID_Patente], [Nombre]) VALUES (25, N'Crear cliente');
INSERT INTO [dbo].[Patente_64PR] ([ID_Patente], [Nombre]) VALUES (26, N'Modificar cliente');
INSERT INTO [dbo].[Patente_64PR] ([ID_Patente], [Nombre]) VALUES (27, N'Eliminar cliente');
INSERT INTO [dbo].[Patente_64PR] ([ID_Patente], [Nombre]) VALUES (28, N'Generar contrato');
INSERT INTO [dbo].[Patente_64PR] ([ID_Patente], [Nombre]) VALUES (29, N'Generar factura');
INSERT INTO [dbo].[Patente_64PR] ([ID_Patente], [Nombre]) VALUES (30, N'Registrar devolucion');
INSERT INTO [dbo].[Patente_64PR] ([ID_Patente], [Nombre]) VALUES (31, N'Registrar reporte');
INSERT INTO [dbo].[Patente_64PR] ([ID_Patente], [Nombre]) VALUES (32, N'Asignar criticidad');
INSERT INTO [dbo].[Patente_64PR] ([ID_Patente], [Nombre]) VALUES (33, N'Determinar modalidad');
INSERT INTO [dbo].[Patente_64PR] ([ID_Patente], [Nombre]) VALUES (34, N'Registrar acreditacion');
INSERT INTO [dbo].[Patente_64PR] ([ID_Patente], [Nombre]) VALUES (35, N'Ver bitacora de cambios de vehiculo');
INSERT INTO [dbo].[Patente_64PR] ([ID_Patente], [Nombre]) VALUES (36, N'Serializar clientes');
INSERT INTO [dbo].[Patente_64PR] ([ID_Patente], [Nombre]) VALUES (37, N'Deserializar clientes');
INSERT INTO [dbo].[Patente_64PR] ([ID_Patente], [Nombre]) VALUES (38, N'Reporte de mantenimiento preventivo');
SET IDENTITY_INSERT [dbo].[Patente_64PR] OFF;
GO

-- ======================================================================
-- DATOS: Roles_64PR (2 filas)
-- ======================================================================
SET IDENTITY_INSERT [dbo].[Roles_64PR] ON;
INSERT INTO [dbo].[Roles_64PR] ([ID_Rol], [Nombre]) VALUES (1, N'Basico');
INSERT INTO [dbo].[Roles_64PR] ([ID_Rol], [Nombre]) VALUES (2, N'Administrador');
SET IDENTITY_INSERT [dbo].[Roles_64PR] OFF;
GO

-- ======================================================================
-- DATOS: RolPatente_64PR (37 filas)
-- ======================================================================
INSERT INTO [dbo].[RolPatente_64PR] ([ID_Rol], [ID_Patente]) VALUES (2, 1);
INSERT INTO [dbo].[RolPatente_64PR] ([ID_Rol], [ID_Patente]) VALUES (2, 2);
INSERT INTO [dbo].[RolPatente_64PR] ([ID_Rol], [ID_Patente]) VALUES (2, 3);
INSERT INTO [dbo].[RolPatente_64PR] ([ID_Rol], [ID_Patente]) VALUES (2, 4);
INSERT INTO [dbo].[RolPatente_64PR] ([ID_Rol], [ID_Patente]) VALUES (2, 5);
INSERT INTO [dbo].[RolPatente_64PR] ([ID_Rol], [ID_Patente]) VALUES (2, 6);
INSERT INTO [dbo].[RolPatente_64PR] ([ID_Rol], [ID_Patente]) VALUES (2, 7);
INSERT INTO [dbo].[RolPatente_64PR] ([ID_Rol], [ID_Patente]) VALUES (2, 8);
INSERT INTO [dbo].[RolPatente_64PR] ([ID_Rol], [ID_Patente]) VALUES (2, 9);
INSERT INTO [dbo].[RolPatente_64PR] ([ID_Rol], [ID_Patente]) VALUES (2, 10);
INSERT INTO [dbo].[RolPatente_64PR] ([ID_Rol], [ID_Patente]) VALUES (2, 11);
INSERT INTO [dbo].[RolPatente_64PR] ([ID_Rol], [ID_Patente]) VALUES (2, 12);
INSERT INTO [dbo].[RolPatente_64PR] ([ID_Rol], [ID_Patente]) VALUES (2, 13);
INSERT INTO [dbo].[RolPatente_64PR] ([ID_Rol], [ID_Patente]) VALUES (2, 14);
INSERT INTO [dbo].[RolPatente_64PR] ([ID_Rol], [ID_Patente]) VALUES (2, 16);
INSERT INTO [dbo].[RolPatente_64PR] ([ID_Rol], [ID_Patente]) VALUES (2, 17);
INSERT INTO [dbo].[RolPatente_64PR] ([ID_Rol], [ID_Patente]) VALUES (2, 18);
INSERT INTO [dbo].[RolPatente_64PR] ([ID_Rol], [ID_Patente]) VALUES (2, 19);
INSERT INTO [dbo].[RolPatente_64PR] ([ID_Rol], [ID_Patente]) VALUES (2, 20);
INSERT INTO [dbo].[RolPatente_64PR] ([ID_Rol], [ID_Patente]) VALUES (2, 21);
INSERT INTO [dbo].[RolPatente_64PR] ([ID_Rol], [ID_Patente]) VALUES (2, 22);
INSERT INTO [dbo].[RolPatente_64PR] ([ID_Rol], [ID_Patente]) VALUES (2, 23);
INSERT INTO [dbo].[RolPatente_64PR] ([ID_Rol], [ID_Patente]) VALUES (2, 24);
INSERT INTO [dbo].[RolPatente_64PR] ([ID_Rol], [ID_Patente]) VALUES (2, 25);
INSERT INTO [dbo].[RolPatente_64PR] ([ID_Rol], [ID_Patente]) VALUES (2, 26);
INSERT INTO [dbo].[RolPatente_64PR] ([ID_Rol], [ID_Patente]) VALUES (2, 27);
INSERT INTO [dbo].[RolPatente_64PR] ([ID_Rol], [ID_Patente]) VALUES (2, 28);
INSERT INTO [dbo].[RolPatente_64PR] ([ID_Rol], [ID_Patente]) VALUES (2, 29);
INSERT INTO [dbo].[RolPatente_64PR] ([ID_Rol], [ID_Patente]) VALUES (2, 30);
INSERT INTO [dbo].[RolPatente_64PR] ([ID_Rol], [ID_Patente]) VALUES (2, 31);
INSERT INTO [dbo].[RolPatente_64PR] ([ID_Rol], [ID_Patente]) VALUES (2, 32);
INSERT INTO [dbo].[RolPatente_64PR] ([ID_Rol], [ID_Patente]) VALUES (2, 33);
INSERT INTO [dbo].[RolPatente_64PR] ([ID_Rol], [ID_Patente]) VALUES (2, 34);
INSERT INTO [dbo].[RolPatente_64PR] ([ID_Rol], [ID_Patente]) VALUES (2, 35);
INSERT INTO [dbo].[RolPatente_64PR] ([ID_Rol], [ID_Patente]) VALUES (2, 36);
INSERT INTO [dbo].[RolPatente_64PR] ([ID_Rol], [ID_Patente]) VALUES (2, 37);
INSERT INTO [dbo].[RolPatente_64PR] ([ID_Rol], [ID_Patente]) VALUES (2, 38);
GO

-- ======================================================================
-- DATOS: USUARIO_64PR (1 filas)
-- ======================================================================
INSERT INTO [dbo].[USUARIO_64PR] ([DNI], [Apellido], [Nombre], [Login], [Contraseña], [Rol], [Email], [Bloqueo], [Activo], [Intentos], [PrimeraVez], [Idioma]) VALUES (N'00000000', N'admin', N'admin', N'admin', 0x9BECE3951D133F985BF26F8F2C09499F6BFF426CE03896A649D09DE5BA6809955CCCF912308A6D1318C30EBB071026D0, 2, N'n5FBjHrHRuy7D+iU3r3VTmjZjkS/gGuwDZsmzaaPdBw=', 0, 1, 0, 1, N'es');
GO

-- ======================================================================
-- DATOS: VersionBDJP86 (1 fila)
-- ======================================================================
INSERT INTO [dbo].[VersionBDJP86] ([Version], [Descripcion]) VALUES ('2.0', N'Instalacion SIGAM 2.0');
GO

-- ======================================================================
-- RESTRICCIONES UNIQUE (6)
-- ======================================================================
ALTER TABLE [dbo].[Categoria_64PR] ADD CONSTRAINT [UQ_Categoria_Nombre] UNIQUE NONCLUSTERED ([Nombre]);
ALTER TABLE [dbo].[FacturaJP86] ADD CONSTRAINT [UQ_FacturaJP86_Contrato] UNIQUE NONCLUSTERED ([NumeroContrato]);
ALTER TABLE [dbo].[Modulo_64PR] ADD CONSTRAINT [UQ_Modulo_64PR_Nombre] UNIQUE NONCLUSTERED ([Nombre]);
ALTER TABLE [dbo].[USUARIO_64PR] ADD CONSTRAINT [UQ_USUARIO_64PR_Email] UNIQUE NONCLUSTERED ([Email]);
ALTER TABLE [dbo].[USUARIO_64PR] ADD CONSTRAINT [UQ_USUARIO_64PR_Login] UNIQUE NONCLUSTERED ([Login]);
ALTER TABLE [dbo].[VersionBDJP86] ADD CONSTRAINT [UQ_VersionBDJP86_Version] UNIQUE NONCLUSTERED ([Version]);
GO

-- ======================================================================
-- RESTRICCIONES CHECK (17)
-- ======================================================================
ALTER TABLE [dbo].[Categoria_64PR] WITH CHECK ADD CONSTRAINT [CK_Categoria_TarifaDiaria] CHECK ([TarifaDiaria]>(0));
ALTER TABLE [dbo].[ContratoJP86] WITH CHECK ADD CONSTRAINT [CK_ContratoJP86_Estado] CHECK ([Estado]='ACTIVO' OR [Estado]='FACTURADO' OR [Estado]='CERRADO');
ALTER TABLE [dbo].[ContratoJP86] WITH CHECK ADD CONSTRAINT [CK_ContratoJP86_EstadoUnidadDevolucion] CHECK ([EstadoUnidadDevolucion] IS NULL OR [EstadoUnidadDevolucion]='SIN_NOVEDADES' OR [EstadoUnidadDevolucion]='CON_OBSERVACIONES');
ALTER TABLE [dbo].[ContratoJP86] WITH CHECK ADD CONSTRAINT [CK_ContratoJP86_Fechas] CHECK ([FechaFin]>=[FechaInicio]);
ALTER TABLE [dbo].[ContratoJP86] WITH CHECK ADD CONSTRAINT [CK_ContratoJP86_ImporteTotal] CHECK ([ImporteTotal]>(0));
ALTER TABLE [dbo].[ContratoJP86] WITH CHECK ADD CONSTRAINT [CK_ContratoJP86_KilometrajeEntrega] CHECK ([KilometrajeEntrega]>=(0));
ALTER TABLE [dbo].[ContratoJP86] WITH CHECK ADD CONSTRAINT [CK_ContratoJP86_KilometrajeRetorno] CHECK ([KilometrajeRetorno] IS NULL OR [KilometrajeRetorno]>=[KilometrajeEntrega]);
ALTER TABLE [dbo].[ContratoJP86] WITH CHECK ADD CONSTRAINT [CK_ContratoJP86_TarifaDiaria] CHECK ([TarifaDiaria]>(0));
ALTER TABLE [dbo].[Evento_64PR] WITH CHECK ADD CONSTRAINT [CK_Evento_64PR_criticidad] CHECK ([criticidad]>=(1) AND [criticidad]<=(5));
ALTER TABLE [dbo].[FacturaJP86] WITH CHECK ADD CONSTRAINT [CK_FacturaJP86_MetodoPago] CHECK ([MetodoPago]='EFECTIVO' OR [MetodoPago]='TARJETA' OR [MetodoPago]='TRANSFERENCIA' OR [MetodoPago]='MERCADOPAGO');
ALTER TABLE [dbo].[FacturaJP86] WITH CHECK ADD CONSTRAINT [CK_FacturaJP86_MontoFactura] CHECK ([MontoFactura]>(0));
ALTER TABLE [dbo].[ReporteJP86] WITH CHECK ADD CONSTRAINT [CK_ReporteJP86_Criticidad] CHECK ([Criticidad] IS NULL OR ([Criticidad]='CRITICA' OR [Criticidad]='ALTA' OR [Criticidad]='MEDIA' OR [Criticidad]='BAJA'));
ALTER TABLE [dbo].[ReporteJP86] WITH CHECK ADD CONSTRAINT [CK_ReporteJP86_Estado] CHECK ([Estado]='ACREDITADO' OR [Estado]='EN_REPARACION' OR [Estado]='PENDIENTE');
ALTER TABLE [dbo].[ReporteJP86] WITH CHECK ADD CONSTRAINT [CK_ReporteJP86_Fuente] CHECK ([Fuente]='INSPECCION_INTERNA' OR [Fuente]='CLIENTE');
ALTER TABLE [dbo].[ReporteJP86] WITH CHECK ADD CONSTRAINT [CK_ReporteJP86_ModalidadReparacion] CHECK ([ModalidadReparacion] IS NULL OR ([ModalidadReparacion]='EXTERNA' OR [ModalidadReparacion]='INTERNA'));
ALTER TABLE [dbo].[VehiculoJP86] WITH CHECK ADD CONSTRAINT [CK_VehiculoJP86_Estado] CHECK ([Estado]='EN_REVISION' OR [Estado]='ALQUILADO' OR [Estado]='DISPONIBLE' OR [Estado]='EN_REPARACION');
ALTER TABLE [dbo].[VehiculoJP86] WITH CHECK ADD CONSTRAINT [CK_VehiculoJP86_Kilometraje] CHECK ([Kilometraje]>=(0));
GO

-- ======================================================================
-- CLAVES FORANEAS (19)
-- ======================================================================
ALTER TABLE [dbo].[ContratoJP86] WITH CHECK ADD CONSTRAINT [FK_ContratoJP86_Cliente] FOREIGN KEY ([DNI_Cliente]) REFERENCES [dbo].[ClienteJP86] ([DNI]);
ALTER TABLE [dbo].[ContratoJP86] WITH CHECK ADD CONSTRAINT [FK_ContratoJP86_Vehiculo] FOREIGN KEY ([Patente_Vehiculo]) REFERENCES [dbo].[VehiculoJP86] ([Patente]);
ALTER TABLE [dbo].[Evento_64PR] WITH CHECK ADD CONSTRAINT [FK_Bitacora_Modulo] FOREIGN KEY ([Id_modulo]) REFERENCES [dbo].[Modulo_64PR] ([Id_modulo]);
ALTER TABLE [dbo].[Evento_64PR] WITH CHECK ADD CONSTRAINT [FK_Bitacora_Evento] FOREIGN KEY ([Id_Tipo]) REFERENCES [dbo].[TipoEvento_64PR] ([Id_tipo]);
ALTER TABLE [dbo].[Evento_64PR] WITH CHECK ADD CONSTRAINT [FK_Bitacora_Usuario] FOREIGN KEY ([Login]) REFERENCES [dbo].[USUARIO_64PR] ([Login]);
ALTER TABLE [dbo].[FacturaJP86] WITH CHECK ADD CONSTRAINT [FK_FacturaJP86_Contrato] FOREIGN KEY ([NumeroContrato]) REFERENCES [dbo].[ContratoJP86] ([NumeroContrato]);
ALTER TABLE [dbo].[Familia_N_64PR] WITH CHECK ADD CONSTRAINT [FK_FamiliaN_Hija] FOREIGN KEY ([ID_FamiliaHija]) REFERENCES [dbo].[Familia_64PR] ([ID_Familia]);
ALTER TABLE [dbo].[Familia_N_64PR] WITH CHECK ADD CONSTRAINT [FK_FamiliaN_Padre] FOREIGN KEY ([ID_FamiliaPadre]) REFERENCES [dbo].[Familia_64PR] ([ID_Familia]) ON DELETE CASCADE;
ALTER TABLE [dbo].[PatenteFamilia_64PR] WITH CHECK ADD CONSTRAINT [FK_PatenteFamilia_Familia] FOREIGN KEY ([ID_Familia]) REFERENCES [dbo].[Familia_64PR] ([ID_Familia]) ON DELETE CASCADE;
ALTER TABLE [dbo].[PatenteFamilia_64PR] WITH CHECK ADD CONSTRAINT [FK_PatenteFamilia_64PR_ID_Patente] FOREIGN KEY ([ID_Patente]) REFERENCES [dbo].[Patente_64PR] ([ID_Patente]);
ALTER TABLE [dbo].[ReporteJP86] WITH CHECK ADD CONSTRAINT [FK_ReporteJP86_USUARIO_64PR] FOREIGN KEY ([DNI_ResponsableCierre]) REFERENCES [dbo].[USUARIO_64PR] ([DNI]);
ALTER TABLE [dbo].[ReporteJP86] WITH CHECK ADD CONSTRAINT [FK_ReporteJP86_VehiculoJP86] FOREIGN KEY ([Patente_Vehiculo]) REFERENCES [dbo].[VehiculoJP86] ([Patente]);
ALTER TABLE [dbo].[RolFamilia_64PR] WITH CHECK ADD CONSTRAINT [FK_RolFamilia_Familia] FOREIGN KEY ([ID_Familia]) REFERENCES [dbo].[Familia_64PR] ([ID_Familia]) ON DELETE CASCADE;
ALTER TABLE [dbo].[RolFamilia_64PR] WITH CHECK ADD CONSTRAINT [FK_RolFamilia_Rol] FOREIGN KEY ([ID_Rol]) REFERENCES [dbo].[Roles_64PR] ([ID_Rol]) ON DELETE CASCADE;
ALTER TABLE [dbo].[RolPatente_64PR] WITH CHECK ADD CONSTRAINT [FK_RolPatente_64PR_ID_Patente] FOREIGN KEY ([ID_Patente]) REFERENCES [dbo].[Patente_64PR] ([ID_Patente]);
ALTER TABLE [dbo].[RolPatente_64PR] WITH CHECK ADD CONSTRAINT [FK_RolPatente_Rol] FOREIGN KEY ([ID_Rol]) REFERENCES [dbo].[Roles_64PR] ([ID_Rol]) ON DELETE CASCADE;
ALTER TABLE [dbo].[USUARIO_64PR] WITH CHECK ADD CONSTRAINT [FK_Usuario_Rol] FOREIGN KEY ([Rol]) REFERENCES [dbo].[Roles_64PR] ([ID_Rol]);
ALTER TABLE [dbo].[Vehiculo_BitacoraCambios] WITH CHECK ADD CONSTRAINT [FK_VehiculoBitacoraCambios_VehiculoJP86] FOREIGN KEY ([Patente]) REFERENCES [dbo].[VehiculoJP86] ([Patente]);
ALTER TABLE [dbo].[VehiculoJP86] WITH CHECK ADD CONSTRAINT [FK_VehiculoJP86_Categoria] FOREIGN KEY ([ID_Categoria]) REFERENCES [dbo].[Categoria_64PR] ([ID_Categoria]);
GO

-- ======================================================================
-- INDICES (1)
-- ======================================================================
CREATE UNIQUE NONCLUSTERED INDEX [UX_VehiculoBitacoraCambios_Act] ON [dbo].[Vehiculo_BitacoraCambios] ([Patente]) WHERE ([Act]=(1));
GO

-- ======================================================================
-- PROCEDIMIENTOS ALMACENADOS (35)
-- ======================================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 CREATE PROCEDURE SP_Categoria_ActDesact
      @Id INT
  AS
  BEGIN
      SET NOCOUNT ON;
      UPDATE Categoria_64PR
      SET Activo = Activo ^ 1
      WHERE ID_Categoria = @Id;
  END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE SP_Categoria_Crear
      @Nombre VARCHAR(100),
      @Descripcion VARCHAR(255) = NULL,
      @TarifaDiaria DECIMAL(10,2)
  AS
  BEGIN
      SET NOCOUNT ON;
      INSERT INTO Categoria_64PR (Nombre, Descripcion, TarifaDiaria, Activo)
      VALUES (@Nombre, @Descripcion, @TarifaDiaria, 1);
  END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE SP_Categoria_Listar
  AS
  BEGIN
      SET NOCOUNT ON;
      SELECT ID_Categoria, Nombre, Descripcion, TarifaDiaria, Activo
      FROM Categoria_64PR;
  END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE SP_Categoria_Modificar
      @Id INT,
      @Nombre VARCHAR(100),
      @Descripcion VARCHAR(255) = NULL,
      @TarifaDiaria DECIMAL(10,2)
  AS
  BEGIN
      SET NOCOUNT ON;
      UPDATE Categoria_64PR
      SET Nombre = @Nombre,
          Descripcion = @Descripcion,
          TarifaDiaria = @TarifaDiaria
      WHERE ID_Categoria = @Id;
  END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 CREATE PROCEDURE SP_ClienteJP86_ActDesact
      @DNI NVARCHAR(20)
  AS
  BEGIN
      SET NOCOUNT ON;
      UPDATE ClienteJP86
      SET Activo = Activo ^ 1
      WHERE DNI = @DNI;
  END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 CREATE PROCEDURE SP_ClienteJP86_Crear
      @DNI NVARCHAR(20),
      @Nombre NVARCHAR(100),
      @Apellido NVARCHAR(100),
      @Telefono NVARCHAR(20)
  AS
  BEGIN
      SET NOCOUNT ON;
      INSERT INTO ClienteJP86 (DNI, Nombre, Apellido, Telefono, Activo)
      VALUES (@DNI, @Nombre, @Apellido, @Telefono, 1);
  END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_ClienteJP86_ExisteDNI]
    @DNI NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT CASE WHEN EXISTS (SELECT 1 FROM ClienteJP86 WHERE DNI = @DNI) THEN 1 ELSE 0 END AS Existe;
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE SP_ClienteJP86_Listar
  AS
  BEGIN
      SET NOCOUNT ON;
      SELECT DNI, Nombre, Apellido, Telefono, Activo
      FROM ClienteJP86;
  END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 CREATE PROCEDURE SP_ClienteJP86_Modificar
      @DNI NVARCHAR(20),
      @Nombre NVARCHAR(100),
      @Apellido NVARCHAR(100),
      @Telefono NVARCHAR(20)
  AS
  BEGIN
      SET NOCOUNT ON;
      UPDATE ClienteJP86
      SET Nombre = @Nombre,
          Apellido = @Apellido,
          Telefono = @Telefono
      WHERE DNI = @DNI;
  END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_ContratoJP86_BuscarUnidadesDisponibles]
    @ID_Categoria INT = NULL,
    @Marca NVARCHAR(50) = NULL,
    @KilometrajeMaximo INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT v.Patente, v.Marca, v.Modelo, v.ID_Categoria, v.Kilometraje, v.Estado, v.Activo,
           c.Nombre AS CategoriaNombre, c.TarifaDiaria
    FROM VehiculoJP86 v
    INNER JOIN Categoria_64PR c ON c.ID_Categoria = v.ID_Categoria
    WHERE v.Estado = 'DISPONIBLE' AND v.Activo = 1
      AND (@ID_Categoria IS NULL OR v.ID_Categoria = @ID_Categoria)
      AND (@Marca IS NULL OR v.Marca LIKE '%' + @Marca + '%')
      AND (@KilometrajeMaximo IS NULL OR v.Kilometraje <= @KilometrajeMaximo);
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_ContratoJP86_CerrarPorDevolucion]
    @NumeroContrato INT,
    @KilometrajeRetorno INT,
    @EstadoUnidadDevolucion VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE ContratoJP86
    SET KilometrajeRetorno = @KilometrajeRetorno,
        EstadoUnidadDevolucion = @EstadoUnidadDevolucion,
        Estado = 'CERRADO'
    WHERE NumeroContrato = @NumeroContrato AND Estado = 'FACTURADO';

    SELECT @@ROWCOUNT AS FilasAfectadas;
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_ContratoJP86_Crear]
    @DNI_Cliente NVARCHAR(20),
    @Patente_Vehiculo NVARCHAR(10),
    @FechaInicio DATE,
    @FechaFin DATE,
    @TarifaDiaria DECIMAL(10,2),
    @CargosAdicionales BIT,
    @ImporteTotal DECIMAL(10,2),
    @KilometrajeEntrega INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    DECLARE @NumeroContrato INT;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- 1) Primero se toma el lock sobre el vehículo: serializa altas concurrentes de la misma unidad
        IF NOT EXISTS (SELECT 1 FROM VehiculoJP86 WITH (UPDLOCK, HOLDLOCK)
                       WHERE Patente = @Patente_Vehiculo AND Estado = 'DISPONIBLE' AND Activo = 1)
        BEGIN
            ROLLBACK TRANSACTION;
            SELECT CAST(0 AS INT) AS NumeroContrato;
            RETURN;
        END

        -- 2) Con el lock tomado: ningún contrato vigente sobre la unidad y cliente activo
        IF EXISTS (SELECT 1 FROM ContratoJP86
                   WHERE Patente_Vehiculo = @Patente_Vehiculo AND Estado IN ('ACTIVO', 'FACTURADO'))
           OR NOT EXISTS (SELECT 1 FROM ClienteJP86 WHERE DNI = @DNI_Cliente AND Activo = 1)
        BEGIN
            ROLLBACK TRANSACTION;
            SELECT CAST(0 AS INT) AS NumeroContrato;
            RETURN;
        END

        INSERT INTO ContratoJP86 (DNI_Cliente, Patente_Vehiculo, FechaInicio, FechaFin, TarifaDiaria,
                                  CargosAdicionales, ImporteTotal, KilometrajeEntrega, Estado)
        VALUES (@DNI_Cliente, @Patente_Vehiculo, @FechaInicio, @FechaFin, @TarifaDiaria,
                @CargosAdicionales, @ImporteTotal, @KilometrajeEntrega, 'ACTIVO');

        SET @NumeroContrato = CAST(SCOPE_IDENTITY() AS INT);
        COMMIT TRANSACTION;
        SELECT @NumeroContrato AS NumeroContrato;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_ContratoJP86_ExisteAbiertoPorCliente]
      @DNI_Cliente NVARCHAR(20)
AS
BEGIN
      SET NOCOUNT ON;
      SELECT CASE WHEN EXISTS (
              SELECT 1 FROM ContratoJP86
              WHERE DNI_Cliente = @DNI_Cliente AND Estado IN ('ACTIVO', 'FACTURADO')
      ) THEN 1 ELSE 0 END
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_ContratoJP86_ExisteActivoPorVehiculo]
      @Patente_Vehiculo NVARCHAR(10)
AS
BEGIN
      SET NOCOUNT ON;
      SELECT CASE WHEN EXISTS (
              SELECT 1 FROM ContratoJP86
              WHERE Patente_Vehiculo = @Patente_Vehiculo AND Estado IN ('ACTIVO', 'FACTURADO')
      ) THEN 1 ELSE 0 END
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_ContratoJP86_ListarActivos]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT c.NumeroContrato, c.DNI_Cliente, cl.Nombre AS ClienteNombre, cl.Apellido AS ClienteApellido, cl.Telefono AS ClienteTelefono, cl.Activo AS ClienteActivo,
           c.Patente_Vehiculo, v.Marca, v.Modelo, v.ID_Categoria, cat.Nombre AS CategoriaNombre, cat.TarifaDiaria AS CategoriaTarifaDiaria,
           v.Kilometraje AS VehiculoKilometraje, v.Estado AS VehiculoEstado, v.Activo AS VehiculoActivo,
           c.FechaInicio, c.FechaFin, c.TarifaDiaria, c.CargosAdicionales, c.ImporteTotal, c.KilometrajeEntrega, c.KilometrajeRetorno, c.EstadoUnidadDevolucion, c.Estado
    FROM ContratoJP86 c
    INNER JOIN ClienteJP86 cl ON cl.DNI = c.DNI_Cliente
    INNER JOIN VehiculoJP86 v ON v.Patente = c.Patente_Vehiculo
    INNER JOIN Categoria_64PR cat ON cat.ID_Categoria = v.ID_Categoria
    WHERE c.Estado = 'ACTIVO';
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_ContratoJP86_ListarFacturados]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT c.NumeroContrato, c.DNI_Cliente, cl.Nombre AS ClienteNombre, cl.Apellido AS ClienteApellido, cl.Telefono AS ClienteTelefono, cl.Activo AS ClienteActivo,
           c.Patente_Vehiculo, v.Marca, v.Modelo, v.ID_Categoria, cat.Nombre AS CategoriaNombre, cat.TarifaDiaria AS CategoriaTarifaDiaria,
           v.Kilometraje AS VehiculoKilometraje, v.Estado AS VehiculoEstado, v.Activo AS VehiculoActivo,
           c.FechaInicio, c.FechaFin, c.TarifaDiaria, c.CargosAdicionales, c.ImporteTotal, c.KilometrajeEntrega, c.KilometrajeRetorno, c.EstadoUnidadDevolucion, c.Estado
    FROM ContratoJP86 c
    INNER JOIN ClienteJP86 cl ON cl.DNI = c.DNI_Cliente
    INNER JOIN VehiculoJP86 v ON v.Patente = c.Patente_Vehiculo
    INNER JOIN Categoria_64PR cat ON cat.ID_Categoria = v.ID_Categoria
    WHERE c.Estado = 'FACTURADO';
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_FacturaJP86_Crear]
    @NumeroContrato INT,
    @MetodoPago VARCHAR(20),
    @MontoFactura DECIMAL(10,2)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    DECLARE @NumeroFactura INT;

    BEGIN TRANSACTION
    BEGIN TRY
        UPDATE ContratoJP86
        SET Estado = 'FACTURADO'
        WHERE NumeroContrato = @NumeroContrato AND Estado = 'ACTIVO';

        IF @@ROWCOUNT = 0
        BEGIN
            ROLLBACK TRANSACTION;
            SELECT CAST(0 AS INT) AS NumeroFactura;
            RETURN;
        END

        INSERT INTO FacturaJP86 (NumeroContrato, FechaFactura, MetodoPago, MontoFactura)
        VALUES (@NumeroContrato, GETDATE(), @MetodoPago, @MontoFactura);

        SET @NumeroFactura = CAST(SCOPE_IDENTITY() AS INT);
        COMMIT TRANSACTION;
        SELECT @NumeroFactura AS NumeroFactura;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_ReporteJP86_Acreditar]
    @NumeroReporte INT,
    @DescripcionCierre NVARCHAR(500),
    @DNI_ResponsableCierre NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    DECLARE @Filas INT;
    DECLARE @Patente NVARCHAR(10);

    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE ReporteJP86
        SET DescripcionCierre = @DescripcionCierre,
            FechaAcreditacion = GETDATE(),
            DNI_ResponsableCierre = @DNI_ResponsableCierre,
            Estado = 'ACREDITADO'
        WHERE NumeroReporte = @NumeroReporte AND Estado = 'EN_REPARACION';

        SET @Filas = @@ROWCOUNT;

        IF @Filas > 0
        BEGIN
            SELECT @Patente = Patente_Vehiculo FROM ReporteJP86 WHERE NumeroReporte = @NumeroReporte;
            UPDATE VehiculoJP86 SET Estado = 'DISPONIBLE' WHERE Patente = @Patente;
        END

        COMMIT TRANSACTION;
        SELECT @Filas AS FilasAfectadas;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_ReporteJP86_AsignarCriticidadYModalidad]
    @NumeroReporte INT,
    @Criticidad VARCHAR(10),
    @ModalidadReparacion VARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    DECLARE @Filas INT;
    DECLARE @Patente NVARCHAR(10);

    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE ReporteJP86
        SET Criticidad = @Criticidad,
            ModalidadReparacion = @ModalidadReparacion,
            Estado = 'EN_REPARACION'
        WHERE NumeroReporte = @NumeroReporte AND Estado = 'PENDIENTE';

        SET @Filas = @@ROWCOUNT;

        IF @Filas > 0
        BEGIN
            SELECT @Patente = Patente_Vehiculo FROM ReporteJP86 WHERE NumeroReporte = @NumeroReporte;
            UPDATE VehiculoJP86 SET Estado = 'EN_REPARACION' WHERE Patente = @Patente;
        END

        COMMIT TRANSACTION;
        SELECT @Filas AS FilasAfectadas;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_ReporteJP86_Crear]
	@Patente_Vehiculo NVARCHAR(10),
	@DescripcionProblema NVARCHAR(500),
	@Fuente VARCHAR(20)
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO ReporteJP86 (Patente_Vehiculo, FechaHora, DescripcionProblema, Fuente, Estado)
	VALUES (@Patente_Vehiculo, GETDATE(), @DescripcionProblema, @Fuente, 'PENDIENTE')
	SELECT SCOPE_IDENTITY()
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_ReporteJP86_ExisteAbiertoPorVehiculo]
    @Patente_Vehiculo NVARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT CASE WHEN EXISTS (
        SELECT 1 FROM ReporteJP86
        WHERE Patente_Vehiculo = @Patente_Vehiculo AND Estado IN ('PENDIENTE','EN_REPARACION')
    ) THEN 1 ELSE 0 END
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_ReporteJP86_ListarEnReparacion]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT r.NumeroReporte, r.Patente_Vehiculo, v.Marca, v.Modelo, v.ID_Categoria,
	       c.Nombre AS CategoriaNombre, c.TarifaDiaria, v.Kilometraje AS VehiculoKilometraje,
	       v.Estado AS VehiculoEstado, v.Activo AS VehiculoActivo,
	       r.FechaHora, r.DescripcionProblema, r.Fuente, r.Criticidad, r.ModalidadReparacion,
	       r.DescripcionCierre, r.FechaAcreditacion, r.DNI_ResponsableCierre, r.Estado
	FROM ReporteJP86 r
	INNER JOIN VehiculoJP86 v ON v.Patente = r.Patente_Vehiculo
	INNER JOIN Categoria_64PR c ON c.ID_Categoria = v.ID_Categoria
	WHERE r.Estado = 'EN_REPARACION'
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_ReporteJP86_ListarPendientes]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT r.NumeroReporte, r.Patente_Vehiculo, v.Marca, v.Modelo, v.ID_Categoria,
	       c.Nombre AS CategoriaNombre, c.TarifaDiaria, v.Kilometraje AS VehiculoKilometraje,
	       v.Estado AS VehiculoEstado, v.Activo AS VehiculoActivo,
	       r.FechaHora, r.DescripcionProblema, r.Fuente, r.Criticidad, r.ModalidadReparacion,
	       r.DescripcionCierre, r.FechaAcreditacion, r.DNI_ResponsableCierre, r.Estado
	FROM ReporteJP86 r
	INNER JOIN VehiculoJP86 v ON v.Patente = r.Patente_Vehiculo
	INNER JOIN Categoria_64PR c ON c.ID_Categoria = v.ID_Categoria
	WHERE r.Estado = 'PENDIENTE'
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_ReporteJP86_ListarPorVehiculo]
	@Patente_Vehiculo NVARCHAR(10)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT r.NumeroReporte, r.Patente_Vehiculo, v.Marca, v.Modelo, v.ID_Categoria,
	       c.Nombre AS CategoriaNombre, c.TarifaDiaria, v.Kilometraje AS VehiculoKilometraje,
	       v.Estado AS VehiculoEstado, v.Activo AS VehiculoActivo,
	       r.FechaHora, r.DescripcionProblema, r.Fuente, r.Criticidad, r.ModalidadReparacion,
	       r.DescripcionCierre, r.FechaAcreditacion, r.DNI_ResponsableCierre, r.Estado
	FROM ReporteJP86 r
	INNER JOIN VehiculoJP86 v ON v.Patente = r.Patente_Vehiculo
	INNER JOIN Categoria_64PR c ON c.ID_Categoria = v.ID_Categoria
	WHERE r.Patente_Vehiculo = @Patente_Vehiculo
	ORDER BY r.FechaHora DESC
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_ReporteJP86_ListarVehiculosEnRevision]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT v.Patente, v.Marca, v.Modelo, v.ID_Categoria,
	       c.Nombre AS CategoriaNombre, c.TarifaDiaria,
	       v.Kilometraje, v.Estado, v.Activo
	FROM VehiculoJP86 v
	INNER JOIN Categoria_64PR c ON c.ID_Categoria = v.ID_Categoria
	WHERE v.Estado = 'EN_REVISION'
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_ReporteMantenimientoPreventivoJP86_Obtener]
    @FechaReferencia DATE,
    @DiasVentana INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @InicioVentana DATE = DATEADD(DAY, -@DiasVentana, @FechaReferencia);

    SELECT v.Patente, v.Marca, v.Modelo, v.ID_Categoria, c.Nombre AS CategoriaNombre,
           v.Kilometraje, v.Estado, v.Activo,
           ur.FechaAcreditacion      AS FechaUltimaRevision,
           dr.KilometrajeRetorno     AS KmDevolucionPreviaRevision,
           pc.FechaInicio            AS FechaPrimerContrato,
           pc.KilometrajeEntrega     AS KmEntregaPrimerContrato,
           ISNULL(ve.KmRecorridos, 0) AS KmRecorridosVentana,
           ve.FechaPrimerContrato    AS FechaPrimerContratoVentana,
           ISNULL(rp.ReportesInternos, 0) AS ReportesInternos,
           ISNULL(rp.ReportesCliente, 0)  AS ReportesCliente
    FROM VehiculoJP86 v
    INNER JOIN Categoria_64PR c ON c.ID_Categoria = v.ID_Categoria
    /* Ultimo reporte ACREDITADO (= ultima revision) */
    OUTER APPLY (SELECT TOP 1 r.FechaAcreditacion
                 FROM ReporteJP86 r
                 WHERE r.Patente_Vehiculo = v.Patente AND r.Estado = 'ACREDITADO'
                 ORDER BY r.FechaAcreditacion DESC) ur
    /* Km de devolucion del ultimo contrato devuelto antes (o el mismo dia) de esa revision */
    OUTER APPLY (SELECT TOP 1 k.KilometrajeRetorno
                 FROM ContratoJP86 k
                 WHERE k.Patente_Vehiculo = v.Patente
                   AND k.KilometrajeRetorno IS NOT NULL
                   AND ur.FechaAcreditacion IS NOT NULL
                   AND k.FechaFin <= CAST(ur.FechaAcreditacion AS DATE)
                 ORDER BY k.FechaFin DESC, k.NumeroContrato DESC) dr
    /* Primer contrato del vehiculo (fecha y km de entrega) */
    OUTER APPLY (SELECT TOP 1 k.FechaInicio, k.KilometrajeEntrega
                 FROM ContratoJP86 k
                 WHERE k.Patente_Vehiculo = v.Patente
                 ORDER BY k.FechaInicio ASC, k.NumeroContrato ASC) pc
    /* Uso dentro de la ventana: solo contratos con devolucion registrada */
    OUTER APPLY (SELECT SUM(k.KilometrajeRetorno - k.KilometrajeEntrega) AS KmRecorridos,
                        MIN(k.FechaInicio) AS FechaPrimerContrato
                 FROM ContratoJP86 k
                 WHERE k.Patente_Vehiculo = v.Patente
                   AND k.KilometrajeRetorno IS NOT NULL
                   AND k.FechaInicio >= @InicioVentana
                   AND k.FechaInicio <= @FechaReferencia) ve
    /* Reportes de desperfecto por origen (todos los estados, historico completo) */
    OUTER APPLY (SELECT SUM(CASE WHEN r.Fuente = 'INSPECCION_INTERNA' THEN 1 ELSE 0 END) AS ReportesInternos,
                        SUM(CASE WHEN r.Fuente = 'CLIENTE' THEN 1 ELSE 0 END) AS ReportesCliente
                 FROM ReporteJP86 r
                 WHERE r.Patente_Vehiculo = v.Patente) rp
    ORDER BY v.Patente;
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_VehiculoBitacoraCambios_Activar]
    @IdBitacora INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM [dbo].[Vehiculo_BitacoraCambios] WHERE IdBitacora = @IdBitacora)
    BEGIN
        RAISERROR('IdBitacora inexistente.', 16, 1);
        RETURN;
    END

    -- Evita generar una version duplicada al activar la que ya es vigente
    IF EXISTS (SELECT 1 FROM [dbo].[Vehiculo_BitacoraCambios] WHERE IdBitacora = @IdBitacora AND Act = 1)
    BEGIN
        RAISERROR('La version seleccionada ya es la vigente.', 16, 1);
        RETURN;
    END

    -- UPDATE normal sobre VehiculoJP86: dispara TR_VehiculoJP86_Modificacion,
    -- que crea la nueva fila vigente en la bitacora. Nunca se edita la fila historica.
    UPDATE v
    SET v.Marca        = bc.Marca,
        v.Modelo       = bc.Modelo,
        v.ID_Categoria = bc.ID_Categoria,
        v.Kilometraje  = bc.Kilometraje,
        v.Estado       = bc.Estado,
        v.Activo       = bc.Activo
    FROM [dbo].[VehiculoJP86] v
    INNER JOIN [dbo].[Vehiculo_BitacoraCambios] bc ON bc.Patente = v.Patente
    WHERE bc.IdBitacora = @IdBitacora;
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[SP_VehiculoBitacoraCambios_Listar]
    @Patente     NVARCHAR(10) = NULL,
    @MarcaModelo NVARCHAR(50) = NULL,
    @FechaIni    DATE = NULL,
    @FechaFin    DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        bc.IdBitacora, bc.Patente, bc.Marca, bc.Modelo, bc.ID_Categoria,
        bc.Kilometraje, bc.Estado, bc.Activo, bc.Fecha, bc.Hora, bc.Act
    FROM [dbo].[Vehiculo_BitacoraCambios] bc
    WHERE (@Patente IS NULL OR bc.Patente = @Patente)
      AND (@MarcaModelo IS NULL OR bc.Marca LIKE '%' + @MarcaModelo + '%' OR bc.Modelo LIKE '%' + @MarcaModelo + '%')
      AND (@FechaIni IS NULL OR bc.Fecha >= @FechaIni)
      AND (@FechaFin IS NULL OR bc.Fecha <= @FechaFin)
    ORDER BY bc.Patente, bc.Fecha DESC, bc.Hora DESC;
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE SP_VehiculoJP86_ActDesact
      @Patente NVARCHAR(10)
  AS
  BEGIN
      SET NOCOUNT ON;
      UPDATE VehiculoJP86
      SET Activo = Activo ^ 1
      WHERE Patente = @Patente;
  END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_VehiculoJP86_CambiarEstado]
    @Patente NVARCHAR(10),
    @Estado VARCHAR(20),
    @Kilometraje INT = NULL,
    @Activo BIT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE VehiculoJP86
    SET Estado = @Estado,
        Kilometraje = ISNULL(@Kilometraje, Kilometraje),
        Activo = ISNULL(@Activo, Activo)
    WHERE Patente = @Patente;
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE SP_VehiculoJP86_Crear
      @Patente NVARCHAR(10),
      @Marca NVARCHAR(50),
      @Modelo NVARCHAR(50),
      @ID_Categoria INT,
      @Kilometraje INT
  AS
  BEGIN
      SET NOCOUNT ON;
      INSERT INTO VehiculoJP86 (Patente, Marca, Modelo, ID_Categoria, Kilometraje, Estado, Activo)
      VALUES (@Patente, @Marca, @Modelo, @ID_Categoria, @Kilometraje, 'DISPONIBLE', 1);
  END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_VehiculoJP86_ExistePorCategoria]
      @ID_Categoria INT
AS
BEGIN
      SET NOCOUNT ON;
      SELECT CASE WHEN EXISTS (
              SELECT 1 FROM VehiculoJP86
              WHERE ID_Categoria = @ID_Categoria
                AND Activo = 1          -- ver pregunta abajo
      ) THEN 1 ELSE 0 END
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE SP_VehiculoJP86_Listar
  AS
  BEGIN
      SET NOCOUNT ON;
      SELECT v.Patente, v.Marca, v.Modelo, v.ID_Categoria, v.Kilometraje, v.Estado, v.Activo,
             c.Nombre AS CategoriaNombre, c.TarifaDiaria
      FROM VehiculoJP86 v
      INNER JOIN Categoria_64PR c ON c.ID_Categoria = v.ID_Categoria;
  END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE SP_VehiculoJP86_Modificar
      @Patente NVARCHAR(10),
      @Marca NVARCHAR(50),
      @Modelo NVARCHAR(50),
      @ID_Categoria INT,
      @Kilometraje INT
  AS
  BEGIN
      SET NOCOUNT ON;
      UPDATE VehiculoJP86
      SET Marca = @Marca,
          Modelo = @Modelo,
          ID_Categoria = @ID_Categoria,
          Kilometraje = @Kilometraje
      WHERE Patente = @Patente;
  END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_VehiculoJP86_ObtenerEstado]
	@Patente NVARCHAR(10)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT Estado FROM VehiculoJP86 WHERE Patente = @Patente
END
GO

-- ======================================================================
-- TRIGGERS (5)
-- ======================================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER trg_DV_NoModificarClave
ON dbo.DVH_64PR         -- la tabla con Tabla / IdFila / DVH
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF UPDATE(Tabla) OR UPDATE(IdFila)
    BEGIN
        ROLLBACK TRANSACTION;
        THROW 50001, 'No se permite modificar Tabla ni IdFila en la tabla de DV.', 1;
    END
END
GO
ALTER TABLE [dbo].[DVH_64PR] ENABLE TRIGGER [trg_DV_NoModificarClave]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/* =========================================================================
   2) Bloqueo de borrado: INSTEAD OF DELETE sobre la bitacora
   ========================================================================= */
CREATE   TRIGGER [dbo].[TR_VehiculoBitacoraCambios_BloqueaBorrado]
ON [dbo].[Vehiculo_BitacoraCambios]
INSTEAD OF DELETE
AS
BEGIN
    SET NOCOUNT ON;
    RAISERROR('No se permite eliminar registros de la bitacora de cambios de Vehiculo.', 16, 1);
END
GO
ALTER TABLE [dbo].[Vehiculo_BitacoraCambios] ENABLE TRIGGER [TR_VehiculoBitacoraCambios_BloqueaBorrado]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   TRIGGER [dbo].[TR_VehiculoJP86_Alta]
ON [dbo].[VehiculoJP86]
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    IF TRIGGER_NESTLEVEL() > 1 RETURN;

    INSERT INTO [dbo].[Vehiculo_BitacoraCambios]
        (Patente, Marca, Modelo, ID_Categoria, Kilometraje, Estado, Activo, Fecha, Hora, Act)
    SELECT
        i.Patente, i.Marca, i.Modelo, i.ID_Categoria, i.Kilometraje, i.Estado, i.Activo,
        CAST(GETDATE() AS DATE), CAST(GETDATE() AS TIME(0)), 1
    FROM inserted i;
END
GO
ALTER TABLE [dbo].[VehiculoJP86] ENABLE TRIGGER [TR_VehiculoJP86_Alta]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   TRIGGER [dbo].[TR_VehiculoJP86_BloqueaBorradoFisico]
ON [dbo].[VehiculoJP86]
INSTEAD OF DELETE
AS
BEGIN
    SET NOCOUNT ON;
    IF TRIGGER_NESTLEVEL() > 1 RETURN;

    RAISERROR('No se permite el borrado fisico de Vehiculo. La baja se realiza de forma logica (campo Activo).', 16, 1);
END
GO
ALTER TABLE [dbo].[VehiculoJP86] ENABLE TRIGGER [TR_VehiculoJP86_BloqueaBorradoFisico]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   TRIGGER [dbo].[TR_VehiculoJP86_Modificacion]
ON [dbo].[VehiculoJP86]
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF TRIGGER_NESTLEVEL() > 1 RETURN;

    -- 1) Bajo la fila vigente anterior de cada vehiculo modificado (UPDATE antes que INSERT
    --    para no violar el indice unico filtrado UX_VehiculoBitacoraCambios_Act)
    UPDATE bc
    SET bc.Act = 0
    FROM [dbo].[Vehiculo_BitacoraCambios] bc
    INNER JOIN deleted d ON d.Patente = bc.Patente
    WHERE bc.Act = 1;

    -- 2) Inserto la nueva fila vigente con los valores actuales post-cambio
    INSERT INTO [dbo].[Vehiculo_BitacoraCambios]
        (Patente, Marca, Modelo, ID_Categoria, Kilometraje, Estado, Activo, Fecha, Hora, Act)
    SELECT
        i.Patente, i.Marca, i.Modelo, i.ID_Categoria, i.Kilometraje, i.Estado, i.Activo,
        CAST(GETDATE() AS DATE), CAST(GETDATE() AS TIME(0)), 1
    FROM inserted i;
END
GO
ALTER TABLE [dbo].[VehiculoJP86] ENABLE TRIGGER [TR_VehiculoJP86_Modificacion]
GO
