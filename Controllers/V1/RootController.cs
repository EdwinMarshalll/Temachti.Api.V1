using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Temachti.Api.DTOs;
using Temachti.Api.Utils;

namespace Temachti.Api.Controllers.V1;

[ApiController]
[Route("api")]
[HeaderContainsAttribute("x-version", "1")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class RootController : ControllerBase
{
    private readonly IAuthorizationService authorizationService;

    public RootController(IAuthorizationService authorizationService)
    {
        this.authorizationService = authorizationService;
    }

    [HttpGet(Name = "getRootV1")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<HATEOASData>>> Get()
    {
        var HATEOASDatas = new List<HATEOASData>();

        var isAdmin = await authorizationService.AuthorizeAsync(User, "isAdmin");

        HATEOASDatas.Add(new HATEOASData(href: Url.Link("getRootv1", new { }), rel: "self", method: "GET"));

        #region TECHNOLOGY
        HATEOASDatas.Add(new HATEOASData(href: Url.Link("getTechnologiesV1", new { }), rel: "technologies", method: "GET"));
        #endregion

        if (isAdmin.Succeeded)
        {
            HATEOASDatas.Add(new HATEOASData(href: Url.Link("createTechnology", new { }), rel: "autor-crear", method: "POST"));
            HATEOASDatas.Add(new HATEOASData(href: Url.Link("updateTechnology", new { }), rel: "libro-crear", method: "POST"));
        }

        return HATEOASDatas;
    }
}