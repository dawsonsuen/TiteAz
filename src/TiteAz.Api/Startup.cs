using System;
using System.Data.SqlClient;
using System.Linq;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NEvilES.Pipeline;
using Newtonsoft.Json.Serialization;
using TiteAz.Common;
using Autofac.Extensions.DependencyInjection;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using TiteAz.ReadModel;
using NEvilES;

namespace TiteAz.Api
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
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {

            // Add framework services.
            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });
            services.AddSingleton<IHttpContextAccessor,HttpContextAccessor>();

            var builder = new ContainerBuilder();

            builder.Register(c =>
            {
                var a = c.Resolve<IHttpContextAccessor>();
                var h = a.HttpContext.Request.Headers.FirstOrDefault(x => x.Key == "X-User-Id");
                var uid = Guid.Parse(h.Value.ToString());

                return new CommandContext.User(uid);
            }).Named<CommandContext.IUser>("user");


            builder.RegisterInstance<IEventTypeLookupStrategy>(new EventTypeLookupStrategy());
            builder.RegisterModule(new EventStoreDatabaseModule(Configuration.GetConnectionString("pgsql"), DatabaseType.Postgres));
            builder.RegisterModule(new EventProcessorModule(typeof(Domain.User).GetTypeInfo().Assembly, typeof(ReadModel.User).GetTypeInfo().Assembly));

            services.AddScoped<IWriteReadModel, SqlReadModel>();
            services.AddScoped<IReadFromReadModel, SqlReadModel>();
            builder.Populate(services);

            var container = builder.Build();
            container.Resolve<IEventTypeLookupStrategy>().ScanAssemblyOfType(typeof(Domain.User));

            // Create the IServiceProvider based on the container.
            return new AutofacServiceProvider(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseCors(x =>
            {
                x.AllowAnyOrigin();
                x.AllowAnyHeader();
            });

            app.UseMvc();
        }
    }
}
