using Microsoft.EntityFrameworkCore;

namespace AsigmaApiTemplate.Api.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
}