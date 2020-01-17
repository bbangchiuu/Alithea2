using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Alithea2.Models;

namespace Alithea2.Controllers.Respository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected MyDbContext _db { get; set; }
        protected DbSet<T> _table = null;

        //protected DbContextTransaction _dbContextTransaction { get; set; }

        public BaseRepository()
        {
            _db = new MyDbContext();
            _table = _db.Set<T>();
            //_dbContextTransaction = _db.Database.BeginTransaction();
        }

        public BaseRepository(MyDbContext db)
        {
            _db = db;
            _table = _db.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return _table;
        }

        public T SelectById(object id)
        {
            try
            {
                return _table.Find(id);
            }
            catch
            {
                return null;
            }
        }

        public void Insert(T obj)
        {
            _table.Add(obj);
        }

        public void Update(T obj)
        {
            _table.Attach(obj);
            _db.Entry(obj).State = EntityState.Modified;
        }

        public void Delete(object id)
        {
            T existing = _table.Find(id);
            _table.Remove(existing);
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public DbContextTransaction BeginTransaction()
        {
            return _db.Database.CurrentTransaction;
        }

        public void CommitTransaction()
        {
            _db.Database.CurrentTransaction.Commit();
        }

        public void RollBackTransaction()
        {
            _db.Database.CurrentTransaction.Rollback();
        }
    }
}