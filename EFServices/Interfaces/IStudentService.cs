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
        Task<IEnumerable<StudentDTO>> GetAllAsync();

        Task<StudentDTO?> GetByIdAsync(int id);

        Task<StudentDTO> CreateAsync(CreateStudentDto dto);

        Task<bool> UpdateAsync(int id, CreateStudentDto dto);

        Task<bool> DeleteAsync(int id);
    }
}
