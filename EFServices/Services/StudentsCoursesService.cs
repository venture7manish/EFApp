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
    public class StudentsCoursesService : IStudentsCoursesService
    {
        private readonly IStudentsCoursesRepository _studentsCoursesRepository;
        private readonly IStudentRepository _studentRepository; // Add this
        private readonly ICourseRepository _courseRepository; // Add this

        public StudentsCoursesService(IStudentsCoursesRepository studentsCoursesRepository, IStudentRepository studentRepository, ICourseRepository courseRepository)
        {
            _studentsCoursesRepository = studentsCoursesRepository;
            _studentRepository = studentRepository; // Initialize
            _courseRepository = courseRepository; // Initialize
        }

        public async Task<IEnumerable<StudentsCoursesDto>> GetAllAsync()
        {
            try 
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
            catch (Exception ex)
            {
                // Log the exception appropriately
                Console.WriteLine($"Error in GetAllAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<StudentsCoursesDto?> GetByIdAsync(int id)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetByIdAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<StudentsCoursesDto> CreateAsync(CreateStudentsCoursesDto dto)
        {
            try 
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
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CreateAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(int id, CreateStudentsCoursesDto dto)
        {
            try { 
                var entity = await _studentsCoursesRepository.GetByIdAsync(id);
                if (entity == null) return false;

                entity.StudentId = dto.StudentId;
                entity.CourseId = dto.CourseId;
                entity.Grade = dto.Grade;

                _studentsCoursesRepository.Update(entity);
                await _studentsCoursesRepository.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try { 
                var entity = await _studentsCoursesRepository.GetByIdAsync(id);
                if (entity == null) return false;

                _studentsCoursesRepository.Delete(entity);
                await _studentsCoursesRepository.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<StudentsCoursesDto> RegisterStudentToCourseAsync(int studentId, int courseId)
        {
            try 
            {
                // Check if the student exists and is not deleted
                var student = await _studentRepository.GetByIdAsync(studentId);
                if (student == null || student.IsDeleted)
                {
                    throw new InvalidOperationException($"Student with ID {studentId} does not exist or is deleted.");
                }

                // Check if the course exists and is not deleted
                var course = await _courseRepository.GetByIdAsync(courseId);
                if (course == null || course.IsDeleted)
                {
                    throw new InvalidOperationException($"Course with ID {courseId} does not exist or is deleted.");
                }

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
            catch (Exception ex)
            {
                Console.WriteLine($"Error in RegisterStudentToCourseAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UnregisterStudentFromCourseAsync(int studentId, int courseId)
        {
            try 
            { 
                var allEntities = await _studentsCoursesRepository.GetAllAsync();

                var entity = allEntities.FirstOrDefault(sc => sc.StudentId == studentId && sc.CourseId == courseId);
                if (entity == null) return false;

                _studentsCoursesRepository.Delete(entity);
                await _studentsCoursesRepository.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UnregisterStudentToCourseAsync: {ex.Message}");
                throw;
            }
        }
    }
}