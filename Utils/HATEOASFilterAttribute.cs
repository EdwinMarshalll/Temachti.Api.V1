using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Temachti.Api.Utils;

public class HATEOASFilterAttribute : ResultFilterAttribute
{
    protected bool IncludeHATEOAS(ResultExecutingContext context)
    {
        var result = context.Result as ObjectResult;

        if(!IsSuccessResponse(result))
        {
            return false;
        }

        var header = context.HttpContext.Request.Headers["includeHATEOAS"];

        if(header.Count is 0)
        {
            return false;
        }

        var value = header[0];

        if(!value.Equals("Y", StringComparison.InvariantCultureIgnoreCase))
        {
            return false;
        }

        return true;
    }

    private bool IsSuccessResponse(ObjectResult result)
    {
        if(result is null || result.Value is null)
        {
            return false;
        }

        if(result.StatusCode.HasValue && !result.StatusCode.Value.ToString().StartsWith("2"))
        {
            return false;
        }

        return true;
    }
}