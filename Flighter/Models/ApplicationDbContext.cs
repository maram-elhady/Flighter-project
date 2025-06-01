using Flighter.Models.DBModels;
using Flighter.modelsConfig;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Flighter.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
          
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserLogin<string>>()
                .HasKey(login => new { login.UserId, login.LoginProvider, login.ProviderKey });
            modelBuilder.Entity<IdentityUserRole<string>>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });
            modelBuilder.Entity<IdentityUserToken<string>>()
                .HasKey(t => new { t.UserId, t.LoginProvider, t.Name });

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FlightConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BookingConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ClassTypeConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PaymentConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ScheduleConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TicketConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FlightSeatConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FlightTypeConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CompanyConfiguration).Assembly);

            //Roles seeding
            modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole { Id = "9a2d1c25-4d5b-42b1-846f-0f74813b3c57", Name = "Owner", NormalizedName = "OWNER" },
            new IdentityRole { Id = "bbd6f3b8-3fd3-47a9-b1f7-9eaf1c8b4478", Name = "Company", NormalizedName = "COMPANY" },
            new IdentityRole { Id = "cbd6d8e2-2fe8-47b2-a5dc-0d5a12f98e45", Name = "User", NormalizedName = "USER" }
            );

            //company seeding
            modelBuilder.Entity<CompanyModel>().HasData(
            new CompanyModel { CompanyId = 1, CompanyName = "Egypt Air" },
            new CompanyModel { CompanyId = 2, CompanyName = "Qatar Airways" },
            new CompanyModel { CompanyId = 3, CompanyName = "Emirates" } ,
            new CompanyModel { CompanyId = 4, CompanyName = "Kuwait Airline" }
            );

            //flighttypes seeding
            modelBuilder.Entity<FlightTypeModel>().HasData(
            new FlightTypeModel { FlightTypeId = 1, FlightName = "Direct" },
            new FlightTypeModel { FlightTypeId = 2, FlightName = "Round" }
            );

            //classtypes seeding
            modelBuilder.Entity<ClassTypeModel>().HasData(
            new ClassTypeModel { classTypeId = 1, className = "Business" },
            new ClassTypeModel { classTypeId = 2, className = "Economy" }
            );



        }
        public DbSet<CompanyModel> Companies { get; set; }
        public DbSet<FlightModel> flights { get; set; }
        public DbSet<TicketModel> tickets { get; set; }
        public DbSet<BookingModel> bookings { get; set; }    
        public DbSet<ScheduleModel> schedules { get; set; }
        public DbSet<ClassTypeModel> classTypes { get; set; }
        public DbSet<PaymentModel> payments { get; set; }
        public DbSet<FlightSeatModel> Flightseats     { get; set; }
        public DbSet<FlightTypeModel> FlightTypes { get; set; }
        public DbSet<BookingSeatModel> bookingSeats { get; set; }

    }
}
