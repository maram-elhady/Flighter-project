using Flighter.Models.DBModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Flighter.modelsConfig
{
    public class CompanyConfiguration : IEntityTypeConfiguration<CompanyModel>
    {
        public void Configure(EntityTypeBuilder<CompanyModel> builder)
        {
            builder
                .HasKey(c => c.CompanyId);

            //relationships
            builder
                .HasMany(c => c.Flights)
                .WithOne(c => c.Company)
                .HasForeignKey(c => c.FlightId)
                .OnDelete(DeleteBehavior.Cascade);
            builder
                .HasMany(c => c.Users)
                .WithOne(c => c.Company)
                .HasForeignKey(c => c.CompanyId)
                .OnDelete(DeleteBehavior.SetNull);

            builder
                .HasMany(c => c.Tickets)
                .WithOne(c => c.Company)
                .HasForeignKey(c => c.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
