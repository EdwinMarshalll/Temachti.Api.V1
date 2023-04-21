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

    private IUrlHelper BuildURLHelper()
    {
        var factory = httpContextAccessor.HttpContext.RequestServices.GetRequiredService<IUrlHelperFactory>();
        return factory.GetUrlHelper(actionContextAccessor.ActionContext);
    }

    #region Links
    public async Task GenerateLinks(DTOTechnology dtoTechnology)
    {
        var isAdmin = await IsAdmin();
        var Url = BuildURLHelper();

        dtoTechnology.Links.Add(new HATEOASData(Url.Link("getTechnologyById", new { id = dtoTechnology.Id }), rel: "self", method: "GET"));
        dtoTechnology.Links.Add(new HATEOASData(Url.Link("getTechnologyByCode", new {code = dtoTechnology.Code}), rel: "self", method: "GET"));

        if(isAdmin)
        {
            dtoTechnology.Links.Add(new HATEOASData(Url.Link("updateTechnology", new { id = dtoTechnology.Id }), rel: "update technology", method: "PUT"));
            dtoTechnology.Links.Add(new HATEOASData(Url.Link("deleteTechnology", new { id = dtoTechnology.Id }), rel: "delete technology", method: "DELETE"));
        }
    }
    #endregion

    #region ROLES
    public async Task<bool> IsAdmin()
    {
        var httpContext = httpContextAccessor.HttpContext;
        var result = await authorizationService.AuthorizeAsync(httpContext.User, "isAdmin");
        return result.Succeeded;
    }
    #endregion
}