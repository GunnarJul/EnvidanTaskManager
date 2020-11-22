using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using EnviTaskManagerClient.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using Newtonsoft.Json;
using EnviTaskManagerClient.Models;
using System.Net;
using EnviTaskManagerClient.Services;

namespace EnviTaskManagerClient
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>
                    (options => options.UseSqlServer
                    (Configuration
                    .GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<IdentityUser>
                    (options => {
                        options.SignIn.RequireConfirmedAccount = true;
                        options.Password.RequireDigit = false;
                        options.Password.RequiredLength = 6;
                        options.Password.RequireNonAlphanumeric = false;
                    })
                    .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddControllersWithViews
                    (config =>
                    {
                        var policy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build();
                        config.Filters.Add(new AuthorizeFilter(policy));
                    });

            services.AddRazorPages();
            services.AddScoped<ITaskService, TaskService>();
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
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
                endpoints.MapRazorPages();
            });
            Administrator.AddAdministrator(app.ApplicationServices).Wait();
        }
        
    }

    public static class Administrator
    {
        public static async Task AddAdministrator(IServiceProvider serviceProvider)
        {
            using (var service = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var dbContext = service.ServiceProvider.GetService<ApplicationDbContext>();
                await dbContext.Database.MigrateAsync();
                var userManager = service.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
              

                var administrator = await userManager.FindByNameAsync("gjj@envidan.dk");
                if (administrator != null) return;

                var user = new IdentityUser ();
                user.Email =  "gjj@envidan.dk";
                user.UserName = "gjj@envidan.dk";
                user.EmailConfirmed = true;
                var newUser = await  userManager.CreateAsync(user, "EnvidanTaskManager");
                if (newUser.Succeeded)
                {
                    var test = await userManager.FindByNameAsync("gjj@envidan.dk");

                }

            }
        }
    }
}
