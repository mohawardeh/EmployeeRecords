using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeRecords.Web.Controllers
{
    public class EmployeesController : Controller
    {
        public IActionResult Create()
        {
            return View();
        }
        public ActionResult Details(int id)
        {
            return View();
        }
        public ActionResult Edit(int id)
        {
            return View();
        }
    }
}
