using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Temachti.Api.DTOs;
using Temachti.Api.Entities;
using Temachti.Api.Utils;
using Temachti.Api.Utils.HATEOAS;

namespace Temachti.Api.Controllers.V1;

[ApiController]
[Route("api/technologies")]
[HeaderContainsAttribute("x-version", "1")]
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

    /// <summary>
    /// Regresa todos las tecnologias
    /// </summary>
    [HttpGet(Name = "getTechnologiesV1")]
    [AllowAnonymous]
    [ServiceFilter(typeof(HATEOASTechnologyFilterAttribute))]
    public async Task<ActionResult<List<DTOTechnology>>> Get([FromQuery] DTOPagination dtoPagination)
    {
        var queryable = context.Technologies.AsQueryable();
        await HttpContext.InsertParametersIntoHeader(queryable);
        var technologies = await context.Technologies.OrderBy(techDB => techDB.Name).Paginate(dtoPagination).ToListAsync();
        return mapper.Map<List<DTOTechnology>>(technologies);
    }

    /// <summary>
    /// Regresa una tecnologia por su Id
    /// </summary>
    /// <param name="id">Id de la tecnologia</param>
    [HttpGet("{id:int}", Name = "getTechnologyByIdV1")]
    [AllowAnonymous]
    [ServiceFilter(typeof(HATEOASTechnologyFilterAttribute))]
    public async Task<ActionResult<DTOTechnology>> GetBytId(int id)
    {
        var technology = await context.Technologies.FirstOrDefaultAsync(techDB => techDB.Id == id);

        if (technology is null)
        {
            return NotFound();
        }

        return mapper.Map<DTOTechnology>(technology);
    }

    /// <summary>
    /// Regresa una tecnologia por su codigo
    /// </summary>
    /// <param name="code">Codigo de la tecnologia</param>
    [HttpGet("{code}", Name = "getTechnologyByCodeV1")]
    [AllowAnonymous]
    [ServiceFilter(typeof(HATEOASTechnologyFilterAttribute))]
    public async Task<ActionResult<DTOTechnology>> GetByCode(string code)
    {
        var technology = await context.Technologies.FirstOrDefaultAsync(techDB => techDB.Code == code);
        if (technology is null)
        {
            return NotFound();
        }

        return mapper.Map<DTOTechnology>(technology);
    }

    /// <summary>
    /// Crea una tecnologia
    /// </summary>
    /// <param name="dtoTechnologyCreate">Modelo de la tecnologia a crear</param>
    [HttpPost(Name = "createTechnologyV1")]
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

        return CreatedAtRoute("getTechnologyByIdV1", new { Id = technology.Id }, dtoTechnology);
    }

    /// <summary>
    /// Actualiza toda una tecnologia
    /// </summary>
    /// <param name="dtoTechnologyCreate">Modelo de la tecnologia a actualizar</param>
    /// <param name="id">Id de la tecnologia a actualizar</param>
    [HttpPut("{id:int}", Name = "updateTechnologyV1")]
    [ServiceFilter(typeof(HATEOASTechnologyFilterAttribute))]
    public async Task<ActionResult> Put(DTOTechnologyCreate dtoTechnologyCreate, int id)
    {
        var exists = await context.Technologies.AnyAsync(techDB => techDB.Id == id);
        if (!exists)
        {
            return NotFound();
        }

        var technology = mapper.Map<Technology>(dtoTechnologyCreate);
        technology.Id = id;

        context.Update(technology);
        await context.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>
    /// Actualiza parcialmente una tecnologia
    /// </summary>
    /// <param name="id">Id de la tecnologia a actualizar</param>
    /// <param name="patchDocument">objeto de los campos a actualizar</param>
    [HttpPatch("{id:int}", Name = "patchTechnologyV1")]
    [ServiceFilter(typeof(HATEOASTechnologyFilterAttribute))]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<ActionResult> Patch(int id, JsonPatchDocument<DTOTechnologyPatch> patchDocument)
    {
        if (patchDocument is null)
        {
            return BadRequest();
        }

        var techDB = await context.Technologies.FirstOrDefaultAsync(techDB => techDB.Id == id);

        if (techDB is null)
        {
            return NotFound();
        }

        var dtoTechnology = mapper.Map<DTOTechnologyPatch>(techDB);

        patchDocument.ApplyTo(dtoTechnology, ModelState);

        var isValid = TryValidateModel(dtoTechnology);
        if (!isValid)
        {
            return BadRequest(ModelState);
        }

        mapper.Map(dtoTechnology, techDB);

        await context.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>
    /// Borra una tecnologia
    /// </summary>
    /// <param name="id">Borra una tecnologia</param>
    [HttpDelete("{id:int}", Name = "deleteTechnologyV1")]
    [ServiceFilter(typeof(HATEOASTechnologyFilterAttribute))]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<ActionResult> Delete(int id)
    {
        var exists = await context.Technologies.AnyAsync(techDB => techDB.Id == id);
        if (!exists)
        {
            return NotFound();
        }

        context.Remove(new Technology() { Id = id });
        await context.SaveChangesAsync();
        return NoContent();
    }

}