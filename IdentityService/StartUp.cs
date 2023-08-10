using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using IdentityService.Core;
using IdentityService.Domain;
using IdentityService.Domain.Config;
using System.IO;
using System.Text;

namespace IdentityService
{
    public class Startup
    {
        public Startup(IHostEnvironment hostEnvironment)
        {
            var builder = GetConfigToDevEnv(hostEnvironment.ContentRootPath);

            _config = builder.Build();
        }

        public IConfiguration _config { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthenticationConfig(_config);
            services.AddAuthorization();
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = _config["Redis:RedisConnection"];
            });

           

            services.AddCors(s =>
                s.AddPolicy("CorsPolicy", p => p
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                )
            );

            services.Configure<ConnectionStrings>(_config.GetSection("ConnectionStrings"));
            services.Configure<IdentityServerConfigs>(_config.GetSection("IdentityServerConfigs"));


            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });
            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });


            services.AddSwaggerConfig();


            services.RegisterDependencyInjectionContainers();

            services.Configure<EmailConfig>(_config.GetSection("EmailConfig"));

           
        }

        public void Configure(IApplicationBuilder app, IApiVersionDescriptionProvider provider)
        {
            app.UseSwagger();
            app.UseSwaggerConfig(provider);
            
            // app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
          
            app.UseCors("CorsPolicy");

            app.UseEndpoints(endpoint => endpoint.MapControllers());
           
        }

        private IConfigurationBuilder GetConfigToDevEnv(string basePath) =>
            new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables();
    }
}
