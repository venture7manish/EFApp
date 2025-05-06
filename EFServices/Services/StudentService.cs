using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFServices.DTOs;
using EFData.Models;
using EFData.Repositories;
using EFServices.Interfaces;
using AutoMapper;

namespace EFServices.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;

        public StudentService(IStudentRepository studentRepository, IMapper mapper)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<StudentDTO>> GetAllAsync()
        {
            try
            {
                /*var students = await _studentRepository.GetAllAsync();
                return students.Select(s => new StudentDTO
                {
                    Id = s.Id,
                    FullName = $"{s.FirstName} {s.LastName}",
                    Profile = s.Profile == null ? null : new StudentProfileDTO
                    {
                        Id = s.Profile.Id,
                        Address = s.Profile.Address,
                        PhoneNumber = s.Profile.PhoneNumber
                    },
                    Courses = s.Enrollments.Select(sc => new CourseDto
                    {
                        Id = sc.Course.Id,
                        Title = sc.Course.Title,
                        //Credits = sc.Course.Credits
                    }).ToList()
                });*/

                var students = await _studentRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<StudentDTO>>(students);
            }
            catch (Exception ex)
            {
                // Log the exception appropriately
                Console.WriteLine($"Error in GetAllAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<StudentDTO?> GetByIdAsync(int id)
        {
            try { 
                var student = await _studentRepository.GetStudentWithProfileAsync(id);
                if (student == null) return null;

                return _mapper.Map<StudentDTO>(student);

                /*return new StudentDTO
                {
                    Id = student.Id,
                    FullName = $"{student.FirstName} {student.LastName}",
                    Profile = student.Profile != null ? new StudentProfileDTO
                    {
                        Id = student.Profile.Id,
                        Address = student.Profile.Address,
                        PhoneNumber = student.Profile.PhoneNumber
                    } : null,
                    Courses = student.Enrollments.Select(sc => new CourseDto
                    {
                        Id = sc.Course.Id,
                        Title = sc.Course.Title,
                        //Credits = sc.Course.Credits
                    }).ToList()
                };*/
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetByIdAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<StudentDTO> CreateAsync(CreateStudentDto dto)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CreateAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(int id, CreateStudentDto dto)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var student = await _studentRepository.GetByIdAsync(id);
                if (student == null) return false;

                _studentRepository.Delete(student);
                student.DeletedAt = DateTime.UtcNow;
                await _studentRepository.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteAsync: {ex.Message}");
                throw;
            }
        }
    }
}
