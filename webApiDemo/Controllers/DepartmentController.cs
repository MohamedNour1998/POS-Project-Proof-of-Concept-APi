using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webApiDemo.Models;
using webApiDemo.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace webApiDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DepartmentController : ControllerBase
    {
        private readonly ITIEntity context;

        public DepartmentController(ITIEntity context)
        {
            this.context = context;
        }
        [HttpGet("{id}")]
        public IActionResult getDept(int id)
        {
            Department Deptmodel = context.Department.Include(e => e.Employees).FirstOrDefault(e=>e.Id==id);
            DepartmentWithEmployeeDto DeptDto = new DepartmentWithEmployeeDto();
            DeptDto.Id = Deptmodel.Id;
            DeptDto.Name = Deptmodel.Name;
            foreach (var item in Deptmodel.Employees)
            {
                DeptDto.EmpNames.Add(new EmployeeDto() {Id=item.Id,Name=item.Name });
            }
            return Ok(DeptDto);
        }
    }
}
