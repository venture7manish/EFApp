using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFServices.DTOs;
using EFData.Models;
using EFData.Repositories;
using EFServices.Interfaces;

namespace EFServices.Services
{
    public class StudentsCoursesService: IStudentsCoursesService
    {
        private readonly IStudentsCoursesRepository _studentsCoursesRepository;

        public StudentsCoursesService(IStudentsCoursesRepository studentsCoursesRepository)
        {
            _studentsCoursesRepository = studentsCoursesRepository;
        }

        public async Task<IEnumerable<StudentsCoursesDto>> GetAllAsync()
        {
            var entities = await _studentsCoursesRepository.GetAllAsync();
            return entities.Select(sc => new StudentsCoursesDto
            {
                Id = sc.Id,
                StudentId = sc.StudentId,
                StudentName = sc.Student != null ? $"{sc.Student.FirstName} {sc.Student.LastName}" : string.Empty,
                CourseId = sc.CourseId,
                CourseTitle = sc.Course != null ? sc.Course.Title : string.Empty,
                Grade = sc.Grade
            });
        }

        public async Task<StudentsCoursesDto?> GetByIdAsync(int id)
        {
            var entity = await _studentsCoursesRepository.GetWithDetailsAsync(id);
            if (entity == null) return null;

            return new StudentsCoursesDto
            {
                Id = entity.Id,
                StudentId = entity.StudentId,
                StudentName = entity.Student != null ? $"{entity.Student.FirstName} {entity.Student.LastName}" : string.Empty,
                CourseId = entity.CourseId,
                CourseTitle = entity.Course != null ? entity.Course.Title : string.Empty,
                Grade = entity.Grade
            };
        }

        public async Task<StudentsCoursesDto> CreateAsync(CreateStudentsCoursesDto dto)
        {
            var entity = new StudentsCourses
            {
                StudentId = dto.StudentId,
                CourseId = dto.CourseId,
                Grade = dto.Grade
            };

            await _studentsCoursesRepository.AddAsync(entity);
            await _studentsCoursesRepository.SaveChangesAsync();

            return await GetByIdAsync(entity.Id) ?? throw new InvalidOperationException("Failed to retrieve created entity.");
        }

        public async Task<bool> UpdateAsync(int id, CreateStudentsCoursesDto dto)
        {
            var entity = await _studentsCoursesRepository.GetByIdAsync(id);
            if (entity == null) return false;

            entity.StudentId = dto.StudentId;
            entity.CourseId = dto.CourseId;
            entity.Grade = dto.Grade;

            _studentsCoursesRepository.Update(entity);
            await _studentsCoursesRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _studentsCoursesRepository.GetByIdAsync(id);
            if (entity == null) return false;

            _studentsCoursesRepository.Delete(entity);
            await _studentsCoursesRepository.SaveChangesAsync();

            return true;
        }

        public async Task<StudentsCoursesDto> RegisterStudentToCourseAsync(int studentId, int courseId)
        {
            var allEntities = await _studentsCoursesRepository.GetAllAsync();

            if (allEntities.Any(sc => sc.StudentId == studentId && sc.CourseId == courseId))
                throw new InvalidOperationException("Student already registered to this course.");

            var entity = new StudentsCourses
            {
                StudentId = studentId,
                CourseId = courseId
            };

            await _studentsCoursesRepository.AddAsync(entity);
            await _studentsCoursesRepository.SaveChangesAsync();

            return await GetByIdAsync(entity.Id) ?? throw new InvalidOperationException("Failed to retrieve created entity.");
        }

        public async Task<bool> UnregisterStudentFromCourseAsync(int studentId, int courseId)
        {
            var allEntities = await _studentsCoursesRepository.GetAllAsync();

            var entity = allEntities.FirstOrDefault(sc => sc.StudentId == studentId && sc.CourseId == courseId);
            if (entity == null) return false;

            _studentsCoursesRepository.Delete(entity);
            await _studentsCoursesRepository.SaveChangesAsync();
            return true;
        }
    }
}
