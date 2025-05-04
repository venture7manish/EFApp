using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFData.Models;

namespace EFData.DTOs
{
    public class StudentsCoursesDto
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; } = null!;
        public int CourseId { get; set; }
        public string CourseTitle { get; set; } = null!;
        public Grade? Grade { get; set; }
    }
}
