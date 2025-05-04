using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFServices.DTOs;

namespace EFServices.Interfaces
{
    public interface IInstructorService
    {
        Task<IEnumerable<InstructorDto>> GetAllAsync();
        Task<InstructorDto?> GetByIdAsync(int id);
        Task<InstructorDto> CreateAsync(CreateInstructorDto dto);
        Task<bool> UpdateAsync(int id, CreateInstructorDto dto);
        Task<bool> DeleteAsync(int id);
        Task<bool> AssignInstructorToCourseAsync(int instructorId, int courseId);
        Task<bool> UnassignInstructorFromCourseAsync(int instructorId, int courseId);
    }
}
