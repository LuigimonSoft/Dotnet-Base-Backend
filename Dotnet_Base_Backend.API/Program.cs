using Dotnet_Base_Backend.API.Middleware;
using Dotnet_Base_Backend.Repositories;
using Dotnet_Base_Backend.Repositories.Interfaces;
using Dotnet_Base_Backend.Services.Interfaces;
using Dotnet_Base_Backend.Services;

namespace Dotnet_Base_Backend.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            builder.Services.AddSingleton<IBaseRepository, BaseRepository>();
            builder.Services.AddTransient<IBaseService, BaseService>();

            var app = builder.Build();

            app.UseMiddleware<ErrorHandlerMiddleware>();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
