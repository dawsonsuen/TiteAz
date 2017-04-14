using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NEvilES.Pipeline;
using Newtonsoft.Json.Serialization;
using TiteAz.Common;
using TiteAz.SeedData;
using Autofac.Extensions.DependencyInjection;
using TiteAz.Domain;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using TiteAz.ReadModel;

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

            services.AddScoped<SqlConnection>(x =>
            {
                var sqlConn = new SqlConnection(Configuration.GetConnectionString("TiteAz"));
                sqlConn.Open();
                return sqlConn;
            });

            services.AddScoped<IWriteReadModel, WriteReadModel>();
            services.AddScoped<IReadFromReadModel, SqlReadModel>();

            var builder = new ContainerBuilder();

            builder.Register(c =>
            {
                var a = c.Resolve<IHttpContextAccessor>();
                var h = a.HttpContext.Request.Headers.FirstOrDefault(x=>x.Key == "UserId");
                var uid = Guid.Parse(h.Value.ToString());

                return new CommandContext.User(uid);
            }).Named<CommandContext.IUser>("user");

            builder.RegisterModule(new EventStoreDatabaseModule(Configuration.GetConnectionString("TiteAz1")));
            builder.RegisterModule(new EventProcessorModule(typeof(Domain.User).GetTypeInfo().Assembly, typeof(ReadModel.User).GetTypeInfo().Assembly));
            builder.Populate(services);


            // Create the IServiceProvider based on the container.
            return new AutofacServiceProvider(builder.Build());
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
