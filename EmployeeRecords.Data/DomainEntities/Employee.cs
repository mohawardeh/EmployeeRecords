using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeRecords.Data.DomainEntities
{
    public class Employee
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Department { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Address { get; set; }

        public List<EmployeeFile> Files { get; set; }
    }
}
