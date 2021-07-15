using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkDemo.Models
{
    public class CoursesContext: DbContext
    {

        public CoursesContext(DbContextOptions<CoursesContext> options)
            : base(options)
        {
        }

        public DbSet<Student> students { get; set; }

        public DbSet<Course> courses { get; set; }
    }
}
