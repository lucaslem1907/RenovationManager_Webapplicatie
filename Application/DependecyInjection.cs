using Application.Expenses;
using Application.Projects;
using Application.Rooms;
using Application.Subtaks;
using Application.Tasks;
using Application.Users;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public static class DependecyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<RegisterUseCase>();
            services.AddScoped<LoginUseCase>();

            services.AddScoped<CreateProjectUseCase>();
            services.AddScoped<GetProjectUseCase>();
            services.AddScoped<UpdateProjectUseCase>();
            services.AddScoped<DeleteProjectUseCase>();

            services.AddScoped<CreateExpenseUseCase>();
            services.AddScoped<GetExpenseUseCase>();
            services.AddScoped<UpdateExpenseUseCase>();
            services.AddScoped<DeleteExpenseUseCase>();

            services.AddScoped<CreateRoomUseCase>();
            services.AddScoped<GetRoomUseCase>();
            services.AddScoped<UpdateRoomUseCase>();
            services.AddScoped<DeleteRoomUseCase>();

            services.AddScoped<CreateTaskUseCase>();
            services.AddScoped<GetTaskUseCase>();
            services.AddScoped<UpdateTaskUseCase>();
            services.AddScoped<DeleteTaskUseCase>();

            services.AddScoped<CreateSubTaskUseCase>();
            services.AddScoped<GetSubtaskUseCase>();
            services.AddScoped<UpdateSubtaskUseCase>();
            services.AddScoped<DeleteSubtaskUseCase>();


            return services;
        }
    }
}
