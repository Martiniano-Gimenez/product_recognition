using Service.Implementations;
using Service.ServiceContracts;
using Microsoft.Extensions.Logging;
using Serilog;

namespace WorkerServiceShoppingCart
{
    public class Worker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<Worker> _logger;

        // Inyectamos ILogger<Worker> en el constructor
        public Worker(IServiceProvider serviceProvider, ILogger<Worker> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Log.Information("Iniciando proceso de actualización a las {Time}", DateTimeOffset.Now);

                using (var scope = _serviceProvider.CreateScope())
                {
                    var excelProcessService = scope.ServiceProvider.GetRequiredService<IExcelProcessService>();

                    try
                    {
                        Log.Information("Actualizando productos...");
                        await excelProcessService.UpdateProducts();

                        Log.Information("Actualizando clientes...");
                        await excelProcessService.UpdateClients();

                        Log.Information("Actualización completada con éxito.");
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "Error durante la actualización de datos.");
                    }
                }

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }
}
