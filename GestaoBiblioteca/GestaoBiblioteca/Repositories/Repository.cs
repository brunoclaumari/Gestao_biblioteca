using GestaoBiblioteca.Context;
using GestaoBiblioteca.DTO;
using GestaoBiblioteca.Entities;
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

        //public void IniciaTransacao()
        //{
        //    _context.Database.BeginTransactionAsync();
        //}

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
            //try
            //{
            //    _context.Database.BeginTransactionAsync();
            //    _context.Add(entity);           

            //}
            //catch (Exception ex)
            //{
            //    _context.Database.RollbackTransactionAsync();

            //}

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
            return await _context.SaveChangesAsync() > 1;
        }

        public async Task<List<Livro>> GetAllLivrosAsync()
        {
            IQueryable<Livro> query = _context.Livros;

            query = query.AsNoTracking().OrderBy(a => a.Id);

            return await query.ToListAsync();
        }

        public Task<Livro> GetLivroByIdAsync(int id)
        {
            throw new NotImplementedException();
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

            query = query.Include(e => e.ItensEmprestimos)
                            .ThenInclude(em => em.Livro);

            query = query.AsNoTracking().OrderBy(e => e.Id)
                //.Where(em => em.ItensEmprestimos.Any(i => i. == disciplinaId));
                .Where(em => em.UsuarioId == usuarioId); 

            return await query.ToListAsync();
        }

        public async Task<Emprestimo> GetEmprestimoByIdAsync(int id)
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
