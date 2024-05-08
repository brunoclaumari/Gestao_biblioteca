


SELECT * FROM dbo.tbLivro

SELECT * FROM dbo.tbUsuario

SELECT * FROM dbo.tbEmprestimo

SELECT * FROM dbo.tbItensEmprestimo


SELECT * FROM dbo.tbEmprestimo e 
--INNER JOIN tbItensEmprestimo ie ON e.id = ie.emprestimo_id 
--INNER JOIN tbLivro lv ON ie.livro_id = lv.id
WHERE e.id = 3
--WHERE lv.id = 7
--order by ie.livro_id


SELECT * FROM dbo.tbEmprestimo e 
INNER JOIN tbItensEmprestimo ie ON e.id = ie.emprestimo_id 
INNER JOIN tbLivro lv ON ie.livro_id = lv.id
WHERE e.id = 4






