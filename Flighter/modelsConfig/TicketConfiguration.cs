using Flighter.Models.DBModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Flighter.modelsConfig
{
    public class TicketConfiguration : IEntityTypeConfiguration<TicketModel>
    {
        public void Configure(EntityTypeBuilder<TicketModel> builder)
        {
            builder
                .HasKey(t => t.TicketId);

            //relationships
            builder
                .HasMany(t => t.seats)
                .WithOne(t => t.ticket)
                .HasForeignKey(f => f.ticketId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(t => t.bookings)
                .WithOne(t => t.ticket)
                .HasForeignKey(f => f.ticketId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(t => t.flight)
                .WithMany(f => f.tickets)
                .HasForeignKey(t => t.flightId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(t => t.classType)
                .WithMany(ct => ct.tickets)
                .HasForeignKey(t => t.classTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(t => t.schedule)
                .WithMany(s => s.tickets)
                .HasForeignKey(t => t.scheduleId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(t => t.Company)
                .WithMany(t => t.Tickets)
                .HasForeignKey(t => t.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
               .HasOne(u => u.Admin)
               .WithMany(t => t.tickets)
               .HasForeignKey(t => t.userId)
               .OnDelete(DeleteBehavior.NoAction);


        }
    }
}
