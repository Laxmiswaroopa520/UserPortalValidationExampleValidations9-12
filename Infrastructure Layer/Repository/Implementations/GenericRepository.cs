using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UserPortalValdiationsDBContext.Data;
using UserPortalValdiationsDBContext.Repository.Interfaces;

namespace UserPortalValdiationsDBContext.Repository.Implementations
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _db;
        private readonly DbSet<T> _table;

        public GenericRepository(ApplicationDbContext db)
        {
            _db = db;
            _table = db.Set<T>();
        }

        public T? GetById(int id) => _table.Find(id);

        public IEnumerable<T> GetAll() => _table.ToList();

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate) => _table.Where(predicate).ToList();

        public void Add(T entity) => _table.Add(entity);

        public void AddRange(IEnumerable<T> entities) => _table.AddRange(entities);

        public void Update(T entity) => _table.Update(entity);

        public void Remove(T entity) => _table.Remove(entity);

        public void RemoveRange(IEnumerable<T> entities) => _table.RemoveRange(entities);
    }
}
