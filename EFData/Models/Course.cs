using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFData.Models
{
    public class Course: BaseEntity
    {
        [Required]
        [StringLength(150)]
        public string Title { get; set; } = null!;
        //public int Credits { get; set; }

        public virtual ICollection<StudentsCourses> Enrollments { get; set; } = new HashSet<StudentsCourses>();

        public ICollection<CourseInstructor> CourseInstructors { get; set; } = new List<CourseInstructor>();


    }
}
