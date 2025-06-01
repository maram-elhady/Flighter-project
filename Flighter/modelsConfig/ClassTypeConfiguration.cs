using Flighter.Models.DBModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Flighter.modelsConfig
{
    public class ClassTypeConfiguration : IEntityTypeConfiguration<ClassTypeModel>
    {
        public void Configure(EntityTypeBuilder<ClassTypeModel> builder)
        {
            builder
                .HasKey(ct => ct.classTypeId);


            builder
                .HasMany(ct=> ct.tickets)
                .WithOne(t => t.classType)
                .HasForeignKey(ct => ct.classTypeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
