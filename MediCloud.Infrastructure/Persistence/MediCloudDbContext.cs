using MediCloud.Domain.User;
using Microsoft.EntityFrameworkCore;

namespace MediCloud.Infrastructure.Persistence;

public class MediCloudDbContext(DbContextOptions options) : DbContext(options) {
    public DbSet<User> Users { get; set; }
}
