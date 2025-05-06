using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFServices.DTOs;

namespace EFServices.Interfaces
{
    public interface IStudentService
    {
        Task<IEnumerable<StudentDTO>> GetAllSortedByCoursesAsync(string dir);

        Task<IEnumerable<StudentDTO>> GetAllSortedByCoursesUsingAdoAsync(string dir);

        Task<StudentDTO?> GetByIdAsync(int id);

        Task<StudentDTO> CreateAsync(CreateStudentDto dto);

        Task<bool> UpdateAsync(int id, CreateStudentDto dto);

        Task<bool> DeleteAsync(int id);
    }
}
