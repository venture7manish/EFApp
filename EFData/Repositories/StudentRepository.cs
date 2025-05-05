using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFData.Data;
using EFData.Models;
using Microsoft.EntityFrameworkCore;

namespace EFData.Repositories
{
    public class StudentRepository : Repository<Student>, IStudentRepository
    {
        public StudentRepository(AppDbContext context)
            : base(context)
        {
        }

        public async Task<Student?> GetStudentWithProfileAsync(int studentId)
        {
            return await _dbSet
                .Include(s => s.Profile)
                .FirstOrDefaultAsync(s => s.Id == studentId);
        }

        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            return await _dbSet
                .Include(s => s.Profile)
                .ToListAsync();
        }
    }
}