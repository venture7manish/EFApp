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
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;

        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<IEnumerable<StudentDTO>> GetAllAsync()
        {
            var students = await _studentRepository.GetAllAsync();
            return students.Select(s => new StudentDTO
            {
                Id = s.Id,
                FullName = $"{s.FirstName} {s.LastName}",
                Profile = s.Profile != null ? new StudentProfileDTO
                {
                    Id = s.Profile.Id,
                    Address = s.Profile.Address,
                    PhoneNumber = s.Profile.PhoneNumber
                } : null
            });
        }

        public async Task<StudentDTO?> GetByIdAsync(int id)
        {
            var student = await _studentRepository.GetStudentWithProfileAsync(id);
            if (student == null) return null;

            return new StudentDTO
            {
                Id = student.Id,
                FullName = $"{student.FirstName} {student.LastName}",
                Profile = student.Profile != null ? new StudentProfileDTO
                {
                    Id = student.Profile.Id,
                    Address = student.Profile.Address,
                    PhoneNumber = student.Profile.PhoneNumber
                } : null
            };
        }

        public async Task<StudentDTO> CreateAsync(CreateStudentDto dto)
        {
            var student = new Student
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
            };

            if (dto.Profile != null)
            {
                student.Profile = new StudentProfile
                {
                    Address = dto.Profile.Address,
                    PhoneNumber = dto.Profile.PhoneNumber
                };
            }

            await _studentRepository.AddAsync(student);
            await _studentRepository.SaveChangesAsync();

            return new StudentDTO
            {
                Id = student.Id,
                FullName = $"{student.FirstName} {student.LastName}",
                Profile = student.Profile != null ? new StudentProfileDTO
                {
                    Id = student.Profile.Id,
                    Address = student.Profile.Address,
                    PhoneNumber = student.Profile.PhoneNumber
                } : null
            };
        }

        public async Task<bool> UpdateAsync(int id, CreateStudentDto dto)
        {
            var student = await _studentRepository.GetStudentWithProfileAsync(id);
            if (student == null) return false;

            student.FirstName = dto.FirstName;
            student.LastName = dto.LastName;
            student.UpdatedAt = DateTime.UtcNow;

            if (dto.Profile != null)
            {
                if (student.Profile == null)
                {
                    student.Profile = new StudentProfile
                    {
                        Address = dto.Profile.Address,
                        PhoneNumber = dto.Profile.PhoneNumber
                    };
                }
                else
                {
                    student.Profile.Address = dto.Profile.Address;
                    student.Profile.PhoneNumber = dto.Profile.PhoneNumber;
                }
            }
            else
            {
                // If new profile is null, delete existing
                if (student.Profile != null)
                {
                    student.Profile.IsDeleted = true;
                }
            }

            _studentRepository.Update(student);
            await _studentRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null) return false;

            _studentRepository.Delete(student);
            student.UpdatedAt = DateTime.UtcNow;
            await _studentRepository.SaveChangesAsync();
            return true;
        }
    }
}
