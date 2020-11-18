using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MassTransit;
using MediatR;
using MicroserviceB.Application.Consumers;
using MicroservicesB.Application;

namespace MicroserviceB.API
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
                //x.AddConsumer<DashboardFaultConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("rabbitmq-server-web");

                    cfg.ReceiveEndpoint("MicroserviceB_SimpleMessage_Queue", receiveEndpointConfiguration =>
                    {
                        receiveEndpointConfiguration.AutoDelete = false;
                        receiveEndpointConfiguration.ConfigureConsumer<SimpleMessageConsumer>(context);
                    });

                    //cfg.ReceiveEndpoint("LongProcessEvent_error", e =>
                    //{
                    //    e.ConfigureConsumer<DashboardFaultConsumer>(context);
                    //});
                });
            });

            services.AddMassTransitHostedService();

            services.AddControllers();

            services.AddMediatR(typeof(Register));
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
