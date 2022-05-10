﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Common.Web.Core.TestWebApp;

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
        services.Configure<SmtpConfig>(options => Configuration.GetSection("SmtpConfig").Bind(options));
        services.Configure<AntiDosConfig>(options => Configuration.GetSection("AntiDosConfig").Bind(options));
        services.Configure<AntiXssConfig>(options => Configuration.GetSection("AntiXssConfig").Bind(options));
        services.Configure<ContentSecurityPolicyConfig>(options =>
            Configuration.GetSection("ContentSecurityPolicyConfig").Bind(options));

        services.AddCommonWeb();

        services.AddControllersWithViews(options =>
        {
            // options.UsePersianDateModelBinder(); // To use it globally (assuming your app only sends Persian dates to the server)
            options.UseYeKeModelBinder();
            options.Filters.Add(typeof(ApplyCorrectYeKeFilterAttribute));
        });
        services.AddRazorPages();

        services.AddDNTScheduler(options =>
        {
            // DNTScheduler needs a ping service to keep it alive.
            // If you don't need it, don't add it!
            options.AddPingTask("https://localhost:5001");

            options.AddScheduledTask<DoBackupTask>(
                utcNow =>
                {
                    var now = utcNow.AddHours(3.5);
                    return now.Day % 3 == 0 && now.Hour == 0 && now.Minute == 1 && now.Second == 1;
                },
                2);

            options.AddScheduledTask<SendEmailsTask>(
                utcNow => utcNow.Second == 1,
                1);

            options.AddScheduledTask<ExceptionalTask>(utcNow => utcNow.Second == 1);
            options.AddScheduledTask<LongRunningTask>(utcNow => utcNow.Second == 1);
        });
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
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseAntiDos();

        app.UseHttpsRedirection();

        app.UseContentSecurityPolicy();

        app.UseStaticFiles();

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                "default",
                "{controller=Home}/{action=Index}/{id?}");
            endpoints.MapRazorPages();
        });
    }
}