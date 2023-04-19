using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Temachti.Api.Filters;
using Temachti.Api.Middlewares;
using Temachti.Api.Services;

namespace Temachti.Api;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // limpiamos el mapeo de los tipos de claims. Por ejemplo el claim de tipo email no se reconoce si no se limpia
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
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters{
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["jwtkey"])),
            ClockSkew = TimeSpan.Zero // tiempo de gracia por defecto es 5 minutos
        });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Temachti.api", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            }
        );

        //configuramos AutoMapper
        services.AddAutoMapper(typeof(Startup));

        //configuramos el identity
        services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders()
        ;

        // agregamos autorizacion basada en claims
        services.AddAuthorization(opciones => 
        {
            opciones.AddPolicy("isAdmin", politica => politica.RequireClaim("isAdmin"));
            opciones.AddPolicy("isDeveloper", politica => politica.RequireClaim("isDeveloper"));
        });

        // agregamos politica de CORS - relevante para web apps
        services.AddCors(opciones => 
        {
            opciones.AddDefaultPolicy(builder =>
            {
                builder
                    .WithOrigins("https://localhost:7005","https://otra-ruta.com")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    //.WithExposedHeaders()
                ;
            });
        }); 

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

        app.UseCors();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}