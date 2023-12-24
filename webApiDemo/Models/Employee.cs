using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace webApiDemo.Models
{
    public class Employee
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3)]
        public string Name { get; set; }
        public string Address { get; set; }
        [Range(4000,10000)]
        public int Salary { get; set; }
        public int Age { get; set; }

        [ForeignKey("Department")]
        public int? Dept_Id { get; set; }
        public virtual Department? Department { get; set; }
    }
}
