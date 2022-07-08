using BusinessObject.BusinessObject;
using DataAccess;
using DataAccess.Repository;
using HostelManagement.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HostelManagement
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddSession();
            services.AddMvc();
            services.AddDbContext<HostelManagementContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("HostelManagementContext")));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.AccessDeniedPath = "/AccessDenied";
                    options.LoginPath = "/Login";
                    options.LogoutPath = "/Logout";
                });
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IHostelRepository, HostelRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProvinceRepository, ProvinceRepository>();
            services.AddScoped<IDistrictRepository, DistrictRepository>();
            services.AddScoped<IWardRepository, WardRepository>();
            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<IHostelPicRepository, HostelPicRepository>();
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IRoomPicRepository, RoomPicRepository>();
            services.AddScoped<IRentRepository, RentRepository>();
            services.AddScoped<IBillRepository, BillRepository>();
            services.AddScoped<IBillDetailRepository, BillDetailRepository>();
            services.AddScoped<IRoomMemberRepository, RoomMemberRepository>();
            services.AddOptions();                                         
            var mailsettings = Configuration.GetSection("MailSettings");  
            services.Configure<MailSettings>(mailsettings);
            services.AddTransient<ISendMailService, SendMailService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();


            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapGet("/testmail", async context =>
                {

                    var sendmailservice = context.RequestServices.GetService<ISendMailService>();

                    MailContent content = new MailContent
                    {
                        To = "thanhdat3001@gmail.com",
                        Subject = "Test",
                        Body = "<p><strong>HIIIII</strong></p>"
                    };

                    await sendmailservice.SendMail(content);
                    await context.Response.WriteAsync("Send mail");
                });
            });
        }
    }
}
