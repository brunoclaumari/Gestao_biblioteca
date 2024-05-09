using GestaoBiblioteca.Context;
using GestaoBiblioteca.DTO;
using GestaoBiblioteca.Entities;
using System.Net;

namespace GestaoBiblioteca.Repositories
{
    public interface IRepository
    {
        void Add<T>(T entity) where T : EntidadePadrao;

        void Update<T>(T entity) where T : EntidadePadrao;

        void Delete<T>(T entity) where T : EntidadePadrao;

        void SalvaOuAtualiza<T>(T entity, bool fazRegistroNovo) where T : EntidadePadrao;

        CustomResponse ObtemResponseSucesso<T>(T entity, HttpStatusCode statusCode) where T : EntidadePadrao;

        Task<List<Usuario>> GetAllUsuariosAsync(bool incluiEmprestimos = false);

        Task<Usuario?> GetUsuarioByIdAsync(int id);

        Task<Usuario?> GetUsuarioByTelefoneAsync(string telefone);

        Task<List<Livro>> GetAllLivrosAsync();

        Task<Livro?> GetLivroByIdAsync(int id);

        Task<List<Emprestimo>> GetAllEmprestimosAsync();

        Task<List<Emprestimo>> GetEmprestimosByUsuarioIdAsync(int usuarioId);

        Task<Emprestimo?> GetEmprestimoByIdAsync(int id);

        Task<bool> SaveChangesAsync();

        void IniciaTransacaoAsync();

        void ConfirmaTransacaoAsync();

        void CancelaTransacaoAsync();

        void EmprestaLivro(int livroId);

        void DevolveLivro(int livroId);



    }
}
