using Flighter.Models.DBModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Flighter.modelsConfig
{
    public class BookingSeatsConfiguration : IEntityTypeConfiguration<BookingSeatModel>
    {
        public void Configure(EntityTypeBuilder<BookingSeatModel> builder)
        {
            builder
            .HasKey(bs => bs.BookingSeatId);

            builder
                .Property(bs => bs.BookingSeatId)
                .ValueGeneratedOnAdd();

            // Relationships

            builder
                .HasOne(bs => bs.Booking)
                .WithMany(b => b.BookingSeats)
                .HasForeignKey(bs => bs.BookingId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(bs => bs.Booking)
                .WithMany(b => b.BookingSeats)
                .HasForeignKey(bs => bs.BookingId)
                .OnDelete(DeleteBehavior.Restrict); 


        }
    }
}
