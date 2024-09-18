using AutoMapper;
using OnlineCoursesPlatform.DTO.ResponseDTO;
using OnlineCoursesPlatform.SERVER.Models;

namespace OnlineCoursesPlatform.SERVER.converters
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserResponseDTO>();
            CreateMap<UserResponseDTO, User>();
        }
    }
}
