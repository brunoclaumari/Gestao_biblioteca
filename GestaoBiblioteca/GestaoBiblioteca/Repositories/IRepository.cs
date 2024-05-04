﻿using GestaoBiblioteca.Entities;

namespace GestaoBiblioteca.Repositories
{
    public interface IRepository
    {
        void Add<T>(T entity) where T : EntidadePadrao;

        void Update<T>(T entity) where T : EntidadePadrao;

        void Delete<T>(T entity) where T : EntidadePadrao;

        Task<bool> SaveChangesAsync();
    }
}