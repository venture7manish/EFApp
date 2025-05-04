using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFData.Models;

namespace EFData.Repositories
{
    public interface IStudentsCoursesRepository:IRepository<StudentsCourses>
    {
        Task<StudentsCourses?> GetWithDetailsAsync(int id);
    }
}
