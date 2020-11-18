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
                //x.AddConsumer<DashboardFaultConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("rabbitmq-server-web");

                    cfg.ReceiveEndpoint("MicroserviceC_SimpleMessage_Queue",receiveEndpointConfiguration =>
                    {
                        receiveEndpointConfiguration.AutoDelete = false;
                        receiveEndpointConfiguration.ConfigureConsumer<SimpleMessageConsumer>(context);
                        //receiveEndpointConfiguration.UseMessageRetry(x =>
                        //    x.Incremental(5, TimeSpan.Zero, TimeSpan.FromSeconds(5)));
                        //receiveEndpointConfiguration.UseScheduledRedelivery(r =>
                        //    r.Intervals(
                        //        TimeSpan.FromMinutes(5),
                        //        TimeSpan.FromMinutes(15),
                        //        TimeSpan.FromMinutes(30)));
                    });

                    //cfg.ReceiveEndpoint("LongProcessEvent_error", e =>
                    //{
                    //    e.ConfigureConsumer<DashboardFaultConsumer>(context);
                    //});
                });
            });

            //services.Configure<HealthCheckPublisherOptions>(options =>
            //{
            //    options.Delay = TimeSpan.FromSeconds(2);
            //    options.Predicate = (check) => check.Tags.Contains("ready");
            //});


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

                //endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions()
                //{
                //    Predicate = (check) => check.Tags.Contains("ready"),
                //});

                //endpoints.MapHealthChecks("/health/live", new HealthCheckOptions());

            });
        }

        //public interface Fault<T>
        //    where T : class
        //{
        //    Guid FaultId { get; }
        //    Guid? FaultedMessageId { get; }
        //    DateTime Timestamp { get; }
        //    ExceptionInfo[] Exceptions { get; }
        //    HostInfo Host { get; }
        //    T Message { get; }
        //}

        //public class DashboardFaultConsumer :
        //    IConsumer<Fault<LongProcessEvent>>
        //{
        //    public async Task Consume(ConsumeContext<Fault<LongProcessEvent>> context)
        //    {
        //        // update the dashboard
        //    }
        //}
    }
}
