using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFData.Models;

namespace EFServices.DTOs
{
    public class CreateStudentsCoursesDto
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        //public Grade? Grade { get; set; }
    }
}
