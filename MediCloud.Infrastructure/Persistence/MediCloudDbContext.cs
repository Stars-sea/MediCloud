using System.Reflection;
using MediCloud.Domain.User;
using Microsoft.EntityFrameworkCore;

namespace MediCloud.Infrastructure.Persistence;

public class MediCloudDbContext(DbContextOptions options) : DbContext(options) {

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

}
