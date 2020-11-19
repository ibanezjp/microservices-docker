using System;
using GreenPipes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MassTransit;
using MicroserviceC.Application.Consumers;

namespace MicroserviceC.API
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
            services.AddMassTransit(x =>
            {
                x.AddConsumer<SimpleMessageConsumer>();
                x.AddConsumer<FaultSimpleMessageConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("rabbitmq-server-web");

                    cfg.ReceiveEndpoint("MicroserviceC_SimpleMessage_Queue",receiveEndpointConfiguration =>
                    {
                        receiveEndpointConfiguration.ConfigureConsumer<SimpleMessageConsumer>(context); 
                        receiveEndpointConfiguration.ConfigureConsumer<FaultSimpleMessageConsumer>(context);

                        receiveEndpointConfiguration.AutoDelete = false;

                        receiveEndpointConfiguration.UseMessageRetry(x =>
                            x.Incremental(5, TimeSpan.Zero, TimeSpan.FromSeconds(1)));
                    });

                    //cfg.ReceiveEndpoint("MicroserviceC_SimpleMessage_Queue_error", receiveEndpointConfiguration =>
                    //{


                    //    receiveEndpointConfiguration.AutoDelete = false;

                    //    receiveEndpointConfiguration.UseMessageRetry(x =>
                    //        x.Incremental(2, TimeSpan.Zero, TimeSpan.FromSeconds(1)));
                    //});
                });
            });

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
