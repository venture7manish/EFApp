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
        public async Task<IActionResult> GetAll(
    [FromQuery] string orderBy = "num_courses",
    [FromQuery] string dir = "asc",
    [FromQuery(Name = "using")] string usingMethod = "ef")
        {
            if (orderBy.ToLower() != "num_courses")
            {
                return BadRequest("Invalid orderBy parameter. Only 'num_courses' is supported.");
            }

            dir = dir.ToLower();
            if (dir != "asc" && dir != "desc")
            {
                return BadRequest("Invalid dir parameter. Use 'asc' or 'desc'.");
            }

            usingMethod = usingMethod.ToLower();
            IEnumerable<StudentDTO> students;

            if (usingMethod == "ado")
            {
                students = await _studentService.GetAllSortedByCoursesUsingAdoAsync(dir);
                return Ok(students);
            }
            else if (usingMethod == "ef")
            {
                students = await _studentService.GetAllAsync();
                return Ok(students);
            }
            else
            {
                return BadRequest("Invalid using parameter. Use 'ado' or 'ef'.");
            }
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
