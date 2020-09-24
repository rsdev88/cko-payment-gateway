using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
            services.AddMvcCore(options =>
            {
                options.RespectBrowserAcceptHeader = true;
                options.Filters.Add(new ProducesAttribute("application/json"));
            }).AddJsonOptions(opts =>
            {
                opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });
            services.AddControllers();

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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseGlobalExceptionMiddleware();

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
