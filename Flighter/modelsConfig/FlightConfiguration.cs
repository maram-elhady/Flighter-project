using Flighter.Models.DBModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Flighter.modelsConfig
{
    public class FlightConfiguration : IEntityTypeConfiguration<FlightModel>
    {
        public void Configure(EntityTypeBuilder<FlightModel> builder)
        {
            builder
                .HasKey(f => f.FlightId);

            //relationship

            builder
                .HasOne(f => f.Company)
                .WithMany(f => f.Flights)
                .HasForeignKey(f => f.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);


            builder
                .HasMany(f => f.schedules)
                .WithOne (f=> f.flight)
                .HasForeignKey (f => f.flightId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne (f => f.FlightType)
                .WithMany(f => f.Flights)
                .HasForeignKey(f => f.FlightTypeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany (f => f.tickets)
                .WithOne(f => f.flight)
                .HasForeignKey(f => f.flightId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
