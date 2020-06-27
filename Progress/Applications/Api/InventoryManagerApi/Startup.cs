using System;
using DLogger.Extensions.Logging;
using Domain.Log.Logger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace InventoryManagerApi
{
    public class Startup
    {
        private const string urlPath = "localhost:44310";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddTransient<ILogger, Logger>();
            services.AddMvc();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Version = "v1",
                    Title = "My API",
                    Description = "My First ASP.NET Core Web API",
                    TermsOfService = new Uri(urlPath),
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact() { Name = "InventoryManager", Email = "Email here", Url = new Uri(urlPath) }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Inventory Manager V1");
            });

            loggerFactory.AddFile("Logs/myapp-{Date}.txt");

            var logWriter = new SqlServerLogWriter(Configuration.GetConnectionString("SqlServer3"));
            var logWriter2 = new SqlServerLogWriter(Configuration.GetConnectionString("SqlServer2"));
            loggerFactory.AddDLogger(Configuration.GetSection("Logging"), logWriter);
            loggerFactory.AddDLogger(Configuration.GetSection("Logging"), logWriter2);
            loggerFactory.AddDatabaseLogger();
        }
    }
}
