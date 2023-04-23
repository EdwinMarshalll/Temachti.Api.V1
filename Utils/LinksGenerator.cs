using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Temachti.Api.DTOs;

namespace Temachti.Api.Utils;

public class LinksGenerator
{
    private readonly IAuthorizationService authorizationService;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly IActionContextAccessor actionContextAccessor;

    public LinksGenerator(IAuthorizationService authorizationService, IHttpContextAccessor httpContextAccessor, IActionContextAccessor actionContextAccessor)
    {
        this.authorizationService = authorizationService;
        this.httpContextAccessor = httpContextAccessor;
        this.actionContextAccessor = actionContextAccessor;
    }

    #region ROLES
    public async Task<bool> IsAdmin()
    {
        var httpContext = httpContextAccessor.HttpContext;
        var result = await authorizationService.AuthorizeAsync(httpContext.User, "isAdmin");
        return result.Succeeded;
    }
    #endregion

    private IUrlHelper BuildURLHelper()
    {
        var factory = httpContextAccessor.HttpContext.RequestServices.GetRequiredService<IUrlHelperFactory>();
        return factory.GetUrlHelper(actionContextAccessor.ActionContext);
    }

    private string GetVersion()
    {
        var headers = httpContextAccessor.HttpContext.Request.Headers;
        if (headers.ContainsKey("x-version"))
        {
            return headers["x-version"];
        }

        return "1";
    }

    #region Links
    /// <summary> 
    /// Genera los links de hipermedia para Tecnologias
    /// </summary>
    /// <param name="dtoTechnology">Dto de tecnologia</param>
    public async Task GenerateLinks(DTOTechnology dtoTechnology)
    {
        var isAdmin = await IsAdmin();
        var Url = BuildURLHelper();
        var version = GetVersion();

        dtoTechnology.Links.Add(new HATEOASData(Url.Link($"getTechnologyByIdV{version}", new { id = dtoTechnology.Id }), rel: "self", action: "GET"));
        dtoTechnology.Links.Add(new HATEOASData(Url.Link($"getTechnologyByCodeV{version}", new { code = dtoTechnology.Code }), rel: "self", action: "GET"));

        if (isAdmin)
        {
            dtoTechnology.Links.Add(new HATEOASData(Url.Link($"updateTechnologyV{version}", new { id = dtoTechnology.Id }), rel: "update technology", action: "PUT"));
            dtoTechnology.Links.Add(new HATEOASData(Url.Link($"deleteTechnologyV{version}", new { id = dtoTechnology.Id }), rel: "delete technology", action: "DELETE"));
        }
    }
    /// <summary> 
    /// Genera los links de hipermedia para Entradas
    /// </summary>
    /// <param name="dtoEntry">Dto de entrada</param>
    public async Task GenerateLinks(DTOEntry dtoEntry)
    {
        var isAdmin = await IsAdmin();
        var Url = BuildURLHelper();
        var version = GetVersion();

        dtoEntry.Links.Add(new HATEOASData(Url.Link($"getEntryByIdV{version}", new { id = dtoEntry.Id }), rel: "self", action: "GET"));

        if (isAdmin)
        {
            dtoEntry.Links.Add(new HATEOASData(Url.Link($"updateEntryV{version}", new { id = dtoEntry.Id }), rel: "update entry", action: "PUT"));
            dtoEntry.Links.Add(new HATEOASData(Url.Link($"deleteEntryV{version}", new { id = dtoEntry.Id }), rel: "delete entry", action: "DELETE"));
        }
    }
    #endregion


}