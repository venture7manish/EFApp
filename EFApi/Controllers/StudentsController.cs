using EFServices.DTOs;
using EFServices.Interfaces;
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
        public async Task<IActionResult> GetAll()
        {
            try {
                var students = await _studentService.GetAllAsync();
                return Ok(students);
            }
            catch (InvalidOperationException ex)
            {
                    // Return a 400 Bad Request with the error message
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                Console.WriteLine($"Error in Create: {ex.Message}");
                return StatusCode(500, "An unexpected error occurred."); // Return a 500 Internal Server Error
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try { 
                var student = await _studentService.GetByIdAsync(id);
                if (student == null) return NotFound();
                return Ok(student);
            }
            catch (InvalidOperationException ex)
            {
                    // Return a 400 Bad Request with the error message
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                Console.WriteLine($"Error in Create: {ex.Message}");
                return StatusCode(500, "An unexpected error occurred."); // Return a 500 Internal Server Error
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStudentDto dto)
        {
            try
            {
                var created = await _studentService.CreateAsync(dto);
                return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
            }  
            catch (InvalidOperationException ex)
            {
                    // Return a 400 Bad Request with the error message
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                Console.WriteLine($"Error in Create: {ex.Message}");
                return StatusCode(500, "An unexpected error occurred."); // Return a 500 Internal Server Error
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateStudentDto dto)
        {
            try
            {
                var updated = await _studentService.UpdateAsync(id, dto);
                if (!updated) return NotFound();
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                // Return a 400 Bad Request with the error message
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                Console.WriteLine($"Error in Create: {ex.Message}");
                return StatusCode(500, "An unexpected error occurred."); // Return a 500 Internal Server Error
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _studentService.DeleteAsync(id);
                if (!deleted) return NotFound();
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                // Return a 400 Bad Request with the error message
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                Console.WriteLine($"Error in Create: {ex.Message}");
                return StatusCode(500, "An unexpected error occurred."); // Return a 500 Internal Server Error
            }
        }
    }
}
