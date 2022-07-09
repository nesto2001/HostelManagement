using DataAccess;
using DataAccess.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using HostelManagement.Helpers;
using BusinessObject.BusinessObject;
using System.Configuration;

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
                    services.AddSingleton<IAccountRepository, AccountRepository>();
                    services.AddSingleton<IHostelRepository, HostelRepository>();
                    services.AddSingleton<ICategoryRepository, CategoryRepository>();
                    services.AddSingleton<IProvinceRepository, ProvinceRepository>();
                    services.AddSingleton<IDistrictRepository, DistrictRepository>();
                    services.AddSingleton<IWardRepository, WardRepository>();
                    services.AddSingleton<ILocationRepository, LocationRepository>();
                    services.AddSingleton<IHostelPicRepository, HostelPicRepository>();
                    services.AddSingleton<IRoomRepository, RoomRepository>();
                    services.AddSingleton<IRoomPicRepository, RoomPicRepository>();
                    services.AddSingleton<IRentRepository, RentRepository>();
                    services.AddSingleton<IBillRepository, BillRepository>();
                    services.AddSingleton<IBillDetailRepository, BillDetailRepository>();
                    services.AddSingleton<IRoomMemberRepository, RoomMemberRepository>();
                    services.AddOptions();
                    var mailsettings = configuration.GetSection("MailSettings");
                    services.Configure<MailSettings>(mailsettings);
                    services.AddSingleton<ISendMailService, SendMailService>();
                    services.AddHostedService<Worker>();
                });
    }
}
