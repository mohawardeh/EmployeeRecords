using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeRecords.Web.Models.Dtos
{
    public class EmployeeDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Department { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Address { get; set; }

        public List<EmployeeFileDto> Files { get; set; }
    }
}
