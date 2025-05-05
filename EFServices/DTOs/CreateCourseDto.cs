using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFServices.DTOs
{
    public class CreateCourseDto
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100, ErrorMessage = "Title can't be longer than 100 characters.")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Credits are required.")]
        [Range(0, 100, ErrorMessage = "Credits must be between 0 and 100.")]
        public int Credits { get; set; }
    }
}