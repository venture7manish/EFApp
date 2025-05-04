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
    public class CourseRepository: Repository<Course>, ICourseRepository
    {
        public CourseRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Course?> GetWithInstructorsAsync(int id)
        {
            return await _dbSet.Include(c => c.CourseInstructors)
                               .ThenInclude(ci => ci.Instructor)
                               .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
