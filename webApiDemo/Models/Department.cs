using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace webApiDemo.Models
{
    public class Department
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3)]
        public string Name { get; set; }
        public string ManagerName { get; set; }
       // [JsonIgnore]
        public virtual List<Employee>? Employees { get; set; }

    }
}
