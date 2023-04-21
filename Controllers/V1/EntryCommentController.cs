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
[Route("api/entradas/{entryId:int}/comentarios")]
[HeaderContainsAttribute("x-version", "1")]
public class EntryCommentController : ControllerBase
{
    private readonly ApplicationDbContext context;
    private readonly IMapper mapper;
    private readonly UserManager<IdentityUser> userManager;
    private readonly ILogger<EntryCommentController> logger;

    public EntryCommentController(ApplicationDbContext context, IMapper mapper, UserManager<IdentityUser> userManager, ILogger<EntryCommentController> logger)
    {
        this.context = context;
        this.mapper = mapper;
        this.userManager = userManager;
        this.logger = logger;
    }

    [HttpGet(Name = "getEntryCommentsV1")]
    public async Task<ActionResult<List<DTOEntryComment>>> Get(int entryId)
    {
        var entryExist = await context.Entries.AnyAsync(entryDB => entryDB.Id == entryId);
        if (!entryExist)
        {
            return NotFound();
        }

        var entryComments = await context.EntryComments.Where(entryCommentDB => entryCommentDB.EntryId == entryId).ToListAsync();

        return mapper.Map<List<DTOEntryComment>>(entryComments);
    }

    [HttpGet("{id:int}", Name = "getEntryCommentByIdV1")]
    public async Task<ActionResult<DTOEntryComment>> GetById(int id)
    {
        var entryComment = await context.EntryComments.FirstOrDefaultAsync(entryCommentDB => entryCommentDB.Id == id);

        if (entryComment is null)
        {
            return NotFound();
        }

        return mapper.Map<DTOEntryComment>(entryComment);
    }

    [HttpPost(Name = "createEntryCommentV1")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> Post(int entryId, DTOEntryCommentCreate dtoEntryCommentCreate)
    {
        var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type =="email").FirstOrDefault();
        var email = emailClaim.Value;
        var user = await userManager.FindByEmailAsync(email);
        var userId = user.Id;

        var entryExists = await context.Entries.AnyAsync(entryDB => entryDB.Id == entryId);
        if (!entryExists)
        {
            return NotFound();
        }

        var entryComment = mapper.Map<EntryComment>(dtoEntryCommentCreate);
        entryComment.EntryId = entryId;
        entryComment.UserId = userId;
        entryComment.Likes = 0;
        entryComment.Modified = false;

        context.Add(entryComment);
        await context.SaveChangesAsync();

        var dtoEntryComment = mapper.Map<DTOEntryComment>(entryComment);

        return CreatedAtRoute("getEntryCommentByIdV1", new { Id = entryComment.Id, EntryId = entryId, }, dtoEntryComment);
    }

    [HttpPut("{id:int}", Name = "updateEntryCommentV1")]
    public async Task<ActionResult> Put(int entryId, int id, DTOEntryCommentCreate dtoEntryCommentCreate)
    {
        var entryExists = await context.EntryComments.AnyAsync(entryDB => entryDB.Id == entryId);
        if (!entryExists) 
        {
            return NotFound(); 
        }

        var entryCommentExists = await context.EntryComments.AnyAsync(entryCommentDB => entryCommentDB.Id == id);
        if (!entryCommentExists) 
        { 
            return NotFound(); 
        }

        var entryComment = mapper.Map<EntryComment>(dtoEntryCommentCreate);
        entryComment.EntryId = entryId;
        entryComment.Id = id;

        context.Update(entryComment);
        await context.SaveChangesAsync();
        return NoContent();
    }
}