using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFData.Models;

namespace EFData.Repositories
{
    public interface ICourseRepository: IRepository<Course>
    {
        Task<Course?> GetWithInstructorsAsync(int id);
    }
}
