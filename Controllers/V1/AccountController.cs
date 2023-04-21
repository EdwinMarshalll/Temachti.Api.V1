using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Temachti.Api.DTOs;
using Temachti.Api.Services;
using Temachti.Api.Utils;

namespace Temachti.Api.Controllers.V1;

[ApiController]
[Route("api/accounts")]
[HeaderContainsAttribute("x-version", "1")]
public class AccountController:ControllerBase
{
    private readonly UserManager<IdentityUser> userManager;
    private readonly IConfiguration configuration;
    private readonly SignInManager<IdentityUser> signInManager;
    private readonly HashService hashService;

    public AccountController(UserManager<IdentityUser> userManager, IConfiguration configuration, SignInManager<IdentityUser> signInManager, HashService hashService)
    {
        this.userManager = userManager;
        this.configuration = configuration;
        this.signInManager = signInManager;
        this.hashService = hashService;
    }

    [HttpPost("register", Name = "registerV1")]
    public async Task<ActionResult<DTOAuthenticationRequest>> Register(DTOUserCredentials userCredentials)
    {
        var user = new IdentityUser
        {
            UserName = userCredentials.Email,
            Email = userCredentials.Email
        };

        var result = await userManager.CreateAsync(user, userCredentials.Password);

        if(result.Succeeded)
        {
            return await CreateToken(userCredentials);
        }
        else
        {
            return BadRequest(result.Errors);
        }
    }

    [HttpPost("login", Name = "loginV1")]
    public async Task<ActionResult<DTOAuthenticationRequest>> Login(DTOUserCredentials userCredentials)
    {
        var result = await signInManager.PasswordSignInAsync(userCredentials.Email, userCredentials.Password, isPersistent: false, lockoutOnFailure: false);

        if(result.Succeeded)
        {
            return await CreateToken(userCredentials);
        }
        else
        {
            return BadRequest("Usuario o contrasena incorrectos");
        }
    }

    [HttpGet("RefreshToken", Name = "refreshTokenV1")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<DTOAuthenticationRequest>> RefreshToken()
    {
        var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
        var email = emailClaim.Value;
        var userCredentials = new DTOUserCredentials()
        {
            Email = email
        };

        return await CreateToken(userCredentials);
    }

    [HttpPost("AddAdmin", Name = "addAdminV1")]
    public async Task<ActionResult> ConvertToAdmin(DTOAdminEdit dtoAdminEdit)
    {
        var user = await userManager.FindByEmailAsync(dtoAdminEdit.Email);
        // hacer admin
        await userManager.AddClaimAsync(user, new Claim("isAdmin","algun valor"));
        return NoContent();
    }

    [HttpPost("RemoveAdmin", Name = "removeAdminV1")]
    public async Task<ActionResult> RemoveAdmin(DTOAdminEdit dtoAdminEdit)
    {
        var user = await userManager.FindByEmailAsync(dtoAdminEdit.Email);
        // remover admin
        await userManager.RemoveClaimAsync(user, new Claim("isAdmin","algun valor"));
        return NoContent();
    }

    // NOTE: ejemplo de utilizacion del servicio de Hash
    [HttpGet("Hash/{text}", Name = "hashV1")]
    public ActionResult Hash(string text)
    {
        var result = hashService.Hash(text);
        return Ok(result);
    }

    private async Task<DTOAuthenticationRequest> CreateToken(DTOUserCredentials userCredentials)
    {
        var claims = new List<Claim>()
        {
            new Claim("email", userCredentials.Email)
        };

        var user = await userManager.FindByEmailAsync(userCredentials.Email);
        var claimsDB = await userManager.GetClaimsAsync(user);
        claims.AddRange(claimsDB);

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["jwtkey"]));
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiration = DateTime.UtcNow.AddDays(6);

        var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiration, signingCredentials: cred);

        return new DTOAuthenticationRequest()
        {
            Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
            Expiration = expiration
        };
    }
}