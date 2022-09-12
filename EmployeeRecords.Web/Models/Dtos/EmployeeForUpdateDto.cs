using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeRecords.Web.Models.Dtos
{
    public class EmployeeForUpdateDto
    {
        [Required]
        [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public string Name { get; set; }

        [MaxLength(50, ErrorMessage = "Department name cannot exceed 50 characters")]
        public string Department { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string Address { get; set; }

        public List<IFormFile> Files { get; set; }
    }
}
