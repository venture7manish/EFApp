using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFData.DTOs;

namespace EFServices.Interfaces
{
    public interface IStudentsCoursesService
    {
        Task<IEnumerable<StudentsCoursesDto>> GetAllAsync();
        Task<StudentsCoursesDto?> GetByIdAsync(int id);
        Task<StudentsCoursesDto> CreateAsync(CreateStudentsCoursesDto dto);
        Task<bool> UpdateAsync(int id, CreateStudentsCoursesDto dto);
        Task<bool> DeleteAsync(int id);

        Task<StudentsCoursesDto> RegisterStudentToCourseAsync(int studentId, int courseId);
        Task<bool> UnregisterStudentFromCourseAsync(int studentId, int courseId);

    }
}
