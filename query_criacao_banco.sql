
/***** Criando as tabelas do banco ******/


/*
DROP TABLE IF EXISTS tbItensEmprestimo
DROP TABLE IF EXISTS tbEmprestimo
DROP TABLE IF EXISTS tbLivro
DROP TABLE IF EXISTS tbUsuario
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
		--nacionalidade VARCHAR(MAX) NOT NULL,
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
		status_emprestimo bit NOT NULL CONSTRAINT DF_status DEFAULT 0, /* 0 = em aberto, 1 = devolvido  */
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

/* inserindo Livros (dados mockados iniciais) */

-- Exemplo 1
IF NOT EXISTS (SELECT 1 FROM tbLivro WHERE titulo = 'Dom Casmurro' AND autores = 'Machado de Assis')
BEGIN
    INSERT INTO tbLivro (titulo, autores, genero, quantidade_total)
    VALUES ('Dom Casmurro', 'Machado de Assis', 1, 15);
END;

-- Exemplo 2
IF NOT EXISTS (SELECT 1 FROM tbLivro WHERE titulo = 'A Metamorfose' AND autores = 'Franz Kafka')
BEGIN
    INSERT INTO tbLivro (titulo, autores, genero, quantidade_total)
    VALUES ('A Metamorfose', 'Franz Kafka', 2, 10);
END;

-- Exemplo 3
IF NOT EXISTS (SELECT 1 FROM tbLivro WHERE titulo = 'O Senhor dos Anéis: A Sociedade do Anel' AND autores = 'J.R.R. Tolkien')
BEGIN
    INSERT INTO tbLivro (titulo, autores, genero, quantidade_total)
    VALUES ('O Senhor dos Anéis: A Sociedade do Anel', 'J.R.R. Tolkien', 1, 8);
END;

-- Exemplo 4
IF NOT EXISTS (SELECT 1 FROM tbLivro WHERE titulo = 'O Pequeno Príncipe' AND autores = 'Antoine de Saint-Exupéry')
BEGIN
    INSERT INTO tbLivro (titulo, autores, genero, quantidade_total)
    VALUES ('O Pequeno Príncipe', 'Antoine de Saint-Exupéry', 3, 12);
END;

-- Exemplo 5
IF NOT EXISTS (SELECT 1 FROM tbLivro WHERE titulo = 'Crime e Castigo' AND autores = 'Fiódor Dostoiévski')
BEGIN
    INSERT INTO tbLivro (titulo, autores, genero, quantidade_total)
    VALUES ('Crime e Castigo', 'Fiódor Dostoiévski', 2, 10);
END;

-- Exemplo 6
IF NOT EXISTS (SELECT 1 FROM tbLivro WHERE titulo = '1984' AND autores = 'George Orwell')
BEGIN
    INSERT INTO tbLivro (titulo, autores, genero, quantidade_total)
    VALUES ('1984', 'George Orwell', 1, 10);
END;

-- Exemplo 7
IF NOT EXISTS (SELECT 1 FROM tbLivro WHERE titulo = 'Orgulho e Preconceito' AND autores = 'Jane Austen')
BEGIN
    INSERT INTO tbLivro (titulo, autores, genero, quantidade_total)
    VALUES ('Orgulho e Preconceito', 'Jane Austen', 1, 15);
END;

-- Exemplo 8
IF NOT EXISTS (SELECT 1 FROM tbLivro WHERE titulo = 'Cem Anos de Solidão' AND autores = 'Gabriel García Márquez')
BEGIN
    INSERT INTO tbLivro (titulo, autores, genero, quantidade_total)
    VALUES ('Cem Anos de Solidão', 'Gabriel García Márquez', 2, 16);
END;

-- Exemplo 9
IF NOT EXISTS (SELECT 1 FROM tbLivro WHERE titulo = 'Moby Dick' AND autores = 'Herman Melville')
BEGIN
    INSERT INTO tbLivro (titulo, autores, genero, quantidade_total)
    VALUES ('Moby Dick', 'Herman Melville', 1, 12);
END;

-- Exemplo 10
IF NOT EXISTS (SELECT 1 FROM tbLivro WHERE titulo = 'A Revolução dos Bichos' AND autores = 'George Orwell')
BEGIN
    INSERT INTO tbLivro (titulo, autores, genero, quantidade_total)
    VALUES ('A Revolução dos Bichos', 'George Orwell', 1, 10);
END;

/* inserindo Usuarios (dados mockados iniciais) */

-- Exemplo 1
IF NOT EXISTS (SELECT 1 FROM tbUsuario WHERE nome = 'João Martins de Sousa' AND endereco = 'Rua A, 123')
BEGIN
    INSERT INTO tbUsuario (nome, endereco, data_registro, data_atualizacao)
    VALUES ('João Martins de Sousa', 'Rua A, 123', GETDATE(), NULL);
END;

-- Exemplo 2
IF NOT EXISTS (SELECT 1 FROM tbUsuario WHERE nome = 'Maria do Bairro' AND endereco = 'Avenida B, 456')
BEGIN
    INSERT INTO tbUsuario (nome, endereco, data_registro, data_atualizacao)
    VALUES ('Maria do Bairro', 'Avenida B, 456', GETDATE(), NULL);
END;

-- Exemplo 3
IF NOT EXISTS (SELECT 1 FROM tbUsuario WHERE nome = 'Pedro Carvalho' AND endereco = 'Rua C, 789')
BEGIN
    INSERT INTO tbUsuario (nome, endereco, data_registro, data_atualizacao)
    VALUES ('Pedro Carvalho', 'Rua C, 789', GETDATE(), NULL);
END;

-- Exemplo 4
IF NOT EXISTS (SELECT 1 FROM tbUsuario WHERE nome = 'Ana Maria Braga' AND endereco = 'Avenida D, 1011')
BEGIN
    INSERT INTO tbUsuario (nome, endereco, data_registro, data_atualizacao)
    VALUES ('Ana Maria Braga', 'Avenida D, 1011', GETDATE(), NULL);
END;

-- Exemplo 5
IF NOT EXISTS (SELECT 1 FROM tbUsuario WHERE nome = 'Carlos Tramontina' AND endereco = 'Rua E, 1213')
BEGIN
    INSERT INTO tbUsuario (nome, endereco, data_registro, data_atualizacao)
    VALUES ('Carlos Tramontina', 'Rua E, 1213', GETDATE(), NULL);
END;

-- Exemplo 6
IF NOT EXISTS (SELECT 1 FROM tbUsuario WHERE nome = 'Laura Miller' AND endereco = 'Avenida F, 1415')
BEGIN
    INSERT INTO tbUsuario (nome, endereco, data_registro, data_atualizacao)
    VALUES ('Laura Miller', 'Avenida F, 1415', GETDATE(), NULL);
END;


