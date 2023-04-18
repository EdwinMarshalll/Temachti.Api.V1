using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Temachti.Api.Entities;

namespace Temachti.Api;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder); // este no se borra porque no funcionara el identity

        // aqui se pueden crear las llaves compuestas
    }

    public DbSet<Technology> Technologies { get; set; }
    public DbSet<Entry> Entries { get; set; }
    public DbSet<EntryComment> EntryComments { get; set; }
}