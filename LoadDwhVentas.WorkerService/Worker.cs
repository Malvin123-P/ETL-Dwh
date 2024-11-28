using LoadDwhVentas.Data.Contratos;

namespace LoadDwhVentas.WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration configuration;
        private readonly IServiceScopeFactory serviceScopeFactory;

        public Worker(ILogger<Worker> logger, IConfiguration configuration, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            this.configuration = configuration;
            this.serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                    using (var scope = serviceScopeFactory.CreateScope()) {

                        var dataService = scope.ServiceProvider.GetRequiredService<IDataServicesWorker>();
                        var result = await dataService.LoadDwh();

                        if (!result.Success)
                        {
                            //Enviar notificacion
                        }
                    };
                }

                await Task.Delay(configuration.GetValue<int>("timerTime"), stoppingToken);
            }
        }
    }
}
