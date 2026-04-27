using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.Text;
using ztpai.Middleware;
using ztpai.Models;

namespace ztpai
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDbContext<MyDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddIdentity<User, IdentityRole<int>>()
                .AddEntityFrameworkStores<MyDbContext>()
                .AddDefaultTokenProviders();

            var jwtKey = builder.Configuration["JwtKey"]; //TODO: przenieść do appsettings.json

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true
                };
            });

            builder.Services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        var errors = context.ModelState.Values
                            .SelectMany(v => v.Errors)
                            .Select(e => e.ErrorMessage);

                        var validationError = new
                        {
                            error = "Validation error",
                            message = string.Join(", ", errors)
                        };

                        return new Microsoft.AspNetCore.Mvc.BadRequestObjectResult(validationError);
                    };
                });
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();


            var app = builder.Build();

            // Uruchomienie seedowania (role + admin)
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

                await SeedRoles(roleManager);
                await SeedAdminUser(userManager);
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseExceptionHandler(_ => { });

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();

        }

        private static async Task SeedRoles(RoleManager<IdentityRole<int>> roleManager)
        {
            string[] roleNames = { "admin", "user" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole<int> { Name = roleName });
                }
            }
        }

        private static async Task SeedAdminUser(UserManager<User> userManager)
        {
            var adminEmail = "admin@example.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var user = new User 
                { 
                    Email = adminEmail, 
                    UserName = adminEmail,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(user, "AdminPassword123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "admin");
                }
            }
        }
    }
}
