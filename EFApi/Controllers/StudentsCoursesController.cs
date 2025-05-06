using EFServices.DTOs;
using EFServices.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EFApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsCoursesController : ControllerBase
    {
        private readonly IStudentsCoursesService _studentsCoursesService;

        public StudentsCoursesController(IStudentsCoursesService studentsCoursesService)
        {
            _studentsCoursesService = studentsCoursesService;
        }

        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> GetAll()
        {
            try { 
            var studentsCourses = await _studentsCoursesService.GetAllAsync();
            return Ok(studentsCourses);
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
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Get(int id)
        {
            try {
                var studentsCourse = await _studentsCoursesService.GetByIdAsync(id);
                if (studentsCourse == null) return NotFound();
                return Ok(studentsCourse);
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
        public async Task<IActionResult> Create([FromBody] CreateStudentsCoursesDto dto)
        {
            try
            {
                var created = await _studentsCoursesService.CreateAsync(dto);
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
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Update(int id, [FromBody] CreateStudentsCoursesDto dto)
        {
            try { 
                var updated = await _studentsCoursesService.UpdateAsync(id, dto);
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
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Delete(int id)
        {
            try { 
                var deleted = await _studentsCoursesService.DeleteAsync(id);
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

        //[HttpPost("register")]
        //public async Task<IActionResult> RegisterStudentToCourse([FromBody] RegisterStudentCourseDto dto)
        //{
        //    try
        //    {
        //        var created = await _studentsCoursesService.RegisterStudentToCourseAsync(dto.StudentId, dto.CourseId);
        //        return Ok(created);
        //    }
        //    catch (System.InvalidOperationException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        //[HttpDelete("unregister")]
        //public async Task<IActionResult> UnregisterStudentFromCourse([FromBody] RegisterStudentCourseDto dto)
        //{
        //    var result = await _studentsCoursesService.UnregisterStudentFromCourseAsync(dto.StudentId, dto.CourseId);
        //    if (!result) return NotFound();
        //    return Ok();
        //}
    }
}