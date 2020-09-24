using APIAuthentication.Handlers;
using APIAuthentication.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PaymentGatewayApi.Mappers;
using PaymentGatewayApi.Middleware;
using PaymentGatewayApi.Models.CustomAttributes.ActionFilters;
using PaymentGatewayApi.Services;
using PaymentGatewayApi.Services.Banking;
using System;
using System.Text.Json.Serialization;

namespace PaymentGatewayApi
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
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });
            services.AddMvcCore(options =>
            {
                options.RespectBrowserAcceptHeader = true;
                options.Filters.Add(new ProducesAttribute("application/json"));
            }).AddJsonOptions(opts =>
            {
                opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
            services.AddControllers();

            services.AddAuthentication("BasicAuthentication")
                    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
            services.AddTransient<IApiAuthenicationService, DummyAuthenticationService>();

            services.AddScoped<ModelValidationAttribute>();
            services.AddTransient<IPaymentsProcessingService, DefaultPaymentsProcessingService>();
            services.AddTransient<IPaymentsRetrievalService, DefaultPaymentsRetrievalService>();
            services.AddTransient<IDtoMapper, DtoMapper>();
            services.AddHttpClient<IBankingService, BankingService>(client =>
                {
                    client.BaseAddress = new Uri(Configuration["bankingApi:baseUrl"]); 
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseGlobalExceptionMiddleware();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            loggerFactory.AddFile(Configuration["Logging:FilePath"]);
        }
    }
}
