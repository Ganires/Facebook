using System;
using System.Configuration;
using System.Net.Http;
using FacebookService.ApiConfig;
using FacebookService.Facebook;
using FacebookService.Quartz;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace FacebookService
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup (IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddTransient<FacebookApi>();
            services.AddTransient<IFacebookHttpClient, FacebookHttpClient>();
            services.AddTransient<IFacebookService, FacebookApiService>();
            services.AddTransient<Job>();
            services.AddTransient<IJobFactory, IoCJobFactory>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider container)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();

            QuartzStartup.Start(container);
        }
    }
}
