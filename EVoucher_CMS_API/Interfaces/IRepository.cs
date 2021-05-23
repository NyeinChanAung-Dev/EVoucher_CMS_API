using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EVoucher_CMS_API.Interfaces
{
    public interface IRepository<T>
    {
        IQueryable<T> GetAll();
        int Insert(T entity);
        int InsertList(List<T> entity);
        int Delete(T entity);
        int DeleteMultipleRecords(List<T> entities);
        int Delete(string id);
        int Delete(Guid id);
        int Delete(int id);
        int Update(T entity);
        int updateMultipleRecords(List<T> entities);

        int Remove(T entity);
        T GetById(int id);
        T GetByGuid(Guid id);

        IQueryable<T> Get { get; }
        void Dispose(bool disposing);


    }

}
