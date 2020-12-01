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
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Definition;
using Microservice.Common.EventBus.Interfaces;
using MicroserviceD.Application.Consumers;
using MicroserviceD.Application.StatesMachines;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Polly;
using System.Net.Http;
using MicroserviceD.API.Middleware;

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
                cfg.AddConsumer<SubmitOrderConsumer>(typeof(SubmitOrderConsumerDefinition));
                cfg.AddConsumer<OrderValidationCompletedConsumer>();

                cfg.AddRequestClient<ISubmitOrder>();
                cfg.AddRequestClient<ICheckOrderState>();
                cfg.AddRequestClient<IRemoteSimpleMessageRequest>();

                cfg.AddSagaStateMachine<OrderStateMachine, OrderState>(typeof(OrderStateMachineDefinition))
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

            #region Http with Polly

            IAsyncPolicy<HttpResponseMessage> httpWaitAndRetryPolicy =
            Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
            (result, span, retryCount, ctx) => Console.WriteLine($"Retrying({retryCount})...")
            );

            IAsyncPolicy<HttpResponseMessage> fallbackPolicy =
                Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                    .FallbackAsync(FallbackAction, OnFallbackAsync);

            IAsyncPolicy<HttpResponseMessage> wrapOfRetryAndFallback = Policy.WrapAsync(fallbackPolicy, httpWaitAndRetryPolicy);

            services.AddHttpClient("RemoteServiceByHttp", client =>
            {
                client.BaseAddress = new Uri("http://microservicec.api/");
                client.DefaultRequestHeaders.Add("Accept", "application/json");

            }).AddPolicyHandler(wrapOfRetryAndFallback);

            #endregion
        }
        private Task OnFallbackAsync(DelegateResult<HttpResponseMessage> response, Context context)
        {
            Console.WriteLine("About to call the fallback action. This is a good place to do some logging");
            return Task.CompletedTask;
        }

        private Task<HttpResponseMessage> FallbackAction(DelegateResult<HttpResponseMessage> responseToFailedRequest, Context context, CancellationToken cancellationToken)
        {
            Console.WriteLine("Fallback action is executing");

            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(responseToFailedRequest.Result.StatusCode)
            {
                Content = new StringContent($"The fallback executed, the original error was {responseToFailedRequest.Result.ReasonPhrase}")
            };
            return Task.FromResult(httpResponseMessage);
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

            app.UseRequestResponseLogging();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
