using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TestAzureSignalRServer
{
    using Hub;
    using Microsoft.AspNetCore.Http.Connections;
    using Microsoft.Azure.SignalR;
    using System;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(c => { c.AllowAnyHeader().AllowAnyMethod().AllowCredentials().SetIsOriginAllowed(o => true); });

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
                {
                    endpoints.MapHub<TestHub>("TestHub", o => { o.Transports = HttpTransportType.ServerSentEvents; });
                });
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSignalR()
                .AddAzureSignalR(o =>
                {
                    o.AccessTokenLifetime = TimeSpan.FromMinutes(1);
                    o.Endpoints = new ServiceEndpoint[]
                    {
                        new ServiceEndpoint(Configuration["AzureSignalRPath"], EndpointType.Primary, "Test")
                    };
                });
        }
    }
}