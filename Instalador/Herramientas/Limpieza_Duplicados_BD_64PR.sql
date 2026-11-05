/*
   SIGAM - Limpieza de restricciones duplicadas en la BD de desarrollo (BD_64PR)
   Generado: 2026-11-05 13:57 junto con SIGAM_Instalacion.sql
   Deja exactamente las mismas UNIQUE / CHECK / FK que el script de instalacion.
   Todo corre en una transaccion: si algo falla, no se aplica nada.
   Recomendado: hacer un backup de BD_64PR antes de ejecutarlo.

   Duplicados eliminados:
     Modulo_64PR                [Nombre]                                 duplicados: 11 -> se conserva UQ_Modulo_64PR_Nombre
     USUARIO_64PR               [Email]                                  duplicados: 11 -> se conserva UQ_USUARIO_64PR_Email
     USUARIO_64PR               [Login]                                  duplicados: 11 -> se conserva UQ_USUARIO_64PR_Login
     Evento_64PR                ([criticidad]>=(1) AND [criticidad]<=(5)) duplicados: 11 -> se conserva CK_Evento_64PR_criticidad
     Familia_N_64PR             [ID_FamiliaHija] -> Familia_64PR[ID_Familia] duplicados: 12 -> se conserva FK_FamiliaN_Hija
     Familia_N_64PR             [ID_FamiliaPadre] -> Familia_64PR[ID_Familia] duplicados: 12 -> se conserva FK_FamiliaN_Padre
     PatenteFamilia_64PR        [ID_Familia] -> Familia_64PR[ID_Familia] duplicados: 12 -> se conserva FK_PatenteFamilia_Familia
     PatenteFamilia_64PR        [ID_Patente] -> Patente_64PR[ID_Patente] duplicados: 11 -> se conserva FK_PatenteFamilia_64PR_ID_Patente
     RolFamilia_64PR            [ID_Familia] -> Familia_64PR[ID_Familia] duplicados: 12 -> se conserva FK_RolFamilia_Familia
     RolFamilia_64PR            [ID_Rol] -> Roles_64PR[ID_Rol]           duplicados: 12 -> se conserva FK_RolFamilia_Rol
     RolPatente_64PR            [ID_Patente] -> Patente_64PR[ID_Patente] duplicados: 11 -> se conserva FK_RolPatente_64PR_ID_Patente
     RolPatente_64PR            [ID_Rol] -> Roles_64PR[ID_Rol]           duplicados: 12 -> se conserva FK_RolPatente_Rol

   Restricciones con nombre generado por el sistema que pasan a tener nombre explicito
   (el mismo que usa el script de instalacion; el codigo no referencia estos nombres):
     PK__Evento_6__D44FA6DEC9C35F6C -> PK_Evento_64PR
     PK__Familia___1DA11CE82734BF5C -> PK_Familia_64PR
     PK__Modulo_6__08C830BA96577AAE -> PK_Modulo_64PR
     PK__Patente___03A881DB1C596646 -> PK_Patente_64PR
     PK__PatenteF__BD9B94F582993932 -> PK_PatenteFamilia_64PR
     PK__Roles_64__202AD220B1CBAB97 -> PK_Roles_64PR
     PK__RolFamil__B1F0C3EE5D1EA4A3 -> PK_RolFamilia_64PR
     PK__RolPaten__80105A3DDB7C778D -> PK_RolPatente_64PR
     PK__TipoEven__70A6B7E75F48FC9A -> PK_TipoEvento_64PR
     PK__USUARIO___C035B8DC5454D208 -> PK_USUARIO_64PR
     DF__USUARIO_6__Activ__60A75C0F -> DF_USUARIO_64PR_Activo
     DF__USUARIO_6__Bloqu__5FB337D6 -> DF_USUARIO_64PR_Bloqueo
     DF__USUARIO_6__Idiom__6383C8BA -> DF_USUARIO_64PR_Idioma
     DF__USUARIO_6__Inten__619B8048 -> DF_USUARIO_64PR_Intentos
     DF__USUARIO_6__Prime__628FA481 -> DF_USUARIO_64PR_PrimeraVez
     UQ__Modulo_6__75E3EFCF6647CCE8 -> UQ_Modulo_64PR_Nombre
     UQ__USUARIO___A9D1053414985554 -> UQ_USUARIO_64PR_Email
     UQ__USUARIO___5E55825B4D975DDD -> UQ_USUARIO_64PR_Login
     CK__Evento_64__criti__498EEC8D -> CK_Evento_64PR_criticidad
     FK__PatenteFa__ID_Pa__0B91BA14 -> FK_PatenteFamilia_64PR_ID_Patente
     FK__RolPatent__ID_Pa__30C33EC3 -> FK_RolPatente_64PR_ID_Patente
*/
USE [BD_64PR];
GO
SET XACT_ABORT ON;
BEGIN TRY
    BEGIN TRANSACTION;
    EXEC sp_rename N'dbo.[PK__Evento_6__D44FA6DEC9C35F6C]', N'PK_Evento_64PR', N'OBJECT';
    EXEC sp_rename N'dbo.[PK__Familia___1DA11CE82734BF5C]', N'PK_Familia_64PR', N'OBJECT';
    EXEC sp_rename N'dbo.[PK__Modulo_6__08C830BA96577AAE]', N'PK_Modulo_64PR', N'OBJECT';
    EXEC sp_rename N'dbo.[PK__Patente___03A881DB1C596646]', N'PK_Patente_64PR', N'OBJECT';
    EXEC sp_rename N'dbo.[PK__PatenteF__BD9B94F582993932]', N'PK_PatenteFamilia_64PR', N'OBJECT';
    EXEC sp_rename N'dbo.[PK__Roles_64__202AD220B1CBAB97]', N'PK_Roles_64PR', N'OBJECT';
    EXEC sp_rename N'dbo.[PK__RolFamil__B1F0C3EE5D1EA4A3]', N'PK_RolFamilia_64PR', N'OBJECT';
    EXEC sp_rename N'dbo.[PK__RolPaten__80105A3DDB7C778D]', N'PK_RolPatente_64PR', N'OBJECT';
    EXEC sp_rename N'dbo.[PK__TipoEven__70A6B7E75F48FC9A]', N'PK_TipoEvento_64PR', N'OBJECT';
    EXEC sp_rename N'dbo.[PK__USUARIO___C035B8DC5454D208]', N'PK_USUARIO_64PR', N'OBJECT';
    EXEC sp_rename N'dbo.[DF__USUARIO_6__Activ__60A75C0F]', N'DF_USUARIO_64PR_Activo', N'OBJECT';
    EXEC sp_rename N'dbo.[DF__USUARIO_6__Bloqu__5FB337D6]', N'DF_USUARIO_64PR_Bloqueo', N'OBJECT';
    EXEC sp_rename N'dbo.[DF__USUARIO_6__Idiom__6383C8BA]', N'DF_USUARIO_64PR_Idioma', N'OBJECT';
    EXEC sp_rename N'dbo.[DF__USUARIO_6__Inten__619B8048]', N'DF_USUARIO_64PR_Intentos', N'OBJECT';
    EXEC sp_rename N'dbo.[DF__USUARIO_6__Prime__628FA481]', N'DF_USUARIO_64PR_PrimeraVez', N'OBJECT';
    ALTER TABLE [dbo].[Modulo_64PR] DROP CONSTRAINT [UQ__Modulo_6__75E3EFCF52E54EE7];
    ALTER TABLE [dbo].[Modulo_64PR] DROP CONSTRAINT [UQ__Modulo_6__75E3EFCF6CA381CA];
    ALTER TABLE [dbo].[Modulo_64PR] DROP CONSTRAINT [UQ__Modulo_6__75E3EFCF8557F54E];
    ALTER TABLE [dbo].[Modulo_64PR] DROP CONSTRAINT [UQ__Modulo_6__75E3EFCF9A42C7C0];
    ALTER TABLE [dbo].[Modulo_64PR] DROP CONSTRAINT [UQ__Modulo_6__75E3EFCFE7140EDB];
    ALTER TABLE [dbo].[Modulo_64PR] DROP CONSTRAINT [UQ__Modulo_6__75E3EFCF0B124080];
    ALTER TABLE [dbo].[Modulo_64PR] DROP CONSTRAINT [UQ__Modulo_6__75E3EFCFA90D677E];
    ALTER TABLE [dbo].[Modulo_64PR] DROP CONSTRAINT [UQ__Modulo_6__75E3EFCF41B48B16];
    ALTER TABLE [dbo].[Modulo_64PR] DROP CONSTRAINT [UQ__Modulo_6__75E3EFCF03BABADA];
    ALTER TABLE [dbo].[Modulo_64PR] DROP CONSTRAINT [UQ__Modulo_6__75E3EFCFF8FF973E];
    ALTER TABLE [dbo].[Modulo_64PR] DROP CONSTRAINT [UQ__Modulo_6__75E3EFCF8DCDE625];
    EXEC sp_rename N'dbo.[UQ__Modulo_6__75E3EFCF6647CCE8]', N'UQ_Modulo_64PR_Nombre', N'OBJECT';
    ALTER TABLE [dbo].[USUARIO_64PR] DROP CONSTRAINT [UQ__USUARIO___A9D105341803A651];
    ALTER TABLE [dbo].[USUARIO_64PR] DROP CONSTRAINT [UQ__USUARIO___A9D105347EADDACF];
    ALTER TABLE [dbo].[USUARIO_64PR] DROP CONSTRAINT [UQ__USUARIO___A9D105342C983392];
    ALTER TABLE [dbo].[USUARIO_64PR] DROP CONSTRAINT [UQ__USUARIO___A9D105348DC1515A];
    ALTER TABLE [dbo].[USUARIO_64PR] DROP CONSTRAINT [UQ__USUARIO___A9D105348AE47831];
    ALTER TABLE [dbo].[USUARIO_64PR] DROP CONSTRAINT [UQ__USUARIO___A9D10534AD1214B9];
    ALTER TABLE [dbo].[USUARIO_64PR] DROP CONSTRAINT [UQ__USUARIO___A9D10534C8DE55BB];
    ALTER TABLE [dbo].[USUARIO_64PR] DROP CONSTRAINT [UQ__USUARIO___A9D10534B14B5C90];
    ALTER TABLE [dbo].[USUARIO_64PR] DROP CONSTRAINT [UQ__USUARIO___A9D105342B9CF9E8];
    ALTER TABLE [dbo].[USUARIO_64PR] DROP CONSTRAINT [UQ__USUARIO___A9D10534EF8DC08F];
    ALTER TABLE [dbo].[USUARIO_64PR] DROP CONSTRAINT [UQ__USUARIO___A9D1053429DC4911];
    EXEC sp_rename N'dbo.[UQ__USUARIO___A9D1053414985554]', N'UQ_USUARIO_64PR_Email', N'OBJECT';
    ALTER TABLE [dbo].[USUARIO_64PR] DROP CONSTRAINT [UQ__USUARIO___5E55825B482B2360];
    ALTER TABLE [dbo].[USUARIO_64PR] DROP CONSTRAINT [UQ__USUARIO___5E55825BEE974CB4];
    ALTER TABLE [dbo].[USUARIO_64PR] DROP CONSTRAINT [UQ__USUARIO___5E55825B2171E542];
    ALTER TABLE [dbo].[USUARIO_64PR] DROP CONSTRAINT [UQ__USUARIO___5E55825B4F834CB8];
    ALTER TABLE [dbo].[USUARIO_64PR] DROP CONSTRAINT [UQ__USUARIO___5E55825B3557C719];
    ALTER TABLE [dbo].[USUARIO_64PR] DROP CONSTRAINT [UQ__USUARIO___5E55825B1CA49FB7];
    ALTER TABLE [dbo].[USUARIO_64PR] DROP CONSTRAINT [UQ__USUARIO___5E55825BA2EBE124];
    ALTER TABLE [dbo].[USUARIO_64PR] DROP CONSTRAINT [UQ__USUARIO___5E55825B22A1E8F0];
    ALTER TABLE [dbo].[USUARIO_64PR] DROP CONSTRAINT [UQ__USUARIO___5E55825BCE25049D];
    ALTER TABLE [dbo].[USUARIO_64PR] DROP CONSTRAINT [UQ__USUARIO___5E55825B1ABDD653];
    ALTER TABLE [dbo].[USUARIO_64PR] DROP CONSTRAINT [UQ__USUARIO___5E55825B6BB987C2];
    EXEC sp_rename N'dbo.[UQ__USUARIO___5E55825B4D975DDD]', N'UQ_USUARIO_64PR_Login', N'OBJECT';
    ALTER TABLE [dbo].[Evento_64PR] DROP CONSTRAINT [CK__Evento_64__criti__4A8310C6];
    ALTER TABLE [dbo].[Evento_64PR] DROP CONSTRAINT [CK__Evento_64__criti__4B7734FF];
    ALTER TABLE [dbo].[Evento_64PR] DROP CONSTRAINT [CK__Evento_64__criti__4C6B5938];
    ALTER TABLE [dbo].[Evento_64PR] DROP CONSTRAINT [CK__Evento_64__criti__4D5F7D71];
    ALTER TABLE [dbo].[Evento_64PR] DROP CONSTRAINT [CK__Evento_64__criti__4E53A1AA];
    ALTER TABLE [dbo].[Evento_64PR] DROP CONSTRAINT [CK__Evento_64__criti__4F47C5E3];
    ALTER TABLE [dbo].[Evento_64PR] DROP CONSTRAINT [CK__Evento_64__criti__503BEA1C];
    ALTER TABLE [dbo].[Evento_64PR] DROP CONSTRAINT [CK__Evento_64__criti__51300E55];
    ALTER TABLE [dbo].[Evento_64PR] DROP CONSTRAINT [CK__Evento_64__criti__5224328E];
    ALTER TABLE [dbo].[Evento_64PR] DROP CONSTRAINT [CK__Evento_64__criti__531856C7];
    ALTER TABLE [dbo].[Evento_64PR] DROP CONSTRAINT [CK__Evento_64__criti__540C7B00];
    EXEC sp_rename N'dbo.[CK__Evento_64__criti__498EEC8D]', N'CK_Evento_64PR_criticidad', N'OBJECT';
    ALTER TABLE [dbo].[Familia_N_64PR] DROP CONSTRAINT [FK__Familia_N__ID_Fa__68487DD7];
    ALTER TABLE [dbo].[Familia_N_64PR] DROP CONSTRAINT [FK__Familia_N__ID_Fa__6A30C649];
    ALTER TABLE [dbo].[Familia_N_64PR] DROP CONSTRAINT [FK__Familia_N__ID_Fa__6C190EBB];
    ALTER TABLE [dbo].[Familia_N_64PR] DROP CONSTRAINT [FK__Familia_N__ID_Fa__6E01572D];
    ALTER TABLE [dbo].[Familia_N_64PR] DROP CONSTRAINT [FK__Familia_N__ID_Fa__6FE99F9F];
    ALTER TABLE [dbo].[Familia_N_64PR] DROP CONSTRAINT [FK__Familia_N__ID_Fa__71D1E811];
    ALTER TABLE [dbo].[Familia_N_64PR] DROP CONSTRAINT [FK__Familia_N__ID_Fa__73BA3083];
    ALTER TABLE [dbo].[Familia_N_64PR] DROP CONSTRAINT [FK__Familia_N__ID_Fa__75A278F5];
    ALTER TABLE [dbo].[Familia_N_64PR] DROP CONSTRAINT [FK__Familia_N__ID_Fa__778AC167];
    ALTER TABLE [dbo].[Familia_N_64PR] DROP CONSTRAINT [FK__Familia_N__ID_Fa__797309D9];
    ALTER TABLE [dbo].[Familia_N_64PR] DROP CONSTRAINT [FK__Familia_N__ID_Fa__7B5B524B];
    ALTER TABLE [dbo].[Familia_N_64PR] DROP CONSTRAINT [FK__Familia_N__ID_Fa__7D439ABD];
    ALTER TABLE [dbo].[Familia_N_64PR] DROP CONSTRAINT [FK__Familia_N__ID_Fa__6754599E];
    ALTER TABLE [dbo].[Familia_N_64PR] DROP CONSTRAINT [FK__Familia_N__ID_Fa__693CA210];
    ALTER TABLE [dbo].[Familia_N_64PR] DROP CONSTRAINT [FK__Familia_N__ID_Fa__6B24EA82];
    ALTER TABLE [dbo].[Familia_N_64PR] DROP CONSTRAINT [FK__Familia_N__ID_Fa__6D0D32F4];
    ALTER TABLE [dbo].[Familia_N_64PR] DROP CONSTRAINT [FK__Familia_N__ID_Fa__6EF57B66];
    ALTER TABLE [dbo].[Familia_N_64PR] DROP CONSTRAINT [FK__Familia_N__ID_Fa__70DDC3D8];
    ALTER TABLE [dbo].[Familia_N_64PR] DROP CONSTRAINT [FK__Familia_N__ID_Fa__72C60C4A];
    ALTER TABLE [dbo].[Familia_N_64PR] DROP CONSTRAINT [FK__Familia_N__ID_Fa__74AE54BC];
    ALTER TABLE [dbo].[Familia_N_64PR] DROP CONSTRAINT [FK__Familia_N__ID_Fa__76969D2E];
    ALTER TABLE [dbo].[Familia_N_64PR] DROP CONSTRAINT [FK__Familia_N__ID_Fa__787EE5A0];
    ALTER TABLE [dbo].[Familia_N_64PR] DROP CONSTRAINT [FK__Familia_N__ID_Fa__7A672E12];
    ALTER TABLE [dbo].[Familia_N_64PR] DROP CONSTRAINT [FK__Familia_N__ID_Fa__7C4F7684];
    ALTER TABLE [dbo].[PatenteFamilia_64PR] DROP CONSTRAINT [FK__PatenteFa__ID_Fa__00200768];
    ALTER TABLE [dbo].[PatenteFamilia_64PR] DROP CONSTRAINT [FK__PatenteFa__ID_Fa__01142BA1];
    ALTER TABLE [dbo].[PatenteFamilia_64PR] DROP CONSTRAINT [FK__PatenteFa__ID_Fa__02084FDA];
    ALTER TABLE [dbo].[PatenteFamilia_64PR] DROP CONSTRAINT [FK__PatenteFa__ID_Fa__02FC7413];
    ALTER TABLE [dbo].[PatenteFamilia_64PR] DROP CONSTRAINT [FK__PatenteFa__ID_Fa__03F0984C];
    ALTER TABLE [dbo].[PatenteFamilia_64PR] DROP CONSTRAINT [FK__PatenteFa__ID_Fa__04E4BC85];
    ALTER TABLE [dbo].[PatenteFamilia_64PR] DROP CONSTRAINT [FK__PatenteFa__ID_Fa__05D8E0BE];
    ALTER TABLE [dbo].[PatenteFamilia_64PR] DROP CONSTRAINT [FK__PatenteFa__ID_Fa__06CD04F7];
    ALTER TABLE [dbo].[PatenteFamilia_64PR] DROP CONSTRAINT [FK__PatenteFa__ID_Fa__07C12930];
    ALTER TABLE [dbo].[PatenteFamilia_64PR] DROP CONSTRAINT [FK__PatenteFa__ID_Fa__08B54D69];
    ALTER TABLE [dbo].[PatenteFamilia_64PR] DROP CONSTRAINT [FK__PatenteFa__ID_Fa__09A971A2];
    ALTER TABLE [dbo].[PatenteFamilia_64PR] DROP CONSTRAINT [FK__PatenteFa__ID_Fa__0A9D95DB];
    ALTER TABLE [dbo].[PatenteFamilia_64PR] DROP CONSTRAINT [FK__PatenteFa__ID_Pa__0C85DE4D];
    ALTER TABLE [dbo].[PatenteFamilia_64PR] DROP CONSTRAINT [FK__PatenteFa__ID_Pa__0D7A0286];
    ALTER TABLE [dbo].[PatenteFamilia_64PR] DROP CONSTRAINT [FK__PatenteFa__ID_Pa__0E6E26BF];
    ALTER TABLE [dbo].[PatenteFamilia_64PR] DROP CONSTRAINT [FK__PatenteFa__ID_Pa__0F624AF8];
    ALTER TABLE [dbo].[PatenteFamilia_64PR] DROP CONSTRAINT [FK__PatenteFa__ID_Pa__10566F31];
    ALTER TABLE [dbo].[PatenteFamilia_64PR] DROP CONSTRAINT [FK__PatenteFa__ID_Pa__114A936A];
    ALTER TABLE [dbo].[PatenteFamilia_64PR] DROP CONSTRAINT [FK__PatenteFa__ID_Pa__123EB7A3];
    ALTER TABLE [dbo].[PatenteFamilia_64PR] DROP CONSTRAINT [FK__PatenteFa__ID_Pa__1332DBDC];
    ALTER TABLE [dbo].[PatenteFamilia_64PR] DROP CONSTRAINT [FK__PatenteFa__ID_Pa__14270015];
    ALTER TABLE [dbo].[PatenteFamilia_64PR] DROP CONSTRAINT [FK__PatenteFa__ID_Pa__151B244E];
    ALTER TABLE [dbo].[PatenteFamilia_64PR] DROP CONSTRAINT [FK__PatenteFa__ID_Pa__160F4887];
    EXEC sp_rename N'dbo.[FK__PatenteFa__ID_Pa__0B91BA14]', N'FK_PatenteFamilia_64PR_ID_Patente', N'OBJECT';
    ALTER TABLE [dbo].[RolFamilia_64PR] DROP CONSTRAINT [FK__RolFamili__ID_Fa__17F790F9];
    ALTER TABLE [dbo].[RolFamilia_64PR] DROP CONSTRAINT [FK__RolFamili__ID_Fa__18EBB532];
    ALTER TABLE [dbo].[RolFamilia_64PR] DROP CONSTRAINT [FK__RolFamili__ID_Fa__19DFD96B];
    ALTER TABLE [dbo].[RolFamilia_64PR] DROP CONSTRAINT [FK__RolFamili__ID_Fa__1AD3FDA4];
    ALTER TABLE [dbo].[RolFamilia_64PR] DROP CONSTRAINT [FK__RolFamili__ID_Fa__1BC821DD];
    ALTER TABLE [dbo].[RolFamilia_64PR] DROP CONSTRAINT [FK__RolFamili__ID_Fa__1CBC4616];
    ALTER TABLE [dbo].[RolFamilia_64PR] DROP CONSTRAINT [FK__RolFamili__ID_Fa__1DB06A4F];
    ALTER TABLE [dbo].[RolFamilia_64PR] DROP CONSTRAINT [FK__RolFamili__ID_Fa__1EA48E88];
    ALTER TABLE [dbo].[RolFamilia_64PR] DROP CONSTRAINT [FK__RolFamili__ID_Fa__1F98B2C1];
    ALTER TABLE [dbo].[RolFamilia_64PR] DROP CONSTRAINT [FK__RolFamili__ID_Fa__208CD6FA];
    ALTER TABLE [dbo].[RolFamilia_64PR] DROP CONSTRAINT [FK__RolFamili__ID_Fa__2180FB33];
    ALTER TABLE [dbo].[RolFamilia_64PR] DROP CONSTRAINT [FK__RolFamili__ID_Fa__22751F6C];
    ALTER TABLE [dbo].[RolFamilia_64PR] DROP CONSTRAINT [FK__RolFamili__ID_Ro__236943A5];
    ALTER TABLE [dbo].[RolFamilia_64PR] DROP CONSTRAINT [FK__RolFamili__ID_Ro__245D67DE];
    ALTER TABLE [dbo].[RolFamilia_64PR] DROP CONSTRAINT [FK__RolFamili__ID_Ro__25518C17];
    ALTER TABLE [dbo].[RolFamilia_64PR] DROP CONSTRAINT [FK__RolFamili__ID_Ro__2645B050];
    ALTER TABLE [dbo].[RolFamilia_64PR] DROP CONSTRAINT [FK__RolFamili__ID_Ro__2739D489];
    ALTER TABLE [dbo].[RolFamilia_64PR] DROP CONSTRAINT [FK__RolFamili__ID_Ro__282DF8C2];
    ALTER TABLE [dbo].[RolFamilia_64PR] DROP CONSTRAINT [FK__RolFamili__ID_Ro__29221CFB];
    ALTER TABLE [dbo].[RolFamilia_64PR] DROP CONSTRAINT [FK__RolFamili__ID_Ro__2A164134];
    ALTER TABLE [dbo].[RolFamilia_64PR] DROP CONSTRAINT [FK__RolFamili__ID_Ro__2B0A656D];
    ALTER TABLE [dbo].[RolFamilia_64PR] DROP CONSTRAINT [FK__RolFamili__ID_Ro__2BFE89A6];
    ALTER TABLE [dbo].[RolFamilia_64PR] DROP CONSTRAINT [FK__RolFamili__ID_Ro__2CF2ADDF];
    ALTER TABLE [dbo].[RolFamilia_64PR] DROP CONSTRAINT [FK__RolFamili__ID_Ro__2DE6D218];
    ALTER TABLE [dbo].[RolPatente_64PR] DROP CONSTRAINT [FK__RolPatent__ID_Pa__31B762FC];
    ALTER TABLE [dbo].[RolPatente_64PR] DROP CONSTRAINT [FK__RolPatent__ID_Pa__32AB8735];
    ALTER TABLE [dbo].[RolPatente_64PR] DROP CONSTRAINT [FK__RolPatent__ID_Pa__339FAB6E];
    ALTER TABLE [dbo].[RolPatente_64PR] DROP CONSTRAINT [FK__RolPatent__ID_Pa__3493CFA7];
    ALTER TABLE [dbo].[RolPatente_64PR] DROP CONSTRAINT [FK__RolPatent__ID_Pa__3587F3E0];
    ALTER TABLE [dbo].[RolPatente_64PR] DROP CONSTRAINT [FK__RolPatent__ID_Pa__367C1819];
    ALTER TABLE [dbo].[RolPatente_64PR] DROP CONSTRAINT [FK__RolPatent__ID_Pa__37703C52];
    ALTER TABLE [dbo].[RolPatente_64PR] DROP CONSTRAINT [FK__RolPatent__ID_Pa__3864608B];
    ALTER TABLE [dbo].[RolPatente_64PR] DROP CONSTRAINT [FK__RolPatent__ID_Pa__395884C4];
    ALTER TABLE [dbo].[RolPatente_64PR] DROP CONSTRAINT [FK__RolPatent__ID_Pa__3A4CA8FD];
    ALTER TABLE [dbo].[RolPatente_64PR] DROP CONSTRAINT [FK__RolPatent__ID_Pa__3B40CD36];
    EXEC sp_rename N'dbo.[FK__RolPatent__ID_Pa__30C33EC3]', N'FK_RolPatente_64PR_ID_Patente', N'OBJECT';
    ALTER TABLE [dbo].[RolPatente_64PR] DROP CONSTRAINT [FK__RolPatent__ID_Ro__3C34F16F];
    ALTER TABLE [dbo].[RolPatente_64PR] DROP CONSTRAINT [FK__RolPatent__ID_Ro__3D2915A8];
    ALTER TABLE [dbo].[RolPatente_64PR] DROP CONSTRAINT [FK__RolPatent__ID_Ro__3E1D39E1];
    ALTER TABLE [dbo].[RolPatente_64PR] DROP CONSTRAINT [FK__RolPatent__ID_Ro__3F115E1A];
    ALTER TABLE [dbo].[RolPatente_64PR] DROP CONSTRAINT [FK__RolPatent__ID_Ro__40058253];
    ALTER TABLE [dbo].[RolPatente_64PR] DROP CONSTRAINT [FK__RolPatent__ID_Ro__40F9A68C];
    ALTER TABLE [dbo].[RolPatente_64PR] DROP CONSTRAINT [FK__RolPatent__ID_Ro__41EDCAC5];
    ALTER TABLE [dbo].[RolPatente_64PR] DROP CONSTRAINT [FK__RolPatent__ID_Ro__42E1EEFE];
    ALTER TABLE [dbo].[RolPatente_64PR] DROP CONSTRAINT [FK__RolPatent__ID_Ro__43D61337];
    ALTER TABLE [dbo].[RolPatente_64PR] DROP CONSTRAINT [FK__RolPatent__ID_Ro__44CA3770];
    ALTER TABLE [dbo].[RolPatente_64PR] DROP CONSTRAINT [FK__RolPatent__ID_Ro__45BE5BA9];
    ALTER TABLE [dbo].[RolPatente_64PR] DROP CONSTRAINT [FK__RolPatent__ID_Ro__46B27FE2];
    COMMIT TRANSACTION;
    PRINT 'Limpieza aplicada correctamente.';
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
    THROW;
END CATCH;
GO
-- Verificacion (esperado: 21 PK, 19 DEFAULT, 6 UNIQUE, 17 CHECK, 19 FK, 0 con nombre de sistema)
SELECT (SELECT COUNT(*) FROM sys.key_constraints WHERE type = 'PK') AS PrimaryKeys,
       (SELECT COUNT(*) FROM sys.default_constraints) AS Defaults,
       (SELECT COUNT(*) FROM sys.key_constraints WHERE type = 'UQ') AS UniqueKeys,
       (SELECT COUNT(*) FROM sys.check_constraints) AS Checks,
       (SELECT COUNT(*) FROM sys.foreign_keys) AS ForeignKeys,
       (SELECT COUNT(*) FROM sys.objects WHERE type IN ('PK','UQ','C','F','D') AND OBJECTPROPERTY(object_id, 'IsSystemNamed') = 1) AS ConNombreDeSistema;
GO
