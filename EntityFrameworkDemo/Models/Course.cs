using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkDemo.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string CourseName { get; set; }
        public int StudentId { get; set; }
        public Student Student { get; set; }

    }
}
