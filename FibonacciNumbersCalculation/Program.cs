using FibonacciNumbersCalculation.Services;
using FibonacciNumbersCalculation.Services.CacheServices;
using FibonacciNumbersCalculation.Services.FibonacciServices;
using FibonacciNumbersCalculation.Services.FibonacciAPIServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Net;
using FibonacciNumbersCalculation.Services.ExceptionFilters;
using FibonacciNumbersCalculation.Services.ExecutionTimeLimiter;
using FibonacciNumbersCalculation.Services.MemoryUsageLimiter;
using Microsoft.AspNetCore.Http;

namespace FibonacciAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

    }

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IFibonacciAPIService, FibonacciAPIService>();
            services.AddScoped<IFibonacciService, FibonacciService>();
            services.AddSingleton<ICacheService, CacheService>();
            services.AddTransient<IExecutionTimeLimiter, ExecutionTimeLimiter>();
            services.AddTransient<IMemoryUsageLimiter, MemoryUsageLimiter>();
            
            services.AddControllers(cfg =>
            {
                cfg.Filters.Add(typeof(FibonacciExceptionFilterAttribute));
            });

            services.AddLogging();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FibonacciAPI", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FibonacciAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
           
        }
    }
}