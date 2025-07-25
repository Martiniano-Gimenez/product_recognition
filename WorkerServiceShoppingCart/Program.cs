using Serilog;
using ShoppingCart.ProgramConfiguration;
using WorkerServiceShoppingCart;

var logPath = Path.Combine(AppContext.BaseDirectory, "logs", "myworker.log");

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File(logPath, rollingInterval: RollingInterval.Day)
    .CreateLogger();
try
{
    Log.Information("Iniciando aplicación...");

    var host = Host.CreateDefaultBuilder(args)
        .UseWindowsService()
        .UseSerilog()   // Aquí va UseSerilog
        .ConfigureServices((context, services) =>
        {
            services.ConfigureServices(context.Configuration);
            services.AddHttpContextAccessor();
            services.AddHostedService<Worker>();
        })
        .Build();

    await host.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "La aplicación falló al iniciar");
}
finally
{
    Log.CloseAndFlush();
}
