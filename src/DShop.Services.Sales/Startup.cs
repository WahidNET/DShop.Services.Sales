﻿using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Consul;
using DShop.Common.Consul;
using DShop.Common.Dispatchers;
using DShop.Common.Fabio;
using DShop.Common.Mvc;
using DShop.Common.RabbitMq;
using DShop.Common.RestEase;
using DShop.Services.Sales.Infrastructure;
using DShop.Services.Sales.Infrastructure.EF;
using DShop.Services.Sales.Services.Clients;
using DShop.Services.Sales.Services.Events;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace DShop.Services.Sales
{
    public class Startup
    {
        public IContainer Container { get; private set; }
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddCustomMvc();
            services.Configure<ApplicationOptions>(Configuration.GetSection("application"));
            services.Configure<SqlOptions>(Configuration.GetSection("sql"));
            services.AddEntityFramework();
//            services.AddHttpClient<IOrdersServiceClient, OrdersServiceClient>();
            services.AddConsul();
            services.AddFabio();
            services.RegisterServiceForwarder<IOrdersServiceClient>("orders-service");
            services.AddMemoryCache();
            services.AddDistributedRedisCache(cfg =>
            {
                cfg.Configuration = "localhost";
                cfg.InstanceName = "sales-service.";
            });

            return BuildContainer(services);
        }
        
        private IServiceProvider BuildContainer(IServiceCollection services)
        {
            var builder = new ContainerBuilder();
            builder.Populate(services);
            builder.AddDispatchers();
            builder.AddRabbitMq();
            builder.RegisterModule<InfrastructureModule>();

            Container = builder.Build();

            return new AutofacServiceProvider(Container);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            IApplicationLifetime lifetime, IDataSeeder dataSeeder,
            IConsulClient consulClient)
        {
            if (env.IsDevelopment() || env.IsEnvironment("local"))
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseErrorHandler();
            app.UseMvc();
            app.UseRabbitMq()
                .SubscribeEvent<ProductCreated>()
                .SubscribeEvent<OrderCreated>();
            var consulServiceId = app.UseConsul();

            lifetime.ApplicationStopped.Register(() =>
            {
                consulClient.Agent.ServiceDeregister(consulServiceId);
                Container.Dispose();
            });
        
            dataSeeder.SeedAsync().Wait();
        }
    }
}
