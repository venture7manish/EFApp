using EFServices.DTOs;
using EFServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EFApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InstructorsController : ControllerBase
    {
        private readonly IInstructorService _instructorService;

        public InstructorsController(IInstructorService instructorService)
        {
            _instructorService = instructorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var instructors = await _instructorService.GetAllAsync();
            return Ok(instructors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var instructor = await _instructorService.GetByIdAsync(id);
            if (instructor == null) return NotFound();
            return Ok(instructor);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateInstructorDto dto)
        {
            var created = await _instructorService.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateInstructorDto dto)
        {
            var updated = await _instructorService.UpdateAsync(id, dto);
            if (!updated) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _instructorService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }

        [HttpPost("assignCourse")]
        public async Task<IActionResult> AssignCourse([FromBody] AssignInstructorCourseDto dto)
        {
            var result = await _instructorService.AssignInstructorToCourseAsync(dto.InstructorId, dto.CourseId);
            if (!result)
                return BadRequest("Instructor is already assigned to this course or instructor/course not found.");
            return Ok();
        }

        [HttpDelete("unassignCourse")]
        public async Task<IActionResult> UnassignCourse([FromBody] AssignInstructorCourseDto dto)
        {
            var result = await _instructorService.UnassignInstructorFromCourseAsync(dto.InstructorId, dto.CourseId);
            if (!result)
                return NotFound("Assignment not found.");
            return Ok();
        }
    }
}
