using Application.Interfaces;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Helpers
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {

            services.AddDbContext<DatabaseContext>(options =>
                options.UseNpgsql(config.GetConnectionString("DefaultConnection")));


            services.AddIdentityCore<User>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 8;
            })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<DatabaseContext>();
            services.AddScoped<PasswordService>();
            services.AddScoped<JwtService>();

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IExportExcelService, GenerateExcelService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<IExpenseRepository, ExpenseRepository>();
            services.AddScoped<ISubtaskRepository, SubtaskRepository>();

            return services;
        }
    }
}