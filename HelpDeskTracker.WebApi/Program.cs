
using HelpDeskTracker.Application;
using HelpDeskTracker.Application.Logic.Abstractions;
using HelpDeskTracker.Infrastructure.Auth;
using HelpDeskTracker.Infrastructure.Persistance;
using HelpDeskTracker.WebApi.Application.Auth;
using HelpDeskTracker.WebApi.Middlewares;
using Serilog;

namespace HelpDeskTracker.WebApi
{
    public class Program
    {
        public static string APP_NAME = "HelpDeskTracker.WebApi";
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.WithProperty("HelpDeskTracker", APP_NAME)
                .Enrich.WithProperty("MachineName", Environment.MachineName)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateBootstrapLogger();

            var builder = WebApplication.CreateBuilder(args);

            if (builder.Environment.IsDevelopment())
            {
                builder.Configuration.AddJsonFile("appsettings.Development.local.json");
            }

            builder.Host.UseSerilog((context, services, configuration) => configuration
                .Enrich.WithProperty("BookServices", APP_NAME)
                .Enrich.WithProperty("MachineName", Environment.MachineName)
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext());

            // Add services to the container.
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddDatabaseCache();
            builder.Services.AddSqlDatabase(builder.Configuration.GetConnectionString("MainDbSql")!);
            builder.Services.Configure<JwtAuthenticationOptions>(builder.Configuration.GetSection("JwtAuthentication"));

            builder.Services.AddJwtAuthenticationDataProvider(builder.Configuration);

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddValidators();

            builder.Services.AddSwaggerGen();


            builder.Services.AddMediatR(c =>
            {
                c.RegisterServicesFromAssemblyContaining(typeof(BaseCommandHandler));
            });

            builder.Services.AddApplicationServices();

            builder.Services.AddSwaggerGen(o =>
            {
                o.CustomSchemaIds(x =>
                {
                    var name = x.FullName;
                    if (name != null)
                    {
                        name = name.Replace("+", "_"); // swagger bug fix
                    }

                    return name;
                });
            });

            builder.Services.AddCors();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors(builder => builder
               .WithOrigins(app.Configuration.GetValue<string>("WebAppBaseUrl") ?? "")
               .WithOrigins(app.Configuration.GetSection("AdditionalCorsOrigins").Get<string[]>() ?? new string[0])
               .WithOrigins((Environment.GetEnvironmentVariable("AdditionalCorsOrigins") ?? "").Split(',').Where(h => !string.IsNullOrEmpty(h)).Select(h => h.Trim()).ToArray())
               .AllowAnyHeader()
               .AllowCredentials()
               .AllowAnyMethod());

            app.UseExceptionResultMiddleware();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
