using FCG.Users.API.Filters;
using FCG.Users.API.Middlewares;
using FCG.Users.Domain.Interfaces.Common;
using FCG.Users.Domain.Interfaces.Repositories;
using FCG.Users.Domain.Interfaces.Security;
using FCG.Users.Domain.Interfaces.Services;
using FCG.Users.Domain.Validators;
using FCG.Users.Infra.Contexts;
using FCG.Users.Infra.Logging;
using FCG.Users.Infra.Middleware;
using FCG.Users.Infra.Repository;
using FCG.Users.Services.Auth;
using FCG.Users.Services.Security;
using FCG.Users.Services.User;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace FCG.Users.API.Extensions
{
    public static class BuilderExtensions
    {
        public static void AddProjectServices(this WebApplicationBuilder builder)
        {
            builder.UseJsonFileConfiguration();
            builder.ConfigureDbContext();
            builder.ConfigureJwt();
            //builder.ConfigureLogMongo();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.ConfigureSwagger();
            builder.ConfigureDI();
            builder.ConfigureHealthCheck();
            builder.ConfigureValidators();
        }

        private static void UseJsonFileConfiguration(this WebApplicationBuilder builder)
        {
            var keysDirectoryPath = Path.Combine(AppContext.BaseDirectory, "dataprotection-keys");
            var keysDirectory = new DirectoryInfo(keysDirectoryPath);

            if (!keysDirectory.Exists)
                keysDirectory.Create();

            builder.Services.AddDataProtection()
                .PersistKeysToFileSystem(keysDirectory)
                .SetApplicationName("FCG.Users");

            builder.Configuration
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
        }

        private static void ConfigureDbContext(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("FCG")));
        }

        private static void ConfigureJwt(this WebApplicationBuilder builder)
        {
            var configuration = builder.Configuration.GetSection("Jwt");

            builder.Services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = configuration["Issuer"],
                        ValidAudience = configuration["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(configuration["Key"]!)
                        ),

                        RoleClaimType = ClaimTypes.Role,
                        NameClaimType = ClaimTypes.NameIdentifier
                    };
                });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
            });

            builder.Services.AddScoped<TokenService>();
        }

        private static void ConfigureSwagger(this WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "FCG.Users.API",
                    Version = "v1",
                    Description = "Web API ASP.NET Core - Microserviço de Usuários",
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                            Scheme = "bearer"
                        },
                        Array.Empty<string>()
                    }
                });

                options.SchemaFilter<EnumSchemaFilter>();

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
        }


        private static void ConfigureDI(this WebApplicationBuilder builder)
        {
            // Repositories
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            // Services
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IUserService, UserService>();

            // Security
            builder.Services.AddScoped<IPasswordHasher, Argon2PasswordHasher>();

            // Observability
            builder.Services.AddScoped<ICorrelationIdGenerator, CorrelationIdGenerator>();
            builder.Services.AddScoped(typeof(IAppLogger<>), typeof(AppLogger<>));

            // Middlewares
            builder.Services.AddScoped<ExceptionMiddleware>();
            builder.Services.AddScoped(typeof(ValidationFilter<>));
        }

        private static void ConfigureHealthCheck(this WebApplicationBuilder builder)
        {
            builder.Services.AddHealthChecks()
                .AddNpgSql(
                    builder.Configuration.GetConnectionString("FCG")!,
                    name: "postgresql",
                    timeout: TimeSpan.FromSeconds(5)
                );
        }

        private static void ConfigureValidators(this WebApplicationBuilder builder)
        {
            builder.Services.AddValidatorsFromAssembly(typeof(CreateAdminRequestValidator).Assembly);
        }
    }
}
