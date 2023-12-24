using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webApiDemo.Models
{
    public class ITIEntity :IdentityDbContext<ApplicationUser>
    {
        public ITIEntity()
        {

        }
        public ITIEntity(DbContextOptions options):base(options)
        {

        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Department { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server =.; database = WebApiITI; Integrated Security = true");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
