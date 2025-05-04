using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFData.DTOs;

namespace EFServices.Interfaces
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseDto>> GetAllAsync();
        Task<CourseDto?> GetByIdAsync(int id);
        Task<CourseDto> CreateAsync(CreateCourseDto dto);
        Task<bool> UpdateAsync(int id, CreateCourseDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
