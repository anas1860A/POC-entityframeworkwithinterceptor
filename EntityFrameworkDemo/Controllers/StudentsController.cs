using EntityFrameworkDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkDemo.Controllers
{
    public class StudentsController : Controller
    {
        private readonly CoursesContext _context;
        private readonly ILogger<StudentsController> _logger;


        public StudentsController(CoursesContext context, ILogger<StudentsController> logger)
        {
            _context = context;
            _logger = logger;
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

        public async Task<IActionResult> GetCourses()
        {
            var Courses = await _context.courses
                .ToArrayAsync();

            return Ok(Courses);
        }

        public IActionResult RemoveStudent(int id = 1)
        {
            var Message = "Try To remove student";
            var student = _context.students.Include(x => x.Courses).FirstOrDefault(s => s.Id == id);
            _context.Remove(student);
            var response = _context.SaveChanges();
            if (response == 1)
                Message = $"Remove student {student} at {DateTime.UtcNow.ToLongTimeString()}";
            else
                Message += "Faild";
            
            _logger.LogInformation(Message);
            return Ok(response);
        }

        public IActionResult RemoveStudentCourses(int studentId = 1)
        {
            var courses = _context.courses.Where(s => s.StudentId == studentId);
            _context.RemoveRange(courses);
            var response = _context.SaveChanges();
            return Ok(response);
        }

        public IActionResult UpdateStudentInfo(int studentId = 1)
        {
            var student = _context.students
                .Include(u => u.Courses)
                .FirstOrDefault(s=>s.Id== studentId);

            student.Level = 5;
            student.Name = "Eman";
            _context.Update(student);
            var response = _context.SaveChanges();
            return Ok(response);
        }
    }
}
