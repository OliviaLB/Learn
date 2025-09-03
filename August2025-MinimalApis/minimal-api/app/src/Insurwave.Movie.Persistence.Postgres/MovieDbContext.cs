using Microsoft.EntityFrameworkCore;

namespace Insurwave.Movie.Persistence.Postgres;

public class MovieDbContext : DbContext
{
    protected MovieDbContext() : base(new DbContextOptions<MovieDbContext>())
    {
    }

    public MovieDbContext(DbContextOptions<MovieDbContext> options) : base(options)
    {
    }

    public virtual DbSet<Interfaces.Contracts.Movie> Movies { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}
