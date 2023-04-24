using AutoMapper;
using Temachti.Api.DTOs;
using Temachti.Api.Entities;

namespace Temachti.Api.Utils;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        // Tecnologias
        CreateMap<DTOTechnologyCreate, Technology>();
        CreateMap<Technology, DTOTechnology>();
        CreateMap<Technology, DTOTechnologyPatch>().ReverseMap();

        // Entradas
        CreateMap<DTOEntryCreate, Entry>();
        CreateMap<Entry, DTOEntry>();
        CreateMap<Entry, DTOEntryWithTechnology>();
        CreateMap<Entry, DTOEntryPatch>().ReverseMap();

        // Comentarios de entradas
        CreateMap<DTOEntryCommentCreate, EntryComment>();
        CreateMap<EntryComment, DTOEntryComment>();
    }

}