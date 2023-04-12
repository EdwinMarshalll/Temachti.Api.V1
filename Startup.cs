using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Temachti.Api.Filters;
using Temachti.Api.Middlewares;
using Temachti.Api.Services;

namespace Temachti.Api;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers(opciones =>
            {
                // agregamos el filtro de excepciones
                opciones.Filters.Add(typeof(FilterException));
            }
        ).AddJsonOptions(x =>
            {
                // ignoramos las referencias en bucle de EF
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            }
        ).AddNewtonsoftJson();

        // Configuramos el servicio de DbContext
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
        );

        //TODO: servicio recurrente comentado
        // services.AddHostedService<HostedWriteFile>();

        //TODO: agregamos cache a las respuestas
        // services.AddResponseCaching();

        // agregamos autenticacion
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Temachti.api", Version = "v1" });
            }
        );

        //configuramos AutoMapper
        services.AddAutoMapper(typeof(Startup));
    }

    /// <summary>
    /// Configuracion de los middlewares
    /// </summary>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
    {
        app.UseLoggerRequestHTTP();

        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseResponseCaching();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}