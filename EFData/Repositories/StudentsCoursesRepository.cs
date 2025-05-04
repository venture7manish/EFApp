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
    public class StudentsCoursesRepository: Repository<StudentsCourses>, IStudentsCoursesRepository
    {
        public StudentsCoursesRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<StudentsCourses?> GetWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(sc => sc.Student)
                .Include(sc => sc.Course)
                .FirstOrDefaultAsync(sc => sc.Id == id);
        }
    }
}
