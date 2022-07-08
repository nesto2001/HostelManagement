using DataAccess;
using DataAccess.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

namespace HostelManagementWorkerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    IConfiguration configuration = hostContext.Configuration;
                    services.AddDbContext<HostelManagementContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("HostelManagementContext")));
                    services.AddSingleton<IRentRepository, RentRepository>();
                    services.AddSingleton<IHostelRepository, HostelRepository>();
                    services.AddHostedService<Worker>();
                });
    }
}
