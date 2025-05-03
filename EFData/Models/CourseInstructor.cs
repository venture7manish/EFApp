using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFData.Models
{
    // Join entity for many-to-many Instructor and Course
    public class CourseInstructor
    {
        public int? CourseId { get; set; }
        public Course? Course { get; set; }

        public int? InstructorId { get; set; }
        public Instructor? Instructor { get; set; }
    }
}
