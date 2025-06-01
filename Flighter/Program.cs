using Flighter.DTO.UserDto;
using Flighter.Helper;
using Flighter.Models;
using Flighter.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Flighter
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;

           // CreateHostBuilder(args).Build().Run(); //to run Remove Service



            // Add services to the container.

            

            builder.Services.AddMemoryCache();
            builder.Services.Configure<IConfiguration>(configuration.GetSection("SmtpSettings"));
            builder.Services.AddScoped<IPasswordHasher<RegisterModelDto>, PasswordHasher<RegisterModelDto>>();
            builder.Services.Configure<JWT>(configuration.GetSection("JWT"));

            

            builder.Services.AddControllers();


            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders(); // Add this line to enable token providers

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(configuration.GetConnectionString("cs1")));

            //builder.Services.AddDbContext<ApplicationDbContext>(options =>
            //   options.UseSqlServer(configuration.GetConnectionString("cs")));

            //Adding cors 
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                        {
                            builder
                              .AllowAnyMethod()
                              .AllowAnyHeader()
                              .WithOrigins("https://localhost:7246") //blazor url
                              .WithOrigins("http://flighter-dashboard.runasp.net")
                              .AllowCredentials();
                        });
            });

           
            //builder.Services.AddCors(options =>
            //{
            //    options.AddPolicy("AllowAll",
            //        policy => policy.AllowAnyOrigin()
            //                        .AllowAnyMethod()
            //                        .AllowAnyHeader());
            //});

            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IDashboardService, DashboardService>();
            builder.Services.AddScoped<IFlightService, FlightSerivce>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();
            builder.Services.AddScoped<IOfferService, OfferService>();


            builder.Services.AddHostedService<RemoveExpiredBookingsService>();
           // builder.Services.AddScoped<IServiceProvider>();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
             .AddCookie(options =>
              {
                     options.LoginPath = "/Dashboard/admin-login"; // 
                     options.AccessDeniedPath = "/Forbidden"; // Optional
                     options.Cookie.HttpOnly = true;
                     options.Cookie.SecurePolicy = CookieSecurePolicy.None;  // allow http
                     options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                     options.SlidingExpiration = true;
              });


            //add authorize button
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Flighter API", Version = "v1" });

                // Add JWT Authentication to Swagger
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' followed by a space and your JWT token.",
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
               {
                {
                     new OpenApiSecurityScheme
                 {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
                },
            new string[] {}
             }
             });
            });

            

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = false;
                o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = configuration["JWT:Issue"],
                    ValidAudience = configuration["JWT:Audeince"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"])),
                    ClockSkew = TimeSpan.Zero // no duration for expiration of token to still active
                };
            });

            

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("SpecificUserOnly", policy =>
                    policy.RequireAssertion(context =>
                        context.User.Identity?.Name == "admin@example.com"));
            });


            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Use `None` if running locally without HTTPS
                options.Cookie.SameSite = SameSiteMode.Lax; // Can try `None` if needed
                options.Cookie.Name = ".AspNetCore.Cookies";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(120);
                options.LoginPath = "/Dashboard/admin-login";
                options.AccessDeniedPath = "/Dashboard/access-denied";
            });


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            
            var app = builder.Build();



            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();
            app.UseDefaultFiles(); //redirect
            app.UseStaticFiles(); // Enable serving static files from wwwroot

            app.UseAuthentication();
            app.UseCors();
            app.UseAuthorization();


            app.MapControllers();
            app.Run();
        }

    //    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    //    {
    //        if (env.IsDevelopment())
    //        {
    //            app.UseDeveloperExceptionPage();
    //        }
    //        else
    //        {
    //            app.UseExceptionHandler("/Error");
    //            app.UseHsts();
    //        }
    //    }

    //    public static IHostBuilder CreateHostBuilder(string[] args) =>
    //Host.CreateDefaultBuilder(args)
    //    .ConfigureWebHostDefaults(webBuilder =>
    //    {
    //        webBuilder.ConfigureKestrel((context, options) =>
    //        {
    //            // Configure Kestrel to listen on HTTPS
    //            options.ListenAnyIP(7284, listenOptions =>
    //            {
    //                // listenOptions.UseHttps("C:\\Users\\Default\\AppData\\Roaming\\Microsoft\\dotnet\\https\\dev-certs.pfx");
    //                options.ListenAnyIP(7284);

    //            });
    //        });

    //        // Add the configuration directly here, without using Startup.cs
    //        webBuilder.ConfigureServices(services =>
    //        {
    //            // Register your services here
    //            services.AddHostedService<RemoveExpiredBookingsService>(); // Add background service
    //            // services.AddScoped<IPaymentService, PaymentService>(); // Register other services
    //        });

    //        // Define the middleware pipeline
    //        webBuilder.Configure(app =>
    //        {
    //            app.UseRouting();
    //            app.UseEndpoints(endpoints =>
    //            {
    //                endpoints.MapControllers(); // Assuming you're using controllers
    //            });
    //        });
    //    });



    }
}
