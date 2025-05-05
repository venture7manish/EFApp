using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFData.Models;

namespace EFServices.DTOs
{
    public class StudentDTO
    {
        public int Id { get; set; }

        public string FullName { get; set; } = null!;

        public StudentProfileDTO? Profile { get; set; }
        public List<CourseDto> Courses { get; set; } = new List<CourseDto>();
    }

   
}
