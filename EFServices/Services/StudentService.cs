﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EFData.Models;
using EFData.Repositories;
using EFServices.DTOs;
using EFServices.Interfaces;
using Microsoft.Extensions.Configuration;

namespace EFServices.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;
        private readonly string? _connectionString;

        public StudentService(IStudentRepository studentRepository, IMapper mapper, IConfiguration configuration)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
            _connectionString = configuration.GetConnectionString("dbcs");
        }

        // ✅ ADO.NET implementation to get students sorted by number of courses
        public async Task<IEnumerable<StudentDTO>> GetAllSortedByCoursesUsingAdoAsync(string dir)
        {
            var students = new List<StudentDTO>();

            // Sanitize input
            string orderDirection = dir?.ToLower() == "desc" ? "DESC" : "ASC";

            string query = $@"
                SELECT s.Id, s.FirstName, s.LastName, COUNT(sc.CourseId) AS CourseCount
                FROM Students s
                LEFT JOIN StudentsCourses sc ON s.Id = sc.StudentId
                GROUP BY s.Id, s.FirstName, s.LastName
                ORDER BY CourseCount {orderDirection}";

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var student = new StudentDTO
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FullName = $"{reader.GetString(reader.GetOrdinal("FirstName"))} {reader.GetString(reader.GetOrdinal("LastName"))}",
                            Courses = new List<CourseDto>(), // Optional
                            // Add this to your DTO
                        };
                        students.Add(student);
                    }
                }
            }

            return students;
        }

        // ✅ Entity Framework implementation to get students sorted by number of courses
        public async Task<IEnumerable<StudentDTO>> GetAllSortedByCoursesAsync(string dir)
        {
            try
            {
                // 1. Fetch all students (including Enrollments & Profile)
                var students = await _studentRepository.GetAllAsync();

                // 2. Sort in‐memory based on enrollment count
                var sortedStudents = dir?.ToLower() == "desc"
                    ? students.OrderByDescending(s => s.Enrollments.Count)
                    : students.OrderBy(s => s.Enrollments.Count);

                // 3. Let AutoMapper map each Student → StudentDTO
                return _mapper.Map<IEnumerable<StudentDTO>>(sortedStudents);
            }
            catch (Exception ex)
            {
                // Use your ILogger here in production
                Console.WriteLine($"Error in GetAllSortedByCoursesAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<StudentDTO?> GetByIdAsync(int id)
        {
            try
            {
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
                    LastName = dto.LastName
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
                else if (student.Profile != null)
                {
                    student.Profile.IsDeleted = true;
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

                student.DeletedAt = DateTime.UtcNow; // Set before deleting
                _studentRepository.Delete(student);
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
