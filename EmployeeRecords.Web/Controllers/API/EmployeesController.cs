using AutoMapper;
using EmployeeRecords.Data.DomainEntities;
using EmployeeRecords.Data.Repositories;
using EmployeeRecords.Web.Models.Dtos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeRecords.Web.Controllers.API
{
    [Route("api/employees")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IFileRepository _fileRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EmployeesController(IEmployeeRepository employeeRepository, IFileRepository fileRepository, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            this._employeeRepository = employeeRepository;
            this._fileRepository = fileRepository;
            this._mapper = mapper;
            this._webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        [Route("Search/{searchWord}", Name = "SearchEmployee")]
        public IActionResult Search(string searchWord)
        {
            if (string.IsNullOrEmpty(searchWord))
                return Ok();
            var result = _employeeRepository.Search(searchWord);
            result.ForEach(e => e.Files = _fileRepository.GetEmployeeFiles(e.Id));
            return Ok(_mapper.Map<List<EmployeeDto>>(result));
        }

        [HttpPost(Name = "CreateEmployee")]
        public ActionResult CreateEmployee([FromForm] EmployeeForCreationDto employeeDto)
        {
            // No need to check if employee is null
            // because [ApiController] attribute does that automatically for us
            Employee employeeEntity = _mapper.Map<Employee>(employeeDto);
            var employeeId = _employeeRepository.Create(employeeEntity);
            foreach (var file in employeeDto.Files)
            {
                var fileEntity = _mapper.Map<EmployeeFile>(file);
                fileEntity.EmployeeId = employeeId;
                var fileId = _fileRepository.Create(fileEntity);
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "EmployeeFiles", employeeId.ToString() + fileId.ToString() + "." + file.FileName.Split(".")[file.FileName.Split(".").Length - 1]);
                using (var stream = new FileStream(filePath, FileMode.Create))
                    file.CopyTo(stream);
            }
            var result = _employeeRepository.GetById(employeeId);
            result.Files = _fileRepository.GetEmployeeFiles(employeeId);
            return CreatedAtRoute("GetEmployeeById", new { employeeId = result.Id }, result);
        }

        [HttpGet]
        [Route("{employeeId:int}", Name = "GetEmployeeById")]
        public IActionResult GetEmployee(int employeeId)
        {
            Employee emp = _employeeRepository.GetById(employeeId);
            if (emp == null)
                return NotFound();
            emp.Files = _fileRepository.GetEmployeeFiles(emp.Id);
            return Ok(_mapper.Map<EmployeeDto>(emp));
        }

        [HttpDelete("{employeeId:int}")]
        public IActionResult DeleteEmployee(int employeeId)
        {
            Employee employee = _employeeRepository.GetById(employeeId);
            if (employee == null)
                return NotFound();
            employee.Files = _fileRepository.GetEmployeeFiles(employee.Id);
            foreach (var file in employee.Files)
            {
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "EmployeeFiles", employeeId.ToString() + file.Id.ToString() + "." + file.FileName.Split(".")[file.FileName.Split(".").Length - 1]);
                System.IO.File.Delete(filePath);
            }
            _employeeRepository.Delete(employeeId);
            _fileRepository.DeleteEmployeeFiles(employee.Id);
            return NoContent();
        }
        [HttpPut()]
        [Route("{employeeId:int}", Name = "UpdateEmployee")]
        public IActionResult UpdateEmployee(int employeeId, [FromForm] EmployeeForUpdateDto emp)
        {
            Employee employee = _employeeRepository.GetById(employeeId);
            if (employee == null)
                return NotFound();
            employee.Files = _fileRepository.GetEmployeeFiles(employeeId);

            var employeeForUpdate = _mapper.Map<Employee>(emp);
            _employeeRepository.Update(employeeId, employeeForUpdate);
            if (emp.Files!=null)
            {
                foreach (var file in employee.Files)
                {
                    var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "EmployeeFiles", employeeId.ToString() + file.Id.ToString() + "." + file.FileName.Split(".")[file.FileName.Split(".").Length - 1]);
                    System.IO.File.Delete(filePath);
                }
                _fileRepository.DeleteEmployeeFiles(employeeId);
            }
            if (emp.Files!=null)
                foreach (var file in emp.Files)
                {
                    var fileEntity = _mapper.Map<EmployeeFile>(file);
                    fileEntity.EmployeeId = employeeId;
                    var fileId = _fileRepository.Create(fileEntity);
                    var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "EmployeeFiles", employeeId.ToString() + fileId.ToString() + "." + file.FileName.Split(".")[file.FileName.Split(".").Length - 1]);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                        file.CopyTo(stream);
                }
            return NoContent();
        }


        [HttpGet]
        [Route("file/{fileId:int}", Name = "GetFileById")]
        public IActionResult FileGet(int fileId)
        {
            EmployeeFile file = _fileRepository.GetById(fileId);
            if (file == null)
                return NotFound();
            string[] fileNames = Directory.GetFiles(Path.Combine(_webHostEnvironment.WebRootPath, "EmployeeFiles"), file.EmployeeId.ToString() + file.Id.ToString() + "*");
            if (fileNames.Length == 0 || !System.IO.File.Exists(fileNames[0]))
                return NotFound();
            return File(System.IO.File.OpenRead(fileNames[0]), file.ContentType, file.FileName);
        }

    }
}
