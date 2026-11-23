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
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260329175111_AddIdToCharacter'
)
BEGIN
    CREATE TABLE [Characters] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        [Status] nvarchar(max) NOT NULL,
        [FirstAppearance] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Characters] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260329175111_AddIdToCharacter'
)
BEGIN
    CREATE TABLE [Entries] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        [Status] nvarchar(max) NOT NULL,
        [FirstAppearance] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Entries] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260329175111_AddIdToCharacter'
)
BEGIN
    CREATE TABLE [Games] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        [Status] nvarchar(max) NOT NULL,
        [FirstAppearance] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Games] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260329175111_AddIdToCharacter'
)
BEGIN
    CREATE TABLE [Locations] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        [Status] nvarchar(max) NOT NULL,
        [FirstAppearance] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Locations] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260329175111_AddIdToCharacter'
)
BEGIN
    CREATE TABLE [Users] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        [Status] nvarchar(max) NOT NULL,
        [FirstAppearance] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260329175111_AddIdToCharacter'
)
BEGIN
    CREATE TABLE [Viruses] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        [Status] nvarchar(max) NOT NULL,
        [FirstAppearance] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Viruses] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260329175111_AddIdToCharacter'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260329175111_AddIdToCharacter', N'10.0.5');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260329180837_SyncEntriesTable'
)
BEGIN
    DECLARE @var nvarchar(max);
    SELECT @var = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Entries]') AND [c].[name] = N'FirstAppearance');
    IF @var IS NOT NULL EXEC(N'ALTER TABLE [Entries] DROP CONSTRAINT ' + @var + ';');
    ALTER TABLE [Entries] DROP COLUMN [FirstAppearance];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260329180837_SyncEntriesTable'
)
BEGIN
    EXEC sp_rename N'[Entries].[Status]', N'Title', 'COLUMN';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260329180837_SyncEntriesTable'
)
BEGIN
    EXEC sp_rename N'[Entries].[Name]', N'Category', 'COLUMN';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260329180837_SyncEntriesTable'
)
BEGIN
    ALTER TABLE [Entries] ADD [Date] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260329180837_SyncEntriesTable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260329180837_SyncEntriesTable', N'10.0.5');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260329185214_UpdateTableStructures'
)
BEGIN
    DECLARE @var1 nvarchar(max);
    SELECT @var1 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Games]') AND [c].[name] = N'FirstAppearance');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Games] DROP CONSTRAINT ' + @var1 + ';');
    ALTER TABLE [Games] DROP COLUMN [FirstAppearance];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260329185214_UpdateTableStructures'
)
BEGIN
    EXEC sp_rename N'[Viruses].[Status]', N'Symptoms', 'COLUMN';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260329185214_UpdateTableStructures'
)
BEGIN
    EXEC sp_rename N'[Viruses].[FirstAppearance]', N'DiscoveryDate', 'COLUMN';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260329185214_UpdateTableStructures'
)
BEGIN
    EXEC sp_rename N'[Users].[Status]', N'Username', 'COLUMN';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260329185214_UpdateTableStructures'
)
BEGIN
    EXEC sp_rename N'[Users].[Name]', N'Role', 'COLUMN';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260329185214_UpdateTableStructures'
)
BEGIN
    EXEC sp_rename N'[Users].[FirstAppearance]', N'Password', 'COLUMN';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260329185214_UpdateTableStructures'
)
BEGIN
    EXEC sp_rename N'[Locations].[Status]', N'Type', 'COLUMN';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260329185214_UpdateTableStructures'
)
BEGIN
    EXEC sp_rename N'[Locations].[FirstAppearance]', N'City', 'COLUMN';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260329185214_UpdateTableStructures'
)
BEGIN
    EXEC sp_rename N'[Games].[Status]', N'Title', 'COLUMN';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260329185214_UpdateTableStructures'
)
BEGIN
    EXEC sp_rename N'[Games].[Name]', N'Platform', 'COLUMN';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260329185214_UpdateTableStructures'
)
BEGIN
    ALTER TABLE [Viruses] ADD [Creator] nvarchar(max) NOT NULL DEFAULT N'';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260329185214_UpdateTableStructures'
)
BEGIN
    ALTER TABLE [Users] ADD [Email] nvarchar(max) NOT NULL DEFAULT N'';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260329185214_UpdateTableStructures'
)
BEGIN
    ALTER TABLE [Games] ADD [ReleaseYear] int NOT NULL DEFAULT 0;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260329185214_UpdateTableStructures'
)
BEGIN
    ALTER TABLE [Characters] ADD [Affiliation] nvarchar(max) NOT NULL DEFAULT N'';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260329185214_UpdateTableStructures'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260329185214_UpdateTableStructures', N'10.0.5');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260329185429_UpdateToNewParams'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260329185429_UpdateToNewParams', N'10.0.5');
END;

COMMIT;
GO