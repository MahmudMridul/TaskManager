
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TaskManagerApi.Db;
using TaskManagerApi.Models;

namespace TaskManagerApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            #region Database Configuration
            builder.Services.AddDbContext<TaskManagerContext>(ops => 
            {
                ops.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
            });
            #endregion

            #region Identity Configuration
            builder.Services.AddIdentity<User, IdentityRole>(ops =>
            {
                ops.User.RequireUniqueEmail = true;

                ops.Password.RequiredLength = 8;
                ops.Password.RequireDigit = true;
                ops.Password.RequireUppercase = true;
                ops.Password.RequireNonAlphanumeric = true;
                
            })
            .AddEntityFrameworkStores<TaskManagerContext>()
            .AddDefaultTokenProviders();
            #endregion

            #region JWT Configuration
            builder.Services.AddAuthentication(ops =>
            {
                ops.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                ops.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(ops =>
            {
                ops.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });
            #endregion

            #region Role Configuraion
            builder.Services.AddAuthorization(ops =>
            {
                ops.AddPolicy("ApproverOnly", policy => policy.RequireRole("Approver"));
                ops.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
                ops.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
            });
            #endregion

            #region CORS
            builder.Services.AddCors(op =>
                op.AddPolicy(
                    "AllowAll",
                    policy => policy
                    .WithOrigins("http://localhost:5173")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                )
            );
            #endregion
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // CORS
            app.UseCors("AllowAll");

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
