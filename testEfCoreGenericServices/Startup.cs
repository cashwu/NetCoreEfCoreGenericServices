using System;
using System.Collections.Generic;
using System.Reflection;
using GenericServices.Configuration;
using GenericServices.Setup;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql.Logging;

namespace testEfCoreGenericServices
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
            services.AddControllers();
            
            NpgsqlLogManager.Provider = new ConsoleLoggingProvider(NpgsqlLogLevel.Debug);
            
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"), builder =>
                {
                    builder.CommandTimeout(30);
                    builder.EnableRetryOnFailure(3, TimeSpan.FromSeconds(10), new List<string>());
                    builder.RemoteCertificateValidationCallback((sender, certificate, chain, errors) => true);
                });

                options.EnableSensitiveDataLogging();
            });

            services.GenericServicesSimpleSetup<AppDbContext>(new GenericServicesConfig
            {
                DtoAccessValidateOnSave = true,
                DirectAccessValidateOnSave = true,
                // SaveChangesExceptionHandler = (exception, context) =>
                // {
                //     Console.WriteLine("---- exceptions ----");    
                //     Console.WriteLine(exception.ToString());    
                //     
                //     return null;
                // }
            }, Assembly.GetAssembly(typeof(AppDbContext)));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}