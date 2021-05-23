using EVoucher_CMS_API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EVoucher_CMS_API.Interfaces
{
    public class BaseRepository<T> : IRepository<T> where T : class

    {
        private readonly EVoucherSystemDBContext _dbContext;

        public BaseRepository(EVoucherSystemDBContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public IQueryable<T> GetAll()
        {
            return _dbContext.Set<T>().AsQueryable();
        }

        public int Insert(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            return _dbContext.SaveChanges();
        }
        public int InsertList(List<T> entities)
        {
            for (int i = 0; i < entities.Count(); i++)
            {
                _dbContext.Set<T>().Add(entities[i]);
            }
            return _dbContext.SaveChanges();
        }

        public T InsertReturn(T entity)
        {
            T newEntity = _dbContext.Set<T>().Add(entity).Entity;
            var result = _dbContext.SaveChanges();
            if (result > 0)
            {
                return newEntity;
            }
            else
            {
                return null;
            }
        }

        public int Delete(string id)
        {
            _dbContext.Set<T>().Remove(_dbContext.Set<T>().Find(id));
            return _dbContext.SaveChanges();
        }

        public int Delete(Guid id)
        {
            _dbContext.Set<T>().Remove(_dbContext.Set<T>().Find(id));
            return _dbContext.SaveChanges();
        }

        public int Delete(int id)
        {
            _dbContext.Set<T>().Remove(_dbContext.Set<T>().Find(id));
            return _dbContext.SaveChanges();
        }

        public int Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            return _dbContext.SaveChanges();
        }

        public int DeleteMultipleRecords(List<T> entities)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                _dbContext.Set<T>().Remove(entities[i]);
            }
            return _dbContext.SaveChanges();
        }

        public int Update(T entity)
        {
            //_dbContext.Entry(entity).State = EntityState.Modified;
            return _dbContext.SaveChanges();
        }

        public int updateMultipleRecords(List<T> entities)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                _dbContext.Entry(entities[i]).State = EntityState.Modified;
            }
            return _dbContext.SaveChanges();
        }

        public T GetById(int id)
        {
            return _dbContext.Set<T>().Find(id);
        }

        public T GetByGuid(Guid id)
        {
            return _dbContext.Set<T>().Find(id);
        }


        public int Remove(T entity)
        {
            this._dbContext.Set<T>().Remove(entity);
            return this._dbContext.SaveChanges();
        }

        public IQueryable<T> Get
        {
            get { return this._dbContext.Set<T>(); }
        }

        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
            this.Dispose(disposing);
        }

    }

}
