﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFServices.DTOs
{
    public class CreateInstructorDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
    }
}
