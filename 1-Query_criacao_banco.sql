
/***** Criando as tabelas do banco ******/


/*
DROP TABLE IF EXISTS tbItensEmprestimo
DROP TABLE IF EXISTS tbEmprestimo
DROP TABLE IF EXISTS tbLivro
DROP TABLE IF EXISTS tbUsuario
DROP TABLE IF EXISTS __EFMigrationsHistory
*/

/*
Estrutura de criação do banco usada para iniciar o Entity framework na API.
Como as migrations vão controlar as atualizações no banco, não será necessário 
executar esse script. 
Mas serve para demonstrar como criar direto no banco.

*/


USE GESTAO_BIBLIOTECA
GO


IF EXISTS (SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'tbLivro')
    PRINT 'A tabela "tbLivro" existe.'
ELSE
BEGIN
   PRINT 'A tabela "tbLivro" não existe e será inserida.'
   
	CREATE TABLE tbLivro (
		id INT IDENTITY(1, 1) NOT NULL,
		titulo VARCHAR(MAX) NOT NULL,
		autores VARCHAR(300),
		genero INT NULL,--genero vai ser um enum no código
		quantidade_total INT NOT NULL,		
		CONSTRAINT PK_tbLivro PRIMARY KEY CLUSTERED (id)		
	);
END
GO

IF EXISTS (SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'tbUsuario')
    PRINT 'A tabela "tbUsuario" existe.'
ELSE
BEGIN
   PRINT 'A tabela "tbUsuario" não existe e será inserida.'
   
	CREATE TABLE tbUsuario (
		id INT IDENTITY(1, 1) NOT NULL,
		nome VARCHAR(MAX) NOT NULL,		
		endereco VARCHAR(250) NOT NULL,--tentar facilitar com cep no frontend
		data_registro DATETIME NOT NULL,
		data_atualizacao DATETIME NULL,
		possui_pendencias bit NOT NULL DEFAULT 0,
		CONSTRAINT PK_tbUsuario PRIMARY KEY CLUSTERED (id)		
	);
END
GO

/*
Esta tabela abaixo contém um relacionamento 1 para 1 com a tbUsuario
*/

IF EXISTS (SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'tbEmprestimo')
    PRINT 'A tabela "tbEmprestimo" existe.'
ELSE
BEGIN
   PRINT 'A tabela "tbEmprestimo" não existe e será inserida.'
   
	CREATE TABLE tbEmprestimo (
		id INT IDENTITY(1, 1) NOT NULL,		
		usuario_id INT NOT NULL,		
		data_emprestimo DATETIME NOT NULL,
		data_devolucao DATETIME NOT NULL,
		status_emprestimo bit NOT NULL DEFAULT 0, /* 0 = em aberto, 1 = devolvido  */
		CONSTRAINT PK_tbEmprestimo PRIMARY KEY CLUSTERED (id),
		CONSTRAINT FK_tbEmprestimo_tbUsuario FOREIGN KEY (usuario_id) REFERENCES tbUsuario(id), -- Chave estrangeira nomeada 1 para 1		
	);
END
GO

/*
Esta tabela abaixo é do tipo mestre-detalhe para conter o relacionamento N * N entre 
a tbLivro e a tbEmprestimo. Contém o id dessas duas tabelas
*/
IF EXISTS (SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'tbItensEmprestimo')
    PRINT 'A tabela "tbItensEmprestimo" existe.'
ELSE
BEGIN
   PRINT 'A tabela "tbItensEmprestimo" não existe e será inserida.'
   
	CREATE TABLE tbItensEmprestimo (
		id INT IDENTITY(1, 1) NOT NULL,		
		livro_id		INT NOT NULL,		
		emprestimo_id	INT NOT NULL,
		CONSTRAINT PK_tbItensEmprestimo					PRIMARY KEY CLUSTERED (id),
		CONSTRAINT FK_tbItensEmprestimo_tbLivro			FOREIGN KEY (livro_id)		REFERENCES tbLivro(id), -- Chave estrangeira nomeada 
		CONSTRAINT FK_tbItensEmprestimo_tbEmprestimo	FOREIGN KEY (emprestimo_id) REFERENCES tbEmprestimo(id) -- Chave estrangeira nomeada 
	);
END
GO



