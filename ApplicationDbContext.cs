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
        base.OnModelCreating(builder); // este no se borra sino,no funcionara el identity

        // aqui se pueden crear las llaves compuestas
    }

    // public DbSet<User> User { get; set; }
    // public DbSet<Role> Role { get; set; }
}