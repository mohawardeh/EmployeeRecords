using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeRecords.Data.Repositories
{
    public interface IRepository<TEntity,TKey>
    {

        TEntity GetById(TKey id);
        TKey Create(TEntity entity);
        void Update(TKey id, TEntity entity);
        void Delete(TKey id);
    }
}
