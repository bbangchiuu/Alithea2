using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Alithea2.Controllers.Respository
{
    public interface IBaseRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T SelectById(object id);
        void Insert(T obj);
        void Update(T obj);
        void Delete(object id);
        void Save();
    }
}