using Iter.Api.Mapping;
using Iter.Core.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using System.Text;
using Iter.Services.Interface;
using Iter.Core.EntityModels;
using Iter.Repository.Interface;
using Iter.Repository;
using Iter.Services;
using Iter.Infrastrucure;

namespace Iter.Api.Infrastructure
{
    public static class StartupHelper
    {
        private const string DefaultCulture = "bs-BA";

        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<ICountryRepository, CountryRepository>();


            services.AddScoped<ICityService, CityService>();
            services.AddScoped<ICityRepository, CityRepository>();


            services.AddScoped<IReportService, ReportService>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();


            services.AddScoped<IAccommodationService, AccommodationService>();
            services.AddScoped<IAccommodationRepository, AccommodationRepository>();


            services.AddScoped<IArrangementService, ArrangementService>();
            services.AddScoped<IArrangementRepository, ArrangementRepository>();


            services.AddScoped<IDestinationService, DestinationService>();
            services.AddScoped<IDestinationRepository, DestinationRepository>();


            services.AddScoped<IEmployeeArrangmentService, EmployeeArrangmentService>();
            services.AddScoped<IEmployeeArrangmentRepository, EmployeeArrangmentRepository>();


            services.AddScoped<IReservationService, ReservationService>();
            services.AddScoped<IReservationRepository, ReservationRepository>();


            services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();

            services.AddScoped<IAgencyService, AgencyService>();
            services.AddScoped<IAgencyRepository, AgencyRepository>();

            services.AddScoped<IDropdownRepository, DropdownRepository>();
            services.AddScoped<IVerificationTokenRepository, VerificationTokenRepository>();

            services.AddTransient<IEmailService, EmailService>();

            services.AddTransient<IRecommendationSystemService, RecommendationSystemService>();

            services.AddTransient<IStatisticService, StatisticService>();

            services.AddScoped<IRabbitMQProducer, RabbitMQProducer>();

            services.AddSingleton(AutoMapperConfig.CreateMapping());

            services.AddHttpClient();
        }

        public static void ConfigureDb(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("AppDatabase");

            services.AddDbContext<IterContext>(options =>
                options.UseSqlServer(connectionString));

            var serviceProvider = services.BuildServiceProvider();
            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<IterContext>();
                dbContext.Database.Migrate();
            }
        }

        public static void ConfigureResponseCaching(this IServiceCollection services) => services.AddResponseCaching();

        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var builder = services.AddIdentity<User, IdentityRole>(o =>
            {
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequireUppercase = false;
                o.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<IterContext>()
            .AddDefaultTokenProviders();
        }

        public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtConfig = configuration.GetSection("jwtConfiguration");
            var secretKey = jwtConfig["secret"];
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtConfig["validIssuer"],
                    ValidAudience = jwtConfig["validAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Iter API",
                    Version = "v1",
                    Description = "Iter API Services.",
                    Contact = new OpenApiContact
                    {
                        Name = "Admir Numanović."
                    },
                });
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
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
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Program>()
                              .UseUrls("https://0.0.0.0:44351", "http://0.0.0.0:5156");
                });

        public static IServiceCollection AddCustomOptions(
                this IServiceCollection services,
                IConfiguration configuration)
        {
            return services
                .Configure<ApplicationOptions>(configuration)
                .AddSingleton(x => x.GetRequiredService<IOptions<ApplicationOptions>>().Value)
                .Configure<JwtConfiguration>(configuration.GetSection(nameof(ApplicationOptions.JwtConfiguration)))
                .AddSingleton(x => x.GetRequiredService<IOptions<JwtConfiguration>>().Value)
                .Configure<EmailSettings>(configuration.GetSection(nameof(ApplicationOptions.EmailSettings)))
                .AddSingleton(x => x.GetRequiredService<IOptions<EmailSettings>>().Value)
                .Configure<Core.Options.RabbitMqSettings>(configuration.GetSection(nameof(ApplicationOptions.RabbitMqSettings)))
                .AddSingleton(x => x.GetRequiredService<IOptions<Core.Options.RabbitMqSettings>>().Value)
                .Configure<ApplicationSettings>(configuration.GetSection(nameof(ApplicationOptions.ApplicationSettings)))
                .AddSingleton(x => x.GetRequiredService<IOptions<ApplicationSettings>>().Value);
        }
    }
}
