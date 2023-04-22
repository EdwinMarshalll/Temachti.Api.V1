using Temachti.Api.DTOs;

namespace Temachti.Api.Utils;

public static class IQueryableExtensions
{
    /// <summary>
    /// Pagina los registros
    /// </summary>
    public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, DTOPagination dtoPagination)
    {
        return queryable
        .Skip((dtoPagination.Page - 1) * dtoPagination.RecordsPerPage)
        .Take(dtoPagination.RecordsPerPage);
    }
}