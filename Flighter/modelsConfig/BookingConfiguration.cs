using Flighter.Models.DBModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Flighter.modelsConfig
{
    public class BookingConfiguration : IEntityTypeConfiguration<BookingModel>
    {
        public void Configure(EntityTypeBuilder<BookingModel> builder)
        {
            builder
                .HasKey(b => b.BookingId);

            builder.Property(b => b.BookingId)
           .ValueGeneratedOnAdd();

            // relationships
            builder
                .HasOne(b => b.ticket)
                .WithMany(t => t.bookings)
                .HasForeignKey(b => b.ticketId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(b => b.payment)
           .WithOne(p => p.booking)
           .HasForeignKey<PaymentModel>(p => p.BookingId) // foreign key in Payments table
           .OnDelete(DeleteBehavior.Cascade);

            // relationships
            builder
                .HasOne(b => b.user)
                .WithMany(t => t.bookings)
                .HasForeignKey(b => b.userId)
                .OnDelete(DeleteBehavior.Restrict);


        }
    }
}
