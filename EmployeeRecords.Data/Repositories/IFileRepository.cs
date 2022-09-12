using EmployeeRecords.Data.DomainEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeRecords.Data.Repositories
{
    public interface IFileRepository:IRepository<EmployeeFile,int>
    {
        void AddRange(IEnumerable<EmployeeFile> files);
        List<EmployeeFile> GetEmployeeFiles(int employeeId);
        void DeleteEmployeeFiles(int employeeId);
    }
}
