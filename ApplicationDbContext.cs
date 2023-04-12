using Microsoft.EntityFrameworkCore;
using Temachti.Api.Entities;

namespace Temachti.Api;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {

    }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
}