using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Temachti.Api.DTOs;
using Temachti.Api.Entities;

namespace Temachti.Api.Controllers;

[ApiController]
[Route("api/tecnologias")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "isAdmin")]
public class TechnologyController : ControllerBase
{
    private readonly ApplicationDbContext context;
    private readonly IMapper mapper;
    private readonly UserManager<IdentityUser> userManager;
    private readonly ILogger<TechnologyController> logger;

    public TechnologyController(ApplicationDbContext context, IMapper mapper, UserManager<IdentityUser> userManager, ILogger<TechnologyController> logger)
    {
        this.context = context;
        this.mapper = mapper;
        this.userManager = userManager;
        this.logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<List<DTOTechnology>>> Get()
    {
        var technologies = await context.Technologies.ToListAsync();
        return mapper.Map<List<DTOTechnology>>(technologies);
    }

    [HttpGet("{id:int}", Name = "GetTechnologyById")]
    public async Task<ActionResult<DTOTechnology>> GetBytId(int id)
    {
        var technology = await context.Technologies.FirstOrDefaultAsync(techDB => techDB.Id == id);

        if (technology is null)
        {
            return NotFound();
        }

        return mapper.Map<DTOTechnology>(technology);
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> Post(DTOTechnologyCreate dtoTechnologyCreate)
    {
        var codeExists = await context.Technologies.AnyAsync(techDB => techDB.Code == dtoTechnologyCreate.Code);
        if (codeExists)
        {
            return BadRequest($"El codigo ya existe {dtoTechnologyCreate.Code}");
        }

        var nameExists = await context.Technologies.AnyAsync(techDB => techDB.Name == dtoTechnologyCreate.Name);
        if (nameExists)
        {
            return BadRequest($"El nombre ya existe {dtoTechnologyCreate.Name}");
        }

        var technology = mapper.Map<Technology>(dtoTechnologyCreate);
        technology.CreatedAt = DateTime.Now;
        
        context.Add(technology);
        await context.SaveChangesAsync();

        var dtoTechnology = mapper.Map<DTOTechnology>(technology);

        return CreatedAtRoute("GetTechnologyById", new { Id = technology.Id }, dtoTechnology);
    }
}