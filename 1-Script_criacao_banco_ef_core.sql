

--Cria banco de dados
IF DB_ID('GESTAO_BIBLIOTECA') IS NOT NULL
    PRINT('DataBase "GESTAO_BIBLIOTECA" já existe');
ELSE
BEGIN
    --SELECT 0 AS DatabaseExists;
	PRINT('Criando DataBase "GESTAO_BIBLIOTECA"');
	CREATE DATABASE GESTAO_BIBLIOTECA;
END
GO

/*Abaixo Script gerado pelo entity framework*/


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

CREATE TABLE [tbLivro] (
    [id] int NOT NULL IDENTITY,
    [titulo] varchar(max) NOT NULL,
    [autores] varchar(300) NULL,
    [genero] int NULL,
    [quantidade_total] int NOT NULL,
    CONSTRAINT [PK_tbLivro] PRIMARY KEY ([id])
);
GO

CREATE TABLE [tbUsuario] (
    [id] int NOT NULL IDENTITY,
    [nome] varchar(max) NOT NULL,
    [endereco] varchar(250) NOT NULL,
    [data_registro] datetime NOT NULL,
    [data_atualizacao] datetime NULL,
    [possui_pendencias] bit NOT NULL DEFAULT CAST(0 AS bit),
    CONSTRAINT [PK_tbUsuario] PRIMARY KEY ([id])
);
GO

CREATE TABLE [tbEmprestimo] (
    [id] int NOT NULL IDENTITY,
    [usuario_id] int NOT NULL,
    [data_emprestimo] datetime NOT NULL,
    [data_devolucao] datetime NOT NULL,
    [status_emprestimo] int NOT NULL DEFAULT 0,
    CONSTRAINT [PK_tbEmprestimo] PRIMARY KEY ([id]),
    CONSTRAINT [FK_tbEmprestimo_tbUsuario] FOREIGN KEY ([usuario_id]) REFERENCES [tbUsuario] ([id])
);
GO

CREATE TABLE [tbItensEmprestimo] (
    [id] int NOT NULL IDENTITY,
    [livro_id] int NOT NULL,
    [emprestimo_id] int NOT NULL,
    CONSTRAINT [PK_tbItensEmprestimo] PRIMARY KEY ([id]),
    CONSTRAINT [FK_tbItensEmprestimo_tbEmprestimo] FOREIGN KEY ([emprestimo_id]) REFERENCES [tbEmprestimo] ([id]),
    CONSTRAINT [FK_tbItensEmprestimo_tbLivro] FOREIGN KEY ([livro_id]) REFERENCES [tbLivro] ([id])
);
GO

CREATE INDEX [IX_tbEmprestimo_usuario_id] ON [tbEmprestimo] ([usuario_id]);
GO

CREATE INDEX [IX_tbItensEmprestimo_emprestimo_id] ON [tbItensEmprestimo] ([emprestimo_id]);
GO

CREATE INDEX [IX_tbItensEmprestimo_livro_id] ON [tbItensEmprestimo] ([livro_id]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240506065237_Inicia_Tabelas', N'7.0.18');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [tbItensEmprestimo] DROP CONSTRAINT [FK_tbItensEmprestimo_tbEmprestimo];
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[tbUsuario]') AND [c].[name] = N'nome');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [tbUsuario] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [tbUsuario] ALTER COLUMN [nome] varchar(100) NOT NULL;
GO

ALTER TABLE [tbItensEmprestimo] ADD CONSTRAINT [FK_tbItensEmprestimo_tbEmprestimo] FOREIGN KEY ([emprestimo_id]) REFERENCES [tbEmprestimo] ([id]) ON DELETE CASCADE;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240507064502_AddDeleteCascade', N'7.0.18');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[tbLivro]') AND [c].[name] = N'titulo');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [tbLivro] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [tbLivro] ALTER COLUMN [titulo] varchar(200) NOT NULL;
GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[tbLivro]') AND [c].[name] = N'genero');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [tbLivro] DROP CONSTRAINT [' + @var2 + '];');
UPDATE [tbLivro] SET [genero] = 0 WHERE [genero] IS NULL;
ALTER TABLE [tbLivro] ALTER COLUMN [genero] int NOT NULL;
ALTER TABLE [tbLivro] ADD DEFAULT 0 FOR [genero];
GO

ALTER TABLE [tbLivro] ADD [quantidade_emprestada] int NOT NULL DEFAULT 0;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240508045136_Alterando_tbLivro', N'7.0.18');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [tbUsuario] ADD [telefone] nvarchar(15) NOT NULL DEFAULT N'';
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240509034619_Add_campo_telefone_tbUsuario', N'7.0.18');
GO

COMMIT;
GO

