using System;
using EfProblemSample.WebApi.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Extensions.Logging;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace EfProblemSample.WebApi
{
    public class Startup
    {
        public Startup()
        {
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddRouting(x => x.LowercaseUrls = true)
                .AddHealthChecks().AddDbContextCheck<Context>();
            services.AddMvcCore(options => options.EnableEndpointRouting = false);
            services.AddControllers();
            services.AddSwaggerGen();

            AddDatabase(services);
            AddLogging(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app
                .UseHealthChecks("/health")
                .UseSerilogRequestLogging()
                .UseSwagger()
                .UseMvc()
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "EfProblemSample V1");
                    c.RoutePrefix = "api";
                    c.DocExpansion(DocExpansion.List);
                    c.DisplayRequestDuration();
                });
        }

        public virtual void AddLogging(IServiceCollection services)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .Enrich.WithProperty("environment", environment)
                .Enrich.WithExceptionDetails()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();
            services.AddSingleton(new SerilogLoggerFactory(Log.Logger));
        }

        public virtual void AddDatabase(IServiceCollection services)
        {
            services.AddDbContext<Context>(
                o => o.UseSqlite("Data Source=EfProblemSample.db"));
        }
    }
}
