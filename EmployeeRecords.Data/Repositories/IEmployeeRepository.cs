using EmployeeRecords.Data.DomainEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeRecords.Data.Repositories
{
    public interface IEmployeeRepository:IRepository<Employee,int>
    {
        List<Employee> Search(string keyword);
    }
}
