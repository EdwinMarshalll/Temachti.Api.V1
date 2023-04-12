using AutoMapper;
using Temachti.Api.DTOs;
using Temachti.Api.Entities;

namespace Temachti.Api.Utils;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        // Roles
        CreateMap<DTORoleCreate, Role>();
        CreateMap<Role, DTORole>().ReverseMap();
        // CreateMap<Role, DTORolePatch>().ReverseMap();
        CreateMap<Role, DTORoleCreate>();

        // Usuarios
        CreateMap<DTOUserCreate, User>();
        CreateMap<User, DTOUser>();
    }
}