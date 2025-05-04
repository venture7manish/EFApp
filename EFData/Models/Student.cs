using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFData.Models
{
    public class Student:BaseEntity
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;

        public StudentProfile? Profile { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; } = new HashSet<Enrollment>();


    }
}
