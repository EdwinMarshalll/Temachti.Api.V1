using Temachti.Api.Entities;

namespace Temachti.Api.DTOs;

public class DTOEntryWithTechnology : DTOEntry
{
    public Technology Technology { get; set; }
}
