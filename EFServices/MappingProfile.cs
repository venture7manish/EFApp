using AutoMapper;
using EFData.Models;
using EFServices.DTOs;


namespace EFServices
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Student, StudentDTO>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName))
                .ForMember(dest => dest.Courses, opt => opt.MapFrom(src => src.Enrollments.Select(sc => sc.Course)));
            
            CreateMap<StudentProfile, StudentProfileDTO>();

            CreateMap<Course, CourseDto>();
        }
    }
}
