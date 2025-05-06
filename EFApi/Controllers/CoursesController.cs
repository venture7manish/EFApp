using EFServices.DTOs;
using EFServices.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EFApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try { 
                var courses = await _courseService.GetAllAsync();
                return Ok(courses);
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
                var course = await _courseService.GetByIdAsync(id);
                if (course == null) return NotFound();
                return Ok(course);
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
        public async Task<IActionResult> Create([FromBody] CreateCourseDto dto)
        {
            try { 
                var created = await _courseService.CreateAsync(dto);
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
        public async Task<IActionResult> Update(int id, [FromBody] CreateCourseDto dto)
        {
            try { 
                var updated = await _courseService.UpdateAsync(id, dto);
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
            try { 
                var deleted = await _courseService.DeleteAsync(id);
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