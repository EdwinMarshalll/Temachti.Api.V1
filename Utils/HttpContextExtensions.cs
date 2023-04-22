using Microsoft.EntityFrameworkCore;

namespace Temachti.Api.Utils;

public static class HttpContextExtensions
{
    public async static Task InsertParametersIntoHeader<T>(this HttpContext httpContext, IQueryable<T> queryable)
    {
        if(httpContext is null)
        {
            throw new ArgumentNullException(nameof(httpContext));
        }

        double quantity = await queryable.CountAsync();
        httpContext.Response.Headers.Add("totalRecordsQuantity", quantity.ToString());
    }
}