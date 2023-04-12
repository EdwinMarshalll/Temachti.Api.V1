namespace Temachti.Api.Services;

/// <summary>
/// Proceso recurrente que escribe en un archivo cada 60 segundos
/// <para>* Se debe injectar en el startup.</para>
/// </summary>
public class HostedWriteFile : IHostedService
{
    private readonly IWebHostEnvironment env;
    private readonly string fileName = "MyFile.txt";
    private Timer timer;

    public HostedWriteFile(IWebHostEnvironment env)
    {
        this.env = env;
    }

    ///<summary>
    /// Se ejecuta al incio de la api
    ///</summary>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(60)); // realiza la operacion 'DoWork', sin estado, que empiece en 0 segundos, y se ejecute cada 60 segundos
        WriteFile("Proceso iniciado");
        return Task.CompletedTask;
    }

    ///<summary>
    /// Se ejecuta al finalizar de la api de manera manual
    ///</summary>
    public Task StopAsync(CancellationToken cancellationToken)
    {
        timer.Dispose();
        WriteFile("Proceso finalizado");
        return Task.CompletedTask;
    }

    ///<summary>
    /// Metodo a ejecutarse de manera recurrente
    ///</summary>
    private void DoWork(object state)
    {
        WriteFile("Proceso en ejecucion: " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));
    }

    ///<summary>
    /// Escribe en un archivo
    ///</summary>
    public void WriteFile(string message)
    {
        var path = $@"{env.ContentRootPath}\wwwroot\{fileName}";

        using (StreamWriter writer = new StreamWriter(path, append: true))// append indica que se va a abrir el archivo y se escribira sobre ese. sin sustituir el archivo.
        {
            writer.WriteLine(message);
        }
    }
}