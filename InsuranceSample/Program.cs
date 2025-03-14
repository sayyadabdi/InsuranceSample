using Domain;
using EF.Services;
using FluentValidation;
using InsuranceSample.Handlers;
using Microsoft.EntityFrameworkCore;

namespace InsuranceSample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(DomainAssembly).Assembly);

                cfg.AddOpenBehavior(typeof(ValidationBehaviorHandler<,>));
            });

            builder.Services.AddDbContext<DbService>(opt => opt.UseInMemoryDatabase("Demo"));
            builder.Services.AddScoped<DbService>();

            builder.Services.AddValidatorsFromAssembly(typeof(DomainAssembly).Assembly);

            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseExceptionHandler();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
