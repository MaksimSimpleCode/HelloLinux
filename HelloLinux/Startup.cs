using HelloLinux.Infrastructure;
using HelloLinux.Models;
using HelloLinux.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HelloLinux
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<PictureContext>(options =>
            options.UseNpgsql(Configuration.GetConnectionString("PostgreConnection")));

            services.AddDbContext<ApplicationContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("PostgreConnection")));

            services.AddDbContext<ToDoContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("PostgreConnection")));

            services.AddIdentity<User, IdentityRole>(opts => {
                opts.Password.RequiredLength = 5;   // ����������� �����
                opts.Password.RequireNonAlphanumeric = false;   // ��������� �� �� ���������-�������� �������
                opts.Password.RequireLowercase = false; // ��������� �� ������� � ������ ��������
                opts.Password.RequireUppercase = false; // ��������� �� ������� � ������� ��������
                opts.Password.RequireDigit = false; // ��������� �� �����
                opts.User.RequireUniqueEmail = true;    // ���������� email
                //opts.User.AllowedUserNameCharacters = ".@abcdefghijklmnopqrstuvwxyz"; // ���������� �������
                opts.User.AllowedUserNameCharacters = null;
            })
                .AddEntityFrameworkStores<ApplicationContext>();
            services.AddTransient<IUserRepository, UserService>();
            services.AddControllersWithViews();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var appContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationContext>();
                var pictureContext = serviceScope.ServiceProvider.GetRequiredService<PictureContext>();
                var toDoContext = serviceScope.ServiceProvider.GetRequiredService<ToDoContext>();
                pictureContext.Database.Migrate();
                toDoContext.Database.Migrate();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
