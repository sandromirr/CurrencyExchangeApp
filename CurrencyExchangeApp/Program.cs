using Microsoft.EntityFrameworkCore;
using CurrencyExchangeApp.Database;
using CurrencyExchangeApp.Repositories;

namespace CurrencyExchangeApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // Microsoft SQL Server
            builder.Services.AddDbContext<CurrencyExchangeDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("Local")));

            // Add Repositories
            builder.Services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            builder.Services.AddScoped<IAccountRepository, AccountRepository>();

            
            builder.Services.AddCors();
            
            builder.Services.AddControllers();
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

            app.UseCors(x => x
               .AllowAnyMethod()
               .AllowAnyHeader()
               .SetIsOriginAllowed(origin => true)
               .AllowCredentials());

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}