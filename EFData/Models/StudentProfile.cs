using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFData.Models
{
    public class StudentProfile: BaseEntity
    {
        public string Address { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;
        public int StudentId { get; set; }
        public virtual Student Student { get; set; } = null!;
    }
}
