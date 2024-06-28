using ClassMate.Data;
using ClassMate.Domain.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

namespace ClassMate.API.ExtensionMethods
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDatabseContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ClassMateDbContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("DatabaseConnection")));
        }

        public static void AddJwtAuth(this IServiceCollection services, IConfiguration configuration)
        {
            var settingsSection = configuration.GetSection(typeof(JwtSettings).Name);

            var settings = settingsSection.Get<JwtSettings>();

            services.Configure<JwtSettings>(settingsSection);

            services.AddAuthentication(options => options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(cfg =>
                {
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = settings!.Issuer,
                        ValidAudience = settings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Key!))
                    };
                });
        }

        public static void AddMapperProfiles(this IServiceCollection services)
        {
            services.AddAutoMapper(mc =>
            {
                mc.AddProfile(new ClassMate.Data.MapperProfile());
                mc.AddProfile(new ClassMate.API.MapperProfile());
            });
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            var dataAssembly = Assembly.Load("ClassMate.Data");

            var dataTypes = dataAssembly.GetTypes().Where(p => p.Name.EndsWith("Repository"));

            foreach (var dataType in dataTypes)
            {
                var interfaceType = dataType.GetInterfaces().SingleOrDefault(i => i.Name == "I" + dataType.Name);
                if (interfaceType != null)
                {
                    services.AddScoped(interfaceType, dataType);
                }
            }
        }

        public static void AddServices(this IServiceCollection services)
        {
            var dataAssembly = Assembly.Load("ClassMate.Services");

            var dataTypes = dataAssembly.GetTypes().Where(p => p.Name.EndsWith("Service"));

            foreach (var dataType in dataTypes)
            {
                var interfaceType = dataType.GetInterfaces().SingleOrDefault(i => i.Name == "I" + dataType.Name);
                if (interfaceType != null)
                {
                    services.AddScoped(interfaceType, dataType);
                }
            }
        }

        public static void AddEmailSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
        }

        public static void AddGoogleSettigns(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<GoogleSettings>(configuration.GetSection("GoogleSettings"));
        }

        public static void AddCorsConfiguration(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder => builder.AllowAnyOrigin()
                                      .AllowAnyMethod()
                                      .AllowAnyHeader());
            });
        }

        public static void AddAuthSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(config =>
            {
                config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    In = ParameterLocation.Header,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                config.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
            });
        }
    }
}
