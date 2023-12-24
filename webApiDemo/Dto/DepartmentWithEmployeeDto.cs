using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webApiDemo.Dto
{
    public class DepartmentWithEmployeeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<EmployeeDto> EmpNames { get; set; } = new List<EmployeeDto>();
    }
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }
}
