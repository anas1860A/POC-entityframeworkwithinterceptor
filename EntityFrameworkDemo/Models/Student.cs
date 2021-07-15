using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkDemo.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public List<Course> Courses { get; set; }
    }
}
