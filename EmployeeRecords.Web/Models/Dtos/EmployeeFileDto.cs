using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeRecords.Web.Models.Dtos
{
    public class EmployeeFileDto
    {
        public int Id { get; set; }

        public string FileName { get; set; }

        public long FileSize { get; set; }

        public int EmployeeId { get; set; }
    }
}
