using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Application.Interfaces;
using Infrastructure.Repositories;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {

            services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

            services.AddScoped<PasswordService>();
            services.AddScoped<JwtService>();

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IExportExcelService, GenerateExcelService> ();
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