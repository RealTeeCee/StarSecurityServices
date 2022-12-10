using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DataAccess.Data;

public class StarSecurityDbContext : IdentityDbContext<User>
{
    public StarSecurityDbContext(DbContextOptions options) : base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        //foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        //{
        //    var tableName = entityType.GetTableName();
        //    if (tableName.StartsWith("AspNet"))
        //    {
        //        entityType.SetTableName(tableName.Substring(6));
        //    }
        //}
    }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Branch> Branches { get; set; }
    public DbSet<CategoryBranch> CategoriesBranches { get; set; }
    public DbSet<Module> Modules { get; set; }
    public DbSet<Testimonial> Testimonials { get; set; }
    public DbSet<UserDetail> UserDetails { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<Language> Languages { get; set; }
    public DbSet<CategoryTranslation> CategoryTranslations { get; set; }
    public DbSet<ServiceTranslation> ServiceTranslations { get; set; }
    public DbSet<Session> Sessions { get; set; }
}
