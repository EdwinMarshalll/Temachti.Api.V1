using AutoMapper;
using Temachti.Api.DTOs;
using Temachti.Api.Entities;

namespace Temachti.Api.Utils;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        // Entradas
        CreateMap<DTOEntryCreate, Entry>();
        CreateMap<Entry, DTOEntry>();
        CreateMap<Entry, DTOEntryWithTechnology>();

        // Tecnologias
        CreateMap<DTOTechnologyCreate, Technology>();
        CreateMap<Technology, DTOTechnology>();
    }

}