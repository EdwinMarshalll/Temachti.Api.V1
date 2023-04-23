using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Temachti.Api.DTOs;

namespace Temachti.Api.Utils.HATEOAS;

public class HATEOASTechnologyFilterAttribute : HATEOASFilterAttribute
{
    private readonly LinksGenerator linksGenerator;

    public HATEOASTechnologyFilterAttribute(LinksGenerator linksGenerator)
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

        var dtoTechnology = result.Value as DTOTechnology;
        if (dtoTechnology is null)
        {
            var dtoTechnologies = result.Value as List<DTOTechnology> ?? throw new ArgumentException("Se esperaba una instancia de DTOTechnoloy o List<DTOTechnology>");

            dtoTechnologies.ForEach(async tech => await linksGenerator.GenerateLinks(tech));
            result.Value = dtoTechnologies;
        }
        else
        {
            await linksGenerator.GenerateLinks(dtoTechnology);
        }

        await next();
    }
}