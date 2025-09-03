using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Insurwave.Movie.Persistence.Postgres.EntityConfiguration;

public class MovieEntityConfiguration : IEntityTypeConfiguration<Interfaces.Contracts.Movie>
{
    public void Configure(EntityTypeBuilder<Interfaces.Contracts.Movie> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(p => p.Id).HasColumnOrder(0);
        builder.Property(p => p.OwningOrganisationId).HasColumnOrder(1);
        builder.Property(p => p.Title).HasColumnOrder(2);
        builder.Property(p => p.YearOfRelease).HasColumnOrder(3);
        builder.Property(p => p.ChangeTimestamp).HasColumnOrder(4);
    }
}
