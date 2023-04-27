using Microsoft.EntityFrameworkCore;

namespace Temachti.Api.Utils;

public static class HttpContextExtensions
{
    public async static Task InsertParametersIntoHeader<T>(this HttpContext httpContext, IQueryable<T> queryable, int quantiyPerPage)
    {
        if (httpContext is null)
        {
            throw new ArgumentNullException(nameof(httpContext));
        }

        double quantity = await queryable.CountAsync();
        double quantityPages = Math.Ceiling(quantity / quantiyPerPage);
        httpContext.Response.Headers.Add("totalRecordsQuantity", quantity.ToString());
        httpContext.Response.Headers.Add("totalPagesQuantity", quantityPages.ToString());
    }
}