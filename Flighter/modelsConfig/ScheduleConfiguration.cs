using Flighter.Models.DBModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Flighter.modelsConfig
{
    public class ScheduleConfiguration : IEntityTypeConfiguration<ScheduleModel>
    {
        public void Configure(EntityTypeBuilder<ScheduleModel> builder)
        {
            builder
                .HasKey(s => s.ScheduleId);


            builder
                .HasMany(s => s.tickets)
                .WithOne(t => t.schedule)
                .HasForeignKey(s => s.scheduleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne (s => s.flight)
                .WithMany( s=> s.schedules)
                .HasForeignKey (s => s.flightId)
                .OnDelete (DeleteBehavior.Cascade);
        }
    }
}
