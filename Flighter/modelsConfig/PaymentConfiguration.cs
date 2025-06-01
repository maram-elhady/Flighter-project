using Flighter.Models.DBModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Flighter.modelsConfig
{
    public class PaymentConfiguration : IEntityTypeConfiguration<PaymentModel>
    {
        public void Configure(EntityTypeBuilder<PaymentModel> builder)
        {
            builder
                .HasKey(p => p.PaymentId);

            //relationships
            //builder
            //    .HasOne(p => p.user)
            //    .WithMany(u => u.payments)
            //    .HasForeignKey(p => p.userId)
            //    .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(p => p.booking)
                   .WithOne(b => b.payment)
                   .HasForeignKey<PaymentModel>(p => p.BookingId)
                   .OnDelete(DeleteBehavior.Cascade);


        }
    }
}
