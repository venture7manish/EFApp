using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFServices.DTOs
{
    public class CreateStudentDto
    {
        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, ErrorMessage = "First name can't be longer than 50 characters.")]
        public string FirstName { get; set; } = null!;

        [StringLength(50, ErrorMessage = "Last name can't be longer than 50 characters.")]
        public string? LastName { get; set; }

        public CreateStudentProfileDto? Profile { get; set; }
    }
}