using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFServices.DTOs
{
    public class StudentProfileDTO
    {
        public int Id { get; set; }
        public string Address { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;

    }
}
