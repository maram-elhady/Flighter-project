using Flighter.Models.DBModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Flighter.modelsConfig
{
    public class FlightSeatConfiguration : IEntityTypeConfiguration<FlightSeatModel>
    {
        public void Configure(EntityTypeBuilder<FlightSeatModel> builder)
        {
            builder
                .HasKey(f => f.SeatId);

            //relationship

            builder
                .HasOne(f => f.ticket)
                .WithMany(f => f.seats)
                .HasForeignKey(f => f.ticketId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(s => s.User)
                .WithMany(u => u.seats)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
