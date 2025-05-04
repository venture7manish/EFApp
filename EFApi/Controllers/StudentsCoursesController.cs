using EFData.DTOs;
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
        public async Task<IActionResult> GetAll()
        {
            var studentsCourses = await _studentsCoursesService.GetAllAsync();
            return Ok(studentsCourses);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var studentsCourse = await _studentsCoursesService.GetByIdAsync(id);
            if (studentsCourse == null) return NotFound();
            return Ok(studentsCourse);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStudentsCoursesDto dto)
        {
            var created = await _studentsCoursesService.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateStudentsCoursesDto dto)
        {
            var updated = await _studentsCoursesService.UpdateAsync(id, dto);
            if (!updated) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _studentsCoursesService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
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
