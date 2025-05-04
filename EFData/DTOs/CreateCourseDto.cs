using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFData.DTOs
{
    public class CreateCourseDto
    {
        public string Title { get; set; } = null!;
        public int Credits { get; set; }
    }
}
