using Flighter.Models.DBModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Flighter.modelsConfig
{
    public class FlightTypeConfiguration : IEntityTypeConfiguration<FlightTypeModel>
    {
        public void Configure(EntityTypeBuilder<FlightTypeModel> builder)
        {
            builder
                .HasKey(f => f.FlightTypeId);

            //relationship
            builder
                .HasMany(f => f.Flights)
                .WithOne(f => f.FlightType)
                .HasForeignKey(f => f.FlightTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
