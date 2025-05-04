using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFServices.DTOs
{
    public class CreateStudentProfileDto
    {
        public string Address { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
    }
}
