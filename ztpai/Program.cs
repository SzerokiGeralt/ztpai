using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Minio;
using Scalar.AspNetCore;
using System.Text;
using ztpai.Middleware;
using ztpai.Repository;
using ztpai.Services;

namespace ztpai
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

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

            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IProductsRepository, ProductsRepository>();
            builder.Services.AddScoped<IProductsService, ProductsService>();
            builder.Services.AddScoped<IUsersRepository, UsersRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IProductImageService, MinioService>();
            builder.Services.AddScoped<ILoggingService, LoggingRabbitMQ>();

            builder.Services.AddDbContext<MyDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => 
                {
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters 
                    {
                        ValidateIssuer = true,
                        ValidIssuer = builder.Configuration["AppSettings:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = builder.Configuration["AppSettings:Audience"],
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Token"]!))
                    };
                });

            builder.Services.AddMinio(configureSource => configureSource
                .WithEndpoint(builder.Configuration["Minio:Address"])
                .WithCredentials(builder.Configuration["Minio:Login"], builder.Configuration["Minio:Password"])
                .WithSSL(false)); // When local we dont use SSL

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<MyDbContext>();
                dbContext.Database.Migrate();

                if (!dbContext.Users.Any(u => u.Role == "Admin"))
                {
                    var adminUser = new Models.User
                    {
                        Id = Guid.NewGuid(),
                        Username = "admin",
                        Role = "Admin"
                    };

                    adminUser.PasswordHash = new PasswordHasher<Models.User>().HashPassword(adminUser, "admin");
                    dbContext.Users.Add(adminUser);
                    dbContext.SaveChanges();
                }
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            app.UseCors(policy =>
            policy.WithOrigins("https://localhost:7222")
            .AllowAnyMethod()
            .AllowAnyHeader());

            app.UseExceptionHandler(_ => { });

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();

        }
    }
}
