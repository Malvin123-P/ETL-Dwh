using LoadDwhVentas.Data.Context.Destino;
using LoadDwhVentas.Data.Context.Fuente;
using LoadDwhVentas.Data.Contratos;
using LoadDwhVentas.Data.Services;
using LoadDwhVentas.WorkerService;
using Microsoft.EntityFrameworkCore;

public class Program
{
    private static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {

                // Agregar aquí otros servicios necesarios

                services.AddDbContextPool<DbContextDwhSales>(options =>
                    options.UseSqlServer(hostContext.Configuration.GetConnectionString("DbSales")));

                services.AddDbContextPool<DbContexNortwind>(options =>
                    options.UseSqlServer(hostContext.Configuration.GetConnectionString("DbNortwind")));

                services.AddScoped<IDataServicesWorker,DataServicesWorker>();
                services.AddHostedService<Worker>();

             
            });


}