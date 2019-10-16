using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SampleApi.BusinessLogic;

namespace SampleApi
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
            services.Configure<Storage.DatabaseSettings>(Configuration.GetSection(nameof(Storage.DatabaseSettings)));
            services.Configure<ExternalProvider.ExternalProviderConfiguration>(Configuration.GetSection(nameof(ExternalProvider.ExternalProviderConfiguration)));

            services.AddSingleton<Storage.IDatabaseSettings>(sp => sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<Storage.DatabaseSettings>>().Value);
            services.AddSingleton(sp => sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<ExternalProvider.ExternalProviderConfiguration>>().Value);

            services.AddSingleton<Storage.DataService>();

            services.AddTransient<BusinessLogic.AlertSystem>();

            services.AddHttpClient();
           // services.AddHttpClient<BusinessLogic.AlertSystem>();

            services.AddLogging();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseErrorHandling();

           // app.UseCors();
            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
