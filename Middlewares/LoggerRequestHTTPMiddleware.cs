namespace Temachti.Api.Middlewares;

// clase de extension usada para ocultar en el startup el uso de la clase en el Startup
public static class LoggerRequestHTTPMiddlewareExtension
{
    public static IApplicationBuilder UseLoggerRequestHTTP(this IApplicationBuilder app)
    {
        return app.UseMiddleware<LoggerRequestHTTPMiddleware>();
    }
}

public class LoggerRequestHTTPMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger<LoggerRequestHTTPMiddleware> logger;

    public LoggerRequestHTTPMiddleware(RequestDelegate next, ILogger<LoggerRequestHTTPMiddleware> logger)
    {
        this.next = next;
        this.logger = logger;
    }

    //debe tener un metodo publico llamado Invoke o InvokeAsync y aceptar como primer parametro HttpContext
    public async Task InvokeAsync(HttpContext context)
    {
        using (var ms = new MemoryStream())
        {
            // guardamos la respuesta original
            var originalBodyRequest = context.Response.Body;

            // le agregamos un memory
            context.Response.Body = ms;

            //permitimos que la tuberia de procesos pueda continuar
            await next(context);

            //lo que venga de aqui en adelante se va a ejecutar cuando los middlewares me esten devolviendo una respuesta

            // vamos al inicio del stream
            ms.Seek(0, SeekOrigin.Begin);
            // Leemos el memory hasta el final y lo guardamos los que sea que se vaya a retornar al cliente
            string request = new StreamReader(ms).ReadToEnd();
            // vamos a colocar el memory en el inicio para enviar correctamente la respuesta al usuario
            ms.Seek(0, SeekOrigin.Begin);

            // copiamos el ms al cuerpo original
            await ms.CopyToAsync(originalBodyRequest);
            context.Response.Body = originalBodyRequest;

            //escribimos el log
            logger.LogInformation(request);
        }
    }
}