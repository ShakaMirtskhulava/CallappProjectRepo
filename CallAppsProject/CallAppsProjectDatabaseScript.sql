IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230531200526_InitialMigration')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230531200526_InitialMigration', N'7.0.5');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230531202040_AddAPIUserEntityToTheDatabase')
BEGIN
    CREATE TABLE [APIUsers] (
        [APIUserId] int NOT NULL IDENTITY,
        [PasswordHash] nvarchar(200) NOT NULL,
        CONSTRAINT [PK_APIUsers] PRIMARY KEY ([APIUserId])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230531202040_AddAPIUserEntityToTheDatabase')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230531202040_AddAPIUserEntityToTheDatabase', N'7.0.5');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230531202617_AddUserNameColumntToAPIUserEntity')
BEGIN
    ALTER TABLE [APIUsers] ADD [UserName] nvarchar(100) NOT NULL DEFAULT N'';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230531202617_AddUserNameColumntToAPIUserEntity')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230531202617_AddUserNameColumntToAPIUserEntity', N'7.0.5');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230531203751_AddJWTColumntToAPIUserEntity')
BEGIN
    ALTER TABLE [APIUsers] ADD [JWT] nvarchar(max) NOT NULL DEFAULT N'';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230531203751_AddJWTColumntToAPIUserEntity')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230531203751_AddJWTColumntToAPIUserEntity', N'7.0.5');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230601074849_AddUserAndUserProfileEntitiesWithRelations')
BEGIN
    CREATE TABLE [Users] (
        [Username] nvarchar(450) NOT NULL,
        [Password] nvarchar(30) NOT NULL,
        [Email] nvarchar(max) NOT NULL,
        [IsActive] bit NOT NULL,
        CONSTRAINT [PK_Users] PRIMARY KEY ([Username])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230601074849_AddUserAndUserProfileEntitiesWithRelations')
BEGIN
    CREATE TABLE [UserProfiles] (
        [PersonalNumber] char(11) NOT NULL,
        [Firstname] nvarchar(30) NOT NULL,
        [Lastname] nvarchar(30) NOT NULL,
        [Username] nvarchar(450) NULL,
        CONSTRAINT [PK_UserProfiles] PRIMARY KEY ([PersonalNumber]),
        CONSTRAINT [FK_UserProfiles_Users_Username] FOREIGN KEY ([Username]) REFERENCES [Users] ([Username]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230601074849_AddUserAndUserProfileEntitiesWithRelations')
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_UserProfiles_Username] ON [UserProfiles] ([Username]) WHERE [Username] IS NOT NULL');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230601074849_AddUserAndUserProfileEntitiesWithRelations')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230601074849_AddUserAndUserProfileEntitiesWithRelations', N'7.0.5');
END;
GO

COMMIT;
GO

