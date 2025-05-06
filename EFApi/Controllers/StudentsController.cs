using EFServices.DTOs;
using EFServices.Interfaces;
using EFServices.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EFApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class StudentsController : ControllerBase
    {

        private readonly IStudentService _studentService;
        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string orderBy = "num_courses", [FromQuery] string dir = "asc", [FromQuery(Name = "using")] string usingMethod = "ef")
        {
            if (orderBy != "num_courses")
            {
                return BadRequest("Invalid orderBy parameter. Only 'num_courses' is supported.");
            }

            if (dir.ToLower() != "asc" && dir.ToLower() != "desc")
            {
                return BadRequest("Invalid dir parameter. Use 'asc' or 'desc'.");
            }

            IEnumerable<StudentDTO> students;

            if (usingMethod.ToLower() == "ado")
            {
                students = await _studentService.GetAllSortedByCoursesUsingAdoAsync(dir.ToLower());
            }
            else
            {
                students = await _studentService.GetAllSortedByCoursesAsync(dir.ToLower());
            }

            return Ok(students);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var student = await _studentService.GetByIdAsync(id);
            if (student == null) return NotFound();
            return Ok(student);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStudentDto dto)
        {
            var created = await _studentService.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateStudentDto dto)
        {
            var updated = await _studentService.UpdateAsync(id, dto);
            if (!updated) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _studentService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
