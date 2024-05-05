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
