using MediCloud.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MediCloud.Infrastructure.Persistence;

public class MediCloudDbContext(DbContextOptions options) : IdentityDbContext<User>(options) {
    
}
