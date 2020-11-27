using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Definition;
using Microservice.Common.EventBus.Interfaces;
using MicroserviceD.Application.Consumers;
using MicroserviceD.Application.StatesMachines;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MicroserviceD.API
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
            services.AddMediator(cfg =>
            {
                //cfg.AddConsumer<SubmitOrderConsumer>();
                //cfg.AddRequestClient<ISubmitOrder>();
            });

            services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);

            services.AddMassTransit(cfg =>
            {
                cfg.AddConsumer<SubmitOrderConsumer>();
                cfg.AddConsumer<OrderValidationConsumer>();
                cfg.AddRequestClient<ISubmitOrder>();
                cfg.AddRequestClient<ICheckOrderState>();
                
                cfg.AddSagaStateMachine<OrderStateMachine, OrderState>()
                    .RedisRepository(r =>
                    {
                        r.DatabaseConfiguration("redis-server");
                    });

                cfg.UsingRabbitMq((context, rabbitMqConfig) =>
                {
                    rabbitMqConfig.Host("rabbitmq-server-web");
                    rabbitMqConfig.ConfigureEndpoints(context);
                });
            });

            services.AddOpenApiDocument(cfg => cfg.PostProcess = d => d.Info.Title = "MassTransit Sample");

            services.AddMassTransitHostedService();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseOpenApi(); // serve OpenAPI/Swagger documents
            app.UseSwaggerUi3(); // serve Swagger UI

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
