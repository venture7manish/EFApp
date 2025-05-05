using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFData.Models
{
    public class StudentsCourses: BaseEntity
    {
        public int CourseId { get; set; }
        public int StudentId { get; set; }
        //public Grade? Grade { get; set; }

        public virtual Course Course { get; set; } = null!;
        public virtual Student Student { get; set; } = null!;


    }
    //public enum Grade
    //{
    //    A, B, C, D, F
    //}
}
