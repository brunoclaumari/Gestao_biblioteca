/* Procedures úteis no banco  */



CREATE or ALTER PROCEDURE spDiminuirQuantidadeLivroPorID
    @livro_id INT
AS
BEGIN
    -- Diminuir a quantidade de livros em 1
    UPDATE tbLivro
    SET quantidade_total = quantidade_total - 1
    WHERE id = @livro_id
END
GO

