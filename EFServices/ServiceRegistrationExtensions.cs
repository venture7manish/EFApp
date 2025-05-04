using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFData.Repositories;
using EFServices.Interfaces;
using EFServices.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EFServices
{
    public static class ServiceRegistrationExtensions
    {
        public static void AddProjectServices(this IServiceCollection services)
        {
            // Register Repositories
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IInstructorRepository, InstructorRepository>();
            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<IStudentsCoursesRepository, StudentsCoursesRepository>();

            // Register Services
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<IInstructorService, InstructorService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<IStudentsCoursesService, StudentsCoursesService>();
        }
    }
}
