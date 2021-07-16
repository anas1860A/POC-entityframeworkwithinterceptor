using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using EntityFrameworkDemo.Models;

namespace EntityFrameworkDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CoursesContext>(
                opt => opt.UseInMemoryDatabase("EFDemoDB")
                .AddInterceptors(new EventBrokerListener())
                );

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<CoursesContext>();
           var student= AddTestData(context);
            UpdateTestData(context, student);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private static Student AddTestData(CoursesContext context)
        {
            var testStudent1 = new Student
            {
                Id = 1,
                Name = "Ahmed Ali",
                Level=3
            };

            var testCourse1 = new Course
            {
                Id = 1,
                StudentId = testStudent1.Id,
                CourseName="English"
            };

            var testCourse2 = new Course
            {
                Id = 2,
                StudentId = testStudent1.Id,
                CourseName = "C#"
            };

            var testCourse3 = new Course
            {
                Id = 3,
                StudentId = testStudent1.Id,
                CourseName = "React"
            };

            context.courses.Add(testCourse1);
            context.courses.Add(testCourse2);
            context.courses.Add(testCourse3);
            context.students.Add(testStudent1);


            context.SaveChanges();
            return testStudent1;
        }


        private static void UpdateTestData(CoursesContext context, Student studant)
        {
           var course =studant.Courses.Where(M => M.Id == 1).SingleOrDefault();
            studant.Courses.Remove(course);
            studant.Name = "Test After Update";

            context.Update(studant);

            context.SaveChanges();




        }
    }
}
