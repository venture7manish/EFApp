using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFData.Models
{
    public class Instructor: BaseEntity
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateTime HireDate { get; set; }

        public ICollection<CourseInstructor> CourseInstructors { get; set; } = new List<CourseInstructor>();


    }
}
