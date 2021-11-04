using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDBSample.Models;
using MongoDBSample.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDBSample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly EmployeeService _employeeService;

        public EmployeesController(EmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public ActionResult<List<Employee>> Get() =>
            _employeeService.Get();

        [HttpGet("{id:length(24)}", Name = "GetEmployee")]
        public ActionResult<Employee> Get(string id)
        {
            var emp = _employeeService.Get(id);

            if (emp == null)
            {
                return NotFound();
            }

            return emp;
        }

        [HttpDelete("{id:length(24)}", Name = "DeleteEmployee")]
        public ActionResult<Employee> Delete(string id)
        {
            var emp = _employeeService.Get(id);

            if (emp == null)
            {
                return NotFound();
            }

            _ = _employeeService.Delete(id);

            return Ok(emp);
        }

        [HttpPut]
        public ActionResult<Employee> Add([FromBody] Employee emp)
        {
            _employeeService.Add(emp);
            return Ok(emp);
        }

        [HttpPost]
        public ActionResult<Employee> Update([FromBody] Employee emp)
        {
            var employee = _employeeService.Get(emp.Id);

            if (employee == null)
            {
                return NotFound();
            }

            _employeeService.Update(emp);
            return Ok(emp);
        }

    }
}
