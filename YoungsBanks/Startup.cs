using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Scrutor;
using AutoMapper;
using CQRSlite.Bus;
using CQRSlite.Commands;
using CQRSlite.Events;
using CQRSlite.Domain;
using CQRSlite.Cache;
using CQRSlite.Config;
using Swashbuckle.Swagger.Model;
using FluentValidation.AspNetCore;
using YoungsCQRS.ReadModel.Infrastructure;
using YoungsCQRS.WriteModel;
using YoungsCQRS.ReadModel;
using YoungsCQRS.WriteModel.Handlers;
using YoungsBanks.AutoMapProfile;
using YoungsBanks.Requests;
using YoungsBanks.Filters;

namespace YoungsBanks
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            
            services.AddEntityFrameworkSqlServer();
            services.AddMvc();

            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AccountProfile());
            });
            var mapper = config.CreateMapper();
            services.AddSingleton(mapper);

            services.AddDbContext<AccountContext>(options => options.UseSqlServer(Configuration.GetConnectionString("AccountDB")));
            services.AddScoped(typeof(IAccountRepository<>), typeof(AccountRepository<>));
            services.AddScoped<IEventRepository, EventRepository>();

            //Add Cqrs services
            services.AddSingleton<InProcessBus>(new InProcessBus());
            services.AddSingleton<ICommandSender>(y => y.GetService<InProcessBus>());
            services.AddSingleton<IEventPublisher>(y => y.GetService<InProcessBus>());
            services.AddSingleton<IHandlerRegistrar>(y => y.GetService<InProcessBus>());
            services.AddScoped<ISession, Session>();
            services.AddSingleton<IEventStore, InMemoryEventStore>();            
            services.AddScoped<ICache, MemoryCache>();
            services.AddScoped<IRepository>(y => new CacheRepository(new Repository(y.GetService<IEventStore>()), y.GetService<IEventStore>(), y.GetService<ICache>()));                   
            
            //Scan for commandhandlers and eventhandlers
            services.Scan(scan => scan
                .FromAssemblies(typeof(AccountCommandHandler).GetTypeInfo().Assembly)
                    .AddClasses(classes => classes.Where(x => {
                        var allInterfaces = x.GetInterfaces();
                        return
                            allInterfaces.Any(y => y.GetTypeInfo().IsGenericType && y.GetTypeInfo().GetGenericTypeDefinition() == typeof(ICommandHandler<>)) ||
                            allInterfaces.Any(y => y.GetTypeInfo().IsGenericType && y.GetTypeInfo().GetGenericTypeDefinition() == typeof(IEventHandler<>));
                    }))
                    .AsSelf()
                    .WithTransientLifetime()
                    );

            //Register bus
            var serviceProvider = services.BuildServiceProvider();
            var registrar = new BusRegistrar(new DependencyResolver(serviceProvider));
            registrar.Register(typeof(AccountCommandHandler));

            // Add framework services.
            services.AddMvc()
                    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateAccountRequestValidator>());

            services.AddMvc().AddFluentValidation();

            services.AddScoped<BadRequestActionFilter>();

            services.AddSwaggerGen(options => {
                options.SingleApiVersion(new Info
                {
                    Version = "v1",
                    Title = "YoungsBanks Swagger Sample API",
                    Description = "API Sample made for YoungBanks",
                    TermsOfService = "Developped by Young Ho Son"
                });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));

            loggerFactory.AddDebug();

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));

            app.UseMvc(routes =>
            {
                 routes.MapRoute(
                 name: "default",
                 template: "{controller}/{action}/{id?}",
                 defaults: new { controller = "Account", action = "Index" });
            });
            

            app.UseSwagger();

            app.UseSwaggerUi();

        }
    }
}
