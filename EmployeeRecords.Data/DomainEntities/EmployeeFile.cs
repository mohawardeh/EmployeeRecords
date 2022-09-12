using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeRecords.Data.DomainEntities
{
    public class EmployeeFile
    {
        public int Id { get; set; }

        public string FileName { get; set; }

        public string ContentType { get; set; }

        public long FileSize { get; set; }

        public int EmployeeId { get; set; }
    }
}
