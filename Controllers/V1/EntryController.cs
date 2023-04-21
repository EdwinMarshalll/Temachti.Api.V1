using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Temachti.Api.DTOs;
using Temachti.Api.Entities;
using Temachti.Api.Utils;

namespace Temachti.Api.Controllers.V1;

[ApiController]
[Route("api/blog/entradas")]
[HeaderContainsAttribute("x-version", "1")]
public class EntryController : ControllerBase
{
    private readonly ApplicationDbContext context;
    private readonly IMapper mapper;
    private readonly UserManager<IdentityUser> userManager;
    private readonly ILogger<EntryController> logger;

    public EntryController(ApplicationDbContext context, IMapper mapper, UserManager<IdentityUser> userManager, ILogger<EntryController> logger)
    {
        this.context = context;
        this.mapper = mapper;
        this.userManager = userManager;
        this.logger = logger;
    }

    [HttpGet("{id:int}", Name = "getEntryByIdV1")]
    public async Task<ActionResult<DTOEntryWithTechnology>> GetEntryById(int id)
    {
        var entry = await context.Entries.Include(entryDB => entryDB.Technology).FirstOrDefaultAsync(entryDB => entryDB.Id == id);

        if (entry is null)
        {
            return NotFound();
        }

        return mapper.Map<DTOEntryWithTechnology>(entry);
    }

    [HttpPost(Name = "createEntryV1")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> Post(DTOEntryCreate dtoEntryCreate)
    {
        var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
        var email = emailClaim.Value;
        var user = await userManager.FindByEmailAsync(email);
        var userId = user.Id;

        var codeExists = context.Entries.Any(entryDB => entryDB.Code == dtoEntryCreate.Code);
        if (codeExists)
        {
            return BadRequest($"Ya existe una entrada con el codigo {dtoEntryCreate.Code}.");
        }

        var titleExists = context.Entries.Any(entryDB => entryDB.Title == dtoEntryCreate.Title);
        if (titleExists)
        {
            return BadRequest($"Ya existe una entrada con el titulo {dtoEntryCreate.Title}.");
        }

        var techExists = await context.Technologies.AnyAsync(techDB => techDB.Id == dtoEntryCreate.TechnologyId);
        if (!techExists)
        {
            return NotFound($"No se encontro la tecnologia {dtoEntryCreate.Code}.");
        }

        var entry = mapper.Map<Entry>(dtoEntryCreate);
        entry.CreatedAt = DateTime.Now;
        entry.ModifiedAt = DateTime.Now;
        entry.TechnologyId = dtoEntryCreate.TechnologyId;
        entry.Rating = 0;
        entry.UserId = userId;

        context.Add(entry);
        await context.SaveChangesAsync();

        var dtoEntry = mapper.Map<DTOEntry>(entry);

        return CreatedAtRoute("getEntryByIdV1", new { Id = entry.Id }, dtoEntry);
    }
}