using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Temachti.Api.Filters;
using Temachti.Api.Middlewares;
using Temachti.Api.Services;
using Temachti.Api.Utils;

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
        #region CONTROLLERS
        services.AddControllers(opciones =>
            {
                // agregamos el filtro de excepciones
                opciones.Filters.Add(typeof(FilterException));
                // agregamos el versionador de swagger
                opciones.Conventions.Add(new SwaggerGroupByVersion());
            }
        ).AddJsonOptions(x =>
            {
                // ignoramos las referencias en bucle de EF
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            }
        ).AddNewtonsoftJson();
        #endregion

        #region DBCONTEXT
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
        );
        #endregion

        // NOTE: servicio recurrente comentado
        // services.AddHostedService<HostedWriteFile>();

        // NOTE: agregamos cache a las respuestas
        // services.AddResponseCaching();

        #region AUTHETICATION
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters{
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["jwtkey"])),
            ClockSkew = TimeSpan.Zero // tiempo de gracia por defecto es 5 minutos
        });
        #endregion

        #region SWAGGER
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Temachti.Api", Version = "v1" });
                c.SwaggerDoc("v2", new OpenApiInfo { Title = "Temachti.Api", Version = "v2" });
                
                // agregamos el filtro de HATEOAS y XVersion
                c.OperationFilter<AddHATEOASParameter>();
                c.OperationFilter<AddXVersionParameter>();
                
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
        #endregion

        #region AUTOMAPPER
        services.AddAutoMapper(typeof(Startup));
        #endregion

        #region IDENTITY
        services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders()
        ;
        #endregion

        #region CLAIMS AUTHORIZATION
        services.AddAuthorization(opciones => 
        {
            opciones.AddPolicy("isAdmin", politica => politica.RequireClaim("isAdmin"));
            opciones.AddPolicy("isDeveloper", politica => politica.RequireClaim("isDeveloper"));
        });
        #endregion

        #region CORS - reelevante unicamente para web apps
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
        #endregion

        // servicio para Hashear con una sal
        services.AddTransient<HashService>();

        #region HATEOAS
        services.AddTransient<LinksGenerator>();
        services.AddTransient<HATEOASTechnologyFilterAttribute>();
        services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
        #endregion
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
            app.UseSwaggerUI(c => 
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Temachti.Api v1");
                    c.SwaggerEndpoint("/swagger/v2/swagger.json", "Temachti.Api v2");
                }                
            );
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