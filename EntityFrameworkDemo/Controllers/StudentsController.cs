using EntityFrameworkDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkDemo.Controllers
{
    public class StudentsController : Controller
    {
        private readonly CoursesContext _context;

        public StudentsController(CoursesContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Get()
        {
            var students = await _context.students
                .Include(u => u.Courses)
                .ToArrayAsync();

            var response = students.Select(u => new
            {
                Name = u.Name,
                Level = u.Level,
                Courses = u.Courses.Select(p => p.CourseName)
            });

            return Ok(response);
        }
    }
}
