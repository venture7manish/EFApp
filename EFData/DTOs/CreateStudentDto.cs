using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFData.DTOs
{
    public class CreateStudentDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public CreateStudentProfileDto? Profile { get; set; }
    }
}
