using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFData.Data;
using EFData.Models;
using Microsoft.EntityFrameworkCore;

namespace EFData.Repositories
{
    public class InstructorRepository: Repository<Instructor>, IInstructorRepository
    {
        public InstructorRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Instructor?> GetWithCoursesAsync(int id)
        {
            return await _dbSet.Include(i => i.CourseInstructors)
                               .ThenInclude(ci => ci.Course)
                               .FirstOrDefaultAsync(i => i.Id == id);
        }
    }
}
