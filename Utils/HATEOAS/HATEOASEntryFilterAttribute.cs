using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Temachti.Api.DTOs;

namespace Temachti.Api.Utils.HATEOAS;

public class HATEOASEntryFilterAttribute : HATEOASFilterAttribute
{
    private readonly LinksGenerator linksGenerator;

    public HATEOASEntryFilterAttribute(LinksGenerator linksGenerator)
    {
        this.linksGenerator = linksGenerator;
    }

    public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        var include = IncludeHATEOAS(context);

        if (!include)
        {
            await next();
            return;
        }

        var result = context.Result as ObjectResult;

        var dtoEntry = result.Value as DTOEntry;
        if (dtoEntry is null)
        {
            var dtoEntries = result.Value as List<DTOEntry> ?? throw new ArgumentException("Se esperaba una instancia de DTOEntry o List<DTOEntry>");

            dtoEntries.ForEach(async entry => await linksGenerator.GenerateLinks(entry));
            result.Value = dtoEntries;
        }
        else
        {
            await linksGenerator.GenerateLinks(dtoEntry);
        }

        await next();
    }
}