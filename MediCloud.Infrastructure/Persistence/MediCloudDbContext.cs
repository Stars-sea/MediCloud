using System.Reflection;
using MediCloud.Domain.Live;
using MediCloud.Domain.LiveRoom;
using MediCloud.Domain.Record;
using MediCloud.Domain.User;
using Microsoft.EntityFrameworkCore;

namespace MediCloud.Infrastructure.Persistence;

public class MediCloudDbContext(DbContextOptions options) : DbContext(options) {

    public DbSet<User> Users { get; set; }
    
    public DbSet<LiveRoom> LiveRooms { get; set; }
    
    public DbSet<Live> Lives { get; set; }
    
    public DbSet<Record> Records { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

}
