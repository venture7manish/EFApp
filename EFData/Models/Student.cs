using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFData.Models
{
    public class Student:BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = null!;
        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = null!;
        public StudentProfile? Profile { get; set; }
        public virtual ICollection<StudentsCourses> Enrollments { get; set; } = new HashSet<StudentsCourses>();


    }
}
