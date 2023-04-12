using Microsoft.AspNetCore.Mvc;
using Temachti.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Temachti.Api.DTOs;
using AutoMapper;
using Temachti.Api.Utils;

namespace Temachti.Api.Controllers;

[ApiController]
[Route("api/users")]
// [Authorize]
public class UsersController : ControllerBase
{
    private readonly ApplicationDbContext context;
    private readonly IMapper mapper;
    private readonly ILogger<UsersController> logger;

    public UsersController(ApplicationDbContext context, IMapper mapper, ILogger<UsersController> logger)
    {
        this.context = context;
        this.mapper = mapper;
        this.logger = logger;
    }

    [HttpGet]               // api/users
    // [ResponseCache(Duration = 10)] //TODO: se almacena en cache la respuesta por 10 segundos.
    public async Task<ActionResult<List<DTOUser>>> Get()
    {
        var users = await context.Users.ToListAsync();
        return mapper.Map<List<DTOUser>>(users);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<DTOUser>> Get(int id)
    {
        var user = await context.Users.
            Include(userDb => userDb.Role).FirstOrDefaultAsync(userDB => userDB.Id == id);
        if (user is null)
        {
            return NotFound();
        }

        return mapper.Map<DTOUser>(user);
    }

    [HttpGet("{name}")]
    public async Task<ActionResult<List<DTOUser>>> Get([FromRoute] string name)
    {
        var users = await context.Users.Where(autorBD => autorBD.FirstName.Contains(name)).ToListAsync();
        return mapper.Map<List<DTOUser>>(users);
    }

    [HttpPost]
    public async Task<ActionResult> Post(string roleCode, DTOUserCreate dtoUserCreate)
    {
        var role = await context.Roles.FirstOrDefaultAsync(roleDB => roleDB.Code == roleCode);
        if(role is null)
        {
            return BadRequest("El rol no existe");
        }

        var userExists = await context.Users.FirstOrDefaultAsync(x => x.Nickname == dtoUserCreate.Nickname || x.Email == dtoUserCreate.Email);

        if (userExists is not null)
        {
            if (userExists.Email == dtoUserCreate.Email)
            {
                return BadRequest($"Ya existe el correo {dtoUserCreate.Email}.");
            }
            else
            {
                return BadRequest($"El nombre de usuario {dtoUserCreate.Nickname} ya existe.");
            }
        }

        var user = mapper.Map<User>(dtoUserCreate);
        user.RoleId = role.Id;
        user.CreatedAt = DateTime.Now;
        user.ModifiedAt = user.CreatedAt;

        context.Add(user);
        await context.SaveChangesAsync();
        return Ok();
    }

    [HttpPut]
    public ActionResult Put(User user)
    {
        return Ok();
    }

    [HttpDelete("{userId:int}")] // api/users/2
    public async Task<ActionResult> Delete(int id)
    {
        var exists = await context.Users.AnyAsync(x => x.Id == id);
        if (!exists)
        {
            return NotFound();
        }

        context.Remove(new User() { Id = id });
        await context.SaveChangesAsync();
        return Ok();
    }
}