using Microsoft.AspNetCore.Mvc.Filters;

namespace Temachti.Api.Filters;

/// <summary>
/// Filtro global para registrar en el log los errores no manejados
/// </summary>
public class FilterException : ExceptionFilterAttribute
{
    private readonly ILogger<FilterException> logger;

    public FilterException(ILogger<FilterException> logger)
    {
        this.logger = logger;
    }

    public override void OnException(ExceptionContext context)
    {
        logger.LogError(context.Exception, context.Exception.Message);
        base.OnException(context);
    }
}