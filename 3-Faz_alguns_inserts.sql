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
    VALUES ('O Pequeno Príncipe', 'Antoine de Saint-Exupéry', 4, 12);
END;

-- Exemplo 5
IF NOT EXISTS (SELECT 1 FROM tbLivro WHERE titulo = 'Crime e Castigo' AND autores = 'Fiódor Dostoiévski')
BEGIN
    INSERT INTO tbLivro (titulo, autores, genero, quantidade_total)
    VALUES ('Crime e Castigo', 'Fiódor Dostoiévski', 5, 10);
END;

-- Exemplo 6
IF NOT EXISTS (SELECT 1 FROM tbLivro WHERE titulo = '1984' AND autores = 'George Orwell')
BEGIN
    INSERT INTO tbLivro (titulo, autores, genero, quantidade_total)
    VALUES ('1984', 'George Orwell', 7, 10);
END;

-- Exemplo 7
IF NOT EXISTS (SELECT 1 FROM tbLivro WHERE titulo = 'Orgulho e Preconceito' AND autores = 'Jane Austen')
BEGIN
    INSERT INTO tbLivro (titulo, autores, genero, quantidade_total)
    VALUES ('Orgulho e Preconceito', 'Jane Austen', 8, 15);
END;

-- Exemplo 8
IF NOT EXISTS (SELECT 1 FROM tbLivro WHERE titulo = 'Cem Anos de Solidão' AND autores = 'Gabriel García Márquez')
BEGIN
    INSERT INTO tbLivro (titulo, autores, genero, quantidade_total)
    VALUES ('Cem Anos de Solidão', 'Gabriel García Márquez', 5, 16);
END;

-- Exemplo 9
IF NOT EXISTS (SELECT 1 FROM tbLivro WHERE titulo = 'Moby Dick' AND autores = 'Herman Melville')
BEGIN
    INSERT INTO tbLivro (titulo, autores, genero, quantidade_total)
    VALUES ('Moby Dick', 'Herman Melville', 9, 12);
END;

-- Exemplo 10
IF NOT EXISTS (SELECT 1 FROM tbLivro WHERE titulo = 'A Revolução dos Bichos' AND autores = 'George Orwell')
BEGIN
    INSERT INTO tbLivro (titulo, autores, genero, quantidade_total)
    VALUES ('A Revolução dos Bichos', 'George Orwell', 7, 10);
END;

-- Exemplo 11
IF NOT EXISTS (SELECT 1 FROM tbLivro WHERE titulo = 'Robinson Crusoé' AND autores = 'Daniel Defoe')
BEGIN
    INSERT INTO tbLivro (titulo, autores, genero, quantidade_total)
    VALUES ('Robinson Crusoé', 'Daniel Defoe', 5, 25);
END;

-- Exemplo 12
IF NOT EXISTS (SELECT 1 FROM tbLivro WHERE titulo = 'Clean Code' AND autores = 'Robert C. Martin')
BEGIN
    INSERT INTO tbLivro (titulo, autores, genero, quantidade_total)
    VALUES ('Clean Code', 'Robert C. Martin', 10, 14);
END;

-- Exemplo 13
IF NOT EXISTS (SELECT 1 FROM tbLivro WHERE titulo = 'Clean Architecture' AND autores = 'Robert C. Martin')
BEGIN
    INSERT INTO tbLivro (titulo, autores, genero, quantidade_total)
    VALUES ('Clean Architecture', 'Robert C. Martin', 10, 18);
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


/* Inserts de empréstimo   */


-- Inserção 1
IF NOT EXISTS (SELECT 1 FROM tbEmprestimo WHERE id = 1)
BEGIN
    -- Inserir registro na tabela tbEmprestimo
    INSERT INTO tbEmprestimo (usuario_id, data_emprestimo, data_devolucao, status_emprestimo)
    VALUES (1, GETDATE(), DATEADD(DAY, 7, GETDATE()), 0);

    -- Inserir registros na tabela tbItensEmprestimo
    INSERT INTO tbItensEmprestimo (livro_id, emprestimo_id) VALUES (1, 1);
	EXEC spDiminuirQuantidadeLivroPorID @livro_id = 1;

    INSERT INTO tbItensEmprestimo (livro_id, emprestimo_id) VALUES (2, 1);
	EXEC spDiminuirQuantidadeLivroPorID @livro_id = 2;

    INSERT INTO tbItensEmprestimo (livro_id, emprestimo_id) VALUES (3, 1);
	EXEC spDiminuirQuantidadeLivroPorID @livro_id = 3;
END
ELSE
BEGIN
    PRINT 'O empréstimo 1 já existe.'
END
GO

-- Inserção 2
IF NOT EXISTS (SELECT 1 FROM tbEmprestimo WHERE id = 2)
BEGIN
    -- Inserir registro na tabela tbEmprestimo
    INSERT INTO tbEmprestimo (usuario_id, data_emprestimo, data_devolucao, status_emprestimo)
    VALUES (2, GETDATE(), DATEADD(DAY, 7, GETDATE()), 0);

    -- Inserir registros na tabela tbItensEmprestimo
    INSERT INTO tbItensEmprestimo (livro_id, emprestimo_id) VALUES (4, 2);
	EXEC spDiminuirQuantidadeLivroPorID @livro_id = 4;

    INSERT INTO tbItensEmprestimo (livro_id, emprestimo_id) VALUES (5, 2);
	EXEC spDiminuirQuantidadeLivroPorID @livro_id = 5;

    INSERT INTO tbItensEmprestimo (livro_id, emprestimo_id) VALUES (6, 2);
	EXEC spDiminuirQuantidadeLivroPorID @livro_id = 6;

END
ELSE
BEGIN
    PRINT 'O empréstimo 2 já existe.'
END
GO

-- Inserção 3
IF NOT EXISTS (SELECT 1 FROM tbEmprestimo WHERE id = 3)
BEGIN
    -- Inserir registro na tabela tbEmprestimo
    INSERT INTO tbEmprestimo (usuario_id, data_emprestimo, data_devolucao, status_emprestimo)
    VALUES (3, GETDATE(), DATEADD(DAY, 7, GETDATE()), 0);

    -- Inserir registros na tabela tbItensEmprestimo
    INSERT INTO tbItensEmprestimo (livro_id, emprestimo_id) VALUES (7, 3);
	EXEC spDiminuirQuantidadeLivroPorID @livro_id = 7;

    INSERT INTO tbItensEmprestimo (livro_id, emprestimo_id) VALUES (8, 3);
	EXEC spDiminuirQuantidadeLivroPorID @livro_id = 8;

    INSERT INTO tbItensEmprestimo (livro_id, emprestimo_id) VALUES (9, 3);
	EXEC spDiminuirQuantidadeLivroPorID @livro_id = 9;
END
ELSE
BEGIN
    PRINT 'O empréstimo 3 já existe.'
END
GO

-- Inserção 4
IF NOT EXISTS (SELECT 1 FROM tbEmprestimo WHERE id = 4)
BEGIN
    -- Inserir registro na tabela tbEmprestimo
    INSERT INTO tbEmprestimo (usuario_id, data_emprestimo, data_devolucao, status_emprestimo)
    VALUES (4, GETDATE(), DATEADD(DAY, 7, GETDATE()), 0);

    -- Inserir registros na tabela tbItensEmprestimo
    INSERT INTO tbItensEmprestimo (livro_id, emprestimo_id) VALUES (7, 4);
	EXEC spDiminuirQuantidadeLivroPorID @livro_id = 7;

    INSERT INTO tbItensEmprestimo (livro_id, emprestimo_id) VALUES (8, 4);
	EXEC spDiminuirQuantidadeLivroPorID @livro_id = 8;

    INSERT INTO tbItensEmprestimo (livro_id, emprestimo_id) VALUES (9, 4);
	EXEC spDiminuirQuantidadeLivroPorID @livro_id = 9;
END
ELSE
BEGIN
    PRINT 'O empréstimo 4 já existe.'
END
GO
