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

CREATE TABLE [Albums] (
    [Id] int NOT NULL IDENTITY,
    [UserId] int NOT NULL,
    [Title] nvarchar(256) NOT NULL,
    [Description] nvarchar(256) NULL,
    [ThumbnailName] nvarchar(256) NOT NULL,
    [FolderName] nvarchar(256) NOT NULL,
    [CreationTime] datetime2 NOT NULL,
    [EditTime] datetime2 NOT NULL,
    CONSTRAINT [PK_Albums] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [AppUsers] (
    [Id] int NOT NULL IDENTITY,
    [VendorId] nvarchar(256) NULL,
    [AuthorizationMethod] int NOT NULL,
    [Email] nvarchar(24) NOT NULL,
    [Password] nvarchar(24) NULL,
    [Name] nvarchar(24) NULL,
    [Surname] nvarchar(24) NULL,
    [AccountCreated] datetime2 NOT NULL,
    [LastLogin] datetime2 NOT NULL,
    [Picture] nvarchar(256) NULL,
    CONSTRAINT [PK_AppUsers] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Images] (
    [Id] int NOT NULL IDENTITY,
    [AlbumId] int NOT NULL,
    [ImageName] nvarchar(40) NULL,
    CONSTRAINT [PK_Images] PRIMARY KEY ([Id])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220615173426_createdatabase', N'6.0.6');
GO

COMMIT;
GO

