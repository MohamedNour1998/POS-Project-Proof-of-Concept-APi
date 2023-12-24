using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webApiDemo.Models;
using webApiDemo.Dto;

namespace webApiDemo.Controllers
{
    [Route("api/[controller]")] //Resourse URl:api/Employee
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly ITIEntity context;

        public EmployeeController(ITIEntity context)
        {
            this.context = context;
        }

        [HttpGet("Dept/{id}")]
        public IActionResult GetEmployeeWithDept(int id)
        {
            Employee emp = context.Employees.Include(e=>e.Department).FirstOrDefault(e => e.Id == id);
            return Ok(emp);
        }

        [HttpGet("Dto/{id}")]
        public IActionResult GetEmployeeWithDept2(int id)
        {
            Employee emp = context.Employees.Include(e => e.Department).FirstOrDefault(e => e.Id == id);
            EmployeeNameWithDEpartmentNameDto empDto = new EmployeeNameWithDEpartmentNameDto();
            empDto.EmpId = emp.Id;
            empDto.EmpName = emp.Name;
            empDto.DeptName = emp.Department.Name;
            return Ok(empDto);
        }
        [HttpGet]
        public IActionResult GetEmployee()
        {
            List<Employee> emps = context.Employees.ToList();
            return Ok(emps);
        }
        [HttpGet]
        [Route("{id:int}",Name = "EmployeeDetailsRoute")]
        public IActionResult GetEmployeeById(int id)
        {
            Employee emp = context.Employees.FirstOrDefault(e => e.Id == id);
            return Ok(emp);
        }

        [HttpPut("{id}")]
        public IActionResult PutEmployee(int id, Employee newEmp)
        {
            if (ModelState.IsValid)
            {
                Employee oldEmp = context.Employees.FirstOrDefault(e => e.Id == id);
                oldEmp.Age = newEmp.Age;
                oldEmp.Name = newEmp.Name;
                oldEmp.Salary = newEmp.Salary;
                oldEmp.Address = newEmp.Address;
                context.SaveChanges();
                return StatusCode(StatusCodes.Status204NoContent);
            }
            return BadRequest(ModelState);
        }
        [HttpDelete("{id}")]
        public IActionResult Remove(int id)
        {
            try
            {
                Employee emp = context.Employees.FirstOrDefault(e => e.Id == id);
                context.Employees.Remove(emp);
                context.SaveChanges();
                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        public IActionResult PostEmployee(Employee newEmp)
        {
            if (ModelState.IsValid)
            {
                context.Employees.Add(newEmp);
                context.SaveChanges();
                string url = Url.Link("EmployeeDetailsRoute",new { id=newEmp.Id});
                return Created(url,newEmp);
            }
            return BadRequest(ModelState);
        }
    }

    

}
