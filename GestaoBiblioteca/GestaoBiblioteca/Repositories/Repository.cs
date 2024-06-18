using GestaoBiblioteca.Context;
using GestaoBiblioteca.DTO;
using GestaoBiblioteca.Entities;
using GestaoBiblioteca.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace GestaoBiblioteca.Repositories
{
    public class Repository : IRepository
    {
        private readonly GestaoBibliotecaContext _context;

        public Repository(GestaoBibliotecaContext context)
        {
            _context = context;
        }

        public void IniciaTransacaoAsync()
        {
            _context.Database.BeginTransactionAsync();
        }

        public void ConfirmaTransacaoAsync()
        {
            _context.Database.CommitTransactionAsync();
        }

        public void CancelaTransacaoAsync()
        {
            _context.Database.RollbackTransactionAsync();
        }

        public void SalvaOuAtualiza<T>(T entity, bool fazRegistroNovo) where T : EntidadePadrao
        {
            if (fazRegistroNovo)
            {                
                Add(entity);
            }
            else
                Update(entity);
        }

        public void Add<T>(T entity) where T : EntidadePadrao
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : EntidadePadrao
        {
            _context.Remove(entity);
        }

        public void Update<T>(T entity) where T : EntidadePadrao
        {
           _context.Update(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<Livro>> GetAllLivrosAsync()
        {
            IQueryable<Livro> query = _context.Livros;

            query = query.AsNoTracking().OrderBy(a => a.Id);

            return await query.ToListAsync();
        }

        public async Task<Livro?> GetLivroByIdAsync(int id)
        {
            //var livro = await _context.Livros.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            return await _context.Livros.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public void DevolveLivro(int livroId)
        {
            var livro = GetLivroByIdAsync(livroId);
            if (livro != null && livro.Result != null)
            {
                LivroHelper.Devolve(livro: livro.Result);
            }
        }

        public void EmprestaLivro(int livroId)
        {
            var livro = GetLivroByIdAsync(livroId);
            if (livro != null && livro.Result != null)
            {
                LivroHelper.Empresta(livro: livro.Result);
            }
        }

        public async Task<List<Usuario>> GetAllUsuariosAsync(bool incluiEmprestimos = false)
        {
            IQueryable<Usuario> query = _context.Usuarios;

            //if (incluiEmprestimos)
            //{
            //    query = query.Include(e => e.Emprestimos)
            //    .ThenInclude(em => em.ItensEmprestimos);
            //}

            query = query.Include(e => e.Emprestimos)
                        .ThenInclude(em => em.ItensEmprestimos);

            query = query.AsNoTracking().OrderBy(a => a.Id);

            return await query.ToListAsync();

        }

        public async Task<Usuario?> GetUsuarioByIdAsync(int id)
        {
            return await _context.Usuarios.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Usuario?> GetUsuarioByTelefoneAsync(string telefone)
        {
            return await _context.Usuarios.AsNoTracking().FirstOrDefaultAsync(x => x.Telefone == telefone);
        }

        public async Task<List<Emprestimo>> GetAllEmprestimosAsync()
        {
            IQueryable<Emprestimo> query = _context.Emprestimos;

            query = query.Include(e => e.ItensEmprestimos)
                            .ThenInclude(em => em.Livro);

            query = query.AsNoTracking().OrderBy(a => a.Id);

            return await query.ToListAsync();

        }

        public async Task<List<Emprestimo>> GetEmprestimosByUsuarioIdAsync(int usuarioId)
        {
            IQueryable<Emprestimo> query = _context.Emprestimos;

            query = query.Include(emm => emm.Usuario)
                         .Include(e => e.ItensEmprestimos)
                         .ThenInclude(em => em.Livro);

            query = query.AsNoTracking().OrderBy(e => e.Id)                
                            .Where(em => em.UsuarioId == usuarioId); 

            return await query.ToListAsync();
        }
        

        public async Task<List<Emprestimo>> GetEmprestimosByLivroIdAsync(int livroId, bool incluiItens = false)
        {
            IQueryable<Emprestimo> query = _context.Emprestimos;

            if (incluiItens)
            {
                query = query.Include(e => e.ItensEmprestimos)
                .ThenInclude(em => em.Livro);
            }

            query = query.AsNoTracking().OrderBy(e => e.Id)
                            .Where(em => em.ItensEmprestimos.Any(i => i.LivroId == livroId));

            return await query.ToListAsync();
        }

        public async Task<Emprestimo?> GetEmprestimoByIdAsync(int id)
        {
            IQueryable<Emprestimo> query = _context.Emprestimos;
            query = query.Include(a => a.ItensEmprestimos)
                         .ThenInclude(ad => ad.Livro);                         

            query = query.AsNoTracking()
                         .OrderBy(a => a.Id)
                         .Where(al => al.Id == id);

            return await query.FirstOrDefaultAsync();
        }

        public CustomResponse ObtemResponseSucesso<T>(T entity, HttpStatusCode statusCode) where T : EntidadePadrao
        {
            var response = new CustomResponse { FoiSucesso = true };
            response.StatusCode = (int)statusCode;
            response.Entidade = entity;

            return response;
        }
    }
}
