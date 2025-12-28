using FCG.Users.API.Middlewares;
using FCG.Users.Domain.Entities;
using FCG.Users.Domain.Enums;
using FCG.Users.Domain.Interfaces.Security;
using FCG.Users.Infra.Contexts;
using Isopoh.Cryptography.Blake2b;
using Microsoft.EntityFrameworkCore;

namespace FCG.Users.API.Extensions
{
    public static class ApplicationExtensions
    {
        public static void UseProjectConfiguration(this WebApplication app)
        {
            app.UseCustomSwagger();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCustomMiddlewares();
            app.MapControllers();
            app.GenerateMigrations();
            app.MapHealthChecks("/health");
        }

        private static void UseCustomSwagger(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            var swaggerEnabled = config["Swagger:Enabled"];
            if (!string.IsNullOrEmpty(swaggerEnabled) && swaggerEnabled.ToLower() == "true")
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
        }

        public static void UseCustomMiddlewares(this WebApplication app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }

        private static void GenerateMigrations(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

            dbContext.Database.Migrate();

            var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();

            var email = config["SeedAdmin:Email"];
            var password = config["SeedAdmin:Password"];

            if (!string.IsNullOrEmpty(email) &&
            !string.IsNullOrEmpty(password) &&
            !dbContext.Users.Any(u => u.Email == email))
            {
                var hashedPassword = passwordHasher.Hash(password);

                var admin = new UserEntity()
                {
                    Name = "Admin",
                    Email = email,
                    Password = hashedPassword,
                    Role = ERole.Admin
                };

                dbContext.Users.Add(admin);
                dbContext.SaveChanges();
            }
        }
    }
}
