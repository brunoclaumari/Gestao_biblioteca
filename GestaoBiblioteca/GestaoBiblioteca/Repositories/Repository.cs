using GestaoBiblioteca.Context;
using GestaoBiblioteca.Entities;
using Microsoft.EntityFrameworkCore;

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

        public Task<Emprestimo> GetEmprestimoByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

 



        //public T Add<T>(T entity) where T : EntidadePadrao
        //{
        //    _context.Add(entity);
        //}

        //public T Update<T>(T entity) where T : EntidadePadrao
        //{
        //    _context.Update(entity);
        //}
        //public void Delete<T>(T entity) where T : EntidadePadrao
        //{
        //    _context.Remove(entity);
        //}

    }
}
