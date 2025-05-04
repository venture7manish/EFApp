using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFData.DTOs;
using EFData.Models;
using EFData.Repositories;
using EFServices.Interfaces;

namespace EFServices.Services
{
    public class InstructorService: IInstructorService
    {
        private readonly IInstructorRepository _instructorRepository;
        public InstructorService(IInstructorRepository instructorRepository)
        {
            _instructorRepository = instructorRepository;
        }

        public async Task<IEnumerable<InstructorDto>> GetAllAsync()
        {
            var instructors = await _instructorRepository.GetAllAsync();
            return instructors.Select(i => new InstructorDto
            {
                Id = i.Id,
                FullName = $"{i.FirstName} {i.LastName}"
            });
        }

        public async Task<InstructorDto?> GetByIdAsync(int id)
        {
            var instructor = await _instructorRepository.GetByIdAsync(id);
            if (instructor == null) return null;

            return new InstructorDto
            {
                Id = instructor.Id,
                FullName = $"{instructor.FirstName} {instructor.LastName}"
            };
        }

        public async Task<InstructorDto> CreateAsync(CreateInstructorDto dto)
        {
            var instructor = new Instructor
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName
            };

            await _instructorRepository.AddAsync(instructor);
            await _instructorRepository.SaveChangesAsync();

            return new InstructorDto
            {
                Id = instructor.Id,
                FullName = $"{instructor.FirstName} {instructor.LastName}"
            };
        }

        public async Task<bool> UpdateAsync(int id, CreateInstructorDto dto)
        {
            var instructor = await _instructorRepository.GetByIdAsync(id);
            if (instructor == null) return false;

            instructor.FirstName = dto.FirstName;
            instructor.LastName = dto.LastName;
            instructor.UpdatedAt = DateTime.UtcNow;
            _instructorRepository.Update(instructor);
            await _instructorRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var instructor = await _instructorRepository.GetByIdAsync(id);
            if (instructor == null) return false;
            instructor.UpdatedAt = DateTime.UtcNow;
            _instructorRepository.Delete(instructor);
            await _instructorRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AssignInstructorToCourseAsync(int instructorId, int courseId)
        {
            // Check if already assigned
            var instructor = await _instructorRepository.GetWithCoursesAsync(instructorId);
            if (instructor == null) return false;

            bool alreadyAssigned = instructor.CourseInstructors.Any(ci => ci.CourseId == courseId);
            if (alreadyAssigned)
                return false;

            var courseInstructor = new CourseInstructor
            {
                CourseId = courseId,
                InstructorId = instructorId
            };

            instructor.CourseInstructors.Add(courseInstructor);
            _instructorRepository.Update(instructor);
            await _instructorRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UnassignInstructorFromCourseAsync(int instructorId, int courseId)
        {
            var instructor = await _instructorRepository.GetWithCoursesAsync(instructorId);
            if (instructor == null) return false;

            var courseInstructor = instructor.CourseInstructors.FirstOrDefault(ci => ci.CourseId == courseId);
            if (courseInstructor == null)
                return false;

            instructor.CourseInstructors.Remove(courseInstructor);
            _instructorRepository.Update(instructor);
            await _instructorRepository.SaveChangesAsync();

            return true;
        }
    }
}
