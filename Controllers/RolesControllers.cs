using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Temachti.Api.DTOs;
using Temachti.Api.Entities;

namespace Temachti.Api.Controllers;

[ApiController]
[Route("api/roles")]
// [Authorize]
public class RolesController : ControllerBase
{
    private readonly ApplicationDbContext context;
    private readonly IMapper mapper;
    private readonly ILogger<RolesController> logger;

    public RolesController(ApplicationDbContext context, IMapper mapper, ILogger<RolesController> logger)
    {
        this.context = context;
        this.mapper = mapper;
        this.logger = logger;
    }

    // [HttpGet("{id:int}", Name = "GetRolById")]
    // public async Task<ActionResult<DTORole>> Get(int id)
    // {
    //     var role = await context.Roles.FirstOrDefaultAsync(roleDB => roleDB.Id == id);
    //     if (role is null)
    //     {
    //         return NotFound();
    //     }

    //     return mapper.Map<DTORole>(role);
    // }

    // [HttpGet]
    // public async Task<ActionResult<List<DTORole>>> Get()
    // {
    //     var roles = await context.Roles.ToListAsync();
    //     return mapper.Map<List<DTORole>>(roles);
    // }

    // [HttpPost]
    // public async Task<ActionResult> Post(DTORoleCreate dtoRoleCreate)
    // {
    //     var roleExists = await context.Roles.AnyAsync(roleDb =>
    //         roleDb.Code == dtoRoleCreate.Code ||
    //         roleDb.Name == dtoRoleCreate.Name
    //     );

    //     if (roleExists)
    //     {
    //         return BadRequest($"Ya existe un rol con el nombre o codigo proporcionado");
    //     }

    //     var role = mapper.Map<Role>(dtoRoleCreate);
    //     role.IsActive = true;

    //     context.Add(role);
    //     await context.SaveChangesAsync();

    //     var dtoRole = mapper.Map<DTORole>(role);

    //     return CreatedAtRoute("GetRolById", new { Id = role.Id }, dtoRole);
    // }

    // [HttpPut("{id:int}")]
    // public async Task<ActionResult> Put(DTORoleCreate dtoRoleCreate, int id)
    // {
    //     var roleExists = await context.Roles.AnyAsync(roleDB => roleDB.Id == id);
    //     if (!roleExists)
    //     {
    //         return NotFound();
    //     }

    //     var role = mapper.Map<Role>(dtoRoleCreate);
    //     role.Id = id;

    //     context.Update(role);
    //     await context.SaveChangesAsync();
    //     return NoContent();
    // }

    // [HttpPatch("{id:int}")]
    // public async Task<ActionResult> Pacth(int id, JsonPatchDocument<DTORoleCreate> patchDocument)
    // {
    //     if (patchDocument is null)
    //     {
    //         return BadRequest();
    //     }

    //     var role = await context.Roles.FirstOrDefaultAsync(roleDB => roleDB.Id == id);
    //     if (role is null)
    //     {
    //         return NotFound();
    //     }

    //     var dtoRole = mapper.Map<DTORoleCreate>(role);

    //     patchDocument.ApplyTo(dtoRole, ModelState);

    //     var isValid = TryValidateModel(dtoRole);
    //     if (!isValid)
    //     {
    //         return BadRequest(ModelState);
    //     }

    //     mapper.Map(dtoRole, role);

    //     await context.SaveChangesAsync();
    //     return NoContent();
    // }

    // [HttpDelete("{id:int}")]
    // public async Task<ActionResult> Delete(int id)
    // {
    //     var roleExists = await context.Roles.AnyAsync(x => x.Id == id);
    //     if (!roleExists)
    //     {
    //         return NotFound();
    //     }

    //     context.Remove(new Role() { Id = id });
    //     await context.SaveChangesAsync();
    //     return NoContent();
    // }

}