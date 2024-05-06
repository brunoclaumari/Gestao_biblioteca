using GestaoBiblioteca.Context;
using GestaoBiblioteca.Entities;

namespace GestaoBiblioteca.Repositories
{
    public interface IRepository
    {
        void Add<T>(T entity) where T : EntidadePadrao;

        void Update<T>(T entity) where T : EntidadePadrao;

        void Delete<T>(T entity) where T : EntidadePadrao;

        Task<List<Livro>> GetAllLivrosAsync();

        Task<Livro> GetLivroByIdAsync(int id);

        Task<List<Emprestimo>> GetAllEmprestimosAsync();

        Task<List<Emprestimo>> GetEmprestimosByUsuarioIdAsync(int usuarioId);

        Task<Emprestimo> GetEmprestimoByIdAsync(int id);



        Task<bool> SaveChangesAsync();

        void IniciaTransacaoAsync();

        void ConfirmaTransacaoAsync();

        void CancelaTransacaoAsync();        


    }
}
